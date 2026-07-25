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
using System.IO;

namespace Destrospean.CmarNYCBorrowed
{
    public class Tone
    {
        int mAgeGenderSetCount, mShaderCount, mSubSkinRampIndex, mTGICount, mTGIOffset, mTGISize, mToneRampIndex, mVersion;

        List<AgeGenderSet> mAgeGenderSets;

        byte mDominant;

        Shader[] mShaders;

        List<TGI> mTGIList;

        public AgeGenderSet[] AgeGenderSets
        {
            get
            {
                return mAgeGenderSets.ToArray();
            }
            set
            {
                mAgeGenderSets = new List<AgeGenderSet>(value);
            }
        }

        public int EdgeColorAdjustment
        {
            get
            {
                return EdgeColorAdjuster(41, mShaders[2].EdgeColor[1]);
            }
        }

        public int SpecularPowerAdjustment
        {
            get
            {
                return SpecularPowerAdjuster(4, mShaders[2].SpecularPower);
            }
        }

        public TGI[] TGIs
        {
            get
            {
                return mTGIList.ToArray();
            }
            set
            {
                mTGIList = new List<TGI>(value);
            }
        }

        public TGI ToneRampLink
        {
            get
            {
                return mTGIList[mToneRampIndex];
            }
        }

        public int Version
        {
            get
            {
                return mVersion;
            }
        }

        public class AgeGenderSet
        {
            uint mAgeGenderSpecies, mPartType;

            int mCleavageIndex, mCutnessIndex, mDarkIndex, mLightIndex, mNormalIndex, mOverlayIndex, mSpecularIndex;

            public AgeGender Age
            {
                get
                {
                    return (AgeGender)(mAgeGenderSpecies & 0xFF);
                }
            }

            public uint AgeGenderSpecies
            {
                get
                {
                    return mAgeGenderSpecies;
                }
            }

            public AgeGender Gender
            {
                get
                {
                    return (AgeGender)(mAgeGenderSpecies & 0xF000);
                }
            }

            public PartType PartType
            {
                get
                {
                    return (PartType)mPartType;
                }
                set
                {
                    mPartType = (uint)value;
                }
            }

            public int[] SkinLinks
            {
                get
                {
                    return new[]
                    {
                        mSpecularIndex,
                        mDarkIndex,
                        mLightIndex,
                        mNormalIndex,
                        mOverlayIndex,
                        mCutnessIndex,
                        mCleavageIndex
                    };
                }
            }

            public Species Species
            {
                get
                {
                    return (Species)(mAgeGenderSpecies & 0xF00);
                }
            }

            public AgeGenderSet(BinaryReader reader, int version)
            {
                mAgeGenderSpecies = reader.ReadUInt32();
                mPartType = reader.ReadUInt32();
                mSpecularIndex = reader.ReadInt32();
                mDarkIndex = reader.ReadInt32();
                mLightIndex = reader.ReadInt32();
                mNormalIndex = reader.ReadInt32();
                mOverlayIndex = reader.ReadInt32();
                if (version > 4)
                {
                    mCutnessIndex = reader.ReadInt32();
                    mCleavageIndex = reader.ReadInt32();
                }
                else
                {
                    mCutnessIndex = -1;
                    mCleavageIndex = -1;
                }
            }

            public override string ToString()
            {
                return mAgeGenderSpecies.ToString("X8") + ", " + mPartType + ": " + mSpecularIndex + ", " + mDarkIndex + ", " + mLightIndex + ", " + mNormalIndex + ", " + mOverlayIndex + ", " + mCutnessIndex + ", " + mCleavageIndex;
            }

            public void Write(BinaryWriter writer, int version)
            {
                writer.Write(mAgeGenderSpecies);
                writer.Write(mPartType);
                writer.Write(mSpecularIndex);
                writer.Write(mDarkIndex);
                writer.Write(mLightIndex);
                writer.Write(mNormalIndex);
                writer.Write(mOverlayIndex);
                if (version > 4)
                {
                    writer.Write(mCutnessIndex);
                    writer.Write(mCleavageIndex);
                }
            }
        }

        public class Shader
        {
            byte mAge, mGenderSpecies, mGenetic, mHandedness, mUnknown;

