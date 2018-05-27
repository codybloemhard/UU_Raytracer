using System;
using FrockRaytracer.Structs;
using FrockRaytracer.Utils;
using OpenTK;

namespace FrockRaytracer.Objects
{
    public class EnvironmentBox : Environent
    {
        public CubeMapTexture Texture;

        public EnvironmentBox(CubeMapTexture texture)
        {
            Texture = texture;
        }

        /// <summary>
        /// Gets color on the environment box
        /// https://scalibq.wordpress.com/2013/06/23/cubemaps/
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        public override Vector3 GetColor(Ray ray)
        {
            var D = ray.Direction;
            var dominant_axis = Vectors.DominantAxis(D);

            // Determine face by bitmasking
            BoxFace face = (BoxFace) (D[(int) dominant_axis] > 0 ? (int) dominant_axis : (int) dominant_axis | 4);

            Vector2 uv;
            switch (dominant_axis) { // TODO: i am pretty sure this can be shorter
                case Vectors.Axis.X: 
                    uv = new Vector2((-D.Z/Math.Abs(D.X) + 1)/2, (-D.Y/Math.Abs(D.X) + 1)/2);
                    break;
                case Vectors.Axis.Y: 
                    uv = new Vector2((-D.Z/Math.Abs(D.Y) + 1)/2, (-D.X/Math.Abs(D.Y) + 1)/2);
                    break;
                case Vectors.Axis.Z: 
                    uv = new Vector2((-D.X/Math.Abs(D.Z) + 1)/2, (-D.Y/Math.Abs(D.Z) + 1)/2);
                    break;
                default:
                    uv = Vector2.Zero;
                    break;
            }

//            Console.WriteLine($"Ray {ray.Direction}; Dominant Axis {dominant_axis}; Face {face}; UV {uv}");

            var color = Texture.GetColor(face, uv);
            return color;
        }
    }
}