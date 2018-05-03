using Engine.Objects;
using OpenTK;
using System;

namespace RaytraceEngine.Objects
{
    public interface ITraceable
    {
        bool CheckHit(Ray ray, out RayHit hit);
    }
    
    public abstract class Primitive : ITraceable
    {
        public abstract bool CheckHit(Ray ray, out RayHit hit);
    }

    public class Sphere : Primitive
    {
        public Vector3 pos;
        public float r;

        public Sphere(Vector3 pos, float r)
        {
            this.pos = pos;
            this.r = r;
        }

        public override bool CheckHit(Ray ray, out RayHit hit)
        {
            hit = new RayHit();
            Vector3 c = pos - ray.origin;
            float t = RMath.dot(c, ray.dir);
            Vector3 q = c - t * ray.dir;
            float p2 = RMath.dot(q, q);
            if (p2 > r * r) return false;
            t -= (float)Math.Sqrt(r*r - p2);
            if (t < 0) return false;
            hit.pos = ray.origin + ray.dir * t;
            hit.dist = t;
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