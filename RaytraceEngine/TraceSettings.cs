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
        
        /// <summary>
        /// With refractivity of 1 light ray through object of length of 20 is fully dimmed 
        /// </summary>
        public static float RefractLightDecayConstant = 15; 
        public static uint MaxReflectionSamples = 8;
    }
}