using Engine;
using Engine.Helpers;
using Engine.Objects;
using OpenTK;
using OpenTK.Input;
using Template;
using GameWindow = Engine.GameWindow;

namespace DemoGame
{
    public class DemoGame : Game
    {
        public static void Main(string[] args)
        {
            //using (var win = new GameWindow(new DemoGame())) { win.Run(30.0, 60.0); }
            using (var win = new GameWindow(new DemoRaytrace())) { win.Run(30.0, 60.0); }
        }

        public override void Init()
        {
            var camera = new Camera(new Vector3(0, 0, -50 * 3.0f), new Quaternion(0, 1 * 0.01f, 120 * Help.Deg2Rad));
            CurrentScene = new Scene(camera);
            
            CurrentScene.Objects.Add(new TerrainGenerated(512, 1, 50));
        }

        public override void Update()
        {
            CurrentScene.Update();
            var delta = 0.2f;
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Left)) CurrentScene.CurrentCamera.Position += new Vector3(delta, 0, 0);
            if (keyState.IsKeyDown(Key.Right)) CurrentScene.CurrentCamera.Position += new Vector3(-delta, 0, 0);
            if (keyState.IsKeyDown(Key.Up)) CurrentScene.CurrentCamera.Position += new Vector3(0, delta, 0);
            if (keyState.IsKeyDown(Key.Down)) CurrentScene.CurrentCamera.Position += new Vector3(0, -delta, 0);
            if (keyState.IsKeyDown(Key.W)) CurrentScene.CurrentCamera.Position += new Vector3(0, 0, delta);
            if (keyState.IsKeyDown(Key.S)) CurrentScene.CurrentCamera.Position += new Vector3(0, 0, -delta);
        }

        public override void Render()
        {
            Matrix4 worldM = Matrix4.Identity;
            worldM *= Matrix4.CreateFromQuaternion(new Quaternion(0, 0, 0));
            worldM *= Matrix4.CreateTranslation(0, 0, 0);
            worldM *= Matrix4.CreateScale(1, 1, 1);
            CurrentScene.Render(Matrix4.Identity, worldM);
        }
    }
}