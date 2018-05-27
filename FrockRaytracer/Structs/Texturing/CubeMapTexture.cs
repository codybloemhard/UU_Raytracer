using System;
using System.Collections.Generic;
using FrockRaytracer.Utils;
using OpenTK;

namespace FrockRaytracer.Structs
{
    public class CubeMapTexture : Texture, ICubeMap
    {
        struct FaceMapping
        {
            public Vector2 Offset;
            public bool FlipX;
            public bool FlipY;
            public bool SwapAxes;

            public FaceMapping(Vector2 offset, bool flipX = false, bool flipY = false, bool swapAxes = false)
            {
                Offset = offset;
                FlipX = flipX;
                FlipY = flipY;
                SwapAxes = swapAxes;
            }

            public Vector2 TransformUV(Vector2 uv)
            {
                var ret = uv;
                if(SwapAxes) Mem.Swap(ref ret.X, ref ret.Y);
                if (FlipX) ret.X = 1f - ret.X;
                if (FlipY) ret.Y = 1f - ret.Y;
                ret += Offset;
                return ret;
            }
        }
        
        struct CubeMapCache
        {
            public Dictionary<BoxFace, FaceMapping> FaceMaps;
            public int FaceSize;

            // TODO: Add posibillity to use different orientations
            public static CubeMapCache VerticalCubeMap(SizedTexture texture)
            {
                var ret = new CubeMapCache();
                ret.FaceSize = texture.Width / 3;
                ret.FaceMaps = new Dictionary<BoxFace, FaceMapping>();
                ret.FaceMaps.Add(BoxFace.TOP, new FaceMapping(new Vector2(1, 0), true, true, true));
                ret.FaceMaps.Add(BoxFace.LEFT, new FaceMapping(new Vector2(0, 1), true));
                ret.FaceMaps.Add(BoxFace.FRONT, new FaceMapping(new Vector2(1, 1), true));
                ret.FaceMaps.Add(BoxFace.RIGHT, new FaceMapping(new Vector2(2, 1)));
                ret.FaceMaps.Add(BoxFace.BOTTOM, new FaceMapping(new Vector2(1, 2), true, false, true));
                ret.FaceMaps.Add(BoxFace.BACK, new FaceMapping(new Vector2(1, 3), true, true));
                return ret;
            }
        }
        
        public SizedTexture Texture;
        private CubeMapCache Cache;

        public CubeMapTexture(SizedTexture texture)
        {
            Texture = texture;
            Cache = CubeMapCache.VerticalCubeMap(texture);
        }

        public Vector3 GetColor(Vector2 uv)
        {
            return Texture.GetColor(uv);
        }

        public Vector3 GetColor(BoxFace face, Vector2 uv)
        {
            var coords =  Cache.FaceMaps[face].TransformUV(uv) * Cache.FaceSize;
            var color = Texture.GetPixel((int) Math.Min(coords.X, Texture.Width-1), (int) Math.Min(coords.Y, Texture.Height-1));
            return color;
        }
    }
}