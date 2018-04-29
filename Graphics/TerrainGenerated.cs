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
    public class TerrainGenerated
    {
        private TerrainMesh mesh;
        private int size = 512;
        private float scale = 1f;

        public TerrainGenerated(float hScale, Shader s)
        {
            mesh = new TerrainMesh(size, size, hScale, s);
            Generate();

            mesh.BakeMap();
            mesh.CalcNormals();
            mesh.UpdateMesh();
        }

        public void Generate()
        {
            for(int x = 0; x < size; x++)
                for(int y = 0; y < size; y++)
                {
                    double res = 0;
                    res = PerlinNoise.Perlin(x * 0.01f * scale, y * 0.01f * scale);
                    res = Math.Pow(res, 4);
                    double tmp = PerlinNoise.Perlin(x * 0.05f * scale, y * 0.05f * scale);
                    res += 0.1f * tmp * tmp;
                    tmp = PerlinNoise.Perlin(x * 0.32f * scale, y * 0.32f * scale);
                    res += tmp * 0.01f;
                    mesh.map[x, y] = (float)res;
                }
            mesh.NormalizeMap();
        }

        public void Render(ref Matrix4 m, Vector3 lightDir)
        {
            mesh.Render(ref m, lightDir);
        }
    }
}