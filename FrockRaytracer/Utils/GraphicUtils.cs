using System.IO;
using OpenTK.Graphics.OpenGL;

namespace FrockRaytracer.Utils
{
    public static class GraphicUtils
    {
        /// <summary>
        /// Load a shader from file. Shader identifier is returned
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="program"></param>
        /// <param name="id"></param>
        public static void LoadShader(string name, ShaderType type, int program, out int id)
        {
            id = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(name))
                GL.ShaderSource(id, sr.ReadToEnd());
            GL.CompileShader(id);
            GL.AttachShader(program, id);
        }
    }
}