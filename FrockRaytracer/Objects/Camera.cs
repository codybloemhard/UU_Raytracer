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
        protected float aspect, fovy, znear;
        public FinitePlane FOVPlane { get; private set; }

        public Camera(Vector3 position, Quaternion rotation, float aspect = 1f, float fovy = 1.6f, float znear = .1f)
            : base(position, rotation)
        {
            this.aspect = aspect;
            this.fovy = fovy;
            this.znear = znear;
        }

        public float Aspect
        {
            get => aspect;
            set
            {
                aspect = value;
                position_cached = false;
            }
        }

        public float Fovy
        {
            get => fovy;
            set
            {
                fovy = value;
                position_cached = false;
            }
        }

        public float Znear
        {
            get => znear;
            set
            {
                znear = value;
                position_cached = false;
            }
        }

        void RotateBy(Vector3 v)
        {
            v *= Constants.DEG_TO_RAD;
            Rotation = (Rotation * new Quaternion(v)).Normalized();
        }

        public override void Cache()
        {
            if (position_cached) return;
            float halfHeight = (float) Math.Atan(fovy) * znear * 2;
            float halfWidth = (float) Math.Atan(fovy * aspect) * znear * 2;

            var leftTop = Rotation * new Vector3(-halfWidth, halfHeight, znear);
            var rightTop = Rotation * new Vector3(halfWidth, halfHeight, znear);
            var leftBottom = Rotation * new Vector3(-halfWidth, -halfHeight, znear);

            FOVPlane = new FinitePlane(leftTop, rightTop - leftTop, leftBottom - leftTop);
            position_cached = true;
        }
    }
}