            byte[] mEdgeColor, mSpecularColor;

            float mSpecularPower;

            public byte[] EdgeColor
            {
                get
                {
                    return new[]
                    {
                        mEdgeColor[0],
                        mEdgeColor[1],
                        mEdgeColor[2],
                        mEdgeColor[3]
                    };
                }
                set
                {
                    mEdgeColor = new[]
                        {
                            value[0],
                            value[1],
                            value[2],
                            value[3]
                        };
                }
            }

            public byte[] SpecularColor
            {
                get
                {
                    return new[]
                    {
                        mSpecularColor[0],
                        mSpecularColor[1],
                        mSpecularColor[2],
                        mSpecularColor[3]
                    };
                }
                set
                {
                    mSpecularColor = new[]
                        {
                            value[0],
                            value[1],
                            value[2],
                            value[3]
                        };
                }
            }

            public float SpecularPower
            {
                get
                {
                    return mSpecularPower;
                }
                set
                {
                    mSpecularPower = value;
                }
            }

            public Shader(BinaryReader reader)
            {
                mAge = reader.ReadByte();
                mGenderSpecies = reader.ReadByte();
                mHandedness = reader.ReadByte();
                mUnknown = reader.ReadByte();
                mEdgeColor = new byte[4];
                for (var i = 0; i < 4; i++)
                {
                    mEdgeColor[i] = reader.ReadByte();
                }
                mSpecularColor = new byte[4];
                for (var i = 0; i < 4; i++)
                {
                    mSpecularColor[i] = reader.ReadByte();
                }
                mSpecularPower = reader.ReadSingle();
                mGenetic = reader.ReadByte();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(mAge);
                writer.Write(mGenderSpecies);
                writer.Write(mHandedness);
                writer.Write(mUnknown);
                for (var i = 0; i < 4; i++)
                {
                    writer.Write(mEdgeColor[i]);
                }
                for (var i = 0; i < 4; i++)
                {
                    writer.Write(mSpecularColor[i]);
                }
                writer.Write(mSpecularPower);
                writer.Write(mGenetic);
            }
        }

        public class SkinSet : IComparable<SkinSet>
        {
            public TGI CleavageLink, CutnessLink, DarkLink, LightLink, NormalsLink, OverlayLink, SpecularLink;

            public PartType PartType;

            /// <summary>
            /// Returns dark, cutness, and cleavage in that order
            /// </summary>
            public TGI[] SliderLinks
            {
                get
                {
                    return new TGI[]
                    {
                        DarkLink,
                        CutnessLink,
                        CleavageLink
                    };
                }
            }

            public SkinSet(PartType partType, TGI[] links)
            {
                SpecularLink = links[0];
                DarkLink = links[1];
                LightLink = links[2];
                NormalsLink = links[3];
                OverlayLink = links[4];
                CutnessLink = links[5];
                CleavageLink = links[6];
            }

            public int CompareTo(SkinSet other)
            {
                return PartType.CompareTo(other.PartType);
            }
        }

        public Tone(BinaryReader reader)
        {
            reader.BaseStream.Position = 0;
            mVersion = reader.ReadInt32();
            mTGIOffset = reader.ReadInt32();
            mTGISize = reader.ReadInt32();
            mShaderCount = reader.ReadInt32();
            mShaders = new Shader[mShaderCount];
            for (var i = 0; i < mShaderCount; i++)
            {
                mShaders[i] = new Shader(reader);
            }
            mToneRampIndex = reader.ReadInt32();
            mSubSkinRampIndex = reader.ReadInt32();
            mAgeGenderSetCount = reader.ReadInt32();
            mAgeGenderSets = new List<AgeGenderSet>();
            for (var i = 0; i < mAgeGenderSetCount; i++)
            {
                mAgeGenderSets.Add(new AgeGenderSet(reader, mVersion));
            }
            mDominant = reader.ReadByte();
            mTGICount = reader.ReadInt32();
            mTGIList = new List<TGI>();
            for (var i = 0; i < mTGICount; i++)
            {
                mTGIList.Add(new TGI(reader));
            }
        }

