using System;
using OpenTK;

namespace FrockRaytracer.Structs
{
    public class CheckerboardTexture : Texture
    {
        public float DownScaleX, DownScaleY;

        public CheckerboardTexture(float downScale = 1)
        {
            DownScaleX = DownScaleY = downScale;
        }

        public CheckerboardTexture(float downScaleX, float downScaleY)
        {
            DownScaleX = downScaleX;
            DownScaleY = downScaleY;
        }

        public Vector3 GetColor(Vector2 uv)
        {
            return (((int) Math.Round(uv.X * DownScaleX) + (int) Math.Round(uv.Y * DownScaleY)) & 1) == 0 ? Vector3.One : Vector3.Zero;
        }
    }
}