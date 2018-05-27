using System;
using System.Collections.Generic;
using System.Threading;
using FrockRaytracer.Objects;
using FrockRaytracer.Structs;
using FrockRaytracer.Utils;
using OpenTK;

namespace FrockRaytracer
{
    public class Raytracer
    {
        private World world;
        private Raster raster;
        private DebugData ddat = new DebugData();
        private List<Thread> Threads = new List<Thread>();

        /// <summary>
        /// Render everythign to raster. It is important that world stays the same
        /// </summary>
        /// <param name="_world"></param>
        /// <param name="_raster"></param>
        public void Raytrace(World _world, Raster _raster)
        {
            world = _world;
            raster = _raster;
            ddat.Clear();
            world.Cache();
            Threads.Clear();

            if (Settings.IsMultithread) {
                var ConcurrentWorkerCount = Environment.ProcessorCount;
                var rpt = Window.RAYTRACE_AREA_HEIGHT / ConcurrentWorkerCount;

                for (int i = 0; i < ConcurrentWorkerCount; ++i) {
                    var i1 = i;
                    Threads.Add( new Thread(() => RenderRows(i1 * rpt, (i1 + 1) * rpt)));
                    Threads[i].Start();
                }

                for (int i = 0; i < ConcurrentWorkerCount; ++i) Threads[i].Join();
            }
            else {
                RenderRows(0, Window.RAYTRACE_AREA_HEIGHT);
            }
            
            DebugRenderer.DebugDraw(ddat, raster, world);
        }
        
        /// <summary>
        /// Render a specified y range
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        protected void RenderRows(int start, int end)
        {
            bool is_debug_row = false;
            int debug_column = Settings.RaytraceDebugFrequency;

            for (int y = start; y < end; ++y) {
                if (y == Settings.RaytraceDebugRow) is_debug_row = true;

                for (int x = 0; x < Window.RAYTRACE_AREA_WIDTH; ++x) {
                    float wt = (float) x / Window.RAYTRACE_AREA_WIDTH;
                    float ht = (float) y / Window.RAYTRACE_AREA_HEIGHT;
                    
                    Vector3 onPlane = world.Camera.FOVPlane.PointAt(wt, ht);
                    onPlane.Normalize();
                    
                    bool debug = is_debug_row && x == debug_column;
                    if (debug) debug_column += Settings.RaytraceDebugFrequency;

                    var color = traceRay(new Ray(world.Camera.Position, onPlane), debug);
                    raster.setPixel(x, y, color);
                }
            }
        }
        
        /// <summary>
        /// Trace the ray and go around the world in 80ns. Or more :S
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        protected Vector3 traceRay(Ray ray, bool debug = false)
        {
            Vector3 ret = Vector3.Zero;
            if (ray.Depth > Settings.MaxDepth) return ret;
            
            // Check for intersections
            RayHit hit = intersect(ray);
            if (hit.Obj == null) return world.Environent == null ? Vector3.Zero : world.Environent.GetColor(ray);
            if (debug) ddat.PrimaryRays.Add(new Tuple<Ray, RayHit>(ray, hit));
            
            // Calculate color and specular highlights. Pure mirrors dont have diffuse
            Vector3 specular = Vector3.Zero;
            Vector3 color = hit.Obj.Material.IsMirror ? Vector3.Zero : illuminate(ray, hit, ref specular, debug);
            
            // Different materials are handled differently. Would be cool to move that into material
            if (hit.Obj.Material.IsMirror) {
                ret = traceRay(RayTrans.Reflect(ray, hit), debug);
            } else if (hit.Obj.Material.IsDielectic) {
                var n1 = ray.isOutside() ? Constants.LIGHT_IOR : hit.Obj.Material.RefractionIndex; // TODO: shorten this shizzle into a func
                var n2 = ray.isOutside() ? hit.Obj.Material.RefractionIndex : Constants.LIGHT_IOR;
                float reflect_multiplier = QuickMaths.Fresnel(n1, n2, hit.Normal, ray.Direction);
                reflect_multiplier = hit.Obj.Material.Reflectivity + (1f - hit.Obj.Material.Reflectivity) * reflect_multiplier;
                float transmission_multiplier = 1 - reflect_multiplier;

                // Reflect if its worth it
                if (reflect_multiplier > Constants.EPSILON)
                    ret = reflect_multiplier * traceRay(RayTrans.Reflect(ray, hit), debug);
                if (transmission_multiplier > Constants.EPSILON) {
                    if(hit.Obj.Material.IsRefractive) {
                        var tmp_ray = RayTrans.Refract(ray, hit);
                        if(!float.IsNaN(tmp_ray.Direction.X)) // Happens rarely
                            ret += transmission_multiplier * traceRayInside(tmp_ray, hit.Obj, debug);
                    } else {
                        ret += transmission_multiplier * color;
                    }
                }
            } else {
                // Standard diffuse
                ret = color;
            }

            return ret + specular;
        }

