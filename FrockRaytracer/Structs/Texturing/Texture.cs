using System;
using FrockRaytracer.Utils;
using OpenTK;

namespace FrockRaytracer.Structs
{
    public enum BoxFace
    {
        TOP = Vectors.Axis.Y,
        BOTTOM = Vectors.Axis.Y | 4,
        FRONT = Vectors.Axis.Z ,
        BACK = Vectors.Axis.Z| 4,
        LEFT = Vectors.Axis.X  | 4,
        RIGHT = Vectors.Axis.X
    }

    public interface ICubeMap
    {
        Vector3 GetColor(BoxFace face, Vector2 uv);
    }

    public interface Texture
    {
        Vector3 GetColor(Vector2 uv);
    }

    public abstract class SizedTexture : Texture
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public abstract Vector3 GetPixel(int x, int y);
        
        public abstract Vector3 GetColor(Vector2 uv);
    }

    public class DiffuseTexture : SizedTexture
    {
        private Raster raster;
        private float texScale = 1f;
        public float TextureScale
        {
            get { return texScale; }
            set { texScale = 1f / value; }
        }

        public DiffuseTexture(string fileName, float scale = 1f) : base()
        {
            raster = new Raster(fileName);
            Width = raster.Width;
            Height = raster.Height;
            TextureScale = scale;
        }

        public override Vector3 GetColor(Vector2 uv)
        {
            uv *= texScale;
            uv.X = uv.X - (float)Math.Truncate(uv.X);
            uv.Y = uv.Y - (float)Math.Truncate(uv.Y);
            if (uv.X < 0) uv.X = 1f + uv.X;
            if (uv.Y < 0) uv.Y = 1f + uv.Y;
            if (uv.X > 0.999f) uv.X = 0.999f;
            if (uv.Y > 0.999f) uv.Y = 0.999f;
            int xx = (int)(Width * uv.X);
            int yy = (int)(Height * uv.Y);
            int offset = (xx + yy * Width) * 3;
            byte r = raster.Pixels[offset++];
            byte g = raster.Pixels[offset++];
            byte b = raster.Pixels[offset];
            return new Vector3(b, g, r) / 255f;
        }

        public override Vector3 GetPixel(int x, int y)
        {
            int offset = (x + y * Width) * 3;
            byte r = raster.Pixels[offset++];
            byte g = raster.Pixels[offset++];
            byte b = raster.Pixels[offset];
            return new Vector3(r, g, b);
        }
    }
}