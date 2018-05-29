using System;
using System.Drawing;
using System.Net.WebSockets;
using FrockRaytracer.Graphics;
using FrockRaytracer.Objects;
using FrockRaytracer.Structs;
using ImageMagick;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace FrockRaytracer
{
    public class Window : GameWindow
    {
        protected ProjectionPlane Projection;
        public MultiResolutionRaster Raster => Projection.Raster;
        public World World;
        public RaytraceMotherBee MotherBee;
        private int PresetID = 2;

        public Window(Size size)
        {
            ClientSize = size;
            Projection = new ProjectionPlane();
            World = new World(new Camera(new Vector3(0, 1, 0), Quaternion.Identity));
            MotherBee = new RaytraceMotherBee(new Raytracer(), Raster);
            Settings.FastMediumQualityPreset();
        }

        protected override void OnLoad(EventArgs e)
        {
            // Creates the projection plane and all the other defaults
            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            
            Projection.Init();
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
            MotherBee.StartRender(World, Raster);
            MotherBee.MaintainThreads();
            
            Projection.Render();
            SwapBuffers();
        }

        /// <summary>
        /// Readjusts the viewport to new settings
        /// </summary>
        private void UpdateViewport()
        {
            Projection.Resize(ClientSize);

            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
        }

        public virtual void Init() {}

        public virtual void Update()
        {
            var keyState = Keyboard.GetState();
            // Check for preset keys
            if (keyState.IsKeyDown(Key.Number1) && PresetID != 0) {
                Settings.LowQualityPreset();
                PresetID = 0;
                MotherBee.Cancel();
                Raster.Resize(Raster.BaseWidth, Raster.BaseHeight, Settings.RenderMSAALevels);
                Raster.SwitchLevel(0, false, true);
                World.Changed = true;
            } else if (keyState.IsKeyDown(Key.Number2) && PresetID != 1) {
                Settings.FastMediumQualityPreset();
                PresetID = 1;
                MotherBee.Cancel();
                Raster.Resize(Raster.BaseWidth, Raster.BaseHeight, Settings.RenderMSAALevels);
                Raster.SwitchLevel(0, false, true);
                World.Changed = true;
            } else if (keyState.IsKeyDown(Key.Number3) && PresetID != 2) {
                Settings.MediumQualityPreset();
                PresetID = 2;
                MotherBee.Cancel();
                Raster.Resize(Raster.BaseWidth, Raster.BaseHeight, Settings.RenderMSAALevels);
                Raster.SwitchLevel(0, false, true);
                World.Changed = true;
            } else if (keyState.IsKeyDown(Key.Number4) && PresetID != 3) {
                Settings.FastHighQualityPreset();
                PresetID = 3;
                MotherBee.Cancel();
                Raster.Resize(Raster.BaseWidth, Raster.BaseHeight, Settings.RenderMSAALevels);
                Raster.SwitchLevel(0, false, true);
                World.Changed = true;
            } else if (keyState.IsKeyDown(Key.Number5) && PresetID != 3) {
                Settings.HighQualityPreset();
                PresetID = 3;
                MotherBee.Cancel();
                Raster.Resize(Raster.BaseWidth, Raster.BaseHeight, Settings.RenderMSAALevels);
                Raster.SwitchLevel(0, false, true);
                World.Changed = true;
            } else if (keyState.IsKeyDown(Key.Number6) && PresetID != 4) {
                Settings.UltraQualityPreset();
                PresetID = 4;
                MotherBee.Cancel();
                Raster.Resize(Raster.BaseWidth, Raster.BaseHeight, Settings.RenderMSAALevels);
                Raster.SwitchLevel(0, false, true);
                World.Changed = true;
            }
        }

        public virtual void Render()
        {
        }
    }
}