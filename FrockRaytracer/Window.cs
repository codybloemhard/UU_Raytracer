using System;
using System.Drawing;
using FrockRaytracer.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace FrockRaytracer
{
    public class Window : OpenTK.GameWindow
    {
        public static int RAYTRACE_AREA_WIDTH = 512;
        public static int RAYTRACE_AREA_HEIGHT = 512;
        
        protected bool Changed = true;

        protected ProjectionPlane Projection;
        protected Raster Raster => Projection.Raster;

        public Window(Size size)
        {
            ClientSize = size;
        }

        protected override void OnLoad(EventArgs e)
        {
            // Creates the projection plane and all the other defaults
            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            Projection = new ProjectionPlane();
            Init();
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.DeleteTextures(1, ref Projection.Texture);
            Environment.Exit(0); // bypass wait for key on CTRL-F5
        }

        protected override void OnResize(EventArgs e)
        {
            UpdateViewport();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Update the keyboard
            var keyboard = OpenTK.Input.Keyboard.GetState();
            Update();
            if (keyboard[OpenTK.Input.Key.Escape]) Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Raytrace only if changed
            if (Changed) {
                Raster.Clear();
                Render();
            } 
            
            Projection.Render();
            SwapBuffers();
            Changed = false;
        }

        /// <summary>
        /// Readjusts the viewport to new settings
        /// </summary>
        private void UpdateViewport()
        {
            RAYTRACE_AREA_WIDTH = ClientSize.Width / 2;
            RAYTRACE_AREA_HEIGHT = ClientSize.Height;

            Projection.Resize(ClientSize);

            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
        }


        public virtual void Init()  {}

        public virtual void Update() { }

        public virtual void Render() {}
    }
}