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
        private int debug_freq = 64;

        public void Render(Surface surface, RayScene scene)
        {
            Rays.Clear();
            LightRays.Clear();
            ShadowRays.Clear();
            RefractRays.Clear();
            var projectionPlane = scene.CurrentCamera.GetNearClippingPlane();
            
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            if(TraceSettings.Multithreading)
                Parallel.For(0, winHeight - 1, parallelOptions,
                i => { RenderArea(new Area(0, winWidth, i, i + 1), projectionPlane, surface, scene); });
            else
                for (int i = 0; i < winHeight - 1; i++)
                    RenderArea(new Area(0, winWidth, i, i + 1), projectionPlane, surface, scene);
        }
        
        public void RenderArea(Area area, FinitePlane projectionPlane, Surface surface, RayScene scene)
        {
            int a = (int) TraceSettings.AntiAliasing;
            for (int x = area.X1; x < area.X2; ++x)
            for (int y = area.Y1; y < area.Y2; y++) {
                Vector3 colour = Vector3.Zero;
                for (int ix = 0; ix < a; ix++)
                for (int iy = 0; iy < a; iy++) {
                    Ray ray = RayFromPixel(projectionPlane, scene.CurrentCamera, x * a + ix, y * a + iy, winWidth * a,
                        winHeight * a);
                    bool shouldDebug = y == winHeight >> 1;
                    colour += TraceColour(ray, scene, (int) TraceSettings.RecursionDepth, shouldDebug);
                    if (shouldDebug) ri++;
                }

                colour /= a * a;
                surface.Plot(x, y, RMath.ToIntColour(colour));
            }
        }

        private Ray RayFromPixel(FinitePlane projectionPlane, Camera camera, int x, int y, int w, int h)
        {
            float wt = (float) x / w;
            float ht = (float) y / h;
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

            if (hit.HitObject == null) return scene.Sky.SkyColour(ray.Direction);
            
            if (debug && ri % debug_freq == 0) Rays.Add(new Tuple<Ray, RayHit>(ray, hit));

            Vector3 baseCol = hit.HitObject.Material.Colour;
            if (hit.HitObject.Material.Texture != null)
                baseCol = hit.HitObject.Material.TexColour(hit.HitObject.GetUV(hit));
            Vector3 colorComp = baseCol * CalcLightEnergy(scene, hit, debug) +
                                baseCol * TraceSettings.AmbientLight;

            if (hit.HitObject.Material.Reflectivity > 0.01f) {
                ApplyReflectivity(scene, depth, ref ray, ref hit, hit.HitObject.Material.Reflectivity, ref colorComp, debug);
            }

            if (hit.HitObject.Material.Refractivity > 0.01f) {
                ApplyRefractivity(scene, depth, ref ray, ref hit, ref colorComp, debug);
            }

            return colorComp;
        }

        private void ApplyReflectivity(RayScene scene, int depth, ref Ray ray, ref RayHit hit, float reflectivity, ref Vector3 lightComp,
            bool debug = false)
        {
            Ray rRay = new Ray();
            Vector3 reflDir = RMath.Reflect(ray.Direction, hit.Normal);
            rRay.Origin = hit.Position + hit.Normal * 0.001f;
            var reflectColor = Vector3.Zero;

            if(hit.HitObject.Material.Roughness > 0.0001f)
            {
                for (int i = 0; i < TraceSettings.MaxReflectionSamples; i++)
                {
                    rRay.Direction = RMath.RandomChange(reflDir, hit.HitObject.Material.Roughness);
                    reflectColor += TraceColour(rRay, scene, depth, debug);
                }
                reflectColor /= TraceSettings.MaxReflectionSamples;
            }
            else
            {
                rRay.Direction = reflDir;
                reflectColor = TraceColour(rRay, scene, depth, debug);
            }

            lightComp = lightComp * (1f - reflectivity) + reflectivity * reflectColor * hit.HitObject.Material.Colour;
        }

        private void ApplyRefractivity(RayScene scene, int depth, ref Ray ray, ref RayHit hit, ref Vector3 lightComp,
            bool debug = false)
        {
            var refractRays = RefractifityLoop(depth, ref ray, ref hit, debug);
            if (refractRays == null || refractRays.Count == 1) return;
            if (refractRays.Count == 0) {
                // Handle reflection TODO: use refract weight?
                ApplyReflectivity(scene, depth, ref ray, ref hit, RMath.Clamp(0, 1, hit.HitObject.Material.Refractivity), ref lightComp, debug );
                return;
            }

            float compoundLength = 0;
            for (int i = 0; i < refractRays.Count - 1; i++) {
                compoundLength += (refractRays[i + 1].Origin - refractRays[i].Origin).Length; // Square ROOO00oo..
            }

            float materialOpacity = RMath.Clamp(0, 1, compoundLength /
                                                      (TraceSettings.RefractLightDecayConstant *
                                                       hit.HitObject.Material.Refractivity));
            if (materialOpacity > 0.98) return; // Not worth shooting another ray

            var refractColor = TraceColour(refractRays[refractRays.Count - 1], scene, depth, debug);
            lightComp = lightComp * materialOpacity + refractColor * (1f - materialOpacity);
        }

        /// <summary>
        /// Handles refraction with the hit object. All rays fired inside the object are returned including the outgoing
        /// ray(last element). 
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="depth"></param>
        /// <param name="ray"></param>
        /// <param name="hit"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private List<Ray> RefractifityLoop(int depth, ref Ray ray, ref RayHit hit, bool debug = false)
        {
            if (!(hit.HitObject is IVolumetricTraceable)) return null;

            var ret = new List<Ray>();
            var objectOfInterest = (IVolumetricTraceable) hit.HitObject;
            var material = hit.HitObject.Material;

            var rRay = new Ray();
            rRay.Direction = RMath.Refract(ray.Direction, hit.Normal, material.RefractETA);
            rRay.Origin = hit.Position + rRay.Direction * 0.001f;

            // Ray is instantly reflected before entering the object
            if (Vector3.Dot(rRay.Direction, rRay.Direction) < 0.001) return ret;

            // Ray inside object loop
            ret.Add(ray);
            RayHit innerRayHit;
            for (int i = depth; i >= 0; --i) {
                ret.Add(rRay);
                if (!objectOfInterest.CheckHitInside(rRay, out innerRayHit)) break;
                
                if (debug && ri % debug_freq == 0) {
                    RefractRays.Add(new Tuple<Ray, RayHit>(rRay, innerRayHit));
                }

                rRay = new Ray();
                rRay.Direction = RMath.Refract(rRay.Direction, innerRayHit.Normal, 1f / material.RefractETA);
                rRay.Origin = innerRayHit.Position + rRay.Direction * 0.001f;

                // Ray refracts out of the object
                if (Vector3.Dot(rRay.Direction, rRay.Direction) > 0.001) {
                    ret.Add(rRay);
                    return ret;
                }

                // Ray gets reflected back inside the object
                rRay.Direction = RMath.Reflect(rRay.Direction, innerRayHit.Normal);
                rRay.Origin = innerRayHit.Position + rRay.Direction * 0.001f;
            }

            return null;
        }

        private Vector3 CalcLightEnergy(RayScene scene, RayHit hit, bool debug)
        {
            Vector3 lEnergy = TraceSettings.AmbientLight;
            foreach (var light in scene.Lights) {
                var lPoints = light.GetPoints(TraceSettings.MaxLightSamples, TraceSettings.LSM);
                var localEnergy = Vector3.Zero;
                
                bool first = true;
                foreach (var lp in lPoints) {
                    localEnergy += ProbeLight(hit, lp, light, scene, debug && first);
                    first = false;
                }

                localEnergy /= lPoints.Length;
                lEnergy += localEnergy;
                
            }

            return lEnergy;
        }

        private Vector3 ProbeLight(RayHit hit, Vector3 lPos, ILightSource light, RayScene scene, bool debug = false)
        {
            var sRayVec = lPos - hit.Position;
            float distsq = Vector3.Dot(sRayVec, sRayVec);
            sRayVec.Normalize();
            //see if were not turned away from the light
            float power = Math.Max(0, Vector3.Dot(hit.Normal, sRayVec));
            if (power < 0.0001f) return Vector3.Zero;
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
                    if (debug && ri % debug_freq == 0) ShadowRays.Add(new Tuple<Ray, RayHit>(sRay, tmp));
                    return Vector3.Zero;
                }
            }

            if (debug && ri % debug_freq == 0) LightRays.Add(new Tuple<Ray, RayHit>(sRay, new RayHit(lPos, Vector3.One, 1, null)));

            // Calculate the power of the light
            power *= light.AngleEnergy(-sRayVec);
            return light.Colour * Math.Min(light.Intensity * power / distsq, light.MaxEnergy);
        }
    }
}