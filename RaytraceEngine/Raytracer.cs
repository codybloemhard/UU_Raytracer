using Engine;
using Engine.TemplateCode;
using RaytraceEngine.Objects;
using OpenTK;

namespace RaytraceEngine
{
    public class Raytracer
    {
        public void Render(Surface surface, RayScene scene)
        {
            Sphere s = new Sphere();
            s.pos = new Vector3(1, 1, 5);
            s.r = 1f;
            for(int x = 0; x < surface.width; x++)
            for(int y = 0; y < surface.height; y++)
                {
                    Ray r = scene.cam.FromPixel(x, y);
                    RayHit h;
                    bool ok = s.CheckHit(r, out h);
                    if (ok) surface.Plot(x, y, 255);
                }
        }
    }
}