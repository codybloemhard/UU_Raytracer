using Engine.Objects;
using OpenTK;

namespace Engine
{
    public abstract class Game
    {
        protected Scene CurrentScene { get; set; }

        public abstract void Init();

        /// <summary>
        /// Called every tick
        /// </summary>
        public virtual void Update()
        {
            foreach (var obj in CurrentScene.Objects) {
                if(obj is Object) (obj as Object).Update();
            }
        }

        public virtual void Render()
        {
            var worldM = Matrix4.Identity;
            worldM *= Matrix4.CreateFromQuaternion(new Quaternion(0, 0, 0));
            worldM *= Matrix4.CreateTranslation(0, 0, 0);
            worldM *= Matrix4.CreateScale(1, 1, 1);
            CurrentScene.Render(Matrix4.Identity, worldM);
        }
    }
}