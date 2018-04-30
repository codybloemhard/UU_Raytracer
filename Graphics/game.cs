using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{
    public class Game
    {
        private float t;
        private int mode = 1;
        
        private Shader s;
        private TerrainHeightMap hmTerrain;
        private TerrainGenerated gnTerrain;
        private SphereMesh sphere;
        private float hScale = 50;

        private Vector3 lightDir = new Vector3(1f, 0f, 1f);
        private const float degToRad = 0.0174532925f;

        public void Init()
        {
            s = new Shader("../../shaders/gridTerrainVS.glsl", "../../shaders/gridTerrainFS.glsl");
            s.AddAttributeVar("vPos");
            s.AddAttributeVar("vNor");
            s.AddUniformVar("uMat");
            s.AddUniformVar("uMaxHeight");
            s.AddUniformVar("uLightDir");
            
            if (mode == 0)
                hmTerrain = new TerrainHeightMap("vPos", "vNor", "../../assets/map0.png", hScale, s);
            else if (mode == 1)
            {
                gnTerrain = new TerrainGenerated("vPos", "vNor", hScale, s);
                gnTerrain.Bake(s);
            }
            else if (mode == 2)
                sphere = new SphereMesh(s);
            if (mode == 2) hScale = 1f;
        }
        
        public void Update()
        {
            t += 1f;
            //lightDir.Z = (float)Math.Sin(t * 0.02f) + 1f;
        }

        public void Render()
        {
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Matrix4 m = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)t * 0.01f);
            m *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0),  120 * degToRad);
            m *= Matrix4.CreateTranslation(0, 0, -hScale * 3.0f);
            m *= Matrix4.CreatePerspectiveFieldOfView(1.6f, 16f/9f, .1f, 100000000);

            s.Use();
            s.SetVar("uMat", ref m);
            s.SetVar("uMaxHeight", hScale);
            s.SetVar("uLightDir", lightDir);

            if (mode == 0) hmTerrain.Render(s);
            else if (mode == 1) gnTerrain.Render(s);
            else if (mode == 2) sphere.Render(s);
        }
    }
}