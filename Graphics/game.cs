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
            var camera = new Camera(new Vector3(0, 0, -50 * 3.0f), new Quaternion(0, 1 * 0.01f, 120 * Help.Deg2Rad));
            CurrentScene = new Scene(camera);

            CurrentScene.Objects.Add(new TerrainGenerated(512, 1, 50));
            //CurrentScene.Objects.Add(new TerrainImage("../../assets/kikker.png", 1, 10));
        }

        public void Update()
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

        public void Render()
        {
            Matrix4 worldM = Matrix4.Identity;
            worldM *= Matrix4.CreateFromQuaternion(new Quaternion(0, 0, 0));
            worldM *= Matrix4.CreateTranslation(0, 0, 0);
            worldM *= Matrix4.CreateScale(1, 1, 1);
            CurrentScene.Render(Matrix4.Identity, worldM);
        }
    }
}