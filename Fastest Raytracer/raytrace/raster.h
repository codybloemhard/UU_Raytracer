//
// Created by egordm on 19-5-2018.
//

#ifndef RAYTRACE_RASTER_H
#define RAYTRACE_RASTER_H

#include <glm/vec3.hpp>
#include <cmath>
#include <utility>

namespace raytrace {
    typedef glm::tvec3<unsigned char> Color;

    struct Raster {
        uchar *pixels;
        uint width;
        uint height;
        uint length;
        bool debug;

        Raster(uint width, uint height) : width(width), height(height), length(width * height * 3), pixels(new uchar[width * height * 3]) {}

        virtual ~Raster() {
            delete[] pixels;
        }

        virtual void set_pixel(uint x, uint y, glm::vec3 color) {
            set_pixel(x, y, (uchar)(fminf(1.f, color.x) * 0xFF),
                      (uchar)(fminf(1.f,color.y) * 0xFF),
                      (uchar)(fminf(1.f,color.z) * 0xFF));
        }

        virtual void set_pixel(int x, int y, uchar r, uchar g, uchar b) {
            if(debug && x < RAYTRACE_AREA_WIDTH) return;
            if(x < 0 || y < 0 || x >= width || y >= height) return;
            int offset = (x + y * width) * 3;
            pixels[offset++] = r;
            pixels[offset++] = g;
            pixels[offset] = b;
        }

        virtual void set_pixel(int x, int y, const Color &c) {
            set_pixel(x, y, c.x, c.y, c.z);
        }

        void clear() {
            memset(pixels, 0, length);
        }

        void draw_circle(int px, int py, int radius, const Color &c) {
            int x = radius-1;
            int y = 0;
            int dx = 1;
            int dy = 1;
            int err = dx - (radius << 1);
            while (x >= y)
            {
                set_pixel(px + x, (py + y), c);
                set_pixel(px + y, (py + x), c);
                set_pixel(px - y, (py + x), c);
                set_pixel(px - x, (py + y), c);
                set_pixel(px - x, (py - y), c);
                set_pixel(px - y, (py - x), c);
                set_pixel(px + y, (py - x), c);
                set_pixel(px + x, (py - y), c);

                if (err <= 0) {
                    y++;
                    err += dy;
                    dy += 2;
                }

                if (err > 0) {
                    x--;
                    dx += 2;
                    err += dx - (radius << 1);
                }
            }
        }

        void box(int x1, int y1, int x2, int y2, const Color &c) {
            for(int x = x1; x < x2; ++x) {
                set_pixel(x, y1, c);
                set_pixel(x, y2, c);
            }
            for(int y = y1; y < y2; ++y) {
                set_pixel(x1, y, c);
                set_pixel(x2, y, c);
            }
        }

        void draw_line( float x1, float y1, float x2, float y2, const Color& color ) {
            // Bresenham's line algorithm
            const bool steep = (fabs(y2 - y1) > fabs(x2 - x1));
            if(steep)
            {
                std::swap(x1, y1);
                std::swap(x2, y2);
            }

            if(x1 > x2)
            {
                std::swap(x1, x2);
                std::swap(y1, y2);
            }

            const float dx = x2 - x1;
            const float dy = fabs(y2 - y1);

            float error = dx / 2.0f;
            const int ystep = (y1 < y2) ? 1 : -1;
            int y = (int)y1;

            const int maxX = (int)x2;

            for(int x=(int)x1; x<maxX; x++)
            {
                if(steep)
                {
                    set_pixel(y,x, color);
                }
                else
                {
                    set_pixel(x,y, color);
                }

                error -= dy;
                if(error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }
    };
}

#endif //RAYTRACE_RASTER_H