        /// <summary>
        /// Traces the ray if its inside an object which means its probably in refraction loop
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="obj"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        protected Vector3 traceRayInside(Ray ray, Primitive obj, bool debug)
        {
            if (ray.isOutside()) return traceRay(ray, debug); // Should not happen anyway
            Vector3 ret = Vector3.Zero;
            float multiplier = 1;
            float absorb_distance = 0;

            // Do the refraction loop. Aka refract, reflect, repeat
            RayHit hit = RayHit.Default();
            while (ray.Depth < Settings.MaxDepth - 1 && multiplier > Constants.EPSILON) {
                if (!obj.Intersect(ray, ref hit)) return ret; // Should not happen either
                if(debug) ddat.RefractRays.Add(new Tuple<Ray, RayHit>(ray, hit));

                // Beer absorption
                absorb_distance += hit.T;
                var absorb = QuickMaths.Exp(-obj.Material.Absorb * absorb_distance);

                // Fresnel
                var reflect_multiplier = QuickMaths.Fresnel(obj.Material.RefractionIndex, Constants.LIGHT_IOR, ray.Direction, hit.Normal);
                var refract_multiplier = 1f - reflect_multiplier;

                // refract if its worth
                if (refract_multiplier > Constants.EPSILON)
                    ret += traceRay(RayTrans.Refract(ray, hit), debug) * refract_multiplier * multiplier * absorb;

                ray = RayTrans.Reflect(ray, hit);
                multiplier *= reflect_multiplier;
            }

            return ret;
        }

        /// <summary>
        /// Get diffuse color and specular for the hit point with a specific light
        /// </summary>
        /// <param name="light"></param>
        /// <param name="ray"></param>
        /// <param name="hit"></param>
        /// <param name="specular"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        protected Vector3 illuminateBy(Light light, Ray ray, RayHit hit, ref Vector3 specular, bool debug)
        {
            // Calclulate if we are within light range
            var light_vec = light.Position - hit.Position;
            float dist_sq = Vector3.Dot(light_vec, light_vec);
            float intens_sq = light.PointLightCache.IntensitySq;
            if (dist_sq >= intens_sq * Constants.LIGHT_DECAY) return Vector3.Zero;
            
            // Create a shadow ray
            var dist = (float)Math.Sqrt(dist_sq);
            light_vec = light_vec / dist;
            var shadow_ray = new Ray(hit.Position + (light_vec * Constants.EPSILON), light_vec);

            var tmp = RayHit.Default();
            foreach (var o in world.Objects) {
                if (o.Intersect(shadow_ray, ref tmp) && tmp.T < dist) {
                    if (debug) ddat.ShadowRaysOccluded.Add(new Tuple<Ray, RayHit>(shadow_ray, tmp));
                    return new Vector3();
                } 
            }

            // Diffuse
            var materialColor = hit.Obj.Material.Texture == null
                ? hit.Obj.Material.Diffuse
                : hit.Obj.Material.Texture.GetColor(hit.Obj.GetUV(hit));
            var color = materialColor * Math.Max(0.0f, Vector3.Dot(hit.Normal, light_vec));

            // Inverse square law
            var light_powah = light.Intensity / (Constants.PI4 * dist_sq);

            // Specular will be used separately
            if (hit.Obj.Material.IsGlossy) {
                var hardness = Math.Max(.0f, Vector3.Dot(-ray.Direction, QuickMaths.Reflect(-light_vec, hit.Normal)));
                specular += light.Color * light_powah * hit.Obj.Material.Specular *
                    (float)Math.Pow(hardness, hit.Obj.Material.Shinyness);
            }

            if (debug) ddat.ShadowRays.Add(new Tuple<Ray, Vector3>(shadow_ray, light.Position));
            return light.Color * color * light_powah;
        }
       
        /// <summary>
        /// Get diffuse color and specular for the hit point
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="hit"></param>
        /// <param name="specular"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        protected Vector3 illuminate(Ray ray, RayHit hit, ref Vector3 specular, bool debug)
        {
            Vector3 ret = Vector3.Zero;
            foreach(var light in world.Lights) ret += illuminateBy(light, ray, hit, ref specular, debug);
            return ret;
        }
        
        protected RayHit intersect(Ray ray)
        {
            var hit = RayHit.Default();
            foreach (var o in world.Objects) o.Intersect(ray, ref hit);
            return hit;
        }
    }
}