using Engine;
using Engine.Helpers;
using Engine.TemplateCode;
using OpenTK;
using RaytraceEngine.Objects;

namespace RaytraceEngine
{
    public class DebugRenderer : IRenderer
    {
        // TODO: dont hardcode. Instead calc LeftTop&Extent and request window size
        private const int WinWidth = 511;
        private const int WinHeight = 511;
        private const int WinOffsetX = 512;
        private readonly Vector2 LeftTop = new Vector2(-5, 9);
        private readonly float Extent = 10;
        
        
        // TODO: specify z to debug render and slice objects accordingly
        // TODO Rays
        public void Render(Surface surface, Scene scene)
        {
            // Draw spheres
            foreach (var o in scene.Objects) {
                // TODO: more shapes. Mb move this into the shape itself? like IDebuggeble
                if (o is Sphere) {
                    var pos = TranslatePos(o.Position);
                    var r = ((Sphere) o).Radius / Extent * WinWidth;
                    Draw2D.DrawCircle(ref surface.pixels, surface.width, (int)pos.X, (int)pos.Y, (int)r, surface.CreateColor(255, 255, 255));
                }
            }
            
            // Draw box around debug window
            surface.Box(WinOffsetX, 0, WinOffsetX + WinWidth, WinHeight, surface.CreateColor(255, 255, 255));

            // Draw camera 1 filled circle and one bigger
            var camPos = TranslatePos(scene.CurrentCamera.Position);
            var camPosDir = TranslatePos(scene.CurrentCamera.Position + Vector3.UnitZ * Matrix3.CreateFromQuaternion(scene.CurrentCamera.Rotation));
            Draw2D.DrawCircle(ref surface.pixels, surface.width, (int)camPos.X, (int)camPos.Y, 5, surface.CreateColor(255, 255, 255));
            Draw2D.DrawCircle(ref surface.pixels, surface.width, (int)camPos.X, (int)camPos.Y, 2, surface.CreateColor(255, 255, 255));
            surface.Line((int)camPos.X, (int)camPos.Y, (int)camPosDir.X, (int)camPosDir.Y, surface.CreateColor(255, 255, 255));

            // Draw cameras fov
            var projectPlane = scene.CurrentCamera.GetNearClippingPlane();
            var pojPlaneP1 = TranslatePos(projectPlane.Item1);
            var pojPlaneP2 = TranslatePos(projectPlane.Item2);
            surface.Line((int)pojPlaneP1.X, (int)pojPlaneP1.Y, (int)pojPlaneP2.X, (int)pojPlaneP2.Y, surface.CreateColor(255, 255, 0));
        }

        private Vector2 TranslatePos(Vector3 pos)
        {
            var x = (pos.X - LeftTop.X) / Extent * WinWidth + WinOffsetX;
            var y = (LeftTop.Y - pos.Z) / Extent * WinHeight;
            return new Vector2(x, y);
        }
    }
}