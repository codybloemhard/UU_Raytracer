using System;
using OpenTK;

namespace RaytraceEngine
{
    public class RayCamera
    {
        private Vector3 pos;
        private float near;
        private int width, height;
        private float aspect;
        private Vector3 n0, n1, n2;
        private Matrix3 m;

        public RayCamera(int w, int h, float near)
        {
            this.near = near;
            pos = Vector3.Zero;
            width = w;
            height = h;
            aspect = (float)h / w;
            n0 = new Vector3(-1, +aspect, near);
            n1 = new Vector3(+1, +aspect, near);
            n2 = new Vector3(+1, -aspect, near);
            SetPos(Vector3.Zero, new Vector3(0, 0, 0));
        }

        public void SetPos(Vector3 pos, Vector3 pyr)
        {
            this.pos = pos; 
            Quaternion q = Quaternion.FromEulerAngles(pyr.X, pyr.Y, pyr.Z);
            Matrix3.CreateFromQuaternion(ref q, out m);
        }

        public Ray FromPixel(int x, int y)
        {
            float wt = (float)x / width;
            float ht = (float)y / height;
            Vector3 onPlane = Vector3.Zero;
            onPlane.X = RMath.Lerp(n0, n1, wt).X;
            onPlane.Y = RMath.Lerp(n0, n2, ht).Y;
            onPlane.Z = near;
            Ray r = new Ray();
            onPlane *= m;
            r.origin = pos;
            r.dir = onPlane;
            r.dir.Normalize();
            return r;
        }
    }
}