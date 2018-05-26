using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FrockRaytracer.Graphics
{
    /// <summary>
    /// Projection plane which is a plane edge to edge and the pixels are drawn on it. Not to confuse with projection
    /// plane of a camera on which pixels are rasterized.
    /// This projection plane has an option of using fxaa anti aliasing which is fast af
    /// </summary>
    public class ProjectionPlane
    {
        public Shader Shader { get; private set; }
        
        public Mesh Mesh { get; private set; }

        public int Texture;

        public Raster Raster;

        public ProjectionPlane() { }

        public void Init()
        {
            MakeTexture();

            var vertices = new float[] {-1, +1, +0, +1, +1, +0, +1, -1, +0, -1, -1, +0};
            float[] uvs = {0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0};
            var indices = new uint[] {0, 1, 2, 2, 3, 0};

            // Create fxaa shader and add register the uniforms
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

        private void MakeTexture()
        {
            Texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Linear);
        }

        /// <summary>
        /// Resize the raster to a new resolution
        /// </summary>
        /// <param name="size"></param>
        public void Resize(Size size)
        {
            if(size.Width == Raster.Width && size.Height == Raster.Height) return;
            Raster = new Raster(size.Width, size.Height);
        }
        
        public void Render()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
                Raster.Width, Raster.Height, 0, PixelFormat.Rgb,
                PixelType.UnsignedByte, Raster.Pixels
            );

            // Set all FXAA Params
            Shader.Use();
            Shader.SetVar("u_texelStep", new Vector2(1f / Raster.Width, 1f / Raster.Height));
            Shader.SetVar("u_showEdges", Settings.FXAAShowEdges ? 1 : 0);
            Shader.SetVar("u_fxaaOn", Settings.FXAAEnableFXAA ? 1 : 0);
            Shader.SetVar("u_lumaThreshold", Settings.FXAALumaThreashold);
            Shader.SetVar("u_mulReduce", 1 / Settings.FXAAMulReduce);
            Shader.SetVar("u_minReduce", 1 / Settings.FXAAMinReduce);
            Shader.SetVar("u_maxSpan", Settings.FXAAMaxSpan);
            Shader.SetVar("u_colorTexture", 0);

            Mesh.Render(Shader);
        }
    }
}