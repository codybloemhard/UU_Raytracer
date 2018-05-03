using System;
using System.Collections.Generic;
using Engine;
using RaytraceEngine.Objects;

namespace RaytraceEngine
{
    public class RayScene
    {
        public List<ITraceable> primitives;
        public List<ILightSource> sources;
        public RayCamera cam;

        public RayScene()
        {
            primitives = new List<ITraceable>();
            sources = new List<ILightSource>();
        }
    }
}