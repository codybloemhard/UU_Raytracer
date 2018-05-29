//
// Created by egordm on 19-5-2018.
//

#ifndef RAYTRACE_OBJECTS_H
#define RAYTRACE_OBJECTS_H

#include <glm/vec3.hpp>
#include <glm/gtx/quaternion.hpp>
#include "ray.h"
#include "defines.h"


namespace raytrace {
    struct Material {
        glm::vec3 diffuse = VEC_ONE;
        glm::vec3 specular = VEC_ONE;
        glm::vec3 absorb = VEC_ONE;
        bool glossy = false;
        float shinyness = 3;
        bool is_dielectic = false;
        bool is_mirror = false;
        float reflectivity = 0;
        bool is_refractive = false;
        float refraction_index = CONSTANT_LIGHT_IOR;
    };

    class Object {
    private:
        glm::vec3 position;
        glm::quat rotation;
    public:
        Object() = default;

        Object(const glm::vec3 &position, const glm::quat &rotation)
                : position(position), rotation(rotation) {}

        virtual const glm::vec3 &get_position() const {
            return position;
        }

        virtual void set_position(const glm::vec3 &position) {
            Object::position = position;
        }

        virtual const glm::quat &get_rotation() const {
            return rotation;
        }

        virtual void set_rotation(const glm::quat &rotation) {
            Object::rotation = rotation;
        }
    };

    class Light : public Object {
        glm::vec3 color;
        float intensity;
    public:
        Light(const glm::vec3 &position, const glm::vec3 &color, const float &intensity)
                : Object(position, glm::quat()), color(color), intensity(intensity) {}

        Light(const glm::vec3 &position, const glm::quat &rotation, const glm::vec3 &color, const float &intensity)
                : Object(position, rotation), color(color), intensity(intensity) {}

        const glm::vec3 &get_color() const {
            return color;
        }

        void set_color(const glm::vec3 &color) {
            Light::color = color;
        }

        float get_intensity() const {
            return intensity;
        }

        void set_intensity(float intensity) {
            Light::intensity = intensity;
        }
    };

    class Primitive : public Object {
    public:
        Material mat;
        bool volumetric;

        Primitive(const glm::vec3 &position, const glm::quat &rotation, const Material &mat = Material(),
                  const bool &volumetric = false)
                : Object(position, rotation), mat(mat), volumetric(volumetric) {}

        virtual bool intersect(const Ray &ray, RayHit &hit) const = 0;
    };
}


#endif //RAYTRACE_OBJECTS_H
