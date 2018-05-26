using System;
using OpenTK;
using Engine.TemplateCode;

namespace RaytraceEngine.Objects
{
    public class Material
    {
        public Vector3 Diffuse = Vector3.One;
        public Vector3 Specular = Vector3.One;
        public Vector3 Absorb = Vector3.One;
        public bool Glossy = false;
        public float Shinyness = 3;
        public float Roughness = 3;
        public bool IsDielectic = false;
        public bool IsMirror = false;
        public float Reflectivity = 0;
        public bool IsRefractive = false;
        public float RefractionIndex = Constants.LIGHT_IOR;
        
        public Surface Texture = null;
        private float texScale = 1f;
        public float TextureScale
        {
            get { return texScale; }
            set { texScale = 1f / value; }
        }

        public Vector3 TexColour(Vector2 uv)
        {
            uv *= texScale;
            uv.X = uv.X - (float)Math.Truncate(uv.X);
            uv.Y = uv.Y - (float)Math.Truncate(uv.Y);
            if (uv.X < 0) uv.X = 1f + uv.X;
            if (uv.Y < 0) uv.Y = 1f + uv.Y;
            if (uv.X > 0.999f) uv.X = 0.999f;
            if (uv.Y > 0.999f) uv.Y = 0.999f;
            int xx = (int)(Texture.width * uv.X);
            int yy = (int)(Texture.height * uv.Y);
            int c = Texture.pixels[xx * Texture.height + yy];
            return RMath.ToFloatColour(c);
        }
    }
}