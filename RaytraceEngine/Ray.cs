using OpenTK;
using RaytraceEngine.Objects;

namespace RaytraceEngine
{
    public struct Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }
    
    public struct RayHit
    {
        public Vector3 Position;
        public Vector3 Normal;
        public float Distance;
        public Primitive HitObject;

        public RayHit(Vector3 position, Vector3 normal, float distance, Primitive hitObject)
        {
            Position = position;
            Normal = normal;
            Distance = distance;
            HitObject = hitObject;
        }
    }
}