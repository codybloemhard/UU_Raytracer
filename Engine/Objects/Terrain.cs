using System;
using Engine.Graphics;
using Engine.Helpers;
using Engine.TODO;
using OpenTK;

namespace Engine.Objects
{
    public class Terrain : VolumetricObject
    {
        protected int Size;
        protected float MapScale;

        public Terrain(int size = 512, float scale = 1f, float heightScale = 50f) 
            : base(new TerrainMesh("vPos", "vNor", heightScale, size, size), null)
        {
            Size = size;
            MapScale = scale;
            
            Shader = new Shader("assets/shaders/stdVS.glsl", "assets/shaders/stdFS.glsl");
            Shader.AddAttributeVar("vPos");
            Shader.AddAttributeVar("vNor");
            Shader.AddUniformVar("uViewM");
            Shader.AddUniformVar("uWorldM");
            Shader.AddUniformVar("uModelM");
            Shader.AddUniformVar("uLightDir");

            var terrainMesh = ((TerrainMesh) Mesh);
            GenerateHeightMap(ref terrainMesh.HeightMap, Size, MapScale);
            terrainMesh.NormalizeMap();
            terrainMesh.InvalidateVertices();
            terrainMesh.CalcNormals();
            terrainMesh.Upload(Shader);
        }

        protected override void PrepareShaders(Matrix4 viewM, Matrix4 worldM, Matrix4 modelM)
        {
            base.PrepareShaders(viewM, worldM, modelM);
            Shader.SetVar("uLightDir", new Vector3(1f, 0f, 1f));
        }

        public static void GenerateHeightMap(ref float[,] heightMap, int size, float scale)
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
                heightMap[x, y] = (float)res;
            }
        }
    }
}