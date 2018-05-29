using OpenTK;

namespace FrockRaytracer.Objects
{
    //Used in the settings
    public enum LightSampleMode
    {
        FAKE, RANDOM_PRECALC_STRATIFIED, TRUE_STRATIFIED
    }

    public struct LightCache
    {
        public float IntensitySq;
    }
    //Basic light structure
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
        //Return multiple points for softshadows
        public abstract Vector3[] GetPoints(uint maxSamples, LightSampleMode mode);
        //Energy based on angle
        public abstract float AngleEnergy(Vector3 toLight);

        public Light(Vector3 pos, Quaternion rot) : base(pos, rot) { }
    }

    public class PointLight : Light
    {
        //just one point
        public override Vector3[] GetPoints(uint maxSamples, LightSampleMode mode)
        {
            return new Vector3[] { Position };
        }
        //Light to all directions
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