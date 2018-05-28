using OpenTK;

namespace FrockRaytracer.Objects
{
    public enum LightSampleMode
    {
        FAKE, RANDOM_PRECALC_STRATIFIED, TRUE_STRATIFIED
    }

    public struct LightCache
    {
        public float IntensitySq;
    }

    public abstract class Light : Object
    {
        public Vector3 Colour { get; set; }
        public LightCache LightCache;
        private float intensity;
        public float Intensity
        {
            get { return intensity; }
            set
            {
                intensity = value;
                LightCache.IntensitySq = intensity * intensity;
            }
        }

        public abstract Vector3[] GetPoints(uint maxSamples, LightSampleMode mode);

        public abstract float AngleEnergy(Vector3 toLight);

        public Light(Vector3 pos, Quaternion rot) : base(pos, rot) { }
    }

    public class PointLight : Light
    {
        public override Vector3[] GetPoints(uint maxSamples, LightSampleMode mode)
        {
            return new Vector3[] { Position };
        }

        public override float AngleEnergy(Vector3 toLight)
        {
            return 1f;
        }

        public PointLight(Vector3 position, Vector3 colour, float intensity) : base(position, Quaternion.Identity)
        {
            Colour = colour;
            Intensity = intensity;
        }
    }
}