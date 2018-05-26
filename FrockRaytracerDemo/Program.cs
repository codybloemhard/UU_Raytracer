using System.Drawing;
using FrockRaytracer;

namespace FrockRaytracerDemo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var win = new Window(new Size(1024, 512))) { win.Run(30.0, 60.0); }
        }
    }
}