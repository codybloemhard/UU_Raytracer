using FrockRaytracer.Structs;

namespace FrockRaytracer.Utils
{
    /// <summary>
    /// A very diverse kind of ray.
    /// </summary>
    public static class RayTrans
    {
        public static Ray Reflect(Ray ray, RayHit hit) {
            var dir = QuickMaths.Reflect(ray.Direction, hit.Normal);
            return new Ray(hit.Position + dir * Constants.EPSILON, dir, ray.Outside, ray.Depth + 1);
        }

        public static Ray Refract(Ray ray, RayHit hit) {
            float eta = ray.isOutside()
                ? hit.Obj.Material.RefractionIndex / Constants.LIGHT_IOR
                : Constants.LIGHT_IOR / hit.Obj.Material.RefractionIndex; // TODO: cache both

            var dir = QuickMaths.Refract(ray.Direction, hit.Normal, eta);
            short outside = (short) (hit.Obj.IsVolumetric ? (ray.isOutside() ? -1 : 1) : 1);
            return new Ray(hit.Position + dir * Constants.EPSILON, dir, outside, ray.Depth + 1);
        }
    }
}