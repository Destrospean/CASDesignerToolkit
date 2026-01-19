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
    public class VPXY
    {
        float[] mBoundingBox;

        byte mBoxType, mCount, mFlag;

        int mChunkPosition, mChunkSize, mExternalCount, mFTPTIndex, mIndexCount, mInternalCount, mRCOLCount, mRCOLVersion, mTGICount, mTGIOffset, mTGISize, mVersion;

        Entry[] mEntries;

        TGI[] mExternalITG, mInternalITG, mTGIList;

        char[] mMagic;

        uint mUnknown;

        public TGI[] AllLinks
        {
            get
            {
                var temp = new List<TGI>();
                foreach (var entry in mEntries)
                {
                    foreach (var i in entry.IndexArray)
                    {
                        temp.Add(mTGIList[i]);
                    }
                }
                return temp.ToArray();
            }
        }

        public TGI[] BondLinks
        {
            get
            {
                var temp = new List<TGI>();
                foreach (var entry in mEntries)
                {
                    if (entry.Type == EntryType.BoneEntry)
                    {
                        foreach (var i in entry.IndexArray)
                        {
                            temp.Add(mTGIList[i]);
                        }
                    }
                }
                return temp.ToArray();
            }
        }

        public enum EntryType : byte
        {
            MeshEntry,
            BoneEntry
        }

        public class Entry
        {
            byte mLOD, mRefCount;

            int[] mTGIRefs;

            EntryType mType;

            public int[] IndexArray
            {
                get
                {
                    return mTGIRefs;
                }
            }

            public int LOD
            {
                get
                {
                    return mType == EntryType.MeshEntry && mTGIRefs.Length > 0 ? (int)mLOD : -1;
                }
                set
                {
                    if (mType == EntryType.MeshEntry && mTGIRefs.Length > 0)
                    {
                        mLOD = (byte)value;
                    }
                }
            }

            public int Size
            {
                get
                {
                    return mType == EntryType.MeshEntry ? 3 + (mRefCount * 4) : 5;
                }
            }

            public EntryType Type
            {
                get
                {
                    return mType;
                }
                set
                {
                    mType = value;
                }
            }

            public Entry(BinaryReader reader)
            {
                mType = (EntryType)reader.ReadByte();
                if (mType == EntryType.MeshEntry)
                {
                    mLOD = reader.ReadByte();
                    mRefCount = reader.ReadByte();
                    mTGIRefs = new int[mRefCount];
                    for (var i = 0; i < mRefCount; i++)
                    {
                        mTGIRefs[i] = reader.ReadInt32();
                    }
                }
                else if (mType == EntryType.BoneEntry)
                {
                    mTGIRefs = new int[]
                        {
                            reader.ReadInt32()
                        };
                }
            }

            public Entry(int lod, int tgiIndex) : this((byte)lod, new int[]
                {
                    tgiIndex
                })
            {
            }

            public Entry(byte lod, int[] tgiIndexArray)
            {
                mType = EntryType.MeshEntry;
                mLOD = lod;
                mTGIRefs = new int[tgiIndexArray.Length];
                mRefCount = (byte)tgiIndexArray.Length;
                for (var i = 0; i < tgiIndexArray.Length; i++)
                {
                    mTGIRefs[i] = tgiIndexArray[i];
                }
            }

            public Entry(int lod, int[] tgiIndexArray) : this((byte)lod, tgiIndexArray)
            {
            }

            public Entry(int tgiIndex)
            {
                mType = EntryType.BoneEntry;
                mTGIRefs = new int[]
                    {
                        tgiIndex
                    };
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write((byte)mType);
                if (mType == EntryType.MeshEntry)
                {
                    writer.Write(mLOD);
                    writer.Write(mRefCount);
                    for (var i = 0; i < mRefCount; i++)
                    {
                        writer.Write(mTGIRefs[i]);
                    }
                }
                else if (mType == EntryType.BoneEntry)
                {
                    writer.Write(mTGIRefs[0]);
                }
            }
        }

        public VPXY(BinaryReader reader)
        {
            reader.BaseStream.Position = 0;
            mRCOLVersion = reader.ReadInt32();
            mRCOLCount = reader.ReadInt32();
            mIndexCount = reader.ReadInt32();
            mExternalCount = reader.ReadInt32();
            mInternalCount = reader.ReadInt32();
            if (mInternalCount > 0)
            {
                mInternalITG = new TGI[mInternalCount];
            }
            for (var i = 0; i < mInternalCount; i++)
            {
                mInternalITG[i] = new TGI(reader, TGI.TGISequence.ITG);
            }
            if (mExternalCount > 0)
            {
                mExternalITG = new TGI[mExternalCount];
            }
            for (var i = 0; i < mExternalCount; i++)
            {
                mExternalITG[i] = new TGI(reader, TGI.TGISequence.ITG);
            }
            mChunkPosition = reader.ReadInt32();
            mChunkSize = reader.ReadInt32();
            mMagic = reader.ReadChars(4);
            mVersion = reader.ReadInt32();
            mTGIOffset = reader.ReadInt32();
            mTGISize = reader.ReadInt32();
            mCount = reader.ReadByte();
            mEntries = new Entry[mCount];
            for (var i = 0; i < mCount; i++)
            {
                mEntries[i] = new Entry(reader);
            }
            mBoxType = reader.ReadByte();
            mBoundingBox = new float[6];
            for (var i = 0; i < 6; i++)
            {
                mBoundingBox[i] = reader.ReadSingle();
            }
            mUnknown = reader.ReadUInt32();
            if (mVersion <= 4)
            {
                mFlag = reader.ReadByte();
                if (mFlag == 1)
                {
                    mFTPTIndex = reader.ReadInt32();
                }
            }
            if (mTGISize > 0)
            {
                mTGICount = reader.ReadInt32();
                mTGIList = new TGI[mTGICount];
                for (var i = 0; i < mTGICount; i++)
                {
                    mTGIList[i] = new TGI(reader);
                }
            }
            else
            {
                mTGICount = 0;
                mTGIList = new TGI[0];
            }
        }

        public VPXY(TGI tgi, TGI[][] geomTGIs) : this(tgi, new TGI[]
            {
            }, geomTGIs)
        {
        }

        public VPXY(TGI tgi, TGI[] boneTGIs, TGI[][] geomTGIs)
        {
            if (geomTGIs.GetLength(0) != 4)
            {
                throw new ApplicationException("First dimension of LOD TGIs must be 4!");
            }
            mRCOLVersion = 3;
            mRCOLCount = 1;
            mIndexCount = 0;
            mExternalCount = 0;
            mInternalCount = 1;
            mInternalITG = new TGI[]
                {
                    new TGI(tgi.Type, tgi.Group, tgi.Instance)
                };
            mChunkPosition = 44;
            mChunkSize = 141;
            mMagic = new char[]
                {
                    'V',
                    'P',
                    'X',
                    'Y'
                };
            mVersion = 4;
            mCount = 0;
            mTGICount = 0;
            for (var i = 0; i < boneTGIs.Length; i++)
            {
                mCount++;
                mTGICount++;
            }
            for (var i = 0; i < geomTGIs.GetLength(0); i++)
            {
                for (var j = 0; j < geomTGIs[i].Length; j++)
                {
                    mTGICount++;
                }
                if (geomTGIs[i].Length > 0)
                {
                    mCount++;
                }
            }
            mBoxType = 2;
            mBoundingBox = new float[]
                {
                    0,
                    0,
                    0,
                    0,
                    0,
                    0
                };
            mUnknown = 0;
            mFlag = 0;
            mEntries = new Entry[mCount];
            mTGIList = new TGI[mTGICount];
            if (mCount > 0)
            {
                int entryCount = 0,
                tgiCount = 0;
                for (var i = 0; i < boneTGIs.Length; i++)
                {
                    mEntries[entryCount] = new Entry(tgiCount);
                    mTGIList[tgiCount] = new TGI(boneTGIs[i].Type, boneTGIs[i].Group, boneTGIs[i].Instance);
                    entryCount++;
                    tgiCount++;
                }
                for (var i = 0; i < 4; i++)
                {
                    var temp = new List<int>();
                    for (int j = 0; j < geomTGIs[i].Length; j++)
                    {
                        temp.Add(tgiCount);
                        mTGIList[tgiCount] = new TGI(geomTGIs[i][j].Type, geomTGIs[i][j].Group, geomTGIs[i][j].Instance);
                        tgiCount++;
                    }
                    if (temp.Count > 0)
                    {
                        mEntries[entryCount] = new Entry(i, temp.ToArray());
                        entryCount++;
                    }
                }
            }
            mTGIOffset = 35;
            foreach (var entry in mEntries)
            {
                mTGIOffset += entry.Size;
            }
            mTGISize = mTGICount * 16 + 4;
        }

        public TGI[] GetMeshLinks(int lod)
        {
            var temp = new List<TGI>();
            foreach (var entry in mEntries)
            {
                if (entry.Type == EntryType.MeshEntry && entry.LOD == lod)
                {
                    foreach (var i in entry.IndexArray)
                    {
                        temp.Add(mTGIList[i]);
                    }
                }
            }
            return temp.ToArray();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(mRCOLVersion);
            writer.Write(mRCOLCount);
            writer.Write(mIndexCount);
            writer.Write(mExternalCount);
            writer.Write(mInternalCount);
            for (var i = 0; i < mInternalCount; i++)
            {
                mInternalITG[i].Write(writer, TGI.TGISequence.ITG);
            }
            for (var i = 0; i < mExternalCount; i++)
            {
                mExternalITG[i].Write(writer, TGI.TGISequence.ITG);
            }
            writer.Write(mChunkPosition);
            writer.Write(mChunkSize);
            writer.Write(mMagic);
            writer.Write(mVersion);
            writer.Write(mTGIOffset);
            writer.Write(mTGISize);
            writer.Write(mCount);
            for (var i = 0; i < mCount; i++)
            {
                mEntries[i].Write(writer);
            }
            writer.Write(mBoxType);
            for (var i = 0; i < 6; i++)
            {
                writer.Write(mBoundingBox[i]);
            }
            writer.Write(mUnknown);
            writer.Write(mFlag);
            if (mFlag == 1)
            {
                writer.Write(mFTPTIndex);
            }
            if (mTGISize > 0)
            {
                writer.Write(mTGICount);
                for (var i = 0; i < mTGICount; i++)
                {
                    mTGIList[i].Write(writer);
                }
            }
        }
    }
}
