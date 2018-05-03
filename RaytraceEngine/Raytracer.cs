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
            surface.Clear(0);
            
            for(int x = 0; x < surface.width; x++)
            for(int y = 0; y < surface.height; y++)
            for(int i = 0; i < scene.primitives.Count; i++)
                {
                    Ray r = scene.cam.FromPixel(x, y);
                    RayHit h;
                    bool ok = scene.primitives[i].CheckHit(r, out h);
                    if (ok) surface.Plot(x, y, 255);
                }
        }
    }
}