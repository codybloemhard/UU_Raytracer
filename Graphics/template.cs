using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{
    public class OpenTKApp : GameWindow
    {
        static Game game;
        static bool terminated = false;

        protected override void OnLoad(EventArgs e)
        {
            ClientSize = new Size(1600, 900);
            game = new Game();
            game.Init();
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
            game.Update();
            if (keyboard[OpenTK.Input.Key.Escape]) this.Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if (terminated)
            {
                Exit();
                return;
            }
            game.Render();
            SwapBuffers();
        }

        public static void Main(string[] args)
        {
            using (OpenTKApp app = new OpenTKApp()) { app.Run(30.0, 60.0); }
        }
    }
}