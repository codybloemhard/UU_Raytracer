using Engine;

namespace RaytraceEngine
{
    public abstract class RaytraceGame : Game
    {
        protected IRenderer Renderer;
        
        protected RaytraceGame()
        {
            Renderer = new Raytracer();
        }

        public override void Render()
        {
            Renderer.Render(CurrentScene);
        }
    }
}