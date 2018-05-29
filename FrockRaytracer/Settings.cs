using OpenTK;
using FrockRaytracer.Objects;
using System;

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
        
        // Aka anti alias levels
        public static float[] RenderMSAALevels = new float[] {0.5f, 2f};

        public static LightSampleMode LSM = LightSampleMode.TRUE_STRATIFIED;
        public static uint MaxLightSamples = 32;
        public static uint MaxReflectionSamples = 16;

        // Debug
        public static Vector2 DEBUG_AREA_LT = new Vector2(-5, 9);
        public static float DEBUG_AREA_EXT = 10;
        
        public static void LowAAPreset()
        {
            RenderMSAALevels = new float[] {0.125f, 0.5f, 1f};
        }

        public static void MiddleAAPreset()
        {
            RenderMSAALevels = new float[] {0.5f, 1f};
        }
        
        public static void HighAAPreset()
        {
            RenderMSAALevels = new float[] {0.5f, 2f};
        }
        
        public static void UltraAPreset()
        {
            RenderMSAALevels = new float[] {0.5f, 4f};
        }
        
        public static void PhotoPreset()
        {
            RenderMSAALevels = new float[] {4f};
        }
        
        public static void LowQualityPreset()
        {
            FXAAEnableFXAA = true;
            MaxDepth = 4;
            LSM = LightSampleMode.FAKE;
            MaxLightSamples = 1;
            MaxReflectionSamples = 1;
        }
        
        public static void FastMediumQualityPreset()
        {
            FXAAEnableFXAA = true;
            MaxDepth = 4;
            LSM = LightSampleMode.FAKE;
            MaxLightSamples = 8;
            MaxReflectionSamples = 4;
        }
        
        public static void MediumQualityPreset()
        {
            FXAAEnableFXAA = true;
            MaxDepth = 4;
            LSM = LightSampleMode.TRUE_STRATIFIED;
            MaxLightSamples = 8;
            MaxReflectionSamples = 4;
        }
        
        public static void FastHighQualityPreset()
        {
            FXAAEnableFXAA = true;
            MaxDepth = 4;
            LSM = LightSampleMode.FAKE;
            MaxLightSamples = 32;
            MaxReflectionSamples = 16;
        }
        
        
        public static void HighQualityPreset()
        {
            FXAAEnableFXAA = true;
            MaxDepth = 4;
            LSM = LightSampleMode.TRUE_STRATIFIED;
            MaxLightSamples = 32;
            MaxReflectionSamples = 16;
        }
        
        public static void UltraQualityPreset()
        {
            FXAAEnableFXAA = true;
            MaxDepth = 5;
            LSM = LightSampleMode.TRUE_STRATIFIED;
            MaxLightSamples = 64;
            MaxReflectionSamples = 32;
        }
    }
}