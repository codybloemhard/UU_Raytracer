using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template
{
    /// <summary>
    /// Scene object contains a hierarchy of objects and a camera
    /// </summary>
    public class Scene : IRenderable
    {
        public List<Object> Objects { get; set; }
        public Camera CurrentCamera { get; set; }

        public Scene(Camera camera) : this(camera, new List<Object>()) {}

        public Scene(Camera camera, List<Object> objects)
        {
            CurrentCamera = camera;
            this.Objects = objects;
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