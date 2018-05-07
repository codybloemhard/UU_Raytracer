using System;
using OpenTK;

namespace RaytraceEngine
{
    public static class TraceSettings
    {
        public static Vector3 AmbientLight;
        public static uint MaxLightSamples = 8;
        public static bool RealLightSample = false;
        public static uint RecursionDepth = 3;
        public static uint AntiAliasing = 2;
    }
}