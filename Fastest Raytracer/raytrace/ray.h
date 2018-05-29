//
// Created by egordm on 19-5-2018.
//

#ifndef RAYTRACE_RAY_H
#define RAYTRACE_RAY_H

#include <glm/vec3.hpp>
#include "defines.h"

namespace raytrace {
    struct Ray {
        glm::vec3 origin;
        glm::vec3 direction;
        char outside;
        uint depth;

        Ray(const glm::vec3 &origin, const glm::vec3 &direction, const char &outside = 1, const uint &depth = 1)
                : origin(origin), direction(direction), outside(outside), depth(depth) {}

        bool is_outside() const {
            return outside == 1;
        }
    };

    class Primitive;

    struct RayHit {
        glm::vec3 position;
        glm::vec3 normal;
        float t = 999999;
        const Primitive *object = nullptr;
    };
}


#endif //RAYTRACE_RAY_H
