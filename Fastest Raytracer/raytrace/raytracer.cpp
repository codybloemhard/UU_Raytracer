//
// Created by egordm on 19-5-2018.
//

#include "raytracer.h"
#include "quick_maths.h"

using namespace raytrace;
using namespace raytrace::ray_transform;


void Raytracer::raytrace(World *world, Raster *raster) {
    this->world = world;
    this->raster = raster;
    ddat.primary_rays.clear();
    ddat.shadow_rays.clear();
    ddat.shadow_rays_occluded.clear();
    ddat.refract_rays.clear();

#if MULTI_THREAD
    auto thread_count = std::thread::hardware_concurrency();
    std::vector<std::thread> threads;
    threads.reserve(thread_count);
    auto rpt = RAYTRACE_AREA_HEIGHT / thread_count;
    for (int i = 0; i < thread_count; ++i)
        threads.emplace_back(&Raytracer::render_rows, this, i * rpt, (i + 1) * rpt);
    for (int i = 0; i < thread_count; ++i) threads[i].join();
#else
    render_rows(0, RAYTRACE_AREA_HEIGHT);
#endif

    debug::debug_draw(raster, world, &ddat);
}

void Raytracer::render_rows(uint start, uint end) {
    bool is_debug_row = false;
    uint debug_column = RAYTRACE_DEBUG_FREQUENCY;

    for (uint y = start; y < end; ++y) {
        if (y == RAYTRACE_DEBUG_ROW) is_debug_row = true;

        for (uint x = 0; x < RAYTRACE_AREA_WIDTH; ++x) {
            float wt = (float) x / RAYTRACE_AREA_WIDTH;
            float ht = (float) y / RAYTRACE_AREA_HEIGHT;

            glm::vec3 onPlane = glm::normalize(wt * world->camera.get_fov_plane().nhor +
                                               ht * world->camera.get_fov_plane().nvert +
                                               world->camera.get_fov_plane().origin);

            bool debug = is_debug_row && x == debug_column;
            if (debug) debug_column += RAYTRACE_DEBUG_FREQUENCY;

            auto color = trace_ray(Ray(world->camera.get_position(), onPlane), debug);

            raster->set_pixel(x, y, color);
        }
    }
}

glm::vec3 Raytracer::trace_ray(const Ray &ray, bool debug = false) {
    glm::vec3 ret;
    if (ray.depth > CONSTANT_MAX_DEPTH) return ret;

    RayHit hit = intersect(ray);
    if (hit.object == nullptr) return ret;
    if (debug) ddat.primary_rays.push_back(std::make_tuple(ray, hit));

    glm::vec3 specular;
    glm::vec3 color = illuminate(ray, hit, specular, debug);

    if (hit.object->mat.is_mirror) {
        ret = trace_ray(reflect(ray, hit), debug);
    } else if (hit.object->mat.is_dielectic) {
        auto n1 = ray.is_outside() ? CONSTANT_LIGHT_IOR : hit.object->mat.refraction_index;
        auto n2 = ray.is_outside() ? hit.object->mat.refraction_index : CONSTANT_LIGHT_IOR;
        float reflect_multiplier = maths::fresnel(n1, n2, hit.normal, ray.direction);
        reflect_multiplier = hit.object->mat.reflectivity + (1.f - hit.object->mat.reflectivity) * reflect_multiplier;
        float transmission_multiplier = 1 - reflect_multiplier;

        ret = reflect_multiplier * trace_ray(reflect(ray, hit), debug);
        if (transmission_multiplier > EPSILON) {
            if(hit.object->mat.is_refractive) {
                auto tmp_ray = refract(ray, hit);
                if(!std::isnan(tmp_ray.direction.x))
                    ret += transmission_multiplier * trace_ray_inside(tmp_ray, hit.object, debug);
            } else {
                ret += transmission_multiplier * color;
            }
        }
    } else {
        ret = color;
    }

    return ret + specular;
}

glm::vec3 Raytracer::trace_ray_inside(Ray ray, const Primitive *obj, bool debug) {
    if (ray.is_outside()) return trace_ray(ray, debug); // Should not happen anyway
    glm::vec3 ret;
    float multiplier = 1;
    float absorb_distance = 0;

    RayHit hit;
    while (ray.depth < CONSTANT_MAX_DEPTH - 1 && multiplier > EPSILON) {
        if (!obj->intersect(ray, hit))
            return ret; // Should not happen either
        if(debug) ddat.refract_rays.push_back(std::make_tuple(ray, hit));

        // Beer absorption
        absorb_distance += hit.t;
        auto absorb = glm::exp(-obj->mat.absorb * absorb_distance);

        auto reflect_multiplier = maths::fresnel(obj->mat.refraction_index, CONSTANT_LIGHT_IOR, ray.direction, hit.normal);
        auto refract_multiplier = 1.f - reflect_multiplier;

        if (refract_multiplier > EPSILON)
            ret += trace_ray(refract(ray, hit), debug) * refract_multiplier * multiplier * absorb;

        ray = reflect(ray, hit);

        multiplier *= reflect_multiplier;
    }

    return ret;
}

glm::vec3 Raytracer::illuminate_by(const Light *light, const Ray &ray, const RayHit &hit, glm::vec3 &specular,
                                   bool debug = false) {
    auto light_vec = light->get_position() - hit.position;
    float dist_sq = glm::dot(light_vec, light_vec);
    float intens_sq = powf(light->get_intensity(), 2);
    if (dist_sq >= intens_sq * LIGHT_DECAY) return glm::vec3();

    auto dist = sqrtf(dist_sq);
    light_vec = light_vec / dist;
    raytrace::Ray occlusion_ray(hit.position + (light_vec * EPSILON), light_vec);

    RayHit tmp;
    tmp.t = dist;
    for (auto object : world->objects) {
        if (object->intersect(occlusion_ray, tmp)) {
            if (debug) ddat.shadow_rays_occluded.push_back(std::make_tuple(occlusion_ray, tmp));
            return glm::vec3();
        }
    }

    // Diffuse
    glm::vec3 color = hit.object->mat.diffuse * fmaxf(0.0f, glm::dot(hit.normal, light_vec));

    // Specular
    if (hit.object->mat.glossy) {
        auto hardness = fmaxf(.0f, glm::dot(-ray.direction, glm::reflect(-light_vec, hit.normal)));
        specular += light->get_color() * light->get_intensity() * hit.object->mat.specular *
                    powf(hardness, hit.object->mat.shinyness);
    }

    if (debug) ddat.shadow_rays.push_back(std::make_tuple(occlusion_ray, light->get_position()));
    return light->get_color() * color;
}

glm::vec3 Raytracer::illuminate(const Ray &ray, const RayHit &hit, glm::vec3 &specular, bool debug = false) {
    glm::vec3 ret;

    for (const auto &light : world->lights) ret += illuminate_by(light, ray, hit, specular, debug);

    return ret;
}

RayHit Raytracer::intersect(const Ray &ray) {
    RayHit hit = raytrace::RayHit();
    for (auto object : world->objects) object->intersect(ray, hit);
    return hit;
}
