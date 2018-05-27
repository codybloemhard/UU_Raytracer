using FrockRaytracer.Utils;
using OpenTK;

namespace FrockRaytracer.Structs
{
    public enum BoxFace
    {
        TOP = Vectors.Axis.Y,
        BOTTOM = Vectors.Axis.Y | 4,
        FRONT = Vectors.Axis.Z ,
        BACK = Vectors.Axis.Z| 4,
        LEFT = Vectors.Axis.X  | 4,
        RIGHT = Vectors.Axis.X
    }

    public interface ICubeMap
    {
        Vector3 GetColor(BoxFace face, Vector2 uv);
    }

    public interface Texture
    {
        Vector3 GetColor(Vector2 uv);
    }

    public abstract class SizedTexture : Texture
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public abstract Vector3 GetPixel(int x, int y);
        
        public abstract Vector3 GetColor(Vector2 uv);
    }
}