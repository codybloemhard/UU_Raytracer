using Engine;
using Engine.Helpers;
using Engine.Objects;
using Engine.Graphics;
using Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using GameWindow = Engine.GameWindow;

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

            quad = new Mesh("vPos", "vUv");
            
            float[] vertices = new float[] {    -1, +1, +0,
                                                +1, +1, +0,
                                                +1, -1, +0,
                                                -1, -1, +0 };
            float[] uvs = new float[] { 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0 };
            uint[] indices = new uint[] { 0, 1, 2, 2, 3, 0 };

            quad.SetIndices(indices);
            quad.UploadIndices(s);
            quad.SetBuffer(vertices, BufferType.VERTEX);
            quad.SetBuffer(uvs, BufferType.NORMAL);
            quad.UploadBuffer(s, BufferType.VERTEX);
            quad.UploadBuffer(s, BufferType.NORMAL);
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