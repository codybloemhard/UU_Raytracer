using System;
using OpenTK;

namespace RaytraceEngine.Objects
{
    public interface ILightEmitter
    {
        Vector3 Intensity { get; set; }
    }
    
    public class PointLight : Object, ILightEmitter
    {
        public Vector3 Intensity { get; set; }
    }
}