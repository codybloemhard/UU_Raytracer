using System;
using OpenTK;

namespace RaytraceEngine
{
    public static class RMath
    {
        //source: the allmighty wikipedia
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return (1 - t) * a + t * b;
        }

        public static string ToStr(Vector3 v)
        {
            return "" + v.X + " , " + v.Y + " , " + v.Z;
        }
    }
}