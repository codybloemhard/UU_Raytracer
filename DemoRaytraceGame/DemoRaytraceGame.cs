using System;
using Engine;
using Engine.Graphics;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using RaytraceEngine;
using OpenTK;

namespace DemoRaytraceGame
{
    public class DemoRaytraceGame : RaytraceGame
    {
        public static void Main(string[] args)
        {
            using (var win = new Engine.GameWindow(new DemoRaytraceGame())) { win.Run(30.0, 60.0); }
        }

        public override void Init()
        {
            base.Init();
            scene.cam = new RayCamera(Width, Height, 1f);
            Console.WriteLine(RMath.ToStr(scene.cam.FromPixel(Width / 2, Height / 2).dir));
        }
    }
}