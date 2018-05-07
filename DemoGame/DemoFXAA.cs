using Engine;
using Engine.Graphics;
using System.Drawing;
using Engine.Objects;
using Engine.TemplateCode;
using OpenTK.Graphics.OpenGL;

namespace DemoGame
{
    public class DemoFXAA : Game2D<Scene>
    {
        public FXAAPlane Plane;
        
        public override void Init()
        {
            base.Init();
            
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            Plane = new FXAAPlane(Screen, ScreenID);
        }

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
            
            Screen.Clear(0);
            Render2D();
            
            Plane.Render();
        }

        public override void Render2D()
        {
            Screen.Clear(0x0F0F0F);
            Screen.Line(0,0, Screen.width - 1, Screen.height - 1, 0xFFFFFF);
        }
    }
}