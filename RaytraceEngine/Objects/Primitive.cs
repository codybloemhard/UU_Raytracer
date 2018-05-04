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
            hit.Normal = (Position - hit.Position).Normalized();
            return true;
        }
    }

    public class Plane : Primitive
    {
        public override bool CheckHit(Ray ray, out RayHit hit)
        {
            throw new System.NotImplementedException();
        }
    }
}