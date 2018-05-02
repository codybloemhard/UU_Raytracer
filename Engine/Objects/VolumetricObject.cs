using System.Collections.Generic;
using Engine.Graphics;
using OpenTK;

namespace Engine.Objects
{
    /// <summary>
    /// A basic 3d object
    /// </summary>
    public class VolumetricObject : Object, IRenderable
    {
        public Mesh Mesh { get; set; } = null;
        public Shader Shader { get; set; } = null;
        public List<Object> Children { get; set; }

        public VolumetricObject(Mesh mesh, Shader shader) : this(mesh, shader, Vector3.Zero, Quaternion.Identity,
            Vector3.One) { }

        public VolumetricObject(Mesh mesh, Shader shader, Vector3 position, Quaternion rotation, Vector3 scale)
            : base(position, rotation, scale)
        {
            Mesh = mesh;
            Shader = shader;
            Children = new List<Object>();
        }
        
        /// <summary>
        /// Used to prepare all the shaders before drawing the object.
        /// Here shader should be enabled and all the uniforms should be set
        /// </summary>
        /// <param name="modelM"></param>
        protected virtual void PrepareShaders(Matrix4 viewM, Matrix4 worldM, Matrix4 modelM)
        {
            if (Shader == null) return;

            Shader.SetVar("uViewM", ref viewM);
            Shader.SetVar("uWorldM", ref worldM);
            Shader.SetVar("uModelM", ref modelM);
        }

        public void Render(Matrix4 viewM, Matrix4 worldM)
        {
            var modelM = TransformMatrix(Matrix4.Identity);
            if (Mesh != null && Shader != null) {
                Shader.Use();
                PrepareShaders(viewM, worldM, modelM);
                Mesh.Render(Shader);
            }

            foreach (var child in Children) {
                if(child is IRenderable renderable) renderable.Render(viewM, worldM);
            }
        }
    }
}