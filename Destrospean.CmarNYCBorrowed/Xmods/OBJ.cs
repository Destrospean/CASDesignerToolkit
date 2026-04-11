/*
    Xmods Data Library, a library to support tools for The Sims 4,
    Copyright (C) 2014  C. Marinetti

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
    The author may be contacted at modthesims.info, username cmarNYC.
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Destrospean.CmarNYCBorrowed
{
    public class OBJ
    {
        List<Group> mGroupList;

        List<Normal> mNormalList;

        List<UV> mUVList;

        List<Vertex> mVertexList;

        public int FaceCount
        {
            get
            {
                var faceCount = 0;
                foreach (var group in mGroupList)
                {
                    faceCount += group.FaceCount;
                }
                return faceCount;
            }
        }

        public Group[] GroupArray
        {
            get
            {
                return mGroupList.ToArray();
            }
            set
            {
                mGroupList = new List<Group>(value);
            }
        }

        public int GroupCount
        {
            get
            {
                return mGroupList.Count;
            }
        }

        public Normal[] NormalArray
        {
            get
            {
                return mNormalList.ToArray();
            }
            set
            {
                mNormalList = new List<Normal>(value);
            }
        }

        public UV[] UVArray
        {
            get
            {
                return mUVList.ToArray();
            }
            set
            {
                mUVList = new List<UV>(value);
            }
        }

        public Vertex[] VertexArray
        {
            get
            {
                return mVertexList.ToArray();
            }
            set
            {
                mVertexList = new List<Vertex>(value);
            }
        }

        public enum MeshType
        {
            Base,
            Morph
        }

        public class Face
        {
            public int[][] FacePoints
            {
                get
                {
                    int[][] points =
                        {
                            new int[3],
                            new int[3],
                            new int[3]
                        };
                    for (var i = 0; i < 3; i++)
                    {
                        points[0][i] = Point1[i];
                        points[1][i] = Point2[i];
                        points[2][i] = Point3[i];
                    }
                    return points;
                }
            }

            public int[] Point1, Point2, Point3;

            public Face()
            {
            }

            public Face(int[] point1, int[] point2, int[] point3)
            {
                Point1 = new int[3];
                Point2 = new int[3];
                Point3 = new int[3];
                for (var i = 0; i < 3; i++)
                {
                    if (i < point1.Length)
                    {
                        Point1[i] = point1[i];
                    }
                    if (i < point2.Length)
                    {
                        Point2[i] = point2[i];
                    }
                    if (i < point3.Length)
                    {
                        Point3[i] = point3[i];
                    }
                }
            }

            public Face(int[] points, int offset, MeshType type)
            {
                Point1 = new int[3];
                Point2 = new int[3];
                Point3 = new int[3];
                for (var i = 0; i < 3; i++)
                {
                    if (!(i == 1 && type == MeshType.Morph))
                    {
                        Point1[i] = points[0] + offset;
                        Point2[i] = points[1] + offset;
                        Point3[i] = points[2] + offset;
                    }
                }
            }

            public string PointString(int[] point)
            {
                var text = point[0].ToString();
                for (var i = 1; i < point.Length; i++)
                {
                    text += "/";
                    if (point[i] > 0)
                    {
                        text += point[i].ToString();
                    }
                }
                return text;
            }

            public override string ToString()
            {
                return string.Concat(new[]
                    {
                        PointString(Point1),
                        " ",
                        PointString(Point2),
                        " ",
                        PointString(Point3)
                    });
            }
        }

        public class Group
        {
            public int FaceCount
            {
                get
                {
                    return Faces.Count;
                }
            }

            public List<Face> Faces
            {
                get;
                private set;
            }

            public string GroupName;

            public Group()
            {
                GroupName = "default";
                Faces = new List<Face>();
            }

            public Group(string groupName)
            {
                GroupName = groupName;
                Faces = new List<Face>();
            }

            public void AddFace(Face face)
            {
                Faces.Add(face);
            }
        }

        public class Normal
        {
            public float[] Coordinates
            {
                get
                {
                    return new[]
                    {
                        X,
                        Y,
                        Z
                    };
                }
            }

            public float X
            {
                get;
                private set;
            }

            public float Y
            {
                get;
                private set;
            }

            public float Z
            {
                get;
                private set;
            }

            public Normal()
            {
            }

            public Normal(float[] coordinates)
            {
                X = coordinates[0];
                Y = coordinates[1];
                Z = coordinates[2];
            }

            public Normal(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public override string ToString()
            {
                return string.Concat(new[]
                    {
                        X.ToString("N7", CultureInfo.InvariantCulture),
                        " ",
                        Y.ToString("N7", CultureInfo.InvariantCulture),
                        " ",
                        Z.ToString("N7", CultureInfo.InvariantCulture)
                    });
            }
        }

        public class UV
        {
            public float[] Coordinates
            {
                get
                {
                    return new[]
                    {
                        U,
                        V
                    };
                }
            }

            public float U
            {
                get;
                private set;
            }

            public float V
            {
                get;
                private set;
            }

            public UV()
            {
            }

            public UV(float[] coordinates, bool verticalFlip = false, bool horizontalFlip = false)
            {
                U = horizontalFlip ? 1 - coordinates[0] : coordinates[0];
                V = verticalFlip ? 1 - coordinates[1] : coordinates[1];
            }

            public UV(float u, float v)
            {
                U = u;
                V = v;
            }

            public override string ToString()
            {
                return U.ToString("N7", CultureInfo.InvariantCulture) + " " + V.ToString("N7", CultureInfo.InvariantCulture);
            }
        }

        public class Vertex
        {
            public float[] Coordinates
            {
                get
                {
                    return new[]
                    {
                        X,
                        Y,
                        Z
                    };
                }
            }

            public float X
            {
                get;
                private set;
            }

            public float Y
            {
                get;
                private set;
            }

            public float Z
            {
                get;
                private set;
            }

            public Vertex()
            {
            }

            public Vertex(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public Vertex(float[] coordinates)
            {
                X = coordinates[0];
                Y = coordinates[1];
                Z = coordinates[2];
            }

            public override string ToString()
            {
                return string.Concat(new[]
                    {
                        X.ToString("N7", CultureInfo.InvariantCulture),
                        " ",
                        Y.ToString("N7", CultureInfo.InvariantCulture),
                        " ",
                        Z.ToString("N7", CultureInfo.InvariantCulture)
                    });
            }
        }

        public OBJ(StreamReader reader)
        {
            mVertexList = new List<Vertex>();
            mUVList = new List<UV>();
            mNormalList = new List<Normal>();
            mGroupList = new List<Group>();
            string[] slashSeparator = new[]
                {
                    "/"
                },
            spaceSeparator = new[]
                {
                    " "
                };
            var index = 0;
            string text;
            while ((text = reader.ReadLine()) != null)
            {
                if (text.StartsWith("v "))
                {
                    var coordinateStrings = text.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        mVertexList.Add(new Vertex(float.Parse(coordinateStrings[1], CultureInfo.InvariantCulture), float.Parse(coordinateStrings[2], CultureInfo.InvariantCulture), float.Parse(coordinateStrings[3], CultureInfo.InvariantCulture)));
                        continue;
                    }
                    catch
                    {
                        /*
                        if (MessageBox.Show("Error in .obj text: " + text + Environment.NewLine + "Skip and continue anyway?", "OBJ Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        {
                            break;
                        }
                        */
                        mVertexList.Add(new Vertex(0, 0, 0));
                        continue;
                    }
                }
                if (text.StartsWith("vt "))
                {
                    var coordinateStrings = text.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        mUVList.Add(new UV(float.Parse(coordinateStrings[1], CultureInfo.InvariantCulture), float.Parse(coordinateStrings[2], CultureInfo.InvariantCulture)));
                        continue;
                    }
                    catch
                    {
                        /*
                        if (MessageBox.Show("Error in .obj text: " + text + Environment.NewLine + "Skip and continue anyway?", "OBJ Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        {
                            break;
                        }
                        */
                        mUVList.Add(new UV(0, 0));
                        continue;
                    }
                }
                if (text.StartsWith("vn "))
                {
                    var coordinateStrings = text.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        mNormalList.Add(new Normal(float.Parse(coordinateStrings[1], CultureInfo.InvariantCulture), float.Parse(coordinateStrings[2], CultureInfo.InvariantCulture), float.Parse(coordinateStrings[3], CultureInfo.InvariantCulture)));
                        continue;
                    }
                    catch
                    {
                        /*
                        if (MessageBox.Show("Error in .obj text: " + text + Environment.NewLine + "Skip and continue anyway?", "OBJ Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        {
                            break;
                        }
                        */
                        mNormalList.Add(new Normal(0, 0, 0));
                        continue;
                    }
                }
                if (text.StartsWith("f "))
                {
                    if (mGroupList.Count == 0)
                    {
                        mGroupList.Add(new Group());
                        index = 0;
                    }
                    try
                    {
                        var faceElements = text.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);
                        var facePoints = new int[4][];
                        for (var i = 1; i < faceElements.Length; i++)
                        {
                            var vertexUVNormalStrings = faceElements[i].Split(slashSeparator, StringSplitOptions.None);
                            var vertexUVNormal = new int[vertexUVNormalStrings.Length];
                            for (var j = 0; j < vertexUVNormal.Length; j++)
                            {
                                if (vertexUVNormalStrings[j] == string.Empty)
                                {
                                    vertexUVNormal[j] = 0;
                                }
                                else
                                {
                                    vertexUVNormal[j] = int.Parse(vertexUVNormalStrings[j], CultureInfo.InvariantCulture);
                                }
                                if (vertexUVNormal[j] < 0)
                                {
                                    if (j == 0)
                                    {
                                        vertexUVNormal[j] = mVertexList.Count + vertexUVNormal[j];
                                    }
                                    else if (j == 1)
                                    {
                                        vertexUVNormal[j] = mUVList.Count + vertexUVNormal[j];
                                    }
                                    else if (j == 2)
                                    {
                                        vertexUVNormal[j] = mNormalList.Count + vertexUVNormal[j];
                                    }
                                }
                            }
                            facePoints[i - 1] = vertexUVNormal;
                        }
                        mGroupList[index].AddFace(new Face(facePoints[0], facePoints[1], facePoints[2]));
                        if (faceElements.Length > 4)
                        {
                            mGroupList[index].AddFace(new Face(facePoints[2], facePoints[3], facePoints[0]));
                        }
                        continue;
                    }
                    catch
                    {
                        /*
                        if (MessageBox.Show("Error in .obj text: " + text + Environment.NewLine + "Skip and continue anyway?", "OBJ Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        {
                            break;
                        }
                        */
                        continue;
                    }
                }
                if (text.StartsWith("g "))
                {
                    var groupNames = text.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);
                    if (groupNames.Length > 1)
                    {
                        var exists = false;
                        for (var i = 0; i < mGroupList.Count; i++)
                        {
                            if (string.CompareOrdinal(mGroupList[i].GroupName, groupNames[1]) == 0)
                            {
                                index = i;
                                exists = true;
                                break;
                            }
                        }
                        if (!exists)
                        {
                            mGroupList.Add(new Group(groupNames[1]));
                            index = mGroupList.Count - 1;
                        }
                    }
                }
            }
        }

        public OBJ()
        {
        }

        public OBJ(GEOM baseMesh, GEOM[] morphs)
        {
            var groupNames = new[]
                {
                    "group_fat",
                    "group_fit",
                    "group_thin",
                    "group_special"
                };
            mVertexList = new List<Vertex>();
            mNormalList = new List<Normal>();
            mUVList = new List<UV>();
            mGroupList = new List<Group>();
            for (var i = 0; i < baseMesh.VertexCount; i++)
            {
                mVertexList.Add(new Vertex(baseMesh.GetPosition(i)));
                mNormalList.Add(new Normal(baseMesh.GetNormal(i)));
                mUVList.Add(new UV(baseMesh.GetUV(i, 0), true));
            }
            mGroupList.Add(new Group("group_base"));
            for (var i = 0; i < baseMesh.FaceCount; i++)
            {
                mGroupList[0].AddFace(new Face(baseMesh.GetFaceIndices(i), 1, MeshType.Base));
            }
            int index = 0,
            vertexCount = baseMesh.VertexCount + 1;
            for (var i = 0; i < 4; i++)
            {
                if (morphs[i] != null)
                {
                    for (var j = 0; j < morphs[i].VertexCount; j++)
                    {
                        mVertexList.Add(new Vertex(AddArrays(baseMesh.GetPosition(j), morphs[i].GetPosition(j))));
                        mNormalList.Add(new Normal(AddArrays(baseMesh.GetNormal(j), morphs[i].GetNormal(j))));
                    }
                    mGroupList.Add(new Group(groupNames[i]));
                    index++;
                    for (var j = 0; j < morphs[i].FaceCount; j++)
                    {
                        mGroupList[index].AddFace(new Face(morphs[i].GetFaceIndices(j), vertexCount, MeshType.Morph));
                    }
                    vertexCount += morphs[i].VertexCount;
                }
            }
        }

        public OBJ(GEOM baseMesh, GEOM fatMesh, GEOM fitMesh, GEOM thinMesh, GEOM specialMesh) : this(baseMesh, new[]
            {
                fatMesh,
                fitMesh,
                thinMesh,
                specialMesh
            })
        {
        }

        public OBJ(WSO wso)
        {
            mVertexList = new List<Vertex>();
            mNormalList = new List<Normal>();
            mUVList = new List<UV>();
            mGroupList = new List<Group>();
            var offset = 1;
            for (var i = 0; i < wso.MeshCount; i++)
            {
                var extendedVertices = wso.GetMesh(i).GetExtendedVertices();
                for (var j = 0; j < extendedVertices.Length; j++)
                {
                    var vertexExtended = extendedVertices[j];
                    mVertexList.Add(new Vertex(vertexExtended.GetPosition()));
                    mNormalList.Add(new Normal(vertexExtended.GetNormals()));
                    mUVList.Add(new UV(vertexExtended.GetUVs(), true));
                }
                mGroupList.Add(new Group(wso.GetMesh(i).MeshName));
                var facePoints = wso.GetMesh(i).FacePoints;
                for (var j = 0; j < facePoints.Length; j += 3)
                {
                    mGroupList[i].AddFace(new Face(new[]
                        {
                            (int)facePoints[j].VertexIndex,
                            (int)facePoints[j + 1].VertexIndex,
                            (int)facePoints[j + 2].VertexIndex
                        }, offset, MeshType.Base));
                }
                offset += extendedVertices.Length;
            }
        }

        public float[] AddArrays(float[] v1, float[] v2)
        {
            var array = new float[v1.Length];
            for (var i = 0; i < v1.Length; i++)
            {
                array[i] = v1[i] + v2[i];
            }
            return array;
        }

        public void AddEmptyUV()
        {
            mUVList.Add(new UV(0, 0));
            for (var i = 0; i < GroupCount; i++)
            {
                for (var j = 0; j < mGroupList[i].FaceCount; j++)
                {
                    mGroupList[i].Faces[j].Point1[1] = 1;
                    mGroupList[i].Faces[j].Point2[1] = 1;
                    mGroupList[i].Faces[j].Point3[1] = 1;
                }
            }
        }

        public void CalculateNormals(bool ignoreSeams)
        {
            var refFaces = new Vector3[FaceCount][];
            var refIndices = new int[FaceCount][];
            var index = 0;
            foreach (var group in mGroupList)
            {
                for (var i = 0; i < group.Faces.Count; i++)
                {
                    var facePoints = group.Faces[i].FacePoints;
                    refIndices[index] = new int[3];
                    refIndices[index][0] = facePoints[0][0];
                    refIndices[index][1] = facePoints[1][0];
                    refIndices[index][2] = facePoints[2][0];
                    refFaces[index] = new Vector3[3];
                    refFaces[index][0] = new Vector3(mVertexList[facePoints[0][0] - 1].Coordinates);
                    refFaces[index][1] = new Vector3(mVertexList[facePoints[1][0] - 1].Coordinates);
                    refFaces[index][2] = new Vector3(mVertexList[facePoints[2][0] - 1].Coordinates);
                    group.Faces[i].Point1[2] = group.Faces[i].Point1[0];
                    group.Faces[i].Point2[2] = group.Faces[i].Point2[0];
                    group.Faces[i].Point3[2] = group.Faces[i].Point3[0];
                    index++;
                }
            }
            var refFaceRefs = new List<int>[mVertexList.Count];
            for (var i = 0; i < mVertexList.Count; i++)
            {
                refFaceRefs[i] = new List<int>();
            }
            for (var i = 0; i < mVertexList.Count; i++)
            {
                for (var j = 0; j < refIndices.Length; j++)
                {
                    for (var l = 0; l < 3; l++)
                    {
                        if (i + 1 == refIndices[j][l])
                        {
                            refFaceRefs[i].Add(j);
                        }
                    }
                }
                if (ignoreSeams)
                {
                    for (var j = 0; j < mVertexList.Count; j++)
                    {
                        if (i != j && new Vector3(mVertexList[i].Coordinates) == new Vector3(mVertexList[j].Coordinates))
                        {
                            refFaceRefs[j].AddRange(refFaceRefs[i]);
                        }
                    }
                }
            }
            mNormalList = new List<Normal>();
            for (var i = 0; i < refFaceRefs.Length; i++)
            {
                var normal = new Vector3(0, 0, 0);
                foreach (var vertex in refFaceRefs[i])
                {
                    normal += Triangle.Normal(refFaces[vertex]);
                }
                normal.Normalize();
                mNormalList.Add(new Normal(normal.Coordinates));
            }
        }

        public void FlipUV(bool verticalFlip, bool horizontalFlip)
        {
            var uvs = new UV[UVArray.Length];
            for (var i = 0; i < UVArray.Length; i++)
            {
                uvs[i] = new UV(UVArray[i].Coordinates, verticalFlip, horizontalFlip);
            }
            UVArray = uvs;
        }

        public bool TryGetVertexIndex(int[] point, List<int[]> vertices, out int vertexIndex, bool cleanModel)
        {
            for (var i = 0; i < vertices.Count; i++)
            {
                var exists = true;
                if (cleanModel)
                {
                    for (var j = 0; j < 3 && exists; j++)
                    {
                        if (VertexArray[point[0] - 1].Coordinates[j] != VertexArray[vertices[i][0] - 1].Coordinates[j])
                        {
                            exists = false;
                        }
                    }
                    for (var j = 0; j < 2 && exists; j++)
                    {
                        if (UVArray[point[1] - 1].Coordinates[j] != UVArray[vertices[i][1] - 1].Coordinates[j])
                        {
                            exists = false;
                        }
                    }
                    for (var j = 0; j < 3 && exists; j++)
                    {
                        if (NormalArray[point[2] - 1].Coordinates[j] != NormalArray[vertices[i][2] - 1].Coordinates[j])
                        {
                            exists = false;
                        }
                    }
                }
                else
                {
                    for (var j = 0; j < vertices[i].Length && exists; j++)
                    {
                        if (point[j] != vertices[i][j])
                        {
                            exists = false;
                        }
                    }
                }
                if (exists)
                {
                    vertexIndex = i;
                    return true;
                }
            }
            vertexIndex = -1;
            return false;
        }

        public void Write(StreamWriter writer)
        {
            writer.WriteLine("# Generated by " + Array.FindLast(System.Reflection.Assembly.GetEntryAssembly().Location.Split(System.IO.Path.DirectorySeparatorChar), x => true) + " (" + Array.FindLast(System.Reflection.Assembly.GetExecutingAssembly().Location.Split(System.IO.Path.DirectorySeparatorChar), x => true) + "; code borrowed from CmarNYC's XmodsDataLib) on " + DateTime.Now.ToString("yyyy/MM/dd"));
            writer.WriteLine();
            foreach (var vertex in mVertexList)
            {
                writer.WriteLine("v " + vertex.ToString());
            }
            writer.WriteLine();
            foreach (var uv in mUVList)
            {
                writer.WriteLine("vt " + uv.ToString());
            }
            writer.WriteLine();
            foreach (var normal in mNormalList)
            {
                writer.WriteLine("vn " + normal.ToString());
            }
            writer.WriteLine();
            foreach (var group in mGroupList)
            {
                writer.WriteLine("g " + group.GroupName);
                foreach (var face in group.Faces)
                {
                    writer.WriteLine("f " + face.ToString());
                }
                writer.WriteLine();
                writer.WriteLine(string.Concat(new object[]
                    {
                        "# Group ",
                        group.GroupName,
                        " Total Faces: ",
                        group.FaceCount
                    }));
                writer.WriteLine();
            }
            writer.WriteLine("# Total groups: " + mGroupList.Count.ToString() + ", Total vertices: " + mVertexList.Count.ToString());
            writer.Flush();
        }
    }
}
