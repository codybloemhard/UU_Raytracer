using System;
using OpenTK;

namespace RaytraceEngine.Objects
{
    public class Plane : Primitive
    {
        private Quaternion rotation;
        private Vector3 u, v;
        private Vector3 normal;
        
        public override Quaternion Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                Normal = Vector3.UnitY * Matrix3.CreateFromQuaternion(Rotation);
            } 
        }

        public Vector3 Normal
        {
            get => normal;
            set
            {
                normal = value; 
                u = new Vector3(Normal.Y, Normal.Z, -Normal.X);
                v = Vector3.Cross(u, Normal);
                u.Normalize();
                v.Normalize();
            }
        }

        public override bool CheckHit(Ray ray, ref RayHit hit)
        {
            float divisor = Vector3.Dot(ray.Direction, normal);
            if(Math.Abs(divisor) < Constants.EPSILON) return false;

            var plane_vec = Position - ray.Origin;
            float t = Vector3.Dot(plane_vec, normal) / divisor;
            if(t < Constants.EPSILON || t > hit.Length) return false;

            hit.Normal = normal;
            hit.Position = ray.Origin + ray.Direction * t;
            hit.HitObject = this;
            hit.Length = t;
            return true;
        }
        
        public override Vector2 GetUV(RayHit hit)
        {
            return new Vector2(Vector3.Dot(hit.Position, u), Vector3.Dot(hit.Position, v));
        }
    }
}