using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer.Objects.Primitives
{
    public class AABB : Primitive
    {
        public Vector3 min, max;
        public AABB(Vector3 position, Quaternion rotation) : base(position, rotation, true){ }

        //source: powerpoint of lecture 'acceleration'
        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            float tx1, tx2, ty1, ty2, tz1, tz2, tmin, tmax;

            tx1 = (min.X - ray.Origin.X) / ray.Direction.X;
            tx2 = (max.X - ray.Origin.X) / ray.Direction.X;
            tmin = Math.Min(tx1, tx2);
            tmax = Math.Max(tx1, tx2);

            ty1 = (min.Y - ray.Origin.Y) / ray.Direction.Y;
            ty2 = (max.Y - ray.Origin.Y) / ray.Direction.Y;
            tmin = Math.Max(tmin, Math.Min(ty1, ty2));
            tmax = Math.Min(tmax, Math.Max(ty1, ty2));

            tz1 = (min.Z - ray.Origin.Z) / ray.Direction.Z;
            tz2 = (max.Z - ray.Origin.Z) / ray.Direction.Z;
            tmin = Math.Max(tmin, Math.Min(tz1, tz2));
            tmax = Math.Min(tmax, Math.Max(tz1, tz2));

            if (tmax < tmin || tmax < 0 || tmax > hit.T) return false; // Check if is relevant hit

            hit.T = tmax;
            hit.Position = ray.Origin * tmax;
            hit.Normal = Vector3.UnitY;
            hit.Obj = this;
            return true;
        }
    }
}
