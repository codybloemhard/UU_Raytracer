using System;
using System.Collections.Generic;
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
        private int ri = 0;

        public void Render(Surface surface, RayScene scene)
        {
            Rays.Clear();
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
            bool addRays = false, isHit = false;;
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
                if (ri % 8 == 0)
                {
                    Rays.Add(new Tuple<Ray, RayHit>(ray, hit));
                    addRays = true;
                }
                ++ri;
            }
            Vector3 lEnergy = Vector3.Zero;
            foreach (var light in scene.Lights)
            {
                Vector3 lightPos = light.GetPos();
                Vector3 toLight = lightPos - hit.Position;
                float dist = toLight.Length;
                toLight.Normalize();
                Ray lRay = new Ray();
                lRay.Origin = hit.Position + toLight * 0.001f;
                lRay.Direction = toLight;
                //Solve light.Intensity * power * (1f / (dist * dist * 4 * RMath.PI));
                //for dist(now range), and only check for shadow if dist < distance_to_light (dist here)
                float range = light.Intensity.Length * RMath.roll0;
                bool inRange = dist < range;
                bool blocked = false;
                if(inRange)
                    foreach (var prim in scene.Primitives)
                    {
                        if (prim == primitive) continue;
                        blocked = prim.CheckHit(lRay, out lHit);
                        if (blocked) break;
                    }
                if (blocked) continue;
                float power = RMath.Dot(hit.Normal, toLight);
                if (power < 0) power = 0;
                lEnergy += light.Intensity * power * (1f / (dist * dist * 4 * RMath.PI));
            }
            return hit.Material.Colour * lEnergy + hit.Material.Colour * scene.ambientLight;
        }
    }
}