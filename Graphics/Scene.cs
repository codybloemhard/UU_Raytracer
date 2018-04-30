using System.Collections.Generic;
using System.Security.Permissions;
using OpenTK;
using OpenTK.Graphics.OpenGL;

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
        /// Called very tick
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

        public void Render(Matrix4 origin)
        {
            CurrentCamera.Clear();
            var m = CurrentCamera.TransformMatrix(origin);
            var candidates = CurrentCamera.Cull(Objects);
            foreach (var candidate in candidates) {
                candidate.Render(m);
            }
        }
    }
}