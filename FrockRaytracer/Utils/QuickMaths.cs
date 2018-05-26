using System;
using OpenTK;

namespace FrockRaytracer.Utils
{
    public static class QuickMaths
    {
        /// <summary>
        /// Reflect the ray
        /// https://www.scratchapixel.com/lessons/3d-basic-rendering/introduction-to-shading/reflection-refraction-fresnel
        /// </summary>
        /// <param name="incidentVector"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static Vector3 Reflect(Vector3 incidentVector, Vector3 N) 
        { 
            return incidentVector - 2 * Vector3.Dot(incidentVector, N) * N; 
        }
        
        /// <summary>
        /// Refractive boi.
        /// </summary>
        /// <param name="I"></param>
        /// <param name="N"></param>
        /// <param name="ETA"></param>
        /// <returns></returns>
        public static Vector3 Refract(Vector3 I, Vector3 N, float ETA)
        {
            float cosi = Clamp(-1, 1, Vector3.Dot(I, N)); 
            float etai = 1, etat = ETA; 
            Vector3 n = N; 
            if (cosi < 0) { cosi = -cosi; } else { Mem.Swap(ref etai, ref etat); n= -N; } 
            float eta = etai / etat; 
            float k = 1 - eta * eta * (1 - cosi * cosi); 
            return k < 0 ? Vector3.Zero : eta * I + (eta * cosi - (float)Math.Sqrt(k)) * n; 
        }
        
        /// <summary>
        /// Exponentiate a vector
        /// if(sarcasm) { Thanks to opentk for providing this simple function }
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 Exp(Vector3 v)
        {
            v.X = (float)Math.Exp(v.X);
            v.Y = (float)Math.Exp(v.Y);
            v.Z = (float)Math.Exp(v.Z);
            return v;
        }
        
        /// <summary>
        /// Not really math but yep
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static float Clamp(float min, float max, float val)
        {
            if (val < min) val = min;
            if (val > max) val = max;
            return val;
        }
        
     /// <summary>
     /// The big boi
     /// Calculates fresnel with schlicks approximation
     /// </summary>
     /// <param name="n1"></param>
     /// <param name="n2"></param>
     /// <param name="N"></param>
     /// <param name="I"></param>
     /// <returns></returns>
        public static float Fresnel(float n1, float n2, Vector3 N, Vector3 I) 
        {
            float r0 = (float)Math.Pow((n1 - n2) / (n1 + n2), 2);
            float cosX = -Vector3.Dot(N, I);

            if (n1 > n2) {
                float n = (float)Math.Pow(n1 / n2, 2);
                float sinT2 = n * (1.0f - cosX * cosX);

                // Total internal reflection
                if (sinT2 > 1) return 1f;
                cosX = (float)Math.Sqrt(1.0f - sinT2);
            }
            float x = (float)Math.Pow(1.0f - cosX, 5);
            float ret = r0 + (1.0f - r0) * x;

            return ret;
        }
    }
}