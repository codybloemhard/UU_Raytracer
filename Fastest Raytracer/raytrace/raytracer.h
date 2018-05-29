//
// Created by egordm on 19-5-2018.
//

#ifndef RAYTRACE_RAYTRACER_H
#define RAYTRACE_RAYTRACER_H

#include <thread>
#include "world.h"
#include "raster.h"
#include "debug_draw.h"

namespace raytrace {
    class Raytracer {
    private:
        World *world = nullptr;
        Raster *raster = nullptr;
        debug::DebugData ddat;

    public:
        void raytrace(World *world, Raster *raster);

        void render_rows(uint start, uint end);

        glm::vec3 trace_ray(const Ray &ray, bool debug);

        glm::vec3 trace_ray_inside(Ray ray, const Primitive *obj, bool debug);

        glm::vec3 illuminate_by(const Light *light, const Ray &ray, const RayHit &hit, glm::vec3 &specular, bool debug);

        glm::vec3 illuminate(const Ray &ray, const RayHit &hit, glm::vec3 &specular, bool debug);

        RayHit intersect(const Ray &ray);
    };
}

#endif //RAYTRACE_RAYTRACER_H
