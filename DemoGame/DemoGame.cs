using Engine;
using Engine.Objects;
using OpenTK;
using OpenTK.Input;
using GameWindow = Engine.GameWindow;

namespace DemoGame
{
    public class DemoGame : Game3D
    {
        private Terrain _terrain;
        private float _t = 0;
 
        public static void Main(string[] args)
        {
            using (var win = new GameWindow(new DemoGame())) { win.Run(30.0, 60.0); }
            //using (var win = new GameWindow(new DemoRaytrace())) { win.Run(30.0, 60.0); }
        }

        public override void Init()
        {
            var camera = new Camera(new Vector3(0, 0, -50 * 3.0f), new Quaternion(0, 1 * 0.01f,  MathHelper.DegreesToRadians(120)));
            CurrentScene = new Scene(camera);

            _terrain = new Terrain(512, 1, 50);
            CurrentScene.Objects.Add(_terrain);
            _terrain.Position = new Vector3(0, 0, 0);
            _terrain.Rotation = new Quaternion(0, 0, 0);
            _terrain.Scale = new Vector3(1, 1, 1) * 0.5f;
        }
        
        public override void Update()
        {
            base.Update();
            _t += 0.01f;
            _terrain.Rotation = new Quaternion(-_t, 0, 0);
            
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
    }
}