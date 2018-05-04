using System;
using OpenTK;

namespace RaytraceEngine
{
    public static class RMath
    {
        //source: my friend google
        public static float PI = 3.14159265359f;

        //source: the allmighty wikipedia
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return (1 - t) * a + t * b;
        }

        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
        
        public static int ToIntColour(Vector3 c)
        {
            int i = (int)(Math.Min(255, c.X * 255)) << 16;
            i += (int)(Math.Min(255, c.Y * 255)) << 8;
            i += (int)(Math.Min(255, c.Z * 255));
            return i;
        }

        public static string ToStr(Vector3 v)
        {
            return "" + v.X + " , " + v.Y + " , " + v.Z;
        }
    }
}