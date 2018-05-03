using OpenTK;
using RaytraceEngine.Objects;

namespace RaytraceEngine
{
    public struct Ray
    {
        public Vector3 origin;
        public Vector3 dir;
    }

    public struct RayHit
    {
        public Vector3 pos;
        public Vector3 normal;
        public float dist;
        public ITraceable obj;
    }
}