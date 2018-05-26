using OpenTK;
using RaytraceEngine.Objects;

namespace RaytraceEngine
{
    public struct Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;
        public short outside;
        public uint depth;

        public Ray(Vector3 origin, Vector3 direction, short outside = 1, uint depth = 1)
        {
            Origin = origin;
            Direction = direction;
            this.outside = outside;
            this.depth = depth;
        }

        bool is_outside()
        {
            return outside == 1;
        }
    }

    public class RayHit
    {
        public Vector3 Position;
        public Vector3 Normal;
        public float Distance;
        public Primitive HitObject;

        public RayHit()
        {
            Position = Vector3.Zero;
            Normal = Vector3.Zero;
            Distance = 999999;
            HitObject = null;
        }

        public RayHit(Vector3 position, Vector3 normal, float length = 999999,
            Primitive hitObject = null)
        {
            Position = position;
            Normal = normal;
            Distance = length;
            HitObject = hitObject;
        }
    }
}