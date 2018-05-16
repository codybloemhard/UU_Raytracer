using Engine;
using System.Drawing;
using Engine.Objects;
using RaytraceEngine;
using OpenTK;
using OpenTK.Input;
using RaytraceEngine.Objects;
using GameWindow = Engine.GameWindow;
using OpenTK.Graphics.OpenGL;
using RaytraceEngine.Objects.Lights;
using Engine.TemplateCode;

namespace DemoRaytraceGame
{
    public class DemoRaytraceGame : RaytraceGame
    {
        protected DebugRenderer DebugRenderer;
        protected FXAAPlane FXAA;

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
            FXAA = new FXAAPlane(Screen, ScreenID);

            // Gebruik show edges om te kijken ward wordt ge antialiast
            //FXAA.ShowEdges = true;
            //FXAA.LumaThreashold = 0.1f;

            var camera = new Camera(new Vector3(0, 1, 0), new Quaternion(0, 0, 0), 1, 1);
            camera.Aspect = 1;
            CubeMap sky = new CubeMap("assets/cubemap.png");

            Scene = new RayScene(camera, sky);

            Surface wallTex = new Surface("assets/wall.png");
            Surface woodTex = new Surface("assets/wood.png");
            Surface metalTex = new Surface("assets/metal.png");
            
            var floor = new Plane();
            floor.Position = new Vector3(0, 0, 0);
            floor.Material = new Material(Vector3.One, 0.05f, 0.5f, 0f);
            floor.Material.Texture = wallTex;
            floor.Material.TextureScale = 4f;
            Scene.AddObject(floor);
            
            Plane wall1 = new Plane();
            wall1.Position = new Vector3(0, 0, 5);
            wall1.Rotation = new Quaternion(MathHelper.DegreesToRadians(-90), 0, 0);
            wall1.Material = new Material(Vector3.One, 0.0f, 0f, 0f);
            wall1.Material.Texture = woodTex;
            wall1.Material.TextureScale = 8f;
            Scene.AddObject(wall1);
            
            Plane wall2 = new Plane();
            wall2.Position = new Vector3(0, 0, -1);
            wall2.Rotation = new Quaternion(MathHelper.DegreesToRadians(90), 0, 0);
            wall2.Material = new Material(Vector3.One, 0f, 0f, 0f);
            wall2.Material.Texture = woodTex;
            wall2.Material.TextureScale = 8f;
            //Scene.AddObject(wall2);

            Plane roof = new Plane();
            roof.Position = new Vector3(0, 7, 0);
            roof.Rotation = new Quaternion(MathHelper.DegreesToRadians(180), 0, 0);
            roof.Material = new Material(new Vector3(1f), 0f, 0f, 0f);
            roof.Material.Texture = metalTex;
            roof.Material.TextureScale = 8f;
            //Scene.AddObject(roof);

            var sphere1 = new Sphere();
            sphere1.Position = new Vector3(-3, 1, 3.5f);
            sphere1.Radius = 1;
            sphere1.Material = new Material(new Vector3(1, 0.2f, 0.2f), 0f, 0.5f, 0f);
            Scene.AddObject(sphere1);

            var sphere2 = new Sphere();
            sphere2.Position = new Vector3(0, 1, 4);
            sphere2.Radius = 1;
            sphere2.Material = new Material(new Vector3(1f), 0.2f, 1f, 0f);
            Scene.AddObject(sphere2);
            
            var sphere3 = new Sphere();
            sphere3.Position = new Vector3(3, 1, 3.5f);
            sphere3.Radius = 1;
            sphere3.Material = new Material(new Vector3(0.2f, 0.2f, 1), 0f, 0f, 0f);
            Scene.AddObject(sphere3);
            
            var sphere4 = new Sphere();
            sphere4.Position = new Vector3(-1, 1, 2f);
            sphere4.Radius = 1;
            sphere4.Material = new Material(new Vector3(1f), 0f, 0f, 0.6f, 1.5f);
            //Scene.AddObject(sphere4);

            var triangle1 = new Triangle();
            triangle1.Vertices = new Vector3[] { new Vector3(-1, 0, 1), new Vector3(1, 0, 1), new Vector3(0, 1, 1) };
            triangle1.Material = new Material(new Vector3(0.9f, 0.1f, 0.1f), 0f, .8f, 0f);
            Scene.AddObject(triangle1);

            var light1 = new SphereAreaLight(2048);
            light1.Colour = Vector3.One;
            light1.Intensity = 1000;
            light1.MaxEnergy = 1f;
            light1.Position = new Vector3(0f, 1f, 2f);
            light1.Radius = 1f;
            Scene.AddObject(light1);
            
            var light2 = new SpotLightMultiSample(2048);
            light2.Colour = new Vector3(1f, 1f, 0.1f).Normalized();
            light2.Intensity = 2000;
            light2.MaxEnergy = 1;
            light2.Position = new Vector3(-3, 6, 4f);
            light2.Radius = 1f;
            light2.Normal = (sphere1.Position - light2.Position).Normalized();
            light2.AngleMin = 15;
            light2.AngleMax = 20f;
            //Scene.AddObject(light2);
            
            var light3 = new SphereAreaLight(2048);
            light3.Colour = Vector3.One;
            light3.Intensity = 100;
            light3.MaxEnergy = 2f;
            light3.Position = new Vector3(-3f, 1f, 6f);
            light3.Radius = 0.5f;
            //Scene.AddObject(light3);

            TraceSettings.Multithreading = true;
            TraceSettings.AmbientLight = new Vector3(1f) * 0.05f;
            TraceSettings.RealLightSample = false;
            TraceSettings.MaxLightSamples = 4;
            TraceSettings.RecursionDepth = 3;
            TraceSettings.AntiAliasing = 1;
            TraceSettings.MaxReflectionSamples = 4;
            FXAA.EnableFXAA = false;
            FXAA.LumaThreashold = 0.1f;
            FXAA.MulReduce = 8f;
            FXAA.MinReduce = 128f;
            FXAA.MaxSpan = 8f;
            FXAA.ShowEdges = false;
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
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            if(shouldRender) Screen.Clear(0);
            Render2D();
            
            FXAA.Render();
        }
    }
}