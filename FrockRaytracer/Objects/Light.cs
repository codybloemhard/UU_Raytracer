using OpenTK;

namespace FrockRaytracer.Objects
{
    
    public struct PointLightCache
    {
        public float IntensitySq;
    }
    
    public class Light : Object
    {
        private float intensity;

        public Vector3 Color;
        public PointLightCache PointLightCache;
        public float Intensity
        {
            get => intensity;
            set
            {
                intensity = value;
                PointLightCache.IntensitySq = intensity * intensity;
            }
        }

        public Light(Vector3 position, Vector3 color, float intensity) : base(position, Quaternion.Identity)
        {
            Color = color;
            Intensity = intensity;
        }
    }
}