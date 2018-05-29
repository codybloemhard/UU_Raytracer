//
// Created by egordm on 19-5-2018.
//

#ifndef RAYTRACE_DEBUG_DRAW_H
#define RAYTRACE_DEBUG_DRAW_H

#include <tuple>
#include <queue>
#include "ray.h"
#include "primitives.h"
#include "raster.h"
#include "world.h"

using namespace raytrace;

namespace raytrace { namespace debug {
    const glm::vec2 raytrace_debug_area_lt = RAYTRACE_DEBUG_AREA_LT;

    struct DebugData {
        std::vector<std::tuple<Ray, RayHit>> primary_rays;
        std::vector<std::tuple<Ray, RayHit>> shadow_rays_occluded;
        std::vector<std::tuple<Ray, glm::vec3>> shadow_rays;
        std::vector<std::tuple<Ray, RayHit>> refract_rays;
    };

    inline glm::vec2 translate_to_debug(const glm::vec3 &pos) {
        return glm::vec2(
                (pos.x - raytrace_debug_area_lt.x) / RAYTRACE_DEBUG_AREA_EXT * RAYTRACE_DEBUG_AREA_WIDTH +
                RAYTRACE_AREA_WIDTH,
                (raytrace_debug_area_lt.y - pos.z) / RAYTRACE_DEBUG_AREA_EXT * DEMO_HEIGHT
        );
    }

    inline void debug_draw(raytrace::Raster *raster, const World *world, const DebugData *data) {
        raster->debug = true;
        for (auto primitive : world->objects) {
            auto sphere = dynamic_cast<Sphere *>(primitive);
            if (sphere != nullptr) {
                auto pos = translate_to_debug(sphere->get_position());
                auto r = sphere->get_radius() / RAYTRACE_DEBUG_AREA_EXT * RAYTRACE_DEBUG_AREA_WIDTH;
                raster->draw_circle(pos.x, pos.y, r, RAYTRACE_DEBUG_OBJ_COLOR);
                continue;
            }
        }

        raster->debug = false;
        raster->draw_line(0, RAYTRACE_DEBUG_ROW, RAYTRACE_AREA_WIDTH, RAYTRACE_DEBUG_ROW, {0xEE, 0xEE, 0xEE});
        for(int i = 0; i < RAYTRACE_DEBUG_AREA_WIDTH / RAYTRACE_DEBUG_FREQUENCY; ++i) {
            raster->set_pixel(i * RAYTRACE_DEBUG_FREQUENCY, RAYTRACE_DEBUG_ROW, {0xFF, 0, 0});
        }
        raster->debug = true;

        for(auto tr : data->primary_rays) {
            auto p1 = translate_to_debug(std::get<0>(tr).origin);
            auto p2 = translate_to_debug(std::get<1>(tr).position);
            auto color = RAYTRACE_DEBUG_PRIMARY_RAY_COLOR ;
            auto shift = (int)std::get<0>(tr).depth * RAYTRACE_DEBUG_PRIMARY_RAY_COLOR_INCREMENT;
            color.x = static_cast<unsigned char>(color.x + shift.x);
            color.y = static_cast<unsigned char>(color.y + shift.y);
            color.z = static_cast<unsigned char>(color.z + shift.z);
            raster->draw_line(p1.x, p1.y, p2.x, p2.y, color);
        }

        for(auto tr : data->shadow_rays_occluded) {
            auto p1 = translate_to_debug(std::get<0>(tr).origin);
            auto p2 = translate_to_debug(std::get<1>(tr).position);
            raster->draw_line(p1.x, p1.y, p2.x, p2.y, RAYTRACE_DEBUG_SHADOW_RAY_COLOR);
        }

        for(auto tr : data->shadow_rays) {
            auto p1 = translate_to_debug(std::get<0>(tr).origin);
            auto p2 = translate_to_debug(std::get<1>(tr));
            raster->draw_line(p1.x, p1.y, p2.x, p2.y, RAYTRACE_DEBUG_SHADOW_OCCLUDED_RAY_COLOR);
        }

        for(auto tr : data->refract_rays) {
            auto p1 = translate_to_debug(std::get<0>(tr).origin);
            auto p2 = translate_to_debug(std::get<1>(tr).position);
            raster->draw_line(p1.x, p1.y, p2.x, p2.y, RAYTRACE_DEBUG_REFRACT_RAY_COLOR);
        }

        raster->debug = false;
    }
}}

#endif //RAYTRACE_DEBUG_DRAW_H
