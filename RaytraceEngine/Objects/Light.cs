using Engine.Objects;
using OpenTK;
using System;

namespace RaytraceEngine.Objects
{
    public interface ILightSource
    {
        Vector3 Intensity { get; set; }

        Vector3[] GetPoints(uint maxSamples);
    }

    public class PointLight : ILightSource, ITransformative, ITraceable
    {
        public Vector3 Intensity { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public Vector3[] GetPoints(uint maxSamples)
        {
            return new Vector3[] { Position };
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

    public class SphereVolumeLight : ILightSource, ITransformative, ITraceable
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
        
        private int uniqSamples = 100;
        private Vector3[] allPoints;
        private float radius;
        private Vector3 position;

        public SphereVolumeLight()
        {
            allPoints = new Vector3[uniqSamples];
        }

        private void InitSamples()
        {
            for (int i = 0; i < uniqSamples; i++)
                allPoints[i] = position + RMath.RndUnit() * radius;
        }

        public float Radius { get { return radius; }
            set
            {
                radius = value;
                InitSamples();
            }
        }

        public Vector3[] GetPoints(uint maxSamples)
        {
            int size = 1 + (int)Math.Abs(maxSamples - 1);
            Vector3[] res = new Vector3[size];
            res[size - 1] = position;
            for (int i = 0; i < size - 1; i++)
            {
                res[i] = allPoints[RMath.random.Next(0, uniqSamples - 1)];
            }
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
}