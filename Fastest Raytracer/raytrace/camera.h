//
// Created by egordm on 18-5-2018.
//

#ifndef RAYTRACE_CAMERA_H
#define RAYTRACE_CAMERA_H

#include <glm/gtc/matrix_transform.hpp>
#include "defines.h"
#include "objects.h"

namespace raytrace {
    struct FinitePlane {
        glm::vec3 origin{};
        glm::vec3 nhor{};
        glm::vec3 nvert{};

        FinitePlane() {}

        FinitePlane(const glm::vec3 &origin, const glm::vec3 &nhor, const glm::vec3 &nvert)
                : origin(origin), nhor(nhor), nvert(nvert) {}
    };

    class Camera : public Object {
    private:
        float aspect, fovy, znear;
        FinitePlane fov_plane;
    public:
        Camera(const glm::vec3 &position, const glm::quat &rotation, float aspect = 1.7778f, float fovy = 1.6f,
               float znear = .1f)
                : Object(position, rotation), aspect(aspect), fovy(fovy), znear(znear) {
            generate_fov_plane();
        }

        void set_fov(float angle) {
            fovy = (znear * sinf(angle * DEG_TO_RAD)) / aspect;
            generate_fov_plane();
        }

        void set_position(const glm::vec3 &position) override {
            Object::set_position(position);
            generate_fov_plane();
        }

        void set_rotation(const glm::quat &rotation) override {
            Object::set_rotation(rotation);
            generate_fov_plane();
        }

        void rotate_by(glm::vec3 v) {
            v *= DEG_TO_RAD;
            set_rotation(glm::normalize(get_rotation() * glm::quat(v)));
        }

        FinitePlane generate_fov_plane() {
            float halfHeight = atanf(fovy) * znear * 2;
            float halfWidth = atanf(fovy * aspect) * znear * 2;

            glm::vec3 leftTop = get_rotation() *  glm::vec3(-halfWidth, halfHeight, znear);
            glm::vec3 rightTop = get_rotation() * glm::vec3(halfWidth, halfHeight, znear);
            glm::vec3 leftBottom = get_rotation() * glm::vec3(-halfWidth, -halfHeight, znear);

            fov_plane = FinitePlane(leftTop, rightTop - leftTop, leftBottom - leftTop);
        }

        const FinitePlane &get_fov_plane() const {
            return fov_plane;
        }
    };
}


#endif //RAYTRACE_CAMERA_H
