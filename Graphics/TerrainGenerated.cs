using System;
using OpenTK;

namespace Template
{
    public class TerrainGenerated : VolumetricObject
    {
        protected int size;
        protected float heightScale;
        protected float scale;
        private float t = 0;
        
        public TerrainGenerated(int size = 512, float scale = 1f, float heightScale = 50f) 
            : base(null, null)
        {
            this.heightScale = heightScale;
            this.size = size;
            this.scale = scale;
            
            //Shader = new Shader("../../assets/gridTerrainVS.glsl", "../../assets/gridTerrainFS.glsl"); // Its magic :S
            Shader = new Shader("../../assets/stdVS.glsl", "../../assets/stdFS.glsl");
            Shader.AddAttributeVar("vPos");
            Shader.AddAttributeVar("vNor");
            Shader.AddUniformVar("uViewM");
            Shader.AddUniformVar("uWorldM");
            Shader.AddUniformVar("uModelM");
            //Shader.AddUniformVar("uMat");
            //Shader.AddUniformVar("uMaxHeight");
            Shader.AddUniformVar("uLightDir");
            
            Mesh = new TerrainMesh("vPos", "vNor", size, size, this.heightScale, Shader);
            Generate();
            Bake(Shader);

            Position = new Vector3(0, 0, 0);
            Rotation = new Quaternion(0, 0, 0);
            Scale = new Vector3(1, 1, 1) * 0.5f;
        }

        public override void Update()
        {
            base.Update();
            t += 0.01f;
            Rotation = new Quaternion(-t, 0, 0);
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
                ((TerrainMesh)Mesh).map[x, y] = (float)res;
            }
            ((TerrainMesh)Mesh).NormalizeMap();
        }
        
        public void Bake(Shader s)
        {
            ((TerrainMesh)Mesh).BakeMap();
            ((TerrainMesh)Mesh).CalcNormals();
            ((TerrainMesh)Mesh).UpdateMesh(s);
        }

        protected override void PrepareShaders(Matrix4 viewM, Matrix4 worldM, Matrix4 modelM)
        {
            base.PrepareShaders(viewM, worldM, modelM);
            //Shader.SetVar("uMaxHeight", 50);
            Shader.SetVar("uLightDir", new Vector3(1f, 0f, 1f));
        }
    }
}