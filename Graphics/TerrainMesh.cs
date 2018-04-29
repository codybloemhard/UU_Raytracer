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
    public class TerrainMesh
    {
        public float[,] map;
        private Mesh mesh;
        private Vector3[,] normals;
        private float[] vertexData, normalData;
        private int halfW, halfH;
        private float hScale;
        public int w { get; private set; }
        public int h { get; private set; }

        public TerrainMesh(string pos, string nor, int w, int h, float hScale, Shader s)
        {
            this.hScale = hScale;
            this.w = w;
            this.h = h;
            halfW = w / 2;
            halfH = h / 2;

            mesh = new Mesh(pos, nor);
            int len = (w) * (h) * 3;
            normals = new Vector3[w, h];
            vertexData = new float[len];
            normalData = new float[len];
            map = new float[w, h];
            //create vertices
            int i = 0;
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    vertexData[i + 0] = x - halfW;
                    vertexData[i + 1] = y - halfH;
                    vertexData[i + 2] = 0;
                    i += 3;
                }
            //create indices
            uint[] indices = new uint[(w - 1) * (h - 1) * 6];
            i = 0;
            for (int x = 0; x < w - 1; x++)
                for (int y = 0; y < h - 1; y++)
                {
                    indices[i + 0] = (uint)((x + 0) * w + (y + 0));
                    indices[i + 1] = (uint)((x + 1) * w + (y + 0));
                    indices[i + 2] = (uint)((x + 1) * w + (y + 1));

                    indices[i + 3] = (uint)((x + 1) * w + (y + 1));
                    indices[i + 4] = (uint)((x + 0) * w + (y + 1));
                    indices[i + 5] = (uint)((x + 0) * w + (y + 0));
                    i += 6;
                }
            mesh.SetIndices(indices, indices.Length);
            mesh.UploadIndices(s);
        }
        
        public void NormalizeMap()
        {
            //find max height
            float hMax = 0f, hMin = 10000000f;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    if (map[x, y] > hMax)
                        hMax = map[x, y];
                    if (map[x, y] < hMin)
                        hMin = map[x, y];
                }
            //normalize heightmap
            float diff = hMax - hMin;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    map[x, y] -= hMin;
                    map[x, y] /= diff;
                }
        }

        public void BakeMap()
        {
            int i = 0;
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    vertexData[i + 2] = -map[x, y] * hScale;
                    i += 3;
                }
        }
        
        public void CalcNormals()
        {
            //put normals in a grid
            Vector3 zero = new Vector3(0, 0, 0);
            for (int x = 0; x < w - 1; x++)
                for (int y = 0; y < h - 1; y++)
                    normals[x, y] = zero;
            Vector3 a = Vector3.Zero, b = Vector3.Zero, c = Vector3.Zero;
            for (int x = 0; x < w - 1; x++)
                for (int y = 0; y < h - 1; y++)
                {
                    a = Help.Read(vertexData, ((x + 0) * w + (y + 0)) * 3);
                    b = Help.Read(vertexData, ((x + 1) * w + (y + 0)) * 3);
                    c = Help.Read(vertexData, ((x + 1) * w + (y + 1)) * 3);
                    normals[x + 0, y + 0] += Help.Normal(a, b, c);
                    normals[x + 1, y + 0] += Help.Normal(a, b, c);
                    normals[x + 1, y + 1] += Help.Normal(a, b, c);
                    a = Help.Read(vertexData, ((x + 1) * w + (y + 1)) * 3);
                    b = Help.Read(vertexData, ((x + 0) * w + (y + 1)) * 3);
                    c = Help.Read(vertexData, ((x + 0) * w + (y + 0)) * 3);
                    normals[x + 1, y + 1] += Help.Normal(a, b, c);
                    normals[x + 0, y + 1] += Help.Normal(a, b, c);
                    normals[x + 0, y + 0] += Help.Normal(a, b, c);
                }
            //create normals from grid
            int i = 0;
            for (int x = 0; x < w - 0; x++)
                for (int y = 0; y < h - 0; y++)
                {
                    normalData[i + 0] = normals[x, y].X;
                    normalData[i + 1] = normals[x, y].Y;
                    normalData[i + 2] = normals[x, y].Z;
                    i += 3;
                }
        }

        public void UpdateMesh(Shader s)
        {
            mesh.SetBuffer(vertexData, vertexData.Length, BufferType.VERTEX);
            mesh.SetBuffer(normalData, normalData.Length, BufferType.NORMAL);
            mesh.UploadBuffer(s, BufferType.VERTEX);
            mesh.UploadBuffer(s, BufferType.NORMAL);
        }

        public void Render(Shader s)
        {
            mesh.Render(s);
        }
    }
}