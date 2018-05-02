using Engine;
using Engine.Graphics;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace DemoGame
{
    public class DemoRaytrace : Game
    {
        private Mesh quad;
        private Shader s;

        public override void Init()
        {
            s = new Shader("../../../Engine/assets/shaders/traceVS.glsl", "../../../Engine/assets/shaders/traceFS.glsl");
            s.Use();
            s.AddAttributeVar("vPos");
            s.AddAttributeVar("vUv");

            quad = new Mesh();
            
            float[] vertices = new float[] {    -1, +1, +0,
                                                +1, +1, +0,
                                                +1, -1, +0,
                                                -1, -1, +0 };
            float[] uvs = new float[] { 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0 };
            uint[] indices = new uint[] { 0, 1, 2, 2, 3, 0 };

            quad.IndexBuffer = new IndexBuffer(indices);
            quad.Buffers.Add(new AttribBuffer<float>(vertices, "vPos",VertexAttribPointerType.Float));
            quad.Buffers.Add(new AttribBuffer<float>(uvs, "vUv",VertexAttribPointerType.Float));
            quad.Upload(s);
        }

        public override void Update() { }

        public override void Render()
        {
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            s.Use();
            quad.Render(s);
        }
    }
}