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
using System.IO;
using System.Collections.Generic;
using s3pi.GenericRCOLResource;

namespace Destrospean.CmarNYCBorrowed
{
    public class GEOM : IComparable<GEOM>
    {
        int mAbsolutePosition, mBoneHashCount, mCount, mExternalCount, mFaceCount, mFacePointCount, mIndexCount, mInternalCount, mMergeGroup, mMeshSize, mMTNFSize, mSeamStitchCount, mSKCONIndex, mSlotCount, mSortOrder, mSubMeshCount, mTGICount, mTGIOffset, mTGISize, mUVStitchCount, mVersion, mVersion1, mVertexCount;

        uint[] mBoneHashArray = null;

        Bones[] mBones = null;

        byte mBytesPerFacePoint;

        TGI mDummyTGI;

        Face[] mFaces = null;

        char[] mMagic;

        MTNF mMTNF;

        Normal[] mNormals = null;

        Position[] mPositions = null;

        SeamStitch[] mSeamStitches = null;

        uint mShaderHash;

        SlotrayIntersection[] mSlotrayIntersections = null;

        TagValue[] mTags = null;

        Tangent[] mTangents = null;

        TGI[] mTGIs = null;

        UV[][] mUVs = null;

        UVStitch[] mUVStitches = null;

        VertexFormat[] mVertexFormats = null;

        int[] mVertexIDs = null;

        public int BoneCount
        {
            get
            {
                return mBoneHashArray.Length;
            }
        }

        public uint[] BoneHashList
        {
            get
            {
                return mBoneHashArray;
            }
        }

        public int BytesPerFacePoint
        {
            get
            {
                return mBytesPerFacePoint;
            }
        }

        public bool CopyFaceMorphs = false;

        public Vector3[] DeltaPosition;

        public int FaceCount
        {
            get
            {
                return mFacePointCount / 3;
            }
        }

        public static List<int> FemaleWaistSequence = new List<int>
            {
                0x10,
                0x11,
                0x12,
                0x13,
                0x14,
                0x15,
                1,
                0xB,
                0xA,
                9,
                8,
                7,
                6,
                2,
                3,
                5,
                4,
                0,
                0xE,
                0xF,
                0xD,
                0xC
            },
        MaleWaistSequence = new List<int>
            {
                0x13,
                0x12,
                0x14,
                0x15,
                0x10,
                0x11,
                0xB,
                6,
                5,
                0xA,
                9,
                7,
                8,
                0,
                1,
                3,
                2,
                4,
                0xE,
                0xF,
                0xD,
                0xC
            };

        public bool HasBones
        {
            get
            {
                return Array.IndexOf(VertexFormatList, 4) > -1 && mBones.Length > 0;
            }
        }

        public bool HasNormals
        {
            get
            {
                return Array.IndexOf(VertexFormatList, 2) > -1 && mNormals.Length > 0;
            }
        }

        public bool HasPositions
        {
            get
            {
                return Array.IndexOf(VertexFormatList, 1) > -1 && mPositions.Length > 0;
            }
        }

        public bool HasTags
        {
            get
            {
                return Array.IndexOf(VertexFormatList, 7) > -1 && mTags.Length > 0;
            }
        }

        public bool HasTangents
        {
            get
            {
                return Array.IndexOf(VertexFormatList, 6) > -1 && mTangents.Length > 0;
            }
        }

        public bool HasUVs
        {
            get
            {
                return Array.IndexOf(VertexFormatList, 3) > -1 && mUVs.Length > 0;
            }
        }

        public bool HasVertexIDs
        {
            get
            {
                if (Array.IndexOf(VertexFormatList, 10) > -1 && mVertexIDs.Length > 0)
                {
                    for (var i = 0; i < mVertexCount; i++)
                    {
                        if (mVertexIDs[i] > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool IsBase
        {
            get
            {
                return mFaceCount > 3;
            }
        }

        public bool IsMorph
        {
            get
            {
                return mFaceCount == 3 && mVertexFormats[0].FormatDataType == 1 && mVertexFormats[0].FormatSubType == 1 && mVertexFormats[0].FormatDataLength == 12 && mVertexFormats[1].FormatDataType == 2 && mVertexFormats[1].FormatSubType == 1 && mVertexFormats[1].FormatDataLength == 12 && mVertexFormats[2].FormatDataType == 10 && mVertexFormats[2].FormatSubType == 4 && mVertexFormats[2].FormatDataLength == 4;
            }
        }

        public bool IsValid
        {
            get
            {
                var isValid = false;
                if (new string(mMagic) == "GEOM" && mVertexCount > 0 && (mVersion == 5 || mVersion == 12 || mVersion == 13 || mVersion == 14) && mFaceCount > 2)
                {
                    isValid = true;
                    var uvIndex = 0;
                    for (var i = 0; i < VertexFormatList.Length; i++)
                    {
                        switch (VertexFormatList[i])
                        {
                            case 1:
                                if (mPositions.Length != mVertexCount)
                                {
                                    isValid = false;
                                }
                                break;
                            case 2:
                                if (mNormals.Length != mVertexCount)
                                {
                                    isValid = false;
                                }
                                break;
                            case 3:
                                if (mUVs[uvIndex].Length != mVertexCount)
                                {
                                    isValid = false;
                                }
                                uvIndex += 1;
                                break;
                            case 4:
                                if (mBones.Length != mVertexCount)
                                {
                                    isValid = false;
                                }
                                break;
                            case 5:
                                if (mBones.Length != mVertexCount)
                                {
                                    isValid = false;
                                }
                                break;
                            case 6:
                                if (mTangents.Length != mVertexCount)
                                {
                                    isValid = false;
                                }
                                break;
                            case 7:
                                if (mTags.Length != mVertexCount)
                                {
                                    isValid = false;
                                }
                                break;
                            case 10:
                                if (mVertexIDs == null || mVertexIDs.Length != mVertexCount)
                                {
                                    isValid = false;
                                }
                                break;
                        }
                    }
                }
                return isValid;
            }
        }

        public int MaxVertexID
        {
            get
            {
                if (mVertexIDs == null)
                {
                    return -1;
                }
                var maxVertexID = 0;
                for (var i = 0; i < mVertexIDs.Length; i++)
                {
                    if (maxVertexID < mVertexIDs[i])
                    {
                        maxVertexID = mVertexIDs[i];
                    }
                }
                return maxVertexID;
            }
        }

        public int MergeGroup
        {
            get
            {
                return mMergeGroup;
            }
        }

        public static Vector3[][][][][] MeshSeamVertices = SetupSeamVertexPositions();

        public int MinVertexID
        {
            get
            {
                if (mVertexIDs == null)
                {
                    return -1;
                }
                var minVertexID = mVertexIDs[0];
                for (var i = 1; i < mVertexIDs.Length; i++)
                {
                    if (minVertexID > mVertexIDs[i])
                    {
                        minVertexID = mVertexIDs[i];
                    }
                }
                return minVertexID;
            }
        }

        public SeamStitch[] SeamStitches
        {
            get
            {
                return mSeamStitches;
            }
            set
            {
                if (value == null)
                {
                    mSeamStitches = new SeamStitch[0];
                    mSeamStitchCount = 0;
                }
                else
                {
                    mSeamStitches = value;
                    mSeamStitchCount = mSeamStitches.Length;
                }
            }
        }

        public MTNF Shader
        {
            get
            {
                return mMTNF;
            }
        }

        public uint ShaderHash
        {
            get
            {
                return mShaderHash;
            }
        }

        public int SkeletonIndex
        {
            get
            {
                return mSKCONIndex;
            }
        }

        public SlotrayIntersection[] SlotrayAdjustments
        {
            get
            {
                return mSlotrayIntersections;
            }
            set
            {
                if (value == null)
                {
                    mSlotrayIntersections = new SlotrayIntersection[0];
                    mSlotCount = 0;
                }
                else
                {
                    mSlotrayIntersections = value;
                    mSlotCount = mSlotrayIntersections.Length;
                }
            }
        }

        public int SlotrayAdjustmentsSize
        {
            get
            {
                return mSlotrayIntersections.Length * 63;
            }
        }

        public int SortOrder
        {
            get
            {
                return mSortOrder;
            }
        }

        public int SubMeshCount
        {
            get
            {
                return mSubMeshCount;
            }
        }

        public TGI[] TGIList
        {
            get
            {
                var temp = new TGI[mTGICount];
                for (var i = 0; i < mTGICount; i++)
                {
                    temp[i] = mTGIs[i];
                }
                return temp;
            }
            set
            {
                mTGICount = value.Length;
                var temp = new TGI[mTGICount];
                for (var i = 0; i < mTGICount; i++)
                {
                    temp[i] = value[i];
                }
                mTGIs = temp;
            }
        }

        public string[] TGIListText
        {
            get
            {
                var temp = new string[mTGICount];
                for (var i = 0; i < mTGICount; i++)
                {
                    temp[i] = mTGIs[i].ToString();
                }
                return temp;
            }
        }

        public int UVCount
        {
            get
            {
                if (mUVs == null)
                {
                    return 0;
                }
                else
                {
                    return mUVs.Length;
                }
            }
        }

        public UVStitch[] UVStitches
        {
            get
            {
                return mUVStitches;
            }
            set
            {
                if (value == null)
                {
                    mUVStitches = new UVStitch[0];
                    mUVStitchCount = 0;
                }
                else
                {
                    mUVStitches = value;
                    mUVStitchCount = mUVStitches.Length;
                }
            }
        }

        public int UVStitchesSize
        {
            get
            {
                var temp = 0;
                foreach (var uvStitch in mUVStitches)
                {
                    temp += uvStitch.Size;
                }
                return temp;
            }
        }

        public int Version
        {
            get
            {
                return mVersion;
            }
        }

        public int VertexCount
        {
            get
            {
                return mVertexCount;
            }
        }

        public int VertexDataLength
        {
            get
            {
                var length = 0;
                for (var i = 0; i < mFaceCount; i++)
                {
                    length = length + mVertexFormats[i].FormatDataLength;
                }
                return length;
            }
        }

        public int[] VertexFormatList
        {
            get
            {
                var temp = new int[mFaceCount];
                for (var i = 0; i < mFaceCount; i++)
                {
                    temp[i] = mVertexFormats[i].FormatDataType;
                }
                return temp;
            }
        }

        public VertexFormat[] VertexFormats
        {
            get
            {
                return mVertexFormats;
            }
        }

        public bool VertexIDsAreSequential
        {
            get
            {
                for (var i = 0; i < mVertexCount; i++)
                {
                    if (mVertexIDs[i] != i)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public enum SeamType
        {
            Ankles,
            Neck = 3,
            Waist,
            WaistAdultFemale,
            WaistAdultMale
        }

        public enum VertexFormatNames
        {
            Position = 1,
            Normals,
            UV,
            BoneAssignment,
            BoneWeight,
            Tangents,
            TagVal,
            VertexID = 10
        }

        public class Bones
        {
            byte[] mAssignments = new byte[4];

            float[] mWeights = new float[4];

            public byte[] BoneAssignments
            {
                get
                {
                    return new[]
                    {
                        mAssignments[0],
                        mAssignments[1],
                        mAssignments[2],
                        mAssignments[3]
                    };
                }
                set
                {
                    for (var i = 0; i < mAssignments.Length; i++)
                    {
                        mAssignments[i] = value[i];
                    }
                }
            }

            public byte[] BoneWeights
            {
                get
                {
                    return new[]
                    {
                        (byte)(mWeights[0] * byte.MaxValue),
                        (byte)(mWeights[1] * byte.MaxValue),
                        (byte)(mWeights[2] * byte.MaxValue),
                        (byte)(mWeights[3] * byte.MaxValue)
                    };
                }
                set
                {
                    var total = 0;
                    for (var i = 0; i < mAssignments.Length; i++)
                    {
                        mWeights[i] = value[i];
                        total += value[i];
                    }
                    mWeights[0] += (float)(byte.MaxValue - total) / byte.MaxValue;
                }
            }

            public float[] BoneWeightsV5
            {
                get
                {
                    return new[]
                    {
                        mWeights[0],
                        mWeights[1],
                        mWeights[2],
                        mWeights[3]
                    };
                }
                set
                {
                    for (var i = 0; i < mAssignments.Length; i++)
                    {
                        mWeights[i] = value[i];
                    }
                }
            }

            public Bones()
            {
            }

            public Bones(byte[] assignmentsIn, byte[] weightsIn)
            {
                for (var i = 0; i < 4; i++)
                {
                    mAssignments[i] = assignmentsIn[i];
                    mWeights[i] = (float)weightsIn[i] / byte.MaxValue;
                }
            }

            public Bones(byte[] assignmentsIn, float[] weightsIn)
            {
                for (var i = 0; i < 4; i++)
                {
                    mAssignments[i] = assignmentsIn[i];
                    mWeights[i] = weightsIn[i];
                }
            }

            public Bones(int[] assignmentsIn, byte[] weightsIn)
            {
                for (var i = 0; i < 4; i++)
                {
                    mAssignments[i] = (byte)assignmentsIn[i];
                    mWeights[i] = (float)weightsIn[i] / byte.MaxValue;
                }
            }

            public Bones(int[] assignmentsIn, float[] weightsIn)
            {
                for (var i = 0; i < 4; i++)
                {
                    mAssignments[i] = (byte)assignmentsIn[i];
                    mWeights[i] = weightsIn[i];
                }
            }

            public Bones(Bones source)
            {
                for (var i = 0; i < 4; i++)
                {
                    mAssignments[i] = source.mAssignments[i];
                    mWeights[i] = source.mWeights[i];
                }
            }

            public bool Equals(Bones compareBones)
            {
                return mAssignments[0] == compareBones.mAssignments[0] && mAssignments[1] == compareBones.mAssignments[1] && mAssignments[2] == compareBones.mAssignments[2] && mAssignments[3] == compareBones.mAssignments[3] && mWeights[0] == compareBones.mWeights[0] && mWeights[1] == compareBones.mWeights[1] && mWeights[2] == compareBones.mWeights[2] && mWeights[3] == compareBones.mWeights[3];
            }

            public void ReadAssignments(BinaryReader reader)
            {
                for (var i = 0; i < 4; i++)
                {
                    mAssignments[i] = reader.ReadByte();
                }
            }

            public void ReadWeights(BinaryReader reader, int subType)
            {
                if (subType == 1)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        mWeights[i] = reader.ReadSingle();
                    }
                }
                else if (subType == 2)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        mWeights[i] = (float)reader.ReadByte() / byte.MaxValue;
                    }
                }
            }

            public void Sort(int version)
            {
                for (var i = mAssignments.Length - 2; i >= 0; i--)
                {
                    for (var j = 0; j <= i; j++)
                    {
                        if (mWeights[j] < mWeights[j + 1])
                        {
                            var tempAssignments = mAssignments[j];
                            var tempWeights = mWeights[j];
                            mAssignments[j] = mAssignments[j + 1];
                            mAssignments[j + 1] = tempAssignments;
                            mWeights[j] = mWeights[j + 1];
                            mWeights[j + 1] = tempWeights;
                        }
                    }
                }
            }

            public override string ToString()
            {
                return mAssignments[0].ToString() + mAssignments[1].ToString() + mAssignments[2].ToString() + mAssignments[3].ToString() + mWeights[0].ToString() + ", " + mWeights[1].ToString() + ", " + mWeights[2].ToString() + ", " + mWeights[3].ToString();
            }

            public void WriteAssignments(BinaryWriter writer, int version, int maxBoneIndex)
            {
                if (version == 5)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        if (mWeights[i] > 0)
                        {
                            writer.Write(mAssignments[i]);
                        }
                        else
                        {
                            writer.Write((byte)2);
                        }
                    }
                }
                else if (version >= 12)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        writer.Write(mAssignments[i]);
                    }
                }
            }

            public void WriteWeights(BinaryWriter writer, int version)
            {
                for (var i = 0; i < 4; i++)
                {
                    if (version == 5)
                    {
                        writer.Write(mWeights[i]);
                    }
                    else if (version >= 12)
                    {
                        writer.Write((byte)(mWeights[i] * byte.MaxValue));
                    }
                }
            }
        }

        public class Face
        {
            uint[] mFace = new uint[3];

            public int FacePoint0
            {
                get
                {
                    return (int)mFace[0];
                }
            }

            public int FacePoint1
            {
                get
                {
                    return (int)mFace[1];
                }
            }

            public int FacePoint2
            {
                get
                {
                    return (int)mFace[2];
                }
            }

            public uint[] FacePoints
            {
                get
                {
                    return new[]
                    {
                        mFace[0],
                        mFace[1],
                        mFace[2]
                    };
                }
            }

            public Face()
            {
            }

            public Face(byte[] face)
            {
                for (var i = 0; i < 3; i++)
                {
                    mFace[i] = (uint)face[i];
                }
            }

            public Face(int[] face)
            {
                for (var i = 0; i < 3; i++)
                {
                    mFace[i] = (uint)face[i];
                }
            }

            public Face(uint[] face)
            {
                for (var i = 0; i < 3; i++)
                {
                    mFace[i] = face[i];
                }
            }

            public Face(ushort[] face)
            {
                for (var i = 0; i < 3; i++)
                {
                    mFace[i] = (uint)face[i];
                }
            }

            public Face(int facePoint0, int facePoint1, int facePoint2)
            {
                mFace[0] = (uint)facePoint0;
                mFace[1] = (uint)facePoint1;
                mFace[2] = (uint)facePoint2;
            }

            public Face(uint facePoint0, uint facePoint1, uint facePoint2)
            {
                mFace[0] = facePoint0;
                mFace[1] = facePoint1;
                mFace[2] = facePoint2;
            }

            public Face(ushort facePoint0, ushort facePoint1, ushort facePoint2)
            {
                mFace[0] = (uint)facePoint0;
                mFace[1] = (uint)facePoint1;
                mFace[2] = (uint)facePoint2;
            }

            public Face(BinaryReader reader, byte bytesPerFacePoint)
            {
                for (var i = 0; i < 3; i++)
                {
                    switch (bytesPerFacePoint)
                    {
                        case 1:
                            mFace[i] = reader.ReadByte();
                            break;
                        case 2:
                            mFace[i] = reader.ReadUInt16();
                            break;
                        case 4:
                            mFace[i] = reader.ReadUInt32();
                            break;
                    }

                }
            }

            public Face(Face source)
            {
                for (var i = 0; i < 3; i++)
                {
                    mFace[i] = source.mFace[i];
                }
            }

            public bool Equals(Face face)
            {
                return mFace[0] == face.mFace[0] && mFace[1] == face.mFace[1] && mFace[2] == face.mFace[2];
            }

            public void Reverse()
            {
                var temp = mFace[0];
                mFace[0] = mFace[2];
                mFace[2] = temp;
            }

            public static Face Reverse(Face source)
            {
                return new Face(source.FacePoint2, source.FacePoint1, source.FacePoint0);
            }

            public override string ToString()
            {
                return mFace[0].ToString() + ", " + mFace[1].ToString() + ", " + mFace[2].ToString();
            }

            public void Write(BinaryWriter writer, byte bytesPerFacePoint)
            {
                for (var i = 0; i < 3; i++)
                {
                    switch (bytesPerFacePoint)
                    {
                        case 1:
                            writer.Write((byte)mFace[i]);
                            break;
                        case 2:
                            writer.Write((ushort)mFace[i]);
                            break;
                        case 4:
                            writer.Write(mFace[i]);
                            break;
                    }
                }
            }
        }

        [Serializable]
        public class MeshException : ApplicationException
        {
            public MeshException()
            {
            }

            public MeshException(string message) : base(message)
            {
            }

            public MeshException(string message, Exception inner) : base(message, inner)
            {
            }

            protected MeshException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
            {
            }
        }

        public class MTNF
        {
            object[][] mData;

            uint[][] mFields;

            char[] mMagic;

            int mZero;

            public int ChunkSize
            {
                get
                {
                    return 16 + FieldCount * 16 + DataSize;
                }
            }

            public int DataSize
            {
                get
                {
                    var dataSize = 0;
                    foreach (var element in mData)
                    {
                        dataSize += element.Length * 4;
                    }
                    return dataSize;
                }
            }

            public int FieldCount
            {
                get;
                private set;
            }

            public int EmissionIndex
            {
                get
                {
                    for (var i = 0; i < FieldCount; i++)
                    {
                        if (mFields[i][0] == (uint)FieldType.EmissionMap)
                        {
                            return (int)(uint)mData[i][0];
                        }
                    }
                    return -1;
                }
                set
                {
                    for (var i = 0; i < FieldCount; i++)
                    {
                        if (mFields[i][0] == (uint)FieldType.EmissionMap)
                        {
                            mData[i][0] = (uint)value;
                        }
                    }

                }
            }

            public int NormalIndex
            {
                get
                {
                    for (var i = 0; i < FieldCount; i++)
                    {
                        if (mFields[i][0] == (uint)FieldType.NormalMap)
                        {
                            return (int)(uint)mData[i][0];
                        }
                    }
                    return -1;
                }
                set
                {
                    for (var i = 0; i < FieldCount; i++)
                    {
                        if (mFields[i][0] == (uint)FieldType.NormalMap)
                        {
                            mData[i][0] = (uint)value;
                        }
                    }

                }
            }

            public MTNF()
            {
            }

