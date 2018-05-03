using Engine;
using Engine.TemplateCode;

namespace RaytraceEngine
{
    public class Raytracer : IRenderer
    {

        public void Render(Surface surface, Scene scene)
        {
            surface.Box(50, 50, 200, 200, 255);
        }
    }
}