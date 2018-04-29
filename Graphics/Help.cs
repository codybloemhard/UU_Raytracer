﻿using System;
using OpenTK;

namespace Template
{ 
    public static class Help
    {
        public static Vector3 Read(float[] arr, int start)
        {
            return new Vector3(arr[start + 0], arr[start + 1], arr[start + 2]);
        }

        public static Vector3 Normal(float[] arr, int start)
        {
            Vector3 a = Read(arr, start + 0);
            Vector3 b = Read(arr, start + 3);
            Vector3 c = Read(arr, start + 6);
            Vector3 u = b - a;
            Vector3 v = c - a;
            Vector3 n = new Vector3();
            n.X = u.Y * v.Z - u.Z * v.Y;
            n.Y = u.Z * v.X - u.X * v.Z;
            n.Z = u.X * v.Y - u.Y * v.X;
            return n;
        }

    }
}