using System;
using OpenTK;

namespace Engine.Helpers
{ 
    public static class Help
    {
        public static float Deg2Rad = 0.0174532925f;
        
        public static Vector3 ArrayToVec(float[] arr, int start)
        {
            return new Vector3(arr[start + 0], arr[start + 1], arr[start + 2]);
        }

        public static Vector3 Normal(float[] arr, int start)
        {
            return Normal(ArrayToVec(arr, start + 0), ArrayToVec(arr, start + 3), ArrayToVec(arr, start + 6));
        }

        public static Vector3 Normal(Vector3 a, Vector3 b, Vector3 c)
        {
            return Vector3.Cross(b - a, c - a);
        }

        public static Vector3 Midpoint(Vector3 a, Vector3 b)
        {
            return (a + b) / 2f;
        }
    }
}