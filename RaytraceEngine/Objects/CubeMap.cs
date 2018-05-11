using System;
using Engine.TemplateCode;
using RaytraceEngine.Objects;
using OpenTK;
using RaytraceEngine;

namespace RaytraceEngine.Objects
{
    public class CubeMap
    {
        private Surface texture;
        private Plane[] faces;
        private Vector2[] corners;
        private int size;

        public CubeMap(string file)
        {
            texture = new Surface(file);
            size = (texture.width / 4);
            faces = new Plane[6];
            for (int i = 0; i < 6; i++)
                faces[i] = new Plane();
            //left
            faces[0].Position = new Vector3(-1, 0, 0);
            faces[0].UpdateNormal(new Vector3(1, 0, 0));
            //right
            faces[1].Position = new Vector3(1, 0, 0);
            faces[1].UpdateNormal(new Vector3(-1, 0, 0));
            //down
            faces[2].Position = new Vector3(0, -1, 0);
            faces[2].UpdateNormal(new Vector3(0, 1, 0));
            //up
            faces[3].Position = new Vector3(0, 1, 0);
            faces[3].UpdateNormal(new Vector3(0, -1, 0));
            //back
            faces[4].Position = new Vector3(0, 0, -1);
            faces[4].UpdateNormal(new Vector3(0, 0, 1));
            //front
            faces[5].Position = new Vector3(0, 0, 1);
            faces[5].UpdateNormal(new Vector3(0, 0, -1));

            corners = new Vector2[6];
            //left, right, down, up, back, front
            corners[0] = new Vector2(0, size);
            corners[1] = new Vector2(size * 2, size);
            corners[2] = new Vector2(size, size * 2);
            corners[3] = new Vector2(size, 0);
            corners[4] = new Vector2(size * 3, size);
            corners[5] = new Vector2(size, size);
        }

        public Vector3 SkyColour(Vector3 dir)
        {
            RayHit hit;
            Ray ray = new Ray();
            ray.Origin = Vector3.Zero;
            ray.Direction = dir;
            float min = 10000f;
            RayHit closeHit = new RayHit();
            int closeIndex = -1;
            for(int i = 0; i < 6; i++)
            {
                if(!faces[i].CheckHit(ray, out hit)) continue;
                if(hit.Distance < min)
                {
                    min = hit.Distance;
                    closeHit = hit;
                    closeIndex = i;
                }
            }
            Vector2 uv = Vector2.Zero;
            switch (closeIndex)
            {
                case 0:
                case 1:
                    uv.X = closeHit.Position.Z;
                    uv.Y = closeHit.Position.Y;
                    break;
                case 2:
                case 3:
                    uv.X = closeHit.Position.X;
                    uv.Y = closeHit.Position.Z;
                    break;
                case 4:
                case 5:
                    uv.X = closeHit.Position.X;
                    uv.Y = closeHit.Position.Y;
                    break;
                default: break;
            }
            switch (closeIndex)
            {
                case 0:
                    uv.Y *= -1;
                    break;
                case 1:
                    uv.Y *= -1;
                    uv.X *= -1;
                    break;
                case 2:
                    uv.Y *= -1;
                    break;
                case 3:
                    uv.Y *= 1;
                    break;
                case 4:
                    uv.Y *= -1;
                    uv.X *= -1;
                    break;
                case 5:
                    uv.Y *= -1;
                    break;
                default: break;
            }
            uv += Vector2.One;
            uv *= 0.5f;
            if (uv.X < 0) uv.X = 0;
            if (uv.X > 1) uv.X = 1;
            if (uv.Y < 0) uv.Y = 0;
            if (uv.Y > 1) uv.Y = 0;
            uv *= size;
            uv += corners[closeIndex];
            uv.X = RMath.Clamp(corners[closeIndex].X + 1, corners[closeIndex].X + size - 1, uv.X);
            uv.Y = RMath.Clamp(corners[closeIndex].Y + 1, corners[closeIndex].Y + size - 1, uv.Y);
            int pixel = (int)uv.X + ((int)uv.Y * texture.width);
            int c = texture.pixels[pixel];
            return RMath.ToFloatColour(c);
        }
    }
}