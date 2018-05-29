using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer.Objects
{
    public class Environent
    {
        public Vector3 AmbientLight { get; set; } = Vector3.Zero;
        public Vector3 AmbientColor { get; set; } = Vector3.Zero;

        public virtual Vector3 GetColor(Ray ray)
        {
            return AmbientColor;
        }
    }
}