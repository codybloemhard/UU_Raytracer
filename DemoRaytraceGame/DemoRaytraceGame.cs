using Engine;
using System.Drawing;
using RaytraceEngine;
using OpenTK;
using OpenTK.Input;
using RaytraceEngine.Objects;
using GameWindow = Engine.GameWindow;

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
            floor.Normal = new Vector3(0, 1, 0);
            floor.Material = new Material(Vector3.One, 0f, 0f, 0f);
            Scene.AddObject(floor);

            var sphere1 = new Sphere();
            sphere1.Position = new Vector3(-3, 1, 4);
            sphere1.Radius = 1;
            sphere1.Material = new Material(new Vector3(1, 0.2f, 0.2f), 0f, 0f, 0f);
            Scene.AddObject(sphere1);

            var sphere2 = new Sphere();
            sphere2.Position = new Vector3(0, 1, 4);
            sphere2.Radius = 1;
            sphere2.Material = new Material(new Vector3(0.2f, 1, 0.2f), 0f, 0f, 0f);
            Scene.AddObject(sphere2);
            
            var sphere3 = new Sphere();
            sphere3.Position = new Vector3(3, 1, 4);
            sphere3.Radius = 1;
            sphere3.Material = new Material(new Vector3(0.2f, 0.2f, 1), 0f, 0f, 0f);
            Scene.AddObject(sphere3);

            Scene.ambientLight = new Vector3(1f) * 0.1f;

            var light1 = new PointLight();
            light1.Intensity = Vector3.One * 100;
            light1.Position = new Vector3(0, 4, 5);
            Scene.AddObject(light1);

            var light2 = new PointLight();
            light2.Intensity = Vector3.One * 100;
            light2.Position = new Vector3(2f, 4f, 2f);
            Scene.AddObject(light2);
        }

        private int anglx, angly, anglz = 0;
        public override void Update()
        {
            base.Update();
            var delta = 1;
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Left)) angly += delta;
            if (keyState.IsKeyDown(Key.Right)) angly -= delta;
            if (keyState.IsKeyDown(Key.Up)) anglx += delta;
            if (keyState.IsKeyDown(Key.Down)) anglx -= delta;
            if (keyState.IsKeyDown(Key.W)) anglz += delta;
            if (keyState.IsKeyDown(Key.S)) anglz -= delta;
            Scene.CurrentCamera.Rotation = new Quaternion(MathHelper.DegreesToRadians(anglx), MathHelper.DegreesToRadians(angly), MathHelper.DegreesToRadians(anglz));
        }

        public override void Render2D()
        {
            base.Render2D();
            DebugRenderer.Render(Screen, Scene);
        }
    }
}