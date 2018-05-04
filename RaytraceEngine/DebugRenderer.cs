using Engine;
using Engine.Helpers;
using Engine.Objects;
using Engine.TemplateCode;
using OpenTK;
using RaytraceEngine.Objects;

namespace RaytraceEngine
{
    public class DebugRenderer : IRenderer<RayScene>
    {
        // TODO: dont hardcode. Instead calc LeftTop&Extent and request window size
        private int WinWidth;
        private int WinHeight;
        private int WinOffsetX;
        private readonly Vector2 LeftTop = new Vector2(-5, 9);
        private readonly float Extent = 10;

        public DebugRenderer(int winWidth, int winHeight, int winOffsetX)
        {
            WinWidth = winWidth;
            WinHeight = winHeight;
            WinOffsetX = winOffsetX;
        }


        // TODO: specify z to debug render and slice objects accordingly
        // TODO Rays
        public void Render(Surface surface, RayScene scene)
        {
            // Draw spheres
            foreach (var o in scene.Primitives) {
                // TODO: more shapes. Mb move this into the shape itself? like IDebuggeble
                if (o is Sphere) {
                    var pos = TranslatePos(o.Position);
                    var r = ((Sphere) o).Radius / Extent * WinWidth;
                    Draw2D.DrawCircle(surface, (int)pos.X, (int)pos.Y, (int)r, surface.CreateColor(255, 255, 255));
                }
            }


            foreach (var lightSource in scene.Lights) {
                if(!(lightSource is ITransformative)) continue;
                var pos = TranslatePos((lightSource as ITransformative).Position);
                Draw2D.DrawCircle(surface, (int)pos.X, (int)pos.Y, 5, RMath.ToIntColour(lightSource.Intensity));
            }
            
            // Draw box around debug window
            surface.Box(WinOffsetX, 0, WinOffsetX + WinWidth, WinHeight, surface.CreateColor(255, 255, 255));

            // Draw camera 1 filled circle and one bigger
            var camPos = TranslatePos(scene.CurrentCamera.Position);
            var camPosDir = TranslatePos(scene.CurrentCamera.Position + Vector3.UnitZ * Matrix3.CreateFromQuaternion(scene.CurrentCamera.Rotation));
            Draw2D.DrawCircle(surface, (int)camPos.X, (int)camPos.Y, 5, surface.CreateColor(255, 255, 255));
            Draw2D.DrawCircle(surface, (int)camPos.X, (int)camPos.Y, 2, surface.CreateColor(255, 255, 255));
            surface.Line((int)camPos.X, (int)camPos.Y, (int)camPosDir.X, (int)camPosDir.Y, surface.CreateColor(255, 255, 255));

            // Draw cameras fov
            var projectPlane = scene.CurrentCamera.GetNearClippingPlane();
            var projectPlaneP2 = projectPlane.Origin + projectPlane.NHor + projectPlane.NVert;
            var pojPlaneP1 = TranslatePos(projectPlane.Origin + scene.CurrentCamera.Position);
            var pojPlaneP2 = TranslatePos(projectPlaneP2 + scene.CurrentCamera.Position);
            surface.Line((int)pojPlaneP1.X, (int)pojPlaneP1.Y, (int)pojPlaneP2.X, (int)pojPlaneP2.Y, surface.CreateColor(255, 255, 0));
            
            
            // Draw Rays
            foreach (var ray in Raytracer.Rays) {
                var p1 = TranslatePos(ray.Item1.Origin);
                var p2 = TranslatePos(ray.Item2.Position);
                surface.Line((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, surface.CreateColor(0, 255, 255));
            }
            foreach (var ray in Raytracer.LightRays) {
                var p1 = TranslatePos(ray.Item1.Origin);
                var p2 = TranslatePos(ray.Item2.Position);
                surface.Line((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, 0xF5F749);
            }
            foreach (var ray in Raytracer.ShadowRays) {
                var p1 = TranslatePos(ray.Item1.Origin);
                var p2 = TranslatePos(ray.Item2.Position);
                surface.Line((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, 0x6200B3);
            }
        }

        private Vector2 TranslatePos(Vector3 pos)
        {
            var x = (pos.X - LeftTop.X) / Extent * WinWidth + WinOffsetX;
            var y = (LeftTop.Y - pos.Z) / Extent * WinHeight;
            return new Vector2(x, y);
        }
    }
}