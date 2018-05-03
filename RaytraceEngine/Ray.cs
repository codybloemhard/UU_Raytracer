using OpenTK;
using RaytraceEngine.Objects;

namespace RaytraceEngine
{
    public struct Ray
    {
        public Vector3 Point;
        public Vector3 Direction;
    }

    public struct RayHit
    {
        public Vector3 Point;
        public Vector3 Normal;
        public ITraceable Object;
    }
}