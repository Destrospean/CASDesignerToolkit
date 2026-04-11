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
    public struct Vector2
    {
        public float[] Coordinates
        {
            get
            {
                return new[]
                {
                    X,
                    Y
                };
            }
            set
            {
                X = value[0];
                Y = value[1];
            }
        }

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        public float X, Y;

        public Vector2(float[] coordinates)
        {
            X = coordinates[0];
            Y = coordinates[1];
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(Vector2 vector)
        {
            X = vector.X;
            Y = vector.Y;
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            var epsilon = .00005f;
            return Math.Abs(v1.X - v2.X) < epsilon && Math.Abs(v1.Y - v2.Y) < epsilon;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1 == v2);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator *(Vector2 v1, float m2)
        {
            return new Vector2(v1.X * m2, v1.Y * m2);
        }

        public static Vector2 operator *(float m1, Vector2 v2)
        {
            return v2 * m1;
        }

        public float Distance(Vector2 other)
        {
            return (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        public float DistanceFromLine(Vector2 p1, Vector2 p2)
        {
            return Math.Abs(((p2.Y - p1.Y) * X) - ((p2.X - p1.X) * Y) + (p2.X * p1.Y) - (p2.Y * p1.X)) / p1.Distance(p2);
        }

        public bool DistanceFromLineRestricted(Vector2 p1, Vector2 p2, out float distance, out int endpointIndex)
        {
            Vector2 line = p2 - p1,
            temp = this - p1;
            float lineLenSq = (line.X * line.X) + (line.Y * line.Y),
            distanceOnSegment = Vector2.Dot(temp, line) / lineLenSq;
            endpointIndex = 0;
            if (distanceOnSegment >= 0 && distanceOnSegment <= 1)
            {
                Vector2 projectedPoint = p1 + (line * distanceOnSegment);
                distance = Distance(projectedPoint);
                return true;
            }
            else
            {
                if (distanceOnSegment > 1)
                {
                    endpointIndex = 1;
                }
                distance = Distance(endpointIndex == 0 ? p1 : p2);
                return false;
            }
        }

        public float Dot(Vector2 other)
        {
            return Dot(this, other);
        }

        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 ? (Vector2)obj == this : false;
        }

        public bool Equals(Vector2 obj)
        {
            return obj == this;
        }

        public override int GetHashCode()
        {
            return (int)((X + Y) % Int32.MaxValue);
        }

        public static Vector2 Normalize(Vector2 vector)
        {
            if (vector.Magnitude == 0)
            {
                throw new DivideByZeroException("Cannot normalize a vector with magnitude of zero!");
            }
            else
            {
                var inverse = (float)(1 / vector.Magnitude);
                return new Vector2(vector.X * inverse, vector.Y * inverse);
            }
        }

        public bool ProjectsWithinLine(Vector2 v1, Vector2 v2)
        {
            bool temp;
            ProjectToLine(v1, v2, out temp);
            return temp;
        }

        public Vector2 ProjectToLine(Vector2 a, Vector2 b, out bool withinLine)
        {
            Vector2 ab = b - a,
            ap = this - a;
            float magnitudeAB = (ab.X * ab.X) + (ab.Y * ab.Y),
            distance = Vector2.Dot(ap, ab) / magnitudeAB;
            withinLine = distance >= 0 && distance <= 1;
            return a + ab * distance;
        }

        public override string ToString()
        {
            return X.ToString() + ", " + Y.ToString();
        }
    }
}
