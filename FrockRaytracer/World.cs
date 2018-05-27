using System.Collections.Generic;
using FrockRaytracer.Objects;
using FrockRaytracer.Objects.Primitives;
using FrockRaytracer.Structs;
using OpenTK;

namespace FrockRaytracer
{
    public class World
    {
        public List<Primitive> Objects { get; private set; } = new List<Primitive>();
        public List<Light> Lights { get; private set; } = new List<Light>();
        public Camera Camera;
        public Environent Environent;
        public bool Changed = true;

        public World(Camera camera)
        {
            Camera = camera;
        }

        /// <summary>
        /// Add an object to world
        /// </summary>
        /// <param name="o"></param>
        public void addObject(Primitive o)
        {
            Objects.Add(o);
            Changed = true;
        }
        
        /// <summary>
        /// Add a light to world
        /// </summary>
        /// <param name="l"></param>
        public void addLight(Light l)
        {
            Lights.Add(l);
            Changed = true;
        }

        public void Cache()
        {
            foreach (var o in Objects) o.Cache();
            foreach (var l in Lights) l.Cache();
            Camera.Cache();
        }
    }
}