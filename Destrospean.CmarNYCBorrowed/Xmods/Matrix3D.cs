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
    public class Matrix3D
    {
        float[,] mMatrix;

        public static Matrix3D Identity
        {
            get
            {
                return new Matrix3D(new float[,]
                    {
                        {
                            1,
                            0,
                            0
                        },
                        {
                            0,
                            1,
                            0
                        },
                        {
                            0,
                            0,
                            1
                        }
                    });
            }
        }

        public float[,] Matrix
        {
            get
            {
                return new float[,]
                {
                    {
                        mMatrix[0, 0],
                        mMatrix[0, 1],
                        mMatrix[0, 2]
                    },
                    {
                        mMatrix[1, 0],
                        mMatrix[1, 1],
                        mMatrix[1, 2]
                    },
                    {
                        mMatrix[2, 0],
                        mMatrix[2, 1],
                        mMatrix[2, 2]
                    }
                };
            }
        }

        public Vector3 Scale
        {
            get
            {
                Vector3 scaleX = new Vector3(mMatrix[0, 0], mMatrix[0, 1], mMatrix[0, 2]),
                scaleY = new Vector3(mMatrix[1, 0], mMatrix[1, 1], mMatrix[1, 2]),
                scaleZ = new Vector3(mMatrix[2, 0], mMatrix[2, 1], mMatrix[2, 2]);
                return new Vector3(scaleX.Magnitude, scaleY.Magnitude, scaleZ.Magnitude);
            }
        }

        public static Matrix3D RotateYUpToZUp
        {
            get
            {
                return new Matrix3D(new float[,]
                    {
                        {
                            1,
                            0,
                            0
                        },
                        {
                            0,
                            0,
                            -1
                        },
                        {
                            0,
                            1,
                            0
                        }
                    });
            }
        }

        public static Matrix3D RotateZUpToYUp
        {
            get
            {
                return new Matrix3D(new float[,]
                    {
                        {
                            1,
                            0,
                            0
                        },
                        {
                            0,
                            0,
                            1
                        },
                        {
                            0,
                            -1,
                            0
                        }
                    });
            }
        }

        public Matrix3D()
        {
            mMatrix = new float[,]
                {
                    {
                        0,
                        0,
                        0
                    },
                    {
                        0,
                        0,
                        0
                    },
                    {
                        0,
                        0,
                        0
                    }
                };
        }

        public Matrix3D(float[,] matrix)
        {
            mMatrix = new float[,]
                {
                    {
                        matrix[0, 0],
                        matrix[0, 1],
                        matrix[0, 2]
                    },
                    {
                        matrix[1, 0],
                        matrix[1, 1],
                        matrix[1, 2]
                    },
                    {
                        matrix[2, 0],
                        matrix[2, 1],
                        matrix[2, 2]
                    }
                };
        }

        public static Vector3 operator *(Matrix3D m, Vector3 v)
        {
            float x = 0,
            y = 0,
            z = 0;
            for (var i = 0; i < 3; i++)
            {
                x += m.mMatrix[0, i] * v.Coordinates[i];
                y += m.mMatrix[1, i] * v.Coordinates[i];
                z += m.mMatrix[2, i] * v.Coordinates[i];
            }
            return new Vector3(x, y, z);
        }

        public static Matrix3D operator *(Matrix3D m, float f)
        {
            var result = new float[3, 3];
            for (var r = 0; r < 3; r++)
            {
                for (var c = 0; c < 3; c++)
                {
                    result[r, c] = m.mMatrix[r, c] * f;
                }
            }
            return new Matrix3D(result);
        }

        public static float[] operator *(Matrix3D m, float[] v)
        {
            var temp = new float[3];
            for (var i = 0; i < 3; i++)
            {
                temp[0] += m.mMatrix[0, i] * v[i];
                temp[1] += m.mMatrix[1, i] * v[i];
                temp[2] += m.mMatrix[2, i] * v[i];
            }
            return temp;
        }

        public static Matrix3D operator *(Matrix3D m1, Matrix3D m2)
        {
            var v = new float[3][];
            for (var i = 0; i < 3; i++)
            {
                v[i] = m1 * new float[]
                    {
                        m2.mMatrix[0, i],
                        m2.mMatrix[1, i],
                        m2.mMatrix[2, i]
                    };
            }
            return new Matrix3D(new float[,] {
                {
                    v[0][0],
                    v[1][0],
                    v[2][0]
                },
                {
                    v[0][1],
                    v[1][1],
                    v[2][1]
                },
                {
                    v[0][2],
                    v[1][2],
                    v[2][2]
                }
            });
        }

        public static Matrix3D FromScale(Vector3 scale)
        {
            return new Matrix3D(new float[,]
                {
                    {
                        scale.X,
                        0,
                        0
                    },
                    {
                        0,
                        scale.Y,
                        0
                    },
                    {
                        0,
                        0,
                        scale.Z
                    }
                });
        }

        public Matrix3D Inverse()
        {
            float determinant = mMatrix[0, 0] * (mMatrix[1, 1] * mMatrix[2, 2] - mMatrix[2, 1] * mMatrix[1, 2]) - mMatrix[0, 1] * (mMatrix[1, 0] * mMatrix[2, 2] - mMatrix[1, 2] * mMatrix[2, 0]) + mMatrix[0, 2] * (mMatrix[1, 0] * mMatrix[2, 1] - mMatrix[1, 1] * mMatrix[2, 0]),
            inverseDeterminant = 1 / determinant;
            var inverseMatrix = new float[3, 3];
            inverseMatrix[0, 0] = (mMatrix[1, 1] * mMatrix[2, 2] - mMatrix[2, 1] * mMatrix[1, 2]) * inverseDeterminant;
            inverseMatrix[0, 1] = (mMatrix[0, 2] * mMatrix[2, 1] - mMatrix[0, 1] * mMatrix[2, 2]) * inverseDeterminant;
            inverseMatrix[0, 2] = (mMatrix[0, 1] * mMatrix[1, 2] - mMatrix[0, 2] * mMatrix[1, 1]) * inverseDeterminant;
            inverseMatrix[1, 0] = (mMatrix[1, 2] * mMatrix[2, 0] - mMatrix[1, 0] * mMatrix[2, 2]) * inverseDeterminant;
            inverseMatrix[1, 1] = (mMatrix[0, 0] * mMatrix[2, 2] - mMatrix[0, 2] * mMatrix[2, 0]) * inverseDeterminant;
            inverseMatrix[1, 2] = (mMatrix[1, 0] * mMatrix[0, 2] - mMatrix[0, 0] * mMatrix[1, 2]) * inverseDeterminant;
            inverseMatrix[2, 0] = (mMatrix[1, 0] * mMatrix[2, 1] - mMatrix[2, 0] * mMatrix[1, 1]) * inverseDeterminant;
            inverseMatrix[2, 1] = (mMatrix[2, 0] * mMatrix[0, 1] - mMatrix[0, 0] * mMatrix[2, 1]) * inverseDeterminant;
            inverseMatrix[2, 2] = (mMatrix[0, 0] * mMatrix[1, 1] - mMatrix[1, 0] * mMatrix[0, 1]) * inverseDeterminant;
            return new Matrix3D(inverseMatrix);
        }

        public override string ToString()
        {
            var text = "";
            for (var r = 0; r < 3; r++)
            {
                for (var c = 0; c < 3; c++)
                {
                    text += mMatrix[r, c].ToString();
                    if (c != 2 || r != 2)
                    {
                        text += ", ";
                    }
                }
                text += Environment.NewLine;
            }
            return text;
        }

        public Matrix3D Transpose()
        {
            return new Matrix3D(new float[,]
                {
                    {
                        mMatrix[0, 0],
                        mMatrix[1, 0],
                        mMatrix[2, 0]
                    },
                    {
                        mMatrix[0, 1],
                        mMatrix[1, 1],
                        mMatrix[2, 1]
                    },
                    {
                        mMatrix[0, 2],
                        mMatrix[1, 2],
                        mMatrix[2, 2]
                    }
                });
        }
    }
}
