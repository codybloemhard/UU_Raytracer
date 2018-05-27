using System;
using ImageMagick;
using OpenTK;

namespace FrockRaytracer.Structs
{
    public class HDRTexture : SizedTexture
    {
        public float[] Pixels;
        public float GammaCorrect;
        
        public HDRTexture(string imagePath, float gammaCorrect = 66878)
        {
            GammaCorrect = gammaCorrect;
            var img = new MagickImage(imagePath);
            Width = img.Width;
            Height = img.Height;
            Pixels = img.GetPixels().ToArray();
        }

        public override Vector3 GetPixel(int x, int y)
        {
            var offset = (x + y * Width)*3;
            return new Vector3(Pixels[offset++], Pixels[offset++], Pixels[offset]) / GammaCorrect;
        }

        public override Vector3 GetColor(Vector2 uv)
        {
            return GetPixel((int) Math.Min(uv.X * Width, Width - 1), (int) Math.Min(uv.Y * Height, Height - 1));
        }
    }
}