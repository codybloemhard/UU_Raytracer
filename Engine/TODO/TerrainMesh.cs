using Engine.Graphics;
using Engine.Helpers;
using OpenTK;


namespace Engine.TODO
{
    public class TerrainMesh : Mesh
    {
        public float[,] map;
        private Vector3[,] normals;
        private float[] vertexData, normalData;
        private int halfW, halfH;
        private float hScale;
        public int w { get; private set; }
        public int h { get; private set; }

        public TerrainMesh(string pos, string nor, int w, int h, float hScale, Shader s) : base(pos, nor)
        {
            this.hScale = hScale;
            this.w = w;
            this.h = h;
            halfW = w / 2;
            halfH = h / 2;

            int len = (w) * (h) * 3;
            normals = new Vector3[w, h];
            vertexData = new float[len];
            normalData = new float[len];
            map = new float[w, h];
            //create vertices
            int i = 0;
            for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++) {
                vertexData[i++] = x - halfW;
                vertexData[i++] = y - halfH;
                vertexData[i++] = 0;
            }

            //create indices
            uint[] indices = new uint[(w - 1) * (h - 1) * 6];
            i = 0;
            for (int x = 0; x < w - 1; x++)
            for (int y = 0; y < h - 1; y++) {
                indices[i++] = (uint) ((x + 0) * w + (y + 0));
                indices[i++] = (uint) ((x + 1) * w + (y + 0));
                indices[i++] = (uint) ((x + 1) * w + (y + 1));

                indices[i++] = (uint) ((x + 1) * w + (y + 1));
                indices[i++] = (uint) ((x + 0) * w + (y + 1));
                indices[i++] = (uint) ((x + 0) * w + (y + 0));
            }

            SetIndices(indices);
            UploadIndices(s);
        }

        public void NormalizeMap()
        {
            //find max height
            float hMax = 0f, hMin = 10000000f;
            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++) {
                if (map[x, y] > hMax)
                    hMax = map[x, y];
                if (map[x, y] < hMin)
                    hMin = map[x, y];
            }

            //normalize heightmap
            float diff = hMax - hMin;
            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++) {
                map[x, y] -= hMin;
                map[x, y] /= diff;
            }
        }

        public void BakeMap()
        {
            int i = 0;
            for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++) {
                vertexData[i + 2] = -map[x, y] * hScale;
                i += 3;
            }
        }

        public void CalcNormals()
        {
            //put normals in a grid
            Vector3 zero = new Vector3(0, 0, 0);
            for (int x = 0; x < w - 1; x++)
            for (int y = 0; y < h - 1; y++)
                normals[x, y] = zero;
            Vector3 a = Vector3.Zero, b = Vector3.Zero, c = Vector3.Zero;
            for (int x = 0; x < w - 1; x++)
            for (int y = 0; y < h - 1; y++) {
                a = Help.Read(vertexData, ((x + 0) * w + (y + 0)) * 3);
                b = Help.Read(vertexData, ((x + 1) * w + (y + 0)) * 3);
                c = Help.Read(vertexData, ((x + 1) * w + (y + 1)) * 3);
                normals[x + 0, y + 0] += Help.Normal(a, b, c);
                normals[x + 1, y + 0] += Help.Normal(a, b, c);
                normals[x + 1, y + 1] += Help.Normal(a, b, c);
                a = Help.Read(vertexData, ((x + 1) * w + (y + 1)) * 3);
                b = Help.Read(vertexData, ((x + 0) * w + (y + 1)) * 3);
                c = Help.Read(vertexData, ((x + 0) * w + (y + 0)) * 3);
                normals[x + 1, y + 1] += Help.Normal(a, b, c);
                normals[x + 0, y + 1] += Help.Normal(a, b, c);
                normals[x + 0, y + 0] += Help.Normal(a, b, c);
            }

            //create normals from grid
            int i = 0;
            for (int x = 0; x < w - 0; x++)
            for (int y = 0; y < h - 0; y++) {
                normalData[i++] = normals[x, y].X;
                normalData[i++] = normals[x, y].Y;
                normalData[i++] = normals[x, y].Z;
            }
        }

        public void UpdateMesh(Shader s)
        {
            SetBuffer(vertexData, BufferType.VERTEX);
            SetBuffer(normalData, BufferType.NORMAL);
            UploadBuffer(s, BufferType.VERTEX);
            UploadBuffer(s, BufferType.NORMAL);
        }
    }
}