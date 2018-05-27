using System;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer.Objects
{
    public class Plane : Primitive
    {
        private Vector3 normal;
        private Vector3 u, v;
        
        public Vector3 Normal
        {
            get => normal;
            set
            {
                normal = value;
                position_cached = false;
            }
        }

        public Plane(Vector3 position, Quaternion rotation) : base(position, rotation, false) { }

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            float divisor = Vector3.Dot(ray.Direction, normal);
            if (Math.Abs(divisor) < Constants.EPSILON) return false;

            var plane_vec = Position - ray.Origin;
            float t = Vector3.Dot(plane_vec, normal) / divisor;
            if (t < Constants.EPSILON || t > hit.T) return false; // Check if is relevant hit

            hit.Normal = normal;
            hit.Position = ray.Origin + ray.Direction * t;
            hit.Obj = this;
            hit.T = t;
            return true;
        }

        public override Vector2 GetUV(RayHit hit)
        {
            return new Vector2(Vector3.Dot(hit.Position, u), Vector3.Dot(hit.Position, v));
        }

        public override void Cache()
        {
            if (position_cached) return;
            normal = Rotation * Vector3.UnitY;
            
            u = new Vector3(Normal.Y, Normal.Z, -Normal.X);
            v = Vector3.Cross(u, Normal);
            u.Normalize();
            v.Normalize();

            position_cached = true;
        }
    }
}