            public MTNF(BinaryReader reader)
            {
                mMagic = reader.ReadChars(4);
                mZero = reader.ReadInt32();
                reader.ReadInt32();
                FieldCount = reader.ReadInt32();
                mFields = new uint[FieldCount][];
                for (var i = 0; i < FieldCount; i++)
                {
                    mFields[i] = new uint[4];
                    for (var j = 0; j < 4; j++)
                    {
                        mFields[i][j] = reader.ReadUInt32();
                    }
                }
                mData = new object[FieldCount][];
                for (var i = 0; i < FieldCount; i++)
                {
                    mData[i] = new object[mFields[i][2]];
                    if (mFields[i][1] == 1)
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            mData[i][j] = reader.ReadSingle();
                        }
                    }
                    else if (mFields[i][1] == 2)
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            mData[i][j] = reader.ReadInt32();
                        }
                    }
                    else
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            mData[i][j] = reader.ReadUInt32();
                        }
                    }
                }
            }

            public MTNF(uint[] shaderDataArray)
            {
                mMagic = System.Text.Encoding.UTF8.GetString(BitConverter.GetBytes(shaderDataArray[0])).ToCharArray();
                mZero = (int)shaderDataArray[1];
                FieldCount = (int)shaderDataArray[3];
                mFields = new uint[FieldCount][];
                mData = new object[FieldCount][];
                var index = 4;
                for (var i = 0; i < FieldCount; i++)
                {
                    mFields[i] = new uint[4];
                    for (var j = 0; j < 4; j++)
                    {
                        mFields[i][j] = shaderDataArray[index];
                        index++;
                    }
                }
                for (var i = 0; i < FieldCount; i++)
                {
                    mData[i] = new object[mFields[i][2]];
                    if (mFields[i][1] == 1)
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            mData[i][j] = BitConverter.ToSingle(BitConverter.GetBytes(shaderDataArray[index]), 0);
                            index++;
                        }
                    }
                    else if (mFields[i][1] == 2)
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            mData[i][j] = (int)shaderDataArray[index];
                            index++;
                        }
                    }
                    else
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            mData[i][j] = shaderDataArray[index];
                            index++;
                        }
                    }
                }
            }

            public MTNF(MTNF source)
            {
                mMagic = source.mMagic;
                mZero = source.mZero;
                FieldCount = source.FieldCount;
                mFields = new uint[source.mFields.Length][];
                for (var i = 0; i < source.mFields.Length; i++)
                {
                    mFields[i] = new uint[source.mFields[i].Length];
                    for (var j = 0; j < source.mFields[i].Length; j++)
                    {
                        mFields[i][j] = source.mFields[i][j];
                    }
                }
                mData = new object[source.mData.Length][];
                for (var i = 0; i < source.mData.Length; i++)
                {
                    mData[i] = new object[source.mData[i].Length];
                    for (var j = 0; j < source.mData[i].Length; j++)
                    {
                        mData[i][j] = source.mData[i][j];
                    }
                }
            }

            public void AddField(uint fieldType, uint valueType, uint valueCount)
            {
                InsertField(FieldCount, fieldType, valueType, valueCount);
            }

            public object[] GetField(int index, out uint fieldType, out uint valueType)
            {
                fieldType = mFields[index][0];
                valueType = mFields[index][1];
                return mData[index];
            }

            public void InsertField(int index, uint fieldType, uint valueType, uint valueCount)
            {
                var elementList = new List<object[]>(mData);
                var fieldList = new List<uint[]>(mFields);
                fieldList.Insert(index, new[]
                    {
                        fieldType,
                        valueType,
                        valueCount
                    });
                elementList.Insert(index, new object[valueCount]);
                for (var i = 0; i < valueCount; i++)
                {
                    switch (valueType)
                    {
                        case 1:
                            elementList[index][i] = 0f;
                            break;
                        case 2:
                            elementList[index][i] = 0;
                            break;
                        default:
                            elementList[index][i] = 0u;
                            break;
                    }
                }
                mData = elementList.ToArray();
                mFields = fieldList.ToArray();
                FieldCount++;
            }

            public void RemoveField(int index)
            {
                var elementList = new List<object[]>(mData);
                var fieldList = new List<uint[]>(mFields);
                elementList.RemoveAt(index);
                fieldList.RemoveAt(index);
                mData = elementList.ToArray();
                mFields = fieldList.ToArray();
                FieldCount--;
            }

            public uint[] ToDataArray()
            {
                var temp = new List<uint>();
                temp.Add(BitConverter.ToUInt32(System.Text.Encoding.UTF8.GetBytes(mMagic), 0));
                temp.Add((uint)mZero);
                temp.Add((uint)DataSize);
                temp.Add((uint)FieldCount);
                for (var i = 0; i < FieldCount; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        temp.Add(mFields[i][j]);
                    }
                }
                for (var i = 0; i < FieldCount; i++)
                {
                    if (mFields[i][1] == 1)
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            temp.Add(BitConverter.ToUInt32(BitConverter.GetBytes((float)mData[i][j]), 0));
                        }
                    }
                    else if (mFields[i][1] == 2)
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            temp.Add((uint)(int)mData[i][j]);
                        }
                    }
                    else
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            temp.Add((uint)mData[i][j]);
                        }
                    }
                }
                return temp.ToArray();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(mMagic);
                writer.Write(mZero);
                writer.Write(DataSize);
                writer.Write(FieldCount);
                for (var i = 0; i < FieldCount; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        writer.Write(mFields[i][j]);
                    }
                }
                for (var i = 0; i < FieldCount; i++)
                {
                    if (mFields[i][1] == 1)
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            writer.Write((float)mData[i][j]);
                        }
                    }
                    else if (mFields[i][1] == 2)
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            writer.Write((int)mData[i][j]);
                        }
                    }
                    else
                    {
                        for (var j = 0; j < mFields[i][2]; j++)
                        {
                            writer.Write((uint)mData[i][j]);
                        }
                    }
                }
            }
        }

        public class Normal
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
            }

            public float X, Y, Z;

            public Normal()
            {
                X = 0;
                Y = 0;
                Z = 0;
            }

            public Normal(float[] newNormal)
            {
                X = newNormal[0];
                Y = newNormal[1];
                Z = newNormal[2];
            }

            public Normal(BinaryReader reader)
            {
                X = reader.ReadSingle();
                Y = reader.ReadSingle();
                Z = reader.ReadSingle();
            }

            public Normal(Normal source)
            {
                X = source.X;
                Y = source.Y;
                Z = source.Z;
            }

            public Normal(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public void AddDeltas(float[] deltas)
            {
                X += deltas[0];
                Y += deltas[1];
                Z += deltas[2];
            }

            public bool Equals(Normal compareNormal)
            {
                return IsEqual(X, compareNormal.X) && IsEqual(Y, compareNormal.Y) && IsEqual(Z, compareNormal.Z);
            }

            public override string ToString()
            {
                return X.ToString() + ", " + Y.ToString() + ", " + Z.ToString();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(X);
                writer.Write(Y);
                writer.Write(Z);
            }
        }

        public class Position
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
            }

            public float X, Y, Z;

            public Position()
            {
                X = 0;
                Y = 0;
                Z = 0;
            }

            public Position(float[] newPosition)
            {
                X = newPosition[0];
                Y = newPosition[1];
                Z = newPosition[2];
            }

            public Position(BinaryReader reader)
            {
                X = reader.ReadSingle();
                Y = reader.ReadSingle();
                Z = reader.ReadSingle();
            }

            public Position(Position source)
            {
                X = source.X;
                Y = source.Y;
                Z = source.Z;
            }

            public Position(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public void AddDeltas(float[] deltas)
            {
                X += deltas[0];
                Y += deltas[1];
                Z += deltas[2];
            }

            public bool Equals(Position comparePosition)
            {
                return IsEqual(X, comparePosition.X) && IsEqual(Y, comparePosition.Y) && IsEqual(Z, comparePosition.Z);
            }

            public override string ToString()
            {
                return X.ToString() + ", " + Y.ToString() + ", " + Z.ToString();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(X);
                writer.Write(Y);
                writer.Write(Z);
            }
        }

        public class SeamStitch : IComparable, IEquatable<SeamStitch>
        {
            public uint Index;

            public int SeamIndex
            {
                get
                {
                    return VertexID & 0xFFF;
                }
            }

            public int SeamType
            {
                get
                {
                    return VertexID >> 12;
                }
            }

            public const int Size = 6;

            public float UVX;

            public ushort VertexID;

            public int WaistSequence;

            public SeamStitch(SeamStitch adjustment)
            {
                Index = adjustment.Index;
                VertexID = adjustment.VertexID;
                WaistSequence = adjustment.WaistSequence;
                UVX = adjustment.UVX;
            }

            public SeamStitch(int index, SeamType seam, int vertex, UV[] uvs)
            {
                Index = (uint)index;
                VertexID = (ushort)(((int)seam << 12) + vertex);
                if (SeamType == (int)GEOM.SeamType.WaistAdultFemale)
                {
                    WaistSequence = FemaleWaistSequence.IndexOf(SeamIndex);
                }
                else if (SeamType == (int)GEOM.SeamType.WaistAdultMale)
                {
                    WaistSequence = MaleWaistSequence.IndexOf(SeamIndex);
                }
                UVX = uvs[index].U;
            }

            public SeamStitch(int index, int seam, int vertex, UV[] uvs)
            {
                Index = (uint)index;
                VertexID = (ushort)((seam << 12) + vertex);
                if (SeamType == (int)GEOM.SeamType.WaistAdultFemale)
                {
                    WaistSequence = FemaleWaistSequence.IndexOf(SeamIndex);
                }
                else if (SeamType == (int)GEOM.SeamType.WaistAdultMale)
                {
                    WaistSequence = MaleWaistSequence.IndexOf(SeamIndex);
                }
                UVX = uvs[index].U;
            }

            public SeamStitch(BinaryReader reader, UV[] uvs)
            {
                Index = reader.ReadUInt32();
                VertexID = reader.ReadUInt16();
                if (SeamType == (int)GEOM.SeamType.WaistAdultFemale)
                {
                    WaistSequence = FemaleWaistSequence.IndexOf(SeamIndex);
                }
                else if (SeamType == (int)GEOM.SeamType.WaistAdultMale)
                {
                    WaistSequence = MaleWaistSequence.IndexOf(SeamIndex);
                }
                UVX = uvs[Index].U;
            }

            public int CompareTo(object obj)
            {
                var seamStitch = obj as SeamStitch;
                return seamStitch == null ? 0 : WaistSequence.Equals(seamStitch.WaistSequence) ? UVX.CompareTo(seamStitch.UVX) : WaistSequence.CompareTo(seamStitch.WaistSequence);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as SeamStitch);
            }

            public bool Equals(SeamStitch seamStitch)
            {
                return VertexID == seamStitch.VertexID && UVX == seamStitch.UVX;
            }

            public override int GetHashCode()
            {
                return (VertexID + UVX).GetHashCode();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(Index);
                writer.Write(VertexID);
            }
        }

        public class SlotrayIntersection
        {
            float[] mCoordinates, mOffsetFromIntersectionObjectSpace, mSlotAveragePositionObjectSpace, mTransformToLocalSpace;

            int mVersion;

            short[] mVertexIndices;

            public Vector2 Coordinates
            {
                get
                {
                    return new Vector2(mCoordinates);
                }
                set
                {
                    mCoordinates = value.Coordinates;
                }
            }

            public float Distance;

            public Vector3 OffsetFromIntersectionObjectSpace
            {
                get
                {
                    return new Vector3(mOffsetFromIntersectionObjectSpace);
                }
                set
                {
                    mOffsetFromIntersectionObjectSpace = value.Coordinates;
                }
            }

            public uint PivotBone, SlotIndex;

            public Vector3 SlotAveragePositionObjectSpace
            {
                get
                {
                    return new Vector3(mSlotAveragePositionObjectSpace);
                }
                set
                {
                    mSlotAveragePositionObjectSpace = value.Coordinates;
                }
            }

            public Quaternion TransformToLocalSpace
            {
                get
                {
                    return new Quaternion(mTransformToLocalSpace);
                }
                set
                {
                    mTransformToLocalSpace = new[]
                        {
                            (float)value.X,
                            (float)value.Y,
                            (float)value.Z,
                            (float)value.W
                        };
                }
            }

            public int[] TrianglePointIndices
            {
                get
                {
                    return new int[]
                    {
                        mVertexIndices[0],
                        mVertexIndices[1],
                        mVertexIndices[2]
                    };
                }
                set
                {
                    mVertexIndices[0] = (short)value[0];
                    mVertexIndices[1] = (short)value[1];
                    mVertexIndices[2] = (short)value[2];
                }
            }

            public SlotrayIntersection(SlotrayIntersection faceAdjustment)
            {
                SlotIndex = faceAdjustment.SlotIndex;
                mVertexIndices = new short[3];
                for (var i = 0; i < 3; i++)
                {
                    mVertexIndices[i] = faceAdjustment.mVertexIndices[i];
                }
                mCoordinates = new float[2];
                for (var i = 0; i < 2; i++)
                {
                    mCoordinates[i] = faceAdjustment.mCoordinates[i];
                }
                Distance = faceAdjustment.Distance;
                mOffsetFromIntersectionObjectSpace = new float[3];
                for (var i = 0; i < 3; i++)
                {
                    mOffsetFromIntersectionObjectSpace[i] = faceAdjustment.mOffsetFromIntersectionObjectSpace[i];
                }
                mSlotAveragePositionObjectSpace = new float[3];
                for (var i = 0; i < 3; i++)
                {
                    mSlotAveragePositionObjectSpace[i] = faceAdjustment.mSlotAveragePositionObjectSpace[i];
                }
                mTransformToLocalSpace = new float[4];
                for (var i = 0; i < 4; i++)
                {
                    mTransformToLocalSpace[i] = faceAdjustment.mTransformToLocalSpace[i];
                }
                PivotBone = faceAdjustment.PivotBone;
            }

            public SlotrayIntersection(BinaryReader reader, int version)
            {
                mVersion = version;
                SlotIndex = reader.ReadUInt32();
                mVertexIndices = new short[3];
                for (var i = 0; i < 3; i++)
                {
                    mVertexIndices[i] = reader.ReadInt16();
                }
                mCoordinates = new float[2];
                for (var i = 0; i < 2; i++)
                {
                    mCoordinates[i] = reader.ReadSingle();
                }
                Distance = reader.ReadSingle();
                mOffsetFromIntersectionObjectSpace = new float[3];
                for (var i = 0; i < 3; i++)
                {
                    mOffsetFromIntersectionObjectSpace[i] = reader.ReadSingle();
                }
                mSlotAveragePositionObjectSpace = new float[3];
                for (var i = 0; i < 3; i++)
                {
                    mSlotAveragePositionObjectSpace[i] = reader.ReadSingle();
                }
                mTransformToLocalSpace = new float[4];
                for (var i = 0; i < 4; i++)
                {
                    mTransformToLocalSpace[i] = reader.ReadSingle();
                }
                if (mVersion >= 14)
                {
                    PivotBone = reader.ReadUInt32();
                }
                else
                {
                    PivotBone = reader.ReadByte();
                }
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(SlotIndex);
                for (var i = 0; i < mVertexIndices.Length; i++)
                {
                    writer.Write(mVertexIndices[i]);
                }
                for (var i = 0; i < mCoordinates.Length; i++)
                {
                    writer.Write(mCoordinates[i]);
                }
                writer.Write(Distance);
                for (var i = 0; i < mOffsetFromIntersectionObjectSpace.Length; i++)
                {
                    writer.Write(mOffsetFromIntersectionObjectSpace[i]);
                }
                for (var i = 0; i < mSlotAveragePositionObjectSpace.Length; i++)
                {
                    writer.Write(mSlotAveragePositionObjectSpace[i]);
                }
                for (var i = 0; i < mTransformToLocalSpace.Length; i++)
                {
                    writer.Write(mTransformToLocalSpace[i]);
                }
                if (mVersion >= 14)
                {
                    writer.Write(PivotBone);
                }
                else
                {
                    writer.Write((byte)PivotBone);
                }
            }
        }

        public class TagValue
        {
            public uint Tags
            {
                get;
                private set;
            }

            public TagValue()
            {
            }

            public TagValue(BinaryReader reader)
            {
                Tags = reader.ReadUInt32();
            }

            public TagValue(TagValue source)
            {
                Tags = source.Tags;
            }

            public TagValue(uint tagValue)
            {
                Tags = tagValue;
            }

            public bool Equals(TagValue compareTagValue)
            {
                return Tags == compareTagValue.Tags;
            }

            public override string ToString()
            {
                return Convert.ToString(Tags, 16).ToUpper().PadLeft(8, '0');
            }
                
            public void Write(BinaryWriter writer)
            {
                writer.Write(Tags);
            }
        }

        public class Tangent
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
            }

            public float X, Y, Z;

            public Tangent()
            {
            }

            public Tangent(float[] newTangent)
            {
                X = newTangent[0];
                Y = newTangent[1];
                Z = newTangent[2];
            }

            public Tangent(BinaryReader reader)
            {
                X = reader.ReadSingle();
                Y = reader.ReadSingle();
                Z = reader.ReadSingle();
            }

            public Tangent(Tangent source)
            {
                X = source.X;
                Y = source.Y;
                Z = source.Z;
            }

            public Tangent(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public bool Equals(Tangent compareTangent)
            {
                return IsEqual(X, compareTangent.X) && IsEqual(Y, compareTangent.Y) && IsEqual(Z, compareTangent.Z);
            }

            public override string ToString()
            {
                return X.ToString() + ", " + Y.ToString() + ", " + Z.ToString();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(X);
                writer.Write(Y);
                writer.Write(Z);
            }
        }

        public class UV
        {
            public float[] Coordinates
            {
                get
                {
                    return new[]
                    {
                        U,
                        V
                    };
                }
            }

            public float U, V;

            public UV()
            {
            }

            public UV(float[] newUV, bool verticalFlip = false)
            {
                U = newUV[0];
                V = verticalFlip ? 1 - newUV[1] : newUV[1];
            }

            public UV(BinaryReader reader)
            {
                U = reader.ReadSingle();
                V = reader.ReadSingle();
            }

            public UV(UV source)
            {
                U = source.U;
                V = source.V;
            }

            public UV(float u, float v)
            {
                U = u;
                V = v;
            }

            public bool CloseTo(UV other)
            {
                var diff = .001f;
                return Math.Abs(U - other.U) < diff && Math.Abs(V - other.V) < diff;
            }

            public bool Equals(UV compareUV)
            {
                return IsEqual(U, compareUV.U) && IsEqual(V, compareUV.V);
            }

            public override string ToString()
            {
                return U.ToString() + ", " + V.ToString();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(U);
                writer.Write(V);
            }
        }

        public class UVStitch
        {
            float[][] mCoordinates;

            public int Count
            {
                get;
                private set;
            }

            public int Index;

            public int Size
            {
                get
                {
                    return 8 + Count * 8;
                }
            }

            public List<float[]> UV1Coordinates
            {
                get
                {
                    var pairs = new List<float[]>();
                    for (var i = 0; i < mCoordinates.Length; i++)
                    {
                        pairs.Add(mCoordinates[i]);
                    }
                    return pairs;
                }
            }

            public List<Vector2> UV1Vectors
            {
                get
                {
                    var pairs = new List<Vector2>();
                    for (var i = 0; i < mCoordinates.Length; i++)
                    {
                        pairs.Add(new Vector2(mCoordinates[i]));
                    }
                    return pairs;
                }
            }

            public UVStitch(BinaryReader reader)
            {
                Index = reader.ReadInt32();
                Count = reader.ReadInt32();
                mCoordinates = new float[Count][];
                for (var i = 0; i < Count; i++)
                {
                    mCoordinates[i] = new[]
                        {
                            reader.ReadSingle(),
                            reader.ReadSingle()
                        };
                }
            }

            public UVStitch(UVStitch source)
            {
                Index = source.Index;
                Count = source.Count;
                mCoordinates = new float[source.Count][];
                for (var i = 0; i < Count; i++)
                {
                    mCoordinates[i] = new[]
                        {
                            source.mCoordinates[i][0],
                            source.mCoordinates[i][1]
                        };
                }
            }

            public UVStitch(int vertexIndex, Vector2[] uv0Coordinates)
            {
                Index = vertexIndex;
                Count = uv0Coordinates.Length;
                mCoordinates = new float[uv0Coordinates.Length][];
                for (var i = 0; i < uv0Coordinates.Length; i++)
                {
                    mCoordinates[i] = new[]
                        {
                            uv0Coordinates[i].X,
                            uv0Coordinates[i].Y
                        };
                }
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(Index);
                writer.Write(Count);
                for (var i = 0; i < Count; i++)
                {
                    writer.Write(mCoordinates[i][0]);
                    writer.Write(mCoordinates[i][1]);
                }
            }
        }

        public class VertexData : IComparable<VertexData>
        {
            public Bones Bones;

            public uint ID;

            public ushort Index;

            public Normal Normal;

            public Position Position;

            public TagValue TagValue;

            public UV UV;

            public VertexData(ushort index, Position position, Normal normal, UV uv, Bones bones, TagValue color, uint vertexID)
            {
                ID = vertexID;
                Bones = bones;
                Index = index;
                Normal = normal;
                Position = position;
                TagValue = color;
                UV = uv;
            }

            public int CompareTo(VertexData other)
            {
                if (other == null)
                {
                    throw new ArgumentException("VertexData is null!");
                }
                return Index.CompareTo(other.Index);
            }

            public bool Equals(VertexData other)
            {
                return Position.Equals(other.Position) && Normal.Equals(other.Normal) && UV.Equals(other.UV);
            }
        }

        public class VertexFormat
        {
            public byte FormatDataLength;

            public int FormatDataType, FormatSubType;

            public VertexFormat()
            {
            }

            public VertexFormat(int dataType, int subType, byte bytesPer)
            {
                FormatDataType = dataType;
                FormatSubType = subType;
                FormatDataLength = bytesPer;
            }

            public VertexFormat(BinaryReader reader)
            {
                FormatDataType = reader.ReadInt32();
                FormatSubType = reader.ReadInt32();
                FormatDataLength = reader.ReadByte();
            }

            public VertexFormat(VertexFormat source)
            {
                FormatDataType = source.FormatDataType;
                FormatSubType = source.FormatSubType;
                FormatDataLength = source.FormatDataLength;
            }

            public override string ToString()
            {
                return Enum.GetName(typeof(VertexFormatNames), FormatDataType);
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(FormatDataType);
                writer.Write(FormatSubType);
                writer.Write(FormatDataLength);
            }
        }

        public GEOM()
        {
        }

        public GEOM(GEOM baseMesh, BGEO bgeo, int bgeoSection1EntryNumber, int lod)
        {
            if (!baseMesh.IsValid || !baseMesh.IsBase || baseMesh.VertexCount <= 0)
            {
                throw new MeshException("Invalid base mesh, cannot construct new mesh!");
            }
            mVersion1 = baseMesh.mVersion1;
            mCount = baseMesh.mCount;
            mIndexCount = baseMesh.mIndexCount;
            mExternalCount = baseMesh.mExternalCount;
            mInternalCount = baseMesh.mInternalCount;
            mDummyTGI = new TGI(baseMesh.mDummyTGI.Type, 0, 0);
            mAbsolutePosition = baseMesh.mAbsolutePosition;
            mMagic = baseMesh.mMagic;
            mVersion = baseMesh.mVersion;
            mShaderHash = 0;
            mMTNFSize = 0;
            mMergeGroup = baseMesh.mMergeGroup;
            mSortOrder = baseMesh.mSortOrder;
            mVertexCount = baseMesh.mVertexCount;
            mFaceCount = 3;
            mVertexFormats = new[]
                {
                    new VertexFormat(1, 1, 12),
                    new VertexFormat(2, 1, 12),
                    new VertexFormat(10, 4, 4)
                };
            mPositions = new Position[mVertexCount];
            mNormals = new Normal[mVertexCount];
            mVertexIDs = baseMesh.mVertexIDs;
            mSubMeshCount = baseMesh.mSubMeshCount;
            mBytesPerFacePoint = baseMesh.mBytesPerFacePoint;
            mFacePointCount = baseMesh.mFacePointCount;
            mFaces = baseMesh.mFaces;
            mSKCONIndex = 0;
            mBoneHashCount = baseMesh.mBoneHashCount;
            mBoneHashArray = baseMesh.mBoneHashArray;
            mTGICount = 1;
            mTGIs = new TGI[]
                {
                    new TGI(0, 0, 0)
                };
            for (var i = 0; i < mVertexCount; i++)
            {
                mPositions[i] = new Position();
                mNormals[i] = new Normal();
            }
            int currentVertexID = bgeo.GetLODStartVertexID(bgeoSection1EntryNumber, lod),
            section2StartIndex = bgeo.GetSection2StartIndex(bgeoSection1EntryNumber, lod),
            section3Index = bgeo.GetSection3StartIndex(bgeoSection1EntryNumber, lod) + bgeo.GetLODInitialOffset(bgeoSection1EntryNumber, lod);
            float[] normalDeltas = null,
            positionDeltas = null;
            for (var i = section2StartIndex; i < section2StartIndex + bgeo.GetSection2Count(bgeoSection1EntryNumber, lod); i++)
            {                                                           //navigate list of flags and offsets in section 2
                var section2 = bgeo.GetSection2(i);
                if (section2.HasPosition || section2.HasNormals)        //check flags for whether position data/normals data is present
                {
                    section3Index = section3Index + section2.Offset;    //position to section 3 index
                    var advance = 0;
                    positionDeltas = new float[]
                        {
                            0,
                            0,
                            0
                        };
                    normalDeltas = new float[]
                        {
                            0,
                            0,
                            0
                        };
                    if (section2.HasPosition)                           //read position data if present
                    {
                        positionDeltas = bgeo.GetSection3(section3Index);
                        advance = 1;
                    }
                    if (section2.HasNormals)                            //normal data follows if present
                    {
                        normalDeltas = bgeo.GetSection3(section3Index + advance);
                    }
                    for (var j = 0; j < VertexCount; j++)               //search mesh for all vertices with matching vertex ID
                    {
                        if (mVertexIDs != null && mVertexIDs[j] == currentVertexID)
                        {
                            mPositions[j].AddDeltas(positionDeltas);
                            mNormals[j].AddDeltas(normalDeltas);
                        }
                    }
                }
                currentVertexID += 1;
            }
        }

        public GEOM(GEOM baseMesh, Vector3[] deltaPositions, Vector3[] deltaNormals)
        {
            if (!baseMesh.IsValid || !baseMesh.IsBase || baseMesh.mVertexCount <= 0)
            {
                throw new MeshException("Invalid base mesh, cannot construct new mesh!");
            }
            if (baseMesh.VertexCount != deltaPositions.Length || baseMesh.VertexCount != deltaNormals.Length)
            {
                throw new MeshException("Lists of positions and normals do not match number of base mesh vertices!");
            }
            mVersion1 = baseMesh.mVersion1;
            mCount = baseMesh.mCount;
            mIndexCount = baseMesh.mIndexCount;
            mExternalCount = baseMesh.mExternalCount;
            mInternalCount = baseMesh.mInternalCount;
            mDummyTGI = new TGI(baseMesh.mDummyTGI.Type, 0, 0);
            mAbsolutePosition = baseMesh.mAbsolutePosition;
            mMagic = baseMesh.mMagic;
            mVersion = baseMesh.mVersion;
            mShaderHash = 0;
            mMTNFSize = 0;
            mMergeGroup = baseMesh.mMergeGroup;
            mSortOrder = baseMesh.mSortOrder;
            mVertexCount = baseMesh.VertexCount;
            mFaceCount = 3;
            mVertexFormats = new[]
                {
                    new VertexFormat(1, 1, 12),
                    new VertexFormat(2, 1, 12),
                    new VertexFormat(10, 4, 4)
                };
            mPositions = new Position[mVertexCount];
            mNormals = new Normal[mVertexCount];
            mVertexIDs = baseMesh.mVertexIDs;
            mSubMeshCount = baseMesh.mSubMeshCount;
            mBytesPerFacePoint = baseMesh.mBytesPerFacePoint;
            mFacePointCount = baseMesh.mFacePointCount;
            mFaces = baseMesh.mFaces;
            mSKCONIndex = 0;
            mBoneHashCount = baseMesh.mBoneHashCount;
            mBoneHashArray = baseMesh.mBoneHashArray;
            mTGICount = 1;
            mTGIs = new TGI[]
                {
                    new TGI(0, 0, 0)
                };
            for (var i = 0; i < mVertexCount; i++)
            {
                mPositions[i] = new Position(deltaPositions[i].X, deltaPositions[i].Y, deltaPositions[i].Z);
                mNormals[i] = new Normal(deltaNormals[i].X, deltaNormals[i].Y, deltaNormals[i].Z);
            }
        }

        public GEOM(BinaryReader reader)
        {
            Read(reader);
        }

        public GEOM(GEOM source)
        {
            mVersion1 = source.mVersion1;
            mCount = source.mCount;
            mIndexCount = source.mIndexCount;
            mExternalCount = source.mExternalCount;
            mInternalCount = source.mInternalCount;
            mDummyTGI = new TGI(source.mDummyTGI);
            mAbsolutePosition = source.mAbsolutePosition;
            mMagic = source.mMagic;
            mVersion = source.mVersion;
            mShaderHash = source.mShaderHash;
            mMTNFSize = source.mMTNFSize;
            if (mShaderHash > 0)
            {
                mMTNF = new MTNF(source.mMTNF);
            }
            mMergeGroup = source.mMergeGroup;
            mSortOrder = source.mSortOrder;
            mVertexCount = source.VertexCount;
            mFaceCount = source.mFaceCount;
            mVertexFormats = new VertexFormat[source.mFaceCount];
            for (var i = 0; i < source.mFaceCount; i++)
            {
                mVertexFormats[i] = new VertexFormat(source.mVertexFormats[i]);
            }
            if (source.HasBones)
            {
                mBones = new Bones[mVertexCount];
            }
            for (var i = 0; i < mVertexFormats.Length; i++)
            {
                switch (mVertexFormats[i].FormatDataType)
                {
                    case 1:
                        mPositions = new Position[mVertexCount];
                        for (var j = 0; j < mVertexCount; j++)
                        {
                            mPositions[j] = new Position(source.mPositions[j]);
                        }
                        break;
                    case 2:
                        mNormals = new Normal[mVertexCount];
                        for (var j = 0; j < mVertexCount; j++)
                        {
                            mNormals[j] = new Normal(source.mNormals[j]);
                        }
                        break;
                    case 3:
                        mUVs = new UV[source.mUVs.Length][];
                        for (var j = 0; j < source.mUVs.Length; j++)
                        {
                            mUVs[j] = new UV[mVertexCount];
                            for (var k = 0; k < mVertexCount; k++)
                            {
                                mUVs[j][k] = new UV(source.mUVs[j][k]);
                            }
                        }
                        break;
                    case 4:
                        for (var j = 0; j < mVertexCount; j++)
                        {
                            if (mBones[j] == null)
                            {
                                mBones[j] = new Bones();
                            }
                            byte[] temp =
                                {
                                    source.mBones[j].BoneAssignments[0],
                                    source.mBones[j].BoneAssignments[1], 
                                    source.mBones[j].BoneAssignments[2],
                                    source.mBones[j].BoneAssignments[3]
                                };
                            mBones[j].BoneAssignments = temp;
                        }
                        break;
                    case 5:
                        for (var j = 0; j < mVertexCount; j++)
                        {
                            if (mBones[j] == null)
                            {
                                mBones[j] = new Bones();
                            }
                            float[] temp =
                                {
                                    source.mBones[j].BoneWeightsV5[0],
                                    source.mBones[j].BoneWeightsV5[1], 
                                    source.mBones[j].BoneWeightsV5[2],
                                    source.mBones[j].BoneWeightsV5[3]
                                };
                            mBones[j].BoneWeightsV5 = temp;
                        }
                        break;
                    case 6:
                        mTangents = new Tangent[mVertexCount];
                        for (var j = 0; j < mVertexCount; j++)
                        {
                            mTangents[j] = new Tangent(source.mTangents[j]);
                        }
                        break;
                    case 7:
                        mTags = new TagValue[mVertexCount];
                        for (var j = 0; j < mVertexCount; j++)
                        {
                            mTags[j] = new TagValue(source.mTags[j]);
                        }
                        break;
                    case 10:
                        mVertexIDs = new int[mVertexCount];
                        for (var j = 0; j < mVertexCount; j++)
                        {
                            mVertexIDs[j] = source.mVertexIDs[j];
                        }
                        break;
                }
            }
            mSubMeshCount = source.mSubMeshCount;
            mBytesPerFacePoint = source.mBytesPerFacePoint;
            mFacePointCount = source.mFacePointCount;
            mFaces = new Face[source.mFaces.Length];
            for (var i = 0; i < source.mFaces.Length; i++)
            {
                mFaces[i] = new Face(source.mFaces[i]);
            }
            if (source.mVersion == 5)
            {
                mSKCONIndex = source.mSKCONIndex;
            }
            else if (source.mVersion >= 12)
            {
                mUVStitchCount = source.mUVStitchCount;
                mUVStitches = new UVStitch[mUVStitchCount];
                for (var i = 0; i < mUVStitchCount; i++)
                {
                    mUVStitches[i] = new UVStitch(source.mUVStitches[i]);
                }
                if (source.mVersion >= 13)
                {
                    mSeamStitchCount = source.mSeamStitchCount;
                    mSeamStitches = new SeamStitch[mSeamStitchCount];
                    for (var i = 0; i < mSeamStitchCount; i++)
                    {
                        mSeamStitches[i] = new SeamStitch(source.mSeamStitches[i]);
                    }
                }
                mSlotCount = source.mSlotCount;
                mSlotrayIntersections = new SlotrayIntersection[mSlotCount];
                for (var i = 0; i < mSlotCount; i++)
                {
                    mSlotrayIntersections[i] = new SlotrayIntersection(source.mSlotrayIntersections[i]);
                }
            }
            mBoneHashCount = source.mBoneHashCount;
            mBoneHashArray = new uint[source.mBoneHashArray.Length];
            for (var i = 0; i < source.mBoneHashArray.Length; i++)
            {
                mBoneHashArray[i] = source.mBoneHashArray[i];
            }
            mTGICount = source.mTGICount;
            mTGIs = new TGI[source.mTGIs.Length];
            for (var i = 0; i < source.mTGIs.Length; i++)
            {
                mTGIs[i] = new TGI(source.mTGIs[i]);
            }
            CopyFaceMorphs = source.CopyFaceMorphs;
        }

        public void AppendMesh(GEOM meshToAppend)
        {
            if (meshToAppend == null)
            {
                return;
            }
            if (!IsValid)
            {
                throw new MeshException("Not a valid mesh!");
            }
            if (!meshToAppend.IsValid)
            {
                throw new MeshException("The mesh to be appended is not a valid mesh!");
            }
            int uvIndex = 0;
            for (var i = 0; i < VertexFormatList.Length; i++)
            {
                switch (VertexFormatList[i])
                {
                    case 1:
                        var newPositions = new Position[mVertexCount + meshToAppend.mVertexCount];
                        Array.Copy(mPositions, 0, newPositions, 0, mVertexCount);
                        Array.Copy(meshToAppend.mPositions, 0, newPositions, mVertexCount, meshToAppend.mVertexCount);
                        mPositions = newPositions;
                        break;
                    case 2:
                        var newNormals = new Normal[mVertexCount + meshToAppend.mVertexCount];
                        Array.Copy(mNormals, 0, newNormals, 0, mVertexCount);
                        Array.Copy(meshToAppend.mNormals, 0, newNormals, mVertexCount, meshToAppend.mVertexCount);
                        mNormals = newNormals;
                        break;
                    case 3:
                        uvIndex += 1;
                        break;
                    case 4:
                        var newBones = new Bones[mVertexCount + meshToAppend.mVertexCount];
                        var tempBoneHash = new List<uint>(mBoneHashArray);
                        foreach (var hash in meshToAppend.mBoneHashArray)
                        {
                            if (tempBoneHash.IndexOf(hash) < 0)
                            {
                                tempBoneHash.Add(hash);
                            }
                        }
                        var newBoneHashArray = tempBoneHash.ToArray();
                        for (var j = 0; j < mVertexCount; j++)
                        {
                            byte[] oldBones = GetBones(j),
                            tempBones = new byte[oldBones.Length];
                            var oldWeights = GetBoneWeightsV5(j);
                            for (var k = 0; k < oldBones.Length; k++)
                            {
                                if (oldWeights[k] > 0 && oldBones[k] < mBoneHashArray.Length)
                                {
                                    tempBones[k] = (byte)Array.IndexOf(newBoneHashArray, mBoneHashArray[oldBones[k]]);
                                }
                                else
                                {
                                    tempBones[k] = 0;
                                }
                            }
                            newBones[j] = new Bones(tempBones, oldWeights);
                        }
                        for (var j = 0; j < meshToAppend.mVertexCount; j++)
                        {
                            byte[] oldBones = meshToAppend.GetBones(j),
                            tempBones = new byte[oldBones.Length];
                            var oldWeights = meshToAppend.GetBoneWeightsV5(j);
                            for (var k = 0; k < oldBones.Length; k++)
                            {
                                if (oldWeights[k] > 0 && oldBones[k] < meshToAppend.mBoneHashArray.Length)
                                {
                                    tempBones[k] = (byte)Array.IndexOf(newBoneHashArray, meshToAppend.mBoneHashArray[oldBones[k]]);
                                }
                                else
                                {
                                    tempBones[k] = 0;
                                }
                            }
                            newBones[j + mVertexCount] = new Bones(tempBones, oldWeights);
                        }
                        mBones = newBones;
                        mBoneHashArray = newBoneHashArray;
                        mBoneHashCount = newBoneHashArray.Length;
                        break;
                    case 6:
                        var newTangent = new Tangent[mVertexCount + meshToAppend.mVertexCount];
                        Array.Copy(mTangents, 0, newTangent, 0, mVertexCount);
                        if (meshToAppend.HasTangents)
                        {
                            Array.Copy(meshToAppend.mTangents, 0, newTangent, mVertexCount, meshToAppend.mVertexCount);
                        }
                        else
                        {
                            for (var v = mVertexCount; v < newTangent.Length; v++)
                            {
                                newTangent[v] = new Tangent();
                            }
                        }
                        mTangents = newTangent;
                        break;
                    case 7:
                        var newTag = new TagValue[mVertexCount + meshToAppend.mVertexCount];
                        Array.Copy(mTags, 0, newTag, 0, mVertexCount);
                        if (meshToAppend.HasTags)
                        {
                            Array.Copy(meshToAppend.mTags, 0, newTag, mVertexCount, meshToAppend.mVertexCount);
                        }
                        else
                        {
                            for (var v = mVertexCount; v < newTag.Length; v++)
                            {
                                newTag[v] = new TagValue();
                            }
                        }
                        mTags = newTag;
                        break;
                    case 10:
                        var newIDs = new int[mVertexCount + meshToAppend.mVertexCount];
                        Array.Copy(mVertexIDs, 0, newIDs, 0, mVertexCount);
                        if (meshToAppend.HasVertexIDs)
                        {
                            Array.Copy(meshToAppend.mVertexIDs, 0, newIDs, mVertexCount, meshToAppend.mVertexCount);
                        }
                        mVertexIDs = newIDs;
                        break;
                }
            }
            if (uvIndex > 0)
            {
                var newUV = new UV[uvIndex][];
                for (var i = 0; i < uvIndex; i++)
                {
                    newUV[i] = new UV[mVertexCount + meshToAppend.mVertexCount];
                    for (var j = 0; j < mVertexCount; j++)
                    {
                        newUV[i][j] = mUVs[i][j];
                    }
                    if (meshToAppend.HasUVSet(i))
                    {
                        for (var j = 0; j < meshToAppend.mVertexCount; j++)
                        {
                            newUV[i][j + mVertexCount] = meshToAppend.mUVs[i][j];
                        }
                    }
                    else
                    {
                        for (var j = 0; j < meshToAppend.mVertexCount; j++)
                        {
                            newUV[i][j + mVertexCount] = new UV();
                        }
                    }
                }
                mUVs = newUV;
            }
            var newFaces = new Face[FaceCount + meshToAppend.FaceCount];
            Array.Copy(mFaces, 0, newFaces, 0, FaceCount);
            for (var i = 0; i < meshToAppend.FaceCount; i++)
            {
                newFaces[i + FaceCount] = new Face(meshToAppend.mFaces[i].FacePoint0 + mVertexCount, meshToAppend.mFaces[i].FacePoint1 + mVertexCount, meshToAppend.mFaces[i].FacePoint2 + mVertexCount);
            }
            mFaces = newFaces;
            if (mVersion >= 12)
            {
                var adj0 = new UVStitch[(mUVStitches == null ? 0 : mUVStitches.Length) + (meshToAppend.mUVStitches == null ? 0 : meshToAppend.mUVStitches.Length)];
                if (mUVStitches != null)
                {
                    Array.Copy(mUVStitches, 0, adj0, 0, mUVStitches.Length);
                }
                if (meshToAppend.mUVStitches != null)
                {
                    for (var i = 0; i < meshToAppend.mUVStitches.Length; i++)
                    {
                        adj0[i + mUVStitches.Length] = new UVStitch(meshToAppend.mUVStitches[i]);
                        adj0[i + mUVStitches.Length].Index += mVertexCount;
                    }
                }
                mUVStitches = adj0;
                mUVStitchCount = adj0.Length;
                if (mVersion >= 13)
                {
                    var seam = new SeamStitch[(mSeamStitches == null ? 0 : mSeamStitches.Length) + (meshToAppend.mSeamStitches == null ? 0 : meshToAppend.mSeamStitches.Length)];
                    if (mSeamStitches != null)
                    {
                        Array.Copy(mSeamStitches, 0, seam, 0, mSeamStitches.Length);
                    }
                    if (meshToAppend.mSeamStitches != null)
                    {
                        for (var i = 0; i < meshToAppend.mSeamStitches.Length; i++)
                        {
                            seam[i + mSeamStitches.Length] = new SeamStitch(meshToAppend.mSeamStitches[i]);
                            seam[i + mSeamStitches.Length].Index += (uint)mVertexCount;
                        }
                    }
                    mSeamStitches = seam;
                    mSeamStitchCount = seam.Length;
                }
                var adj1 = new SlotrayIntersection[(mSlotrayIntersections == null ? 0 : mSlotrayIntersections.Length) + (meshToAppend.mSlotrayIntersections == null ? 0 : meshToAppend.mSlotrayIntersections.Length)];
                if (mSlotrayIntersections != null)
                {
                    Array.Copy(mSlotrayIntersections, 0, adj1, 0, mSlotrayIntersections.Length);
                }
                if (meshToAppend.mSlotrayIntersections != null)
                {
                    for (var i = 0; i < meshToAppend.mSlotrayIntersections.Length; i++)
                    {
                        adj1[i + mSlotrayIntersections.Length] = new SlotrayIntersection(meshToAppend.mSlotrayIntersections[i]);
                        var indices = meshToAppend.mSlotrayIntersections[i].TrianglePointIndices;
                        for (var j = 0; j < indices.Length; j++)
                        {
                            indices[j] += mVertexCount;
                        }
                        adj1[i + mSlotrayIntersections.Length].TrianglePointIndices = indices;
                    }
                }
                mSlotrayIntersections = adj1;
                mSlotCount = adj1.Length;
            }
            mVertexCount += meshToAppend.mVertexCount;
            mFacePointCount += meshToAppend.mFacePointCount;
        }

        public void AutoBone(GEOM refMesh, bool unassignedVerticesOnly, bool interpolate, int interpolationPointCount, float weightingFactor, bool restrictToFace, Action updateProgressCallback = null)
        {
            uint[] newBoneHashList,
            refBoneHashList = refMesh.BoneHashList;
            if (unassignedVerticesOnly)
            {
                var tempBoneHashList = new List<uint>(BoneHashList);
                for (var i = 0; i < refMesh.BoneHashList.Length; i++)
                {
                    if (Array.IndexOf(BoneHashList, refBoneHashList[i]) < 0)
                    {
                        tempBoneHashList.Add(refBoneHashList[i]);
                    }
                }
                newBoneHashList = tempBoneHashList.ToArray();
            }
            else
            {
                newBoneHashList = refMesh.BoneHashList;
            }
            SetBoneHashList(newBoneHashList);
            var refVertices = new Vector3[refMesh.VertexCount];
            for (var i = 0; i < refMesh.VertexCount; i++)
            {
                refVertices[i] = new Vector3(refMesh.GetPosition(i));
            }
            var refFaces = new int[refMesh.FaceCount][];
            for (var i = 0; i < refMesh.FaceCount; i++)
            {
                refFaces[i] = refMesh.GetFaceIndices(i);
            }
            var stepIt = 0;
            for (var i = 0; i < VertexCount; i++)
            {
                stepIt++;
                if (stepIt >= 100)
                {
                    if (updateProgressCallback != null)
                    {
                        updateProgressCallback();
                    }
                    stepIt = 0;
                }
                if (unassignedVerticesOnly && GetBoneWeightsV5(i)[0] > 0 && ValidBones(i))
                {
                    continue;
                }
                var position = new Vector3(GetPosition(i));
                var refPoints = position.GetReferenceMeshPoints(refVertices, refFaces, interpolate, restrictToFace, interpolationPointCount);
                var refArray = new Vector3[refPoints.Length];
                for (var j = 0; j < refPoints.Length; j++)
                {
                    refArray[j] = new Vector3(refMesh.GetPosition(refPoints[j]));
                }
                var newBones = new List<byte>();
                var newWeights = new List<float>();
                var valueWeights = position.GetInterpolationWeights(refArray, weightingFactor);
                for (var j = 0; j < refPoints.Length; j++)
                {
                    var refBones = refMesh.GetBones(refPoints[j]);
                    var refWeights = refMesh.GetBoneWeightsV5(refPoints[j]);
                    for (var k = 0; k < refBones.Length; k++)
                    {
                        if (refBones[k] < refBoneHashList.Length)
                        {
                            var index = newBones.IndexOf(refBones[k]);
                            if (index >= 0)
                            {
                                newWeights[index] += valueWeights[j] * refWeights[k];
                            }
                            else
                            {
                                newBones.Add(refBones[k]);
                                newWeights.Add(valueWeights[j] * refWeights[k]);
                            }
                        }
                    }
                }
                SortBones(ref newBones, ref newWeights);
                for (var j = newBones.Count; j < 4; j++)
                {
                    newBones.Add((byte)(newBoneHashList.Length));
                    newWeights.Add(0);
                }
                for (var j = 0; j < 4; j++)
                {
                    if (newBones[j] < refBoneHashList.Length)
                    {
                        newBones[j] = (byte)Array.IndexOf(newBoneHashList, refBoneHashList[newBones[j]]);
                    }
                }
                SetBones(i, newBones.GetRange(0, 4).ToArray());
                SetBoneWeightsV5(i, newWeights.GetRange(0, 4).ToArray());
            }
        }

        public void AutoSeamStitches(Species species, AgeGender age, AgeGender gender, int lod)
        {
            var seamStitches = new List<SeamStitch>();
            for (var i = 0; i < mVertexCount; i++)
            {
                foreach (var seam in Enum.GetValues(typeof(SeamType)))
                {
                    var vertices = GetSeamVertexPositions(species, age, gender, lod, (SeamType)seam);
                    for (var j = 0; j < vertices.Length; j++)
                    {
                        if (vertices[j].PositionMatches(mPositions[i].Coordinates))
                        {
                            seamStitches.Add(new SeamStitch(i, (SeamType)seam, j, mUVs[0]));
                        }
                    }
                }
            }
            mSeamStitches = seamStitches.ToArray();
            mSeamStitchCount = mSeamStitches.Length;
        }

        public void AutoUV1Stitches()
        {
            var newStitch = new List<UVStitch>();
            for (var i = 0; i < mVertexCount; i++)
            {
                var position = new Vector3(GetPosition(i));
                var stitches = new List<Vector2>();
                for (var j = 0; j < mVertexCount; j++)
                {
                    if (position.PositionMatches(GetPosition(j)))
                    {
                        var tempUV = GetUV(j, 1);
                        tempUV[0] = Math.Abs(tempUV[0]);
                        var temp = new Vector2(tempUV);
                        if (stitches.IndexOf(temp) < 0)
                        {
                            stitches.Add(temp);
                        }
                    }
                }
                if (stitches.Count > 0)
                {
                    newStitch.Add(new UVStitch(i, stitches.ToArray()));
                }
            }
            mUVStitches = newStitch.ToArray();
            mUVStitchCount = mUVStitches.Length;
        }

        public void AutoVertexID(GEOM refMesh)
        {
            if (!HasVertexIDs)
            {
                InsertVertexIDInFormatList();
                mVertexIDs = new int[VertexCount];
            }
            var refVertices = new Vector3[refMesh.VertexCount];
            for (var i = 0; i < refMesh.VertexCount; i++)
            {
                refVertices[i] = new Vector3(refMesh.GetPosition(i));
            }
            for (var i = 0; i < VertexCount; i++)
            {
                mVertexIDs[i] = refMesh.mVertexIDs[new Vector3(GetPosition(i)).NearestPointIndexSimple(refVertices)];
            }
            CopyFaceMorphs = true;
        }

        public void BoneDeVectorize(Rig.Bone bone, float weight)
        {
            if (weight == 0)
            {
                return;
            }
            var bonePosition = bone.WorldPosition;
            for (var i = 0; i < mPositions.Length; i++)
            {
                var vertexWeight = GetBoneWeightForVertex(i, bone.BoneHash);
                if (vertexWeight == 0)
                {
                    continue;
                }
                Vector3 newPosition = DeltaPosition[i] + bonePosition,
                oldPosition = new Vector3(mPositions[i].Coordinates);
                oldPosition += (newPosition - oldPosition) * vertexWeight * weight;
                mPositions[i] = new Position(oldPosition.Coordinates);
            }
        }

        public void BoneMorpher(Rig.Bone bone, float weight, Vector3 offset, Vector3 scale, Quaternion rotation)
        {
            if (weight == 0)
            {
                return;
            }
            var unit = new Vector3(1, 1, 1);
            var boneHashes = new List<uint>();
            bone.Rig.GetDescendants(bone.BoneHash, ref boneHashes);
            for (var i = 0; i < mPositions.Length; i++)
            {
                var boneWeight = GetTotalBoneWeight(i, boneHashes);
                if (boneWeight == 0)
                {
                    continue;
                }
                var adjustedWeight = boneWeight * weight;
                Vector3 vertexNormal = new Vector3(GetNormal(i)),
                vertexPosition = new Vector3(GetPosition(i)) - bone.WorldPosition,
                weightedOffset = offset * adjustedWeight,
                weightedScale = (scale * adjustedWeight) + unit;
                var weightedRotation = rotation * adjustedWeight;
                var weightedTransform = weightedRotation.ToMatrix4D(weightedOffset, weightedScale);
                vertexPosition = weightedTransform * vertexPosition;
                vertexPosition += bone.WorldPosition;
                DeltaPosition[i] += vertexPosition - new Vector3(GetPosition(i));
                var normalTransform = weightedTransform.Inverse().Transpose();
                vertexNormal = (normalTransform * vertexNormal);
                vertexNormal.Normalize();
                SetNormal(i, vertexNormal.Coordinates);
            }
        }

        public void BoneMorpher2(Rig.Bone bone, float weight, Vector3 scale, Quaternion rotation)
        {
            if (weight == 0)
            {
                return;
            }
            var unit = new Vector3(1, 1, 1);
            for (var i = 0; i < mPositions.Length; i++)
            {
                var boneWeight = GetBoneWeightForVertex(i, bone.BoneHash);
                if (boneWeight == 0)
                {
                    continue;
                }
                Vector3 vertexNormal = new Vector3(GetNormal(i)) - bone.WorldPosition,
                weightedScale = (scale * boneWeight * weight) + unit;
                var weightedRotation = rotation * boneWeight * weight;
                DeltaPosition[i] = DeltaPosition[i].Scale(weightedScale);
                DeltaPosition[i] = (weightedRotation * DeltaPosition[i] * weightedRotation.Conjugate()).ToVector3();
                vertexNormal = (weightedRotation * vertexNormal * weightedRotation.Conjugate()).ToVector3();
                vertexNormal += bone.WorldPosition;
                SetNormal(i, vertexNormal.Coordinates);
            }
        }

        public void BoneVectorize(Rig.Bone bone)
        {
            var bonePosition = bone.WorldPosition;
            DeltaPosition = new Vector3[mPositions.Length];
            for (var i = 0; i < mPositions.Length; i++)
            {
                DeltaPosition[i] = (new Vector3(mPositions[i].Coordinates)) - bonePosition;
            }
        }

        public bool CalculateTangents(bool showError = true)
        {
            //Code adapted from NOAA_Julien on Unity forums (and adapted again by Destrospean from CmarNYC's adaptation)
            var triangles = new int[FaceCount * 3];
            for (var i = 0; i < FaceCount; i++)
            {
                var temp = GetFaceIndices(i);
                triangles[i * 3] = temp[0];
                triangles[i * 3 + 1] = temp[1];
                triangles[i * 3 + 2] = temp[2];
            }
            Vector3[] normals = new Vector3[VertexCount],
            vertices = new Vector3[VertexCount];
            var uvs = new Vector2[VertexCount];
            for (var i = 0; i < VertexCount; i++)
            {
                vertices[i] = new Vector3(GetPosition(i));
                uvs[i] = new Vector2(GetUV(i, 0));
                normals[i] = new Vector3(GetNormal(i));
            }
            int triangleCount = triangles.Length,
            vertexCount = vertices.Length;
            var tan1 = new Vector3[vertexCount];
            for (var i = 0; i < triangleCount; i += 3)
            {
                int i1 = triangles[i],
                i2 = triangles[i + 1],
                i3 = triangles[i + 2];
                Vector3 v1 = vertices[i1],
                v2 = vertices[i2],
                v3 = vertices[i3];
                Vector2 w1 = uvs[i1],
                w2 = uvs[i2],
                w3 = uvs[i3];
                float x1 = v2.X - v1.X,
                x2 = v3.X - v1.X,
                y1 = v2.Y - v1.Y,
                y2 = v3.Y - v1.Y,
                z1 = v2.Z - v1.Z,
                z2 = v3.Z - v1.Z,
                s1 = w2.X - w1.X,
                s2 = w3.X - w1.X,
                t1 = w2.Y - w1.Y,
                t2 = w3.Y - w1.Y,
                r = 1 / (s1 * t2 - s2 * t1);
                var sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                tan1[i1] += sdir;
                tan1[i2] += sdir;
                tan1[i3] += sdir;
            }
            var normalizeError = false;
            for (var i = 0; i < vertexCount; ++i)
            {
                Vector3 n = normals[i],
                t = tan1[i],
                temp = t - n * Vector3.Dot(n, t);
                try
                {
                    temp.Normalize();
                    SetTangent(i, temp.X, temp.Y, temp.Z);
                }
                catch (Exception e)
                {
                    if (String.CompareOrdinal(e.Message, "Cannot normalize a vector with magnitude of zero!") == 0 && showError && !normalizeError)
                    {
                        /*
                        if (MessageBox.Show("At least one triangle has a side of zero length." + System.Environment.NewLine + "Skip bad triangles and continue?", "Mesh Error Found", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        {
                            return false;
                        }
                        */
                        normalizeError = true;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return true;
        }

        public void Clean()
        {
            var indexTrans0 = new int[mVertexCount];
            for (var i = 0; i < indexTrans0.Length; i++)
            {
                indexTrans0[i] = i;
            }
            for (var i = 0; i < mVertexCount - 1; i++)
            {
                if (indexTrans0[i] != i)
                {
                    continue;
                }
                for (var j = i + 1; j < mVertexCount; j++)
                {
                    if (mPositions[i].Equals(mPositions[j]) & mNormals[i].Equals(mNormals[j]))
                    {
                        var match = true;
                        for (var k = 0; k < UVCount; k++)
                        {
                            if (!mUVs[k][i].CloseTo(mUVs[k][j]))
                            {
                                match = false;
                            }
                        }
                        if (match)
                        {
                            indexTrans0[j] = i;
                        }
                    }
                }
            }
            var indexTrans1 = new List<int>();
            for (var i = 0; i < indexTrans0.Length; i++)
            {
                if (indexTrans0[i] == i)
                {
                    indexTrans1.Add(i);
                }
            }
            for (var i = 0; i < indexTrans0.Length; i++)
            {
                indexTrans0[i] = indexTrans1.IndexOf(indexTrans0[i]);
            }
            var newSlots = new List<SlotrayIntersection>();
            for (var i = 0; i < mFaces.Length; i++)
            {
                int facePoint0 = indexTrans0[mFaces[i].FacePoint0],
                facePoint1 = indexTrans0[mFaces[i].FacePoint1],
                facePoint2 = indexTrans0[mFaces[i].FacePoint2];
                for (var j = 0; j < SlotrayAdjustments.Length; j++)
                {
                    var slotVertIndices = mSlotrayIntersections[j].TrianglePointIndices;
                    if (mFaces[i].FacePoint0 == slotVertIndices[0] && mFaces[i].FacePoint1 == slotVertIndices[1] && mFaces[i].FacePoint2 == slotVertIndices[2])
                    {
                        var temp = new SlotrayIntersection(mSlotrayIntersections[j]);
                        temp.TrianglePointIndices = new[]
                            {
                                facePoint0,
                                facePoint1,
                                facePoint2
                            };
                        newSlots.Add(temp);
                    }
                }
                mFaces[i] = new Face(facePoint0, facePoint1, facePoint2);
            }
            SlotrayAdjustments = newSlots.ToArray();
            mSlotCount = newSlots.Count;
            var newPositions = new List<Position>();
            var newNormals = new List<Normal>();
            var newUVs = new List<UV>[UVCount];
            for (var i = 0; i < UVCount; i++)
            {
                newUVs[i] = new List<UV>();
            }
            var newBones = new List<Bones>();
            var newTangents = new List<Tangent>();
            var newTagValues = new List<TagValue>();
            var newIDs = new List<int>();
            for (var i = 0; i < indexTrans1.Count; i++)
            {
                newPositions.Add(new Position(mPositions[indexTrans1[i]]));
                newNormals.Add(new Normal(mNormals[indexTrans1[i]]));
                for (var j = 0; j < UVCount; j++)
                {
                    newUVs[j].Add(new UV(mUVs[j][indexTrans1[i]]));
                }
                newBones.Add(new Bones(mBones[indexTrans1[i]]));
                if (HasTangents)
                {
                    newTangents.Add(new Tangent(mTangents[indexTrans1[i]]));
                }
                if (HasTags)
                {
                    newTagValues.Add(new TagValue(mTags[indexTrans1[i]]));
                }
                if (HasVertexIDs)
                {
                    newIDs.Add(mVertexIDs[indexTrans1[i]]);
                }
            }
            mPositions = newPositions.ToArray();
            mNormals = newNormals.ToArray();
            for (var i = 0; i < UVCount; i++)
            {
                mUVs[i] = newUVs[i].ToArray();
            }
            mBones = newBones.ToArray();
            if (HasTangents)
            {
                mTangents = newTangents.ToArray();
            }
            if (HasTags)
            {
                mTags = newTagValues.ToArray();
            }
            if (HasVertexIDs)
            {
                mVertexIDs = newIDs.ToArray();
            }
            mVertexCount = indexTrans1.Count;
            var newSeamStitches = new List<SeamStitch>();
            for (var i = 0; i < indexTrans1.Count; i++)
            {
                for (var j = 0; j < mSeamStitches.Length; j++)
                {
                    if (indexTrans1[i] == mSeamStitches[j].Index)
                    {
                        var seamStitch = new SeamStitch(mSeamStitches[j]);
                        seamStitch.Index = (uint)i;
                        newSeamStitches.Add(seamStitch);
                    }
                }
            }
            mSeamStitches = newSeamStitches.ToArray();
            mSeamStitchCount = newSeamStitches.Count;
            if (HasUVSet(1))
            {
                AutoUV1Stitches();
            }
        }

        public int CompareTo(GEOM other)
        {
            return VertexCount > other.VertexCount ? -1 : VertexCount < other.VertexCount ? 1 : 0;
        }

        public string[] DataString(int lineNumber)
        {
            var vertexFormatList = VertexFormatList;
            var text = new string[vertexFormatList.Length];
            var uvIndex = 0;
            for (var i = 0; i < vertexFormatList.Length; i++)
            {
                switch (vertexFormatList[i])
                {
                    case 1:
                        text[i] = "Position: " + mPositions[lineNumber].ToString();
                        break;
                    case 2:
                        text[i] = "Normals: " + mNormals[lineNumber].ToString();
                        break;
                    case 3:
                        text[i] = "UV: " + mUVs[uvIndex][lineNumber].ToString();
                        uvIndex += 1;
                        break;
                    case 4:
                        text[i] = "Bones: " + mBones[lineNumber].ToString();
                        break;
                    case 6:
                        text[i] = "Tangents: " + mTangents[lineNumber].ToString();
                        break;
                    case 7:
                        text[i] = "TagVals: " + mTags[lineNumber].ToString();
                        break;
                    case 10:
                        text[i] = "Vertex ID: " + mVertexIDs[lineNumber].ToString();
                        break;
                }
            }
            return text;
        }

        /// <summary>
        /// Fills gap at waist when wearing a female top and male bottom
        /// </summary>
        public void FillWaistGap()
        {
            uint femaleWaist = (uint)SeamType.WaistAdultFemale,
            maleWaist = (uint)SeamType.WaistAdultMale;
            List<SeamStitch> femaleVertices = new List<SeamStitch>(),
            maleVertices = new List<SeamStitch>();
            for (var i = 0; i < mSeamStitches.Length; i++)
            {
                if (mSeamStitches[i].SeamType == femaleWaist)
                {
                    femaleVertices.Add(mSeamStitches[i]);
                }
                if (mSeamStitches[i].SeamType == maleWaist)
                {
                    maleVertices.Add(mSeamStitches[i]);
                }
            }
            if (femaleVertices.Count == 0 || maleVertices.Count == 0)
            {
                return;
            }
            femaleVertices.Sort();
            maleVertices.Sort();
            SeamStitch femaleLast = femaleVertices[0],
            maleLast = maleVertices[0];
            femaleVertices.RemoveAt(0);
            femaleVertices.Add(femaleLast);
            maleVertices.RemoveAt(0);
            maleVertices.Add(maleLast);
            if (femaleVertices.Count != maleVertices.Count)
            {
                return;
            }
            var newFaces = new List<Face>(mFaces);
            for (var i = 0; i < femaleVertices.Count - 1; i++)
            {
                newFaces.Add(new Face(femaleVertices[i].Index, femaleVertices[i + 1].Index, maleVertices[i].Index));
                newFaces.Add(new Face(maleVertices[i].Index, femaleVertices[i + 1].Index, maleVertices[i + 1].Index));
            }
            mFaces = newFaces.ToArray();
            mFacePointCount = mFaces.Length * 3;
        }

        public void FixBoneWeights()
        {
            for (var i = 0; i < mVertexCount; i++)
            {
                var boneWeights = GetBoneWeightsV5(i);
                var totalWeight = 0f;
                for (var j = 0; j < 4; j++)
                {
                    totalWeight += boneWeights[j];
                }
                if (!IsEqual(totalWeight, 1))
                {
                    boneWeights[0] += 1 - totalWeight;
                }
                SetBoneWeightsV5(i, boneWeights);
            }
        }

        public void FixUnusedBones()
        {
            if (mBoneHashArray == null || mBoneHashArray.Length == 0)
            {
                return;
            }
            var usedBones = new List<byte>();
            for (var i = 0; i < mVertexCount; i++)
            {
                var sourceBones = GetBones(i);
                var sourceWeights = GetBoneWeightsV5(i);
                for (var j = 0; j < 4; j++)
                {
                    if (sourceWeights[j] > 0 & !(usedBones.IndexOf(sourceBones[j]) >= 0))
                    {
                        usedBones.Add(sourceBones[j]);
                    }
                }
            }
            var usedBoneHash = new List<uint>();
            var oldBoneHash = BoneHashList;
            foreach (var bone in usedBones)
            {
                usedBoneHash.Add(oldBoneHash[bone]);
            }
            for (var i = 0; i < mVertexCount; i++)
            {
                byte[] sourceBones = GetBones(i),
                bones = new byte[sourceBones.Length];
                var sourceWeights = GetBoneWeightsV5(i);
                for (var j = 0; j < 4; j++)
                {
                    if (sourceWeights[j] > 0)
                    {
                        bones[j] = (byte)usedBoneHash.IndexOf(oldBoneHash[sourceBones[j]]);
                    }
                    else
                    {
                        bones[j] = 0;
                    }
                }
                SetBones(i, bones);
            }
            mBoneHashCount = usedBoneHash.Count;
            mBoneHashArray = usedBoneHash.ToArray();
        }

        public static GEOM[] GEOMsFromOBJ(OBJ obj, GEOM refMesh, TGI bumpMapTGI, bool smoothModel, bool cleanModel, Action updateProgressCallback = null)
        {
            if (!refMesh.IsValid || !refMesh.IsBase)
            {
                throw new MeshException("Reference mesh must be a valid base GEOM mesh!");
            }
            var isMorph = new bool[obj.GroupCount];
            for (var i = 0; i < obj.GroupCount; i++)
            {
                isMorph[i] = obj.GroupArray[i].GroupName.Contains("_fat") || obj.GroupArray[i].GroupName.Contains("_fit") || obj.GroupArray[i].GroupName.Contains("_thin") || obj.GroupArray[i].GroupName.Contains("_special");
            }
            if (isMorph[0])
            {
                throw new MeshException("The first mesh group must be a base mesh!");
            }
            if (obj.UVArray.Length == 0)
            {
                /*
                if (MessageBox.Show("This OBJ mesh has no UV mapping. Continue with a blank UV map?", "No UV mapping found", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return null;
                }
                */
                Console.WriteLine("This OBJ mesh has no UV mapping.");
                obj.UVArray = new OBJ.UV[1]
                    {
                        new OBJ.UV()
                    };
            }
            if (obj.NormalArray.Length == 0 && !smoothModel)
            {
                /*
                if (MessageBox.Show("This OBJ mesh has no normals. Continue and calculate normals?", "No normals found", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return null;
                }
                */
                Console.WriteLine("This OBJ mesh has no normals.");
                smoothModel = true;
            }
            if (smoothModel)
            {
                obj.CalculateNormals(true);
            }
            var geoms = new GEOM[obj.GroupCount];
            var currentBase = 0;
            for (var i = 0; i < geoms.Length; i++)
            {
                if (!isMorph[i])
                {
                    currentBase = i;
                }
                List<int[]> faces = new List<int[]>(),
                vertices = new List<int[]>();
                OBJ.Group[] groupArray = 
                    {
                        Array.Find(obj.GroupArray, x => x.GroupName.EndsWith("_base")),
                        Array.Find(obj.GroupArray, x => x.GroupName.EndsWith("_fat")),
                        Array.Find(obj.GroupArray, x => x.GroupName.EndsWith("_fit")),
                        Array.Find(obj.GroupArray, x => x.GroupName.EndsWith("_thin")),
                        Array.Find(obj.GroupArray, x => x.GroupName.EndsWith("_special"))
                    };
                foreach (var face in groupArray[i].Faces)
                {
                    if (updateProgressCallback != null)
                    {
                        updateProgressCallback();
                    }
                    int j = 0,
                    vertexIndex = 0;
                    var temp = new int[3];
                    foreach (var facePoint in face.FacePoints)
                    {
                        if (!obj.TryGetVertexIndex(facePoint, vertices, out vertexIndex, cleanModel))
                        {
                            temp[j] = vertices.Count;
                            vertices.Add(facePoint);
                        }
                        else
                        {
                            temp[j] = vertexIndex;
                        }
                        j++;
                    }
                    faces.Add(temp);
                }
                geoms[i] = new GEOM();
                geoms[i].mVersion1 = refMesh.mVersion1;
                geoms[i].mCount = refMesh.mCount;
                geoms[i].mIndexCount = refMesh.mIndexCount;
                geoms[i].mExternalCount = refMesh.mExternalCount;
                geoms[i].mInternalCount = refMesh.mInternalCount;
                geoms[i].mDummyTGI = new TGI(refMesh.mDummyTGI);
                geoms[i].mAbsolutePosition = refMesh.mAbsolutePosition;
                geoms[i].mMagic = refMesh.mMagic;
                geoms[i].mVersion = refMesh.mVersion;
                if (isMorph[i])
                {
                    geoms[i].mShaderHash = 0;
                    geoms[i].mMTNFSize = 0;
                }
                else
                {
                    geoms[i].mShaderHash = refMesh.mShaderHash;
                    geoms[i].mMTNFSize = refMesh.mMTNFSize;
                    geoms[i].mMTNF = new MTNF(refMesh.mMTNF);
                }
                geoms[i].mMergeGroup = refMesh.mMergeGroup;
                geoms[i].mSortOrder = refMesh.mSortOrder;
                geoms[i].mVertexCount = vertices.Count;
                if (isMorph[i] && geoms[i].mVertexCount != geoms[currentBase].mVertexCount)
                {
                    throw new MeshException("The number of vertices in " + groupArray[i].GroupName + " does not match the base mesh!");
                }
                if (isMorph[i])
                {
                    geoms[i].mFaceCount = 3;
                    geoms[i].mVertexFormats = new[]
                        {
                            new VertexFormat(1, 1, 12),
                            new VertexFormat(2, 1, 12),
                            new VertexFormat(10, 4, 4)
                        };
                }
                else
                {
                    geoms[i].mFaceCount = refMesh.mFaceCount;
                    geoms[i].mVertexFormats = new VertexFormat[refMesh.mFaceCount];
                    for (var j = 0; j < refMesh.mFaceCount; j++)
                    {
                        geoms[i].mVertexFormats[j] = new VertexFormat(refMesh.mVertexFormats[j]);
                    }
                }
                geoms[i].mSubMeshCount = refMesh.mSubMeshCount;
                geoms[i].mBytesPerFacePoint = refMesh.mBytesPerFacePoint;
                geoms[i].mFacePointCount = faces.Count * 3;
                if (isMorph[i] && geoms[i].mFacePointCount != geoms[currentBase].mFacePointCount)
                {
                    throw new MeshException("The number of faces in " + groupArray[i].GroupName + " does not match the base mesh!");
                }
                geoms[i].mFaces = new Face[faces.Count];
                for (var j = 0; j < faces.Count; j++)
                {
                    geoms[i].mFaces[j] = new Face(faces[j]);
                }
                geoms[i].mSKCONIndex = refMesh.mSKCONIndex;
                geoms[i].mBoneHashCount = refMesh.mBoneHashCount;
                geoms[i].mBoneHashArray = new uint[refMesh.mBoneHashArray.Length];
                for (var j = 0; j < refMesh.mBoneHashArray.Length; j++)
                {
                    geoms[i].mBoneHashArray[j] = refMesh.mBoneHashArray[j];
                }
                if (isMorph[i])
                {
                    geoms[i].mTGICount = 1;
                    geoms[i].mTGIs = new TGI[1];
                    geoms[i].mTGIs[0] = new TGI(0, 0, 0);
                }
                else
                {
                    geoms[i].mTGICount = refMesh.mTGICount;
                    geoms[i].mTGIs = new TGI[refMesh.mTGIs.Length];
                    for (var j = 0; j < refMesh.mTGIs.Length; j++)
                    {
                        geoms[i].mTGIs[j] = new TGI(refMesh.mTGIs[j]);
                    }
                    if (bumpMapTGI.Instance > 0 && geoms[i].Shader.NormalIndex >= 0)
                    {
                        geoms[i].mTGIs[geoms[i].Shader.NormalIndex] = bumpMapTGI;
                    }
                }
                for (var j = 0; j < geoms[i].mVertexFormats.Length; j++)
                {
                    switch (geoms[i].mVertexFormats[j].FormatDataType) //OBJ vertex references are 1-based, so subtract one
                    {
                        case 1:
                            geoms[i].mPositions = new Position[vertices.Count];
                            for (var k = 0; k < vertices.Count; k++)
                            {
                                if (isMorph[i])
                                {
                                    Vector3 basePosition = new Vector3(geoms[currentBase].GetPosition(k)),
                                    morphPosition = new Vector3(obj.VertexArray[vertices[k][0] - 1].Coordinates),
                                    morph = morphPosition - basePosition;
                                    geoms[i].mPositions[k] = new Position(morph.Coordinates);
                                }
                                else
                                {
                                    geoms[i].mPositions[k] = new Position(obj.VertexArray[vertices[k][0] - 1].Coordinates);
                                }
                            }
                            break;
                        case 2:
                            geoms[i].mNormals = new Normal[vertices.Count];
                            for (var k = 0; k < vertices.Count; k++)
                            {
                                if (isMorph[i])
                                {
                                    Vector3 baseNormal = new Vector3(geoms[currentBase].GetNormal(k)),
                                    morphNormal = new Vector3(obj.NormalArray[vertices[k][2] - 1].Coordinates),
                                    morph = morphNormal - baseNormal;
                                    geoms[i].mNormals[k] = new Normal(morph.Coordinates);
                                }
                                else
                                {
                                    geoms[i].mNormals[k] = new Normal(obj.NormalArray[vertices[k][2] - 1].Coordinates);
                                }
                            }
                            break;
                        case 3:
                            geoms[i].mUVs = new UV[1][];
                            geoms[i].mUVs[0] = new UV[vertices.Count];
                            for (var k = 0; k < vertices.Count; k++)
                            {
                                geoms[i].mUVs[0][k] = new UV(obj.UVArray[vertices[k][1] - 1].Coordinates, true);
                            }
                            break;
                    }
                }
                for (var j = 0; j < geoms[i].mVertexFormats.Length; j++)
                {
                    switch (geoms[i].mVertexFormats[j].FormatDataType)
                    {
                        case 4:
                            geoms[i].mBones = new Bones[geoms[i].mVertexCount];
                            for (var k = 0; k < geoms[i].mVertexCount; k++)
                            {
                                geoms[i].mBones[k] = new Bones();
                            }
                            geoms[i].AutoBone(refMesh, false, true, 3, 2, false, null);
                            geoms[i].FixBoneWeights();
                            break;
                        case 6:
                            geoms[i].mTangents = new Tangent[geoms[i].mVertexCount];
                            for (var k = 0; k < geoms[i].mVertexCount; k++)
                            {
                                geoms[i].mTangents[k] = new Tangent();
                            }
                            geoms[i].CalculateTangents();
                            break;
                        case 7:
                            geoms[i].mTags = new TagValue[geoms[i].mVertexCount];
                            for (var k = 0; k < geoms[i].mVertexCount; k++)
                            {
                                geoms[i].mTags[k] = new TagValue(0xFFFFFFFF);
                            }
                            break;
                    }
                }
                for (var j = 0; j < geoms[i].mVertexFormats.Length; j++)
                {
                    switch (geoms[i].mVertexFormats[j].FormatDataType)
                    {
                        case 10:
                            geoms[i].mVertexIDs = new int[geoms[i].mVertexCount];
                            if (geoms[i].IsBase)
                            {
                                geoms[i].RenumberBase(refMesh.MinVertexID);
                            }
                            else if (geoms[i].IsMorph)
                            {
                                geoms[i].RenumberMorph(geoms[currentBase]);
                            }
                            break;
                    }
                }
            }
            return geoms;
        }

        public static GEOM[] GEOMsFromWSO(WSO wso, GEOM refMesh, TGI bumpMapTGI, Action updateProgressCallback = null)
        {
            return GEOMsFromOBJ(new OBJ(wso), refMesh, bumpMapTGI, false, false, updateProgressCallback);
        }

        public int GetBoneIndex(uint boneHash)
        {
            for (var i = 0; i < BoneCount; i++)
            {
                if (boneHash == mBoneHashArray[i])
                {
                    return i;
                }
            }
            return -1;
        }

        public byte[] GetBones(int vertexSequenceNumber)
        {
            return mBones[vertexSequenceNumber].BoneAssignments;
        }

        public float GetBoneWeightForVertex(int vertexIndex, uint boneHash)
        {
            var vertexBones = GetBones(vertexIndex);
            var vertexWeights = GetBoneWeightsV5(vertexIndex);
            for (var i = 0; i < 4; i++)
            {
                if (mBoneHashArray[vertexBones[i]] == boneHash)
                {
                    return vertexWeights[i];
                }
            }
            return 0;
        }

        public byte[] GetBoneWeights(int vertexSequenceNumber)
        {
            return mBones[vertexSequenceNumber].BoneWeights;
        }

        public float[] GetBoneWeightsV5(int vertexSequenceNumber)
        {
            return mBones[vertexSequenceNumber].BoneWeightsV5;
        }

        public int[] GetFaceIndices(int faceSequenceNumber)
        {
            return new[]
            {
                (int)mFaces[faceSequenceNumber].FacePoints[0],
                (int)mFaces[faceSequenceNumber].FacePoints[1],
                (int)mFaces[faceSequenceNumber].FacePoints[2]
            };
        }

        public uint[] GetFaceIndicesUInt(int faceSequenceNumber)
        {
            return new[]
            {
                mFaces[faceSequenceNumber].FacePoints[0],
                mFaces[faceSequenceNumber].FacePoints[1],
                mFaces[faceSequenceNumber].FacePoints[2]
            };
        }

        public Vector3[] GetFacePoints(int faceSequenceNumber)
        {
            return new[]
            {
                new Vector3(mPositions[mFaces[faceSequenceNumber].FacePoints[0]].Coordinates), 
                new Vector3(mPositions[mFaces[faceSequenceNumber].FacePoints[1]].Coordinates), 
                new Vector3(mPositions[mFaces[faceSequenceNumber].FacePoints[2]].Coordinates)
            };
        }

        public float[] GetHeightAndDepth()
        {
            float yMax = 0,
            zMax = 0;
            foreach (var position in mPositions)
            {
                if (position.Y > yMax)
                {
                    yMax = position.Y;
                }
                if (position.Z > zMax)
                {
                    zMax = position.Z;
                }
            }
            return new[]
            {
                yMax,
                zMax
            };
        }

        public float[] GetNormal(int vertexSequenceNumber)
        {
            return mNormals[vertexSequenceNumber].Coordinates;
        }

        public float[] GetPosition(int vertexSequenceNumber)
        {
            return mPositions[vertexSequenceNumber].Coordinates;
        }

        public static Vector3[] GetSeamVertexPositions(Species species, AgeGender age, AgeGender gender, int lod, GEOM.SeamType seam)
        {
            int ageGenderIndex,
            speciesIndex = (int)species - 1;
            if (species == Species.Human)
            {
                if (age >= AgeGender.Teen)
                {
                    if (gender == AgeGender.Male)
                    {
                        ageGenderIndex = 0;
                    }
                    else
                    {
                        ageGenderIndex = 1;
                    }
                }
                else if (age == AgeGender.Child)
                {
                    ageGenderIndex = 2;
                }
                else
                {
                    ageGenderIndex = 3;
                }
            }
            else if (species == Species.LittleDog && age <= AgeGender.Child)
            {
                speciesIndex = 1; //little dogs only have adult form so go to dog/child
                ageGenderIndex = 1;
            }
            else
            {
                ageGenderIndex = age > AgeGender.Child ? 0 : 1;
            }
            return MeshSeamVertices[speciesIndex][ageGenderIndex][lod][(int)seam];
        }

        public List<float[]> GetStitchUVs(int vertexIndex)
        {
            var uvList = new List<float[]>();
            foreach (var uvStitch in mUVStitches)
            {
                if (uvStitch.Index == vertexIndex)
                {
                    uvList.AddRange(uvStitch.UV1Coordinates);
                    break;
                }
            }
            return uvList;
        }

        public uint GetTagValue(int vertexSequenceNumber)
        {
            return mTags[vertexSequenceNumber].Tags;
        }

        public float[] GetTangent(int vertexSequenceNumber)
        {
            return mTangents[vertexSequenceNumber].Coordinates;
        }

        public float GetTotalBoneWeight(int vertexIndex, List<uint> boneHashes)
        {
            var vertexBones = GetBones(vertexIndex);
            var vertexWeights = GetBoneWeightsV5(vertexIndex);
            var totalWeight = 0f;
            for (var i = 0; i < 4; i++)
            {
                if (vertexBones[i] < mBoneHashArray.Length && vertexBones[i] >= 0 && boneHashes.Contains(mBoneHashArray[vertexBones[i]]))
                {
                    totalWeight += vertexWeights[i];
                }
            }
            return totalWeight;
        }

        public float[] GetUV(int vertexSequenceNumber, int uvSet)
        {
            return mUVs[uvSet][vertexSequenceNumber].Coordinates;
        }

        public int GetVertexID(int vertexSequenceNumber)
        {
            return mVertexIDs[vertexSequenceNumber];
        }
            
        public int[] GetVertexIndicesAssignedtoBone(uint boneHash)
        {
            var index = GetBoneIndex(boneHash);
            if (index < 0)
            {
                return new int[0];
            }
            var vertices = new List<int>();
            for (var i = 0; i < VertexCount; i++)
            {
                byte[] bones = GetBones(i),
                weights = GetBoneWeights(i);
                for (var j = 0; j < 4; j++)
                {
                    if (weights[j] > 0 && bones[j] == index)
                    {
                        vertices.Add(i);
                    }
                } 
            }
            return vertices.ToArray();
        }

        public bool HasBlueVertexColor
        {
            get
            {
                for (var i = 0; i < VertexCount; i++)
                {
                    if ((GetTagValue(i) & 0xFF0000) > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool HasUVSet(int uvSequence)
        {
            return (Array.IndexOf(VertexFormatList, 3) > -1 && uvSequence < mUVs.Length && mUVs[uvSequence] != null);
        }

        public void InsertFormatDescriptor(MeshFormatFlag flag)
        {
            if (Array.IndexOf(VertexFormatList, (int)flag) >= 0)
            {
                return;
            }
            MeshFormatDataType dataType = 0;
            byte byteLength = 0;
            switch (flag)
            {
                case MeshFormatFlag.Position:
                    dataType = MeshFormatDataType.Float;
                    byteLength = 12;
                    break;
                case MeshFormatFlag.Normals:
                    dataType = MeshFormatDataType.Float;
                    byteLength = 12;
                    break;
                case MeshFormatFlag.UV:
                    dataType = MeshFormatDataType.Float;
                    byteLength = 8;
                    break;
                case MeshFormatFlag.BoneIndices:
                    dataType = MeshFormatDataType.Byte4;
                    byteLength = 4;
                    break;
                case MeshFormatFlag.BoneWeights:
                    dataType = MeshFormatDataType.Byte4;
                    byteLength = 4;
                    break;
                case MeshFormatFlag.Tangents:
                    dataType = MeshFormatDataType.Float;
                    byteLength = 12;
                    break;
                case MeshFormatFlag.Color:
                    dataType = MeshFormatDataType.Color;
                    byteLength = 4;
                    break;
                case MeshFormatFlag.VertexID:
                    dataType = MeshFormatDataType.Uint;
                    byteLength = 4;
                    break;
                default:
                    dataType = MeshFormatDataType.Float;
                    byteLength = 12;
                    break;
            }
            var newFormat = new VertexFormat[VertexFormatList.Length + 1];
            if (flag == MeshFormatFlag.UV && Array.IndexOf(VertexFormatList, (int)flag) >= 0)
            {
                var uv = Array.IndexOf(VertexFormatList, (int)flag);
                Array.Copy(mVertexFormats, newFormat, uv + 1);
                newFormat[uv + 1] = new VertexFormat((int)flag, (int)dataType, byteLength);
                Array.Copy(mVertexFormats, uv + 1, newFormat, uv + 2, mVertexFormats.Length - uv - 1);
            }
            else
            {
                Array.Copy(mVertexFormats, newFormat, VertexFormatList.Length);
                newFormat[VertexFormatList.Length] = new VertexFormat((int)flag, (int)dataType, byteLength);
            }
            mVertexFormats = newFormat;
            mFaceCount += 1;
        }

        public void InsertVertexIDInFormatList()
        {
            if (Array.IndexOf(VertexFormatList, 10) >= 0)
            {
                return;
            }
            var newFormat = new VertexFormat[VertexFormatList.Length + 1];
            if (Array.IndexOf(VertexFormatList, 5) >= 0)
            {
                var index = 0;
                for (var i = 0; i < VertexFormatList.Length; i++)
                {
                    newFormat[index] = mVertexFormats[i];
                    index += 1;
                    if (mVertexFormats[i].FormatDataType == 5)
                    {
                        newFormat[index] = new VertexFormat(10, 4, 4);
                        index += 1;
                    }
                }
            }
            else
            {
                Array.Copy(mVertexFormats, newFormat, VertexFormatList.Length);
                newFormat[VertexFormatList.Length] = new VertexFormat(10, 4, 4);
            }
            mVertexFormats = newFormat;
            mFaceCount += 1;
        }

        public static bool IsEqual(float x, float y)
        {
            return Math.Abs(x - y) < 1e-4f;
        }

        public void MatchFormats(VertexFormat[] vertexFormatToMatch)
        {
            var uvIndex = 0;
            for (var i = 0; i < vertexFormatToMatch.Length; i++)
            {
                switch (vertexFormatToMatch[i].FormatDataType)
                {
                    case 1:
                        if (mPositions == null || mPositions.Length != mVertexCount)
                        {
                            mPositions = new Position[mVertexCount];
                            for (var j = 0; j < mVertexCount; j++)
                            {
                                mPositions[j] = new Position();
                            }
                        }
                        break;
                    case 2:
                        if (mNormals == null || mNormals.Length != mVertexCount)
                        {
                            mNormals = new Normal[mVertexCount];
                            for (var j = 0; j < mVertexCount; j++)
                            {
                                mNormals[j] = new Normal();
                            }
                        }
                        break;
                    case 3:
                        uvIndex += 1;
                        break;
                    case 4:
                        if (mBones == null || mBones.Length != mVertexCount)
                        {
                            mBones = new Bones[mVertexCount];
                            for (var j = 0; j < mVertexCount; j++)
                            {
                                mBones[j] = new Bones();
                            }
                        }
                        break;
                    case 5:
                        if (mBones == null || mBones.Length != mVertexCount)
                        {
                            mBones = new Bones[mVertexCount];
                            for (var j = 0; j < mVertexCount; j++)
                            {
                                mBones[j] = new Bones();
                            }
                        }
                        break;
                    case 6:
                        if (!HasTangents)
                        {
                            mTangents = new Tangent[mVertexCount];
                        }
                        break;
                    case 7:
                        if (mTags == null || mTags.Length != mVertexCount)
                        {
                            mTags = new TagValue[mVertexCount];
                            for (var j = 0; j < mVertexCount; j++)
                            {
                                mTags[j] = new TagValue();
                            }
                        }
                        break;
                    case 10:
                        if (mVertexIDs == null || mVertexIDs.Length != mVertexCount)
                        {
                            mVertexIDs = new int[mVertexCount];
                            for (var j = 0; j < mVertexCount; j++)
                            {
                                mVertexIDs[j] = -1;
                            }
                        }
                        break;
                }
            }
            if (uvIndex > 0 && UVCount != uvIndex)
            {
                var newUV = new UV[uvIndex][];
                for (var i = 0; i < uvIndex; i++)
                {
                    newUV[i] = new UV[mVertexCount];
                    if (HasUVSet(i))
                    {
                        for (var j = 0; j < mVertexCount; j++)
                        {
                            newUV[i][j] = mUVs[i][j];
                        }
                    }
                    else
                    {
                        for (var j = 0; j < mVertexCount; j++)
                        {
                            newUV[i][j] = new UV();
                        }
                    }
                }
                mUVs = newUV;
            }
            var newFormat = new VertexFormat[vertexFormatToMatch.Length];
            for (var i = 0; i < vertexFormatToMatch.Length; i++)
            {
                newFormat[i] = new VertexFormat(vertexFormatToMatch[i].FormatDataType, vertexFormatToMatch[i].FormatSubType, vertexFormatToMatch[i].FormatDataLength);
            }
            mVertexFormats = newFormat;
            mFaceCount = mVertexFormats.Length;
        }

        public void MatchPartSeamStitches()
        {
            if (mSeamStitches == null)
            {
                return;
            }
            var ids = new List<ushort>();
            var hitCount = new List<int>();
            List<Vector3> normals = new List<Vector3>(),
            positions = new List<Vector3>();
            foreach (var seamStitch in mSeamStitches)
            {
                var i = ids.IndexOf(seamStitch.VertexID);
                if (i < 0)
                {
                    ids.Add(seamStitch.VertexID);
                    positions.Add(new Vector3(mPositions[seamStitch.Index].Coordinates));
                    normals.Add(new Vector3(mNormals[seamStitch.Index].Coordinates));
                    hitCount.Add(1);
                }
                else
                {
                    positions[i] = (new Vector3(mPositions[seamStitch.Index].Coordinates) + (hitCount[i] * positions[i])) / (hitCount[i] + 1);
                    normals[i] = (new Vector3(mNormals[seamStitch.Index].Coordinates) + (hitCount[i] * normals[i])) / (hitCount[i] + 1);
                    hitCount[i]++;
                }
            }
            foreach (var seamStitch in mSeamStitches)
            {
                var i = ids.IndexOf(seamStitch.VertexID);
                if (i >= 0)
                {
                    mPositions[seamStitch.Index] = new Position(positions[i].Coordinates);
                    mNormals[seamStitch.Index] = new Normal(normals[i].Coordinates);
                }
            }
        }

        public static void MatchSeamStitches(GEOM geom1, GEOM geom2)
        {
            var ids = new List<ushort>();
            List<Vector3> normals = new List<Vector3>(),
            positions = new List<Vector3>();
            foreach (var seamStitch1 in geom1.mSeamStitches)
            {
                foreach (var seamStitch2 in geom2.mSeamStitches)
                {
                    if (seamStitch1.VertexID == seamStitch2.VertexID)
                    {
                        var i = ids.IndexOf(seamStitch1.VertexID);
                        if (i >= 0)
                        {
                            geom1.mPositions[seamStitch1.Index] = new Position(positions[i].Coordinates);
                            geom2.mPositions[seamStitch2.Index] = new Position(positions[i].Coordinates);
                            geom1.mNormals[seamStitch1.Index] = new Normal(normals[i].Coordinates);
                            geom2.mNormals[seamStitch2.Index] = new Normal(normals[i].Coordinates);
                        }
                        else
                        {
                            Vector3 normal = (new Vector3(geom1.mNormals[seamStitch1.Index].Coordinates) + new Vector3(geom2.mNormals[seamStitch2.Index].Coordinates)) / 2,
                            position = (new Vector3(geom1.mPositions[seamStitch1.Index].Coordinates) + new Vector3(geom2.mPositions[seamStitch2.Index].Coordinates)) / 2;
                            geom1.mPositions[seamStitch1.Index] = new Position(position.Coordinates);
                            geom2.mPositions[seamStitch2.Index] = new Position(position.Coordinates);
                            geom1.mNormals[seamStitch1.Index] = new Normal(normal.Coordinates);
                            geom2.mNormals[seamStitch2.Index] = new Normal(normal.Coordinates);
                            ids.Add(seamStitch1.VertexID);
                            positions.Add(position);
                            normals.Add(normal);
                        }
                    }
                }
            }
        }

        public static void MatchSeamVertices(GEOM geom)
        {
            for (var i = 0; i < geom.mVertexCount; i++)
            {
                for (var j = 0; j < geom.mVertexCount; j++)
                {
                    if (new Vector3(geom.GetPosition(i)).PositionMatches(geom.GetPosition(j)))
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        byte[] sourceBones = geom.GetBones(i),
                        sourceWeights = geom.GetBoneWeights(i);
                        geom.SetBones(j, sourceBones);
                        geom.SetBoneWeights(j, sourceWeights);
                        geom.SetNormal(j, geom.GetNormal(i));
                        UVStitch stitch = null;
                        for (var k = 0; k < geom.mUVStitches.Length; k++)
                        {
                            if (geom.mUVStitches[k].Index == i)
                            {
                                stitch = new UVStitch(geom.mUVStitches[k]);
                                break;
                            }
                        }
                        for (var k = 0; k < geom.mUVStitches.Length; k++)
                        {
                            if (geom.mUVStitches[k].Index == j)
                            {
                                if (stitch != null)
                                {
                                    geom.mUVStitches[k] = stitch;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static void MatchSeamVertices(GEOM geom1, GEOM geom2)
        {
            List<UVStitch> geom1Stitches = new List<UVStitch>(geom1.mUVStitches),
            geom2Stitches = new List<UVStitch>(geom2.mUVStitches);
            for (var i = 0; i < geom1.mVertexCount; i++)
            {
                for (var j = 0; j < geom2.mVertexCount; j++)
                {
                    if (new Vector3(geom1.GetPosition(i)).PositionMatches(geom2.GetPosition(j)))
                    {
                        byte[] sourceBones = geom1.GetBones(i),
                        sourceWeights = geom1.GetBoneWeights(i),
                        targetBones = new byte[sourceBones.Length];
                        for (var k = 0; k < sourceBones.Length; k++)
                        {
                            if (!TranslateBone(geom1.mBoneHashArray[sourceBones[k]], geom2.mBoneHashArray, out targetBones[k]) && sourceWeights[k] > 0)
                            {
                                var temp = new List<uint>(geom2.mBoneHashArray);
                                temp.Add(geom1.mBoneHashArray[sourceBones[k]]);
                                targetBones[k] = (byte)(temp.Count - 1);
                            }
                        }
                        geom2.SetBones(j, targetBones);
                        geom2.SetBoneWeights(j, sourceWeights);
                        geom2.SetNormal(j, geom1.GetNormal(i));
                        List<Vector2> geom1Stitch = new List<Vector2>(),
                        geom2Stitch = new List<Vector2>();
                        int geom1Index = -1,
                        geom2Index = -1;
                        for (var k = 0; k < geom1.mUVStitches.Length; k++)
                        {
                            if (geom1.mUVStitches[k].Index == i)
                            {
                                geom1Stitch.AddRange(geom1Stitches[k].UV1Vectors);
                                geom1Index = k;
                                break;
                            }
                        }
                        for (var k = 0; k < geom2.mUVStitches.Length; k++)
                        {
                            if (geom2.mUVStitches[k].Index == j)
                            {
                                geom2Stitch.AddRange(geom2Stitches[k].UV1Vectors);
                                geom2Index = k;
                                break;
                            }
                        }
                        Vector2 geom1UV1 = new Vector2(geom1.GetUV(i, 1)), 
                        geom2UV1 = new Vector2(geom2.GetUV(j, 1));
                        geom1UV1.X = Math.Abs(geom1UV1.X);
                        if (geom2Stitch.IndexOf(geom1UV1) < 0)
                        {
                            geom2Stitch.Add(geom1UV1);
                        }
                        geom2UV1.X = Math.Abs(geom2UV1.X);
                        if (geom1Stitch.IndexOf(geom2UV1) < 0)
                        {
                            geom1Stitch.Add(geom2UV1);
                        }
                        if (geom1Index >= 0)
                        {
                            geom1Stitches[geom1Index] = new UVStitch(i, geom1Stitch.ToArray());
                        }
                        else
                        {
                            geom1Stitches.Add(new UVStitch(i, geom1Stitch.ToArray()));
                        }
                        if (geom2Index >= 0)
                        {
                            geom2Stitches[geom2Index] = new UVStitch(j, geom2Stitch.ToArray());
                        }
                        else
                        {
                            geom2Stitches.Add(new UVStitch(j, geom2Stitch.ToArray()));
                        }
                    }
                }
            }
            geom1.mUVStitches = geom1Stitches.ToArray();
            geom2.mUVStitches = geom2Stitches.ToArray();
        }

        public void Read(BinaryReader reader)
        {
            reader.BaseStream.Position = 0;
            if (reader.BaseStream.Length < 12)
            {
                return;
            }
            reader.BaseStream.Position = 0;
            mVersion1 = reader.ReadInt32();
            mCount = reader.ReadInt32();
            mIndexCount = reader.ReadInt32();
            mExternalCount = reader.ReadInt32();
            mInternalCount = reader.ReadInt32();
            mDummyTGI = new TGI(reader);
            mAbsolutePosition = reader.ReadInt32();
            mMeshSize = reader.ReadInt32();
            mMagic = reader.ReadChars(4);
            if (new string(mMagic) != "GEOM")
            {
                throw new MeshException("Not a valid GEOM file.");
            }
            mVersion = reader.ReadInt32();
            mTGIOffset = reader.ReadInt32();
            mTGISize = reader.ReadInt32();
            mShaderHash = reader.ReadUInt32();
            if (mShaderHash != 0)
            {
                mMTNFSize = reader.ReadInt32();
                mMTNF = new MTNF(reader);
            }
            mMergeGroup = reader.ReadInt32();
            mSortOrder = reader.ReadInt32();
            mVertexCount = reader.ReadInt32();
            mFaceCount = reader.ReadInt32();
            mVertexFormats = new VertexFormat[mFaceCount];
            for (var i = 0; i < mFaceCount; i++)
            {
                mVertexFormats[i] = new VertexFormat(reader);
            }
            var vertexFormatList = VertexFormatList;
            var uvIndex = 0;
            for (var i = 0; i < vertexFormatList.Length; i++)
            {
                switch (vertexFormatList[i])
                {
                    case 1:
                        mPositions = new Position[mVertexCount];
                        break;
                    case 2:
                        mNormals = new Normal[mVertexCount];
                        break;
                    case 3:
                        uvIndex += 1;
                        break;
                    case 4:
                        mBones = new Bones[mVertexCount];
                        break;
                    case 6:
                        mTangents = new Tangent[mVertexCount];
                        break;
                    case 7:
                        mTags = new TagValue[mVertexCount];
                        break;
                    case 10:
                        mVertexIDs = new int[mVertexCount];
                        break;
                }
            }
            if (uvIndex > 0)
            {
                mUVs = new UV[uvIndex][];
            }
            for (var i = 0; i < uvIndex; i++)
            {
                mUVs[i] = new UV[mVertexCount];
            }
            for (var i = 0; i < mVertexCount; i++)
            {
                uvIndex = 0;
                for (var j = 0; j < mFaceCount; j++)
                {
                    switch (mVertexFormats[j].FormatDataType)
                    {
                        case 1:
                            mPositions[i] = new Position(reader);
                            break;
                        case 2:
                            mNormals[i] = new Normal(reader);
                            break;
                        case 3:
                            mUVs[uvIndex][i] = new UV(reader);
                            uvIndex += 1;
                            break;
                        case 4:
                            if (mBones[i] == null)
                            {
                                mBones[i] = new Bones();
                            }
                            mBones[i].ReadAssignments(reader);
                            break;
                        case 5:
                            if (mBones[i] == null)
                            {
                                mBones[i] = new Bones();
                            }
                            mBones[i].ReadWeights(reader, mVertexFormats[j].FormatSubType);
                            break;
                        case 6:
                            mTangents[i] = new Tangent(reader);
                            break;
                        case 7:
                            mTags[i] = new TagValue(reader);
                            break;
                        case 10:
                            mVertexIDs[i] = reader.ReadInt32();
                            break;
                    }
                }
            }
            mSubMeshCount = reader.ReadInt32();
            mBytesPerFacePoint = reader.ReadByte();
            mFacePointCount = reader.ReadInt32();
            mFaces = new Face[mFacePointCount / 3];
            for (var i = 0; i < mFacePointCount / 3; i++)
            {
                mFaces[i] = new Face(reader, mBytesPerFacePoint);
            }
            if (mVersion == 5)
            {
                mSKCONIndex = reader.ReadInt32();
            }
            else if (mVersion >= 12)
            {
                mUVStitchCount = reader.ReadInt32();
                mUVStitches = new UVStitch[mUVStitchCount];
                for (var i = 0; i < mUVStitchCount; i++)
                {
                    mUVStitches[i] = new UVStitch(reader);
                }
                if (mVersion >= 13)
                {
                    mSeamStitchCount = reader.ReadInt32();
                    mSeamStitches = new SeamStitch[mSeamStitchCount];
                    for (var i = 0; i < mSeamStitchCount; i++)
                    {
                        mSeamStitches[i] = new SeamStitch(reader, mUVs[0]);
                    }
                }
                mSlotCount = reader.ReadInt32();
                mSlotrayIntersections = new SlotrayIntersection[mSlotCount];
                for (var i = 0; i < mSlotCount; i++)
                {
                    mSlotrayIntersections[i] = new SlotrayIntersection(reader, mVersion);
                }
            }
            mBoneHashCount = reader.ReadInt32();
            mBoneHashArray = new uint[mBoneHashCount];
            for (var i = 0; i < mBoneHashCount; i++)
            {
                mBoneHashArray[i] = reader.ReadUInt32();
            }
            if (reader.BaseStream.Length <= reader.BaseStream.Position)
            {
                return;
            }
            mTGICount = reader.ReadInt32();
            mTGIs = new TGI[mTGICount];
            for (var i = 0; i < mTGICount; i++)
            {
                mTGIs[i] = new TGI(reader);
            }
            if (IsMorph && mSKCONIndex >= mTGICount)
            {
                mSKCONIndex = 0;
            }
            return;
        }

        public int RenumberBase(int startID)
        {
            if (!IsBase || !IsValid)
            {
                throw new MeshException("This mesh is not a valid base mesh!");
            }
            if (!HasVertexIDs)
            {
                InsertVertexIDInFormatList();
            }
            var newIDs = new int[mVertexCount];
            var nextID = Math.Max(0, startID);
            bool incrementID;
            for (var i = 0; i < mVertexCount; i++)
            {
                incrementID = true;
                for (var j = 0; j < i; j++)
                {
                    if (mPositions[i].Equals(mPositions[j]) && mNormals[i].Equals(mNormals[j]))
                    {
                        newIDs[i] = newIDs[j];
                        incrementID = false;
                        break;
                    }
                }
                if (incrementID)
                {
                    newIDs[i] = nextID;
                    nextID++;
                }
            }
            mVertexIDs = newIDs;
            return nextID;
        }

        public void RenumberMorph(GEOM baseMesh)
        {
            if (!IsMorph || !IsValid)
            {
                throw new MeshException("This mesh is not a valid morph mesh!");
            }
            if (mVertexCount != baseMesh.mVertexCount)
            {
                throw new MeshException("This morph does not have the same number of vertices as the base!");
            }
            var newIDs = new int[mVertexCount];
            Array.Copy(baseMesh.mVertexIDs, newIDs, mVertexCount);
            mVertexIDs = newIDs;
        }

        public void SetBoneHashList(uint[] boneHashList)
        {
            mBoneHashArray = boneHashList;
            mBoneHashCount = boneHashList.Length;
        }

        public void SetBones(int vertexSequenceNumber, byte bone0, byte bone1, byte bone2, byte bone3)
        {
            mBones[vertexSequenceNumber].BoneAssignments = new[]
                {
                    bone0,
                    bone1,
                    bone2,
                    bone3
                };
        }

        public void SetBones(int vertexSequenceNumber, byte[] newBones)
        {
            mBones[vertexSequenceNumber].BoneAssignments = newBones;
        }

        public void SetBoneWeights(int vertexSequenceNumber, byte[] newWeights)
        {
            mBones[vertexSequenceNumber].BoneWeights = newWeights;
        }

        public void SetBoneWeights(int vertexSequenceNumber, byte weight0, byte weight1, byte weight2, byte weight3)
        {
            mBones[vertexSequenceNumber].BoneWeights = new[]
                {
                    weight0,
                    weight1,
                    weight2,
                    weight3
                };
        }

        public void SetBoneWeightsV5(int vertexSequenceNumber, float[] newWeights)
        {
            mBones[vertexSequenceNumber].BoneWeightsV5 = newWeights;
        }

        public void SetBoneWeightsV5(int vertexSequenceNumber, float weight0, float weight1, float weight2, float weight3)
        {
            mBones[vertexSequenceNumber].BoneWeightsV5 = new[]
                {
                    weight0,
                    weight1,
                    weight2,
                    weight3
                };
        }

        public void SetNormal(int vertexSequenceNumber, float[] newNormal)
        {
            mNormals[vertexSequenceNumber] = new Normal(newNormal);
        }

        public void SetNormal(int vertexSequenceNumber, float x, float y, float z)
        {
            mNormals[vertexSequenceNumber] = new Normal(x, y, z);
        }

        public void SetPosition(int vertexSequenceNumber, float[] newPosition)
        {
            mPositions[vertexSequenceNumber] = new Position(newPosition);
        }

        public void SetPosition(int vertexSequenceNumber, float x, float y, float z)
        {
            mPositions[vertexSequenceNumber] = new Position(x, y, z);
        }

        public void SetShader(uint shaderHash)
        {
            mShaderHash = shaderHash;
        }

        public void SetShader(uint shaderHash, MTNF shader)
        {
            mShaderHash = shaderHash;
            mMTNF = shader;
        }

        public void SetTagValue(int vertexSequenceNumber, uint newTag)
        {
            mTags[vertexSequenceNumber] = new TagValue(newTag);
        }

        public void SetTangent(int vertexSequenceNumber, float[] newTangent)
        {
            mTangents[vertexSequenceNumber] = new Tangent(newTangent);
        }

        public void SetTangent(int vertexSequenceNumber, float x, float y, float z)
        {
            mTangents[vertexSequenceNumber] = new Tangent(x, y, z);
        }

        public void SetTGI(int index, TGI tgi)
        {
            mTGIs[index] = new TGI(tgi);
        }

        public void SetupDeltas()
        {
            DeltaPosition = new Vector3[mPositions.Length];
            for (var i = 0; i < mPositions.Length; i++)
            {
                DeltaPosition[i] = new Vector3();
            }
        }

        public static Vector3[][][][][] SetupSeamVertexPositions()
        {
            var meshSeamVertices = new Vector3[4][][][][];      //indices: species, age/gender, lod, seam, verts
            //dimension 0: 0 = human, 1 = dog, 2 = cat, 3 = little dog
            //dimension 1: human: 0 = male, 1 = female, 2 = child, 3 = toddler; little dog: 0 = adult; dog/cat: 0 = adult, 1 = child
            meshSeamVertices[0] = new Vector3[4][][][];         //ageGenders
            meshSeamVertices[0][0] = new Vector3[4][][];        //Adult Male
            meshSeamVertices[0][0][0] = new Vector3[7][];       //Adult Male LOD0 seams
            meshSeamVertices[0][0][0][0] = new[]
                {
                    new Vector3(.10318f, .16812f, .01464f),
                    new Vector3(.08145f, .16812f, .006759f),
                    new Vector3(.12142f, .16812f, .002309f),
                    new Vector3(.1301f, .16812f, -.02376f),
                    new Vector3(.12235f, .16812f, -.04518f),
                    new Vector3(.10225f, .16812f, -.06237f),
                    new Vector3(.06959f, .16812f, -.01289f),
                    new Vector3(.07157f, .16812f, -.03863f),
                    new Vector3(.08476f, .16812f, -.05846f),
                    new Vector3(-.10318f, .168119f, .01464f),
                    new Vector3(-.08145f, .168119f, .00676f),
                    new Vector3(-.12142f, .168119f, .00231f),
                    new Vector3(-.1301f, .168119f, -.02376f),
                    new Vector3(-.12235f, .168119f, -.04518f),
                    new Vector3(-.10225f, .168119f, -.06237f),
                    new Vector3(-.06959f, .168119f, -.01289f),
                    new Vector3(-.07157f, .168119f, -.03863f),
                    new Vector3(-.08476f, .168119f, -.05846f)
                };                                              //Ankles
            meshSeamVertices[0][0][0][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][0][0][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][0][0][3] = new[]
                {
                    new Vector3(.04994f, 1.65732f, -.04331f),
                    new Vector3(.05748f, 1.65212f, -.02185f),
                    new Vector3(.02016f, 1.62796f, .02991f),
                    new Vector3(0, 1.62329f, .03646f),
                    new Vector3(.02658f, 1.65984f, -.06291001f),
                    new Vector3(.04268f, 1.63725f, .01346f),
                    new Vector3(.03073f, 1.63173f, .02297f),
                    new Vector3(.05114f, 1.64436f, -.00103f),
                    new Vector3(0, 1.66001f, -.07078f),
                    new Vector3(-.04994f, 1.65732f, -.04331f),
                    new Vector3(-.05748f, 1.65212f, -.02185f),
                    new Vector3(-.02016f, 1.62796f, .02991f),
                    new Vector3(-.02658f, 1.65984f, -.06291001f),
                    new Vector3(-.04268f, 1.63725f, .01346f),
                    new Vector3(-.03074f, 1.63173f, .02296f),
                    new Vector3(-.05114f, 1.64436f, -.00103f)
                };                                              //Neck
            meshSeamVertices[0][0][0][4] = new Vector3[0];      //Waist
            meshSeamVertices[0][0][0][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][0][0][6] = new[]
                {
                    new Vector3(.13477f, 1.10102f, .05168f),
                    new Vector3(.09531f, 1.09588f, .08789f),
                    new Vector3(.0283f, 1.09001f, .11046f),
                    new Vector3(.06203f, 1.0929f, .10109f),
                    new Vector3(0, 1.08875f, .11338f),
                    new Vector3(.0537f, 1.10483f, -.07900001f),
                    new Vector3(.02362f, 1.10363f, -.08015f),
                    new Vector3(.12888f, 1.10734f, -.03361f),
                    new Vector3(.14252f, 1.10484f, .00736f),
                    new Vector3(.10903f, 1.10822f, -.05758001f),
                    new Vector3(.08691f, 1.10752f, -.0717f),
                    new Vector3(0, 1.10245f, -.07855f),
                    new Vector3(-.13477f, 1.10102f, .05168f),
                    new Vector3(-.09531f, 1.09588f, .08789f),
                    new Vector3(-.0283f, 1.09001f, .11046f),
                    new Vector3(-.06203f, 1.0929f, .10109f),
                    new Vector3(-.0537f, 1.10483f, -.07900001f),
                    new Vector3(-.02362f, 1.10363f, -.08015f),
                    new Vector3(-.12888f, 1.10734f, -.03361f),
                    new Vector3(-.14252f, 1.10484f, .00736f),
                    new Vector3(-.10903f, 1.10822f, -.05758001f),
                    new Vector3(-.08691f, 1.10752f, -.0717f)
                };                                              //WaistAdultMale
            meshSeamVertices[0][0][1] = new Vector3[7][];       //Adult Male LOD1 seams
            meshSeamVertices[0][0][1][0] = new[]
                {
                    new Vector3(.10318f, .16812f, .01464f),
                    new Vector3(.08145f, .16812f, .006759f),
                    new Vector3(.12142f, .16812f, .002309f),
                    new Vector3(.1301f, .16812f, -.02376f),
                    new Vector3(.12235f, .16812f, -.04518f),
                    new Vector3(.09351f, .16812f, -.06042f),
                    new Vector3(.06959f, .16812f, -.01289f),
                    new Vector3(.07157f, .16812f, -.03863f),
                    new Vector3(-.10318f, .168119f, .01464f),
                    new Vector3(-.08145f, .168119f, .00676f),
                    new Vector3(-.12142f, .168119f, .00231f),
                    new Vector3(-.1301f, .168119f, -.02376f),
                    new Vector3(-.12235f, .168119f, -.04518f),
                    new Vector3(-.09351f, .168119f, -.06042f),
                    new Vector3(-.06959f, .168119f, -.01289f),
                    new Vector3(-.07157f, .168119f, -.03863f)
                };                                              //Ankles
            meshSeamVertices[0][0][1][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][0][1][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][0][1][3] = new[]
                {
                    new Vector3(.04994f, 1.65732f, -.04331f),
                    new Vector3(.05748f, 1.65212f, -.02185f),
                    new Vector3(.02016f, 1.62796f, .02991f),
                    new Vector3(0, 1.62329f, .03646f),
                    new Vector3(.02658f, 1.65984f, -.06291001f),
                    new Vector3(.04268f, 1.63725f, .01346f),
                    new Vector3(.03074f, 1.63173f, .02297f),
                    new Vector3(.05114f, 1.64436f, -.00103f),
                    new Vector3(0, 1.66001f, -.07078f),
                    new Vector3(-.04994f, 1.65732f, -.04331f),
                    new Vector3(-.05748f, 1.65212f, -.02185f),
                    new Vector3(-.02016f, 1.62796f, .02991f),
                    new Vector3(-.02658f, 1.65984f, -.06291001f),
                    new Vector3(-.04268f, 1.63725f, .01346f),
                    new Vector3(-.03074f, 1.63173f, .02296f),
                    new Vector3(-.05114f, 1.64436f, -.00103f)
                };                                              //Neck
            meshSeamVertices[0][0][1][4] = new Vector3[0];      //Waist
            meshSeamVertices[0][0][1][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][0][1][6] = new[]
                {
                    new Vector3(.13477f, 1.10102f, .05168f),
                    new Vector3(.07867001f, 1.09439f, .09449001f),
                    new Vector3(0, 1.08875f, .11338f),
                    new Vector3(.03866f, 1.10423f, -.07958f),
                    new Vector3(.12888f, 1.10734f, -.03361f),
                    new Vector3(.14252f, 1.10484f, .00736f),
                    new Vector3(.09797f, 1.10787f, -.06464f),
                    new Vector3(0, 1.10245f, -.07855f),
                    new Vector3(-.13477f, 1.10102f, .05168f),
                    new Vector3(-.07867001f, 1.09439f, .09449001f),
                    new Vector3(-.03866f, 1.10423f, -.07958f),
                    new Vector3(-.12888f, 1.10734f, -.03361f),
                    new Vector3(-.14252f, 1.10484f, .00736f),
                    new Vector3(-.09797f, 1.10787f, -.06464f)
                };                                              //WaistAdultMale
            meshSeamVertices[0][0][2] = new Vector3[7][];       //Adult Male LOD2 seams
            meshSeamVertices[0][0][2][0] = new[]
                {
                    new Vector3(.1123f, .16812f, .008479001f),
                    new Vector3(.08145f, .16812f, .006759f),
                    new Vector3(.1301f, .16812f, -.02376f),
                    new Vector3(.12235f, .16812f, -.04518f),
                    new Vector3(.08254f, .16812f, -.04952f),
                    new Vector3(.06959f, .16812f, -.01289f),
                    new Vector3(-.1123f, .168119f, .00848f),
                    new Vector3(-.08145f, .168119f, .00676f),
                    new Vector3(-.1301f, .168119f, -.02376f),
                    new Vector3(-.12235f, .168119f, -.04518f),
                    new Vector3(-.08254f, .168119f, -.04952f),
                    new Vector3(-.06959f, .168119f, -.01289f)
                };                                              //Ankles
            meshSeamVertices[0][0][2][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][0][2][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][0][2][3] = new[]
                {
                    new Vector3(.04994f, 1.65732f, -.04331f),
                    new Vector3(.05748f, 1.65212f, -.02185f),
                    new Vector3(.02016f, 1.62796f, .02991f),
                    new Vector3(0, 1.62329f, .03646f),
                    new Vector3(.02658f, 1.65984f, -.06291001f),
                    new Vector3(.04268f, 1.63725f, .01346f),
                    new Vector3(.03074f, 1.63173f, .02297f),
                    new Vector3(.05114f, 1.64436f, -.00103f),
                    new Vector3(0, 1.66001f, -.07078f),
                    new Vector3(-.04994f, 1.65732f, -.04331f),
                    new Vector3(-.05748f, 1.65212f, -.02185f),
                    new Vector3(-.02016f, 1.62796f, .02991f),
                    new Vector3(-.02658f, 1.65984f, -.06291001f),
                    new Vector3(-.04268f, 1.63725f, .01346f),
                    new Vector3(-.03074f, 1.63173f, .02296f),
                    new Vector3(-.05114f, 1.64436f, -.00103f)
                };                                              //Neck
            meshSeamVertices[0][0][2][4] = new Vector3[0];      //Waist
            meshSeamVertices[0][0][2][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][0][2][6] = new[]
                {
                    new Vector3(.13477f, 1.10102f, .05168f),
                    new Vector3(.07867001f, 1.09439f, .09449001f),
                    new Vector3(0, 1.08875f, .11338f),
                    new Vector3(.06831001f, 1.10605f, -.07211f),
                    new Vector3(.12888f, 1.10734f, -.03361f),
                    new Vector3(.14252f, 1.10484f, .00736f),
                    new Vector3(0, 1.10245f, -.07855f),
                    new Vector3(-.13477f, 1.10102f, .05168f),
                    new Vector3(-.07867001f, 1.09439f, .09449001f),
                    new Vector3(-.06831001f, 1.10605f, -.07211f),
                    new Vector3(-.12888f, 1.10734f, -.03361f),
                    new Vector3(-.14252f, 1.10484f, .00736f)
                };                                              //WaistAdultMale
            meshSeamVertices[0][0][3] = new Vector3[7][];       //Adult Male LOD3 seams
            meshSeamVertices[0][0][3][0] = new[]
                {
                    new Vector3(.10318f, .16812f, .01464f),
                    new Vector3(.1301f, .16812f, -.02376f),
                    new Vector3(.09351f, .16812f, -.06042f),
                    new Vector3(.06959f, .16812f, -.01289f),
                    new Vector3(-.10318f, .168119f, .01464f),
                    new Vector3(-.1301f, .168119f, -.02376f),
                    new Vector3(-.09351f, .168119f, -.06042f),
                    new Vector3(-.06959f, .168119f, -.01289f)
                };                                              //Ankles
            meshSeamVertices[0][0][3][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][0][3][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][0][3][3] = new[]
                {
                    new Vector3(.05748f, 1.65212f, -.02185f),
                    new Vector3(0, 1.62329f, .03646f),
                    new Vector3(.03826f, 1.65858f, -.05311f),
                    new Vector3(.03074f, 1.63173f, .02297f),
                    new Vector3(0, 1.66001f, -.07078f),
                    new Vector3(-.05748f, 1.65212f, -.02185f),
                    new Vector3(-.03826f, 1.65858f, -.05311f),
                    new Vector3(-.03074f, 1.63173f, .02296f)
                };                                              //Neck
            meshSeamVertices[0][0][3][4] = new Vector3[0];      //Waist
            meshSeamVertices[0][0][3][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][0][3][6] = new[]
                {
                    new Vector3(.10672f, 1.09771f, .07308f),
                    new Vector3(0, 1.08875f, .11338f),
                    new Vector3(.0986f, 1.1067f, -.05286f),
                    new Vector3(.14252f, 1.10484f, .00736f),
                    new Vector3(0, 1.10245f, -.07855f),
                    new Vector3(-.10672f, 1.09771f, .07308f),
                    new Vector3(-.0986f, 1.1067f, -.05286f),
                    new Vector3(-.14252f, 1.10484f, .00736f)
                };                                              //WaistAdultMale
            meshSeamVertices[0][1] = new Vector3[4][][];        //Adult Female
            meshSeamVertices[0][1][0] = new Vector3[7][];       //Adult Female LOD0 seams
            meshSeamVertices[0][1][0][0] = new[]
                {
                    new Vector3(.10061f, .17831f, .01385f),
                    new Vector3(.08411001f, .17831f, .01062f),
                    new Vector3(.11955f, .17831f, -.00055f),
                    new Vector3(.12174f, .17831f, -.02315f),
                    new Vector3(.11393f, .17831f, -.04578f),
                    new Vector3(.1008f, .17831f, -.05814f),
                    new Vector3(.07353f, .17831f, -.01651f),
                    new Vector3(.07809f, .17831f, -.04168f),
                    new Vector3(.08404f, .17831f, -.05354f),
                    new Vector3(-.10061f, .17831f, .01385f),
                    new Vector3(-.08411001f, .17831f, .01062f),
                    new Vector3(-.11955f, .17831f, -.00055f),
                    new Vector3(-.12174f, .17831f, -.02315f),
                    new Vector3(-.11393f, .17831f, -.04578f),
                    new Vector3(-.10078f, .17831f, -.05815f),
                    new Vector3(-.07353f, .17831f, -.01651f),
                    new Vector3(-.07809f, .17831f, -.04168f),
                    new Vector3(-.08404f, .17831f, -.05354f)
                };                                              //Ankles
            meshSeamVertices[0][1][0][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][1][0][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][1][0][3] = new[]
                {
                    new Vector3(.04228f, 1.65728f, -.03741f),
                    new Vector3(.04676f, 1.65358f, -.01843f),
                    new Vector3(.01541f, 1.62769f, .02782f),
                    new Vector3(0, 1.62476f, .03136f),
                    new Vector3(.02362f, 1.65786f, -.05106f),
                    new Vector3(.03554f, 1.6385f, .013f),
                    new Vector3(.02586f, 1.6321f, .02275f),
                    new Vector3(.04565f, 1.64751f, -.00218f),
                    new Vector3(0, 1.65824f, -.05823f),
                    new Vector3(-.04228f, 1.65728f, -.03741f),
                    new Vector3(-.04676f, 1.65358f, -.01843f),
                    new Vector3(-.01541f, 1.62769f, .02782f),
                    new Vector3(-.02361f, 1.65786f, -.05106f),
                    new Vector3(-.03554f, 1.6385f, .013f),
                    new Vector3(-.02586f, 1.6321f, .02275f),
                    new Vector3(-.04565f, 1.64751f, -.00218f)
                };                                              //Neck
            meshSeamVertices[0][1][0][4] = new Vector3[0];      //Waist
            meshSeamVertices[0][1][0][5] = new[]
                {
                    new Vector3(0, 1.16153f, .10832f),
                    new Vector3(0, 1.17486f, -.05726f),
                    new Vector3(.11117f, 1.17097f, .05975f),
                    new Vector3(.08326f, 1.16772f, .08886f),
                    new Vector3(.02036f, 1.1624f, .10652f),
                    new Vector3(.05003f, 1.16448f, .10046f),
                    new Vector3(.11914f, 1.17389f, .01728f),
                    new Vector3(.11197f, 1.17412f, -.01527f),
                    new Vector3(.09584f, 1.17388f, -.04124f),
                    new Vector3(.07065f, 1.1729f, -.05077f),
                    new Vector3(.0456f, 1.17315f, -.05503f),
                    new Vector3(.01515f, 1.17431f, -.05672f),
                    new Vector3(-.11117f, 1.17097f, .05975f),
                    new Vector3(-.08326f, 1.16772f, .08886f),
                    new Vector3(-.02036f, 1.1624f, .10652f),
                    new Vector3(-.05003f, 1.16448f, .10046f),
                    new Vector3(-.11914f, 1.17389f, .01728f),
                    new Vector3(-.11197f, 1.17412f, -.01527f),
                    new Vector3(-.09584f, 1.17388f, -.04124f),
                    new Vector3(-.07065f, 1.1729f, -.05077f),
                    new Vector3(-.0456f, 1.17315f, -.05503f),
                    new Vector3(-.01515f, 1.17431f, -.05672f)
                };                                              //WaistAdultFemale
            meshSeamVertices[0][1][0][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[0][1][1] = new Vector3[7][];       //Adult Female LOD1 seams
            meshSeamVertices[0][1][1][0] = new[]
                {
                    new Vector3(.10061f, .17831f, .01385f),
                    new Vector3(.08411001f, .17831f, .01062f),
                    new Vector3(.11955f, .17831f, -.00055f),
                    new Vector3(.12174f, .17831f, -.02315f),
                    new Vector3(.11393f, .17831f, -.04578f),
                    new Vector3(.09242f, .17831f, -.05584f),
                    new Vector3(.07353f, .17831f, -.01651f),
                    new Vector3(.07809f, .17831f, -.04168f),
                    new Vector3(-.10061f, .17831f, .01385f),
                    new Vector3(-.08411001f, .17831f, .01062f),
                    new Vector3(-.11955f, .17831f, -.00055f),
                    new Vector3(-.12174f, .17831f, -.02315f),
                    new Vector3(-.11393f, .17831f, -.04578f),
                    new Vector3(-.09241f, .17831f, -.05585f),
                    new Vector3(-.07353f, .17831f, -.01651f),
                    new Vector3(-.07809f, .17831f, -.04168f)
                };                                              //Ankles
            meshSeamVertices[0][1][1][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][1][1][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][1][1][3] = new[]
                {
                    new Vector3(.04228f, 1.65729f, -.03741f),
                    new Vector3(.04676f, 1.65359f, -.01843f),
                    new Vector3(.01541f, 1.6277f, .02782f),
                    new Vector3(0, 1.62477f, .03136f),
                    new Vector3(.02362f, 1.65786f, -.05105f),
                    new Vector3(.03554f, 1.63851f, .013f),
                    new Vector3(.02586f, 1.63211f, .02275f),
                    new Vector3(.04565f, 1.64751f, -.00218f),
                    new Vector3(0, 1.65824f, -.05822f),
                    new Vector3(-.04228f, 1.65729f, -.03741f),
                    new Vector3(-.04676f, 1.65359f, -.01843f),
                    new Vector3(-.01541f, 1.6277f, .02782f),
                    new Vector3(-.02361f, 1.65786f, -.05105f),
                    new Vector3(-.03554f, 1.63851f, .013f),
                    new Vector3(-.02586f, 1.63211f, .02275f),
                    new Vector3(-.04565f, 1.64751f, -.00218f)
                };                                              //Neck
            meshSeamVertices[0][1][1][4] = new Vector3[0];      //Waist
            meshSeamVertices[0][1][1][5] = new[]
                {
                    new Vector3(0, 1.16153f, .10832f),
                    new Vector3(0, 1.17486f, -.05726f),
                    new Vector3(.11117f, 1.17097f, .05975f),
                    new Vector3(.06664f, 1.1661f, .09466001f),
                    new Vector3(.11914f, 1.17389f, .01728f),
                    new Vector3(.11197f, 1.17412f, -.01527f),
                    new Vector3(.08324f, 1.17339f, -.04601f),
                    new Vector3(.0456f, 1.17315f, -.05503f),
                    new Vector3(-.11117f, 1.17097f, .05975f),
                    new Vector3(-.06664f, 1.1661f, .09466001f),
                    new Vector3(-.11914f, 1.17389f, .01728f),
                    new Vector3(-.11197f, 1.17412f, -.01527f),
                    new Vector3(-.08324f, 1.17339f, -.04601f),
                    new Vector3(-.0456f, 1.17315f, -.05503f)
                };                                              //WaistAdultFemale
            meshSeamVertices[0][1][1][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[0][1][2] = new Vector3[7][];       //Adult Female LOD2 seams
            meshSeamVertices[0][1][2][0] = new[]
                {
                    new Vector3(.11008f, .17831f, .00665f),
                    new Vector3(.08411001f, .17831f, .01062f),
                    new Vector3(.12174f, .17831f, -.02315f),
                    new Vector3(.11393f, .17831f, -.04578f),
                    new Vector3(.08525001f, .17831f, -.04876f),
                    new Vector3(.07353f, .17831f, -.01651f),
                    new Vector3(-.11008f, .17831f, .00665f),
                    new Vector3(-.08411001f, .17831f, .01062f),
                    new Vector3(-.12174f, .17831f, -.02315f),
                    new Vector3(-.11393f, .17831f, -.04578f),
                    new Vector3(-.08525001f, .17831f, -.04876f),
                    new Vector3(-.07353f, .17831f, -.01651f)
                };                                              //Ankles
            meshSeamVertices[0][1][2][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][1][2][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][1][2][3] = new[]
                {
                    new Vector3(.04228f, 1.65729f, -.03742f),
                    new Vector3(.04676f, 1.65358f, -.01844f),
                    new Vector3(.01541f, 1.6277f, .02782f),
                    new Vector3(0, 1.62477f, .03136f),
                    new Vector3(.02362f, 1.65786f, -.05105f),
                    new Vector3(.03554f, 1.6385f, .013f),
                    new Vector3(.02586f, 1.63212f, .02275f),
                    new Vector3(.04565f, 1.64751f, -.00218f),
                    new Vector3(0, 1.65824f, -.05822f),
                    new Vector3(-.04228f, 1.65729f, -.03742f),
                    new Vector3(-.04676f, 1.65358f, -.01844f),
                    new Vector3(-.01541f, 1.6277f, .02782f),
                    new Vector3(-.02361f, 1.65786f, -.05105f),
                    new Vector3(-.03554f, 1.63851f, .013f),
                    new Vector3(-.02586f, 1.63212f, .02275f),
                    new Vector3(-.04565f, 1.64751f, -.00218f)
                };                                              //Neck
            meshSeamVertices[0][1][2][4] = new Vector3[0];      //Waist
            meshSeamVertices[0][1][2][5] = new[]
                {
                    new Vector3(0, 1.16153f, .10832f),
                    new Vector3(0, 1.17486f, -.05726f),
                    new Vector3(.08891001f, 1.16853f, .07720001f),
                    new Vector3(.11914f, 1.17389f, .01728f),
                    new Vector3(.11197f, 1.17412f, -.01527f),
                    new Vector3(.06442f, 1.17327f, -.05052f),
                    new Vector3(-.08891001f, 1.16853f, .07720001f),
                    new Vector3(-.11914f, 1.17389f, .01728f),
                    new Vector3(-.11197f, 1.17412f, -.01527f),
                    new Vector3(-.06442f, 1.17327f, -.05052f)
                };                                              //WaistAdultFemale
            meshSeamVertices[0][1][2][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[0][1][3] = new Vector3[7][];       //Adult Female LOD3 seams
            meshSeamVertices[0][1][3][0] = new[]
                {
                    new Vector3(.10061f, .17831f, .01385f),
                    new Vector3(.12174f, .17831f, -.02315f),
                    new Vector3(.09242f, .17831f, -.05584f),
                    new Vector3(.07353f, .17831f, -.01651f),
                    new Vector3(-.10061f, .17831f, .01385f),
                    new Vector3(-.12174f, .17831f, -.02315f),
                    new Vector3(-.09241f, .17831f, -.05585f),
                    new Vector3(-.07353f, .17831f, -.01651f)
                };                                              //Ankles
            meshSeamVertices[0][1][3][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][1][3][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][1][3][3] = new[]
                {
                    new Vector3(.04676f, 1.65358f, -.01844f),
                    new Vector3(0, 1.62477f, .03136f),
                    new Vector3(.03295f, 1.65757f, -.04423f),
                    new Vector3(.02583f, 1.63211f, .02276f),
                    new Vector3(0, 1.65824f, -.05822f),
                    new Vector3(-.04676f, 1.65358f, -.01844f),
                    new Vector3(-.03295f, 1.65757f, -.04423f),
                    new Vector3(-.02586f, 1.63212f, .02275f)
                };                                              //Neck
            meshSeamVertices[0][1][3][4] = new Vector3[0];      //Waist
            meshSeamVertices[0][1][3][5] = new[]
                {
                    new Vector3(0, 1.16153f, .10832f),
                    new Vector3(0, 1.17486f, -.05726f),
                    new Vector3(.08891001f, 1.16853f, .07720001f),
                    new Vector3(.11914f, 1.17389f, .01728f),
                    new Vector3(.0882f, 1.17369f, -.03289f),
                    new Vector3(-.08891001f, 1.16853f, .07720001f),
                    new Vector3(-.11914f, 1.17389f, .01728f),
                    new Vector3(-.0882f, 1.17369f, -.03289f)
                };                                              //WaistAdultFemale
            meshSeamVertices[0][1][3][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[0][2] = new Vector3[4][][];        //Child
            meshSeamVertices[0][2][0] = new Vector3[7][];       //Child LOD0 seams
            meshSeamVertices[0][2][0][0] = new[]
                {
                    new Vector3(.07243f, .11592f, .01694f),
                    new Vector3(.05307f, .11592f, .0112f),
                    new Vector3(.09189f, .11592f, .00385f),
                    new Vector3(.0952f, .11592f, -.01715f),
                    new Vector3(.08587f, .11592f, -.03915f),
                    new Vector3(.07431f, .11592f, -.04292f),
                    new Vector3(.04333f, .11592f, -.00869f),
                    new Vector3(.04767f, .11593f, -.03418f),
                    new Vector3(.06141f, .11592f, -.04133f),
                    new Vector3(-.05307f, .11592f, .0112f),
                    new Vector3(-.07243f, .11592f, .01694f),
                    new Vector3(-.09189f, .11592f, .00385f),
                    new Vector3(-.0952f, .11592f, -.01715f),
                    new Vector3(-.08587f, .11592f, -.03915f),
                    new Vector3(-.07431f, .11592f, -.04292f),
                    new Vector3(-.04333f, .11592f, -.00869f),
                    new Vector3(-.04767f, .11593f, -.03418f),
                    new Vector3(-.06141f, .11592f, -.04133f)
                };                                              //Ankles
            meshSeamVertices[0][2][0][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][2][0][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][2][0][3] = new[]
                {
                    new Vector3(-.03752f, 1.12267f, -.03273f),
                    new Vector3(-.02065f, 1.12468f, -.04379001f),
                    new Vector3(0, 1.12563f, -.04968f),
                    new Vector3(.02065f, 1.12477f, -.04379001f),
                    new Vector3(.03752f, 1.12268f, -.03273f),
                    new Vector3(-.04215f, 1.11826f, -.01802f),
                    new Vector3(-.03877f, 1.11206f, -.00206f),
                    new Vector3(-.03276f, 1.10712f, .00977f),
                    new Vector3(-.01436f, 1.09967f, .02605f),
                    new Vector3(0, 1.09771f, .029229f),
                    new Vector3(-.02296f, 1.10289f, .01801f),
                    new Vector3(.04215f, 1.11824f, -.01801f),
                    new Vector3(.03877f, 1.11205f, -.00205f),
                    new Vector3(.03276f, 1.10712f, .00977f),
                    new Vector3(.01436f, 1.09965f, .02605f),
                    new Vector3(.02295f, 1.10288f, .01802f)
                };                                              //Neck
            meshSeamVertices[0][2][0][4] = new[]
                {
                    new Vector3(.0914f, .7533001f, .03828f),
                    new Vector3(.06577f, .7498001f, .06943001f),
                    new Vector3(.01949f, .74579f, .08594f),
                    new Vector3(.04228f, .74777f, .08012f),
                    new Vector3(0, .74493f, .08794f),
                    new Vector3(.03662f, .7558801f, -.06594001f),
                    new Vector3(.01616f, .7548701f, -.06698f),
                    new Vector3(.09703f, .75579f, .00505f),
                    new Vector3(.08787f, .75756f, -.02397f),
                    new Vector3(.07434f, .75819f, -.04569f),
                    new Vector3(.05926f, .75773f, -.05923f),
                    new Vector3(0, .75421f, -.06595f),
                    new Vector3(-.0914f, .7533001f, .03828f),
                    new Vector3(-.06577f, .7498001f, .06944f),
                    new Vector3(-.04228f, .74777f, .08014001f),
                    new Vector3(-.01949f, .74579f, .08594f),
                    new Vector3(-.01616f, .7548701f, -.06698f),
                    new Vector3(-.03662f, .7558801f, -.06594001f),
                    new Vector3(-.09703f, .75579f, .00505f),
                    new Vector3(-.08787f, .75756f, -.02397f),
                    new Vector3(-.07434f, .7582f, -.04569f),
                    new Vector3(-.05926f, .75773f, -.05923f)
                };                                              //Waist
            meshSeamVertices[0][2][0][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][2][0][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[0][2][1] = new Vector3[7][];       //Child LOD1 seams
            meshSeamVertices[0][2][1][0] = new[]
                {
                    new Vector3(.053066f, .115924f, .011202f),
                    new Vector3(.043329f, .115923f, -.008687f),
                    new Vector3(.072431f, .115924f, .016942f),
                    new Vector3(.09188901f, .115924f, .003849f),
                    new Vector3(.095204f, .115924f, -.017147f),
                    new Vector3(.08587401f, .115924f, -.039149f),
                    new Vector3(.067858f, .115924f, -.042117f),
                    new Vector3(.047673f, .115932f, -.034183f),
                    new Vector3(-.053066f, .115924f, .011202f),
                    new Vector3(-.043329f, .115923f, -.008687f),
                    new Vector3(-.072431f, .115924f, .016942f),
                    new Vector3(-.09188901f, .115924f, .003849f),
                    new Vector3(-.095204f, .115924f, -.017147f),
                    new Vector3(-.08587401f, .115924f, -.039149f),
                    new Vector3(-.067858f, .115924f, -.042117f),
                    new Vector3(-.047673f, .115932f, -.034183f)
                };                                              //Ankles
            meshSeamVertices[0][2][1][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][2][1][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][2][1][3] = new[]
                {
                    new Vector3(-.03752f, 1.12268f, -.03273f),
                    new Vector3(-.02065f, 1.12477f, -.04379001f),
                    new Vector3(0, 1.12563f, -.04968f),
                    new Vector3(.02065f, 1.12477f, -.04379001f),
                    new Vector3(.03752f, 1.12268f, -.03273f),
                    new Vector3(-.03276f, 1.10712f, .00977f),
                    new Vector3(-.03877f, 1.11205f, -.002049f),
                    new Vector3(-.04215f, 1.11824f, -.01801f),
                    new Vector3(.04215f, 1.11824f, -.01801f),
                    new Vector3(.03877f, 1.11205f, -.002049f),
                    new Vector3(.03276f, 1.10712f, .00977f),
                    new Vector3(0, 1.09771f, .02923f),
                    new Vector3(-.01436f, 1.09965f, .02605f),
                    new Vector3(-.02295f, 1.10288f, .01802f),
                    new Vector3(.01436f, 1.09965f, .02605f),
                    new Vector3(.02295f, 1.10288f, .01802f)
                };                                              //Neck
            meshSeamVertices[0][2][1][4] = new[]
                {
                    new Vector3(.0914f, .7533001f, .03828f),
                    new Vector3(.05402f, .74878f, .07478f),
                    new Vector3(0, .74493f, .08794f),
                    new Vector3(.09703f, .75579f, .00505f),
                    new Vector3(.08787f, .75756f, -.02397f),
                    new Vector3(.0668f, .75796f, -.05246f),
                    new Vector3(.02639f, .75537f, -.06646f),
                    new Vector3(0, .75421f, -.06595f),
                    new Vector3(-.0914f, .7533001f, .03828f),
                    new Vector3(-.05402f, .74878f, .07478f),
                    new Vector3(-.09703f, .75579f, .00505f),
                    new Vector3(-.08787f, .75756f, -.02397f),
                    new Vector3(-.0668f, .75796f, -.05246f),
                    new Vector3(-.02639f, .75537f, -.06646f)
                };                                              //Waist
            meshSeamVertices[0][2][1][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][2][1][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[0][2][2] = new Vector3[7][];       //Child LOD2 seams
            meshSeamVertices[0][2][2][0] = new[]
                {
                    new Vector3(.053066f, .115924f, .016333f),
                    new Vector3(),
                    new Vector3(.043329f, .115923f, -.007374f),
                    new Vector3(.08216001f, .115924f, .015517f),
                    new Vector3(.095204f, .115924f, -.017455f),
                    new Vector3(.08587401f, .115924f, -.041221f),
                    new Vector3(.057831f, .115938f, -.038038f),
                    new Vector3(-.053066f, .115924f, .016333f),
                    new Vector3(-.043329f, .115923f, -.007374f),
                    new Vector3(-.08216001f, .115924f, .015517f),
                    new Vector3(-.095204f, .115924f, -.017455f),
                    new Vector3(-.08587401f, .115924f, -.041221f),
                    new Vector3(-.057831f, .115938f, -.038038f)
                };                                              //Ankles
            meshSeamVertices[0][2][2][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][2][2][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][2][2][3] = new[]
                {
                    new Vector3(-.03752f, 1.12267f, -.03273f),
                    new Vector3(0, 1.12563f, -.04968f),
                    new Vector3(-.02065f, 1.12468f, -.04379001f),
                    new Vector3(.02065f, 1.12468f, -.04379001f),
                    new Vector3(.03752f, 1.12267f, -.03273f),
                    new Vector3(-.04215f, 1.11826f, -.01802f),
                    new Vector3(-.03877f, 1.11206f, -.002059f),
                    new Vector3(-.03276f, 1.10712f, .00977f),
                    new Vector3(.03276f, 1.10712f, .00977f),
                    new Vector3(.03877f, 1.11206f, -.002059f),
                    new Vector3(.04215f, 1.11826f, -.01802f),
                    new Vector3(0, 1.09771f, .02923f),
                    new Vector3(-.01436f, 1.09967f, .02605f),
                    new Vector3(-.02296f, 1.10289f, .01801f),
                    new Vector3(.01436f, 1.09967f, .02605f),
                    new Vector3(.02296f, 1.10289f, .01801f)
                };                                              //Neck
            meshSeamVertices[0][2][2][4] = new[]
                {
                    new Vector3(.0914f, .7533001f, .03828f),
                    new Vector3(.05402f, .74878f, .07478f),
                    new Vector3(0, .74493f, .08794f),
                    new Vector3(.09703f, .75579f, .00505f),
                    new Vector3(.08787f, .75756f, -.02397f),
                    new Vector3(.04613f, .75671f, -.05946f),
                    new Vector3(0, .75421f, -.06595f),
                    new Vector3(-.0914f, .7533001f, .03828f),
                    new Vector3(-.05402f, .74878f, .07479f),
                    new Vector3(-.09703f, .75579f, .00505f),
                    new Vector3(-.08787f, .75756f, -.02397f),
                    new Vector3(-.04613f, .75672f, -.05946f)
                };                                              //Waist
            meshSeamVertices[0][2][2][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][2][2][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[0][2][3] = new Vector3[7][];       //Child LOD3 seams
            meshSeamVertices[0][2][3][0] = new[]
                {
                    new Vector3(.07243f, .11592f, .01694f),
                    new Vector3(.04333f, .11592f, -.00869f),
                    new Vector3(.0952f, .11592f, -.01715f),
                    new Vector3(.06786f, .11592f, -.04212f),
                    new Vector3(-.07243f, .11592f, .01694f),
                    new Vector3(-.04333f, .11592f, -.00869f),
                    new Vector3(-.0952f, .11592f, -.01715f),
                    new Vector3(-.06786f, .11592f, -.04212f)
                };                                              //Ankles
            meshSeamVertices[0][2][3][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][2][3][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][2][3][3] = new[]
                {
                    new Vector3(.02908f, 1.12368f, -.03826001f),
                    new Vector3(0, 1.12563f, -.04968f),
                    new Vector3(-.02908f, 1.12368f, -.03826001f),
                    new Vector3(-.04215f, 1.11826f, -.01802f),
                    new Vector3(.02296f, 1.10289f, .01801f),
                    new Vector3(.04215f, 1.11826f, -.01802f),
                    new Vector3(0, 1.09771f, .02923f),
                    new Vector3(-.02296f, 1.10289f, .01801f)
                };                                              //Neck
            meshSeamVertices[0][2][3][4] = new[]
                {
                    new Vector3(0, .74493f, .08794f),
                    new Vector3(.0721f, .7513f, .05675f),
                    new Vector3(.09703f, .75579f, .00505f),
                    new Vector3(0, .75421f, -.06595f),
                    new Vector3(-.0721f, .7513f, .05675f),
                    new Vector3(-.09703f, .75579f, .00505f)
                };                                              //Waist
            meshSeamVertices[0][2][3][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][2][3][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[0][3] = new Vector3[4][][];        //Toddler
            meshSeamVertices[0][3][0] = new Vector3[7][];       //Toddler LOD0 seams
            meshSeamVertices[0][3][0][0] = new[]
                {
                    new Vector3(.0581f, .08125f, .02318f),
                    new Vector3(.04031f, .08124f, .01804f),
                    new Vector3(.07382f, .08125f, .01169f),
                    new Vector3(.07935f, .08125f, -.00576f),
                    new Vector3(.07126f, .08125f, -.02278f),
                    new Vector3(.05931f, .08124f, -.02894f),
                    new Vector3(.03273f, .08124f, .00192f),
                    new Vector3(.03487f, .08125f, -.0212f),
                    new Vector3(.04727f, .08125f, -.02768f),
                    new Vector3(-.0581f, .08125f, .02318f),
                    new Vector3(-.04031f, .08124f, .01804f),
                    new Vector3(-.07382f, .08125f, .01169f),
                    new Vector3(-.07935f, .08125f, -.00576f),
                    new Vector3(-.07126f, .08125f, -.02278f),
                    new Vector3(-.05931f, .08124f, -.02894f),
                    new Vector3(-.03273f, .08124f, .00192f),
                    new Vector3(-.03487f, .08125f, -.0212f),
                    new Vector3(-.04727f, .08125f, -.02768f)
                };                                              //Ankles
            meshSeamVertices[0][3][0][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][3][0][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][3][0][3] = new[]
                {
                    new Vector3(0, .72543f, .02841f),
                    new Vector3(0, .74598f, -.04577f),
                    new Vector3(-.03502f, .74528f, -.0298f),
                    new Vector3(-.03926f, .74341f, -.0156f),
                    new Vector3(-.0366f, .73923f, -.00225f),
                    new Vector3(-.0332f, .73407f, .01018f),
                    new Vector3(-.01506f, .72707f, .02594f),
                    new Vector3(-.02401f, .73009f, .01878f),
                    new Vector3(-.01907f, .7453601f, -.0411f),
                    new Vector3(.03502f, .74528f, -.0298f),
                    new Vector3(.03926f, .74341f, -.0156f),
                    new Vector3(.0366f, .73923f, -.00225f),
                    new Vector3(.0332f, .73407f, .01018f),
                    new Vector3(.01506f, .72707f, .02594f),
                    new Vector3(.02401f, .73009f, .01878f),
                    new Vector3(.01907f, .7453601f, -.0411f)
                };                                              //Neck
            meshSeamVertices[0][3][0][4] = new[]
                {
                    new Vector3(.08580001f, .46483f, .03673f),
                    new Vector3(.064f, .46369f, .06124f),
                    new Vector3(.01813f, .46041f, .08473f),
                    new Vector3(.03817f, .46175f, .07651f),
                    new Vector3(0, .45983f, .08692f),
                    new Vector3(.03322f, .46701f, -.06998001f),
                    new Vector3(.01455f, .46646f, -.07039f),
                    new Vector3(.08139001f, .4675f, -.02652f),
                    new Vector3(.09089f, .46698f, .00424f),
                    new Vector3(.06864001f, .46769f, -.04676f),
                    new Vector3(.05398f, .46762f, -.05926f),
                    new Vector3(0, .4661f, -.0696f),
                    new Vector3(-.08580001f, .46483f, .0367f),
                    new Vector3(-.064f, .46369f, .0613f),
                    new Vector3(-.01813f, .46041f, .08473f),
                    new Vector3(-.03818f, .46175f, .07676001f),
                    new Vector3(-.03322f, .46701f, -.06998001f),
                    new Vector3(-.01455f, .46646f, -.07039f),
                    new Vector3(-.08139001f, .4675f, -.02652f),
                    new Vector3(-.09089f, .46698f, .00424f),
                    new Vector3(-.06864001f, .46769f, -.04676f),
                    new Vector3(-.05398f, .46762f, -.05926f)
                };                                              //Waist
            meshSeamVertices[0][3][0][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][3][0][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[0][3][1] = new Vector3[7][];       //Toddler LOD1 seams
            meshSeamVertices[0][3][1][0] = new[]
                {
                    new Vector3(.058104f, .08125f, .023184f),
                    new Vector3(.040309f, .08123501f, .018042f),
                    new Vector3(.07382f, .08125f, .01169f),
                    new Vector3(.079346f, .08125f, -.005758f),
                    new Vector3(.065284f, .081243f, -.025861f),
                    new Vector3(.032729f, .081235f, .001915f),
                    new Vector3(.034873f, .081249f, -.021203f),
                    new Vector3(.047273f, .08125f, -.027681f),
                    new Vector3(-.058104f, .08125f, .023184f),
                    new Vector3(-.040309f, .08123501f, .018042f),
                    new Vector3(-.07382f, .08125f, .01169f),
                    new Vector3(-.079346f, .08125f, -.005758f),
                    new Vector3(-.065284f, .081243f, -.025861f),
                    new Vector3(-.032729f, .081235f, .001915f),
                    new Vector3(-.034873f, .081249f, -.021203f),
                    new Vector3(-.047273f, .08125f, -.027681f)
                };                                              //Ankles
            meshSeamVertices[0][3][1][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][3][1][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][3][1][3] = new[]
                {
                    new Vector3(0, .725429f, .028412f),
                    new Vector3(0, .745978f, -.045768f),
                    new Vector3(-.035021f, .7452821f, -.029798f),
                    new Vector3(-.039258f, .743412f, -.015598f),
                    new Vector3(-.036595f, .739231f, -.002247f),
                    new Vector3(-.033196f, .7340671f, .010182f),
                    new Vector3(-.024012f, .730088f, .018779f),
                    new Vector3(-.019068f, .745362f, -.041102f),
                    new Vector3(-.015064f, .727072f, .025942f),
                    new Vector3(.035021f, .7452821f, -.029798f),
                    new Vector3(.039258f, .743412f, -.015598f),
                    new Vector3(.036595f, .739231f, -.002247f),
                    new Vector3(.033196f, .7340671f, .010182f),
                    new Vector3(.024012f, .730088f, .018779f),
                    new Vector3(.019068f, .745362f, -.041102f),
                    new Vector3(.015064f, .727072f, .025942f)
                };                                              //Neck
            meshSeamVertices[0][3][1][4] = new[]
                {
                    new Vector3(.074902f, .464262f, .048984f),
                    new Vector3(.038165f, .46175f, .076512f),
                    new Vector3(0, .459831f, .086915f),
                    new Vector3(.023888f, .466736f, -.070186f),
                    new Vector3(.090889f, .466976f, .004244f),
                    new Vector3(.075017f, .467595f, -.036638f),
                    new Vector3(.053979f, .467616f, -.05926f),
                    new Vector3(0, .466101f, -.069595f),
                    new Vector3(-.074902f, .464262f, .048999f),
                    new Vector3(-.038183f, .46175f, .076757f),
                    new Vector3(-.023888f, .466736f, -.070186f),
                    new Vector3(-.090889f, .466976f, .004244f),
                    new Vector3(-.075017f, .467595f, -.036638f),
                    new Vector3(-.053979f, .467616f, -.05926f)
                };                                              //Waist
            meshSeamVertices[0][3][1][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][3][1][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[0][3][2] = new Vector3[7][];       //Toddler LOD2 seams
            meshSeamVertices[0][3][2][0] = new[]
                {
                    new Vector3(.065962f, .08125f, .017437f),
                    new Vector3(.040309f, .08123501f, .018042f),
                    new Vector3(.079346f, .08125f, -.005758f),
                    new Vector3(.065284f, .081243f, -.025861f),
                    new Vector3(.032729f, .081235f, .001915f),
                    new Vector3(.041073f, .08125f, -.024442f),
                    new Vector3(-.065962f, .08125f, .017437f),
                    new Vector3(-.040309f, .08123501f, .018042f),
                    new Vector3(-.079346f, .08125f, -.005758f),
                    new Vector3(-.065284f, .081243f, -.025861f),
                    new Vector3(-.032729f, .081235f, .001915f),
                    new Vector3(-.041073f, .08125f, -.024442f)
                };                                              //Ankles
            meshSeamVertices[0][3][2][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][3][2][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][3][2][3] = new[]
                {
                    new Vector3(0, .725429f, .028412f),
                    new Vector3(0, .745978f, -.045768f),
                    new Vector3(-.039258f, .743412f, -.015598f),
                    new Vector3(-.024012f, .730088f, .018779f),
                    new Vector3(-.019068f, .745362f, -.041102f),
                    new Vector3(-.036595f, .739231f, -.002247f),
                    new Vector3(-.033196f, .7340671f, .010182f),
                    new Vector3(-.035021f, .7452821f, -.029798f),
                    new Vector3(-.015064f, .727072f, .025942f),
                    new Vector3(.039258f, .743412f, -.015598f),
                    new Vector3(.024012f, .730088f, .018779f),
                    new Vector3(.019068f, .745362f, -.041102f),
                    new Vector3(.036595f, .739231f, -.002247f),
                    new Vector3(.033196f, .7340671f, .010182f),
                    new Vector3(.035021f, .7452821f, -.029798f),
                    new Vector3(.015064f, .727072f, .025942f)
                };                                              //Neck
            meshSeamVertices[0][3][2][4] = new[]
                {
                    new Vector3(.074902f, .464262f, .048984f),
                    new Vector3(.038165f, .46175f, .076512f),
                    new Vector3(0, .459831f, .086915f),
                    new Vector3(.038933f, .467176f, -.064723f),
                    new Vector3(.090889f, .466976f, .004244f),
                    new Vector3(.075017f, .467595f, -.036638f),
                    new Vector3(0, .466101f, -.069595f),
                    new Vector3(-.074902f, .464262f, .048999f),
                    new Vector3(-.038183f, .46175f, .076757f),
                    new Vector3(-.038933f, .467176f, -.064723f),
                    new Vector3(-.090889f, .466976f, .004244f),
                    new Vector3(-.075017f, .467595f, -.036638f)
                };                                              //Waist
            meshSeamVertices[0][3][2][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][3][2][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[0][3][3] = new Vector3[7][];       //Toddler LOD3 seams
            meshSeamVertices[0][3][3][0] = new[]
                {
                    new Vector3(.053136f, .081243f, .017739f),
                    new Vector3(.079346f, .08125f, -.005758f),
                    new Vector3(.053179f, .081246f, -.025151f),
                    new Vector3(.032729f, .081235f, .001915f),
                    new Vector3(-.053136f, .081243f, .017739f),
                    new Vector3(-.079346f, .08125f, -.005758f),
                    new Vector3(-.053179f, .081246f, -.025151f),
                    new Vector3(-.032729f, .081235f, .001915f)
                };                                              //Ankles
            meshSeamVertices[0][3][3][1] = new Vector3[0];      //Tail
            meshSeamVertices[0][3][3][2] = new Vector3[0];      //Ears
            meshSeamVertices[0][3][3][3] = new[]
                {
                    new Vector3(0, .725429f, .028412f),
                    new Vector3(0, .745978f, -.045768f)
                };                                              //Neck
            meshSeamVertices[0][3][3][4] = new[]
                {
                    new Vector3(.056534f, .463006f, .062748f),
                    new Vector3(0, .459831f, .086915f),
                    new Vector3(.056975f, .467386f, -.050681f),
                    new Vector3(.090889f, .466976f, .004244f),
                    new Vector3(0, .466101f, -.069595f),
                    new Vector3(-.056542f, .463006f, .062878f),
                    new Vector3(-.056975f, .467386f, -.050681f),
                    new Vector3(-.090889f, .466976f, .004244f)
                };                                              //Waist
            meshSeamVertices[0][3][3][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[0][3][3][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[1] = new Vector3[4][][][];         //ageSpecies
            meshSeamVertices[1][0] = new Vector3[4][][];        //Adult Dog
            meshSeamVertices[1][0][0] = new Vector3[7][];       //Adult Dog LOD0 seams
            meshSeamVertices[1][0][0][0] = new[]
                {
                    new Vector3(.091319f, .110002f, .100705f),
                    new Vector3(.089504f, .111499f, .110355f),
                    new Vector3(.083803f, .1126f, .11691f),
                    new Vector3(.072392f, .112377f, .120393f),
                    new Vector3(.059744f, .111575f, .116427f),
                    new Vector3(.052912f, .110878f, .108974f),
                    new Vector3(.050505f, .109698f, .09901001f),
                    new Vector3(.050042f, .107311f, .088836f),
                    new Vector3(.054359f, .104351f, .079998f),
                    new Vector3(.06345101f, .101799f, .071559f),
                    new Vector3(.077975f, .101195f, .071651f),
                    new Vector3(.087592f, .104716f, .081086f),
                    new Vector3(.091225f, .107681f, .090575f),
                    new Vector3(-.050505f, .109698f, .09901001f),
                    new Vector3(-.052912f, .110878f, .108974f),
                    new Vector3(-.059744f, .111575f, .116427f),
                    new Vector3(-.072392f, .112377f, .120393f),
                    new Vector3(-.083803f, .1126f, .11691f),
                    new Vector3(-.089504f, .111499f, .110355f),
                    new Vector3(-.091319f, .110002f, .100705f),
                    new Vector3(-.091225f, .107681f, .090575f),
                    new Vector3(-.087592f, .104716f, .081086f),
                    new Vector3(-.077975f, .101195f, .071651f),
                    new Vector3(-.06345101f, .101799f, .071559f),
                    new Vector3(-.054359f, .104351f, .079998f),
                    new Vector3(-.050042f, .107311f, .088836f),
                    new Vector3(.070713f, .101497f, .069976f),
                    new Vector3(-.070713f, .101497f, .069976f),
                    new Vector3(.072045f, .10763f, -.502381f),
                    new Vector3(.07268f, .10763f, -.512439f),
                    new Vector3(.069921f, .107654f, -.5222141f),
                    new Vector3(.065669f, .107896f, -.529511f),
                    new Vector3(.053849f, .107928f, -.531096f),
                    new Vector3(.041362f, .107749f, -.528099f),
                    new Vector3(.037166f, .107536f, -.5213591f),
                    new Vector3(.035074f, .10762f, -.512302f),
                    new Vector3(.035216f, .1077f, -.5021471f),
                    new Vector3(.039253f, .107855f, -.491577f),
                    new Vector3(.046065f, .10827f, -.486399f),
                    new Vector3(.054622f, .108626f, -.484898f),
                    new Vector3(.062579f, .108652f, -.485451f),
                    new Vector3(.068362f, .108161f, -.492384f),
                    new Vector3(-.041362f, .107749f, -.528099f),
                    new Vector3(-.037166f, .107536f, -.5213591f),
                    new Vector3(-.035074f, .10762f, -.512302f),
                    new Vector3(-.035216f, .1077f, -.5021471f),
                    new Vector3(-.039253f, .107855f, -.491577f),
                    new Vector3(-.046065f, .10827f, -.486399f),
                    new Vector3(-.054622f, .108626f, -.484898f),
                    new Vector3(-.062579f, .108652f, -.485451f),
                    new Vector3(-.068362f, .108161f, -.492384f),
                    new Vector3(-.072045f, .10763f, -.502381f),
                    new Vector3(-.07268f, .10763f, -.512439f),
                    new Vector3(-.069921f, .107654f, -.5222141f),
                    new Vector3(-.065669f, .107896f, -.529511f),
                    new Vector3(-.053849f, .107928f, -.531096f)
                };                                              //Ankles
            meshSeamVertices[1][0][0][1] = new[]
                {
                    new Vector3(.021073f, .577363f, -.43746f),
                    new Vector3(.02086f, .592635f, -.42631f),
                    new Vector3(.012646f, .600489f, -.423659f),
                    new Vector3(0, .604671f, -.423148f),
                    new Vector3(0, .562809f, -.451407f),
                    new Vector3(.014413f, .569726f, -.445067f),
                    new Vector3(-.021073f, .577363f, -.437461f),
                    new Vector3(-.02086f, .592635f, -.426311f),
                    new Vector3(-.012646f, .600489f, -.423659f),
                    new Vector3(-.014413f, .569726f, -.445067f)
                };                                              //Tail
            meshSeamVertices[1][0][0][2] = new[]
                {
                    new Vector3(-.067989f, .768746f, .27049f),
                    new Vector3(-.063373f, .779868f, .276023f),
                    new Vector3(-.068046f, .761078f, .258345f),
                    new Vector3(-.066053f, .759634f, .242935f),
                    new Vector3(-.06145401f, .769566f, .230691f),
                    new Vector3(-.057924f, .780925f, .229641f),
                    new Vector3(-.05198f, .798391f, .236866f),
                    new Vector3(-.057639f, .793634f, .27381f),
                    new Vector3(-.053146f, .803787f, .266705f),
                    new Vector3(-.06815f, .764306f, .265297f),
                    new Vector3(-.060771f, .786525f, .276518f),
                    new Vector3(-.055271f, .7991011f, .270695f),
                    new Vector3(-.050504f, .808143f, .255652f),
                    new Vector3(-.050653f, .803242f, .243345f),
                    new Vector3(-.053795f, .7924451f, .232829f),
                    new Vector3(-.066883f, .760161f, .251542f),
                    new Vector3(.067989f, .768746f, .27049f),
                    new Vector3(.063373f, .779868f, .276023f),
                    new Vector3(.068046f, .761078f, .258345f),
                    new Vector3(.066053f, .759634f, .242935f),
                    new Vector3(.06145401f, .769566f, .23069f),
                    new Vector3(.057924f, .780925f, .229641f),
                    new Vector3(.05198f, .798391f, .236866f),
                    new Vector3(.057638f, .793634f, .273811f),
                    new Vector3(.053146f, .803787f, .266704f),
                    new Vector3(.06815f, .764306f, .265296f),
                    new Vector3(.060771f, .786525f, .276519f),
                    new Vector3(.055271f, .7991011f, .270695f),
                    new Vector3(.050504f, .808143f, .255652f),
                    new Vector3(.050653f, .803242f, .243345f),
                    new Vector3(.053795f, .7924451f, .232828f),
                    new Vector3(.066883f, .760161f, .251542f)
                };                                              //Ears
            meshSeamVertices[1][0][0][3] = new[]
                {
                    new Vector3(0, .65108f, .263976f),
                    new Vector3(-.031086f, .663975f, .251473f),
                    new Vector3(-.040149f, .6724421f, .244208f),
                    new Vector3(-.052244f, .6996421f, .220295f),
                    new Vector3(-.048166f, .719901f, .202111f),
                    new Vector3(-.036981f, .735922f, .186963f),
                    new Vector3(-.029402f, .743723f, .179634f),
                    new Vector3(-.01039f, .752611f, .171274f),
                    new Vector3(0, .754215f, .169697f),
                    new Vector3(-.012513f, .6536471f, .261375f),
                    new Vector3(-.050186f, .690092f, .229349f),
                    new Vector3(-.019823f, .74883f, .174659f),
                    new Vector3(-.043192f, .7280371f, .194325f),
                    new Vector3(.031086f, .663975f, .251473f),
                    new Vector3(.040149f, .6724421f, .244208f),
                    new Vector3(.052244f, .6996421f, .220295f),
                    new Vector3(.048166f, .719901f, .202111f),
                    new Vector3(.036981f, .735922f, .186963f),
                    new Vector3(.029402f, .743723f, .179634f),
                    new Vector3(.01039f, .752611f, .171274f),
                    new Vector3(.012513f, .6536471f, .261375f),
                    new Vector3(.050186f, .690092f, .229349f),
                    new Vector3(.019823f, .74883f, .174659f),
                    new Vector3(.043192f, .7280371f, .194325f),
                    new Vector3(.051507f, .710206f, .210656f),
                    new Vector3(.046314f, .680477f, .237306f),
                    new Vector3(.022854f, .658232f, .256744f),
                    new Vector3(-.051507f, .710206f, .210656f),
                    new Vector3(-.046314f, .680477f, .237306f),
                    new Vector3(-.022854f, .658232f, .256744f)
                };                                              //Neck
            meshSeamVertices[1][0][0][4] = new Vector3[0];      //Waist
            meshSeamVertices[1][0][0][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[1][0][0][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[1][0][1] = new Vector3[7][];       //Adult Dog LOD1 seams
            meshSeamVertices[1][0][1][0] = new[]
                {
                    new Vector3(.089504f, .111499f, .110355f),
                    new Vector3(.083803f, .1126f, .11691f),
                    new Vector3(.072392f, .112377f, .120393f),
                    new Vector3(.059744f, .111575f, .116427f),
                    new Vector3(.052912f, .110878f, .108974f),
                    new Vector3(.050273f, .108504f, .093923f),
                    new Vector3(.054359f, .104351f, .079998f),
                    new Vector3(.06345101f, .101799f, .071559f),
                    new Vector3(.077975f, .101195f, .071651f),
                    new Vector3(.087592f, .104716f, .081086f),
                    new Vector3(.091272f, .108841f, .09564f),
                    new Vector3(-.052912f, .110878f, .108974f),
                    new Vector3(-.059744f, .111575f, .116427f),
                    new Vector3(-.072392f, .112377f, .120393f),
                    new Vector3(-.083803f, .1126f, .11691f),
                    new Vector3(-.089504f, .111499f, .110355f),
                    new Vector3(-.091272f, .108841f, .09564f),
                    new Vector3(-.087592f, .104716f, .081086f),
                    new Vector3(-.077975f, .101195f, .071651f),
                    new Vector3(-.06345101f, .101799f, .071559f),
                    new Vector3(-.054359f, .104351f, .079998f),
                    new Vector3(-.050273f, .108504f, .093923f),
                    new Vector3(.070713f, .101497f, .069976f),
                    new Vector3(-.070713f, .101497f, .069976f),
                    new Vector3(.067952f, .119676f, -.490161f),
                    new Vector3(.071998f, .120309f, -.505297f),
                    new Vector3(.06767501f, .121354f, -.524048f),
                    new Vector3(.053809f, .121686f, -.530085f),
                    new Vector3(.039275f, .121442f, -.522727f),
                    new Vector3(.035499f, .12054f, -.504781f),
                    new Vector3(.039851f, .1193f, -.489503f),
                    new Vector3(.046574f, .119315f, -.484606f),
                    new Vector3(.054669f, .119597f, -.483415f),
                    new Vector3(.062031f, .119659f, -.483632f),
                    new Vector3(-.039275f, .121442f, -.522727f),
                    new Vector3(-.035499f, .12054f, -.504781f),
                    new Vector3(-.039851f, .1193f, -.489503f),
                    new Vector3(-.046574f, .119315f, -.484606f),
                    new Vector3(-.054669f, .119597f, -.483415f),
                    new Vector3(-.062031f, .119659f, -.483632f),
                    new Vector3(-.06767501f, .121354f, -.524048f),
                    new Vector3(-.053809f, .121686f, -.530085f),
                    new Vector3(-.067952f, .119676f, -.490161f),
                    new Vector3(-.071998f, .120309f, -.505297f)
                };                                              //Ankles
            meshSeamVertices[1][0][1][1] = new[]
                {
                    new Vector3(.021073f, .577363f, -.43746f),
                    new Vector3(.016753f, .596562f, -.424984f),
                    new Vector3(0, .604671f, -.423148f),
                    new Vector3(0, .562809f, -.451407f),
                    new Vector3(.014413f, .569726f, -.445067f),
                    new Vector3(-.021073f, .577363f, -.437461f),
                    new Vector3(-.016753f, .596562f, -.424985f),
                    new Vector3(-.014413f, .569726f, -.445067f)
                };                                              //Tail
            meshSeamVertices[1][0][1][2] = new[]
                {
                    new Vector3(-.067992f, .768754f, .270502f),
                    new Vector3(-.066058f, .759642f, .242946f),
                    new Vector3(-.061457f, .769576f, .230701f),
                    new Vector3(-.057926f, .780935f, .229652f),
                    new Vector3(-.051985f, .798399f, .236878f),
                    new Vector3(-.057639f, .793635f, .27381f),
                    new Vector3(-.053148f, .8037941f, .266716f),
                    new Vector3(-.068101f, .7626981f, .261832f),
                    new Vector3(-.060771f, .786526f, .276518f),
                    new Vector3(-.05527201f, .799109f, .270708f),
                    new Vector3(-.050583f, .8057f, .249509f),
                    new Vector3(-.0538f, .792453f, .232841f),
                    new Vector3(-.066888f, .760171f, .251553f),
                    new Vector3(.067992f, .768754f, .270502f),
                    new Vector3(.068101f, .7626981f, .261832f),
                    new Vector3(.066058f, .759642f, .242946f),
                    new Vector3(.061457f, .769576f, .230701f),
                    new Vector3(.057926f, .780935f, .229652f),
                    new Vector3(.051985f, .798399f, .236878f),
                    new Vector3(.057638f, .793635f, .27381f),
                    new Vector3(.053148f, .8037941f, .266716f),
                    new Vector3(.060771f, .786526f, .276518f),
                    new Vector3(.05527201f, .799109f, .270708f),
                    new Vector3(.050583f, .8057f, .249509f),
                    new Vector3(.0538f, .792453f, .232841f),
                    new Vector3(.066888f, .760171f, .251553f)
                };                                              //Ears
            meshSeamVertices[1][0][1][3] = new[]
                {
                    new Vector3(0, .65108f, .263976f),
                    new Vector3(-.031086f, .663975f, .251473f),
                    new Vector3(-.047474f, .719901f, .202111f),
                    new Vector3(-.039776f, .7319791f, .190644f),
                    new Vector3(-.029402f, .743723f, .179634f),
                    new Vector3(-.015106f, .75072f, .172967f),
                    new Vector3(0, .754215f, .169697f),
                    new Vector3(-.050186f, .690092f, .229349f),
                    new Vector3(.031086f, .663975f, .251473f),
                    new Vector3(.043231f, .67646f, .240757f),
                    new Vector3(.051443f, .704924f, .215476f),
                    new Vector3(.047474f, .719901f, .202111f),
                    new Vector3(.029402f, .743723f, .179634f),
                    new Vector3(.017683f, .65594f, .25906f),
                    new Vector3(.050186f, .690092f, .229349f),
                    new Vector3(.015106f, .75072f, .172967f),
                    new Vector3(.039776f, .7319791f, .190644f),
                    new Vector3(-.051443f, .704924f, .215476f),
                    new Vector3(-.043231f, .67646f, .240757f),
                    new Vector3(-.017683f, .65594f, .25906f)
                };                                              //Neck
            meshSeamVertices[1][0][1][4] = new Vector3[0];      //Waist
            meshSeamVertices[1][0][1][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[1][0][1][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[1][0][2] = new Vector3[7][];       //Adult Dog LOD2 seams
            meshSeamVertices[1][0][2][0] = new[]
                {
                    new Vector3(.09131901f, .110002f, .100705f),
                    new Vector3(.083803f, .1126f, .11691f),
                    new Vector3(.059744f, .111575f, .116427f),
                    new Vector3(.050505f, .109698f, .09901f),
                    new Vector3(.054359f, .104351f, .079998f),
                    new Vector3(.087592f, .104716f, .081086f),
                    new Vector3(-.050505f, .109698f, .09901f),
                    new Vector3(-.059744f, .111575f, .116427f),
                    new Vector3(-.083803f, .1126f, .11691f),
                    new Vector3(-.09131901f, .110002f, .100705f),
                    new Vector3(-.087592f, .104716f, .081086f),
                    new Vector3(-.054359f, .104351f, .079998f),
                    new Vector3(.070713f, .101497f, .069976f),
                    new Vector3(-.070713f, .101497f, .069976f),
                    new Vector3(.071462f, .154987f, -.495282f),
                    new Vector3(.069497f, .159743f, -.5185031f),
                    new Vector3(.053665f, .162502f, -.530194f),
                    new Vector3(.036587f, .159017f, -.515529f),
                    new Vector3(.035549f, .154733f, -.494399f),
                    new Vector3(.0472f, .151351f, -.47717f),
                    new Vector3(.062333f, .151765f, -.476295f),
                    new Vector3(-.036587f, .159017f, -.515529f),
                    new Vector3(-.035549f, .154733f, -.494399f),
                    new Vector3(-.0472f, .151351f, -.47717f),
                    new Vector3(-.062333f, .151765f, -.476295f),
                    new Vector3(-.071462f, .154987f, -.495282f),
                    new Vector3(-.069497f, .159743f, -.5185031f),
                    new Vector3(-.053665f, .162502f, -.530194f)
                };                                              //Ankles
            meshSeamVertices[1][0][2][1] = new[]
                {
                    new Vector3(0, .604671f, -.423148f),
                    new Vector3(0, .562809f, -.451407f),
                    new Vector3(.019152f, .575161f, -.439654f),
                    new Vector3(-.019152f, .575161f, -.439654f)
                };                                              //Tail
            meshSeamVertices[1][0][2][2] = new[]
                {
                    new Vector3(-.067992f, .768753f, .270502f),
                    new Vector3(-.063376f, .779874f, .276034f),
                    new Vector3(-.068048f, .761084f, .258355f),
                    new Vector3(-.066058f, .759641f, .242946f),
                    new Vector3(-.057926f, .780934f, .229652f),
                    new Vector3(-.053148f, .8037931f, .266716f),
                    new Vector3(-.056455f, .796371f, .272259f),
                    new Vector3(-.050583f, .805699f, .24951f),
                    new Vector3(-.052892f, .7954251f, .23486f),
                    new Vector3(.067992f, .768753f, .270503f),
                    new Vector3(.063376f, .779874f, .276035f),
                    new Vector3(.068048f, .761084f, .258356f),
                    new Vector3(.066058f, .759641f, .242947f),
                    new Vector3(.057926f, .780934f, .229653f),
                    new Vector3(.052892f, .7954251f, .23486f),
                    new Vector3(.056455f, .796371f, .27226f),
                    new Vector3(.053148f, .8037931f, .266717f),
                    new Vector3(.050583f, .805699f, .24951f)
                };                                              //Ears
            meshSeamVertices[1][0][2][3] = new[]
                {
                    new Vector3(0, .65108f, .263976f),
                    new Vector3(-.040149f, .6724421f, .244208f),
                    new Vector3(-.051842f, .6996421f, .220295f),
                    new Vector3(-.047474f, .719901f, .202111f),
                    new Vector3(-.036981f, .735922f, .186963f),
                    new Vector3(0, .754215f, .169697f),
                    new Vector3(-.019823f, .74883f, .174659f),
                    new Vector3(.040149f, .6724421f, .244208f),
                    new Vector3(.051842f, .6996421f, .220295f),
                    new Vector3(.047474f, .719901f, .202111f),
                    new Vector3(.036981f, .735922f, .186963f),
                    new Vector3(.019823f, .74883f, .174659f),
                    new Vector3(.022854f, .658232f, .256744f),
                    new Vector3(-.022854f, .658232f, .256744f)
                };                                              //Neck
            meshSeamVertices[1][0][2][4] = new Vector3[0];      //Waist
            meshSeamVertices[1][0][2][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[1][0][2][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[1][0][3] = new Vector3[7][];       //Adult Dog LOD3 seams
            meshSeamVertices[1][0][3][0] = new[]
                {
                    new Vector3(.083803f, .1126f, .11691f),
                    new Vector3(.059744f, .111575f, .116427f),
                    new Vector3(.054359f, .104351f, .079998f),
                    new Vector3(.087592f, .104716f, .081086f),
                    new Vector3(-.059744f, .111575f, .116427f),
                    new Vector3(-.083803f, .1126f, .11691f),
                    new Vector3(-.087592f, .104716f, .081086f),
                    new Vector3(-.054359f, .104351f, .079998f),
                    new Vector3(.070713f, .101497f, .069976f),
                    new Vector3(-.070713f, .101497f, .069976f),
                    new Vector3(.069497f, .159743f, -.5185031f),
                    new Vector3(.053665f, .162502f, -.530194f),
                    new Vector3(.036587f, .159017f, -.515529f),
                    new Vector3(.0472f, .151351f, -.47717f),
                    new Vector3(.062333f, .151765f, -.476295f),
                    new Vector3(-.036587f, .159017f, -.515529f),
                    new Vector3(-.0472f, .151351f, -.47717f),
                    new Vector3(-.062333f, .151765f, -.476295f),
                    new Vector3(-.069497f, .159743f, -.5185031f),
                    new Vector3(-.053665f, .162502f, -.530194f)
                };                                              //Ankles
            meshSeamVertices[1][0][3][1] = new[]
                {
                    new Vector3(0, .604671f, -.423148f),
                    new Vector3(0, .562809f, -.451407f),
                    new Vector3(.019152f, .575161f, -.439654f),
                    new Vector3(-.019152f, .575161f, -.439654f)
                };                                              //Tail
            meshSeamVertices[1][0][3][2] = new[]
                {
                    new Vector3(-.065684f, .774313f, .273268f),
                    new Vector3(-.06681f, .75988f, .25055f),
                    new Vector3(-.057926f, .780934f, .229652f),
                    new Vector3(-.054662f, .800348f, .269728f),
                    new Vector3(-.050898f, .80237f, .242183f),
                    new Vector3(.065684f, .774313f, .273269f),
                    new Vector3(.06681f, .75988f, .25055f),
                    new Vector3(.057926f, .780934f, .229653f),
                    new Vector3(.054662f, .800348f, .269728f),
                    new Vector3(.050898f, .80237f, .242183f)
                };                                              //Ears
            meshSeamVertices[1][0][3][3] = new[]
                {
                    new Vector3(0, .65108f, .263976f),
                    new Vector3(-.03184f, .664723f, .250976f),
                    new Vector3(-.051246f, .710773f, .211633f),
                    new Vector3(0, .754215f, .169697f),
                    new Vector3(-.029526f, .7441601f, .180104f),
                    new Vector3(.051246f, .710773f, .211633f),
                    new Vector3(.029526f, .7441601f, .180104f),
                    new Vector3(.03184f, .664723f, .250976f)
                };                                              //Neck
            meshSeamVertices[1][0][3][4] = new Vector3[0];      //Waist
            meshSeamVertices[1][0][3][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[1][0][3][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[1][1] = new Vector3[4][][];        //Puppy
            meshSeamVertices[1][1][0] = new Vector3[7][];       //Puppy LOD0 seams
            meshSeamVertices[1][1][0][0] = new[]
                {
                    new Vector3(.036768f, .026227f, .013628f),
                    new Vector3(.03593601f, .02782f, .017807f),
                    new Vector3(.033462f, .028612f, .02055f),
                    new Vector3(.027855f, .029023f, .022005f),
                    new Vector3(.022458f, .028545f, .020437f),
                    new Vector3(.019409f, .027613f, .017329f),
                    new Vector3(.018312f, .026146f, .013026f),
                    new Vector3(.018188f, .024345f, .008862f),
                    new Vector3(.019965f, .022352f, .004917f),
                    new Vector3(.024243f, .020768f, .001611f),
                    new Vector3(.03077f, .020477f, .001638f),
                    new Vector3(.035116f, .022422f, .005462f),
                    new Vector3(.036745f, .024428f, .009618001f),
                    new Vector3(-.018312f, .026146f, .013026f),
                    new Vector3(-.019409f, .027613f, .017329f),
                    new Vector3(-.022458f, .028545f, .020437f),
                    new Vector3(-.027855f, .029023f, .022005f),
                    new Vector3(-.033462f, .028612f, .02055f),
                    new Vector3(-.03593601f, .02782f, .017807f),
                    new Vector3(-.036768f, .026227f, .013628f),
                    new Vector3(-.036745f, .024428f, .009618001f),
                    new Vector3(-.035116f, .022422f, .005462f),
                    new Vector3(-.03077f, .020477f, .001639f),
                    new Vector3(-.024243f, .020768f, .001611f),
                    new Vector3(-.019965f, .022352f, .004917f),
                    new Vector3(-.018188f, .024345f, .008862f),
                    new Vector3(.027483f, .020485f, .000959f),
                    new Vector3(-.027483f, .020485f, .000958f),
                    new Vector3(.035483f, .026637f, -.156033f),
                    new Vector3(.035769f, .026894f, -.160339f),
                    new Vector3(.034603f, .027203f, -.1644f),
                    new Vector3(.032792f, .027444f, -.167616f),
                    new Vector3(.02773f, .027534f, -.16831f),
                    new Vector3(.022383f, .027477f, -.16705f),
                    new Vector3(.020598f, .027186f, -.164045f),
                    new Vector3(.019747f, .026911f, -.160276f),
                    new Vector3(.019792f, .026623f, -.155934f),
                    new Vector3(.021326f, .026195f, -.151986f),
                    new Vector3(.024209f, .025911f, -.149262f),
                    new Vector3(.02806f, .025941f, -.148507f),
                    new Vector3(.031656f, .026052f, -.148969f),
                    new Vector3(.03409901f, .026335f, -.152187f),
                    new Vector3(-.022383f, .027477f, -.16705f),
                    new Vector3(-.020598f, .027186f, -.164045f),
                    new Vector3(-.019747f, .026911f, -.160276f),
                    new Vector3(-.019792f, .026623f, -.155934f),
                    new Vector3(-.021326f, .026195f, -.151986f),
                    new Vector3(-.024209f, .025911f, -.149262f),
                    new Vector3(-.02806f, .025941f, -.148507f),
                    new Vector3(-.031656f, .026052f, -.148969f),
                    new Vector3(-.03409901f, .026335f, -.152187f),
                    new Vector3(-.035483f, .026637f, -.156033f),
                    new Vector3(-.035769f, .026894f, -.160339f),
                    new Vector3(-.034603f, .027203f, -.1644f),
                    new Vector3(-.032792f, .027444f, -.167616f),
                    new Vector3(-.02773f, .027534f, -.16831f)
                };                                              //Ankles
            meshSeamVertices[1][1][0][1] = new[]
                {
                    new Vector3(.008108f, .141443f, -.133774f),
                    new Vector3(.008026f, .147319f, -.129635f),
                    new Vector3(.004865f, .150341f, -.128202f),
                    new Vector3(0, .15195f, -.127799f),
                    new Vector3(0, .135842f, -.138938f),
                    new Vector3(.005545001f, .138504f, -.136499f),
                    new Vector3(-.008108f, .141443f, -.133774f),
                    new Vector3(-.008026f, .147319f, -.129635f),
                    new Vector3(-.004865f, .150341f, -.128202f),
                    new Vector3(-.005545001f, .138504f, -.136499f)
                };                                              //Tail
            meshSeamVertices[1][1][0][2] = new[]
                {
                    new Vector3(-.03149f, .202056f, .044791f),
                    new Vector3(-.029873f, .206929f, .047409f),
                    new Vector3(-.032279f, .198817f, .03928f),
                    new Vector3(-.031583f, .198393f, .032406f),
                    new Vector3(-.029422f, .202985f, .0271f),
                    new Vector3(-.027024f, .208296f, .026351f),
                    new Vector3(-.023789f, .217317f, .030254f),
                    new Vector3(-.0268f, .213567f, .047074f),
                    new Vector3(-.024298f, .218782f, .043602f),
                    new Vector3(-.03232f, .200156f, .042419f),
                    new Vector3(-.028384f, .210043f, .047718f),
                    new Vector3(-.025254f, .216639f, .045571f),
                    new Vector3(-.023084f, .220917f, .038748f),
                    new Vector3(-.023264f, .219516f, .033205f),
                    new Vector3(-.02485f, .213747f, .027885f),
                    new Vector3(-.031973f, .198507f, .036242f),
                    new Vector3(.03149f, .202056f, .044791f),
                    new Vector3(.029873f, .206929f, .047409f),
                    new Vector3(.032279f, .198817f, .03928f),
                    new Vector3(.031583f, .198393f, .032406f),
                    new Vector3(.029422f, .202985f, .0271f),
                    new Vector3(.027024f, .208296f, .026351f),
                    new Vector3(.023789f, .217317f, .030254f),
                    new Vector3(.0268f, .213567f, .047074f),
                    new Vector3(.024298f, .218782f, .043602f),
                    new Vector3(.03232f, .200156f, .042419f),
                    new Vector3(.028384f, .210043f, .047718f),
                    new Vector3(.025254f, .216639f, .045571f),
                    new Vector3(.023084f, .220917f, .038748f),
                    new Vector3(.023264f, .219516f, .033205f),
                    new Vector3(.02485f, .213747f, .027885f),
                    new Vector3(.031973f, .198507f, .036242f)
                };                                              //Ears
            meshSeamVertices[1][1][0][3] = new[]
                {
                    new Vector3(0, .145662f, .05409f),
                    new Vector3(-.015691f, .152265f, .04751001f),
                    new Vector3(-.020114f, .156391f, .04372f),
                    new Vector3(-.026019f, .171842f, .030004f),
                    new Vector3(-.024575f, .181631f, .021221f),
                    new Vector3(-.019194f, .18951f, .014427f),
                    new Vector3(-.01529f, .192578f, .011842f),
                    new Vector3(-.005277f, .19623f, .008721f),
                    new Vector3(0, .197183f, .007899f),
                    new Vector3(-.005727f, .146799f, .052907f),
                    new Vector3(-.025166f, .166557f, .034826f),
                    new Vector3(-.010536f, .194739f, .009992f),
                    new Vector3(-.021994f, .185906f, .017399f),
                    new Vector3(.015691f, .152265f, .04751001f),
                    new Vector3(.020114f, .156391f, .04372f),
                    new Vector3(.026019f, .171842f, .030004f),
                    new Vector3(.024575f, .181631f, .021221f),
                    new Vector3(.019194f, .18951f, .014427f),
                    new Vector3(.01529f, .192578f, .011842f),
                    new Vector3(.005277f, .19623f, .008721f),
                    new Vector3(.005727f, .146799f, .052907f),
                    new Vector3(.025166f, .166557f, .034826f),
                    new Vector3(.010536f, .194739f, .009992f),
                    new Vector3(.021994f, .185906f, .017399f),
                    new Vector3(.025781f, .177038f, .025226f),
                    new Vector3(.023169f, .161847f, .038919f),
                    new Vector3(.010967f, .149102f, .050558f),
                    new Vector3(-.025781f, .177038f, .025226f),
                    new Vector3(-.023169f, .161847f, .038919f),
                    new Vector3(-.010967f, .149102f, .050558f)
                };                                              //Neck
            meshSeamVertices[1][1][0][4] = new Vector3[0];      //Waist
            meshSeamVertices[1][1][0][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[1][1][0][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[1][1][1] = new Vector3[7][];       //Puppy LOD1 seams
            meshSeamVertices[1][1][1][0] = new[]
                {
                    new Vector3(.035936f, .027821f, .017807f),
                    new Vector3(.033462f, .028613f, .020549f),
                    new Vector3(.027855f, .029024f, .022004f),
                    new Vector3(.022458f, .028546f, .020436f),
                    new Vector3(.019409f, .027614f, .017328f),
                    new Vector3(.01825f, .025246f, .010943f),
                    new Vector3(.019965f, .022352f, .004917f),
                    new Vector3(.024243f, .020769f, .00161f),
                    new Vector3(.03077f, .020477f, .001638f),
                    new Vector3(.035116f, .022422f, .005462f),
                    new Vector3(.036756f, .025328f, .011623f),
                    new Vector3(-.019409f, .027614f, .017328f),
                    new Vector3(-.022458f, .028546f, .020436f),
                    new Vector3(-.027855f, .029024f, .022004f),
                    new Vector3(-.033462f, .028613f, .020549f),
                    new Vector3(-.035936f, .027821f, .017807f),
                    new Vector3(-.036756f, .025328f, .011623f),
                    new Vector3(-.035116f, .022422f, .005462f),
                    new Vector3(-.03077f, .020477f, .001638f),
                    new Vector3(-.024243f, .020769f, .00161f),
                    new Vector3(-.019965f, .022352f, .004917f),
                    new Vector3(-.01825f, .025246f, .010943f),
                    new Vector3(.027483f, .020485f, .000958f),
                    new Vector3(-.027483f, .020485f, .000958f),
                    new Vector3(.033966f, .02768f, -.151808f),
                    new Vector3(.035505f, .028593f, -.157844f),
                    new Vector3(.03365801f, .029292f, -.165822f),
                    new Vector3(.02771f, .029659f, -.168411f),
                    new Vector3(.021542f, .029336f, -.165628f),
                    new Vector3(.019765f, .028546f, -.157617f),
                    new Vector3(.021437f, .027542f, -.151612f),
                    new Vector3(.024334f, .027091f, -.148868f),
                    new Vector3(.028081f, .027075f, -.148205f),
                    new Vector3(.031514f, .027209f, -.148604f),
                    new Vector3(-.021542f, .029336f, -.165628f),
                    new Vector3(-.019765f, .028546f, -.157617f),
                    new Vector3(-.021437f, .027542f, -.151612f),
                    new Vector3(-.024334f, .027091f, -.148868f),
                    new Vector3(-.028081f, .027075f, -.148205f),
                    new Vector3(-.031514f, .027209f, -.148604f),
                    new Vector3(-.03365801f, .029292f, -.165822f),
                    new Vector3(-.02771f, .029659f, -.168411f),
                    new Vector3(-.033966f, .02768f, -.151808f),
                    new Vector3(-.035505f, .028593f, -.157844f)
                };                                              //Ankles
            meshSeamVertices[1][1][1][1] = new[]
                {
                    new Vector3(.008108f, .141444f, -.133774f),
                    new Vector3(.006446f, .148831f, -.128919f),
                    new Vector3(0, .151951f, -.1278f),
                    new Vector3(0, .135843f, -.138939f),
                    new Vector3(.005545f, .138505f, -.136499f),
                    new Vector3(-.008108f, .141444f, -.133774f),
                    new Vector3(-.006445f, .148831f, -.128919f),
                    new Vector3(-.005545f, .138505f, -.136499f)
                };                                              //Tail
            meshSeamVertices[1][1][1][2] = new[]
                {
                    new Vector3(-.03149001f, .202056f, .044792f),
                    new Vector3(-.031583f, .198393f, .032406f),
                    new Vector3(-.029422f, .202985f, .0271f),
                    new Vector3(-.027024f, .208296f, .026351f),
                    new Vector3(-.023789f, .217317f, .030254f),
                    new Vector3(-.0268f, .213567f, .047074f),
                    new Vector3(-.024298f, .218782f, .043602f),
                    new Vector3(-.032299f, .199486f, .04085f),
                    new Vector3(-.028384f, .210043f, .047718f),
                    new Vector3(-.025254f, .216639f, .045571f),
                    new Vector3(-.023174f, .220216f, .035976f),
                    new Vector3(-.02485f, .213747f, .027885f),
                    new Vector3(-.031973f, .198507f, .036242f),
                    new Vector3(.03149001f, .202056f, .04479101f),
                    new Vector3(.032299f, .199486f, .04085f),
                    new Vector3(.031583f, .198393f, .032406f),
                    new Vector3(.029422f, .202985f, .0271f),
                    new Vector3(.027024f, .208296f, .026351f),
                    new Vector3(.023789f, .217317f, .030254f),
                    new Vector3(.0268f, .213567f, .047074f),
                    new Vector3(.024298f, .218782f, .043602f),
                    new Vector3(.028384f, .210043f, .047718f),
                    new Vector3(.025254f, .216639f, .045571f),
                    new Vector3(.023174f, .220216f, .035976f),
                    new Vector3(.02485f, .213747f, .027885f),
                    new Vector3(.031973f, .198507f, .036242f)
                };                                              //Ears
            meshSeamVertices[1][1][1][3] = new[]
                {
                    new Vector3(0, .145662f, .05409f),
                    new Vector3(-.015691f, .152266f, .04751001f),
                    new Vector3(-.024575f, .181631f, .021221f),
                    new Vector3(-.020594f, .187709f, .015912f),
                    new Vector3(-.01529f, .192578f, .011842f),
                    new Vector3(-.007906f, .195485f, .009356001f),
                    new Vector3(0, .197183f, .007898f),
                    new Vector3(-.025166f, .166557f, .034826f),
                    new Vector3(.015691f, .152266f, .04751001f),
                    new Vector3(.021641f, .159119f, .04131901f),
                    new Vector3(.0259f, .17444f, .027614f),
                    new Vector3(.024575f, .181631f, .021221f),
                    new Vector3(.01529f, .192578f, .011842f),
                    new Vector3(.008347f, .147951f, .051732f),
                    new Vector3(.025166f, .166557f, .034826f),
                    new Vector3(.007906f, .195485f, .009356001f),
                    new Vector3(.020594f, .187709f, .015912f),
                    new Vector3(-.0259f, .17444f, .027614f),
                    new Vector3(-.021641f, .159119f, .04131901f),
                    new Vector3(-.008347f, .147951f, .051732f)
                };                                              //Neck
            meshSeamVertices[1][1][1][4] = new Vector3[0];      //Waist
            meshSeamVertices[1][1][1][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[1][1][1][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[1][1][2] = new Vector3[7][];       //Puppy LOD2 seams
            meshSeamVertices[1][1][2][0] = new[]
                {
                    new Vector3(.036768f, .026228f, .013627f),
                    new Vector3(.033462f, .028613f, .020549f),
                    new Vector3(.022458f, .028546f, .020436f),
                    new Vector3(.018312f, .026146f, .013025f),
                    new Vector3(.019965f, .022352f, .004917f),
                    new Vector3(.035116f, .022422f, .005462f),
                    new Vector3(-.018312f, .026146f, .013025f),
                    new Vector3(-.022458f, .028546f, .020436f),
                    new Vector3(-.033462f, .028613f, .020549f),
                    new Vector3(-.036768f, .026228f, .013627f),
                    new Vector3(-.035116f, .022422f, .005462f),
                    new Vector3(-.019965f, .022352f, .004917f),
                    new Vector3(.027483f, .020485f, .000958f),
                    new Vector3(-.027483f, .020485f, .000958f),
                    new Vector3(.035221f, .033182f, -.154798f),
                    new Vector3(.03444f, .035381f, -.164102f),
                    new Vector3(.027588f, .036252f, -.168875f),
                    new Vector3(.020287f, .035234f, -.163587f),
                    new Vector3(.01974f, .033091f, -.154437f),
                    new Vector3(.024645f, .031387f, -.147424f),
                    new Vector3(.031218f, .031604f, -.147082f),
                    new Vector3(-.020287f, .035234f, -.163587f),
                    new Vector3(-.01974f, .033091f, -.154437f),
                    new Vector3(-.024645f, .031387f, -.147424f),
                    new Vector3(-.031218f, .031604f, -.147082f),
                    new Vector3(-.035221f, .033182f, -.154798f),
                    new Vector3(-.03444f, .035381f, -.164102f),
                    new Vector3(-.027588f, .036252f, -.168875f)
                };                                              //Ankles
            meshSeamVertices[1][1][2][1] = new[]
                {
                    new Vector3(0, .151951f, -.1278f),
                    new Vector3(0, .135843f, -.138939f),
                    new Vector3(.007262f, .14805f, -.129288f),
                    new Vector3(-.007262f, .14805f, -.129289f)
                };                                              //Tail
            meshSeamVertices[1][1][2][2] = new[]
                {
                    new Vector3(-.03149001f, .202056f, .04479101f),
                    new Vector3(-.029873f, .206929f, .047409f),
                    new Vector3(-.032279f, .198817f, .03928f),
                    new Vector3(-.031583f, .198393f, .032406f),
                    new Vector3(-.027024f, .208296f, .026351f),
                    new Vector3(-.024298f, .218782f, .043602f),
                    new Vector3(-.026027f, .215103f, .046323f),
                    new Vector3(-.023174f, .220216f, .035976f),
                    new Vector3(-.02432f, .215532f, .02907f),
                    new Vector3(.03149001f, .202056f, .04479101f),
                    new Vector3(.029873f, .206929f, .047409f),
                    new Vector3(.032279f, .198817f, .03928f),
                    new Vector3(.031583f, .198393f, .032406f),
                    new Vector3(.027024f, .208296f, .026351f),
                    new Vector3(.02432f, .215532f, .02907f),
                    new Vector3(.026027f, .215103f, .046323f),
                    new Vector3(.024298f, .218782f, .043602f),
                    new Vector3(.023174f, .220216f, .035976f)
                };                                              //Ears
            meshSeamVertices[1][1][2][3] = new[]
                {
                    new Vector3(0, .145662f, .05409f),
                    new Vector3(-.020114f, .156391f, .043719f),
                    new Vector3(-.026019f, .171842f, .030003f),
                    new Vector3(-.024575f, .181631f, .021221f),
                    new Vector3(-.019194f, .18951f, .014426f),
                    new Vector3(0, .197183f, .007898f),
                    new Vector3(-.010536f, .19474f, .009992f),
                    new Vector3(.020114f, .156391f, .043719f),
                    new Vector3(.026019f, .171842f, .030003f),
                    new Vector3(.024575f, .181631f, .021221f),
                    new Vector3(.019194f, .18951f, .014426f),
                    new Vector3(.010536f, .19474f, .009992f),
                    new Vector3(.010967f, .149103f, .050557f),
                    new Vector3(-.010967f, .149103f, .050557f)
                };                                              //Neck
            meshSeamVertices[1][1][2][4] = new Vector3[0];      //Waist
            meshSeamVertices[1][1][2][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[1][1][2][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[1][1][3] = new Vector3[7][];       //Puppy LOD3 seams
            meshSeamVertices[1][1][3][0] = new[]
                {
                    new Vector3(.033462f, .028613f, .020549f),
                    new Vector3(.022458f, .028546f, .020436f),
                    new Vector3(.019965f, .022352f, .004917f),
                    new Vector3(.035116f, .022422f, .005462f),
                    new Vector3(-.022458f, .028546f, .020436f),
                    new Vector3(-.033462f, .028613f, .020549f),
                    new Vector3(-.035116f, .022422f, .005462f),
                    new Vector3(-.019965f, .022352f, .004917f),
                    new Vector3(.027483f, .020485f, .000958f),
                    new Vector3(-.027483f, .020485f, .000958f),
                    new Vector3(.03444f, .035381f, -.164102f),
                    new Vector3(.027588f, .036252f, -.168875f),
                    new Vector3(.020287f, .035234f, -.163587f),
                    new Vector3(.024645f, .031387f, -.147424f),
                    new Vector3(.031218f, .031604f, -.147082f),
                    new Vector3(-.020287f, .035234f, -.163587f),
                    new Vector3(-.024645f, .031387f, -.147424f),
                    new Vector3(-.031218f, .031604f, -.147082f),
                    new Vector3(-.03444f, .035381f, -.164102f),
                    new Vector3(-.027588f, .036252f, -.168875f)
                };                                              //Ankles
            meshSeamVertices[1][1][3][1] = new[]
                {
                    new Vector3(0, .151951f, -.1278f),
                    new Vector3(0, .135843f, -.138939f),
                    new Vector3(.007824f, .147512f, -.129544f),
                    new Vector3(-.007824f, .147512f, -.129544f)
                };                                              //Tail
            meshSeamVertices[1][1][3][2] = new[]
                {
                    new Vector3(-.030682f, .204493f, .0461f),
                    new Vector3(-.031915f, .198441f, .035844f),
                    new Vector3(-.027024f, .208296f, .026351f),
                    new Vector3(-.024986f, .217198f, .045089f),
                    new Vector3(-.023382f, .219023f, .032543f),
                    new Vector3(.030682f, .204493f, .0461f),
                    new Vector3(.031915f, .198441f, .035844f),
                    new Vector3(.027024f, .208296f, .026351f),
                    new Vector3(.024986f, .217198f, .045089f),
                    new Vector3(.023382f, .219023f, .032543f)
                };                                              //Ears
            meshSeamVertices[1][1][3][3] = new[]
                {
                    new Vector3(0, .145662f, .05409f),
                    new Vector3(-.01605f, .152601f, .047202f),
                    new Vector3(-.025785f, .176954f, .025303f),
                    new Vector3(0, .197183f, .007898f),
                    new Vector3(-.0154f, .192492f, .011914f),
                    new Vector3(.025785f, .176954f, .025303f),
                    new Vector3(.0154f, .192492f, .011914f),
                    new Vector3(.01605f, .152601f, .047202f)
                };                                              //Neck
            meshSeamVertices[1][1][3][4] = new Vector3[0];      //Waist
            meshSeamVertices[1][1][3][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[1][1][3][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[2] = new Vector3[4][][][];         //ageSpecies
            meshSeamVertices[2][0] = new Vector3[4][][];        //Adult Cat
            meshSeamVertices[2][0][0] = new Vector3[7][];       //Adult Cat LOD0 seams
            meshSeamVertices[2][0][0][0] = new[]
                {
                    new Vector3(.03187f, .04709f, .04364f),
                    new Vector3(.0239f, .04833f, .05513f),
                    new Vector3(.02666f, .05041f, .06838f),
                    new Vector3(.03171f, .05114f, .07282f),
                    new Vector3(.04434f, .05157f, .07285f),
                    new Vector3(.04968f, .05119f, .06906f),
                    new Vector3(.05158f, .04802f, .04905f),
                    new Vector3(.04707f, .04709001f, .04262f),
                    new Vector3(.03326f, .052579f, -.23745f),
                    new Vector3(.02857f, .052609f, -.24304f),
                    new Vector3(.02676f, .052669f, -.25031f),
                    new Vector3(.02741f, .052829f, -.25657f),
                    new Vector3(.02854f, .052829f, -.26258f),
                    new Vector3(.03167f, .052849f, -.26771f),
                    new Vector3(.04803f, .052849f, -.26868f),
                    new Vector3(.05164f, .052829f, -.26331f),
                    new Vector3(.05357f, .052829f, -.25688f),
                    new Vector3(.05385f, .052829f, -.25001f),
                    new Vector3(.05187f, .052759f, -.24285f),
                    new Vector3(.04686f, .052689f, -.23775f),
                    new Vector3(.04009f, .052619f, -.23539f),
                    new Vector3(.04026f, .052849f, -.27037f),
                    new Vector3(.03794f, .05174f, .07458f),
                    new Vector3(.03962f, .04686f, .04151f),
                    new Vector3(.02381f, .04983f, .0623f),
                    new Vector3(.05228f, .05042f, .0631f),
                    new Vector3(.05311f, .04933f, .05638f),
                    new Vector3(.02697f, .04753f, .04869f),
                    new Vector3(-.03187f, .04709f, .04364f),
                    new Vector3(-.0239f, .04833f, .05513f),
                    new Vector3(-.02666f, .05041f, .06838f),
                    new Vector3(-.03171f, .05114f, .07282f),
                    new Vector3(-.04434f, .05157f, .07285f),
                    new Vector3(-.04968f, .05119f, .06906f),
                    new Vector3(-.05158f, .04802f, .04905f),
                    new Vector3(-.04707f, .04709001f, .04262f),
                    new Vector3(-.03326f, .052579f, -.23745f),
                    new Vector3(-.02857f, .052609f, -.24304f),
                    new Vector3(-.02676f, .052669f, -.25031f),
                    new Vector3(-.02741f, .052829f, -.25657f),
                    new Vector3(-.02854f, .052829f, -.26258f),
                    new Vector3(-.03167f, .052849f, -.26771f),
                    new Vector3(-.04803f, .052849f, -.26868f),
                    new Vector3(-.05164f, .052829f, -.26331f),
                    new Vector3(-.05357f, .052829f, -.25688f),
                    new Vector3(-.05385f, .052829f, -.25001f),
                    new Vector3(-.05187f, .052759f, -.24285f),
                    new Vector3(-.04686f, .052689f, -.23775f),
                    new Vector3(-.04009f, .052619f, -.23539f),
                    new Vector3(-.04026f, .052849f, -.27037f),
                    new Vector3(-.03794f, .05174f, .07458f),
                    new Vector3(-.03962f, .04686f, .04151f),
                    new Vector3(-.02381f, .04983f, .0623f),
                    new Vector3(-.05228f, .05042f, .0631f),
                    new Vector3(-.05311f, .04933f, .05638f),
                    new Vector3(-.02697f, .04753f, .04869f)
                };                                              //Ankles
            meshSeamVertices[2][0][0][1] = new[]
                {
                    new Vector3(.01765f, .26168f, -.23504f),
                    new Vector3(0, .25412f, -.23691f),
                    new Vector3(.0238f, .27703f, -.22732f),
                    new Vector3(.01977f, .28779f, -.22239f),
                    new Vector3(.01326f, .29217f, -.2198f),
                    new Vector3(0, .29452f, -.21842f),
                    new Vector3(-.01326f, .29217f, -.2198f),
                    new Vector3(-.01977f, .28779f, -.22239f),
                    new Vector3(-.0238f, .27703f, -.22732f),
                    new Vector3(-.01765f, .26168f, -.23504f)
                };                                              //Tail
            meshSeamVertices[2][0][0][2] = new[]
                {
                    new Vector3(-.01822f, .36544f, .157701f),
                    new Vector3(-.0239f, .36349f, .158141f),
                    new Vector3(-.05495f, .34042f, .133701f),
                    new Vector3(-.02872f, .36095f, .113401f),
                    new Vector3(-.01819f, .37046f, .146851f),
                    new Vector3(-.04298f, .34484f, .113721f),
                    new Vector3(-.03462f, .35361f, .109461f),
                    new Vector3(-.05576f, .33682f, .130501f),
                    new Vector3(-.05117f, .34445f, .137631f),
                    new Vector3(-.04628f, .34964f, .138691f),
                    new Vector3(-.03829f, .35765f, .144011f),
                    new Vector3(-.03082f, .36234f, .150591f),
                    new Vector3(-.02294f, .36797f, .122981f),
                    new Vector3(-.02f, .37197f, .134161f),
                    new Vector3(-.05104f, .33899f, .121241f),
                    new Vector3(.01822f, .36544f, .157701f),
                    new Vector3(.0239f, .36349f, .158141f),
                    new Vector3(.05495f, .34042f, .133701f),
                    new Vector3(.02872f, .36095f, .113401f),
                    new Vector3(.01819f, .37046f, .146851f),
                    new Vector3(.04298f, .34484f, .113721f),
                    new Vector3(.03462f, .35361f, .109461f),
                    new Vector3(.05576f, .33682f, .130501f),
                    new Vector3(.05117f, .34445f, .137631f),
                    new Vector3(.04628f, .34964f, .138691f),
                    new Vector3(.03829f, .35765f, .144011f),
                    new Vector3(.03082f, .36234f, .150591f),
                    new Vector3(.02294f, .36797f, .122981f),
                    new Vector3(.02f, .37197f, .134161f),
                    new Vector3(.05104f, .33899f, .121241f)
                };                                              //Ears
            meshSeamVertices[2][0][0][3] = new[]
                {
                    new Vector3(0, .27073f, .134301f),
                    new Vector3(0, .32871f, .075801f),
                    new Vector3(.042f, .31052f, .093541f),
                    new Vector3(.04632f, .30462f, .100251f),
                    new Vector3(.04845f, .29819f, .107681f),
                    new Vector3(.04489f, .29179f, .114121f),
                    new Vector3(.03882f, .28568f, .120121f),
                    new Vector3(.03178f, .28098f, .124551f),
                    new Vector3(.02408f, .27728f, .127931f),
                    new Vector3(.0159f, .27415f, .130781f),
                    new Vector3(.007960001f, .27215f, .132871f),
                    new Vector3(.01074f, .3273f, .077111f),
                    new Vector3(.02059f, .32455f, .079541f),
                    new Vector3(.02916f, .32087f, .083081f),
                    new Vector3(.03623001f, .31613f, .087741f),
                    new Vector3(-.042f, .31052f, .093541f),
                    new Vector3(-.04632f, .30462f, .100251f),
                    new Vector3(-.04845f, .29819f, .107681f),
                    new Vector3(-.04489f, .29179f, .114121f),
                    new Vector3(-.03882f, .28568f, .120121f),
                    new Vector3(-.03178f, .28098f, .124551f),
                    new Vector3(-.02408f, .27728f, .127931f),
                    new Vector3(-.0159f, .27415f, .130781f),
                    new Vector3(-.007960001f, .27215f, .132871f),
                    new Vector3(-.01074f, .3273f, .077111f),
                    new Vector3(-.02059f, .32455f, .079541f),
                    new Vector3(-.02916f, .32087f, .083081f),
                    new Vector3(-.03623001f, .31613f, .087741f)
                };                                              //Neck
            meshSeamVertices[2][0][0][4] = new Vector3[0];      //Waist
            meshSeamVertices[2][0][0][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[2][0][0][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[2][0][1] = new Vector3[7][];       //Adult Cat LOD1 seams
            meshSeamVertices[2][0][1][0] = new[]
                {
                    new Vector3(.03187f, .04709f, .04364f),
                    new Vector3(.02666f, .05041f, .06838f),
                    new Vector3(.03171f, .05114f, .07282f),
                    new Vector3(.04114f, .051655f, .073715f),
                    new Vector3(.04968f, .05119f, .06906f),
                    new Vector3(.05158f, .04802f, .04905f),
                    new Vector3(.04707f, .04709001f, .04262f),
                    new Vector3(.03326f, .052579f, -.23745f),
                    new Vector3(.02857f, .052609f, -.24304f),
                    new Vector3(.027085f, .052749f, -.25344f),
                    new Vector3(.03167f, .052849f, -.26771f),
                    new Vector3(.049835f, .052839f, -.265995f),
                    new Vector3(.05371f, .052829f, -.253445f),
                    new Vector3(.049365f, .052724f, -.2403f),
                    new Vector3(.04009f, .052619f, -.23539f),
                    new Vector3(.04026f, .052849f, -.27037f),
                    new Vector3(.03962f, .04686f, .04151f),
                    new Vector3(.023855f, .04908f, .058715f),
                    new Vector3(.052695f, .049875f, .05974f),
                    new Vector3(.02697f, .04753f, .04869f),
                    new Vector3(-.03187f, .04709f, .04364f),
                    new Vector3(-.02666f, .05041f, .06838f),
                    new Vector3(-.03171f, .05114f, .07282f),
                    new Vector3(-.04114f, .051655f, .073715f),
                    new Vector3(-.04968f, .05119f, .06906f),
                    new Vector3(-.05158f, .04802f, .04905f),
                    new Vector3(-.04707f, .04709001f, .04262f),
                    new Vector3(-.03326f, .052579f, -.23745f),
                    new Vector3(-.02857f, .052609f, -.24304f),
                    new Vector3(-.027085f, .052749f, -.25344f),
                    new Vector3(-.03167f, .052849f, -.26771f),
                    new Vector3(-.049835f, .052839f, -.265995f),
                    new Vector3(-.05371f, .052829f, -.253445f),
                    new Vector3(-.049365f, .052724f, -.2403f),
                    new Vector3(-.04009f, .052619f, -.23539f),
                    new Vector3(-.04026f, .052849f, -.27037f),
                    new Vector3(-.03962f, .04686f, .04151f),
                    new Vector3(-.023855f, .04908f, .058715f),
                    new Vector3(-.052695f, .049875f, .05974f),
                    new Vector3(-.02697f, .04753f, .04869f)
                };                                              //Ankles
            meshSeamVertices[2][0][1][1] = new[]
                {
                    new Vector3(.01765f, .26168f, -.23504f),
                    new Vector3(0, .25412f, -.23691f),
                    new Vector3(.0238f, .27703f, -.22732f),
                    new Vector3(.016515f, .28998f, -.221095f),
                    new Vector3(0, .29452f, -.21842f),
                    new Vector3(-.016515f, .28998f, -.221095f),
                    new Vector3(-.0238f, .27703f, -.22732f),
                    new Vector3(-.01765f, .26168f, -.23504f)
                };                                              //Tail
            meshSeamVertices[2][0][1][2] = new[]
                {
                    new Vector3(.01822f, .36544f, .157701f),
                    new Vector3(.0239f, .36349f, .158141f),
                    new Vector3(.05495f, .34042f, .133701f),
                    new Vector3(.02583f, .36446f, .118191f),
                    new Vector3(.01819f, .37046f, .146851f),
                    new Vector3(.03462f, .35361f, .109461f),
                    new Vector3(.05576f, .33682f, .130501f),
                    new Vector3(.05117f, .34445f, .137631f),
                    new Vector3(.04628f, .34964f, .138691f),
                    new Vector3(.034555f, .359995f, .147301f),
                    new Vector3(.02f, .37197f, .134161f),
                    new Vector3(.04701f, .341915f, .117481f),
                    new Vector3(-.01822f, .36544f, .157701f),
                    new Vector3(-.0239f, .36349f, .158141f),
                    new Vector3(-.05495f, .34042f, .133701f),
                    new Vector3(-.02583f, .36446f, .118191f),
                    new Vector3(-.01819f, .37046f, .146851f),
                    new Vector3(-.03462f, .35361f, .109461f),
                    new Vector3(-.05576f, .33682f, .130501f),
                    new Vector3(-.05117f, .34445f, .137631f),
                    new Vector3(-.04628f, .34964f, .138691f),
                    new Vector3(-.034555f, .359995f, .147301f),
                    new Vector3(-.02f, .37197f, .134161f),
                    new Vector3(-.04701f, .341915f, .117481f)
                };                                              //Ears
            meshSeamVertices[2][0][1][3] = new[]
                {
                    new Vector3(0, .27073f, .134301f),
                    new Vector3(0, .32871f, .075801f),
                    new Vector3(.039115f, .313325f, .09064101f),
                    new Vector3(.04632f, .30462f, .100251f),
                    new Vector3(.04667f, .29499f, .110901f),
                    new Vector3(.03882f, .28568f, .120121f),
                    new Vector3(.02793f, .27913f, .126241f),
                    new Vector3(.01193f, .27315f, .131826f),
                    new Vector3(.015665f, .325925f, .078326f),
                    new Vector3(.02916f, .32087f, .083081f),
                    new Vector3(-.039115f, .313325f, .09064101f),
                    new Vector3(-.04632f, .30462f, .100251f),
                    new Vector3(-.04667f, .29499f, .110901f),
                    new Vector3(-.03882f, .28568f, .120121f),
                    new Vector3(-.02793f, .27913f, .126241f),
                    new Vector3(-.01193f, .27315f, .131826f),
                    new Vector3(-.015665f, .325925f, .078326f),
                    new Vector3(-.02916f, .32087f, .083081f)
                };                                              //Neck
            meshSeamVertices[2][0][1][4] = new Vector3[0];      //Waist
            meshSeamVertices[2][0][1][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[2][0][1][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[2][0][2] = new Vector3[7][];       //Adult Cat LOD2 seams
            meshSeamVertices[2][0][2][0] = new[]
                {
                    new Vector3(.03187f, .04709f, .04364f),
                    new Vector3(.029185f, .050775f, .0706f),
                    new Vector3(.04114f, .051655f, .073715f),
                    new Vector3(.04968f, .05119f, .06906f),
                    new Vector3(.052137f, .048947f, .054395f),
                    new Vector3(.02857f, .052609f, -.24304f),
                    new Vector3(.027085f, .052749f, -.25344f),
                    new Vector3(.030105f, .052839f, -.265145f),
                    new Vector3(.049835f, .052839f, -.265995f),
                    new Vector3(.05371f, .052829f, -.253445f),
                    new Vector3(.049365f, .052724f, -.2403f),
                    new Vector3(.036675f, .05259901f, -.23642f),
                    new Vector3(.04026f, .052849f, -.27037f),
                    new Vector3(.043345f, .046975f, .042065f),
                    new Vector3(.025412f, .048305f, .053703f),
                    new Vector3(-.03187f, .04709f, .04364f),
                    new Vector3(-.029185f, .050775f, .0706f),
                    new Vector3(-.04114f, .051655f, .073715f),
                    new Vector3(-.04968f, .05119f, .06906f),
                    new Vector3(-.052137f, .048947f, .054395f),
                    new Vector3(-.02857f, .052609f, -.24304f),
                    new Vector3(-.027085f, .052749f, -.25344f),
                    new Vector3(-.030105f, .052839f, -.265145f),
                    new Vector3(-.049835f, .052839f, -.265995f),
                    new Vector3(-.05371f, .052829f, -.253445f),
                    new Vector3(-.049365f, .052724f, -.2403f),
                    new Vector3(-.036675f, .05259901f, -.23642f),
                    new Vector3(-.04026f, .052849f, -.27037f),
                    new Vector3(-.043345f, .046975f, .042065f),
                    new Vector3(-.025412f, .048305f, .053703f)
                };                                              //Ankles
            meshSeamVertices[2][0][2][1] = new[]
                {
                    new Vector3(.01765f, .26168f, -.23504f),
                    new Vector3(0, .25412f, -.23691f),
                    new Vector3(.020157f, .283505f, -.224207f),
                    new Vector3(0, .29452f, -.21842f),
                    new Vector3(-.020157f, .283505f, -.224207f),
                    new Vector3(-.01765f, .26168f, -.23504f)
                };                                              //Tail
            meshSeamVertices[2][0][2][2] = new[]
                {
                    new Vector3(.0239f, .36349f, .158141f),
                    new Vector3(.05495f, .34042f, .133701f),
                    new Vector3(.018205f, .36795f, .152276f),
                    new Vector3(.03167f, .35728f, .111431f),
                    new Vector3(.05576f, .33682f, .130501f),
                    new Vector3(.05117f, .34445f, .137631f),
                    new Vector3(.042285f, .353645f, .141351f),
                    new Vector3(.02147f, .36997f, .128571f),
                    new Vector3(.04701f, .341915f, .117481f),
                    new Vector3(.03082f, .36234f, .150591f),
                    new Vector3(-.0239f, .36349f, .158141f),
                    new Vector3(-.05495f, .34042f, .133701f),
                    new Vector3(-.018205f, .36795f, .152276f),
                    new Vector3(-.03167f, .35728f, .111431f),
                    new Vector3(-.05576f, .33682f, .130501f),
                    new Vector3(-.05117f, .34445f, .137631f),
                    new Vector3(-.042285f, .353645f, .141351f),
                    new Vector3(-.03082f, .36234f, .150591f),
                    new Vector3(-.02147f, .36997f, .128571f),
                    new Vector3(-.04701f, .341915f, .117481f)
                };                                              //Ears
            meshSeamVertices[2][0][2][3] = new[]
                {
                    new Vector3(0, .27073f, .134301f),
                    new Vector3(0, .32871f, .075801f),
                    new Vector3(.039115f, .313325f, .09064101f),
                    new Vector3(.04632f, .30462f, .100251f),
                    new Vector3(.04274501f, .290335f, .115511f),
                    new Vector3(.01993f, .27614f, .129034f),
                    new Vector3(.022412f, .323397f, .08070301f),
                    new Vector3(-.039115f, .313325f, .09064101f),
                    new Vector3(-.04632f, .30462f, .100251f),
                    new Vector3(-.04274501f, .290335f, .115511f),
                    new Vector3(-.01993f, .27614f, .129034f),
                    new Vector3(-.022412f, .323397f, .08070301f)
                };                                              //Neck
            meshSeamVertices[2][0][2][4] = new Vector3[0];      //Waist
            meshSeamVertices[2][0][2][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[2][0][2][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[2][0][3] = new Vector3[7][];       //Adult Cat LOD3 seams
            meshSeamVertices[2][0][3][0] = new[]
                {
                    new Vector3(.04114f, .051655f, .073715f),
                    new Vector3(.04968f, .05119f, .06906f),
                    new Vector3(.052137f, .048947f, .054395f),
                    new Vector3(.02857f, .052609f, -.24304f),
                    new Vector3(.027085f, .052749f, -.25344f),
                    new Vector3(.030105f, .052839f, -.265145f),
                    new Vector3(.05371f, .052829f, -.253445f),
                    new Vector3(.04302f, .052661f, -.23836f),
                    new Vector3(.045053f, .052844f, -.268183f),
                    new Vector3(.043345f, .046975f, .042065f),
                    new Vector3(.028641f, .047697f, .048672f),
                    new Vector3(-.04114f, .051655f, .073715f),
                    new Vector3(-.04968f, .05119f, .06906f),
                    new Vector3(-.052137f, .048947f, .054395f),
                    new Vector3(-.02857f, .052609f, -.24304f),
                    new Vector3(-.027085f, .052749f, -.25344f),
                    new Vector3(-.030105f, .052839f, -.265145f),
                    new Vector3(-.05371f, .052829f, -.253445f),
                    new Vector3(-.04302f, .052661f, -.23836f),
                    new Vector3(-.045053f, .052844f, -.268183f),
                    new Vector3(-.043345f, .046975f, .042065f),
                    new Vector3(-.028641f, .047697f, .048672f)
                };                                              //Ankles
            meshSeamVertices[2][0][3][1] = new[]
                {
                    new Vector3(.01765f, .26168f, -.23504f),
                    new Vector3(0, .25412f, -.23691f),
                    new Vector3(.020157f, .283505f, -.224207f),
                    new Vector3(-.020157f, .283505f, -.224207f),
                    new Vector3(-.01765f, .26168f, -.23504f),
                    new Vector3(0, .29452f, -.21842f)
                };                                              //Tail
            meshSeamVertices[2][0][3][2] = new[]
                {
                    new Vector3(.0239f, .36349f, .158141f),
                    new Vector3(.05495f, .34042f, .133701f),
                    new Vector3(.018205f, .36795f, .152276f),
                    new Vector3(.03167f, .35728f, .111431f),
                    new Vector3(.036552f, .357993f, .145971f),
                    new Vector3(.021477f, .369976f, .128572f),
                    new Vector3(.04701f, .341915f, .117481f),
                    new Vector3(-.0239f, .36349f, .158141f),
                    new Vector3(-.05495f, .34042f, .133701f),
                    new Vector3(-.018205f, .36795f, .152276f),
                    new Vector3(-.03167f, .35728f, .111431f),
                    new Vector3(-.036552f, .357993f, .145971f),
                    new Vector3(-.021477f, .369976f, .128572f),
                    new Vector3(-.04701f, .341915f, .117481f)
                };                                              //Ears
            meshSeamVertices[2][0][3][3] = new[]
                {
                    new Vector3(0, .27073f, .134301f),
                    new Vector3(0, .32871f, .075801f),
                    new Vector3(.042717f, .308972f, .09544601f),
                    new Vector3(.04274501f, .290335f, .115511f),
                    new Vector3(.01993f, .27614f, .129034f),
                    new Vector3(.022412f, .323397f, .08070301f),
                    new Vector3(-.042717f, .308972f, .09544601f),
                    new Vector3(-.04274501f, .290335f, .115511f),
                    new Vector3(-.01993f, .27614f, .129034f),
                    new Vector3(-.022412f, .323397f, .08070301f)
                };                                              //Neck
            meshSeamVertices[2][0][3][4] = new Vector3[0];      //Waist
            meshSeamVertices[2][0][3][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[2][0][3][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[2][1] = new Vector3[4][][];        //Kitten
            meshSeamVertices[2][1][0] = new Vector3[7][];       //Kitten LOD0 seams
            meshSeamVertices[2][1][0][0] = new[]
                {
                    new Vector3(.018497f, .026379f, .002009f),
                    new Vector3(.012387f, .028202f, .010199f),
                    new Vector3(.014981f, .030418f, .020039f),
                    new Vector3(.01866f, .031186f, .023475f),
                    new Vector3(.027665f, .03118f, .023433f),
                    new Vector3(.031687f, .030394f, .019928f),
                    new Vector3(.033337f, .027245f, .00572f),
                    new Vector3(.030188f, .026212f, .001113f),
                    new Vector3(.018861f, .029619f, -.130957f),
                    new Vector3(.014863f, .029804f, -.135698f),
                    new Vector3(.013112f, .02993f, -.140976f),
                    new Vector3(.013336f, .030006f, -.145075f),
                    new Vector3(.014588f, .03012f, -.149169f),
                    new Vector3(.017892f, .030157f, -.153162f),
                    new Vector3(.029672f, .030085f, -.153938f),
                    new Vector3(.033145f, .030043f, -.150467f),
                    new Vector3(.034879f, .029977f, -.146234f),
                    new Vector3(.035254f, .02987f, -.141454f),
                    new Vector3(.033931f, .029635f, -.134763f),
                    new Vector3(.030054f, .029504f, -.129609f),
                    new Vector3(.024389f, .029514f, -.128336f),
                    new Vector3(.023928f, .0301f, -.154478f),
                    new Vector3(.023195f, .031494f, .024832f),
                    new Vector3(.024602f, .02597f, 9.000001E-05f),
                    new Vector3(.012652f, .029413f, .015535f),
                    new Vector3(.033369f, .029321f, .015074f),
                    new Vector3(.033974f, .028346f, .010665f),
                    new Vector3(.014679f, .027247f, .005929f),
                    new Vector3(-.018497f, .026379f, .002009f),
                    new Vector3(-.012387f, .028202f, .010199f),
                    new Vector3(-.014981f, .030418f, .020039f),
                    new Vector3(-.01866f, .031186f, .023475f),
                    new Vector3(-.027665f, .03118f, .023433f),
                    new Vector3(-.031687f, .030394f, .019928f),
                    new Vector3(-.033337f, .027245f, .00572f),
                    new Vector3(-.030188f, .026212f, .001113f),
                    new Vector3(-.018861f, .029619f, -.130957f),
                    new Vector3(-.014863f, .029804f, -.135698f),
                    new Vector3(-.013112f, .02993f, -.140976f),
                    new Vector3(-.013336f, .030006f, -.145075f),
                    new Vector3(-.014588f, .03012f, -.149169f),
                    new Vector3(-.017892f, .030157f, -.153162f),
                    new Vector3(-.029672f, .030085f, -.153938f),
                    new Vector3(-.033145f, .030043f, -.150467f),
                    new Vector3(-.034879f, .029977f, -.146234f),
                    new Vector3(-.035254f, .02987f, -.141454f),
                    new Vector3(-.033931f, .029635f, -.134763f),
                    new Vector3(-.030054f, .029504f, -.129609f),
                    new Vector3(-.024389f, .029514f, -.128336f),
                    new Vector3(-.023928f, .0301f, -.154478f),
                    new Vector3(-.023195f, .031494f, .024832f),
                    new Vector3(-.024602f, .02597f, 9.000001E-05f),
                    new Vector3(-.012652f, .029413f, .015535f),
                    new Vector3(-.033369f, .029321f, .015074f),
                    new Vector3(-.033974f, .028346f, .010665f),
                    new Vector3(-.014679f, .027247f, .005929f)
                };                                              //Ankles
            meshSeamVertices[2][1][0][1] = new[]
                {
                    new Vector3(0, .149463f, -.115957f),
                    new Vector3(0, .129288f, -.128139f),
                    new Vector3(.010423f, .146481f, -.117671f),
                    new Vector3(.00681f, .14851f, -.116407f),
                    new Vector3(.008279f, .133257f, -.127143f),
                    new Vector3(.012724f, .141664f, -.121088f),
                    new Vector3(-.010423f, .146481f, -.117671f),
                    new Vector3(-.00681f, .14851f, -.116407f),
                    new Vector3(-.008279f, .133257f, -.127143f),
                    new Vector3(-.012724f, .141664f, -.121088f)
                };                                              //Tail
            meshSeamVertices[2][1][0][2] = new[]
                {
                    new Vector3(.013742f, .209132f, .067178f),
                    new Vector3(.017547f, .207469f, .067133f),
                    new Vector3(.021947f, .205727f, .062852f),
                    new Vector3(.027174f, .201811f, .058545f),
                    new Vector3(.032633f, .197125f, .056024f),
                    new Vector3(.03535f, .193716f, .056012f),
                    new Vector3(.037451f, .190713f, .053592f),
                    new Vector3(.038267f, .187306f, .05108f),
                    new Vector3(.035437f, .186991f, .04509f),
                    new Vector3(.029585f, .189462f, .039499f),
                    new Vector3(.023855f, .193845f, .036314f),
                    new Vector3(.019417f, .19881f, .038247f),
                    new Vector3(.015631f, .204683f, .043522f),
                    new Vector3(.013788f, .208724f, .050557f),
                    new Vector3(.013337f, .210111f, .059452f),
                    new Vector3(-.013742f, .209132f, .067178f),
                    new Vector3(-.017547f, .207469f, .067133f),
                    new Vector3(-.013337f, .210111f, .059452f),
                    new Vector3(-.013788f, .208724f, .050557f),
                    new Vector3(-.015631f, .204683f, .043522f),
                    new Vector3(-.019417f, .19881f, .038247f),
                    new Vector3(-.023855f, .193845f, .036314f),
                    new Vector3(-.029585f, .189462f, .039499f),
                    new Vector3(-.035437f, .186991f, .04509f),
                    new Vector3(-.038267f, .187306f, .05108f),
                    new Vector3(-.037451f, .190713f, .053592f),
                    new Vector3(-.03535f, .193716f, .056012f),
                    new Vector3(-.032633f, .197125f, .056024f),
                    new Vector3(-.027174f, .201811f, .058545f),
                    new Vector3(-.021947f, .205727f, .062852f)
                };                                              //Ears
            meshSeamVertices[2][1][0][3] = new[]
                {
                    new Vector3(0, .142284f, .059373f),
                    new Vector3(0, .176906f, .019761f),
                    new Vector3(.025839f, .16723f, .029918f),
                    new Vector3(.028225f, .163564f, .034268f),
                    new Vector3(.029034f, .159363f, .039308f),
                    new Vector3(.02783f, .155405f, .043854f),
                    new Vector3(.024934f, .151424f, .048448f),
                    new Vector3(.020576f, .148134f, .052331f),
                    new Vector3(.015405f, .145635f, .055386f),
                    new Vector3(.010212f, .143867f, .057427f),
                    new Vector3(.004888f, .14275f, .058746f),
                    new Vector3(.006122f, .176382f, .020261f),
                    new Vector3(.012127f, .174964f, .021662f),
                    new Vector3(.017547f, .172861f, .023772f),
                    new Vector3(.022247f, .170202f, .026496f),
                    new Vector3(-.025839f, .16723f, .029918f),
                    new Vector3(-.028225f, .163564f, .034268f),
                    new Vector3(-.029034f, .159363f, .039308f),
                    new Vector3(-.02783f, .155405f, .043854f),
                    new Vector3(-.024934f, .151424f, .048448f),
                    new Vector3(-.020576f, .148134f, .052331f),
                    new Vector3(-.015405f, .145635f, .055386f),
                    new Vector3(-.010212f, .143867f, .057427f),
                    new Vector3(-.004888f, .14275f, .058746f),
                    new Vector3(-.006122f, .176382f, .020261f),
                    new Vector3(-.012127f, .174964f, .021662f),
                    new Vector3(-.017547f, .172861f, .023772f),
                    new Vector3(-.022247f, .170202f, .026496f)
                };                                              //Neck
            meshSeamVertices[2][1][0][4] = new Vector3[0];      //Waist
            meshSeamVertices[2][1][0][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[2][1][0][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[2][1][1] = new Vector3[7][];       //Kitten LOD1 seams
            meshSeamVertices[2][1][1][0] = new[]
                {
                    new Vector3(.018497f, .026379f, .002009f),
                    new Vector3(.014981f, .030418f, .020039f),
                    new Vector3(.01866f, .031186f, .023475f),
                    new Vector3(.02543f, .031337f, .024132f),
                    new Vector3(.031687f, .030394f, .019928f),
                    new Vector3(.033337f, .027245f, .00572f),
                    new Vector3(.030188f, .026212f, .001113f),
                    new Vector3(.018861f, .029619f, -.130957f),
                    new Vector3(.014863f, .029804f, -.135698f),
                    new Vector3(.013224f, .029968f, -.143025f),
                    new Vector3(.017892f, .030157f, -.153162f),
                    new Vector3(.031409f, .030064f, -.152202f),
                    new Vector3(.035066f, .029924f, -.143844f),
                    new Vector3(.031992f, .029569f, -.132186f),
                    new Vector3(.024389f, .029514f, -.128336f),
                    new Vector3(.023928f, .0301f, -.154478f),
                    new Vector3(.024602f, .02597f, 9.000001E-05f),
                    new Vector3(.01252f, .028808f, .012867f),
                    new Vector3(.033671f, .028833f, .01287f),
                    new Vector3(.014679f, .027247f, .005929f),
                    new Vector3(-.018497f, .026379f, .002009f),
                    new Vector3(-.014981f, .030418f, .020039f),
                    new Vector3(-.01866f, .031186f, .023475f),
                    new Vector3(-.02543f, .031337f, .024132f),
                    new Vector3(-.031687f, .030394f, .019928f),
                    new Vector3(-.033337f, .027245f, .00572f),
                    new Vector3(-.030188f, .026212f, .001113f),
                    new Vector3(-.018861f, .029619f, -.130957f),
                    new Vector3(-.014863f, .029804f, -.135698f),
                    new Vector3(-.013224f, .029968f, -.143025f),
                    new Vector3(-.017892f, .030157f, -.153162f),
                    new Vector3(-.031409f, .030064f, -.152202f),
                    new Vector3(-.035066f, .029924f, -.143844f),
                    new Vector3(-.031992f, .029569f, -.132186f),
                    new Vector3(-.024389f, .029514f, -.128336f),
                    new Vector3(-.023928f, .0301f, -.154478f),
                    new Vector3(-.024602f, .02597f, 9.000001E-05f),
                    new Vector3(-.01252f, .028808f, .012867f),
                    new Vector3(-.033671f, .028833f, .01287f),
                    new Vector3(-.014679f, .027247f, .005929f)
                };                                              //Ankles
            meshSeamVertices[2][1][1][1] = new[]
                {
                    new Vector3(0, .149463f, -.115957f),
                    new Vector3(0, .129288f, -.128139f),
                    new Vector3(.008616f, .147496f, -.117039f),
                    new Vector3(.008279f, .133257f, -.127143f),
                    new Vector3(.012724f, .141664f, -.121088f),
                    new Vector3(-.008616f, .147496f, -.117039f),
                    new Vector3(-.008279f, .133257f, -.127143f),
                    new Vector3(-.012724f, .141664f, -.121088f)
                };                                              //Tail
            meshSeamVertices[2][1][1][2] = new[]
                {
                    new Vector3(.013742f, .209132f, .067178f),
                    new Vector3(.017547f, .207469f, .067133f),
                    new Vector3(.024561f, .203769f, .060699f),
                    new Vector3(.032633f, .197125f, .056024f),
                    new Vector3(.03535f, .193716f, .056012f),
                    new Vector3(.037451f, .190713f, .053592f),
                    new Vector3(.038267f, .187306f, .05108f),
                    new Vector3(.032511f, .188226f, .042294f),
                    new Vector3(.023855f, .193845f, .036314f),
                    new Vector3(.017524f, .201747f, .040885f),
                    new Vector3(.013788f, .208724f, .050557f),
                    new Vector3(.013337f, .210111f, .059452f),
                    new Vector3(-.013742f, .209132f, .067178f),
                    new Vector3(-.017547f, .207469f, .067133f),
                    new Vector3(-.024561f, .203769f, .060699f),
                    new Vector3(-.032633f, .197125f, .056024f),
                    new Vector3(-.03535f, .193716f, .056012f),
                    new Vector3(-.037451f, .190713f, .053592f),
                    new Vector3(-.038267f, .187306f, .05108f),
                    new Vector3(-.032511f, .188226f, .042294f),
                    new Vector3(-.023855f, .193845f, .036314f),
                    new Vector3(-.017524f, .201747f, .040885f),
                    new Vector3(-.013788f, .208724f, .050557f),
                    new Vector3(-.013337f, .210111f, .059452f)
                };                                              //Ears
            meshSeamVertices[2][1][1][3] = new[]
                {
                    new Vector3(0, .142284f, .059373f),
                    new Vector3(0, .176906f, .019761f),
                    new Vector3(.024043f, .168716f, .028207f),
                    new Vector3(.028225f, .163564f, .034268f),
                    new Vector3(.028432f, .157384f, .041581f),
                    new Vector3(.024934f, .151424f, .048448f),
                    new Vector3(.01799f, .146885f, .053859f),
                    new Vector3(.00755f, .143309f, .058087f),
                    new Vector3(.009124f, .175673f, .020962f),
                    new Vector3(.017547f, .172861f, .023772f),
                    new Vector3(-.024043f, .168716f, .028207f),
                    new Vector3(-.028225f, .163564f, .034268f),
                    new Vector3(-.028432f, .157384f, .041581f),
                    new Vector3(-.024934f, .151424f, .048448f),
                    new Vector3(-.01799f, .146885f, .053859f),
                    new Vector3(-.00755f, .143309f, .058087f),
                    new Vector3(-.009124f, .175673f, .020962f),
                    new Vector3(-.017547f, .172861f, .023772f)
                };                                              //Neck
            meshSeamVertices[2][1][1][4] = new Vector3[0];      //Waist
            meshSeamVertices[2][1][1][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[2][1][1][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[2][1][2] = new Vector3[7][];       //Kitten LOD2 seams
            meshSeamVertices[2][1][2][0] = new[]
                {
                    new Vector3(.018497f, .026379f, .002009f),
                    new Vector3(.01682f, .030802f, .021757f),
                    new Vector3(.02543f, .031337f, .024132f),
                    new Vector3(.031687f, .030394f, .019928f),
                    new Vector3(.033811f, .028065f, .009403f),
                    new Vector3(.014863f, .029804f, -.135698f),
                    new Vector3(.013224f, .029968f, -.143025f),
                    new Vector3(.01624f, .030138f, -.151166f),
                    new Vector3(.031409f, .030064f, -.152202f),
                    new Vector3(.035066f, .029924f, -.143844f),
                    new Vector3(.031992f, .029569f, -.132186f),
                    new Vector3(.021625f, .029566f, -.129647f),
                    new Vector3(.023928f, .0301f, -.154478f),
                    new Vector3(.027395f, .026091f, .000601f),
                    new Vector3(.012889f, .027994f, .009263f),
                    new Vector3(-.018497f, .026379f, .002009f),
                    new Vector3(-.01682f, .030802f, .021757f),
                    new Vector3(-.02543f, .031337f, .024132f),
                    new Vector3(-.031687f, .030394f, .019928f),
                    new Vector3(-.033811f, .028065f, .009403f),
                    new Vector3(-.014863f, .029804f, -.135698f),
                    new Vector3(-.013224f, .029968f, -.143025f),
                    new Vector3(-.01624f, .030138f, -.151166f),
                    new Vector3(-.031409f, .030064f, -.152202f),
                    new Vector3(-.035066f, .029924f, -.143844f),
                    new Vector3(-.031992f, .029569f, -.132186f),
                    new Vector3(-.021625f, .029566f, -.129647f),
                    new Vector3(-.023928f, .0301f, -.154478f),
                    new Vector3(-.027395f, .026091f, .000601f),
                    new Vector3(-.012889f, .027994f, .009263f)
                };                                              //Ankles
            meshSeamVertices[2][1][2][1] = new[]
                {
                    new Vector3(0, .149463f, -.115957f),
                    new Vector3(0, .129288f, -.128139f),
                    new Vector3(.011052f, .144775f, -.119061f),
                    new Vector3(.008279f, .133257f, -.127143f),
                    new Vector3(-.011052f, .144775f, -.119061f),
                    new Vector3(-.008279f, .133257f, -.127143f)
                };                                              //Tail
            meshSeamVertices[2][1][2][2] = new[]
                {
                    new Vector3(.017547f, .207469f, .067133f),
                    new Vector3(.037451f, .190713f, .053592f),
                    new Vector3(.013539f, .209622f, .063315f),
                    new Vector3(.021636f, .196327f, .03728f),
                    new Vector3(.038267f, .187306f, .05108f),
                    new Vector3(.03535f, .193716f, .056012f),
                    new Vector3(.029904f, .199468f, .057284f),
                    new Vector3(.021947f, .205727f, .06285201f),
                    new Vector3(.01471f, .206704f, .047039f),
                    new Vector3(.032511f, .188226f, .042294f),
                    new Vector3(-.017547f, .207469f, .067133f),
                    new Vector3(-.037451f, .190713f, .053592f),
                    new Vector3(-.013539f, .209622f, .063315f),
                    new Vector3(-.021636f, .196327f, .03728f),
                    new Vector3(-.038267f, .187306f, .05108f),
                    new Vector3(-.03535f, .193716f, .056012f),
                    new Vector3(-.029904f, .199468f, .057284f),
                    new Vector3(-.021947f, .205727f, .06285201f),
                    new Vector3(-.01471f, .206704f, .047039f),
                    new Vector3(-.032511f, .188226f, .042294f)
                };                                              //Ears
            meshSeamVertices[2][1][2][3] = new[]
                {
                    new Vector3(0, .142284f, .059373f),
                    new Vector3(0, .176906f, .019761f),
                    new Vector3(.024043f, .168716f, .028207f),
                    new Vector3(.028225f, .163564f, .034268f),
                    new Vector3(.027186f, .154554f, .045188f),
                    new Vector3(.01322f, .144772f, .056121f),
                    new Vector3(.013405f, .174547f, .022202f),
                    new Vector3(-.024043f, .168716f, .028207f),
                    new Vector3(-.028225f, .163564f, .034268f),
                    new Vector3(-.027186f, .154554f, .045188f),
                    new Vector3(-.01322f, .144772f, .056121f),
                    new Vector3(-.013405f, .174547f, .022202f)
                };                                              //Neck
            meshSeamVertices[2][1][2][4] = new Vector3[0];      //Waist
            meshSeamVertices[2][1][2][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[2][1][2][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[2][1][3] = new Vector3[7][];       //Kitten LOD3 seams
            meshSeamVertices[2][1][3][0] = new[]
                {
                    new Vector3(.01483f, .03008f, .019832f),
                    new Vector3(.02543f, .031337f, .024132f),
                    new Vector3(.031687f, .030394f, .019928f),
                    new Vector3(.033811f, .028065f, .009403f),
                    new Vector3(.014863f, .029804f, -.135698f),
                    new Vector3(.013224f, .029968f, -.143025f),
                    new Vector3(.01624f, .030138f, -.151166f),
                    new Vector3(.035066f, .029924f, -.143844f),
                    new Vector3(.027232f, .029509f, -.128975f),
                    new Vector3(.028032f, .030089f, -.154092f),
                    new Vector3(.027395f, .026091f, .000601f),
                    new Vector3(.018497f, .026379f, .002009f),
                    new Vector3(-.01483f, .03008f, .019832f),
                    new Vector3(-.02543f, .031337f, .024132f),
                    new Vector3(-.031687f, .030394f, .019928f),
                    new Vector3(-.033811f, .028065f, .009403f),
                    new Vector3(-.014863f, .029804f, -.135698f),
                    new Vector3(-.013224f, .029968f, -.143025f),
                    new Vector3(-.01624f, .030138f, -.151166f),
                    new Vector3(-.035066f, .029924f, -.143844f),
                    new Vector3(-.027232f, .029509f, -.128975f),
                    new Vector3(-.028032f, .030089f, -.154092f),
                    new Vector3(-.027395f, .026091f, .000601f),
                    new Vector3(-.018497f, .026379f, .002009f)
                };                                              //Ankles
            meshSeamVertices[2][1][3][1] = new[]
                {
                    new Vector3(0, .149463f, -.115957f),
                    new Vector3(0, .129288f, -.128139f),
                    new Vector3(.011052f, .144775f, -.119061f),
                    new Vector3(.008279001f, .133257f, -.127143f),
                    new Vector3(-.011052f, .144775f, -.119061f),
                    new Vector3(-.008279001f, .133257f, -.127143f)
                };                                              //Tail
            meshSeamVertices[2][1][3][2] = new[]
                {
                    new Vector3(.017547f, .207469f, .067133f),
                    new Vector3(.037451f, .190713f, .053592f),
                    new Vector3(.013539f, .209622f, .063315f),
                    new Vector3(.021636f, .196327f, .03728f),
                    new Vector3(.025944f, .202331f, .059947f),
                    new Vector3(.01471f, .206704f, .047039f),
                    new Vector3(.032511f, .188226f, .042294f),
                    new Vector3(-.017547f, .207469f, .067133f),
                    new Vector3(-.037451f, .190713f, .053592f),
                    new Vector3(-.013539f, .209622f, .063315f),
                    new Vector3(-.021636f, .196327f, .03728f),
                    new Vector3(-.025944f, .202331f, .059947f),
                    new Vector3(-.01471f, .206704f, .047039f),
                    new Vector3(-.032511f, .188226f, .042294f)
                };                                              //Ears
            meshSeamVertices[2][1][3][3] = new[]
                {
                    new Vector3(0, .142284f, .059373f),
                    new Vector3(0, .176906f, .019761f),
                    new Vector3(.026663f, .166916f, .031727f),
                    new Vector3(.027186f, .154554f, .045188f),
                    new Vector3(.01322f, .144773f, .056121f),
                    new Vector3(.013405f, .174547f, .022202f),
                    new Vector3(-.026663f, .166916f, .031727f),
                    new Vector3(-.027186f, .154554f, .045188f),
                    new Vector3(-.01322f, .144773f, .056121f),
                    new Vector3(-.013405f, .174547f, .022202f)
                };                                              //Neck
            meshSeamVertices[2][1][3][4] = new Vector3[0];      //Waist
            meshSeamVertices[2][1][3][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[2][1][3][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[3] = new Vector3[4][][][];         //ageSpecies
            meshSeamVertices[3][0] = new Vector3[4][][];        //Adult LittleDog
            meshSeamVertices[3][0][0] = new Vector3[7][];       //Adult LittleDog LOD0 seams
            meshSeamVertices[3][0][0][0] = new[]
                {
                    new Vector3(.053266f, .05217f, .048082f),
                    new Vector3(.052265f, .052938f, .05303301f),
                    new Vector3(.04917501f, .053503f, .056396f),
                    new Vector3(.043012f, .053388f, .058182f),
                    new Vector3(.036192f, .052977f, .056148f),
                    new Vector3(.032485f, .052619f, .052324f),
                    new Vector3(.031167f, .052014f, .047213f),
                    new Vector3(.031019f, .050789f, .042328f),
                    new Vector3(.033381f, .049271f, .037794f),
                    new Vector3(.038213f, .047961f, .03346501f),
                    new Vector3(.046057f, .047652f, .033513f),
                    new Vector3(.05128f, .049458f, .038353f),
                    new Vector3(.053238f, .050979f, .04322f),
                    new Vector3(-.031168f, .052014f, .047213f),
                    new Vector3(-.032486f, .052619f, .052324f),
                    new Vector3(-.036193f, .052977f, .056148f),
                    new Vector3(-.043012f, .053388f, .058182f),
                    new Vector3(-.04917501f, .053503f, .056396f),
                    new Vector3(-.052266f, .052938f, .05303301f),
                    new Vector3(-.053266f, .05217f, .048082f),
                    new Vector3(-.053238f, .050979f, .04322f),
                    new Vector3(-.051281f, .049458f, .038353f),
                    new Vector3(-.046057f, .047652f, .033513f),
                    new Vector3(-.038213f, .047961f, .03346501f),
                    new Vector3(-.033382f, .049271f, .037794f),
                    new Vector3(-.031019f, .050789f, .042328f),
                    new Vector3(.042106f, .047806f, .032653f),
                    new Vector3(-.042107f, .047806f, .032653f),
                    new Vector3(.043172f, .04736301f, -.243378f),
                    new Vector3(.043527f, .047413f, -.248714f),
                    new Vector3(.042082f, .047456f, -.253793f),
                    new Vector3(.039837f, .0475f, -.257573f),
                    new Vector3(.033562f, .047528f, -.258435f),
                    new Vector3(.026935f, .047482f, -.256842f),
                    new Vector3(.024721f, .047435f, -.253338f),
                    new Vector3(.023628f, .047404f, -.248632f),
                    new Vector3(.023723f, .047364f, -.243251f),
                    new Vector3(.025867f, .04733f, -.237971f),
                    new Vector3(.02946f, .047351f, -.235291f),
                    new Vector3(.033971f, .047386f, -.234519f),
                    new Vector3(.038166f, .047399f, -.234816f),
                    new Vector3(.041217f, .047377f, -.238406f),
                    new Vector3(-.026935f, .047482f, -.256842f),
                    new Vector3(-.024721f, .047435f, -.253338f),
                    new Vector3(-.023628f, .047404f, -.248632f),
                    new Vector3(-.023723f, .047364f, -.243251f),
                    new Vector3(-.025867f, .04733f, -.237971f),
                    new Vector3(-.02946f, .047351f, -.235291f),
                    new Vector3(-.033971f, .047386f, -.234519f),
                    new Vector3(-.038166f, .047399f, -.234816f),
                    new Vector3(-.041217f, .047377f, -.238406f),
                    new Vector3(-.043172f, .04736301f, -.243378f),
                    new Vector3(-.043527f, .047413f, -.248714f),
                    new Vector3(-.042082f, .047456f, -.253793f),
                    new Vector3(-.039837f, .0475f, -.257573f),
                    new Vector3(-.033562f, .047528f, -.258435f)
                };                                              //Ankles
            meshSeamVertices[3][0][0][1] = new[]
                {
                    new Vector3(.01081f, .278458f, -.210725f),
                    new Vector3(.010701f, .286293f, -.205004f),
                    new Vector3(.006487f, .290322f, -.203094f),
                    new Vector3(0, .292468f, -.202558f),
                    new Vector3(0, .270991f, -.21788f),
                    new Vector3(.007394f, .27454f, -.214627f),
                    new Vector3(-.01081f, .278458f, -.210725f),
                    new Vector3(-.010701f, .286293f, -.205005f),
                    new Vector3(-.006487f, .290322f, -.203094f),
                    new Vector3(-.007394f, .27454f, -.214627f)
                };                                              //Tail
            meshSeamVertices[3][0][0][2] = new[]
                {
                    new Vector3(-.037003f, .357684f, .114917f),
                    new Vector3(-.035077f, .363509f, .118049f),
                    new Vector3(-.037946f, .35381f, .108325f),
                    new Vector3(-.037114f, .353302f, .100102f),
                    new Vector3(-.034529f, .358795f, .09375401f),
                    new Vector3(-.032545f, .364857f, .093388f),
                    new Vector3(-.029207f, .374027f, .097527f),
                    new Vector3(-.032384f, .370868f, .117098f),
                    new Vector3(-.02986f, .376394f, .113495f),
                    new Vector3(-.037995f, .355409f, .112079f),
                    new Vector3(-.033635f, .36704f, .118419f),
                    new Vector3(-.031054f, .373833f, .11554f),
                    new Vector3(-.028377f, .378899f, .107687f),
                    new Vector3(-.028461f, .376499f, .101057f),
                    new Vector3(-.030227f, .370931f, .095279f),
                    new Vector3(-.03758f, .353438f, .104691f),
                    new Vector3(.037003f, .357684f, .114917f),
                    new Vector3(.035077f, .363509f, .118049f),
                    new Vector3(.037946f, .35381f, .108325f),
                    new Vector3(.037114f, .353302f, .100102f),
                    new Vector3(.034529f, .358795f, .093755f),
                    new Vector3(.032545f, .364857f, .093388f),
                    new Vector3(.029207f, .374027f, .097528f),
                    new Vector3(.032383f, .370868f, .117098f),
                    new Vector3(.02986f, .376394f, .113495f),
                    new Vector3(.037995f, .355409f, .11208f),
                    new Vector3(.033635f, .36704f, .118419f),
                    new Vector3(.031054f, .373833f, .11554f),
                    new Vector3(.028377f, .378899f, .107688f),
                    new Vector3(.028461f, .376499f, .101057f),
                    new Vector3(.030227f, .370931f, .09528001f),
                    new Vector3(.03758f, .353438f, .104691f)
                };                                              //Ears
            meshSeamVertices[3][0][0][3] = new[]
                {
                    new Vector3(0, .295948f, .115474f),
                    new Vector3(-.017193f, .303061f, .109489f),
                    new Vector3(-.022039f, .307239f, .105983f),
                    new Vector3(-.030306f, .322173f, .093075f),
                    new Vector3(-.029397f, .332197f, .084304f),
                    new Vector3(-.022961f, .341648f, .075985f),
                    new Vector3(-.018291f, .345885f, .07224901f),
                    new Vector3(-.006312001f, .350373f, .068202f),
                    new Vector3(0, .351544f, .067248f),
                    new Vector3(-.006851f, .297333f, .114314f),
                    new Vector3(-.028882f, .316425f, .098026f),
                    new Vector3(-.012603f, .34862f, .069776f),
                    new Vector3(-.02631f, .336917f, .080184f),
                    new Vector3(.017193f, .303061f, .109489f),
                    new Vector3(.022039f, .307239f, .105983f),
                    new Vector3(.030306f, .322173f, .093075f),
                    new Vector3(.029397f, .332197f, .084304f),
                    new Vector3(.022961f, .341648f, .075985f),
                    new Vector3(.018291f, .345885f, .07224901f),
                    new Vector3(.006312001f, .350373f, .068202f),
                    new Vector3(.006851f, .297333f, .114314f),
                    new Vector3(.028882f, .316425f, .098026f),
                    new Vector3(.012603f, .34862f, .069776f),
                    new Vector3(.02631f, .336917f, .080184f),
                    new Vector3(.03084f, .3272f, .088763f),
                    new Vector3(.025984f, .311629f, .1024f),
                    new Vector3(.012586f, .299943f, .112173f),
                    new Vector3(-.03084f, .3272f, .088763f),
                    new Vector3(-.025984f, .311629f, .1024f),
                    new Vector3(-.012586f, .299943f, .112173f)
                };                                              //Neck
            meshSeamVertices[3][0][0][4] = new Vector3[0];      //Waist
            meshSeamVertices[3][0][0][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[3][0][0][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[3][0][1] = new Vector3[7][];       //Adult LittleDog LOD1 seams
            meshSeamVertices[3][0][1][0] = new[]
                {
                    new Vector3(.052266f, .052938f, .053031f),
                    new Vector3(.04917501f, .053503f, .056394f),
                    new Vector3(.043012f, .05338901f, .058181f),
                    new Vector3(.036192f, .052977f, .056146f),
                    new Vector3(.032486f, .05262f, .052323f),
                    new Vector3(.031093f, .051402f, .044769f),
                    new Vector3(.033381f, .049271f, .037793f),
                    new Vector3(.038213f, .047962f, .033463f),
                    new Vector3(.046057f, .047652f, .033511f),
                    new Vector3(.05128f, .049459f, .038351f),
                    new Vector3(.053252f, .051575f, .045649f),
                    new Vector3(-.032486f, .05262f, .052323f),
                    new Vector3(-.036192f, .052977f, .056146f),
                    new Vector3(-.043012f, .05338901f, .058181f),
                    new Vector3(-.04917501f, .053503f, .056394f),
                    new Vector3(-.052266f, .052938f, .053031f),
                    new Vector3(-.053252f, .051575f, .045649f),
                    new Vector3(-.05128f, .049459f, .038351f),
                    new Vector3(-.046057f, .047652f, .033511f),
                    new Vector3(-.038213f, .047962f, .033463f),
                    new Vector3(-.033381f, .049271f, .037793f),
                    new Vector3(-.031093f, .051402f, .044769f),
                    new Vector3(.042107f, .047807f, .032652f),
                    new Vector3(-.042107f, .047807f, .032652f),
                    new Vector3(.041092f, .051627f, -.237833f),
                    new Vector3(.043291f, .052228f, -.245455f),
                    new Vector3(.041029f, .052701f, -.255115f),
                    new Vector3(.033536f, .052772f, -.258351f),
                    new Vector3(.025744f, .052608f, -.254548f),
                    new Vector3(.023714f, .052139f, -.24501f),
                    new Vector3(.026088f, .051489f, -.237482f),
                    new Vector3(.02968f, .051407f, -.234561f),
                    new Vector3(.03399801f, .051474f, -.233948f),
                    new Vector3(.037926f, .051507f, -.234069f),
                    new Vector3(-.025744f, .052608f, -.254548f),
                    new Vector3(-.023714f, .052139f, -.24501f),
                    new Vector3(-.026088f, .051489f, -.237482f),
                    new Vector3(-.02968f, .051407f, -.234561f),
                    new Vector3(-.03399801f, .051474f, -.233948f),
                    new Vector3(-.037926f, .051507f, -.234069f),
                    new Vector3(-.041029f, .052701f, -.255115f),
                    new Vector3(-.033536f, .052772f, -.258351f),
                    new Vector3(-.041092f, .051627f, -.237833f),
                    new Vector3(-.043291f, .052228f, -.245455f)
                };                                              //Ankles
            meshSeamVertices[3][0][1][1] = new[]
                {
                    new Vector3(.01081f, .278458f, -.210725f),
                    new Vector3(.008594f, .288307f, -.204049f),
                    new Vector3(0, .292468f, -.202558f),
                    new Vector3(0, .270991f, -.21788f),
                    new Vector3(.007394f, .27454f, -.214627f),
                    new Vector3(-.01081f, .278458f, -.210725f),
                    new Vector3(-.008594f, .288307f, -.20405f),
                    new Vector3(-.007394f, .27454f, -.214627f)
                };                                              //Tail
            meshSeamVertices[3][0][1][2] = new[]
                {
                    new Vector3(-.037003f, .357684f, .114915f),
                    new Vector3(-.037114f, .353302f, .1001f),
                    new Vector3(-.034529f, .358794f, .093752f),
                    new Vector3(-.032545f, .364857f, .09338601f),
                    new Vector3(-.029207f, .374026f, .097525f),
                    new Vector3(-.032383f, .370868f, .117096f),
                    new Vector3(-.02986f, .376394f, .113493f),
                    new Vector3(-.03797f, .354609f, .1102f),
                    new Vector3(-.033635f, .36704f, .118417f),
                    new Vector3(-.031054f, .373832f, .115538f),
                    new Vector3(-.028419f, .377698f, .10437f),
                    new Vector3(-.030226f, .37093f, .095277f),
                    new Vector3(-.03758f, .353438f, .104689f),
                    new Vector3(.037003f, .357684f, .114915f),
                    new Vector3(.03797f, .354609f, .1102f),
                    new Vector3(.037114f, .353302f, .1001f),
                    new Vector3(.034529f, .358794f, .093753f),
                    new Vector3(.032545f, .364857f, .09338601f),
                    new Vector3(.029207f, .374026f, .097526f),
                    new Vector3(.032383f, .370868f, .117096f),
                    new Vector3(.02986f, .376394f, .113493f),
                    new Vector3(.033635f, .36704f, .118417f),
                    new Vector3(.031054f, .373832f, .115538f),
                    new Vector3(.028419f, .377698f, .10437f),
                    new Vector3(.030226f, .37093f, .09527801f),
                    new Vector3(.03758f, .353438f, .104689f)
                };                                              //Ears
            meshSeamVertices[3][0][1][3] = new[]
                {
                    new Vector3(0, .295948f, .115472f),
                    new Vector3(-.017193f, .303061f, .109487f),
                    new Vector3(-.029397f, .332197f, .08430301f),
                    new Vector3(-.024635f, .339282f, .078083f),
                    new Vector3(-.018291f, .345885f, .072248f),
                    new Vector3(-.009458f, .349496f, .06898801f),
                    new Vector3(0, .351544f, .067247f),
                    new Vector3(-.028882f, .316425f, .098024f),
                    new Vector3(.017193f, .303061f, .109487f),
                    new Vector3(.024012f, .309434f, .10419f),
                    new Vector3(.030573f, .324687f, .09091701f),
                    new Vector3(.029397f, .332197f, .08430301f),
                    new Vector3(.018291f, .345885f, .072248f),
                    new Vector3(.009719001f, .298638f, .113242f),
                    new Vector3(.028882f, .316425f, .098024f),
                    new Vector3(.009458f, .349496f, .06898801f),
                    new Vector3(.024635f, .339282f, .078083f),
                    new Vector3(-.030573f, .324687f, .09091701f),
                    new Vector3(-.024012f, .309434f, .10419f),
                    new Vector3(-.009719001f, .298638f, .113242f)
                };                                              //Neck
            meshSeamVertices[3][0][1][4] = new Vector3[0];      //Waist
            meshSeamVertices[3][0][1][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[3][0][1][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[3][0][2] = new Vector3[7][];       //Adult LittleDog LOD2 seams
            meshSeamVertices[3][0][2][0] = new[]
                {
                    new Vector3(.053266f, .05217f, .04808f),
                    new Vector3(.049175f, .053503f, .056394f),
                    new Vector3(.036192f, .052977f, .056146f),
                    new Vector3(.031168f, .052014f, .047211f),
                    new Vector3(.033381f, .049271f, .037793f),
                    new Vector3(.05128f, .049459f, .038351f),
                    new Vector3(-.031168f, .052014f, .047211f),
                    new Vector3(-.036192f, .052977f, .056146f),
                    new Vector3(-.049175f, .053503f, .056394f),
                    new Vector3(-.053266f, .05217f, .04808f),
                    new Vector3(-.05128f, .049459f, .038351f),
                    new Vector3(-.033381f, .049271f, .037793f),
                    new Vector3(.042107f, .047807f, .032652f),
                    new Vector3(-.042107f, .047807f, .032652f),
                    new Vector3(.043424f, .066587f, -.240959f),
                    new Vector3(.042368f, .068555f, -.252335f),
                    new Vector3(.03344f, .069764f, -.258f),
                    new Vector3(.023818f, .06823601f, -.25111f),
                    new Vector3(.023287f, .066453f, -.240506f),
                    new Vector3(.029835f, .065184f, -.231667f),
                    new Vector3(.038287f, .065391f, -.231218f),
                    new Vector3(-.023818f, .06823601f, -.25111f),
                    new Vector3(-.023287f, .066453f, -.240506f),
                    new Vector3(-.029835f, .065184f, -.231667f),
                    new Vector3(-.038287f, .065391f, -.231218f),
                    new Vector3(-.043424f, .066587f, -.240959f),
                    new Vector3(-.042368f, .068555f, -.252335f),
                    new Vector3(-.03344f, .069764f, -.258f)
                };                                              //Ankles
            meshSeamVertices[3][0][2][1] = new[]
                {
                    new Vector3(0, .292468f, -.202558f),
                    new Vector3(0, .270991f, -.21788f),
                    new Vector3(.009682f, .287267f, -.204543f),
                    new Vector3(-.009682f, .287267f, -.204543f)
                };                                              //Tail
            meshSeamVertices[3][0][2][2] = new[]
                {
                    new Vector3(-.037003f, .357684f, .114915f),
                    new Vector3(-.035077f, .363509f, .118047f),
                    new Vector3(-.037946f, .353809f, .108323f),
                    new Vector3(-.037114f, .353302f, .1001f),
                    new Vector3(-.032545f, .364857f, .093386f),
                    new Vector3(-.02986f, .376394f, .113493f),
                    new Vector3(-.031718f, .37235f, .116317f),
                    new Vector3(-.028419f, .377698f, .10437f),
                    new Vector3(-.029717f, .372478f, .096401f),
                    new Vector3(.037003f, .357684f, .114915f),
                    new Vector3(.035077f, .363509f, .118047f),
                    new Vector3(.037946f, .353809f, .108323f),
                    new Vector3(.037114f, .353302f, .1001f),
                    new Vector3(.032545f, .364857f, .093386f),
                    new Vector3(.029717f, .372478f, .096402f),
                    new Vector3(.031718f, .37235f, .116317f),
                    new Vector3(.02986f, .376394f, .113493f),
                    new Vector3(.028419f, .377698f, .10437f)
                };                                              //Ears
            meshSeamVertices[3][0][2][3] = new[]
                {
                    new Vector3(0, .295948f, .115472f),
                    new Vector3(-.022039f, .307239f, .105982f),
                    new Vector3(-.030306f, .322173f, .093073f),
                    new Vector3(-.029397f, .332197f, .08430301f),
                    new Vector3(-.022961f, .341648f, .075984f),
                    new Vector3(0, .351544f, .067247f),
                    new Vector3(-.012603f, .348619f, .069774f),
                    new Vector3(.022039f, .307239f, .105982f),
                    new Vector3(.030306f, .322173f, .093073f),
                    new Vector3(.029397f, .332197f, .08430301f),
                    new Vector3(.022961f, .341648f, .075984f),
                    new Vector3(.012603f, .348619f, .069774f),
                    new Vector3(.012586f, .299942f, .112171f),
                    new Vector3(-.012586f, .299942f, .112171f)
                };                                              //Neck
            meshSeamVertices[3][0][2][4] = new Vector3[0];      //Waist
            meshSeamVertices[3][0][2][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[3][0][2][6] = new Vector3[0];      //WaistAdultMale
            meshSeamVertices[3][0][3] = new Vector3[7][];       //Adult LittleDog LOD3 seams
            meshSeamVertices[3][0][3][0] = new[]
                {
                    new Vector3(.049175f, .053503f, .056394f),
                    new Vector3(.036192f, .052977f, .056146f),
                    new Vector3(.033381f, .049271f, .037793f),
                    new Vector3(.05128f, .049459f, .038351f),
                    new Vector3(-.036192f, .052977f, .056146f),
                    new Vector3(-.049175f, .053503f, .056394f),
                    new Vector3(-.05128f, .049459f, .038351f),
                    new Vector3(-.033381f, .049271f, .037793f),
                    new Vector3(.042107f, .047807f, .032652f),
                    new Vector3(-.042107f, .047807f, .032652f),
                    new Vector3(.042368f, .068555f, -.252335f),
                    new Vector3(.03344f, .069764f, -.258f),
                    new Vector3(.023818f, .06823601f, -.25111f),
                    new Vector3(.029835f, .065184f, -.231667f),
                    new Vector3(.038287f, .065391f, -.231218f),
                    new Vector3(-.023818f, .06823601f, -.25111f),
                    new Vector3(-.029835f, .065184f, -.231667f),
                    new Vector3(-.038287f, .065391f, -.231218f),
                    new Vector3(-.042368f, .068555f, -.252335f),
                    new Vector3(-.03344f, .069764f, -.258f)
                };                                              //Ankles
            meshSeamVertices[3][0][3][1] = new[]
                {
                    new Vector3(0, .292468f, -.202558f),
                    new Vector3(0, .270991f, -.21788f),
                    new Vector3(.010432f, .28655f, -.204883f),
                    new Vector3(-.010433f, .286549f, -.204883f)
                };                                              //Tail
            meshSeamVertices[3][0][3][2] = new[]
                {
                    new Vector3(-.03604f, .360596f, .116481f),
                    new Vector3(-.03751f, .353353f, .104223f),
                    new Vector3(-.032545f, .364857f, .093386f),
                    new Vector3(-.030711f, .374507f, .115047f),
                    new Vector3(-.028628f, .375944f, .100264f),
                    new Vector3(.03604f, .360596f, .116481f),
                    new Vector3(.03751f, .353353f, .104223f),
                    new Vector3(.032545f, .364857f, .093386f),
                    new Vector3(.030711f, .374507f, .115047f),
                    new Vector3(.028628f, .375944f, .100264f)
                };                                              //Ears
            meshSeamVertices[3][0][3][3] = new[]
                {
                    new Vector3(0, .295948f, .115472f),
                    new Vector3(-.017586f, .3034f, .109202f),
                    new Vector3(-.030831f, .327118f, .08883201f),
                    new Vector3(0, .351544f, .067247f),
                    new Vector3(-.018422f, .345765f, .072353f),
                    new Vector3(.030831f, .327118f, .08883201f),
                    new Vector3(.018422f, .345765f, .072353f),
                    new Vector3(.017586f, .3034f, .109202f)
                };                                              //Neck
            meshSeamVertices[3][0][3][4] = new Vector3[0];      //Waist
            meshSeamVertices[3][0][3][5] = new Vector3[0];      //WaistAdultFemale
            meshSeamVertices[3][0][3][6] = new Vector3[0];      //WaistAdultMale
            return meshSeamVertices;
        }

        public void SetUV(int vertexSequenceNumber, int uvSet, float[] newUV)
        {
            mUVs[uvSet][vertexSequenceNumber] = new UV(newUV[0], newUV[1]);
        }

        public void SetUV(int vertexSequenceNumber, int uvSet, float u, float v)
        {
            mUVs[uvSet][vertexSequenceNumber] = new UV(u, v);
        }

        public void SetVersion(int newVersion)
        {
            if (newVersion == 5 && mVersion > 5)
            {
                for (var i = 0; i < mFaceCount; i++)
                {
                    if (mVertexFormats[i].FormatDataType == 5)
                    {
                        mVertexFormats[i].FormatSubType = 1;
                        mVertexFormats[i].FormatDataLength = 16;
                    }
                }
            }
            if (newVersion >= 12 & mVersion == 5)
            {
                for (var i = 0; i < mFaceCount; i++)
                {
                    if (mVertexFormats[i].FormatDataType == 5)
                    {
                        mVertexFormats[i].FormatSubType = 2;
                        mVertexFormats[i].FormatDataLength = 4;
                    }
                }
            }
            mVersion = newVersion;
        }

        public void SetVertexID(int vertexSequenceNumber, int newVertexID)
        {
            mVertexIDs[vertexSequenceNumber] = newVertexID;
        }

        public Vector3[] SlotrayTrianglePositions(int slotrayAdjustmentIndex)
        {
            var vertexIndices = SlotrayAdjustments[slotrayAdjustmentIndex].TrianglePointIndices;
            return new[]
            {
                new Vector3(mPositions[vertexIndices[0]].Coordinates), 
                new Vector3(mPositions[vertexIndices[1]].Coordinates),
                new Vector3(mPositions[vertexIndices[2]].Coordinates)
            };
        }

        public void SnapVertices()
        {
            for (var i = 0; i < mVertexCount - 1; i++)
            {
                Vector3 normal = new Vector3(GetNormal(i)),
                position = new Vector3(GetPosition(i));
                for (var j = i + 1; j < mVertexCount; j++)
                {
                    if (position.PositionMatches(GetPosition(j)))
                    {
                        SetPosition(j, position.Coordinates);
                        if (normal.PositionMatches(GetNormal(j)))
                        {
                            SetNormal(j, normal.Coordinates);
                        }
                    }
                }
            }
        }

        public void SnapVerticesToHead(GEOM head)
        {
            for (var i = 0; i < mVertexCount; i++)
            {
                var position = new Vector3(GetPosition(i));
                for (var j = 0; j < head.mVertexCount; j++)
                {
                    if (position.PositionClose(head.GetPosition(j)))
                    {
                        SetPosition(i, head.GetPosition(j));
                        SetNormal(i, head.GetNormal(j));
                        SetTagValue(i, GetTagValue(i) & 0xFFFFFF00 | 0x00000063);
                    }
                }
            }
        }

        public void SortBones(ref List<byte> newBones, ref List<float> newWeights)
        {
            for (var i = newBones.Count - 1; i > 0; i--)
            {
                for (var j = 0; j < i; j++)
                {
                    if (newWeights[j] < newWeights[j + 1])
                    {
                        var tempBones = newBones[j];
                        newBones[j] = newBones[j + 1];
                        newBones[j + 1] = tempBones;
                        var tempWeights = newWeights[j];
                        newWeights[j] = newWeights[j + 1];
                        newWeights[j + 1] = tempWeights;
                    }
                }
            }
        }

        public void StandardizeFormat()
        {
            if (mVersion < 14)
            {
                mVersion = 14;
            }
            if (mSeamStitches == null)
            {
                mSeamStitches = new SeamStitch[0];
            }
            if (mUVStitches == null)
            {
                mUVStitches = new UVStitch[0];
            }
            if (SlotrayAdjustments == null)
            {
                SlotrayAdjustments = new SlotrayIntersection[0];
            }
            if (mBoneHashArray == null)
            {
                mBoneHashArray = new uint[0];
            }
            if (mTGIs == null)
            {
                mTGIs = new TGI[0];
            }
            if (!HasUVSet(1))
            {
                InsertFormatDescriptor(MeshFormatFlag.UV);
                var temp = new UV[mPositions.Length];
                for (var i = 0; i < temp.Length; i++)
                {
                    temp[i] = new UV();
                }
                var newUV = new UV[2][];
                newUV[0] = mUVs[0];
                newUV[1] = temp;
                mUVs = newUV;
            }
            if (HasUVSet(2))
            {
                var newFormat = new VertexFormat[VertexFormatList.Length - 1];
                var uv = Array.LastIndexOf(VertexFormatList, (int)MeshFormatFlag.UV);
                Array.Copy(mVertexFormats, newFormat, uv);
                Array.Copy(mVertexFormats, uv + 1, newFormat, uv, mVertexFormats.Length - uv - 1);
                mVertexFormats = newFormat;
                mFaceCount--;
                var newUV = new UV[2][];
                newUV[0] = mUVs[0];
                newUV[1] = mUVs[1];
                mUVs = newUV;
            }
            if (!HasNormals)
            {
                InsertFormatDescriptor(MeshFormatFlag.Normals);
                mNormals = new Normal[mPositions.Length];
                for (var i = 0; i < mNormals.Length; i++)
                {
                    mNormals[i] = new Normal();
                }
            }
            if (!HasBones)
            {
                InsertFormatDescriptor(MeshFormatFlag.BoneIndices);
                InsertFormatDescriptor(MeshFormatFlag.BoneWeights);
                mBones = new Bones[mPositions.Length];
                for (var i = 0; i < mBones.Length; i++)
                {
                    mBones[i] = new Bones();
                }
            }
            if (!HasTangents)
            {
                InsertFormatDescriptor(MeshFormatFlag.Tangents);
                mTangents = new Tangent[mPositions.Length];
                for (var i = 0; i < mTangents.Length; i++)
                {
                    mTangents[i] = new Tangent();
                }
            }
            if (!HasTags)
            {
                InsertFormatDescriptor(MeshFormatFlag.Color);
                mTags = new TagValue[mPositions.Length];
                for (var i = 0; i < mTags.Length; i++)
                {
                    mTags[i] = new TagValue();
                }
            }
            if (!HasVertexIDs)
            {
                InsertFormatDescriptor(MeshFormatFlag.VertexID);
                mVertexIDs = new int[mPositions.Length];
                for (var i = 0; i < mVertexIDs.Length; i++)
                {
                    mVertexIDs[i] = -1;
                }
            }
        }

        public static bool TranslateBone(uint boneHash, uint[] boneHashArray, out byte bone)
        {
            for (var i = 0; i < boneHashArray.Length; i++)
            {
                if (boneHash == boneHashArray[i])
                {
                    bone = (byte)i;
                    return true;
                }
            }
            bone = 0;
            return false;
        }

        public void UpdatePositions()
        {
            for (var i = 0; i < mPositions.Length; i++)
            {
                mPositions[i] = new Position((new Vector3(GetPosition(i)) + DeltaPosition[i]).Coordinates);
            }
        }

        public bool UpdateToLatestVersion()
        {
            if (mVersion < 13)
            {
                SetVersion(13);
                return true;
            }
            return false;
        }

        public bool ValidBones(int vertexSequenceNumber)
        {
            var vertexBones = GetBones(vertexSequenceNumber);
            var vertexWeights = GetBoneWeightsV5(vertexSequenceNumber);
            if (mVersion == 5)
            {
                for (var i = 0; i < 4; i++)
                {
                    if (vertexWeights[i] > 0 && (vertexBones[i] < 0 || vertexBones[i] >= BoneCount))
                    {
                        return false;
                    }
                }
            }
            else if (mVersion >= 12)
            {
                for (var i = 0; i < 4; i++)
                {
                    if (vertexBones[i] < 0 || vertexBones[i] >= BoneCount)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public string VertexDataString(int vertexSequenceNumber)
        {
            var vertexFormatList = VertexFormatList;
            string separator = " | ",
            text = "";
            var uvIndex = 0;
            for (var i = 0; i < vertexFormatList.Length; i++)
            {
                switch (vertexFormatList[i])
                {
                    case 1:
                        text += mPositions[vertexSequenceNumber].ToString() + separator;
                        break;
                    case 2:
                        text += mNormals[vertexSequenceNumber].ToString() + separator;
                        break;
                    case 3:
                        text += mUVs[uvIndex][vertexSequenceNumber].ToString() + separator;
                        uvIndex += 1;
                        break;
                    case 4:
                        text += mBones[vertexSequenceNumber].ToString() + separator;
                        break;
                    case 6:
                        text += mTangents[vertexSequenceNumber].ToString() + separator;
                        break;
                    case 7:
                        text += mTags[vertexSequenceNumber].ToString() + separator;
                        break;
                    case 10:
                        text += mVertexIDs[vertexSequenceNumber].ToString() + separator;
                        break;
                }
            }
            return text.Remove(text.LastIndexOf(separator));
        }

        public bool VertexFormatEquals(int[] testFormatList)
        {
            var temp = VertexFormatList;
            if (temp.Length != testFormatList.Length)
            {
                return false;
            }
            for (var i = 0; i < temp.Length; i++)
            {
                if (temp[i] != testFormatList[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int VertexIDSearch(uint vertexID)
        {
            return Array.IndexOf(mVertexIDs, vertexID);
        }

        public void Write(BinaryWriter writer)
        {
            var temp = 0;
            if (mMTNF != null)
            {
                mMTNFSize = mMTNF.ChunkSize;
            }
            else
            {
                mMTNFSize = 0;
            }
            if (mShaderHash != 0)
            {
                temp = mMTNFSize + 4;
            }
            mTGIOffset = 37 + (mFaceCount * 9) + temp + (mVertexCount * VertexDataLength) + (mFacePointCount * mBytesPerFacePoint) + (mBoneHashCount * 4);
            if (mVersion == 5)
            {
                mTGIOffset += 4;
            }
            else if (mVersion >= 12)
            {
                mTGIOffset += 8 + UVStitchesSize + SlotrayAdjustmentsSize;
                if (mVersion >= 13)
                {
                    mTGIOffset += 4 + (mSeamStitchCount * 6);
                }
            }
            mMeshSize = mTGIOffset + 16 + mTGICount * 16;
            mTGISize = 4 + mTGICount * 16;
            writer.Write(mVersion1);
            writer.Write(mCount);
            writer.Write(mIndexCount);
            writer.Write(mExternalCount);
            writer.Write(mInternalCount);
            mDummyTGI.Write(writer);
            writer.Write(mAbsolutePosition);
            writer.Write(mMeshSize);
            writer.Write(mMagic);
            writer.Write(mVersion);
            writer.Write(mTGIOffset);
            writer.Write(mTGISize);
            writer.Write(mShaderHash);
            if (mShaderHash != 0)
            {
                writer.Write(mMTNFSize);
                mMTNF.Write(writer);
            }
            writer.Write(mMergeGroup);
            writer.Write(mSortOrder);
            writer.Write(mVertexCount);
            writer.Write(mFaceCount);
            for (var i = 0; i < mFaceCount; i++)
            {
                mVertexFormats[i].Write(writer);
            }
            for (var i = 0; i < mVertexCount; i++)
            {
                var uvIndex = 0;
                for (var j = 0; j < mFaceCount; j++)
                {
                    switch (mVertexFormats[j].FormatDataType)
                    {
                        case 1:
                            mPositions[i].Write(writer);
                            break;
                        case 2:
                            mNormals[i].Write(writer);
                            break;
                        case 3:
                            mUVs[uvIndex][i].Write(writer);
                            uvIndex += 1;
                            break;
                        case 4:
                            mBones[i].WriteAssignments(writer, mVersion, mBoneHashCount - 1);
                            break;
                        case 5:
                            mBones[i].WriteWeights(writer, mVersion);
                            break;
                        case 6:
                            mTangents[i].Write(writer);
                            break;
                        case 7:
                            mTags[i].Write(writer);
                            break;
                        case 10:
                            if (mVertexIDs != null)
                            {
                                writer.Write(mVertexIDs[i]);
                            }
                            break;
                    }
                }
            }
            writer.Write(mSubMeshCount);
            writer.Write(mBytesPerFacePoint);
            writer.Write(mFaces.Length * 3);
            for (var i = 0; i < mFaces.Length; i++)
            {
                mFaces[i].Write(writer, mBytesPerFacePoint);
            }
            if (mVersion == 5)
            {
                writer.Write(mSKCONIndex);
            }
            else if (mVersion >= 12)
            {
                writer.Write(mUVStitchCount);
                for (var i = 0; i < mUVStitchCount; i++)
                {
                    mUVStitches[i].Write(writer);
                }
                if (mVersion >= 13)
                {
                    writer.Write(mSeamStitchCount);
                    for (var i = 0; i < mSeamStitchCount; i++)
                    {
                        mSeamStitches[i].Write(writer);
                    }
                }
                writer.Write(mSlotCount);
                for (var i = 0; i < mSlotCount; i++)
                {
                    mSlotrayIntersections[i].Write(writer);
                }
            }
            writer.Write(mBoneHashCount);
            for (var i = 0; i < mBoneHashCount; i++)
            {
                writer.Write(mBoneHashArray[i]);
            }
            writer.Write(mTGICount);
            for (var i = 0; i < mTGICount; i++)
            {
                mTGIs[i].Write(writer);
            }
        }
    }
}
