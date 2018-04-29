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

        public TerrainMesh(string pos, string nor, int w, int h, float hScale)
        {
            this.hScale = hScale;
            this.w = w;
            this.h = h;
            halfW = w / 2;
            halfH = h / 2;

            mesh = new Mesh(pos, nor);
            int len = (w - 1) * (h - 1) * 2 * 3 * 3;
            normals = new Vector3[w, h];
            vertexData = new float[len];
            normalData = new float[len];
            map = new float[w, h];
            //create vertices
            int i = 0;
            for (int x = 0; x < w - 1; x++)
                for (int y = 0; y < h - 1; y++)
                {
                    vertexData[i + 0] = x - halfW;
                    vertexData[i + 1] = y - halfH;
                    vertexData[i + 2] = 0;

                    vertexData[i + 3] = x + 1 - halfW;
                    vertexData[i + 4] = y - halfH;
                    vertexData[i + 5] = 0;

                    vertexData[i + 6] = x + 1 - halfW;
                    vertexData[i + 7] = y + 1 - halfH;
                    vertexData[i + 8] = 0;

                    vertexData[i + 9] = x + 1 - halfW;
                    vertexData[i + 10] = y + 1 - halfH;
                    vertexData[i + 11] = 0;

                    vertexData[i + 12] = x - halfW;
                    vertexData[i + 13] = y + 1 - halfH;
                    vertexData[i + 14] = 0;

                    vertexData[i + 15] = x - halfW;
                    vertexData[i + 16] = y - halfH;
                    vertexData[i + 17] = 0;

                    i += 18;
                }
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
            for (int x = 0; x < w - 1; x++)
                for (int y = 0; y < h - 1; y++)
                {
                    vertexData[i + 2] = -map[x, y] * hScale;
                    vertexData[i + 5] = -map[x + 1, y] * hScale;
                    vertexData[i + 8] = -map[x + 1, y + 1] * hScale;
                    vertexData[i + 11] = -map[x + 1, y + 1] * hScale;
                    vertexData[i + 14] = -map[x, y + 1] * hScale;
                    vertexData[i + 17] = -map[x, y] * hScale;
                    i += 18;
                }
        }

        public void CalcNormals()
        {
            //put normals in a grid
            Vector3 zero = new Vector3(0, 0, 0);
            for (int x = 0; x < w - 1; x++)
                for (int y = 0; y < h - 1; y++)
                    normals[x, y] = zero;
            Vector3 surfN = new Vector3(0, 0, 0);
            int i = 0;
            for (int x = 0; x < w - 1; x++)
                for (int y = 0; y < h - 1; y++)
                {
                    surfN = Help.Normal(vertexData, i);
                    normals[x, y] += surfN;
                    normals[x + 1, y] += surfN;
                    normals[x + 1, y + 1] += surfN;

                    surfN = Help.Normal(vertexData, i + 9);
                    normals[x + 1, y + 1] += surfN;
                    normals[x, y + 1] += surfN;
                    normals[x, y] += surfN;

                    i += 18;
                }
            //create normals from grid
            i = 0;
            for (int x = 0; x < w - 1; x++)
                for (int y = 0; y < h - 1; y++)
                {
                    normalData[i + 0] = normals[x, y].X;
                    normalData[i + 1] = normals[x, y].Y;
                    normalData[i + 2] = normals[x, y].Z;

                    normalData[i + 3] = normals[x + 1, y].X;
                    normalData[i + 4] = normals[x + 1, y].Y;
                    normalData[i + 5] = normals[x + 1, y].Z;

                    normalData[i + 6] = normals[x + 1, y + 1].X;
                    normalData[i + 7] = normals[x + 1, y + 1].Y;
                    normalData[i + 8] = normals[x + 1, y + 1].Z;

                    normalData[i + 9] = normals[x + 1, y + 1].X;
                    normalData[i + 10] = normals[x + 1, y + 1].Y;
                    normalData[i + 11] = normals[x + 1, y + 1].Z;

                    normalData[i + 12] = normals[x, y + 1].X;
                    normalData[i + 13] = normals[x, y + 1].Y;
                    normalData[i + 14] = normals[x, y + 1].Z;

                    normalData[i + 15] = normals[x, y].X;
                    normalData[i + 16] = normals[x, y].Y;
                    normalData[i + 17] = normals[x, y].Z;

                    i += 18;
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