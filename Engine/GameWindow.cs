using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine
{
    public class GameWindow : OpenTK.GameWindow
    {
        protected Game Game;

        protected virtual Size GetSize()
        {
            return new Size(1600, 900);
        }

        public GameWindow(Game game)
        {
            Game = game;
        }

        protected override void OnLoad(EventArgs e)
        {
            ClientSize = new Size(1600, 900);
            Game.Init();
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }

        protected override void OnUnload(EventArgs e)
        {
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