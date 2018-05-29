//
// Created by egordm on 20-5-2018.
//

// 2 + 2 = 4, - 1 = 3
#ifndef RAYTRACE_QUICK_MATHS_H
#define RAYTRACE_QUICK_MATHS_H

#include <glm/glm.hpp>
#include <cmath>
#include "ray.h"

using namespace raytrace;

namespace raytrace {
    namespace maths {
        float fresnel(float n1, float n2, glm::vec3 normal, glm::vec3 incident) {
            float r0 = powf((n1 - n2) / (n1 + n2), 2);
            float cosX = -glm::dot(normal, incident);

            if (n1 > n2) {
                float n = powf(n1 / n2, 2);
                float sinT2 = n * (1.0f - cosX * cosX);

                // Total internal reflection
                if (sinT2 > 1) return 1.0;
                cosX = sqrtf(1.0f - sinT2);
            }
            float x = powf(1.0f - cosX, 5);
            float ret = r0 + (1.0f - r0) * x;

            return ret;
        }

        glm::vec3 refract(const glm::vec3 &I, const glm::vec3 &N, const float &eta) {
            float cosI = glm::dot(I, N);
            return I * eta - N * (-cosI + eta * cosI);
        }
    }

    namespace ray_transform {
        inline Ray reflect(const Ray &ray, const RayHit &hit) {
            auto dir = glm::reflect(ray.direction, hit.normal);
            return Ray(hit.position + dir * EPSILON, dir, ray.outside, ray.depth + 1);
        }

        inline Ray refract(const Ray &ray, const RayHit &hit) {
            float eta = ray.is_outside()
                        ? CONSTANT_LIGHT_IOR / hit.object->mat.refraction_index
                        : hit.object->mat.refraction_index / CONSTANT_LIGHT_IOR;

            auto dir = glm::refract(ray.direction, hit.normal, eta);
            char outside = (char) (hit.object->volumetric ? (ray.is_outside() ? -1 : 1) : 1);
            return Ray(hit.position + dir * EPSILON, dir, outside, ray.depth + 1);
        }
    }
}

#endif //RAYTRACE_QUICK_MATHS_H
