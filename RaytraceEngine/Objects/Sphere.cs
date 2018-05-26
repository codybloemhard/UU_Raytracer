using System;
using OpenTK;

namespace RaytraceEngine.Objects
{
    public class Sphere : Primitive
    {
        private float radius, radius_sq, radius_inv;

        public float Radius
        {
            get => radius;
            set
            {
                radius = value;
                radius_sq = radius * radius;
                radius_inv = 1 / radius;
            }
        }

        public override bool CheckHit(Ray ray, ref RayHit hit)
        {
            var L = Position - ray.Origin;
            float tca = Vector3.Dot(L, ray.Direction);
            if(tca < 0) return false;

            var q = L - tca * ray.Direction;
            float d2 = Vector3.Dot(q, q);
            if(d2 > radius_sq) return false;

            var t = tca - ray.outside * (float)Math.Sqrt(radius_sq - d2);
            if(t < 0 || t > hit.Length) return false;

            hit.Length = t;
            hit.Position = ray.Origin + ray.Direction * t;
            hit.Normal = (hit.Position - Position) / radius * ray.outside;
            hit.HitObject = this;
            return true;
        }

        public override Vector2 GetUV(RayHit hit)
        {
            throw new NotImplementedException();
        }
    }
}