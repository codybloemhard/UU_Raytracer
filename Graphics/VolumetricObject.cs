using System.Collections.Generic;
using OpenTK;

namespace Template
{
    /// <summary>
    /// A basic 3d object
    /// </summary>
    public class VolumetricObject : Object
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
        /// <param name="baseMat"></param>
        protected virtual void PrepareShaders(Matrix4 baseMat)
        {
            if (Shader == null) return;

            Shader.Use();
            Shader.SetVar("uMat", ref baseMat);
        }

        public override void Render(Matrix4 origin)
        {
            var m = TransformMatrix(origin);
            if (Mesh != null && Shader != null) {
                PrepareShaders(m);
                Shader.Use();
                Mesh.Render(Shader);
            }

            foreach (var child in Children) {
                child.Render(m);
            }
        }
    }
}