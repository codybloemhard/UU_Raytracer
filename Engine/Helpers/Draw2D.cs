namespace Engine.Helpers
{
    public class Draw2D
    {
        public static void DrawCircle(ref int[] pixels, int width, int px, int py, int radius, int c)
        {
            int x = radius-1;
            int y = 0;
            int dx = 1;
            int dy = 1;
            int err = dx - (radius << 1);
            while (x >= y)
            {
                pixels[px + x + (py + y) * width] = c;
                pixels[px + y+ (py + x) * width] = c;
                pixels[px - y+ (py + x) * width] = c;
                pixels[px - x+ (py + y) * width] = c;
                pixels[px - x+ (py - y) * width] = c;
                pixels[px - y+ (py - x) * width] = c;
                pixels[px + y+ (py - x) * width] = c;
                pixels[px + x+ (py - y) * width] = c;

                if (err <= 0)
                {
                    y++;
                    err += dy;
                    dy += 2;
                }
        
                if (err > 0)
                {
                    x--;
                    dx += 2;
                    err += dx - (radius << 1);
                }
            }
        }
    }
}