using OpenTK;

namespace FrockRaytracer
{
    public class Constants
    {
        public const float DEG_TO_RAD = 0.01745329251f;
        public const float LIGHT_IOR = 1.00029f;
        public const float BIG_NUMBER = 999999;
        public const float EPSILON = 0.0001f;
        public const float LIGHT_DECAY = 0.81f;

        public static readonly Vector3 RAYTRACE_DEBUG_OBJ_COLOR = new Vector3(1, 1, 1);
        public static readonly Vector3 RAYTRACE_DEBUG_PRIMARY_RAY_COLOR = new Vector3(0, 1, 1);
        public static readonly Vector3 RAYTRACE_DEBUG_PRIMARY_RAY_COLOR_INCREMENT = new Vector3(.2f, -.2f, 0);
        public static readonly Vector3 RAYTRACE_DEBUG_SHADOW_RAY_COLOR = new Vector3(0.9f,0.9f, 0f);
        public static readonly Vector3 RAYTRACE_DEBUG_SHADOW_OCCLUDED_RAY_COLOR = new Vector3(.96f, .97f, .28f);
        public static readonly Vector3 RAYTRACE_DEBUG_REFRACT_RAY_COLOR = new Vector3(1, 0, 0);
    }
}