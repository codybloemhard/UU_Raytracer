using System;
using System.Collections.Generic;
using Engine;
using Engine.Objects;
using RaytraceEngine.Objects;
using OpenTK;
using RaytraceEngine.Objects.Lights;

namespace RaytraceEngine
{
    public class RayScene : Scene
    {
        public List<Primitive> Primitives;
        public List<ILightSource> Lights;
        public CubeMap Sky;

        public RayScene(Camera camera, CubeMap sky) : base(camera)
        {
            Primitives = new List<Primitive>();
            Lights = new List<ILightSource>();
            Sky = sky;
        }

        public override void AddObject(ITransformative obj)
        {
            base.AddObject(obj);
            if(obj is Primitive) Primitives.Add(obj as Primitive);
            if(obj is ILightSource) Lights.Add(obj as ILightSource);
        }
    }
}