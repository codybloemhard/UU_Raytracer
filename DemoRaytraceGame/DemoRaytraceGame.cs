using Engine;
using System.Drawing;
using RaytraceEngine;
using OpenTK;
using OpenTK.Input;
using RaytraceEngine.Objects;
using GameWindow = Engine.GameWindow;
using OpenTK.Graphics.OpenGL;
using RaytraceEngine.Objects.Lights;


namespace DemoRaytraceGame
{
    public class DemoRaytraceGame : RaytraceGame
    {
        protected DebugRenderer DebugRenderer;

        public const int Width = 1024;
        public const int Height = 512;

        public static void Main(string[] args)
        {
            using (var win = new GameWindow(new Size(Width, Height), new DemoRaytraceGame())) { win.Run(30.0, 60.0); }
        }

        public override void Init()
        {
            base.Init();
            Renderer = new Raytracer(Width/2-1, Height-1);
            DebugRenderer = new DebugRenderer(Width/2-1, Height-1, Width/2);

            var camera = new Camera(new Vector3(0, 1, 0), new Quaternion(0, 0, 0), 1, 1);
            camera.Aspect = 1;
            Scene = new RayScene(camera);

            var floor = new Plane();
            floor.Position = new Vector3(0, 0, 0);
            floor.Material = new Material(Vector3.One, 0f, 0.3f, 0f);
            Scene.AddObject(floor);
            
            Plane wall = new Plane();
            wall.Position = new Vector3(0, 0, 8);
            wall.Rotation = new Quaternion(MathHelper.DegreesToRadians(-90), 0, 0);
            wall.Material = new Material(Vector3.One, 0f, 0.3f, 0f);
            Scene.AddObject(wall);

            var sphere1 = new Sphere();
            sphere1.Position = new Vector3(-3, 1, 3.5f);
            sphere1.Radius = 1;
            sphere1.Material = new Material(new Vector3(1, 0.2f, 0.2f), 0f, 0.5f, 0f);
            Scene.AddObject(sphere1);

            var sphere2 = new Sphere();
            sphere2.Position = new Vector3(0, 1, 4);
            sphere2.Radius = 1;
            sphere2.Material = new Material(new Vector3(0.2f, 1, 0.2f), 0f, 1f, 0f);
            Scene.AddObject(sphere2);
            
            var sphere3 = new Sphere();
            sphere3.Position = new Vector3(3, 1, 3.5f);
            sphere3.Radius = 1;
            sphere3.Material = new Material(new Vector3(0.2f, 0.2f, 1), 0f, 0f, 0f);
            Scene.AddObject(sphere3);

            var light1 = new SphereVolumeLight(512);
            light1.Colour = Vector3.One;
            light1.Intensity = 500;
            light1.MaxEnergy = 0.7f;
            light1.Position = new Vector3(1.5f, 3f, 2f);
            light1.Radius = 0.2f;
            Scene.AddObject(light1);

            var light2 = new SpotLightMultiSample(512);
            light2.Colour = new Vector3(1f, 1f, 0.1f).Normalized();
            light2.Intensity = 1000;
            light2.MaxEnergy = 1000;
            light2.Position = new Vector3(-1.5f, 8, 3);
            light2.Radius = 1f;
            light2.Normal = new Vector3(0, -1, 0);
            light2.AngleMin = 15;
            light2.AngleMax = 20f;
            //Scene.AddObject(light2);

            TraceSettings.ambientLight = new Vector3(1f) * 0.05f;
            TraceSettings.realLightSample = true;
            TraceSettings.maxLightSamples = 16;
            TraceSettings.recursionDepth = 3;
            TraceSettings.antiAliasing = 2;
        }

        // This is for debugging and should be removed later
        private int anglx, angly, anglz = 0;
        private int anglW = -90;
        private bool shouldRender = true;
        private Key[] InvallidateKeys = new Key[] {
            Key.Left,
            Key.Right,
            Key.Up,
            Key.Down,
            Key.W,
            Key.S,
            Key.J,
            Key.K
        };
        
        public override void Update()
        {
            base.Update();
            var delta = 1;

            var keyState = Keyboard.GetState();
            foreach (var key in InvallidateKeys) {
                if (keyState.IsKeyDown(key)) shouldRender = true;
            }
            
            if (keyState.IsKeyDown(Key.Left)) angly += delta;
            if (keyState.IsKeyDown(Key.Right)) angly -= delta;
            if (keyState.IsKeyDown(Key.Up)) anglx += delta;
            if (keyState.IsKeyDown(Key.Down)) anglx -= delta;
            if (keyState.IsKeyDown(Key.W)) anglz += delta;
            if (keyState.IsKeyDown(Key.S)) anglz -= delta;
            if (keyState.IsKeyDown(Key.J)) anglW -= delta;
            if (keyState.IsKeyDown(Key.K)) anglW += delta;
            Scene.CurrentCamera.Rotation = new Quaternion(MathHelper.DegreesToRadians(anglx), MathHelper.DegreesToRadians(angly), MathHelper.DegreesToRadians(anglz));
        }
        
        public override void Render2D()
        {
            if(!shouldRender) return; 
            DebugRenderer.Render(Screen, Scene);
            base.Render2D();
            shouldRender = false;
        }

        // TODO: remove this when we dont need to render on impulse instead of continious
        public override void Render()
        {
            if(shouldRender) Screen.Clear(0);
            Render2D();
            
            GL.BindTexture( TextureTarget.Texture2D, ScreenID );
            GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                Screen.width, Screen.height, 0, PixelFormat.Bgra, 
                PixelType.UnsignedByte, Screen.pixels 
            );
            
            // clear window contents
            GL.Clear( ClearBufferMask.ColorBufferBit );
            // setup camera
            GL.MatrixMode( MatrixMode.Modelview );
            GL.LoadIdentity();
            GL.MatrixMode( MatrixMode.Projection );
            GL.LoadIdentity();
            // draw screen filling quad
            GL.Begin( PrimitiveType.Quads );
            GL.TexCoord2( 0.0f, 1.0f ); GL.Vertex2( -1.0f, -1.0f );
            GL.TexCoord2( 1.0f, 1.0f ); GL.Vertex2(  1.0f, -1.0f );
            GL.TexCoord2( 1.0f, 0.0f ); GL.Vertex2(  1.0f,  1.0f );
            GL.TexCoord2( 0.0f, 0.0f ); GL.Vertex2( -1.0f,  1.0f );
            GL.End();
        }
    }
}