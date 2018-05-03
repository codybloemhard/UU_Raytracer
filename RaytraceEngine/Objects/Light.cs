using System;
using OpenTK;

namespace RaytraceEngine.Objects
{
    public interface ILightSource
    {
        Vector3 intensity { get; set; }
    }
    
    public class PointLight : ILightSource
    {
        public Vector3 pos;
        public Vector3 intensity { get; set; }
    }
}