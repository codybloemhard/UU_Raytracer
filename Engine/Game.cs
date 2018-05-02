using Template;

namespace Engine
{
    public abstract class Game
    {
        protected Scene CurrentScene { get; set; }

        public abstract void Init();
        
        public abstract void Update();
        
        public abstract void Render();
    }
}