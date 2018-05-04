﻿using System;
using OpenTK;

namespace RaytraceEngine.Objects
{
    public class Material
    {
        public Vector3 Colour { get; set; }
        public float Roughness { get; set; }
        public float Reflectivity { get; private set; }
        public float Refractivity { get; private set; }

        public Material(Vector3 colour, float roughness, float reflectivity, float refractivity)
        {
            Colour = colour;
            Roughness = roughness;
            Set(reflectivity, refractivity);
        }

        public void Set(float reflectivity, float refractivity)
        {
            float total = reflectivity + refractivity;
            Reflectivity = reflectivity / total;
            Refractivity = refractivity / total;
        }
    }
}