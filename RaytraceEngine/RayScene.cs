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
        private List<Primitive> _primitives;
        private List<ILightSource> _lights;
        public Primitive[] Primitives;
        public ILightSource[] Lights;
        public CubeMap Sky;

        public RayScene(Camera camera, CubeMap sky) : base(camera)
        {
            _primitives = new List<Primitive>();
            _lights = new List<ILightSource>();
            Sky = sky;
        }

        public override void AddObject(ITransformative obj)
        {
            base.AddObject(obj);
            if(obj is Primitive) _primitives.Add(obj as Primitive);
            if(obj is ILightSource) _lights.Add(obj as ILightSource);
        }

        public void PrepareForRender()
        {
            Primitives = _primitives.ToArray();
            Lights = _lights.ToArray();
            foreach (var l in Lights) l.Init();
            foreach (var p in Primitives) p.Init();
        }
    }
}