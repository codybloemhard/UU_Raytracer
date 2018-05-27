using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer.Objects
{
    public abstract class Environent
    {
        public Vector3 AmbientLight { get; set; } = Vector3.Zero;

        public abstract Vector3 GetColor(Ray ray);
    }
}