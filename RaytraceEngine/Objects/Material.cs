using System;
using OpenTK;
using Engine.TemplateCode;

namespace RaytraceEngine.Objects
{
    public class Material
    {
        public Vector3 Colour { get; set; }
        public float Roughness { get; set; }
        public float Reflectivity { get; set; }
        public float Refractivity { get; set; }
        public Surface Texture = null;
        private float texScale = 1f;
        public float TextureScale
        {
            get { return texScale; }
            set { texScale = 1f / value; }
        }

        public Material(Vector3 colour, float roughness, float reflectivity, float refractivity)
        {
            Colour = colour;
            Roughness = roughness;
            Reflectivity = reflectivity;
            Refractivity = refractivity;
        }

        public Vector3 TexColour(Vector2 uv)
        {
            uv *= texScale;
            uv.X = uv.X - (float)Math.Truncate(uv.X);
            uv.Y = uv.Y - (float)Math.Truncate(uv.Y);
            if (uv.X < 0) uv.X = 1f + uv.X;
            if (uv.Y < 0) uv.Y = 1f + uv.Y;
            int xx = (int)(Texture.width * uv.X);
            int yy = (int)(Texture.height * uv.Y);
            int c = Texture.pixels[xx * Texture.height + yy];
            float B = (byte)(c & 0x000000FF);
            float G = (byte)((c & 0x0000FF00) >> 8);
            float R = (byte)((c & 0x00FF0000) >> 16);
            return new Vector3(R / 255f, G / 255f, B / 255f);
        }
    }
}