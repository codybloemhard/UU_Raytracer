using System;
using OpenTK;
using Object = Engine.Objects.Object;

namespace RaytraceEngine.Objects
{
    public interface ITraceable
    {
        bool CheckHit(Ray ray, out RayHit hit);
    }

    public interface IVolumetricTraceable
    {
        bool CheckHitInside(Ray ray, out RayHit hit);
    }
    
    public abstract class Primitive : Object, ITraceable
    {
        public Material Material { get; set; }
        public abstract bool CheckHit(Ray ray, out RayHit hit);
        public abstract Vector2 GetUV(RayHit hit);
    }

    public class Sphere : Primitive, IVolumetricTraceable
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
            hit.HitObject = this;
            hit.Normal = (hit.Position - Position).Normalized();
            return true;
        }

        public override Vector2 GetUV(RayHit hit)
        {
            throw new NotImplementedException();
        }

        public bool CheckHitInside(Ray ray, out RayHit hit)
        {
            hit = new RayHit();
            var c = Position - ray.Origin;
            float t = Vector3.Dot(c, ray.Direction);
            var q = c - t * ray.Direction;
            float p2 = Vector3.Dot(q, q);
            float rsq = Radius * Radius;
            if (p2 > rsq) return false;
            t += (float)Math.Sqrt(rsq - p2);
            if (t < 0) return false;
            
            hit.Position = ray.Origin + ray.Direction * t;
            hit.Distance = t;
            hit.HitObject = this;
            hit.Normal = (Position - hit.Position).Normalized();
            return true;
        }
    }

    public class Plane : Primitive
    {
        private Quaternion rotation;
        private Vector3 u, v;
        
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

        public void UpdateNormal(Vector3 normal)
        {
            Normal = normal;
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
            hit.HitObject = this;
            hit.Distance = t;
            return true;
        }

        public override Vector2 GetUV(RayHit hit)
        {
            return new Vector2(Vector3.Dot(hit.Position, u), Vector3.Dot(hit.Position, v));
        }
    }

    public class Triangle : Primitive
    {
        public Vector3 Normal { get; private set; }
        private Vector3[] vertices = new Vector3[3];
        public Vector3[] Vertices
        {
            get { return vertices; }
            set
            {
                vertices = value;
                UpdateNormal();
            }
        }

        public void UpdateNormal()
        {
            Vector3 AB, BC;
            AB = vertices[0] - vertices[1];
            BC = vertices[1] - vertices[2];
            Normal = Vector3.Cross(AB, BC).Normalized();
        }

        public override bool CheckHit(Ray ray, out RayHit hit)
        {
            float eps = 0.000001f;
            hit = new RayHit();
            Vector3 edge1, edge2, h, s, q;
            float a, f, u, v;
            edge1 = vertices[1] - vertices[0];
            edge2 = vertices[2] - vertices[0];
            h = Vector3.Cross(ray.Direction, edge2);
            a = Vector3.Dot(edge1, h);
            if (a > -eps && a < eps) return false;
            f = 1 / a;
            s = ray.Origin - vertices[0];
            u = f * Vector3.Dot(s, h);
            if (u < 0 || u > 1) return false;
            q = Vector3.Cross(s, edge1);
            v = f * Vector3.Dot(ray.Direction, q);
            if (v < 0 || v > 1) return false;

            if (u + v > 1) return false;

            float t = f * Vector3.Dot(edge2, q);
            if(t > eps)
            {
                hit.Position = ray.Origin + ray.Direction * t;
                hit.Distance = t;
                if (Vector3.Dot(Normal, ray.Direction) < 0)
                    hit.Normal = Normal;
                else hit.Normal = -Normal;
                hit.HitObject = this;
                return true;
            }
            return false;
        }
        

        public override Vector2 GetUV(RayHit hit)
        {
            throw new NotImplementedException();
        }
    }
}