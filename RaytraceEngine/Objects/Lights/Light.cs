using System;
using Engine.Objects;
using OpenTK;

namespace RaytraceEngine.Objects.Lights
{
    public interface ILightSource
    {
        Vector3 Colour { get; set; }
        float Intensity { get; set; }

        Vector3[] GetPoints(uint maxSamples, bool rng);

        float AngleEnergy(Vector3 toLight);

        float MaxEnergy { get; set; }
    }
    
    public class PointLight : ILightSource, ITransformative, ITraceable
    {
        public virtual Vector3 Colour { get; set; }
        public float Intensity { get; set; }
        public virtual Vector3 Position { get; set; }
        public virtual Quaternion Rotation { get; set; }

        public virtual Vector3[] GetPoints(uint maxSamples, bool rng)
        {
            return new Vector3[] { Position };
        }

        public virtual float AngleEnergy(Vector3 toLight)
        {
            return 1f;
        }

        public float MaxEnergy { get; set; }

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