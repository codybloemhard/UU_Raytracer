using System.Drawing;
using Engine.TemplateCode;
using OpenTK.Graphics.OpenGL;


namespace Engine
{
    public abstract class Game2D : Game
    {
        protected int ScreenID;
        protected Surface Screen;


        public override void Init()
        {
            Screen = new Surface( Width, Height );
            GL.ClearColor( Color.Black );
            GL.Enable( EnableCap.Texture2D );
            GL.Disable( EnableCap.DepthTest );
            GL.Hint( HintTarget.PerspectiveCorrectionHint, HintMode.Nicest );
            Sprite.target = Screen;
            ScreenID = Screen.GenTexture();
           
        }

        public override void Destroy()
        {
            GL.DeleteTextures( 1, ref ScreenID );
        }

        public override void Render()
        {
            Render2D();
            
            GL.BindTexture( TextureTarget.Texture2D, ScreenID );
            GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                Screen.width, Screen.height, 0, 
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
                PixelType.UnsignedByte, Screen.pixels 
            );
            
            // clear window contents
            GL.Clear( ClearBufferMask.ColorBufferBit );
            // setup camera
            GL.MatrixMode( MatrixMode.Modelview );
            GL.LoadIdentity();
            GL.MatrixMode( MatrixMode.Projection );
            GL.LoadIdentity();
            // draw screen filling quad
            GL.Begin( PrimitiveType.Quads );
            GL.TexCoord2( 0.0f, 1.0f ); GL.Vertex2( -1.0f, -1.0f );
            GL.TexCoord2( 1.0f, 1.0f ); GL.Vertex2(  1.0f, -1.0f );
            GL.TexCoord2( 1.0f, 0.0f ); GL.Vertex2(  1.0f,  1.0f );
            GL.TexCoord2( 0.0f, 0.0f ); GL.Vertex2( -1.0f,  1.0f );
            GL.End();
        }

        public abstract void Render2D();
    }
}