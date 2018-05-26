using System;
using OpenTK;

namespace FrockRaytracer.Utils
{
    public static class Draw
    {
        /// <summary>
        /// Draw circle. With midpoint circle algorithm
        /// https://en.wikipedia.org/wiki/Midpoint_circle_algorithm
        /// </summary>
        /// <param name="raster"></param>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="radius"></param>
        /// <param name="c"></param>
        public static void Circle(Raster raster, int px, int py, int radius, Vector3 c) {
            int x = radius-1;
            int y = 0;
            int dx = 1;
            int dy = 1;
            int err = dx - (radius << 1);
            while (x >= y)
            {
                raster.setPixel(px + x, (py + y), c);
                raster.setPixel(px + y, (py + x), c);
                raster.setPixel(px - y, (py + x), c);
                raster.setPixel(px - x, (py + y), c);
                raster.setPixel(px - x, (py - y), c);
                raster.setPixel(px - y, (py - x), c);
                raster.setPixel(px + y, (py - x), c);
                raster.setPixel(px + x, (py - y), c);

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

        /// <summary>
        /// Draw box. No algorithms here
        /// </summary>
        /// <param name="raster"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="c"></param>
        public static void Box(Raster raster, int x1, int y1, int x2, int y2, Vector3 c) {
            for(int x = x1; x < x2; ++x) {
                raster.setPixel(x, y1, c);
                raster.setPixel(x, y2, c);
            }
            for(int y = y1; y < y2; ++y) {
                raster.setPixel(x1, y, c);
                raster.setPixel(x2, y, c);
            }
        }

        /// <summary>
        /// Draw line with Bresenham's line algorithm
        /// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        /// </summary>
        /// <param name="raster"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="color"></param>
        public static void Line(Raster raster, float x1, float y1, float x2, float y2, Vector3 color ) {
            bool steep = (Math.Abs(y2 - y1) > Math.Abs(x2 - x1));
            if(steep)
            {
                Mem.Swap(ref x1, ref y1);
                Mem.Swap(ref x2, ref y2);
            }

            if(x1 > x2)
            {
                Mem.Swap(ref x1, ref x2);
                Mem.Swap(ref y1, ref y2);
            }

            float dx = x2 - x1;
            float dy = Math.Abs(y2 - y1);

            float error = dx / 2.0f;
            int ystep = (y1 < y2) ? 1 : -1;
            int y = (int)y1;
            int maxX = (int)x2;

            for(int x=(int)x1; x<maxX; x++)
            {
                if(steep) raster.setPixel(y,x, color);
                else  raster.setPixel(x,y, color);

                error -= dy;
                if(error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }
    }
}