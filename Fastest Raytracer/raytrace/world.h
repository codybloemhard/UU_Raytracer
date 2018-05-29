//
// Created by egordm on 19-5-2018.
//

#ifndef RAYTRACE_WORLD_H
#define RAYTRACE_WORLD_H

#include <vector>
#include "objects.h"
#include "camera.h"

namespace raytrace {
    class World {
    public:
        std::vector<Primitive*> objects;
        std::vector<Light*> lights;
        Camera camera;
        bool changed = true;

        explicit World(const Camera &camera) : camera(camera) {}

        virtual ~World() {
            for(int i = 0; i < objects.size(); ++i) delete objects[i];
            objects.clear();
            for(int i = 0; i < lights.size(); ++i) delete lights[i];
            lights.clear();
        }

        void add_object(Primitive *object) {
            objects.push_back(object);
        }

        void add_light(Light *light) {
            lights.push_back(light);
        }


    };
}

#endif //RAYTRACE_WORLD_H
