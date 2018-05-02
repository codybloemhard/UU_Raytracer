using OpenTK;

namespace Engine.Objects
{
    public interface ITransformative
    {
        /// <summary>
        /// Transform a matrix to space relative to object
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        Matrix4 TransformMatrix(Matrix4 matrix);
    }

    public interface IRenderable
    {
        /// <summary>
        /// Used to render the object on canvas. NOTHING ELSE
        /// </summary>
        /// <param name="view"></param>
        /// <param name="world"></param>
        void Render(Matrix4 view, Matrix4 world);
    }

    /// <summary>
    /// Basic scene object
    /// </summary>
    public abstract class Object : ITransformative
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        protected Object() : this(Vector3.Zero, Quaternion.Identity, Vector3.One) { }

        protected Object(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        /// <summary>
        /// Called when object is added to the scene
        /// </summary>
        public virtual void Init() { }

        /// <summary>
        /// Called every tick if object is in the scene
        /// </summary>
        public virtual void Update() { }
        
        public Matrix4 TransformMatrix(Matrix4 matrix)
        {
            matrix *= Matrix4.CreateFromQuaternion(Rotation);
            matrix *= Matrix4.CreateTranslation(Position);
            matrix *= Matrix4.CreateScale(Scale);
            return matrix;
        }
    }
}