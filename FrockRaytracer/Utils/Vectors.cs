using System;
using OpenTK;

namespace FrockRaytracer.Utils
{
    public static class Vectors
    {
        public enum Axis
        {
            X = 0, Y = 1, Z = 2
        }

        public static Axis DominantAxis(Vector3 v)
        {
            v = new Vector3(Math.Abs(v.X), Math.Abs(v.Y), Math.Abs(v.Z));
            var ret = Axis.X;
            var tmp = v.X;
            if (v.Y > tmp) {
                ret = Axis.Y;
                tmp = v.Y;
            }
            if (v.Z > tmp) {
                ret = Axis.Z;
            }

            return ret;
        }
    }
}