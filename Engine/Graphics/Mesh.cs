using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Engine.Graphics
{
    public class Mesh
    {
        public List<IUploadableBuffer> Buffers;
        public IndexBuffer IndexBuffer;
        public int VBO;

        public Mesh() : this(new List<IUploadableBuffer>(), null) { }

        public Mesh(List<IUploadableBuffer> buffers, IndexBuffer indexBuffer)
        {
            Buffers = buffers;
            IndexBuffer = indexBuffer;
            
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        }

        public virtual void Upload(Shader s)
        {
            IndexBuffer.Upload(s);
            foreach (var buffer in Buffers) buffer.Upload(s);
        }

        public virtual void Render(Shader s)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer.Pointer);
            foreach (var buffer in Buffers) {
                if(buffer is IRenderableBuffer renderableBuffer) renderableBuffer.Render(s);
            }
            GL.DrawElements(PrimitiveType.Triangles, IndexBuffer.Data.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}