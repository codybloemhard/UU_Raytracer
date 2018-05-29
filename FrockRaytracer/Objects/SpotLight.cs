using System;
using OpenTK;
using FrockRaytracer.Utils;

namespace FrockRaytracer.Objects
{
    public class SpotLight : PointLight
    {
        private Vector3 normal;
        public Vector3 Normal { get { return -normal; } set { normal = -value; } }
        public float AngleMin = 45f;
        public float AngleMax = 55f;
        //No energy outside the range and fade to it from inner range
        public override float AngleEnergy(Vector3 toLight)
        {
            float ang = (float)Math.Acos(Vector3.Dot(toLight, normal)) * Constants.RAD_TO_DEG;
            if (ang <= AngleMin) return 1f;
            if (ang > AngleMax) return 0f;
            return (AngleMax - ang) / (AngleMax - AngleMin);
        }

        public SpotLight(Vector3 position, Vector3 colour, float intensity)
            : base(position, colour, intensity) { }
    }

    public class SpotLightMultiSample : SphereAreaLight
    {
        private Vector3 normal;
        public Vector3 Normal { get { return -normal; } set { normal = -value; } }
        public float AngleMin = 45f;
        public float AngleMax = 55f;

        public SpotLightMultiSample(Vector3 position, Vector3 colour, float intensity, int uniqSamples = 256)
            : base(position, colour, intensity, uniqSamples)
        {
            int a = 0;
            int steps = (int)Math.Sqrt(uniqSamples);
            for (int i = 0; i < steps; i++)
                for (int j = 0; j < steps; j++)
                    allPoints[a++] = Position + RRandom.RndUnitStratified(steps, i, j) * Radius;
        }

        public override float AngleEnergy(Vector3 toLight)
        {
            float ang = (float)Math.Acos(Vector3.Dot(toLight, normal)) * Constants.RAD_TO_DEG;
            if (ang <= AngleMin) return 1f;
            if (ang > AngleMax) return 0f;
            return (AngleMax - ang) / (AngleMax - AngleMin);
        }
    }
}