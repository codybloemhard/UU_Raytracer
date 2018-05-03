using Engine;

namespace RaytraceEngine
{
    public abstract class RaytraceGame : Game2D
    {
        protected Raytracer renderer;
        protected RayScene scene;
        
        protected RaytraceGame()
        {
            renderer = new Raytracer();
            scene = new RayScene();
        }

        public override void Render2D()
        {
            renderer.Render(Screen, scene);
        }
    }
}