using FrockRaytracer.Objects.Primitives;
using OpenTK;

namespace FrockRaytracer.Structs
{
    public struct Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;
        public short Outside;
        public uint Depth, Refldepth;

        public Ray(Vector3 origin, Vector3 direction, short outside, uint depth, uint rdepth = 999)
        {
            Origin = origin;
            Direction = direction;
            Outside = outside;
            Depth = depth;
            Refldepth = rdepth;
            if (Refldepth == 999) Refldepth = Depth;
        }


        public Ray(Vector3 origin, Vector3 direction) : this()
        {
            Origin = origin;
            Direction = direction;
            Outside = 1;
            Depth = 1;
            Refldepth = 1;
        }

        public bool isOutside()
        {
            return Outside == 1;
        }
        
        public static Ray Default()
        {
            return new Ray(Vector3.Zero, Vector3.One, 1, 1);
        }
    }

    public struct RayHit {
        public Vector3 Position;
        public Vector3 Normal;
        public float T;
        public Primitive Obj;

        public RayHit(Vector3 position, Vector3 normal, float t, Primitive obj)
        {
            Position = position;
            Normal = normal;
            T = t;
            Obj = obj;
        }
        
        public static RayHit Default()
        {
            return new RayHit(Vector3.Zero, Vector3.One, Constants.BIG_NUMBER, null);
        }
    };
}