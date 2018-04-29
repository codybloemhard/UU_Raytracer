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
    public class TerrainHeightMap
    {
        private TerrainMesh mesh;
        private Surface img;

        public TerrainHeightMap(string pos, string nor, string file, float hScale, Shader s)
        {
            img = new Surface(file);
            mesh = new TerrainMesh(pos, nor, img.width, img.height, hScale, s);
            for (int y = 0; y < mesh.h; y++)
                for (int x = 0; x < mesh.w; x++)
                    mesh.map[x, y] = ((float)(img.pixels[x + y * mesh.w] & 255)) / 256;

            mesh.NormalizeMap();
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