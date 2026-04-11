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

namespace Destrospean.CmarNYCBorrowed
{
    public struct Triangle
    {
        public Vector3 Point1, Point2, Point3;

        public Vector3[] TrianglePoints
        {
            get
            {
                return new[]
                {
                    Point1,
                    Point2,
                    Point3
                };
            }
        }
            
        public Triangle(float[] p1, float[] p2, float[] p3)
        {
            Point1 = new Vector3(p1);
            Point2 = new Vector3(p2);
            Point3 = new Vector3(p3);
        }

        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Point1 = p1;
            Point2 = p2;
            Point3 = p3;
        }

        public Triangle(Vector3[] points)
        {
            Point1 = points[0];
            Point2 = points[1];
            Point3 = points[2];
        }

        static bool SameSide(Vector3 p1, Vector3 p2, Vector3 facePointA, Vector3 facePointB)
        {
            Vector3 v0 = Vector3.Cross(facePointB - facePointA, p1 - facePointA),
            v1 = Vector3.Cross(facePointB - facePointA, p2 - facePointA);
            return Vector3.Dot(v0, v1) >= 0;
        }

        public Vector3 Centroid()
        {
            return Triangle.Centroid(this);
        }

        public static Vector3 Centroid(Triangle triangle)
        {
            return new Vector3((triangle.Point1.X + triangle.Point2.X + triangle.Point3.X) / 3, (triangle.Point1.Y + triangle.Point2.Y + triangle.Point3.Y) / 3, (triangle.Point1.Z + triangle.Point2.Z + triangle.Point3.Z) / 3);
        }

        public Vector3 Normal()
        {
            return Triangle.Normal(this);
        }

        public static Vector3 Normal(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            return Triangle.Normal(new Triangle(p1, p2, p3));
        }

        public static Vector3 Normal(Vector3[] points)
        {
            return Triangle.Normal(new Triangle(points));
        }

        public static Vector3 Normal(Triangle face)
        {
            return Vector3.Normalize(Vector3.Cross(face.Point2 - face.Point1, face.Point3 - face.Point1));
        }

        public static bool PointInside(Vector3 facePoint1, Vector3 facePoint2, Vector3 facePoint3, Vector3 point)
        {
            return Triangle.PointInside(new Triangle(facePoint1, facePoint2, facePoint3), point);
        }

        public bool PointInside(Vector3 point)
        {
            return Triangle.PointInside(this, point);
        }

        public static bool PointInside(Triangle triangle, Vector3 point)
        {
            return Triangle.SameSide(point, triangle.Point1, triangle.Point2, triangle.Point3) & Triangle.SameSide(point, triangle.Point2, triangle.Point1, triangle.Point3) & Triangle.SameSide(point, triangle.Point3, triangle.Point1, triangle.Point2);
        }

        public override string ToString()
        {
            return string.Concat(new[]
                {
                    "(",
                    Point1.ToString(),
                    "), (",
                    Point2.ToString(),
                    "), (",
                    Point3.ToString(),
                    ")"
                });
        }
    }
}
