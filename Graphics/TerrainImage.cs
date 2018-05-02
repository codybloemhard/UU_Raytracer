using System;
using OpenTK;

namespace Template
{
    /*public class TerrainImage : VolumetricObject
    {
        protected int width, height;
        protected float heightScale;
        protected float scale;
        protected Surface img;

        public TerrainImage(string file, float scale = 1f, float heightScale = 50f) 
            : base(null, null)
        {
            this.heightScale = heightScale;
            this.scale = scale;

            Shader = new Shader("../../assets/gridTerrainVS.glsl", "../../assets/gridTerrainFS.glsl"); // Its magic :S
            Shader.AddAttributeVar("vPos");
            Shader.AddAttributeVar("vNor");
            Shader.AddUniformVar("uMat");
            Shader.AddUniformVar("uMaxHeight");
            Shader.AddUniformVar("uLightDir");

            img = new Surface(file);
            this.width = img.width;
            this.height = img.height;

            Mesh = new TerrainMesh("vPos", "vNor", width, height, this.heightScale, Shader);
            TerrainMesh tm = (TerrainMesh)Mesh;
            for (int y = 0; y < width; y++)
                for (int x = 0; x < height; x++)
                    tm.map[x, y] = ((float)(img.pixels[x + y * tm.w] & 255)) / 256;
            tm.BakeMap();
            tm.CalcNormals();
            tm.UpdateMesh(Shader);
        }
        
        protected override void PrepareShaders(Matrix4 baseMat)
        {
            base.PrepareShaders(baseMat);
            Shader.SetVar("uMaxHeight", 50);
            Shader.SetVar("uLightDir", new Vector3(1f, 0f, 1f));
        }
    }*/
}