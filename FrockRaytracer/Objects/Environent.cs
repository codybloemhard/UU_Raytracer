using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer.Objects
{
    public abstract class Environent
    {
        public abstract Vector3 GetColor(Ray ray);
    }
}