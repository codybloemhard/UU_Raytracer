using Engine.Graphics;
using Engine.Helpers;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace Engine.TODO
{
    public class TerrainMesh : Mesh
    {
        public float[,] HeightMap;
        public Vector3[,] Normals { protected set; get; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public AttribBuffer<float> VertexBuffer;
        public AttribBuffer<float> NormalBuffer;

        public float HeightScale;
        protected bool UploadedIndices;

        public TerrainMesh(string posAttrName, string norAttrName, float heightScale, int width, int height)
        {
            HeightScale = heightScale;
            Width = width;
            Height = height;
            UploadedIndices = false;
            HeightMap = new float[Width, Height];

            Normals = new Vector3[Width, Height];
            int bufferSize = Width * Height * 3;
            VertexBuffer = new AttribBuffer<float>(new float[bufferSize], posAttrName, VertexAttribPointerType.Float);
            NormalBuffer = new AttribBuffer<float>(new float[bufferSize], norAttrName, VertexAttribPointerType.Float);
            IndexBuffer = new IndexBuffer(new uint[(Width - 1) * (Height - 1) * 6]);
            Buffers.Add(VertexBuffer);
            Buffers.Add(NormalBuffer);
            
            FillBuffers();
        }

        protected void FillBuffers()
        {
            // Fill vertexes. All on ground floor
            int i = 0;
            var halfW = Width / 2;
            var halfH = Height / 2;
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++) {
                VertexBuffer.Data[i++] = x - halfW;
                VertexBuffer.Data[i++] = y - halfH;
                VertexBuffer.Data[i++] = 0;
            }
            
            // Fill indices
            i = 0;
            for (int x = 0; x < Width - 1; x++)
            for (int y = 0; y < Height - 1; y++) {
                IndexBuffer.Data[i++] = (uint) ((x + 0) * Width + (y + 0));
                IndexBuffer.Data[i++] = (uint) ((x + 1) * Width + (y + 0));
                IndexBuffer.Data[i++] = (uint) ((x + 1) * Width + (y + 1));

                IndexBuffer.Data[i++] = (uint) ((x + 1) * Width + (y + 1));
                IndexBuffer.Data[i++] = (uint) ((x + 0) * Width + (y + 1));
                IndexBuffer.Data[i++] = (uint) ((x + 0) * Width + (y + 0));
            }
        }

        public override void Upload(Shader s)
        {
            if (!UploadedIndices) {
                IndexBuffer.Upload(s);
                UploadedIndices = false;
            }
            foreach (var buffer in Buffers) buffer.Upload(s);
        }
        
        public void NormalizeMap()
        {
            //find max height
            float hMax = 0f, hMin = 10000000f;
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++) {
                if (HeightMap[x, y] > hMax)
                    hMax = HeightMap[x, y];
                if (HeightMap[x, y] < hMin)
                    hMin = HeightMap[x, y];
            }

            //normalize heightmap
            float diff = hMax - hMin;
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++) {
                HeightMap[x, y] -= hMin;
                HeightMap[x, y] /= diff;
            }
        }
        
        public void InvalidateVertices()
        {
            int i = 0;
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++) {
                VertexBuffer.Data[i + 2] = -HeightMap[x, y] * HeightScale;
                i += 3;
            }
        }
        
        public void CalcNormals()
        {
            //put normals in a grid
            Vector3 zero = new Vector3(0, 0, 0);
            for (int x = 0; x < Width - 1; x++)
            for (int y = 0; y < Height - 1; y++) Normals[x, y] = zero;
            Vector3 a = Vector3.Zero, b = Vector3.Zero, c = Vector3.Zero;
            for (int x = 0; x < Width - 1; x++)
            for (int y = 0; y < Height - 1; y++) {
                a = Help.ArrayToVec(VertexBuffer.Data, ((x + 0) * Width + (y + 0)) * 3);
                b = Help.ArrayToVec(VertexBuffer.Data, ((x + 1) * Width + (y + 0)) * 3);
                c = Help.ArrayToVec(VertexBuffer.Data, ((x + 1) * Width + (y + 1)) * 3);
                Normals[x + 0, y + 0] += Help.Normal(a, b, c);
                Normals[x + 1, y + 0] += Help.Normal(a, b, c);
                Normals[x + 1, y + 1] += Help.Normal(a, b, c);
                a = Help.ArrayToVec(VertexBuffer.Data, ((x + 1) * Width + (y + 1)) * 3);
                b = Help.ArrayToVec(VertexBuffer.Data, ((x + 0) * Width + (y + 1)) * 3);
                c = Help.ArrayToVec(VertexBuffer.Data, ((x + 0) * Width + (y + 0)) * 3);
                Normals[x + 1, y + 1] += Help.Normal(a, b, c);
                Normals[x + 0, y + 1] += Help.Normal(a, b, c);
                Normals[x + 0, y + 0] += Help.Normal(a, b, c);
            }

            //create normals from grid
            int i = 0;
            for (int x = 0; x < Width - 0; x++)
            for (int y = 0; y < Height - 0; y++) {
                NormalBuffer.Data[i++] = Normals[x, y].X;
                NormalBuffer.Data[i++] = Normals[x, y].Y;
                NormalBuffer.Data[i++] = Normals[x, y].Z;
            }
        }
    }
}