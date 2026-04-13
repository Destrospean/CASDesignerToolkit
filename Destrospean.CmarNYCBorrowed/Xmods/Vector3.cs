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

namespace Destrospean.CmarNYCBorrowed
{
    public struct Vector3 : IEquatable<Vector3>
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
            set
            {
                X = value[0];
                Y = value[1];
                Z = value[2];
            }
        }

        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public float X, Y, Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(float[] coordinates)
        {
            X = coordinates[0];
            Y = coordinates[1];
            Z = coordinates[2];
        }

        public Vector3(Vector3 vector)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return AlmostEquals(v1.X, v2.X) && AlmostEquals(v1.Y, v2.Y) && AlmostEquals(v1.Z, v2.Z);
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator *(Vector3 v1, float s2)
        {
            return new Vector3(v1.X * s2, v1.Y * s2, v1.Z * s2);
        }

        public static Vector3 operator *(float s1, Vector3 v2)
        {
            return v2 * s1;
        }

        public static Vector3 operator /(Vector3 v1, float s2)
        {
            return new Vector3(v1.X / s2, v1.Y / s2, v1.Z / s2);
        }

        public static Vector3 AbsoluteValue(Vector3 vector)
        {
            return new Vector3(Math.Abs(vector.X), Math.Abs(vector.Y), Math.Abs(vector.Z));
        }

        public static bool AlmostEquals(float f1, float f2)
        {
            return Math.Abs(f1 - f2) < .00005f;
        }

        public float Angle(Vector3 other)
        {
            return Angle(this, other);
        }

        public static float Angle(Vector3 v1, Vector3 v2)
        {
            return (float)Math.Acos(Normalize(v1).Dot(Normalize(v2)));
        }

        public int ArrayMinimumIndex(float[] array)
        {
            int tempIndex = -1;
            var tempValue = float.MaxValue;
            for (var i = 0; i < array.Length; i++)
            {
                if (array[i] < tempValue)
                {
                    tempIndex = i;
                    tempValue = array[i];
                }
            }
            return tempIndex;
        }

        public bool Between(Vector3 p1, Vector3 p2)
        {
            float max = Math.Max(p1.X, p2.X),
            min = Math.Min(p1.X, p2.X);
            if (min > X || X > max)
            {
                return false;
            }
            min = Math.Min(p1.Y, p2.Y);
            max = Math.Max(p1.Y, p2.Y);
            if (min > Y || Y > max)
            {
                return false;
            }
            min = Math.Min(p1.Z, p2.Z);
            max = Math.Max(p1.Z, p2.Z);
            if (min > Z || Z > max)
            {
                return false;
            }
            return true;
        }

        public static Vector3 Centroid(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            return new Vector3((p1.X + p2.X + p3.X) / 3f, (p1.Y + p2.Y + p3.Y) / 3f, (p1.Z + p2.Z + p3.Z) / 3f);
        }

        public Vector3 Cross(Vector3 other)
        {
            return Cross(this, other);
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }

        public float Distance(Vector3 other)
        {
            return Distance(this, other);
        }

        public static float Distance(Vector3 v1, Vector3 v2)
        {
            return (float)Math.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y) + (v1.Z - v2.Z) * (v1.Z - v2.Z));
        }

        public float Dot(Vector3 other)
        {
            return Dot(this, other);
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3 ? (Vector3)obj == this : false;
        }

        public bool Equals(Vector3 obj)
        {
            return obj == this;
        }

        public int[] GetFaceReferenceMeshPoints(Vector3[] refMeshPositions, int[][] refMeshFaces, int[][] refFaceRefs, Triangle[] currentVertFaces, int numberOfPoints)
        {
            var indices = new int[numberOfPoints];
            for (var i = 0; i < numberOfPoints; i++)
            {
                indices[i] = -1;
            }
            float[] distance = new float[numberOfPoints],
            refMeshDistances = new float[refMeshPositions.Length];
            for (var i = 0; i < refMeshPositions.Length; i++)
            {
                refMeshDistances[i] = Distance(refMeshPositions[i]);
            }
            var closestVertex = ArrayMinimumIndex(refMeshDistances);
            var closestDistance = refMeshDistances[closestVertex];
            var duplicateVertices = new List<int>();
            for (var i = 0; i < refMeshPositions.Length; i++)
            {
                if (refMeshPositions[i] == refMeshPositions[closestVertex])
                {
                    duplicateVertices.Add(i);
                }
            }
            if (duplicateVertices.Count > 1)
            {
                var centroid = default(Vector3);
                for (var i = 0; i < currentVertFaces.Length; i++)
                {
                    var triangle = currentVertFaces[i];
                    centroid += triangle.Centroid();
                }
                centroid /= (float)currentVertFaces.Length;
                var refCentroid = new Vector3[duplicateVertices.Count];
                var centroidDistance = new float[duplicateVertices.Count];
                for (var i = 0; i < duplicateVertices.Count; i++)
                {
                    var faces = refFaceRefs[duplicateVertices[i]];
                    for (var j = 0; j < faces.Length; j++)
                    {
                        refCentroid[i] += Vector3.Centroid(refMeshPositions[refMeshFaces[faces[j]][0]], refMeshPositions[refMeshFaces[faces[j]][1]], refMeshPositions[refMeshFaces[faces[j]][2]]);
                    }
                    refCentroid[i] /= (float)refFaceRefs[duplicateVertices[i]].Length;
                    centroidDistance[i] = centroid.Distance(refCentroid[i]);
                }
                closestVertex = duplicateVertices[ArrayMinimumIndex(centroidDistance)];
            }
            indices[0] = closestVertex;
            distance[0] = closestDistance;
            var verticesLinkedByFaces = new List<int>();
            var linkedFaces = refFaceRefs[closestVertex];
            foreach (var i in linkedFaces)
            {
                foreach (var j in refMeshFaces[i])
                {
                    if (!verticesLinkedByFaces.Contains(j))
                    {
                        verticesLinkedByFaces.Add(j);
                    }
                }
            }
            foreach (var i in verticesLinkedByFaces)
            {
                for (var j = 0; j < numberOfPoints; j++)
                {
                    if (refMeshDistances[i] <= distance[j])
                    {
                        for (var k = j; k < numberOfPoints - 1; k++)
                        {
                            indices[k + 1] = indices[k];
                            distance[k + 1] = distance[k];
                        }
                        indices[j] = i;
                        distance[j] = refMeshDistances[i];
                    }
                }
            }
            return indices;
        }

        public override int GetHashCode()
        {
            return (int)((X + Y + Z) % Int32.MaxValue);
        }

        public float[] GetInterpolationWeights(Vector3[] points, float weightingFactor)
        {
            var weights = new float[points.Length];
            if (points.Length == 1)
            {
                weights[0] = 1;
                return weights;
            }
            for (var i = 0; i < points.Length; i++)
            {
                if (Vector3.Distance(points[i], this) == 0)
                {
                    weights[i] = 1;
                    return weights;
                }
            }
            var d = new float[points.Length];
            var dt = 0f;
            for (var i = 0; i < points.Length; i++)
            {
                d[i] = 1 / (float)Math.Pow(Vector3.Distance(points[i], this), weightingFactor);
                dt += d[i];
            }
            for (var i = 0; i < points.Length; i++)
            {
                weights[i] = d[i] / dt;
            }
            return weights;
        }

        public int[] GetReferenceMeshPoints(Vector3[] refMeshPositions, int[][] refMeshFaces, bool interpolate, bool restrictToNearestFace, int numberOfPoints = 3)
        {
            if (restrictToNearestFace)
            {
                var distance = 99999999f;
                var indices = new List<int>(1);
                for (var i = 0; i < refMeshPositions.Length; i++)
                {
                    var temp = Distance(refMeshPositions[i]);
                    if (temp < distance)
                    {
                        distance = temp;
                        indices.Clear();
                        indices.Add(i);
                    }
                    else if (temp == distance)
                    {
                        indices.Add(i);
                    }
                }
                if (indices.Count == 0)
                {
                    throw new ApplicationException("No vertices in reference mesh!");
                }
                if (interpolate)
                {
                    var faceDistance = 99999999f;
                    var found = false;
                    var refFace = 0;
                    foreach (var i in indices)
                    {
                        for (var j = 0; j < refMeshFaces.GetLength(0); j++)
                        {
                            var foundTemp = false;
                            for (var k = 0; k < 3; k++)
                            {
                                if (refMeshFaces[j][k] == i)
                                {
                                    foundTemp = true;
                                    continue;
                                }
                            }
                            if (foundTemp)
                            {
                                var tempTriangle = new Triangle(refMeshPositions[refMeshFaces[j][0]], refMeshPositions[refMeshFaces[j][1]], refMeshPositions[refMeshFaces[j][2]]);
                                var tempPlane = new Plane(tempTriangle);
                                if (tempTriangle.PointInside(ProjectToPlane(tempPlane)) && Distance(ProjectToPlane(tempPlane)) < faceDistance)
                                {
                                    found = true;
                                    refFace = j;
                                    faceDistance = Distance(ProjectToPlane(tempPlane));
                                }
                            }
                        }
                    }
                    if (found)
                    {
                        return refMeshFaces[refFace];
                    }
                    else
                    {
                        int index0 = indices[0],
                        index1 = indices[0];
                        foreach (var i in indices)
                        {
                            var refVertex = new Vector3(refMeshPositions[i]);
                            var lineDistance = 99999999f;
                            for (var j = 0; j < refMeshFaces.GetLength(0); j++)
                            {
                                for (var k = 0; k < 3; k++)
                                {
                                    if (refMeshFaces[j][k] == i)
                                    {
                                        for (var l = 0; l < 3; l++)
                                        {
                                            if (k != l)
                                            {
                                                var temp = new Vector3(refMeshPositions[refMeshFaces[j][l]]);
                                                try
                                                {
                                                    if (ProjectToLine(refVertex, temp).Between(refVertex, temp) && Distance(ProjectToLine(refVertex, temp)) < lineDistance)
                                                    {
                                                        index0 = i;
                                                        index1 = refMeshFaces[j][l];
                                                        lineDistance = Distance(ProjectToLine(refVertex, temp));
                                                    }
                                                }
                                                catch
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                    continue;
                                }
                            }
                        }
                        if (index0 != index1)
                        {
                            return new[]
                            {
                                index0,
                                index1
                            };
                        }
                        else
                        {
                            return new[]
                            {
                                indices[0]
                            };
                        }
                    }
                }
                else
                {
                    return new[]
                    {
                        indices[0]
                    };
                }
            }
            else
            {
                var indices = new int[numberOfPoints];
                var distance = new float[numberOfPoints];
                var position = new Vector3[numberOfPoints];
                for (var i = 0; i < numberOfPoints; i++)
                {
                    distance[i] = 999999f;
                }
                for (var i = 0; i < refMeshPositions.Length; i++)
                {
                    if (this == refMeshPositions[i])
                    {
                        return new[]
                        {
                            i
                        };
                    }
                    var temp = Distance(refMeshPositions[i]);
                    for (var j = 0; j < numberOfPoints; j++)
                    {
                        if (temp <= distance[j] && position[j] != refMeshPositions[i])
                        {
                            for (var k = numberOfPoints-2; k >= j; k--)
                            {
                                indices[k + 1] = indices[k];
                                distance[k + 1] = distance[k];
                                position[k + 1] = position[k];
                            }
                            indices[j] = i;
                            distance[j] = temp;
                            position[j] = refMeshPositions[i];
                            break;
                        }
                    }
                }
                return indices;
            }
        }

        public Vector3 NearestPoint(Vector3 thisFacesCentroid, Vector3[] refPointsArray, Vector3[] refPointsFacesCentroids)
        {
            return refPointsArray[NearestPointIndex(thisFacesCentroid, refPointsArray, refPointsFacesCentroids)];
        }

        public int NearestPointIndex(Vector3 thisFacesCentroid, Vector3[] refPointsArray, Vector3[] refPointsFacesCentroids)
        {
            float minDistance = float.MaxValue,
            minFaceDistance = float.MaxValue;
            var workingIndices = new List<int>();
            for (var i = 0; i < refPointsArray.Length; i++)
            {
                if (Distance(refPointsArray[i]) < minDistance)
                {
                    workingIndices.Clear();
                    workingIndices.Add(i);
                    minDistance = Distance(refPointsArray[i]);
                }
                else if (Distance(refPointsArray[i]) == minDistance)
                {
                    workingIndices.Add(i);
                }
            }
            var index = 0;
            for (var i = 0; i < workingIndices.Count; i++)
            {
                if (thisFacesCentroid.Distance(refPointsFacesCentroids[workingIndices[i]]) < minFaceDistance)
                {
                    index = workingIndices[i];
                    minFaceDistance = thisFacesCentroid.Distance(refPointsFacesCentroids[workingIndices[i]]);
                }
            }
            return index;
        }

        public int NearestPointIndexSimple(Vector3[] RefPointsArray)
        {
            var index = 0;
            var minDistance = float.MaxValue;
            for (var i = 0; i < RefPointsArray.Length; i++)
            {
                if (Distance(RefPointsArray[i]) < minDistance)
                {
                    minDistance = Distance(RefPointsArray[i]);
                    index = i;
                }
            }
            return index;
        }

        public Vector3 NearestPointSimple(Vector3[] refPointsArray)
        {
            return refPointsArray[NearestPointIndexSimple(refPointsArray)];
        }

        public void Normalize()
        {
            var n = Normalize(this);
            X = n.X;
            Y = n.Y;
            Z = n.Z;
        }

        public static Vector3 Normalize(Vector3 vector)
        {
            if (vector.Magnitude == 0)
            {
                return vector;
            }
            else
            {
                var inverse = 1 / vector.Magnitude;
                return new Vector3(vector.X * inverse, vector.Y * inverse, vector.Z * inverse);
            }
        }

        public static Vector3 Parse(string coordinateString)
        {
            var coordinatesAsStrings = coordinateString.Split(',');
            if (coordinatesAsStrings.Length != 3)
            {
                throw new FormatException("Input not in correct format");
            }
            var coordinates = new float[3];
            for (var i = 0; i < 3; i++)
            {
                if (!float.TryParse(coordinatesAsStrings[i], NumberStyles.Float, CultureInfo.InvariantCulture, out coordinates[i]))
                {
                    throw new FormatException("Input not in correct format");
                }
            }
            return new Vector3(coordinates);
        }

        public bool PositionClose(float[] other)
        {
            return PositionClose(new Vector3(other));
        }

        public bool PositionClose(Vector3 other)
        {
            var epsilon = .005f;
            return Math.Abs(X - other.X) < epsilon && Math.Abs(Y - other.Y) < epsilon && Math.Abs(Z - other.Z) < epsilon;
        }

        public bool PositionMatches(float[] other)
        {
            return PositionMatches(new Vector3(other));
        }

        public bool PositionMatches(Vector3 other)
        {
            var epsilon = .0005f;
            return Math.Abs(X - other.X) < epsilon && Math.Abs(Y - other.Y) < epsilon && Math.Abs(Z - other.Z) < epsilon;
        }

        public bool PositionMatches(float x, float y, float z)
        {
            return PositionMatches(new Vector3(x, y, z));
        }

        public Vector3 ProjectToLine(Vector3 p1, Vector3 p2)
        {
            Vector3 temp0 = p2 - p1,
            temp1 = this - p1;
            temp0.Normalize();
            return new Vector3(p1 + Vector3.Dot(temp0, temp1) * temp0);
        }

        public Vector3 ProjectToPlane(Plane plane)
        {
            var num = (plane.A + X + plane.B * Y + plane.C * Z + plane.D) / (plane.A * plane.A + plane.B * plane.B + plane.C * plane.C);
            return new Vector3(X + plane.A * num, Y + plane.B * num, Z + plane.C * num);
        }

        public Vector3 Scale(Vector3 other)
        {
            return Scale(this, other);
        }

        public static Vector3 Scale(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public override string ToString()
        {
            return X.ToString() + ", " + Y.ToString() + ", " + Z.ToString();
        }
    }
}
