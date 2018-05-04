using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Engine.Graphics
{
    public class Shader
    {
        private int prID, vsID, fsID;
        private Dictionary<string, int> attributes;

        public Shader(string vs, string fs)
        {
            prID = GL.CreateProgram();
            LoadShader(vs, ShaderType.VertexShader, prID, out vsID);
            LoadShader(fs, ShaderType.FragmentShader, prID, out fsID);
            GL.LinkProgram(prID);
            attributes = new Dictionary<string, int>();
        }

        public int AddAttributeVar(string name)
        {
            if (attributes.ContainsKey(name)) return -1;
            int id = GL.GetAttribLocation(prID, name);
            attributes.Add(name, id);
            return id;
        }

        public int AddUniformVar(string name)
        {
            if (attributes.ContainsKey(name)) return -1;
            int id = GL.GetUniformLocation(prID, name);
            attributes.Add(name, id);
            return id;
        }

        public int GetVar(string name)
        {
            if (!attributes.ContainsKey(name)) return -1;
            return attributes[name];
        }    

        public bool SetVar(string name, float v)
        {
            int id = GetVar(name);
            if (id == -1) return false;
            GL.Uniform1(id, v);
            return true;
        }

        public bool SetVar(string name, Vector3 v)
        {
            int id = GetVar(name);
            if (id == -1) return false;
            GL.Uniform3(id, v);
            return true;
        }

        public bool SetVar(string name, ref Matrix4 v)
        {
            int id = GetVar(name);
            if (id == -1) return false;
            GL.UniformMatrix4(id, false, ref v);
            return true;
        }

        public void Use()
        {
            GL.UseProgram(prID);
        }

        public void UnUse()
        {
            GL.UseProgram(0);
        }

        public void LoadShader(String name, ShaderType type, int program, out int id)
        {
            id = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(name))
                GL.ShaderSource(id, sr.ReadToEnd());
            GL.CompileShader(id);
            GL.AttachShader(program, id);
        }
    }
}