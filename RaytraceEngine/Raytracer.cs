using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Engine;
using Engine.TemplateCode;
using OpenTK;
using RaytraceEngine.Objects;
using RaytraceEngine.Objects.Lights;

namespace RaytraceEngine
{
    public struct Area
    {
        public int X1, X2, Y1, Y2;
        
        public Area(int x1, int x2, int y1, int y2)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
        }
    }
    
    public class Raytracer
    {
        private int winWidth;
        private int winHeight;

        public Raytracer(int winWidth, int winHeight)
        {
            this.winWidth = winWidth;
            this.winHeight = winHeight;
        }

        public static List<Tuple<Ray, RayHit>> Rays = new List<Tuple<Ray, RayHit>>();
        public static List<Tuple<Ray, RayHit>> LightRays = new List<Tuple<Ray, RayHit>>();
        public static List<Tuple<Ray, RayHit>> ShadowRays = new List<Tuple<Ray, RayHit>>();
        public static List<Tuple<Ray, RayHit>> RefractRays = new List<Tuple<Ray, RayHit>>();
        private int ri = 0;
        private int debug_freq = 32;
        
        public void Render(Surface surface, RayScene scene)
        {
            Rays.Clear();
            LightRays.Clear();
            ShadowRays.Clear();
            RefractRays.Clear();
            var projectionPlane = scene.CurrentCamera.GetNearClippingPlane();

            var parallelOptions = new ParallelOptions{MaxDegreeOfParallelism = Environment.ProcessorCount};
            Parallel.For(0, winHeight - 1, parallelOptions, i => {
                RenderArea(new Area(0, winWidth, i, i+1), projectionPlane, surface, scene);
            });
        }

        public void RenderArea(Area area, FinitePlane projectionPlane, Surface surface, RayScene scene)
        {
            int a = (int)TraceSettings.AntiAliasing;
            for (int x = area.X1; x < area.X2; ++x)
            for (int y = area.Y1; y < area.Y2; y++)
            {
                Vector3 colour = Vector3.Zero;
                for (int ix = 0; ix < a; ix++)
                    for (int iy = 0; iy < a; iy++)
                    {
                        Ray ray = RayFromPixel(projectionPlane, scene.CurrentCamera, x * a + ix, y * a + iy, winWidth * a, winHeight * a);
                        bool shouldDebug = y == winHeight >> 1;
                        colour += TraceColour(ray, scene, (int)TraceSettings.RecursionDepth, shouldDebug);
                        if(shouldDebug) ri++;
                    }
                colour /= a * a;
                surface.Plot(x, y, RMath.ToIntColour(colour));
            }
        }

        private Ray RayFromPixel(FinitePlane projectionPlane, Camera camera, int x, int y, int w, int h)
        {
            float wt = (float)x / w;
            float ht = (float)y / h;
            Vector3 onPlane = wt * projectionPlane.NHor + ht * projectionPlane.NVert + projectionPlane.Origin;
            onPlane.Normalize();

            return new Ray {
                Direction = onPlane,
                Origin = camera.Position
            };
        }

        private Vector3 TraceColour(Ray ray, RayScene scene, int depth, bool debug = false)
        {
            if (depth-- == 0) return Vector3.Zero;
            
            var hit = new RayHit {Distance = 10000000f};
            RayHit tmpHit;
            foreach (var primitive in scene.Primitives) {
                if (primitive.CheckHit(ray, out tmpHit) && tmpHit.Distance < hit.Distance) hit = tmpHit;
            }

            if (hit.HitObject == null) return Vector3.Zero;
            
            if (debug && ri % debug_freq == 0) Rays.Add(new Tuple<Ray, RayHit>(ray, hit));

            Vector3 baseCol = hit.HitObject.Material.Colour;
            if (hit.HitObject.Material.Texture != null)
                baseCol = hit.HitObject.Material.TexColour(hit.HitObject.GetUV(hit));
            Vector3 colorComp = baseCol * CalcLightEnergy(scene, hit, debug) +
                                baseCol * TraceSettings.AmbientLight;

            if (hit.HitObject.Material.Reflectivity > 0.01f) {
                ApplyReflectivity(scene, depth, ref ray, ref hit, ref colorComp, debug);
            }
            if (hit.HitObject.Material.Refractivity > 0.01f) {
                ApplyRefractivity(scene, depth, ref ray, ref hit, ref colorComp, debug);
            }

            return colorComp;
        }
        
        private void ApplyReflectivity(RayScene scene, int depth, ref Ray ray, ref RayHit hit, ref Vector3 lightComp, bool debug)
        {
            Ray rRay = new Ray();
            rRay.Direction = RMath.Reflect(ray.Direction, hit.Normal);
            rRay.Origin = hit.Position + rRay.Direction * 0.001f;
            var reflectColor = TraceColour(rRay, scene, depth, debug);

            lightComp = lightComp * (1f - hit.HitObject.Material.Reflectivity) +
                        hit.HitObject.Material.Reflectivity * reflectColor * hit.HitObject.Material.Colour;
        }

        private void ApplyRefractivity(RayScene scene, int depth, ref Ray ray, ref RayHit hit, ref Vector3 lightComp, bool debug)
        {
            if (!(hit.HitObject is IVolumetricTraceable)) return; // TODO: go straight through since object has no volume
            var objectOfInterest = hit.HitObject as IVolumetricTraceable;
            var material = hit.HitObject.Material;
            
            Ray rRay = new Ray();
            rRay.Direction = RMath.Refract(ray.Direction, hit.Normal, material.RefractETA);
            rRay.Origin = hit.Position + rRay.Direction * 0.001f;
            
            // Ray is instantly reflected before entering the object
            if (Vector3.Dot(rRay.Direction, rRay.Direction) < 0.001) {
                rRay.Direction = RMath.Reflect(ray.Direction, hit.Normal);
                rRay.Origin = hit.Position + rRay.Direction * 0.001f;
                var refractColor = TraceColour(rRay, scene, depth, debug);
                lightComp = lightComp * (1f - hit.HitObject.Material.Refractivity) +
                            hit.HitObject.Material.Refractivity * refractColor * hit.HitObject.Material.Colour;
                return;
            }
            
            RayHit innerRayHit;
            for (int i = depth; i >= 0; --i) {
                if (!objectOfInterest.CheckHitInside(rRay, out innerRayHit)) break;
                if (debug && ri % debug_freq == 0) RefractRays.Add(new Tuple<Ray, RayHit>(rRay, innerRayHit));

                rRay.Direction = RMath.Refract(rRay.Direction, innerRayHit.Normal, 1f/material.RefractETA);
                rRay.Origin = innerRayHit.Position + rRay.Direction * 0.001f;

                // Ray refracts out of the object
                if (Vector3.Dot(rRay.Direction, rRay.Direction) > 0.001) {
                    var refractColor = TraceColour(rRay, scene, depth, debug);
                    lightComp = lightComp * (1f - hit.HitObject.Material.Refractivity) +
                                hit.HitObject.Material.Refractivity * refractColor * hit.HitObject.Material.Colour;
                    return;
                }
                
                // Ray gets reflected back inside the object
                rRay.Direction = RMath.Reflect(rRay.Direction, innerRayHit.Normal);
                rRay.Origin = innerRayHit.Position + rRay.Direction * 0.001f;
            }
            
            // This part should not be reachable
        }
        
        private Vector3 CalcLightEnergy(RayScene scene, RayHit hit, bool debug)
        {
            Vector3 lEnergy = Vector3.Zero;
            bool first = true;
            foreach (var light in scene.Lights)
            {
                var lPoints = light.GetPoints(TraceSettings.MaxLightSamples, TraceSettings.RealLightSample);
                var localEnergy = Vector3.Zero;
                foreach (var lp in lPoints) localEnergy += ProbeLight(hit, lp, light, scene, debug && first && ri % debug_freq == 0);
                localEnergy /= lPoints.Length;
                lEnergy += localEnergy;
                first = false;
            }
            return lEnergy;
        }

        private Vector3 ProbeLight(RayHit hit, Vector3 lPos, ILightSource light, RayScene scene, bool debug = false)
        {
            var sRayVec = lPos - hit.Position;
            float distsq = Vector3.Dot(sRayVec, sRayVec);
            sRayVec.Normalize();
            
            //Solve light.Intensity * power * (1f / (dist * dist * 4 * RMath.PI));
            //for dist(now range), and only check for shadow if dist < distance_to_light (dist here)
            float rangeSq = light.Intensity * RMath.roll0_sq;
            if (distsq > rangeSq) return Vector3.Zero;
            
            // Check if something is in rays way
            var sRay = new Ray {
                Origin = hit.Position + sRayVec * 0.001f,
                Direction = sRayVec
            };
            foreach (var prim in scene.Primitives) {
                RayHit tmp;
                if (prim.CheckHit(sRay, out tmp) && tmp.Distance * tmp.Distance < distsq) {
                    if (debug) ShadowRays.Add(new Tuple<Ray, RayHit>(sRay, tmp));
                    return Vector3.Zero;
                }
            }
            if (debug && ri % debug_freq == 0) LightRays.Add(new Tuple<Ray, RayHit>(sRay, new RayHit(lPos, Vector3.One, 1, null)));
            
            // Calculate the power of the light
            float power = Math.Max(0, Vector3.Dot(hit.Normal, sRayVec));
            power *= light.AngleEnergy(-sRayVec);
            return light.Colour * Math.Min(light.Intensity * power / (distsq * 4 * RMath.PI), light.MaxEnergy);
        }
    }
}