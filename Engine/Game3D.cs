using OpenTK;

namespace Engine
{
    public abstract class Game3D : Game<RenderableScene>
    {
        /// <summary>
        /// Called every tick
        /// </summary>
        public override void Update()
        {
            Scene.Update();
        }

        public override void Render()
        {
            var worldM = Matrix4.Identity;
            worldM *= Matrix4.CreateFromQuaternion(new Quaternion(0, 0, 0));
            worldM *= Matrix4.CreateTranslation(0, 0, 0);
            worldM *= Matrix4.CreateScale(1, 1, 1);
            Scene.Render(Matrix4.Identity, worldM);
        }
    }
}