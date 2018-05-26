using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer.Objects
{
    public class Primitive : Object
    {
        public Material Material;
        protected bool is_volumetric;

        public bool IsVolumetric => is_volumetric;

        public Primitive(Vector3 position, Quaternion rotation, bool isVolumetric) : base(position, rotation)
        {
            Material = Material.Default() ;
            is_volumetric = isVolumetric;
        }

        public virtual bool Intersect(Ray ray, ref RayHit hit)
        {
            return false;
        } 
        
        /// <summary>
        /// Get UV coordinates to map texture to.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public virtual Vector2 GetUV(Vector3 pos) { return Vector2.Zero;}
    }
}