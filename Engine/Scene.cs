using System.Collections.Generic;
using Engine;
using Engine.Objects;
using OpenTK;

namespace Engine
{
    /// <summary>
    /// Scene object contains a hierarchy of objects and a camera
    /// </summary>
    public abstract class Scene
    {
        public Camera CurrentCamera { get; set; }

        public Scene(Camera camera)
        {
            CurrentCamera = camera;
        }

        /// <summary>
        /// Called every tick
        /// </summary>
        public virtual void Update() { }

        public virtual void AddObject(ITransformative obj)
        {
            if(obj is Object) (obj as Object).Init();
        }
    }

    public class RenderableScene : Scene, IRenderable
    {
        public List<ITransformative> Objects;

        public RenderableScene(Camera camera) : base(camera)
        {
            Objects = new List<ITransformative>();   
        }

        public override void Update()
        {
            base.Update();
            foreach (var o in Objects) {
                if(o is Object) (o as Object).Update();
            }
        }

        public override void AddObject(ITransformative obj)
        {
            Objects.Add(obj);
            base.AddObject(obj);
        }

        public void Render(Matrix4 view, Matrix4 world)
        {
            CurrentCamera.Clear();
            view = CurrentCamera.TransformMatrix(Matrix4.Identity);
            var candidates = CurrentCamera.Cull(Objects);
            foreach (var candidate in candidates) {
                if(candidate is IRenderable) (candidate as IRenderable).Render(view, world);
            }
        }
    }
}