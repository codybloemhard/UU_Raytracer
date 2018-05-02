using System;
using OpenTK;

namespace Template
{ 
    public static class Help
    {
        public static float Deg2Rad = 0.0174532925f;
        
        public static Vector3 Read(float[] arr, int start)
        {
            return new Vector3(arr[start + 0], arr[start + 1], arr[start + 2]);
        }

        public static Vector3 Normal(float[] arr, int start)
        {
            Vector3 a = Read(arr, start + 0);
            Vector3 b = Read(arr, start + 3);
            Vector3 c = Read(arr, start + 6);
            return Normal(a, b, c);
        }

        public static Vector3 Normal(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 u = b - a;
            Vector3 v = c - a;
            Vector3 n = new Vector3();
            n.X = u.Y * v.Z - u.Z * v.Y;
            n.Y = u.Z * v.X - u.X * v.Z;
            n.Z = u.X * v.Y - u.Y * v.X;
            return n;
        }

        public static Vector3 Midpoint(Vector3 a, Vector3 b)
        {
            Vector3 res = Vector3.Zero;
            res.X = (a.X + b.X) / 2f;
            res.Y = (a.Y + b.Y) / 2f;
            res.Z = (a.Z + b.Z) / 2f;
            return res;
        }
    }
}