using System;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer.Objects.Primitives
{
    public class Polygon : Primitive
    {
        private Vector3 normal, up, right;

        public Vector3 Normal
        {
            get { return normal; }
            set {
                //up and right are two orthonormal vectors on the plane of the polygon, and are used when intersecting it
                normal = value.Normalized();
                up = Vector3.Cross(normal, Vector3.UnitX).Normalized();
                right = Vector3.Cross(normal, up).Normalized();
            }
        }

        private Vector2[] Vertices;
        private Vector3[] originalVertices;

        public Polygon(Vector3 position, Vector3[] verts) : base(position, Quaternion.Identity, false)
        {
            //Vertices are specified in 3d coordinates, and are translated to a plane in 3d space (with a normal and a position),
            //and 2d coordinates on that plane using the vectors up and right
            Vertices = new Vector2[verts.Length];
            Normal = Vector3.Cross(verts[1] - verts[0], verts[2] - verts[0]);
            for(int i = 0; i < verts.Length; i++)
            {
                Vertices[i] = new Vector2(Vector3.Dot(verts[i] - Position, right), Vector3.Dot(verts[i] - Position, up));
            }
            originalVertices = verts;
        }

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            float divisor = Vector3.Dot(ray.Direction, normal);
            if (Math.Abs(divisor) < Constants.EPSILON) return false;

            var plane_vec = Position - ray.Origin;
            float t = Vector3.Dot(plane_vec, normal) / divisor;
            if (t < Constants.EPSILON || t > hit.T) return false; // Check if is relevant hit

            bool l = true, r = true;
            for(int i = 0; i < Vertices.Length; i++) //Check if the hit is on the left of every edge
            {
                Vector3 v = ray.Origin + ray.Direction * t - Position;
                Vector2 hitPoint = new Vector2(Vector3.Dot(v, right), Vector3.Dot(v, up));
                if (isLeft(Vertices[i], Vertices[((i + 1) % Vertices.Length)], hitPoint)) r = false;
                if (!(isLeft(Vertices[i], Vertices[((i + 1) % Vertices.Length)], hitPoint))) l = false;
            }
            if (!l && !r) return false;

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

        public override AABB GetBox()
        {
            Vector3 mn = new Vector3(float.MaxValue), mx = new Vector3(-float.MaxValue);
            for(int i = 0; i < originalVertices.Length; i++)
            {
                mn.X = Math.Min(mn.X, originalVertices[i].X);
                mn.Y = Math.Min(mn.Y, originalVertices[i].Y);
                mn.Z = Math.Min(mn.Z, originalVertices[i].Z);
                mx.X = Math.Max(mx.X, originalVertices[i].X);
                mx.Y = Math.Max(mx.Y, originalVertices[i].Y);
                mx.Z = Math.Max(mx.Z, originalVertices[i].Z);
            }
            return new AABB(Vector3.Zero, Quaternion.Identity) { min = mn, max = mx };
        }
    }
}