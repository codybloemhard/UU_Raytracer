using System;
using OpenTK;

namespace RaytraceEngine.Objects
{
    public class Material
    {
        public Vector3 Colour { get; set; }
        public float Roughness { get; set; }
        public float Reflectivity { get; set; }
        public float Refractivity { get; set; }
        public float RefractETA { get; set; }

        public Material(Vector3 colour, float roughness, float reflectivity, float refractivity)
        {
            Colour = colour;
            Roughness = roughness;
            Reflectivity = reflectivity;
            Refractivity = refractivity;
            RefractETA = 1f;
        }

        public Material(Vector3 colour, float roughness, float reflectivity, float refractivity, float refractEta)
        {
            Colour = colour;
            Roughness = roughness;
            Reflectivity = reflectivity;
            Refractivity = refractivity;
            RefractETA = refractEta;
        }
    }
}