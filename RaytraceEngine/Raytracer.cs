using Engine;
using Engine.TemplateCode;

namespace RaytraceEngine
{
    public class Raytracer
    {
        public void Render(Surface surface, RayScene scene)
        {
            surface.Box(50, 50, 100, 100, 255);
        }
    }
}