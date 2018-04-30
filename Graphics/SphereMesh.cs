using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
//http://paulbourke.net/geometry/circlesphere/
namespace Template
{
    struct Facet3
    {
        public Vector3 a, b, c;
    }

    public class SphereMesh
    {
        private Mesh mesh;
        private int res = 6;
        private float[] vertices, normals;
        uint[] indices;

        public SphereMesh(Shader s)
        {
            mesh = new Mesh("vPos", "vNor");
            Facet3[] f = new Facet3[(int)Math.Pow(4, res)];
            CreateUnitSphere(res, f);
            vertices = new float[f.Length * 3 * 3];
            normals = new float[f.Length * 3 * 3];
            indices = new uint[f.Length * 3];
            int i = 0;
            for (int j = 0; j < f.Length; j++)
            {
                vertices[i + 0] = f[j].a.X;
                vertices[i + 1] = f[j].a.Y;
                vertices[i + 2] = f[j].a.Z;

                vertices[i + 3] = f[j].b.X;
                vertices[i + 4] = f[j].b.Y;
                vertices[i + 5] = f[j].b.Z;

                vertices[i + 6] = f[j].c.X;
                vertices[i + 7] = f[j].c.Y;
                vertices[i + 8] = f[j].c.Z;
                i += 9;
            }
            i = 0;
            for (int j = 0; j < f.Length; j++)
            {
                normals[i + 0] = f[j].a.X;
                normals[i + 1] = f[j].a.Y;
                normals[i + 2] = f[j].a.Z;

                normals[i + 3] = f[j].b.X;
                normals[i + 4] = f[j].b.Y;
                normals[i + 5] = f[j].b.Z;

                normals[i + 6] = f[j].c.X;
                normals[i + 7] = f[j].c.Y;
                normals[i + 8] = f[j].c.Z;
                i += 9;
            }
            for (uint j = 0; j < indices.Length; j++)
                indices[j] = j;

            Generate();

            mesh.SetBuffer(vertices, BufferType.VERTEX);
            mesh.SetBuffer(normals, BufferType.NORMAL);
            mesh.SetIndices(indices);
            mesh.UploadBuffer(s, BufferType.VERTEX);
            mesh.UploadBuffer(s, BufferType.NORMAL);
            mesh.UploadIndices(s);
        }

        private void Generate()
        {
            float s = 0.2f;
            for (int i = 0; i < vertices.Length; i += 3)
            {
                float n = (float)PerlinNoise.Perlin(vertices[i + 0] * s, vertices[i + 1] * s, vertices[i + 2] * s);
                n = 0.9f + n * 0.1f;
                vertices[i + 0] *= n;
                vertices[i + 1] *= n;
                vertices[i + 2] *= n;
            }
        }

        private int CreateUnitSphere(int iterations, Facet3[] facets)
        {
            int i, j, n, nstart;
            Vector3 p1 = new Vector3(+1, +1, +1);
            Vector3 p2 = new Vector3(-1, -1, 1);
            Vector3 p3 = new Vector3(+1, -1, -1);
            Vector3 p4 = new Vector3(-1, +1, -1);
            p1.Normalize();
            p2.Normalize();
            p3.Normalize();
            p4.Normalize();

            facets[0].a = p1; facets[0].b = p2; facets[0].c = p3;
            facets[1].a = p2; facets[1].b = p1; facets[1].c = p4;
            facets[2].a = p2; facets[2].b = p4; facets[2].c = p3;
            facets[3].a = p1; facets[3].b = p3; facets[3].c = p4;

            n = 4;

            for (i = 1; i < iterations; i++)
            {
                nstart = n;
                for (j = 0; j < nstart; j++)
                {
                    /* Create initially copies for the new facets */
                    facets[n] = facets[j];
                    facets[n + 1] = facets[j];
                    facets[n + 2] = facets[j];

                    /* Calculate the midpoints */
                    p1 = Help.Midpoint(facets[j].a, facets[j].b);
                    p2 = Help.Midpoint(facets[j].b, facets[j].c);
                    p3 = Help.Midpoint(facets[j].c, facets[j].a);

                    /* Replace the current facet */
                    facets[j].b = p1;
                    facets[j].c = p3;

                    /* Create the changed vertices in the new facets */
                    facets[n].a = p1;
                    facets[n].c = p2;
                    facets[n + 1].a = p3;
                    facets[n + 1].b = p2;
                    facets[n + 2].a = p1;
                    facets[n + 2].b = p2;
                    facets[n + 2].c = p3;
                    n += 3;
                }
            }

            for (j = 0; j < n; j++)
            {
                facets[j].a.Normalize();
                facets[j].b.Normalize();
                facets[j].c.Normalize();
            }
            return n;
        }

        public void Render(Shader s)
        {
            mesh.Render(s);
        }
    }
}