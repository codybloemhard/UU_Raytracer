//
// Created by egordm on 17-5-2018.
//

#ifndef RAYTRACE_DEFINES_H
#define RAYTRACE_DEFINES_H

typedef unsigned int uint;
typedef unsigned char uchar;

#ifndef M_PI
#define M_PI 3.14159265358979323846264338327950288
#endif

#define DEG_TO_RAD 0.01745329251f
#define RAD_TO_DEG 57.295779513f

#define UNIT_Y glm::vec3(0, 1, 0)
#define VEC_ONE glm::vec3(1, 1, 1)

#define DEMO_WIDTH 1024
#define DEMO_HEIGHT 512
#define DEMO_TITLE "Raytrace DEMO"
#define DEMO_ROTATE_DELTA 3

#define RAYTRACE_AREA_WIDTH 512
#define RAYTRACE_AREA_HEIGHT 512
#define RAYTRACE_DEBUG_ROW 256
#define RAYTRACE_DEBUG_FREQUENCY 16
#define RAYTRACE_DEBUG_AREA_LT glm::vec2(-5, 9)
#define RAYTRACE_DEBUG_AREA_EXT 10
#define RAYTRACE_DEBUG_AREA_WIDTH (DEMO_WIDTH - RAYTRACE_AREA_WIDTH)

#define RAYTRACE_DEBUG_OBJ_COLOR raytrace::Color(0xff, 0xff, 0xff)
#define RAYTRACE_DEBUG_PRIMARY_RAY_COLOR raytrace::Color(0, 0xff, 0xff)
#define RAYTRACE_DEBUG_PRIMARY_RAY_COLOR_INCREMENT glm::tvec3<int>(0x30, -0x30, 0)
#define RAYTRACE_DEBUG_SHADOW_RAY_COLOR raytrace::Color(0x62, 0x00, 0xB3)
#define RAYTRACE_DEBUG_SHADOW_OCCLUDED_RAY_COLOR raytrace::Color(0xF5, 0xF7, 0x49)
#define RAYTRACE_DEBUG_REFRACT_RAY_COLOR raytrace::Color(0xFF, 0x00, 0x00)

#define CONSTANT_MAX_DEPTH 4
#define CONSTANT_LIGHT_IOR 1.00029f

#define LIGHT_DECAY 0.81f
#define EPSILON 0.00001f

#define MULTI_THREAD true

#endif //RAYTRACE_DEFINES_H
