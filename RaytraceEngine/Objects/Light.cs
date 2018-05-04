using Engine.Objects;
using OpenTK;

namespace RaytraceEngine.Objects
{
    public interface ILightSource
    {
        Vector3 Intensity { get; set; }
        Vector3 GetPos();
    }

    public class PointLight : ILightSource, ITransformative
    {
        public Vector3 Intensity { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        
        public Vector3 GetPos()
        {
            return Position;
        }

        public Matrix4 TransformMatrix(Matrix4 matrix)
        {
            throw new System.NotImplementedException();
        }

    }
}