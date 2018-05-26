using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenTK;

namespace FrockRaytracer
{
    public struct Raster
    {
        public byte[] Pixels;
        public int Width;
        public int Height;
        public  bool Debug;

        /// <summary>
        /// Create raster from a size
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Raster(int width, int height) : this()
        {
            this.Width = width;
            this.Height = height;
            Pixels = new byte[width * height * 3];
            Debug = false;
        }

        /// <summary>
        /// Create raster form image
        /// </summary>
        /// <param name="filename"></param>
        public Raster(string filename)
        {
            Bitmap bmp = new Bitmap(filename);
            Width = bmp.Width;
            Height = bmp.Height;
            Pixels = new byte[Width * Height * 3];
            Debug = false;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            Marshal.Copy(data.Scan0, Pixels, 0, Width * Height * 3);
            bmp.UnlockBits(data);
        }

        /// <summary>
        /// Set color from rgb bytes. We also check if we are inside the raster :P
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public void setPixel(int x, int y, byte r, byte g, byte b)
        {
            // If debug is enabled we dont draw on the raytracer side of the window
            if (Debug && x < Window.RAYTRACE_AREA_WIDTH) return;
            if (x < 0 || y < 0 || x >= Width || y >= Height) return;
            int offset = (x + y * Width) * 3;
            Pixels[offset++] = r;
            Pixels[offset++] = g;
            Pixels[offset] = b;
        }

        /// <summary>
        /// Set color from a vector. Colors get clamped
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void setPixel(int x, int y, Vector3 color)
        {
            setPixel(x, y, (byte) (Math.Min(1f, color.X) * 0xFF), (byte) (Math.Min(1, color.Y) * 0xFF),
                (byte) (Math.Min(1, color.Z) * 0xFF));
        }

        /// <summary>
        /// Clear the Pixels by setting by using memset. A handy c function which is proven to be fast.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Pixels.Length; ++i) Pixels[i] = 0;
        }
    }
}