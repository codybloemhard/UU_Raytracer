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

        public static void Main(string[] args)
        {
            using (var win = new GameWindow(new Size(1024, 512), new DemoRaytraceGame())) { win.Run(30.0, 60.0); }
        }

        public override void Init()
        {
            base.Init();
            DebugRenderer = new DebugRenderer();

            var camera = new Camera(new Vector3(0, 3, 0), new Quaternion(0, 0 , 0));
            camera.Aspect = 1;
            Scene = new RayScene(camera);

            var sphere1 = new Sphere();
            sphere1.Position = new Vector3(0, 1, 6);
            sphere1.Radius = 1;
            Scene.AddObject(sphere1);
            
            var sphere2 = new Sphere();
            sphere2.Position = new Vector3(-3, 1, 6);
            sphere2.Radius = 1;
            Scene.AddObject(sphere2);
            
            var sphere3 = new Sphere();
            sphere3.Position = new Vector3(3, 1, 6);
            sphere3.Radius = 1;
            Scene.AddObject(sphere3);
            
            var light = new PointLight();
            light.Intensity = Vector3.One;
            Scene.AddObject(light);
        }

        private int anglx, angly, anglz = 0;
        public override void Update()
        {
            base.Update();
            var delta = 10;
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