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
    public class AxisAngle
    {
        double mAngle, mX, mY, mZ;

        public double Angle
        {
            get
            {
                return mAngle;
            }
        }

        public Vector3 Axis
        {
            get
            {
                return new Vector3((float)mX, (float)mY, (float)mZ);
            }
        }

        public double X
        {
            get
            {
                return mX;
            }
        }

        public double Y
        {
            get
            {
                return mY;
            }
        }

        public double Z
        {
            get
            {
                return mZ;
            }
        }

        public AxisAngle(float angle, float[] axis)
        {
            mX = axis[0];
            mY = axis[1];
            mZ = axis[2];
            mAngle = angle;
        }

        public AxisAngle(float angle, Vector3 axis)
        {
            mX = axis.X;
            mY = axis.Y;
            mZ = axis.Z;
            mAngle = angle;
        }

        public AxisAngle(double angle, double x, double y, double z)
        {
            mX = x;
            mY = y;
            mZ = z;
            mAngle = angle;
        }

        public AxisAngle(double[] values)
        {
            mX = values[0];
            mY = values[1];
            mZ = values[2];
            mAngle = values[3];
        }

        public AxisAngle(float[] values)
        {
            mX = values[0];
            mY = values[1];
            mZ = values[2];
            mAngle = values[3];
        }

        public void Normalize()
        {
            var magnitude = Math.Sqrt(mX * mX + mY * mY + mZ * mZ);
            if (magnitude == 0)
            {
                throw new ApplicationException("Cannot normalize AxisAngle: " + mX.ToString() + " " + mY.ToString() + " " + mZ.ToString());
            }
            mX /= magnitude;
            mY /= magnitude;
            mZ /= magnitude;
        }

        public Matrix3D ToMatrix()
        {
            double c = Math.Cos(mAngle),
            s = Math.Sin(mAngle),
            t = 1 - c,
            temp0 = mX * mY * t,
            temp1 = mZ * s;
            Normalize();
            var m = new float[3, 3];
            m[0, 0] = (float)(c + mX * mX * t);
            m[1, 1] = (float)(c + mY * mY * t);
            m[2, 2] = (float)(c + mZ * mZ * t);
            m[1, 0] = (float)(temp0 + temp1);
            m[0, 1] = (float)(temp0 - temp1);
            temp0 = mX * mZ * t;
            temp1 = mY * s;
            m[2, 0] = (float)(temp0 - temp1);
            m[0, 2] = (float)(temp0 + temp1);
            temp0 = mY * mZ * t;
            temp1 = mX * s;
            m[2, 1] = (float)(temp0 + temp1);
            m[1, 2] = (float)(temp0 - temp1);
            return new Matrix3D(m);
        }
    }
}
