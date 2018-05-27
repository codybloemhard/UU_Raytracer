using System;
using System.Collections.Generic;
using OpenTK;
using System.IO;
using System.Globalization;
using FrockRaytracer.Structs;

namespace FrockRaytracer.Objects.Primitives
{
    public class Mesh : Primitive
    {
        public Vector3 Scale;

        private List<Polygon> polygons = new List<Polygon>();
        public Vector3[] Vertices { get; set; }
        private int[][] faces;
        public int[][] Faces
        {
            get { return faces; }
            set { faces = value; CreateTriangles(); }
        }

        public Mesh(Vector3 position, Quaternion rotation):base(position, rotation, true) { }

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            bool intersects = false;

            foreach (Polygon polygon in polygons)
            {
                if(polygon.Intersect(ray, ref hit)) intersects = true;
            }

            return intersects;
        }

        private void CreateTriangles()
        {
            for (int i = 0; i < faces.GetLength(0); i++)
            {
                Vector3[] newVertices = new Vector3[faces[i].Length];
                Vector3 pos = Vector3.Zero;
                for (int j = 0; j < newVertices.Length; j++)
                {
                    newVertices[j] = Vertices[faces[i][j]];
                    pos += newVertices[j];
                }
                pos /= newVertices.Length;
                Polygon p = new Polygon(pos, newVertices);
                p.Material = Material;
                polygons.Add(p);
            }
        }

        public void ImportMesh(string url)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int[]> facs = new List<int[]>();

            string line;

            // Read the file and display it line by line.  
            StreamReader file = new StreamReader("../../"+ url);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Length == 0) continue;
                if (line[0] == 'v')
                {
                    //vertex
                    string[] coords = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
                    verts.Add(new Vector3(
                        float.Parse(coords[1], CultureInfo.InvariantCulture) * Scale.X + Position.X,
                        float.Parse(coords[2], CultureInfo.InvariantCulture) * Scale.Y + Position.Y,
                        float.Parse(coords[3], CultureInfo.InvariantCulture) * Scale.Z + Position.Z));
                }
                else if (line[0] == 'f')
                {
                    //face
                    string[] coords = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
                    int[] newFacs = new int[coords.Length - 1];
                    for(int i = 0; i < newFacs.Length; i++)
                    {
                        newFacs[i] = int.Parse(coords[i + 1].Split('/')[0], CultureInfo.InvariantCulture) - 1;
                    }
                    facs.Add(newFacs);
                }
            }
            file.Close();

            Vertices = verts.ToArray();
            Faces = facs.ToArray();
        }
    }
}
