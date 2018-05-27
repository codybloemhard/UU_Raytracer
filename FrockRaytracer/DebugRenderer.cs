using System;
using System.Collections.Generic;
using FrockRaytracer.Objects.Primitives;
using FrockRaytracer.Structs;
using FrockRaytracer.Utils;
using OpenTK;

namespace FrockRaytracer
{
    public class DebugData
    {
        public List<Tuple<Ray, RayHit>> PrimaryRays = new List<Tuple<Ray, RayHit>>();
        public List<Tuple<Ray, RayHit>> ShadowRaysOccluded = new List<Tuple<Ray, RayHit>>();
        public List<Tuple<Ray, Vector3>> ShadowRays = new List<Tuple<Ray, Vector3>>();
        public List<Tuple<Ray, RayHit>> RefractRays = new List<Tuple<Ray, RayHit>>();

        public void Clear()
        {
            PrimaryRays.Clear();
            ShadowRaysOccluded.Clear();
            ShadowRays.Clear();
            RefractRays.Clear();
        }
    };

    public static class DebugRenderer
    {
        private static int Width = 512;
        private static int Height = 512;
        
        /// <summary>
        /// Project points X on x axis and Z on z axis. After that transform it to debug scene coordinates
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Vector2 translate_to_debug(Vector3 pos)
        {
            return new Vector2(
                (pos.X - Settings.DEBUG_AREA_LT.X) / Settings.DEBUG_AREA_EXT * Width + Width,
                (Settings.DEBUG_AREA_LT.Y - pos.Z) / Settings.DEBUG_AREA_EXT * Height
            );
        }

        /// <summary>
        /// Debug draw everything on the screen
        /// </summary>
        /// <param name="data"></param>
        /// <param name="raster"></param>
        /// <param name="world"></param>
        public static void DebugDraw(DebugData data, Raster raster, World world)
        {
            Width = raster.WidthHalf;
            Height = raster.Height;
            raster.Clear(Width, raster.Width, 0, raster.Height);
            raster.Debug = true;

            // Draw primitives
            foreach (var primitive in world.Objects) {
                if (primitive is Sphere) {
                    var pos = translate_to_debug(primitive.Position);
                    var r = ((Sphere) primitive).Radius / Settings.DEBUG_AREA_EXT * Width;
                    Draw.Circle(raster, (int) pos.X, (int) pos.Y, (int) r, Constants.RAYTRACE_DEBUG_OBJ_COLOR);
                    continue;
                }
            }

            // Draw a debug line to indictae which row of pixels it debugged
            if (Settings.DrawDebugLine) {
                raster.Debug = false;
                int debugRow = (int) (Settings.RaytraceDebugRow * raster.Height);
                Draw.Line(raster, 0, debugRow, raster.WidthHalf, debugRow, new Vector3(.3f, .3f, .3f));
                for (int i = 0; i < Width / Settings.RaytraceDebugFrequency; ++i) {
                    raster.setPixel(i * Settings.RaytraceDebugFrequency, debugRow, new Vector3(1, 0, 0));
                }

                raster.Debug = true;
            }


            //Draw all rays. Primary rays shift color when depth increases
            foreach (var tr in data.PrimaryRays) {
                var p1 = translate_to_debug(tr.Item1.Origin);
                var p2 = translate_to_debug(tr.Item2.Position);
                var color = Constants.RAYTRACE_DEBUG_PRIMARY_RAY_COLOR;
                var shift = tr.Item1.Depth * Constants.RAYTRACE_DEBUG_PRIMARY_RAY_COLOR_INCREMENT;
                color += shift;
                Draw.Line(raster, p1.X, p1.Y, p2.X, p2.Y, color);
            }

            foreach (var tr in data.ShadowRaysOccluded) {
                var p1 = translate_to_debug(tr.Item1.Origin);
                var p2 = translate_to_debug(tr.Item2.Position);
                Draw.Line(raster, p1.X, p1.Y, p2.X, p2.Y, Constants.RAYTRACE_DEBUG_SHADOW_OCCLUDED_RAY_COLOR);
            }

            foreach (var tr in data.ShadowRays) {
                var p1 = translate_to_debug(tr.Item1.Origin);
                var p2 = translate_to_debug(tr.Item2);
                Draw.Line(raster, p1.X, p1.Y, p2.X, p2.Y, Constants.RAYTRACE_DEBUG_SHADOW_RAY_COLOR);
            }

            foreach (var tr in data.RefractRays) {
                var p1 = translate_to_debug(tr.Item1.Origin);
                var p2 = translate_to_debug(tr.Item2.Position);
                Draw.Line(raster, p1.X, p1.Y, p2.X, p2.Y, Constants.RAYTRACE_DEBUG_REFRACT_RAY_COLOR);
            }
        }
    }
}