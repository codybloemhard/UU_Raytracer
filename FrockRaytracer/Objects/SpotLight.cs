using System;
using OpenTK;

namespace FrockRaytracer.Objects
{
    public class SpotLight : PointLight
    {
        private Vector3 normal;
        public Vector3 Normal { get { return -normal; } set { normal = -value; } }
        public float AngleMin = 45f;
        public float AngleMax = 55f;

        public override float AngleEnergy(Vector3 toLight)
        {
            float ang = (float)Math.Acos(Vector3.Dot(toLight, normal)) * Constants.RAD_TO_DEG;
            if (ang <= AngleMin) return 1f;
            if (ang > AngleMax) return 0f;
            return (AngleMax - ang) / (AngleMax - AngleMin);
        }

        public SpotLight(Vector3 position, Vector3 colour, float intensity) : base(position, colour, intensity) { }
    }
}