using System.Diagnostics;
using Engine.Graphics;
using Engine.TemplateCode;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine.Objects
{
    public class FXAAPlane : Object
    {
        public Surface Surface { get; private set; }
        public Mesh Mesh { get; private set; }
        public Shader Shader { get; private set; }
        public int TextureID { get; private set; }

        public bool ShowEdges = false;
        public bool EnableFXAA = true;
        public float LumaThreashold = 0.5f;
        public float MulReduce = 8.0f;
        public float MinReduce = 128.0f;
        public float MaxSpan = 8.0f;

        public FXAAPlane(Surface surface, int textureID)
        {
            Surface = surface;
            TextureID = textureID > 0 ? textureID : Surface.GenTexture();

            var vertices = new float[] {
                -1, +1, +0,
                +1, +1, +0,
                +1, -1, +0,
                -1, -1, +0
            };
            float[] uvs = {0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0};
            var indices = new uint[] {0, 1, 2, 2, 3, 0};

            Shader = new Shader("assets/shaders/fxaa_VS.glsl", "assets/shaders/fxaa_FS.glsl");
            Shader.Use();
            Shader.AddAttributeVar("vPos");
            Shader.AddAttributeVar("vUv");

            Shader.AddUniformVar("u_colorTexture");
            Shader.AddUniformVar("u_texelStep");
            Shader.AddUniformVar("u_showEdges");
            Shader.AddUniformVar("u_fxaaOn");
            Shader.AddUniformVar("u_lumaThreshold");
            Shader.AddUniformVar("u_mulReduce");
            Shader.AddUniformVar("u_minReduce");
            Shader.AddUniformVar("u_maxSpan");

            Mesh = new Mesh();
            Mesh.IndexBuffer = new IndexBuffer(indices);
            Mesh.Buffers.Add(new AttribBuffer<float>(vertices, "vPos", VertexAttribPointerType.Float));
            Mesh.Buffers.Add(new AttribBuffer<float>(uvs, "vUv", VertexAttribPointerType.Float));

            Mesh.Upload(Shader);
        }

        public void Render()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                Surface.width, Surface.height, 0, PixelFormat.Bgra,
                PixelType.UnsignedByte, Surface.pixels
            );

            Shader.Use();
            Shader.SetVar("u_texelStep", new Vector2(1f / Surface.width, 1f / Surface.height));
            Shader.SetVar("u_showEdges", ShowEdges ? 1 : 0);
            Shader.SetVar("u_fxaaOn", EnableFXAA ? 1 : 0);
            Shader.SetVar("u_lumaThreshold", LumaThreashold);
            Shader.SetVar("u_mulReduce", 1 / MulReduce);
            Shader.SetVar("u_minReduce", 1 / MinReduce);
            Shader.SetVar("u_maxSpan", MaxSpan);
            Shader.SetVar("u_colorTexture", 0);

            Mesh.Render(Shader);
        }
    }
}