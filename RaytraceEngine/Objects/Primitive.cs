using System;
using OpenTK;
using Object = Engine.Objects.Object;

namespace RaytraceEngine.Objects
{
    public interface ITraceable
    {
        bool CheckHit(Ray ray, out RayHit hit);
    }
    
    public abstract class Primitive : Object, ITraceable
    {
        public Material Material { get; set; }
        public abstract bool CheckHit(Ray ray, out RayHit hit);
        public abstract Vector2 GetUV(RayHit hit);
    }

    public class Sphere : Primitive
    {
        public float Radius { get; set; }
        
        public override bool CheckHit(Ray ray, out RayHit hit)
        {
            hit = new RayHit();
            Vector3 c = Position - ray.Origin;
            float t = Vector3.Dot(c, ray.Direction);
            Vector3 q = c - t * ray.Direction;
            float p2 = Vector3.Dot(q, q);
            float rsq = Radius * Radius;
            if (p2 > rsq) return false;
            t -= (float)Math.Sqrt(rsq - p2);
            if (t < 0) return false;
            hit.Position = ray.Origin + ray.Direction * t;
            hit.Distance = t;
            hit.Material = Material;
            hit.Normal = (hit.Position - Position).Normalized();
            hit.Object = this;
            return true;
        }

        public override Vector2 GetUV(RayHit hit)
        {
            throw new NotImplementedException();
        }
    }

    public class Plane : Primitive
    {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 u, v;

        public override Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                UpdateNormal();
            }
        }

        public override Quaternion Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                UpdateNormal();
            } 
        }

        public Vector3 Normal { get;  private set; }
        //uv axis:
        //http://www.flipcode.com/archives/Raytracing_Topics_Techniques-Part_6_Textures_Cameras_and_Speed.shtml
        public void UpdateNormal()
        {
            Normal = Vector3.UnitY * Matrix3.CreateFromQuaternion(Rotation);
            u = new Vector3(Normal.Y, Normal.Z, -Normal.X);
            v = Vector3.Cross(u, Normal);
            u.Normalize();
            v.Normalize();
        }

        public override bool CheckHit(Ray ray, out RayHit hit)
        {
            hit = new RayHit();
            float deler = Vector3.Dot(ray.Direction, Normal);
            if (!(Math.Abs(deler) > 0.0001f)) return false;
            
            Vector3 diff = Position - ray.Origin;
            float t = Vector3.Dot(diff, Normal) / deler;
            if (t < 0.0001f) return false;
            hit.Normal = Normal;
            hit.Position = ray.Origin + ray.Direction * t;
            hit.Material = Material;
            hit.Distance = t;
            hit.Object = this;
            return true;
        }

        public override Vector2 GetUV(RayHit hit)
        {
            return new Vector2(Vector3.Dot(hit.Position, u), Vector3.Dot(hit.Position, v));
        }
    }
}