using Engine.Objects;
using OpenTK;

namespace RaytraceEngine.Objects
{
    public interface ILightSource
    {
        Vector3 Intensity { get; set; }

        Vector3[] GetPoints();
    }

    public class PointLight : ILightSource, ITransformative, ITraceable
    {
        public Vector3 Intensity { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public Vector3[] GetPoints()
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
}