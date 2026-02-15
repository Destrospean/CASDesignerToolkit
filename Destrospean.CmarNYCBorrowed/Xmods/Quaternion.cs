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
    public class Quaternion
    {
        float mW, mX, mY, mZ;

        public float[] Coordinates
        {
            get
            {
                return new float[]
                {
                    mX,
                    mY,
                    mZ,
                    mW
                };
            }
        }

        public static Quaternion Identity
        {
            get
            {
                return new Quaternion(0, 0, 0, 1);
            }
        }

        public bool IsEmpty
        {
            get
            {
                return mX == 0 && mY == 0 && mZ == 0 && (mW == 1 || mW == 0);
            }
        }

        public bool IsIdentity
        {
            get
            {
                return mX == 0 && mY == 0 && mZ == 0 && mW == 1;
            }
        }

        public bool IsNormalized
        {
            get
            {
                double magnitude = mX * mX + mY * mY + mZ * mZ + mW * mW;
                return (Math.Abs(magnitude - 1) <= .000001);
            }
        }

        public static Quaternion RotateYUpToZUp
        {
            get
            {
                return new Quaternion(.7071068f, 0, 0, .7071068f);
            }
        }

        public float W
        {
            get
            {
                return mW;
            }
        }

        public float X
        {
            get
            {
                return mX;
            }
        }

        public float Y
        {
            get
            {
                return mY;
            }
        }

        public float Z
        {
            get
            {
                return mZ;
            }
        }

        public Quaternion(float x, float y, float z, float w)
        {
            mX = x;
            mY = y;
            mZ = z;
            mW = w;
        }

        public Quaternion(float[] quat)
        {
            mX = quat[0];
            mY = quat[1];
            mZ = quat[2];
            mW = quat[3];
        }

        public static Quaternion operator *(Quaternion q, Quaternion r)
        {
            return new Quaternion(r.mW * q.mX + r.mX * q.mW - r.mY * q.mZ + r.mZ * q.mY, r.mW * q.mY + r.mX * q.mZ + r.mY * q.mW - r.mZ * q.mX, r.mW * q.mZ - r.mX * q.mY + r.mY * q.mX + r.mZ * q.mW, r.mW * q.mW - r.mX * q.mX - r.mY * q.mY - r.mZ * q.mZ);
        }

        public static Quaternion operator *(Quaternion q, float f)
        {
            var temp = new Quaternion(q.mX * f, q.mY * f, q.mZ * f, q.mW);
            temp.Normalize();
            return temp;
        }

        public static Quaternion operator *(Quaternion q, Vector3 v)
        {
            var temp = new Quaternion(v.X, v.Y, v.Z, 0);
            return q * temp;
        }

        public void Balance()
        {
            double m = mX * mX - mY * mY - mZ * mZ;
            if (m <= 1)
            {
                mW = (float)Math.Sqrt(1 - m);
            }
            else
            {
                Normalize();
            }
        }

        public Quaternion Conjugate()
        {
            return new Quaternion(-mX, -mY, -mZ, mW);
        }

        public Quaternion Inverse()
        {
            var norm = mX * mX + mY * mY + mZ * mZ + mW * mW;
            if (norm > 0)
            {
                var q = new Quaternion(-mX / norm, -mY / norm, -mZ / norm, mW / norm);
                q.Normalize();
                return q;
            }
            else
            {
                return Quaternion.Identity;
            }
        }

        public void Normalize()
        {
            var magnitude = Math.Sqrt(mX * mX + mY * mY + mZ * mZ + mW * mW);
            mX = (float)(mX / magnitude);
            mY = (float)(mY / magnitude);
            mZ = (float)(mZ / magnitude);
            mW = (float)(mW / magnitude);
        }

        public Euler ToEuler()
        {
            var result = new float[3];
            result[0] = (float)Math.Atan2(2 * (mY * mZ + mW * mX), mW * mW - mX * mX - mY * mY + mZ * mZ);
            var r21 = -2d * (mX * mZ - mW * mY);
            if (r21 < -1)
            {
                r21 = -1;
            }
            if (r21 > 1)
            {
                r21 = 1;
            }
            result[1] = (float)Math.Asin(r21);
            result[2] = (float)Math.Atan2(2 * (mX * mY + mW * mZ), mW * mW + mX * mX - mY * mY - mZ * mZ);
            return new Euler(result[0], result[1], result[2]);
        }

        public Matrix3D ToMatrix3D()
        {
            var matrix = new float[3, 3];
            matrix[0, 0] = 1 - 2 * mY * mY - 2 * mZ * mZ;
            matrix[0, 1] = 2 * mX * mY - 2 * mZ * mW;
            matrix[0, 2] = 2 * mX * mZ + 2 * mY * mW;
            matrix[1, 0] = 2 * mX * mY + 2 * mZ * mW;
            matrix[1, 1] = 1 - 2 * mX * mX - 2 * mZ * mZ;
            matrix[1, 2] = 2 * mY * mZ - 2 * mX * mW;
            matrix[2, 0] = 2 * mX * mZ - 2 * mY * mW;
            matrix[2, 1] = 2 * mY * mZ + 2 * mX * mW;
            matrix[2, 2] = 1 - 2 * mX * mX - 2 * mY * mY;
            return new Matrix3D(matrix);
        }

        public Matrix4D ToMatrix4D()
        {
            return ToMatrix4D(new Vector3(0, 0, 0));
        }

        public Matrix4D ToMatrix4D(Vector3 offset)
        {
            var matrix = new double[4, 4];
            matrix[0, 0] = 1 - 2d * mY * mY - 2d * mZ * mZ;
            matrix[0, 1] = 2d * mX * mY - 2d * mZ * mW;
            matrix[0, 2] = 2d * mX * mZ + 2d * mY * mW;
            matrix[0, 3] = offset.X;
            matrix[1, 0] = 2d * mX * mY + 2d * mZ * mW;
            matrix[1, 1] = 1 - 2d * mX * mX - 2d * mZ * mZ;
            matrix[1, 2] = 2d * mY * mZ - 2d * mX * mW;
            matrix[1, 3] = offset.Y;
            matrix[2, 0] = 2d * mX * mZ - 2d * mY * mW;
            matrix[2, 1] = 2d * mY * mZ + 2d * mX * mW;
            matrix[2, 2] = 1 - 2d * mX * mX - 2d * mY * mY;
            matrix[2, 3] = offset.Z;
            matrix[3, 0] = 0;
            matrix[3, 1] = 0;
            matrix[3, 2] = 0;
            matrix[3, 3] = 1;
            return new Matrix4D(matrix);
        }

        public Matrix4D ToMatrix4D(Vector3 offset, Vector3 scale)
        {
            var matrix = new double[4, 4];
            matrix[0, 0] = scale.X - 2d * mY * mY - 2d * mZ * mZ;
            matrix[0, 1] = 2d * mX * mY - 2d * mZ * mW;
            matrix[0, 2] = 2d * mX * mZ + 2d * mY * mW;
            matrix[0, 3] = offset.X;
            matrix[1, 0] = 2d * mX * mY + 2d * mZ * mW;
            matrix[1, 1] = scale.Y - 2d * mX * mX - 2d * mZ * mZ;
            matrix[1, 2] = 2d * mY * mZ - 2d * mX * mW;
            matrix[1, 3] = offset.Y;
            matrix[2, 0] = 2d * mX * mZ - 2d * mY * mW;
            matrix[2, 1] = 2d * mY * mZ + 2d * mX * mW;
            matrix[2, 2] = scale.Z - 2d * mX * mX - 2d * mY * mY;
            matrix[2, 3] = offset.Z;
            matrix[3, 0] = 0;
            matrix[3, 1] = 0;
            matrix[3, 2] = 0;
            matrix[3, 3] = 1;
            return new Matrix4D(matrix);
        }

        public override string ToString()
        {
            return mX.ToString() + ", " + mY.ToString() + ", " + mZ.ToString() + ", " + mW.ToString();
        }

        public string ToString(string format)
        {
            return mX.ToString(format) + ", " + mY.ToString(format) + ", " + mZ.ToString(format) + ", " + mW.ToString(format);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(mX, mY, mZ);
        }
    }
}
