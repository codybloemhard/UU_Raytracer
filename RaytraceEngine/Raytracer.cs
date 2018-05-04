using System;
using System.Collections.Generic;
using Engine;
using Engine.TemplateCode;
using OpenTK;

namespace RaytraceEngine
{
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

            ri = 0;
            for (int x = 0; x < winWidth; ++x)
            for (int y = 0; y < winHeight; y++) {
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
            RayHit hit, lHit;
            bool addRays = false;
            foreach (var primitive in scene.Primitives)
            {
                bool isHit = primitive.CheckHit(ray, out hit);
                if (!isHit) continue;
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
                    Vector3 toLight = hit.Position - lightPos;
                    toLight.Normalize();
                    Ray lRay = new Ray();
                    lRay.Origin = hit.Position + toLight * 0.001f;
                    lRay.Direction = toLight;
                    foreach (var prim in scene.Primitives)
                    {
                        isHit = primitive.CheckHit(lRay, out lHit);
                        if (isHit) continue;
                        //Werkt niet ??? if (addRays) Rays.Add(new Tuple<Ray, RayHit>(lRay, lHit));
                        float power = RMath.Dot(hit.Normal, toLight);
                        float dist = (hit.Position - lightPos).Length;
                        if (power < 0) power = 0;
                        lEnergy += light.Intensity * power * (1f / (dist * dist * 4 * RMath.PI));
                    }
                }
                return hit.Material.Colour * lEnergy + hit.Material.Colour * scene.ambientLight;
            }
            return Vector3.Zero;
        }
    }
}