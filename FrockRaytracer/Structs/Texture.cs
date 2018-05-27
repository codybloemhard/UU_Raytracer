using OpenTK;

namespace FrockRaytracer.Structs
{
    public interface Texture
    {
        Vector3 GetColor(Vector2 coords);
    }
}