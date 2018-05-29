using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using FrockRaytracer;
using FrockRaytracer.Objects;
using FrockRaytracer.Objects.Primitives;
using FrockRaytracer.Structs;
using OpenTK;
using OpenTK.Graphics.ES30;
using OpenTK.Input;

namespace FrockRaytracerDemo
{
    internal class RaytracerDemo : Window
    {
        private int SceneID = SpheresScene.SceneID;

        public static void Main(string[] args)
        {
            Settings.RenderMSAALevels = new float[] {0.5f, 2f};
            Settings.IsAsync = true;

            using (var win = new RaytracerDemo(new Size(1024, 512))) {
                win.Run(30.0, 60.0);
            }
        }

        public RaytracerDemo(Size size) : base(size) { }

        public override void Init()
        {
            base.Init();
            World = SpheresScene.CreateSphereScene();
        }

        public override void Update()
        {
            base.Update();
            
            if(!Focused) return;
            
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Z) && SceneID != SpheresScene.SceneID) {
                MotherBee.Cancel();
                SceneID = SpheresScene.SceneID;
                World = SpheresScene.CreateSphereScene();
            } else if (keyState.IsKeyDown(Key.X) && SceneID != TestScene.SceneID) {
                MotherBee.Cancel();
                SceneID = TestScene.SceneID;
                World = TestScene.CreateTestScene();
            } // TODO:  add some more scenes
        }
    }
}