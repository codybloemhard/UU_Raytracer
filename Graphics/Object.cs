using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using OpenTK;

namespace Template
{
    public interface ITransformative
    {
        /// <summary>
        /// Transform a matrix to space relative to object
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        Matrix4 TransformMatrix(Matrix4 matrix);
    }

    public interface IRenderable
    {
        /// <summary>
        /// Used to render the object on canvas. NOTHING ELSE
        /// </summary>
        /// <param name="origin"></param>
        void Render(Matrix4 origin);
    }

    /// <summary>
    /// Basic scene object
    /// </summary>
    public abstract class Object : ITransformative, IRenderable
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        protected Object() : this(Vector3.Zero, Quaternion.Identity, Vector3.One) { }

        protected Object(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Matrix4 TransformMatrix(Matrix4 matrix)
        {
            matrix *= Matrix4.CreateFromQuaternion(Rotation);
            matrix *= Matrix4.CreateTranslation(Position);
            matrix *= Matrix4.CreateScale(Scale);
            return matrix;
        }

        public abstract void Render(Matrix4 origin);
    }

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