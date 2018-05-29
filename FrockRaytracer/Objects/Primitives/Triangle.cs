using System;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer.Objects.Primitives
{
    public class Triangle : Primitive
    {
        public Triangle() : base(new Vector3(0), Quaternion.Identity, false) { }

        private Vector3 Normal;
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
            Vector3 AB, CB;
            AB = vertices[1] - vertices[0];
            CB = vertices[1] - vertices[2];
            Normal = Vector3.Cross(AB, CB).Normalized();
        }

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            //Based on source: http://www.lighthouse3d.com/tutorials/maths/ray-triangle-intersection/
            Vector3 edge1, edge2, h, s, q;
            float a, f, u, v;
            edge1 = vertices[1] - vertices[0];
            edge2 = vertices[2] - vertices[0];
            h = Vector3.Cross(ray.Direction, edge2);
            a = Vector3.Dot(edge1, h);
            if (a > -Constants.EPSILON && a < Constants.EPSILON) return false;
            f = 1 / a;
            s = ray.Origin - vertices[0];
            u = f * Vector3.Dot(s, h);
            if (u < 0 || u > 1) return false;
            q = Vector3.Cross(s, edge1);
            v = f * Vector3.Dot(ray.Direction, q);
            if (v < 0 || v > 1) return false;

            if (u + v > 1) return false;

            float t = f * Vector3.Dot(edge2, q);
            if (t > Constants.EPSILON && t < hit.T)
            {
                hit.Position = ray.Origin + ray.Direction * t;
                hit.T = t;
                if (Vector3.Dot(Normal, ray.Direction) < 0)
                    hit.Normal = Normal;
                else hit.Normal = -Normal;
                hit.Obj = this;
                return true;
            }
            return false;
        }
    }
}
