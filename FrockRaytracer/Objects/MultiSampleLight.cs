using System;
using OpenTK;
using FrockRaytracer.Utils;

namespace FrockRaytracer.Objects
{
    public abstract class MultiSampleLight : PointLight
    {
        protected int uniqSamples = 128;
        protected Vector3[] allPoints;

        protected MultiSampleLight(Vector3 pos, Vector3 col, float intens, int uniqSamples) : base(pos, col, intens)
        {
            this.uniqSamples = uniqSamples;
            allPoints = new Vector3[uniqSamples];
        }

        public abstract Vector3[] TrueStratified(uint max);

        public override Vector3[] GetPoints(uint maxSamples, LightSampleMode lsm)
        {
            int size = 1 + (int)Math.Abs(maxSamples - 1);
            Vector3[] res = new Vector3[size];
            res[size - 1] = Position;
            if (lsm == LightSampleMode.TRUE_STRATIFIED)
                return TrueStratified(maxSamples);
            else if (lsm == LightSampleMode.RANDOM_PRECALC_STRATIFIED)
                for (int i = 0; i < size - 1; i++)
                    res[i] = allPoints[RRandom.ThreadLocalRandom.Instance.Next(0, uniqSamples - 1)];
            else
                for (int i = 0; i < size - 1; i++)
                    res[i] = allPoints[i % allPoints.Length];
            return res;
        }
    }

    public class SphereAreaLight : MultiSampleLight
    {
        public float Radius { get; set; }

        public SphereAreaLight(Vector3 pos, Vector3 col, float intens, int uniqSamples = 256) : base(pos, col, intens, uniqSamples)
        {
            int a = 0;
            int steps = (int)Math.Sqrt(uniqSamples);
            for (int i = 0; i < steps; i++)
                for (int j = 0; j < steps; j++)
                    allPoints[a++] = Position + RRandom.RndUnitStratified(steps, i, j) * Radius;
        }

        public override Vector3[] TrueStratified(uint max)
        {
            int a = 0;
            int steps = (int)Math.Sqrt(max);
            Vector3[] arr = new Vector3[steps * steps];
            for (int i = 0; i < steps; i++)
                for (int j = 0; j < steps; j++)
                    arr[a++] = Position + RRandom.RndUnitStratified(steps, i, j) * Radius;
            return arr;
        }
    }
}