using System;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer.Objects
{
    public class Plane : Primitive
    {
        private Vector3 normal;

        public Vector3 Normal
        {
            get { return normal; }
            set { normal = value; }
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

        public override void Cache()
        {
            if (position_cached) return;
            normal = Rotation * Vector3.UnitY;

            position_cached = true;
        }
    }
}