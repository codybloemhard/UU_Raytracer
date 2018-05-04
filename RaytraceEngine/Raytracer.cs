using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Engine;
using Engine.TemplateCode;
using OpenTK;
using RaytraceEngine.Objects;

namespace RaytraceEngine
{
    public struct Area
    {
        public int X1, X2, Y1, Y2;
        
        public Area(int x1, int x2, int y1, int y2)
        {
            this.X1 = x1;
            this.X2 = x2;
            this.Y1 = y1;
            this.Y2 = y2;
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
        private int ri = 0;
        private int debug_freq = 16;

        public void Render(Surface surface, RayScene scene)
        {
            Rays.Clear();
            LightRays.Clear();
            ShadowRays.Clear();
            var projectionPlane = scene.CurrentCamera.GetNearClippingPlane();
            
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };
            Parallel.For(0, winHeight - 1, parallelOptions, i => {
                RenderArea(new Area(0, winWidth, i, i+1), projectionPlane, surface,
                    scene);
            });
        }

        public void RenderArea(Area area, FinitePlane projectionPlane, Surface surface, RayScene scene)
        {
            for (int x = area.X1; x < area.X2; ++x)
            for (int y = area.Y1; y < area.Y2; y++) {
                Ray ray = RayFromPixel(projectionPlane, scene.CurrentCamera, x, y);
                bool shouldDebug = y == winHeight >> 1;
                Vector3 colour = TraceColour(ray, scene, shouldDebug);
                surface.Plot(x, y, RMath.ToIntColour(colour));
            }
        }

        private Ray RayFromPixel(FinitePlane projectionPlane, Camera camera, int x, int y)
        {
            float wt = (float)x / winWidth;
            float ht = (float)y / winHeight;
            Vector3 onPlane = wt * projectionPlane.NHor + ht * projectionPlane.NVert + projectionPlane.Origin;
            onPlane.Normalize();

            return new Ray {
                Direction = onPlane,
                Origin = camera.Position
            };
        }

        private Vector3 TraceColour(Ray ray, RayScene scene, bool shouldDebug = false)
        {
            RayHit hit, lHit, tempHit;
            Primitive primitive = null;
            hit = new RayHit();
            hit.Distance = 10000000f;
            bool isHit = false;
            foreach (var p in scene.Primitives)
            {
                isHit = p.CheckHit(ray, out tempHit);
                if (!isHit) continue;
                if (tempHit.Distance < hit.Distance)
                {
                    hit = tempHit;
                    primitive = p;
                }
            }
            
            if(primitive == null) return Vector3.Zero;
            
            if (shouldDebug)
            {
                if (ri % debug_freq == 0)  Rays.Add(new Tuple<Ray, RayHit>(ray, hit));
                ++ri;
            }
            
            Vector3 lEnergy = Vector3.Zero;
            foreach (var light in scene.Lights)
            {
                ProbeLight(hit, light, scene, ref lEnergy, shouldDebug && ri % debug_freq == 0);
            }
            return hit.Material.Colour * lEnergy + hit.Material.Colour * scene.ambientLight;
        }

        private void ProbeLight(RayHit hit, ILightSource light, RayScene scene, ref Vector3 lightEnergy, bool debug = false)
        {
            var lPos = light.NearestPointTo(hit.Position);
            var sRayVec = lPos - hit.Position;
            float distsq = Vector3.Dot(sRayVec, sRayVec);
            sRayVec.Normalize();
            
            //Solve light.Intensity * power * (1f / (dist * dist * 4 * RMath.PI));
            //for dist(now range), and only check for shadow if dist < distance_to_light (dist here)
            float rangeSq = Vector3.Dot(light.Intensity, light.Intensity) * RMath.roll0_sq;
            if (distsq > rangeSq) return;
            
            // Check if something is in rays way
            var sRay = new Ray {
                Origin = hit.Position + sRayVec * 0.001f,
                Direction = sRayVec
            };
            foreach (var prim in scene.Primitives) {
                if (prim.CheckHit(sRay, out var tmp) && tmp.Distance * tmp.Distance < distsq) {
                    if (debug) ShadowRays.Add(new Tuple<Ray, RayHit>(sRay, tmp));
                    return;
                }
            }
            
            // Calculate the power of the light
            float power = Math.Max(0, Vector3.Dot(hit.Normal, sRayVec));
            lightEnergy += light.Intensity * power / (distsq * 4 * RMath.PI);
           
            if (debug) LightRays.Add(new Tuple<Ray, RayHit>(sRay, new RayHit(lPos, Vector3.One, 1, null)));
        }
    }
}