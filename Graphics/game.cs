using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;


namespace Template
{
    public class Game
    {
        private Scene CurrentScene { get; set; }

        public void Init()
        {
            var camera = new Camera(new Vector3(0, 0, -50 * 3.0f), new Quaternion(0, 1 * 0.01f, 120 * 0.0174532925f));
            CurrentScene = new Scene(camera);

            CurrentScene.Objects.Add(new TerrainObject(512, 1, 50));
        }

        public void Update()
        {
            var delta = 0.2f;
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Left)) CurrentScene.CurrentCamera.Position += new Vector3(delta, 0, 0);
            if (keyState.IsKeyDown(Key.Right)) CurrentScene.CurrentCamera.Position += new Vector3(-delta, 0, 0);
            if (keyState.IsKeyDown(Key.Up)) CurrentScene.CurrentCamera.Position += new Vector3(0, delta, 0);
            if (keyState.IsKeyDown(Key.Down)) CurrentScene.CurrentCamera.Position += new Vector3(0, -delta, 0);
            if (keyState.IsKeyDown(Key.J)) CurrentScene.CurrentCamera.Position += new Vector3(0, 0, delta);
            if (keyState.IsKeyDown(Key.K)) CurrentScene.CurrentCamera.Position += new Vector3(0, 0, -delta);
        }

        public void Render()
        {
            CurrentScene.Render(Matrix4.Identity);
        }
    }
}