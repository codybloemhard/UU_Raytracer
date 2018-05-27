using OpenTK;

namespace FrockRaytracer.Objects
{
    public class Object
    {
        private Vector3 position = Vector3.Zero;
        private Quaternion rotation = Quaternion.Identity;
        protected bool position_cached;

        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                position_cached = false;
            }
        }

        public Quaternion Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                position_cached = false;
            }
        }

        public Object() { }

        public Object(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public virtual void Cache()  { }
    }
}