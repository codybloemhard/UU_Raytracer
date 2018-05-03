using Engine.TemplateCode;

namespace Engine.Helpers
{
    public class Draw2D
    {
        public static void DrawCircle(Surface surface, int px, int py, int radius, int c)
        {
            int x = radius-1;
            int y = 0;
            int dx = 1;
            int dy = 1;
            int err = dx - (radius << 1);
            while (x >= y)
            {
                surface.Plot(px + x, (py + y), c);
                surface.Plot(px + y, (py + x), c);
                surface.Plot(px - y, (py + x), c);
                surface.Plot(px - x, (py + y), c);
                surface.Plot(px - x, (py - y), c);
                surface.Plot(px - y, (py - x), c);
                surface.Plot(px + y, (py - x), c);
                surface.Plot(px + x, (py - y), c);

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