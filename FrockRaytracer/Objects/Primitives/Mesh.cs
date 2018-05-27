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

        private AABB box;
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
            return box.Intersect(ray, ref hit);

            bool i = false;
            foreach (Polygon p in polygons)
                if(p.Intersect(ray, ref hit)) i = true;
            return i;
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

            box = CreateBoxes(polygons);
            (box as AABB).PrintStuff();
        }

        private AABB CreateBoxes(List<Polygon> polys)
        {
            if(polys.Count <= 2)
            {
                AABB newBox = new AABB(Vector3.Zero, Quaternion.Identity);
                newBox.A = polys[0];
                newBox.B = polys[polys.Count - 1];
                Vector3[] minmax = MergeBoxes(newBox.A.GetBox(), newBox.B.GetBox());
                newBox.min = minmax[0];
                newBox.max = minmax[1];
                return newBox;
            }
            List<Polygon> lA, lB;
            lA = polys.GetRange(0, polys.Count / 2);
            lB = polys.GetRange(polys.Count / 2, polys.Count - polys.Count / 2  );
            AABB newBox2 = new AABB(Vector3.Zero, Quaternion.Identity);
            newBox2.A = CreateBoxes(lA);
            newBox2.B = CreateBoxes(lB);
            Vector3[] minmax2 = MergeBoxes((AABB)newBox2.A, (AABB)newBox2.B);
            newBox2.min = minmax2[0];
            newBox2.max = minmax2[1];

            return newBox2;
        }

        private Vector3[] MergeBoxes(AABB A, AABB B)
        {
            Vector3 min, max;
            min.X = Math.Min(A.min.X, B.min.X);
            min.Y = Math.Min(A.min.Y, B.min.Y);
            min.Z = Math.Min(A.min.Z, B.min.Z);
            max.X = Math.Max(A.max.X, B.max.X);
            max.Y = Math.Max(A.max.Y, B.max.Y);
            max.Z = Math.Max(A.max.Z, B.max.Z);
            return new Vector3[] { min, max };
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
