using System;
using OpenTK;

namespace RaytraceEngine
{
    public static class TraceSettings
    {
        public static Vector3 ambientLight;
        public static uint maxLightSamples = 8;
        public static bool realLightSample = false;
        public static uint recursionDepth = 3;
        public static uint antiAliasing = 2;
    }
}