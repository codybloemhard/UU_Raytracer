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
        
        private Shader s;
        private TerrainHeightMap hmTerrain;
        private TerrainGenerated gnTerrain;
        private float hScale = 50;

        private Vector3 lightDir = new Vector3(1, 0.5f, 1.5f);
        private const float degToRad = 0.0174532925f;

        public void Init()
        {
            s = new Shader("../../assets/gridTerrainVS.glsl", "../../assets/gridTerrainFS.glsl");
            //hmTerrain = new TerrainHeightMap("../../assets/map0.png", hScale, s);
            gnTerrain = new TerrainGenerated(hScale, s);
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

            //hmTerrain.Render(ref m, lightDir);
            gnTerrain.Render(ref m, lightDir);
        }
    }
}