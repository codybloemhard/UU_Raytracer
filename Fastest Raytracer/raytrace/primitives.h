//
// Created by egordm on 19-5-2018.
//

#ifndef RAYTRACE_PRIMITIVES_H
#define RAYTRACE_PRIMITIVES_H

#include "objects.h"
#include "defines.h"

namespace raytrace {
    class Sphere : public Primitive {
    private:
        float radius = 1;
        glm::vec3 normal;
    public:
        Sphere(const glm::vec3 &position, const float &radius)
                : Primitive(position, glm::quat(), Material(), true), radius(radius) {}

        bool intersect(const Ray &ray, RayHit &hit) const override {
            auto L = get_position() - ray.origin;
            float tca = glm::dot(L, ray.direction);
            if(tca < 0) return false;

            auto q = L - tca * ray.direction;
            float d2 = glm::dot(q, q);
            if(d2 > powf(radius, 2)) return false;

            auto t = tca - ray.outside * sqrtf(powf(radius, 2) - d2);
            if(t < 0 || t > hit.t) return false;

            hit.t = t;
            hit.position = ray.origin + ray.direction * t;
            hit.normal = (hit.position - get_position()) / radius * (float)ray.outside;
            hit.object = this;
            return true;
        }

        virtual const float get_radius() const {
            return radius;
        }

        virtual void set_radius(float radius) {
            Sphere::radius = radius;
        }
    };

    class Plane : public Primitive {
    private:
        glm::vec3 normal;
    public:
        Plane(const glm::vec3 &position, const glm::quat &rotation)
                : Primitive(position, rotation, Material(), false) {
            normal = UNIT_Y * get_rotation();
        }

        void set_rotation(const glm::quat &rotation) override {
            Object::set_rotation(rotation);
            normal = UNIT_Y * get_rotation();
        }

        bool intersect(const Ray &ray, RayHit &hit) const override {
            float divisor = glm::dot(ray.direction, normal);
            if(fabsf(divisor) < EPSILON) return false;

            auto plane_vec = get_position() - ray.origin;
            float t = glm::dot(plane_vec, normal) / divisor;
            if(t < EPSILON || t > hit.t) return false;

            hit.normal = normal;
            hit.position = ray.origin + ray.direction * t;
            hit.object = this;
            hit.t = t;
            return true;
        }
    };
}

#endif //RAYTRACE_PRIMITIVES_H
