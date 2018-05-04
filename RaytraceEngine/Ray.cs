using OpenTK;
using RaytraceEngine.Objects;

namespace RaytraceEngine
{
    public struct Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;
    }

    public struct RayHit
    {
        public Vector3 Position;
        public Vector3 Normal;
        public float Distance;
        public Material Material;
    }
}