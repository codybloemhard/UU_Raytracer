using System;
using System.Collections.Generic;
using Engine;
using Engine.Objects;
using RaytraceEngine.Objects;
using OpenTK;

namespace RaytraceEngine
{
    public class RayScene : Scene
    {
        public List<Primitive> Primitives;
        public List<ILightSource> Lights;
        public Vector3 ambientLight;
        public uint maxLightSamples = 8;

        public RayScene(Camera camera) : base(camera)
        {
            Primitives = new List<Primitive>();
            Lights = new List<ILightSource>();
        }

        public override void AddObject(ITransformative obj)
        {
            base.AddObject(obj);
            if(obj is Primitive) Primitives.Add(obj as Primitive);
            if(obj is ILightSource) Lights.Add(obj as ILightSource);
        }
    }
}