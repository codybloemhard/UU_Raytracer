using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using OpenTK;

namespace FrockRaytracer.Utils
{
    public static class RRandom
    {
        // Sorry forgot the source
        public static class ThreadLocalRandom
        {
            private static readonly Random globalRandom = new Random();
            private static readonly object globalLock = new object();

            private static readonly ThreadLocal<Random> threadRandom = new ThreadLocal<Random>(NewRandom);

            public static Random NewRandom()
            {
                lock (globalLock)
                {
                    return new Random(globalRandom.Next());
                }
            }

            public static Random Instance => threadRandom.Value;

            public static int Next()
            {
                return Instance.Next();
            }

            public static int Next(int i, int j)
            {
                return Instance.Next(i, j);
            }

            public static double NextDouble()
            {
                return Instance.NextDouble();
            }
        }

        //source of sampling on sphere, stratified by myself.
        //https://www.gamasutra.com/view/news/169816/Indepth_Generating_uniformly_distributed_points_on_a_sphere.php
        public static Vector3 RndUnitStratified(int steps, int stepZ, int stepT)
        {
            float z = (float)((ThreadLocalRandom.Instance.NextDouble() / steps) + (stepZ * (1f / steps))) * 2f - 1f;
            float t = (float)((ThreadLocalRandom.Instance.NextDouble() / steps) + (stepT * (1f / steps)) * Math.PI) * 2f;
            float r = (float)Math.Sqrt(1 - z * z);
            float x = r * (float)Math.Cos(t);
            float y = r * (float)Math.Sin(t);
            return new Vector3(x, y, z);
        }

        public static Vector3 RandomChange(Vector3 vec, float power)
        {
            Vector3 r = new Vector3((float)ThreadLocalRandom.Instance.NextDouble(),
                                    (float)ThreadLocalRandom.Instance.NextDouble(),
                                    (float)ThreadLocalRandom.Instance.NextDouble());
            return (vec + r * power).Normalized();
        }
    }
}