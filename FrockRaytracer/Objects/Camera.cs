using System;
using OpenTK;

namespace FrockRaytracer.Objects
{
    public struct FinitePlane
    {
        Vector3 origin;
        Vector3 nhor;
        Vector3 nvert;

        public FinitePlane(Vector3 origin, Vector3 nhor, Vector3 nvert)
        {
            this.origin = origin;
            this.nhor = nhor;
            this.nvert = nvert;
        }

        public Vector3 PointAt(float wt, float ht)
        {
            return wt * nhor + ht * nvert + origin;
        }
    };

    public class Camera : Object
    {
        protected float aspect, fovy, ZNear;
        public FinitePlane FOVPlane { get; private set; }

        public Camera(Vector3 position, Quaternion rotation, float aspect = 1f, float fovy = 1f, float zNear = .1f)
            : base(position, rotation)
        {
            this.aspect = aspect;
            this.fovy = fovy;
            this.ZNear = zNear;
        }

        public float Aspect
        {
            get { return aspect; }
            set
            {
                aspect = value;
                position_cached = false;
            }
        }

        public float Fovy
        {
            get { return fovy; }
            set
            {
                fovy = value;
                position_cached = false;
            }
        }

        public float Znear
        {
            get { return ZNear; }
            set
            {
                ZNear = value;
                position_cached = false;
            }
        }

        public void RotateBy(Vector3 v)
        {
            v *= Constants.DEG_TO_RAD;
            Rotation = (Rotation * new Quaternion(v)).Normalized();
        }

        public override void Cache()
        {
            if (position_cached) return;
            var rotationMatrix = Matrix3.CreateFromQuaternion(Rotation);

            float halfHeight = (float) (Math.Atan(Fovy) * ZNear * 2);
            float halfWidth = (float) (Math.Atan(Fovy * Aspect) * ZNear * 2); // Fovy * Aspect = Fovx
            
            var leftTop = new Vector3(-halfWidth, halfHeight, ZNear) * rotationMatrix;
            var rightTop = new Vector3(halfWidth, halfHeight, ZNear) * rotationMatrix;
            var leftBottom = new Vector3(-halfWidth, -halfHeight, ZNear) * rotationMatrix ;

            FOVPlane = new FinitePlane(leftTop, rightTop - leftTop, leftBottom - leftTop);
            position_cached = true;
        }
    }
}