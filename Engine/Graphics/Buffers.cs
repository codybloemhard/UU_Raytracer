using System;
using Engine.Helpers;
using OpenTK.Graphics.OpenGL;


namespace Engine.Graphics
{
    public interface IUploadableBuffer
    {
        void Upload(Shader s);
    }
    
    public interface IRenderableBuffer
    {
        void Render(Shader s);
    }

    public abstract class Buffer<T> : IUploadableBuffer where T : struct
    {
        public int Pointer;
        public T[] Data;

        public Buffer(T[] data)
        {
            Data = data;
            Pointer = GL.GenBuffer();
        }

        public abstract void Upload(Shader s);
    }

    public class IndexBuffer : Buffer<uint>
    {
        public IndexBuffer(uint[] data) : base(data) { }

        public override void Upload(Shader s)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Pointer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr) (Data.Length * sizeof(uint)), Data,
                BufferUsageHint.StaticDraw);
        }
    }

    public class AttribBuffer<T> : Buffer<T>, IUploadableBuffer, IRenderableBuffer where T : struct
    {
        public string AttributeName { private set; get; }

        public VertexAttribPointerType AttribPointerType { private set; get; }

        public AttribBuffer(T[] data, string attributeName, VertexAttribPointerType attribPointerType) : base(data)
        {
            AttributeName = attributeName;
            AttribPointerType = attribPointerType;
        }

        public override void Upload(Shader s)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Pointer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (Data.Length * Generics.SizeOf(typeof(T))), Data,
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(s.GetVar(AttributeName), 3, AttribPointerType, false, 0, 0);
        }

        public void Render(Shader s)
        {
            GL.EnableVertexAttribArray(s.GetVar(AttributeName));
        }
    }
}