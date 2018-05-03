using Engine.Objects;
using OpenTK;

namespace Engine
{
    public abstract class Game
    {
        protected int Width { get; set; }
        protected int Height { get; set; }

        public abstract void Init();
        public abstract void Update();
        public abstract void Render();

        public virtual void Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public virtual void Destroy() { }
    }

    public abstract class Game3D : Game
    {
        protected Scene CurrentScene { get; set; }

        /// <summary>
        /// Called every tick
        /// </summary>
        public override void Update()
        {
            foreach (var obj in CurrentScene.Objects) {
                if(obj is Object) (obj as Object).Update();
            }
        }

        public override void Render()
        {
            var worldM = Matrix4.Identity;
            worldM *= Matrix4.CreateFromQuaternion(new Quaternion(0, 0, 0));
            worldM *= Matrix4.CreateTranslation(0, 0, 0);
            worldM *= Matrix4.CreateScale(1, 1, 1);
            CurrentScene.Render(Matrix4.Identity, worldM);
        }
    }
}