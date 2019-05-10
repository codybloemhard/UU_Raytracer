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
        private int PresetID = 1, AAPreset = 2;

        public Window(Size size)
        {
            ClientSize = size;
            Projection = new ProjectionPlane();
            World = new World(new Camera(new Vector3(0, 1, 0), Quaternion.Identity));
            MotherBee = new RaytraceMotherBee(new Raytracer(), Raster, false);
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

        protected void PresetChanged(int presetID, bool aaPreset = false)
        {
            if((!aaPreset && presetID == PresetID) || (aaPreset && presetID == AAPreset)) return;
            if (!aaPreset) PresetID = presetID;
            else AAPreset = presetID;
            
            MotherBee.Cancel();
            Raster.Resize(Raster.BaseWidth, Raster.BaseHeight, Settings.RenderMSAALevels);
            Raster.SwitchLevel(0, false, true);
            World.Changed = true;
        }

        public virtual void Init() {}

        public virtual void Update()
        {
            if(!Focused) return;
            
            // Preset management
            var keyState = Keyboard.GetState();
            // Check for preset keys
            if (keyState.IsKeyDown(Key.Number1)) {
                Settings.LowQualityPreset();
                PresetChanged(0);
            } else if (keyState.IsKeyDown(Key.Number2)) {
                Settings.FastMediumQualityPreset();
                PresetChanged(1);
            } else if (keyState.IsKeyDown(Key.Number3)) {
                Settings.MediumQualityPreset();
                PresetChanged(2);
            } else if (keyState.IsKeyDown(Key.Number4)) {
                Settings.FastHighQualityPreset();
                PresetChanged(3);
            } else if (keyState.IsKeyDown(Key.Number5)) {
                Settings.FastHighQualityPreset();
                PresetChanged(4);
            } else if (keyState.IsKeyDown(Key.Number6)) {
                Settings.HighQualityPreset();
                PresetChanged(5);
            } else if (keyState.IsKeyDown(Key.Number7)) {
                Settings.UltraQualityPreset();
                PresetChanged(6);
            } else if (keyState.IsKeyDown(Key.Y)) {
                Settings.LowAAPreset();
                PresetChanged(0, true);
            } else if (keyState.IsKeyDown(Key.U)) {
                Settings.MiddleAAPreset();
                PresetChanged(1, true);
            } else if (keyState.IsKeyDown(Key.I)) {
                Settings.HighAAPreset();
                PresetChanged(2, true);
            } else if (keyState.IsKeyDown(Key.O)) {
                Settings.UltraAPreset();
                PresetChanged(3, true);
            } else if (keyState.IsKeyDown(Key.P)) {
                Settings.PhotoPreset();
                PresetChanged(4, true);
            }

            // Camera Movement
            if (keyState.IsKeyDown(Key.Down)) {
                World.Camera.RotateBy(new Vector3(3, 0, 0));
                World.Changed = true;
            }
            if (keyState.IsKeyDown(Key.Up)) {
                World.Camera.RotateBy(new Vector3(-3, 0, 0));
                World.Changed = true;
            }
            if (keyState.IsKeyDown(Key.Right)) {
                World.Camera.RotateBy(new Vector3(0, 3, 0));
                World.Changed = true;
            }
            if (keyState.IsKeyDown(Key.Left)) {
                World.Camera.RotateBy(new Vector3(0, -3, 0));
                World.Changed = true;
            }
            if (keyState.IsKeyDown(Key.W)) {
                World.Camera.Position +=  new Vector3(0, 0, 0.1f);
                World.Changed = true;
            }
            if (keyState.IsKeyDown(Key.S)) {
                World.Camera.Position +=  new Vector3(0, 0, -0.1f);
                World.Changed = true;
            }
            if (keyState.IsKeyDown(Key.D)) {
                World.Camera.Position += new Vector3(0.1f, 0, 0);
                World.Changed = true;
            }
            if (keyState.IsKeyDown(Key.A)) {
                World.Camera.Position += new Vector3(-0.1f, 0, 0);
                World.Changed = true;
            }
            if (keyState.IsKeyDown(Key.E)) {
                World.Camera.Position += new Vector3(0, 0.1f, 0);
                World.Changed = true;
            }
            if (keyState.IsKeyDown(Key.Q)) {
                World.Camera.Position += new Vector3(0, -0.1f, 0);
                World.Changed = true;
            }
            if (keyState.IsKeyDown(Key.R)) {
                World.Camera.Rotation +=Quaternion.Identity;
                World.Changed = true;
            }
            
        }
        
        public virtual void Render()
        {
        }
    }
}