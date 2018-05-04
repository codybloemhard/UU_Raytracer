using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
    public class GameWindow : OpenTK.GameWindow
    {
        protected IGame Game;

        protected Size Size { get; set; } = new Size(1600, 900);

        public GameWindow(IGame game)
        {
            Game = game;
        }

        public GameWindow(Size size, IGame game)
        {
            Size = size;
            Game = game;
        }

        protected override void OnLoad(EventArgs e)
        {
            ClientSize = Size;
            Game.Resize(ClientSize.Width, ClientSize.Height);
            Game.Init();
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }

        protected override void OnUnload(EventArgs e)
        {
            Game.Destroy();
            Environment.Exit(0); // bypass wait for key on CTRL-F5
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var keyboard = OpenTK.Input.Keyboard.GetState();
            Game.Update();
            if (keyboard[OpenTK.Input.Key.Escape]) Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Game.Render();
            SwapBuffers();
        }
    }
}