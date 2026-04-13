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
    public class Euler
    {
        float mX, mY, mZ;

        public float[] Rotation
        {
            get
            {
                return new[]
                {
                    mX,
                    mY,
                    mZ
                };
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

        public Euler(float x, float y, float z)
        {
            mX = x;
            mY = y;
            mZ = z;
        }
    }
}
