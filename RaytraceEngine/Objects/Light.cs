using Engine.Objects;
using OpenTK;
using System;

namespace RaytraceEngine.Objects
{
    public interface ILightSource
    {
        Vector3 Intensity { get; set; }

        Vector3[] GetPoints(uint maxSamples, bool rng);

        float AnlgeEnergy(Vector3 toLight);
    }

    public class PointLight : ILightSource, ITransformative, ITraceable
    {
        public Vector3 Intensity { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public Vector3[] GetPoints(uint maxSamples, bool rng)
        {
            return new Vector3[] { Position };
        }

        public float AnlgeEnergy(Vector3 toLight)
        {
            return 1f;
        }

        public Matrix4 TransformMatrix(Matrix4 matrix)
        {
            throw new System.NotImplementedException();
        }

        public bool CheckHit(Ray ray, out RayHit hit)
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class MultiSampleLight : ILightSource, ITransformative, ITraceable
    {
        public Vector3 Intensity { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                InitSamples();
            }
        }

        protected int uniqSamples = 128;
        protected Vector3[] allPoints;
        protected Vector3 position;

        public MultiSampleLight(int uniqSamples)
        {
            this.uniqSamples = uniqSamples;
            allPoints = new Vector3[uniqSamples];
        }

        protected abstract void InitSamples();

        public float AnlgeEnergy(Vector3 toLight)
        {
            return 1f;
        }

        public Vector3[] GetPoints(uint maxSamples, bool rng)
        {
            int size = 1 + (int)Math.Abs(maxSamples - 1);
            Vector3[] res = new Vector3[size];
            res[size - 1] = position;
            if (rng)
                for (int i = 0; i < size - 1; i++)
                    res[i] = allPoints[RMath.ThreadLocalRandom.Instance.Next(0, uniqSamples - 1)];
            else
                for (int i = 0; i < size - 1; i++)
                    res[i] = allPoints[i % allPoints.Length];
            return res;
        }

        public Matrix4 TransformMatrix(Matrix4 matrix)
        {
            throw new System.NotImplementedException();
        }

        public bool CheckHit(Ray ray, out RayHit hit)
        {
            throw new System.NotImplementedException();
        }
    }

    public class SphereAreaLight : MultiSampleLight
    {
        private float radius;
        public float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                InitSamples();
            }
        }

        public SphereAreaLight(int uniqSamples) : base(uniqSamples) { }

        protected override void InitSamples()
        {
            for (int i = 0; i < uniqSamples; i++)
                allPoints[i] = position + RMath.RndUnit() * radius;
        }
    }

    public class SphereVolumeLight : MultiSampleLight
    {
        private float radius;
        public float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                InitSamples();
            }
        }

        public SphereVolumeLight(int uniqSamples) : base(uniqSamples) { }

        protected override void InitSamples()
        {
            for (int i = 0; i < uniqSamples; i++)
                allPoints[i] = position + RMath.RndUnit() * radius * (float)RMath.ThreadLocalRandom.NextDouble();
        }
    }

    public class SpotLight : ILightSource, ITransformative, ITraceable
    {
        public Vector3 Intensity { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Normal { get; set; }
        public float AngleMin = 45f;
        public float AngleMax = 55f;

        public Vector3[] GetPoints(uint maxSamples, bool rng)
        {
            return new Vector3[] { Position };
        }

        public float AnlgeEnergy(Vector3 toLight)
        {
            float ang = (float)Math.Acos(Vector3.Dot(toLight, Normal)) * RMath.R2D;
            if (ang <= AngleMin) return 1f;
            if (ang > AngleMax) return 0f;
            return (AngleMax - ang) / (AngleMax - AngleMin);
        }

        public Matrix4 TransformMatrix(Matrix4 matrix)
        {
            throw new System.NotImplementedException();
        }

        public bool CheckHit(Ray ray, out RayHit hit)
        {
            throw new System.NotImplementedException();
        }
    }
}