using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{
    public enum BufferType : int
    {
        VERTEX = 0, NORMAL = 1
    }
    
    public class Mesh
    {
        private float[][] data;
        private int vbo;
        private int[] buffers;
        private string[] bVars;
        private int size;
        private int indexB;
        private uint[] indices;

        public Mesh(string sPos, string sNor)
        {
            data = new float[2][];
            buffers = new int[2];
            bVars = new string[] { sPos, sNor };
            
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            for(int i = 0; i < 2; i++)
                buffers[i] = GL.GenBuffer();
            indexB = GL.GenBuffer();    
        }
        
        public void SetBuffer(float[] b, BufferType type)
        {
            if(type == BufferType.VERTEX)
                this.size = b.Length;
            data[(int)type] = b;
        }

        public void SetIndices(uint[] i)
        {
            indices = i;
        }

        public float[] Buffer(BufferType type)
        {
            return data[(int)type];
        }

        public uint[] Indices()
        {
            return indices;
        }

        public void UploadBuffer(Shader s, BufferType type)
        {
            int index = (int)type;
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffers[index]);
            GL.BufferData<float>(
                BufferTarget.ArrayBuffer,
                (IntPtr)(data[index].Length * sizeof(float)),
                data[index],
                BufferUsageHint.StaticDraw
                );
            GL.VertexAttribPointer(s.GetVar(bVars[index]), 3,
                VertexAttribPointerType.Float, false, 0, 0);
        }

        public void UploadIndices(Shader s)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexB);
            GL.BufferData<uint>(
                BufferTarget.ElementArrayBuffer,
                (IntPtr)(indices.Length * sizeof(uint)),
                indices,
                BufferUsageHint.StaticDraw
                );
        }

        public void Render(Shader s)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexB);
            for (int i = 0; i < bVars.Length; i++)
                GL.EnableVertexAttribArray(s.GetVar(bVars[i]));
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}