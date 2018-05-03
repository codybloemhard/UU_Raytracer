using System.Collections.Generic;
using Engine;
using Engine.Objects;
using OpenTK;

namespace Engine
{
    /// <summary>
    /// Scene object contains a hierarchy of objects and a camera
    /// </summary>
    public class Scene : IRenderable
    {
        public List<ITransformative> Objects { get; protected set; }
        public Camera CurrentCamera { get; set; }

        public Scene(Camera camera) : this(camera, new List<ITransformative>()) {}

        public Scene(Camera camera, List<ITransformative> objects)
        {
            CurrentCamera = camera;
            Objects = objects;
        }

        /// <summary>
        /// Called every tick
        /// </summary>
        public void Update()
        {
           
        }

        public void AddObject(ITransformative obj)
        {
            Objects.Add(obj);
            if(obj is Object) ((Object)obj).Init();
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