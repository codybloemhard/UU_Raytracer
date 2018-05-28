using OpenTK;
using FrockRaytracer.Objects;

namespace FrockRaytracer
{
    public static class Settings
    {
        // FXAA Settings
        public static bool FXAAShowEdges  = false;
        public static bool FXAAEnableFXAA = true;
        public static float FXAALumaThreashold = 0.5f;
        public static float FXAAMulReduce = 8.0f;
        public static float FXAAMinReduce = 128.0f;
        public static float FXAAMaxSpan = 8.0f;
        
        public static float RaytraceDebugRow = 0.5f;
        public static int RaytraceDebugFrequency = 16;
        public static bool DrawDebugLine = true;
        
        public static int MaxDepth = 4;

        // Multithreading and work
        public static bool IsMultithread = true;
        public static bool IsAsync = true;
        public static bool SplitThreads = true;
        
        public static float[] RenderMSAALevels = new float[] {0.5f, 2f};

        public static uint MaxLightSamples = 32;
        public static LightSampleMode LSM = LightSampleMode.TRUE_STRATIFIED;
        public static uint MaxReflectionSamples = 16;

        // Debug
        public static Vector2 DEBUG_AREA_LT = new Vector2(-5, 9);
        public static float DEBUG_AREA_EXT = 10;
    }
}