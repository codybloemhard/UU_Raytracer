using System;
using Engine;
using Engine.Graphics;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using RaytraceEngine;
using OpenTK;
using RaytraceEngine.Objects;

namespace DemoRaytraceGame
{
    public class DemoRaytraceGame : RaytraceGame
    {
        private float t;

        public static void Main(string[] args)
        {
            using (var win = new Engine.GameWindow(new DemoRaytraceGame())) { win.Run(30.0, 60.0); }
        }

        public override void Init()
        {
            base.Init();
            scene.cam = new RayCamera(Width, Height, 1f);
            scene.primitives.Add(new Sphere(new Vector3(0, 1, 6), 1f));
        }

        public override void Update()
        {
            base.Update();
            t += 0.01f;
            scene.cam.SetPos(Vector3.Zero, new Vector3(0, t, 0));
        }
    }
}