using System;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer.Objects
{
    public class Sphere : Primitive
    {
        struct SphereCache
        {
            public float radius_sq;
            public float radius_inv;
        }

        private SphereCache cache;
        
        private float radius = 1;

        public float Radius
        {
            get { return radius; }

            set
            {
                radius = value;
                position_cached = false;
            }
        }

        public Sphere(Vector3 position, float radius, Quaternion rotation) : base(position, rotation, true)
        {
            Radius = radius;
        }

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            var L = Position - ray.Origin;
            float tca = Vector3.Dot(L, ray.Direction);
            if(tca < 0) return false;

            var q = L - tca * ray.Direction;
            float d2 = Vector3.Dot(q, q);
            if(d2 > cache.radius_sq) return false;

            var t = tca - ray.Outside * (float)Math.Sqrt(cache.radius_sq - d2);
            if(t < 0 || t > hit.T) return false; // Check if is relevant hit

            hit.T = t;
            hit.Position = ray.Origin + ray.Direction * t;
            hit.Normal = (hit.Position - Position) / radius * ray.Outside;
            hit.Obj = this;
            return true;
        }

        public override Vector2 GetUV(RayHit hit)
        {
            var u = .5f + (float)Math.Atan2(hit.Normal.Z, hit.Normal.X);
            var v = .5f - (float)Math.Asin(hit.Normal.Y)/(float)Math.PI;
            return new Vector2(u, v);
        }

        public override void Cache()
        {
            if(position_cached) return;
            cache.radius_sq = radius * radius;
            cache.radius_inv = 1/radius;
            position_cached = true;
        }
    }
}