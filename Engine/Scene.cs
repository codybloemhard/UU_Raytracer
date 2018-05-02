using System.Collections.Generic;
using Engine;
using Engine.Objects;
using OpenTK;

namespace Template
{
    /// <summary>
    /// Scene object contains a hierarchy of objects and a camera
    /// </summary>
    public class Scene : IRenderable
    {
        public List<Object> Objects { get; protected set; }
        public Camera CurrentCamera { get; set; }

        public Scene(Camera camera) : this(camera, new List<Object>()) {}

        public Scene(Camera camera, List<Object> objects)
        {
            CurrentCamera = camera;
            this.Objects = objects;
        }

        /// <summary>
        /// Called every tick
        /// </summary>
        public void Update()
        {
            foreach (var obj in Objects) {
                obj.Update();
            }
        }

        public void AddObject(Object obj)
        {
            Objects.Add(obj);
            obj.Init();
        }

        public void Render(Matrix4 viewM, Matrix4 worldM)
        {
            CurrentCamera.Clear();
            viewM = CurrentCamera.TransformMatrix(Matrix4.Identity);
            var candidates = CurrentCamera.Cull(Objects);
            foreach (var candidate in candidates) {
                candidate.Render(viewM, worldM);
            }
        }
    }
}