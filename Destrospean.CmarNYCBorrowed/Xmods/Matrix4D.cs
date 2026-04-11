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
    public struct Matrix4D
    {
        double[,] mMatrix;

        public static Matrix4D Identity
        {
            get
            {
                return new Matrix4D(new double[,]
                    {
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        {
                            0,
                            1,
                            0,
                            0
                        },
                        {
                            0,
                            0,
                            1,
                            0
                        },
                        {
                            0,
                            0,
                            0,
                            1
                        }
                    });
            }
        }

        public double[,] Matrix
        {
            get
            {
                return new double[,]
                {
                    {
                        mMatrix[0, 0],
                        mMatrix[0, 1],
                        mMatrix[0, 2],
                        mMatrix[0, 3]
                    },
                    {
                        mMatrix[1, 0],
                        mMatrix[1, 1],
                        mMatrix[1, 2],
                        mMatrix[1, 3]
                    },
                    {
                        mMatrix[2, 0],
                        mMatrix[2, 1],
                        mMatrix[2, 2],
                        mMatrix[2, 3]
                    },
                    {
                        mMatrix[3, 0],
                        mMatrix[3, 1],
                        mMatrix[3, 2],
                        mMatrix[3, 3]
                    }
                };
            }
        }

        public Vector3 Offset
        {
            get
            {
                return new Vector3((float)mMatrix[0, 3], (float)mMatrix[1, 3], (float)mMatrix[2, 3]);
            }
        }

        public static Matrix4D RotateYUpToZUp
        {
            get
            {
                return new Matrix4D(new double[,]
                    {
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        {
                            0,
                            0,
                            -1,
                            0
                        },
                        {
                            0,
                            1,
                            0,
                            0
                        },
                        {
                            0,
                            0,
                            0,
                            1
                        }
                    });
            }
        }

        public static Matrix4D RotateZUpToYUp
        {
            get
            {
                return new Matrix4D(new double[,]
                    {
                        {
                            1,
                            0,
                            0,
                            0
                        },
                        {
                            0,
                            0,
                            1,
                            0
                        },
                        {
                            0,
                            -1,
                            0,
                            0
                        },
                        {
                            0,
                            0,
                            0,
                            1
                        }
                    });
            }
        }

        public Vector3 Scale
        {
            get
            {
                Vector3 scaleX = new Vector3((float)mMatrix[0, 0], (float)mMatrix[0, 1], (float)mMatrix[0, 2]),
                scaleY = new Vector3((float)mMatrix[1, 0], (float)mMatrix[1, 1], (float)mMatrix[1, 2]),
                scaleZ = new Vector3((float)mMatrix[2, 0], (float)mMatrix[2, 1], (float)mMatrix[2, 2]);
                return new Vector3(scaleX.Magnitude, scaleY.Magnitude, scaleZ.Magnitude);
            }
        }

        public double[] Values
        {
            get
            {
                return new[]
                {
                    mMatrix[0, 0],
                    mMatrix[0, 1],
                    mMatrix[0, 2],
                    mMatrix[0, 3],
                    mMatrix[1, 0],
                    mMatrix[1, 1],
                    mMatrix[1, 2],
                    mMatrix[1, 3],
                    mMatrix[2, 0],
                    mMatrix[2, 1],
                    mMatrix[2, 2],
                    mMatrix[2, 3],
                    mMatrix[3, 0],
                    mMatrix[3, 1],
                    mMatrix[3, 2],
                    mMatrix[3, 3]
                };
            }
        }

        public Matrix4D(double[] array)
        {
            mMatrix = new double[,]
                {
                    {
                        array[0],
                        array[1],
                        array[2],
                        array[3]
                    },
                    {
                        array[4],
                        array[5],
                        array[6],
                        array[7]
                    },
                    {
                        array[8],
                        array[9],
                        array[10],
                        array[11]
                    },
                    {
                        array[12],
                        array[13],
                        array[14],
                        array[15]
                    }
                };
        }

        public Matrix4D(double[,] array4x4)
        {
            mMatrix = new double[,]
                {
                    {
                        array4x4[0, 0],
                        array4x4[0, 1],
                        array4x4[0, 2],
                        array4x4[0, 3]
                    },
                    {
                        array4x4[1, 0],
                        array4x4[1, 1], 
                        array4x4[1, 2],
                        array4x4[1, 3]
                    },
                    {
                        array4x4[2, 0],
                        array4x4[2, 1],
                        array4x4[2, 2],
                        array4x4[2, 3]
                    },
                    {
                        array4x4[3, 0],
                        array4x4[3, 1],
                        array4x4[3, 2],
                        array4x4[3, 3]
                    }
                };
        }

        public static Matrix4D operator *(Matrix4D m, float f)
        {
            var result = new double[4, 4];
            for (var r = 0; r < 4; r++)
            {
                for (var c = 0; c < 4; c++)
                {
                    result[r, c] = m.mMatrix[r, c] * f;
                }
            }
            return new Matrix4D(result);
        }

        public static Vector3 operator *(Matrix4D m, Vector3 v)
        {
            double x = 0,
            y = 0,
            z = 0;
            var temp = new[]
                {
                    v.X,
                    v.Y,
                    v.Z,
                    1
                };
            for (var i = 0; i < 4; i++)
            {
                x += m.mMatrix[0, i] * temp[i];
                y += m.mMatrix[1, i] * temp[i];
                z += m.mMatrix[2, i] * temp[i];
            }
            return new Vector3((float)x, (float)y, (float)z);
        }

        public static Matrix4D operator *(Matrix4D m1, Matrix4D m2)
        {
            var v = new double[4][];
            for (var i = 0; i < 4; i++)
            {
                v[i] = m1 * new[]
                    {
                        m2.mMatrix[0, i],
                        m2.mMatrix[1, i],
                        m2.mMatrix[2, i],
                        m2.mMatrix[3, i]
                    };
            }
            return new Matrix4D(new double[,]
                {
                    {
                        v[0][0],
                        v[1][0],
                        v[2][0],
                        v[3][0]
                    },
                    {
                        v[0][1],
                        v[1][1],
                        v[2][1],
                        v[3][1]
                    },
                    {
                        v[0][2],
                        v[1][2],
                        v[2][2],
                        v[3][2]
                    },
                    {
                        v[0][3],
                        v[1][3],
                        v[2][3],
                        v[3][3]
                    }
                });
        }

        public static double[] operator *(Matrix4D m, double[] v)
        {
            var temp = new double[4];
            for (var i = 0; i < 4; i++)
            {
                temp[0] += m.mMatrix[0, i] * v[i];
                temp[1] += m.mMatrix[1, i] * v[i];
                temp[2] += m.mMatrix[2, i] * v[i];
                temp[3] += m.mMatrix[3, i] * v[i];
            }
            return temp;
        }

        /// <summary>
        /// Rounds values close to zero
        /// </summary>
        public void Clean()
        {
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    if (Math.Abs(mMatrix[i, j]) < .0000002)
                    {
                        mMatrix[i, j] = 0;
                    }
                }
            }
        }

        public static Matrix4D FromAxisAngle(AxisAngle axisAngle)
        {
            axisAngle.Normalize();
            Matrix4D matrix = new Matrix4D();
            matrix.mMatrix = new double[,]
                {
                    {
                        0,
                        0,
                        0,
                        0
                    },
                    {
                        0,
                        0,
                        0,
                        0
                    },
                    {
                        0,
                        0,
                        0,
                        0
                    },
                    {
                        0,
                        0,
                        0,
                        0
                    }
                };
            double c = Math.Cos(axisAngle.Angle),
            s = Math.Sin(axisAngle.Angle),
            t = 1 - c,
            temp0 = axisAngle.X * axisAngle.Y * t,
            temp1 = axisAngle.Z * s;
            matrix.mMatrix[0, 0] = c + axisAngle.X * axisAngle.X * t;
            matrix.mMatrix[1, 1] = c + axisAngle.Y * axisAngle.Y * t;
            matrix.mMatrix[2, 2] = c + axisAngle.Z * axisAngle.Z * t;
            matrix.mMatrix[1, 0] = temp0 + temp1;
            matrix.mMatrix[0, 1] = temp0 - temp1;
            temp0 = axisAngle.X * axisAngle.Z * t;
            temp1 = axisAngle.Y * s;
            matrix.mMatrix[2, 0] = temp0 - temp1;
            matrix.mMatrix[0, 2] = temp0 + temp1; temp0 = axisAngle.Y * axisAngle.Z * t;
            temp1 = axisAngle.X * s;
            matrix.mMatrix[2, 1] = temp0 + temp1;
            matrix.mMatrix[1, 2] = temp0 - temp1;
            matrix.mMatrix[3, 3] = 1;
            return matrix;
        }

        public static Matrix4D FromOffset(Vector3 offset)
        {
            return new Matrix4D(new double[,]
                {
                    {
                        1,
                        0,
                        0,
                        offset.X
                    },
                    {
                        0,
                        1,
                        0,
                        offset.Y
                    },
                    {
                        0,
                        0,
                        1,
                        offset.Z
                    },
                    {
                        0,
                        0,
                        0,
                        1
                    }
                });
        }

        public static Matrix4D FromOffset(double[] offset)
        {
            return new Matrix4D(new double[,]
                {
                    {
                        1,
                        0,
                        0,
                        offset[0]
                    },
                    {
                        0,
                        1,
                        0,
                        offset[1]
                    },
                    {
                        0,
                        0,
                        1,
                        offset[2]
                    },
                    {
                        0,
                        0,
                        0,
                        1
                    }
                });
        }

        public static Matrix4D FromScale(Vector3 scale)
        {
            return new Matrix4D(new double[,]
                {
                    {
                        scale.X,
                        0,
                        0,
                        0
                    },
                    {
                        0,
                        scale.Y,
                        0,
                        0
                    },
                    {
                        0,
                        0,
                        scale.Z,
                        0
                    },
                    {
                        0,
                        0,
                        0,
                        1
                    }
                });
        }

        public static Matrix4D FromScale(double[] scale)
        {
            return new Matrix4D(new double[,]
                {
                    {
                        scale[0],
                        0,
                        0,
                        0
                    },
                    {
                        0,
                        scale[1],
                        0,
                        0
                    },
                    {
                        0,
                        0,
                        scale[2],
                        0
                    },
                    {
                        0,
                        0,
                        0,
                        1
                    }
                });
        }

        public Matrix4D Inverse()
        {
            double[,] m = mMatrix, inverseMatrix = new double[4, 4];
            double determinant = m[0, 3] * m[1, 2] * m[2, 1] * m[3, 0] - m[0, 2] * m[1, 3] * m[2, 1] * m[3, 0] - m[0, 3] * m[1, 1] * m[2, 2] * m[3, 0] + m[0, 1] * m[1, 3] * m[2, 2] * m[3, 0] + m[0, 2] * m[1, 1] * m[2, 3] * m[3, 0] - m[0, 1] * m[1, 2] * m[2, 3] * m[3, 0] - m[0, 3] * m[1, 2] * m[2, 0] * m[3, 1] + m[0, 2] * m[1, 3] * m[2, 0] * m[3, 1] + m[0, 3] * m[1, 0] * m[2, 2] * m[3, 1] - m[0, 0] * m[1, 3] * m[2, 2] * m[3, 1] - m[0, 2] * m[1, 0] * m[2, 3] * m[3, 1] + m[0, 0] * m[1, 2] * m[2, 3] * m[3, 1] + m[0, 3] * m[1, 1] * m[2, 0] * m[3, 2] - m[0, 1] * m[1, 3] * m[2, 0] * m[3, 2] - m[0, 3] * m[1, 0] * m[2, 1] * m[3, 2] + m[0, 0] * m[1, 3] * m[2, 1] * m[3, 2] + m[0, 1] * m[1, 0] * m[2, 3] * m[3, 2] - m[0, 0] * m[1, 1] * m[2, 3] * m[3, 2] - m[0, 2] * m[1, 1] * m[2, 0] * m[3, 3] + m[0, 1] * m[1, 2] * m[2, 0] * m[3, 3] + m[0, 2] * m[1, 0] * m[2, 1] * m[3, 3] - m[0, 0] * m[1, 2] * m[2, 1] * m[3, 3] - m[0, 1] * m[1, 0] * m[2, 2] * m[3, 3] + m[0, 0] * m[1, 1] * m[2, 2] * m[3, 3],
            inverseDeterminant = 1d / determinant;
            inverseMatrix[0, 0] = (m[1, 2] * m[2, 3] * m[3, 1] - m[1, 3] * m[2, 2] * m[3, 1] + m[1, 3] * m[2, 1] * m[3, 2] - m[1, 1] * m[2, 3] * m[3, 2] - m[1, 2] * m[2, 1] * m[3, 3] + m[1, 1] * m[2, 2] * m[3, 3]) * inverseDeterminant;
            inverseMatrix[0, 1] = (m[0, 3] * m[2, 2] * m[3, 1] - m[0, 2] * m[2, 3] * m[3, 1] - m[0, 3] * m[2, 1] * m[3, 2] + m[0, 1] * m[2, 3] * m[3, 2] + m[0, 2] * m[2, 1] * m[3, 3] - m[0, 1] * m[2, 2] * m[3, 3]) * inverseDeterminant;
            inverseMatrix[0, 2] = (m[0, 2] * m[1, 3] * m[3, 1] - m[0, 3] * m[1, 2] * m[3, 1] + m[0, 3] * m[1, 1] * m[3, 2] - m[0, 1] * m[1, 3] * m[3, 2] - m[0, 2] * m[1, 1] * m[3, 3] + m[0, 1] * m[1, 2] * m[3, 3]) * inverseDeterminant;
            inverseMatrix[0, 3] = (m[0, 3] * m[1, 2] * m[2, 1] - m[0, 2] * m[1, 3] * m[2, 1] - m[0, 3] * m[1, 1] * m[2, 2] + m[0, 1] * m[1, 3] * m[2, 2] + m[0, 2] * m[1, 1] * m[2, 3] - m[0, 1] * m[1, 2] * m[2, 3]) * inverseDeterminant;
            inverseMatrix[1, 0] = (m[1, 3] * m[2, 2] * m[3, 0] - m[1, 2] * m[2, 3] * m[3, 0] - m[1, 3] * m[2, 0] * m[3, 2] + m[1, 0] * m[2, 3] * m[3, 2] + m[1, 2] * m[2, 0] * m[3, 3] - m[1, 0] * m[2, 2] * m[3, 3]) * inverseDeterminant;
            inverseMatrix[1, 1] = (m[0, 2] * m[2, 3] * m[3, 0] - m[0, 3] * m[2, 2] * m[3, 0] + m[0, 3] * m[2, 0] * m[3, 2] - m[0, 0] * m[2, 3] * m[3, 2] - m[0, 2] * m[2, 0] * m[3, 3] + m[0, 0] * m[2, 2] * m[3, 3]) * inverseDeterminant;
            inverseMatrix[1, 2] = (m[0, 3] * m[1, 2] * m[3, 0] - m[0, 2] * m[1, 3] * m[3, 0] - m[0, 3] * m[1, 0] * m[3, 2] + m[0, 0] * m[1, 3] * m[3, 2] + m[0, 2] * m[1, 0] * m[3, 3] - m[0, 0] * m[1, 2] * m[3, 3]) * inverseDeterminant;
            inverseMatrix[1, 3] = (m[0, 2] * m[1, 3] * m[2, 0] - m[0, 3] * m[1, 2] * m[2, 0] + m[0, 3] * m[1, 0] * m[2, 2] - m[0, 0] * m[1, 3] * m[2, 2] - m[0, 2] * m[1, 0] * m[2, 3] + m[0, 0] * m[1, 2] * m[2, 3]) * inverseDeterminant;
            inverseMatrix[2, 0] = (m[1, 1] * m[2, 3] * m[3, 0] - m[1, 3] * m[2, 1] * m[3, 0] + m[1, 3] * m[2, 0] * m[3, 1] - m[1, 0] * m[2, 3] * m[3, 1] - m[1, 1] * m[2, 0] * m[3, 3] + m[1, 0] * m[2, 1] * m[3, 3]) * inverseDeterminant;
            inverseMatrix[2, 1] = (m[0, 3] * m[2, 1] * m[3, 0] - m[0, 1] * m[2, 3] * m[3, 0] - m[0, 3] * m[2, 0] * m[3, 1] + m[0, 0] * m[2, 3] * m[3, 1] + m[0, 1] * m[2, 0] * m[3, 3] - m[0, 0] * m[2, 1] * m[3, 3]) * inverseDeterminant;
            inverseMatrix[2, 2] = (m[0, 1] * m[1, 3] * m[3, 0] - m[0, 3] * m[1, 1] * m[3, 0] + m[0, 3] * m[1, 0] * m[3, 1] - m[0, 0] * m[1, 3] * m[3, 1] - m[0, 1] * m[1, 0] * m[3, 3] + m[0, 0] * m[1, 1] * m[3, 3]) * inverseDeterminant;
            inverseMatrix[2, 3] = (m[0, 3] * m[1, 1] * m[2, 0] - m[0, 1] * m[1, 3] * m[2, 0] - m[0, 3] * m[1, 0] * m[2, 1] + m[0, 0] * m[1, 3] * m[2, 1] + m[0, 1] * m[1, 0] * m[2, 3] - m[0, 0] * m[1, 1] * m[2, 3]) * inverseDeterminant;
            inverseMatrix[3, 0] = (m[1, 2] * m[2, 1] * m[3, 0] - m[1, 1] * m[2, 2] * m[3, 0] - m[1, 2] * m[2, 0] * m[3, 1] + m[1, 0] * m[2, 2] * m[3, 1] + m[1, 1] * m[2, 0] * m[3, 2] - m[1, 0] * m[2, 1] * m[3, 2]) * inverseDeterminant;
            inverseMatrix[3, 1] = (m[0, 1] * m[2, 2] * m[3, 0] - m[0, 2] * m[2, 1] * m[3, 0] + m[0, 2] * m[2, 0] * m[3, 1] - m[0, 0] * m[2, 2] * m[3, 1] - m[0, 1] * m[2, 0] * m[3, 2] + m[0, 0] * m[2, 1] * m[3, 2]) * inverseDeterminant;
            inverseMatrix[3, 2] = (m[0, 2] * m[1, 1] * m[3, 0] - m[0, 1] * m[1, 2] * m[3, 0] - m[0, 2] * m[1, 0] * m[3, 1] + m[0, 0] * m[1, 2] * m[3, 1] + m[0, 1] * m[1, 0] * m[3, 2] - m[0, 0] * m[1, 1] * m[3, 2]) * inverseDeterminant;
            inverseMatrix[3, 3] = (m[0, 1] * m[1, 2] * m[2, 0] - m[0, 2] * m[1, 1] * m[2, 0] + m[0, 2] * m[1, 0] * m[2, 1] - m[0, 0] * m[1, 2] * m[2, 1] - m[0, 1] * m[1, 0] * m[2, 2] + m[0, 0] * m[1, 1] * m[2, 2]) * inverseDeterminant;
            return new Matrix4D(inverseMatrix);
        }

        public Matrix4D RemoveOffset()
        {
            var d = new double[4, 4];
            Array.Copy(mMatrix, d, 16);
            d[0, 3] = 0;
            d[1, 3] = 0;
            d[2, 3] = 0;
            return new Matrix4D(d);
        }

        public AxisAngle ToAxisAngle()
        {
            double angle,
            epsilon0 = .01,
            epsilon1 = .1,
            x,
            y,
            z;
            if ((Math.Abs(mMatrix[0, 1] - mMatrix[1, 0]) < epsilon0) && (Math.Abs(mMatrix[0, 2] - mMatrix[2, 0]) < epsilon0) && (Math.Abs(mMatrix[1, 2] - mMatrix[2, 1]) < epsilon0))
            {
                if ((Math.Abs(mMatrix[0, 1] + mMatrix[1, 0]) < epsilon1) && (Math.Abs(mMatrix[0, 2] + mMatrix[2, 0]) < epsilon1) && (Math.Abs(mMatrix[1, 2] + mMatrix[2, 1]) < epsilon1) && (Math.Abs(mMatrix[0, 0] + mMatrix[1, 1] + mMatrix[2, 2] - 3) < epsilon1))
                {
                    return new AxisAngle(0, 1, 0, 0); 
                }
                angle = Math.PI;
                double xx = (mMatrix[0, 0] + 1) / 2,
                xy = (mMatrix[0, 1] + mMatrix[1, 0]) / 4,
                xz = (mMatrix[0, 2] + mMatrix[2, 0]) / 4,
                yy = (mMatrix[1, 1] + 1) / 2,
                yz = (mMatrix[1, 2] + mMatrix[2, 1]) / 4,
                zz = (mMatrix[2, 2] + 1) / 2;
                if (xx > yy && xx > zz)
                {
                    if (xx < epsilon0)
                    {
                        x = 0;
                        y = .7071;
                        z = .7071;
                    }
                    else
                    {
                        x = Math.Sqrt(xx);
                        y = xy / x;
                        z = xz / x;
                    }
                }
                else if (yy > zz)
                {
                    if (yy < epsilon0)
                    {
                        x = .7071;
                        y = 0;
                        z = .7071;
                    }
                    else
                    {
                        y = Math.Sqrt(yy);
                        x = xy / y;
                        z = yz / y;
                    }
                }
                else
                {
                    if (zz < epsilon0)
                    {
                        x = .7071;
                        y = .7071;
                        z = 0;
                    }
                    else
                    {
                        z = Math.Sqrt(zz);
                        x = xz / z;
                        y = yz / z;
                    }
                }
                return new AxisAngle(angle, x, y, z);
            }
            var s = Math.Sqrt((mMatrix[2, 1] - mMatrix[1, 2]) * (mMatrix[2, 1] - mMatrix[1, 2]) + (mMatrix[0, 2] - mMatrix[2, 0]) * (mMatrix[0, 2] - mMatrix[2, 0]) + (mMatrix[1, 0] - mMatrix[0, 1]) * (mMatrix[1, 0] - mMatrix[0, 1]));
            if (Math.Abs(s) < .001)
            {
                s = 1;
            }
            angle = Math.Acos((mMatrix[0, 0] + mMatrix[1, 1] + mMatrix[2, 2] - 1) / 2);
            x = (mMatrix[2, 1] - mMatrix[1, 2]) / s;
            y = (mMatrix[0, 2] - mMatrix[2, 0]) / s;
            z = (mMatrix[1, 0] - mMatrix[0, 1]) / s;
            return new AxisAngle(angle, x, y, z);
        }

        public Matrix3D ToMatrix3D()
        {
            return new Matrix3D(new float[,]
                {
                    {
                        (float)mMatrix[0, 0],
                        (float)mMatrix[0, 1],
                        (float)mMatrix[0, 2]
                    },
                    {
                        (float)mMatrix[1, 0],
                        (float)mMatrix[1, 1],
                        (float)mMatrix[1, 2]
                    },
                    {
                        (float)mMatrix[2, 0],
                        (float)mMatrix[2, 1],
                        (float)mMatrix[2, 2]
                    }
                });
        }

        public override string ToString()
        {
            var text = "";
            for (var r = 0; r < 4; r++)
            {
                for (var c = 0; c < 4; c++)
                {
                    text += mMatrix[r, c].ToString("G7");
                    if (c != 3 || r != 3)
                    {
                        text += ", ";
                    }
                }
                if (r < 4)
                {
                    text += Environment.NewLine;
                }
            }
            return text;
        }

        public string ToUnpunctuatedString()
        {
            var text = "";
            for (var r = 0; r < 4; r++)
            {
                for (var c = 0; c < 4; c++)
                {
                    text += mMatrix[r, c].ToString("G7", System.Globalization.CultureInfo.InvariantCulture) + " ";
                }
            }
            return text;
        }

        public Matrix4D Transpose()
        {
            return new Matrix4D(new double[,]
                {
                    {
                        mMatrix[0, 0],
                        mMatrix[1, 0],
                        mMatrix[2, 0],
                        mMatrix[3, 0]
                    },
                    {
                        mMatrix[0, 1],
                        mMatrix[1, 1],
                        mMatrix[2, 1],
                        mMatrix[3, 1]
                    },
                    {
                        mMatrix[0, 2],
                        mMatrix[1, 2],
                        mMatrix[2, 2],
                        mMatrix[3, 2]
                    },
                    {
                        mMatrix[0, 3],
                        mMatrix[1, 3],
                        mMatrix[2, 3],
                        mMatrix[3, 3]
                    }
                });
        }
    }
}
