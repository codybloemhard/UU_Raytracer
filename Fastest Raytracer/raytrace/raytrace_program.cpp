//
// Created by egordm on 17-5-2018.
//

#include "raytrace_program.h"


void raytrace::RaytraceProgram::Init() {
}

void raytrace::RaytraceProgram::Update() {
}

void raytrace::RaytraceProgram::Draw() {
    if(!world.changed) return;
    world.changed = false;
    raytracer.raytrace(&world, &raster);
}

void raytrace::RaytraceProgram::Clear() {
    if(!world.changed) return;
    raster.clear();
}
