using Engine;

namespace RaytraceEngine
{
    public abstract class RaytraceGame : Game2D<RayScene>
    {
        protected Raytracer Renderer;
        
        protected RaytraceGame()
        {
           
        }

        public override void Render2D()
        {
            Renderer.Render(Screen, Scene);
        }
    }
}