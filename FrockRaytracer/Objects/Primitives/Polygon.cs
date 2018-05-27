using System;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer.Objects
{
    public class Polygon : Primitive
    {
        private Vector3 normal, up, right;

        public Vector3 Normal
        {
            get { return normal; }
            set {
                normal = value.Normalized();
                up = Vector3.Cross(normal, Vector3.UnitX).Normalized();
                right = Vector3.Cross(normal, up).Normalized();
            }
        }

        private Vector2[] Vertices;

        public Polygon(Vector3 position, Vector3[] verts) : base(position, Quaternion.Identity, false)
        {
            Vertices = new Vector2[verts.Length];
            Normal = Vector3.Cross(verts[1] - verts[0], verts[2] - verts[0]);
            for(int i = 0; i < verts.Length; i++)
            {
                Vertices[i] = new Vector2(Vector3.Dot(verts[i] - Position, right), Vector3.Dot(verts[i] - Position, up));
            }
        }

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            float divisor = Vector3.Dot(ray.Direction, normal);
            if (Math.Abs(divisor) < Constants.EPSILON) return false;

            var plane_vec = Position - ray.Origin;
            float t = Vector3.Dot(plane_vec, normal) / divisor;
            if (t < Constants.EPSILON || t > hit.T) return false; // Check if is relevant hit

            for(int i = 0; i < Vertices.Length; i++) //Check if the hit is on the left of every edge
            {
                Vector3 v = ray.Origin + ray.Direction * t - Position;
                Vector2 hitPoint = new Vector2(Vector3.Dot(v, right), Vector3.Dot(v, up));
                if (isLeft(Vertices[i], Vertices[((i + 1) % Vertices.Length)], hitPoint)) return false;
            }

            if (Vector3.Dot(Normal, ray.Direction) < 0)
                hit.Normal = Normal;
            else hit.Normal = -Normal;
            hit.Position = ray.Origin + ray.Direction * t;
            hit.Obj = this;
            hit.T = t;
            return true;
        }

        private bool isLeft(Vector2 vertice1, Vector2 vertice2, Vector2 point)
        {
            return ((vertice2.X - vertice1.X) * (point.Y - vertice1.Y) - (vertice2.Y - vertice1.Y) * (point.X - vertice1.X)) > 0;
        }
    }
}