using System;
using OpenTK;
using Object = Engine.Objects.Object;

namespace RaytraceEngine.Objects
{
    public interface ITraceable
    {
        bool CheckHit(Ray ray, ref RayHit hit);
        void Init();
    }
    
    public abstract class Primitive : Object, ITraceable
    {
        public Material Material { get; set; } = new Material();
        public abstract bool CheckHit(Ray ray, ref RayHit hit);
        public abstract Vector2 GetUV(RayHit hit);
    }
}