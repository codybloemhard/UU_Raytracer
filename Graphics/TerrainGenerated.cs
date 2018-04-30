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
    [Obsolete] // Use Terrain object or improve it dont create mseh holders but objects which can go inside scene
    public class TerrainGenerated
    {
        private TerrainMesh mesh;
        private int size = 512;
        private float scale = 1f;

        public TerrainGenerated(string pos, string nor, float hScale, Shader s)
        {
            mesh = new TerrainMesh(pos, nor, size, size, hScale, s);
            Generate();
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

        public void Bake(Shader s)
        {
            mesh.BakeMap();
            mesh.CalcNormals();
            mesh.UpdateMesh(s);
        }

        public void Render(Shader s)
        {
            mesh.Render(s);
        }
    }
}