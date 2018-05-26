using System.Collections.Generic;
using FrockRaytracer.Objects;

namespace FrockRaytracer
{
    public class World
    {
        public List<Primitive> Objects { get; private set; } = new List<Primitive>();
        public List<Light> Lights { get; private set; } = new List<Light>();
        public Camera Camera;
        public bool Changed = true;

        public World(Camera camera)
        {
            Camera = camera;
        }

        public void  addObject(Primitive o)
        {
            Objects.Add(o);
            Changed = true;
        }
        
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