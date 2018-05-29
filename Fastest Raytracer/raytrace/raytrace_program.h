//
// Created by egordm on 17-5-2018.
//

#ifndef RAYTRACE_RAYTRACE_PROGRAM_H
#define RAYTRACE_RAYTRACE_PROGRAM_H

#include "defines.h"
#include "raster.h"
#include "raytracer.h"

namespace raytrace {
    class RaytraceProgram {
    public:
        Raster raster;
        Raytracer raytracer;
        World world;

        RaytraceProgram(uint width, uint height, World world) : raster(width, height), world(world), raytracer() {}

        void Init();

        void Update();

        void Draw();

        void Clear();
    };
}


#endif //RAYTRACE_RAYTRACE_PROGRAM_H