        public static int EdgeColorAdjuster(int baseValue, int currentValue)
        {
            double a = 4.48,
            b = -9.4;
            switch (baseValue)
            {
                case 34:
                    a = 3.76;
                    b = -12.2;
                    break;
                case 35:
                    a = 3.72;
                    b = -11.8;
                    break;
                case 36:
                    a = 3.68;
                    b = -11.4;
                    break;
                case 38:
                    a = 3.6;
                    b = -10.6;
                    break;
                case 41:
                    a = 3.48;
                    b = -9.4;
                    break;
                case 42:
                    a = 3.44;
                    b = -9.0;
                    break;
                case 44:
                    a = 3.36;
                    b = -8.2;
                    break;
                case 46:
                    a = 3.28;
                    b = -7.4;
                    break;
                case 51:
                    a = 3.08;
                    b = -5.4;
                    break;
            }
            var temp = (int)Math.Round((-b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * (1 - currentValue))) / (a * 2));
            return temp < 0 ? 0 : temp > 10 ? 10 : temp;
        }

        public static byte EdgeColorCalculator(int baseValue, int adjuster)
        {
            double a = 4.48,
            b = -9.4;
            switch (baseValue)
            {
                case 34:
                    a = 3.76;
                    b = -12.2;
                    break;
                case 35:
                    a = 3.72;
                    b = -11.8;
                    break;
                case 36:
                    a = 3.68;
                    b = -11.4;
                    break;
                case 38:
                    a = 3.6;
                    b = -10.6;
                    break;
                case 41:
                    a = 3.48;
                    b = -9.4;
                    break;
                case 42:
                    a = 3.44;
                    b = -9.0;
                    break;
                case 44:
                    a = 3.36;
                    b = -8.2;
                    break;
                case 46:
                    a = 3.28;
                    b = -7.4;
                    break;
                case 51:
                    a = 3.08;
                    b = -5.4;
                    break;
            }
            return (byte)Math.Round(a * Math.Pow(adjuster, 2) + b * adjuster + 1);
        }

        public SkinSet GetSkinSet(Species species, AgeGender age, AgeGender gender, PartType partType)
        {
            foreach (var set in mAgeGenderSets)
            {
                if (partType == set.PartType && (((uint)species & set.AgeGenderSpecies) > 0 || species == Species.Human && set.Species == 0) && ((uint)age & set.AgeGenderSpecies) > 0 && ((uint)gender & set.AgeGenderSpecies) > 0)
                {
                    var indexes = set.SkinLinks;
                    var tgis = new TGI[indexes.Length];
                    for (var i = 0; i < indexes.Length; i++)
                    {
                        tgis[i] = indexes[i] < 0 ? null : mTGIList[indexes[i]];
                    }
                    return new SkinSet(set.PartType, tgis);
                }
            }
            return null;
        }

        public static int SpecularPowerAdjuster(float baseValue, float currentValue)
        {
            var temp = (int)Math.Round((.15 + Math.Sqrt(.0225 - .68 * (.5 - currentValue))) / .34);
            return temp < 0 ? 0 : temp > 10 ? 10 : temp;
        }

        public static float SpecularPowerCalculator(float baseValue, int adjuster)
        {
            return (float)(.17 * Math.Pow(adjuster, 2) - .15 * adjuster + .5);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(mVersion);
            var ageGenderLength = mVersion == 4 ? 28 : 36;
            mTGIOffset = 8 + mShaderCount * 17 + 12 + mAgeGenderSets.Count * ageGenderLength + 1;
            writer.Write(mTGIOffset);
            mTGISize = mTGIList.Count * 16 + 4;
            writer.Write(mTGISize);
            writer.Write(mShaderCount);
            for (var i = 0; i < mShaderCount; i++)
            {
                mShaders[i].Write(writer);
            }
            writer.Write(mToneRampIndex);
            writer.Write(mSubSkinRampIndex);
            writer.Write(mAgeGenderSets.Count);
            for (var i = 0; i < mAgeGenderSets.Count; i++)
            {
                mAgeGenderSets[i].Write(writer, mVersion);
            }
            writer.Write(mDominant);
            writer.Write(mTGIList.Count);
            for (var i = 0; i < mTGIList.Count; i++)
            {
                mTGIList[i].Write(writer);
            }
        }
    }
}
