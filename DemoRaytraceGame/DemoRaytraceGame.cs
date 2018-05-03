using Engine;
using Engine.Graphics;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using RaytraceEngine;

namespace DemoRaytraceGame
{
    public class DemoRaytraceGame : RaytraceGame
    {

        public static void Main(string[] args)
        {
            using (var win = new GameWindow(new DemoRaytraceGame())) { win.Run(30.0, 60.0); }
        }

        public override void Init()
        {
            base.Init();
        }
    }
}