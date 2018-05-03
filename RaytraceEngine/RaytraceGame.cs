using Engine;

namespace RaytraceEngine
{
    public abstract class RaytraceGame : Game2D<RayScene>
    {
        protected Raytracer Renderer;
        
        protected RaytraceGame()
        {
            Renderer = new Raytracer();
        }

        public override void Render2D()
        {
            Renderer.Render(Screen, Scene);
        }
    }
}