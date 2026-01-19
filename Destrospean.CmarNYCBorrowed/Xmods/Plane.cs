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

namespace Destrospean.CmarNYCBorrowed
{
    public struct Plane
    {
        public float A, B, C, D;

        public Plane(Triangle triangle)
        {
            this = new Plane(triangle.Point1, triangle.Point2, triangle.Point3);
        }

        public Plane(Vector3 v1, Vector3 v2)
        {
            var vector = Vector3.Cross(v2, v1);
            A = vector.X;
            B = vector.Y;
            C = vector.Z;
            D = -(A * v1.X + B * v1.Y + C * v1.Z);
        }

        public Plane(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            A = v1.Y * (v2.Z - v3.Z) + v2.Y * (v3.Z - v1.Z) + v3.Y * (v1.Z - v2.Z);
            B = v1.Z * (v2.X - v3.X) + v2.Z * (v3.X - v1.X) + v3.Z * (v1.X - v2.X);
            C = v1.X * (v2.Y - v3.Y) + v2.X * (v3.Y - v1.Y) + v3.X * (v1.Y - v2.Y);
            D = -(v1.X * (v2.Y * v3.Z - v3.Y * v2.Z) + v2.X * (v3.Y * v1.Z - v1.Y * v3.Z) + v3.X * (v1.Y * v2.Z - v2.Y * v1.Z));
        }

        public float Distance(Vector3 point)
        {
            return Plane.Distance(point, this);
        }

        public static float Distance(Vector3 point, Plane plane)
        {
            return (float)System.Math.Abs((double)(plane.A * point.X + plane.B * point.Y + plane.C * point.Z + plane.D) / System.Math.Sqrt((double)(plane.A * plane.A + plane.B * plane.B + plane.C * plane.C)));
        }

        public float Side(Vector3 vector)
        {
            return Plane.Side(vector, this);
        }

        public static float Side(Vector3 point, Plane plane)
        {
            return plane.A * point.X + plane.B * point.Y + plane.C * point.Z + plane.D;
        }

        public override string ToString()
        {
            return string.Concat(new string[]
                {
                    A.ToString(),
                    "x + ",
                    B.ToString(),
                    "y + ",
                    C.ToString(),
                    "z + ",
                    D.ToString(),
                    " = 0"
                });
        }
    }
}
