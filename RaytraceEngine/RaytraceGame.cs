using Engine;

namespace RaytraceEngine
{
    public abstract class RaytraceGame : Game2D
    {
        protected IRenderer Renderer;
        
        protected RaytraceGame()
        {
            Renderer = new Raytracer();
        }

        public override void Render2D()
        {
            Renderer.Render(Screen, CurrentScene);
        }
    }
}