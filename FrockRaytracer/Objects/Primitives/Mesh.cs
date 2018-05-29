using System;
using System.Collections.Generic;
using OpenTK;
using System.IO;
using System.Globalization;
using System.Linq;
using FrockRaytracer.Structs;

namespace FrockRaytracer.Objects.Primitives
{
    public class Mesh : Primitive
    {
        public int X = 1, Y = 2, Z = 3;
        public Vector3 Scale;
        private AABB box;
        private List<Polygon> polygons = new List<Polygon>();
        public Vector3[] Vertices { get; set; }
        private int[][] faces;
        public int[][] Faces
        {
            get { return faces; }
            set { faces = value; CreatePolygons(); }
        }

        public Mesh(Vector3 position, Quaternion rotation):base(position, rotation, false) { }

        public override bool Intersect(Ray ray, ref RayHit hit)
        {
            return box.Intersect(ray, ref hit);
        }

        private void CreatePolygons()
        {
            //Use the given vertices and faces to construct polygons
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
        }

        private AABB CreateBoxes(List<Polygon> polys)
        {
            //Create encapsulating boxes to speed up the rendering; this is done top-down
            
            //Base case: if a list containing 1 or 2 polygons is given, just put them both in a box and return that box 
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

            //If the list is longer than 2, we will create two boxes that both contain half of the polygons in the list
            //The way we divide the polygons is not very smart, we just split the list into 2: we could improve on this by 
            //sorting the list so that polygons that lie close together get put into the same box. However, despite this,
            //the boxes still improve performance by a great amount
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
            //Merge two boxes into one box, and return the min and max vectors of that box
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
            //Read any .obj file and create polygons out of it
            List<Vector3> verts = new List<Vector3>();
            List<int[]> facs = new List<int[]>();

            string line;

            StreamReader file = new StreamReader("../../"+ url);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Length == 0) continue;
                if (line[0] == 'v' && line[1] == ' ')
                {
                    //vertex
                    string[] coords = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
                    verts.Add(new Vector3(
                        float.Parse(coords[X], CultureInfo.InvariantCulture) * Scale.X + Position.X,
                        float.Parse(coords[Y], CultureInfo.InvariantCulture) * Scale.Y + Position.Y,
                        -float.Parse(coords[Z], CultureInfo.InvariantCulture) * Scale.Z + Position.Z));
                }
                else if (line[0] == 'f')
                {
                    //face
                    string[] coords = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
                    coords = coords.Where(x => !string.IsNullOrEmpty(x)).ToArray();
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
