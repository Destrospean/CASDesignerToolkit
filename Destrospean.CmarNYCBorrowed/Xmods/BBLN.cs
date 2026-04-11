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

namespace Destrospean.CmarNYCBorrowed
{
    public class BBLN
    {
        uint mBGEOGroup, mBGEOType;

        ulong mBGEOInstance;

        Entry[] mEntries;

        string mPartName;

        int mTGIOffset, mTGISize, mUnknown, mVersion;

        TGI[] mTGIList;

        public TGI BGEOTGI
        {
            get
            {
                return new TGI(mBGEOType, mBGEOGroup, mBGEOInstance);
            }
        }

        public Entry[] Entries
        {
            get
            {
                return mEntries;
            }
        }

        public TGI[] TGIList
        {
            get
            {
                return mTGIList;
            }
        }

        public class Entry
        {
            MorphEntry[] mBoneMorphs, mGEOMMorphs;

            CASRegions mRegion;

            public MorphEntry[] BoneMorphs
            {
                get
                {
                    return mBoneMorphs;
                }
            }

            public MorphEntry[] GEOMMorphs
            {
                get
                {
                    return mGEOMMorphs;
                }
            }

            public int Length
            {
                get
                {
                    return 12 + mGEOMMorphs.Length * 12 + mBoneMorphs.Length * 12;
                }
            }

            public Entry(BinaryReader reader)
            {
                mRegion = (CASRegions)reader.ReadUInt32();
                mGEOMMorphs = new MorphEntry[reader.ReadInt32()];
                for (var i = 0; i < mGEOMMorphs.Length; i++)
                {
                    mGEOMMorphs[i] = new MorphEntry(reader);
                }
                mBoneMorphs = new MorphEntry[reader.ReadInt32()];
                for (var i = 0; i < mBoneMorphs.Length; i++)
                {
                    mBoneMorphs[i] = new MorphEntry(reader);
                }
            }

            public Entry(CASRegions region, MorphEntry[] geomMorphs, MorphEntry[] boneMorphs)
            {
                mRegion = region;
                mGEOMMorphs = geomMorphs;
                mBoneMorphs = boneMorphs;
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write((uint)mRegion);
                writer.Write(mGEOMMorphs.Length);
                for (var i = 0; i < mGEOMMorphs.Length; i++)
                {
                    mGEOMMorphs[i].Write(writer);
                }
                writer.Write(mBoneMorphs.Length);
                for (var i = 0; i < mBoneMorphs.Length; i++)
                {
                    mBoneMorphs[i].Write(writer);
                }
            }
        }

        public class MorphEntry
        {
            uint mAgeGenderFlags;

            float mAmount;

            int mTGIIndex;

            public float Amount
            {
                get
                {
                    return mAmount;
                }
            }

            public int TGIIndex
            {
                get
                {
                    return mTGIIndex;
                }
            }

            public MorphEntry(uint ageGenderFlags, float amount, int tgiIndex)
            {
                mAgeGenderFlags = ageGenderFlags;
                mAmount = amount;
                mTGIIndex = tgiIndex;
            }

            public MorphEntry(BinaryReader reader)
            {
                mAgeGenderFlags = reader.ReadUInt32();
                mAmount = reader.ReadSingle();
                mTGIIndex = reader.ReadInt32();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(mAgeGenderFlags);
                writer.Write(mAmount);
                writer.Write(mTGIIndex);
            }
        }

        public BBLN(BinaryReader reader)
        {
            mVersion = reader.ReadInt32();
            mTGIOffset = reader.ReadInt32();
            mTGISize = reader.ReadInt32();
            var count = (int)reader.ReadByte();
            var bytes = reader.ReadBytes(count);
            mPartName = System.Text.Encoding.BigEndianUnicode.GetString(bytes);
            mUnknown = reader.ReadInt32();
            if (mVersion == 8)
            {
                mBGEOType = reader.ReadUInt32();
                mBGEOGroup = reader.ReadUInt32();
                mBGEOInstance = reader.ReadUInt64();
            }
            mEntries = new Entry[reader.ReadInt32()];
            for (var i = 0; i < mEntries.Length; i++)
            {
                mEntries[i] = new Entry(reader);
            }
            mTGIList = new TGI[reader.ReadInt32()];
            for (var i = 0; i < mTGIList.Length; i++)
            {
                mTGIList[i] = new TGI(reader);
            }
        }

        public BBLN(int version, string partName, TGI linkedResourceTGI)
        {
            mVersion = version;
            mPartName = partName;
            mUnknown = 2;
            if (version == 8)
            {
                mBGEOType = linkedResourceTGI.Type;
                mBGEOGroup = linkedResourceTGI.Group;
                mBGEOInstance = linkedResourceTGI.Instance;
            }
            mEntries = new Entry[1];
            mEntries[0] = new Entry(CASRegions.Body, new[]
                {
                    new MorphEntry(77951, 1, 0)
                }, new MorphEntry[0]);
            if (version == 8)
            {
                mTGIList = new[]
                    {
                        new TGI(0, 0, 0)
                    };
                return;
            }
            mTGIList = new[]
                {
                    new TGI(linkedResourceTGI.Type, linkedResourceTGI.Group, linkedResourceTGI.Instance)
                };
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(mVersion);
            var bytes = System.Text.Encoding.BigEndianUnicode.GetBytes(mPartName);
            mTGIOffset = 13 + bytes.Length;
            if (mVersion == 8)
            {
                mTGIOffset += 16;
            }
            for (var i = 0; i < mEntries.Length; i++)
            {
                mTGIOffset += mEntries[i].Length;
            }
            writer.Write(mTGIOffset);
            mTGISize = 4 + mTGIList.Length * 16;
            if (mVersion == 8)
            {
                mTGISize += 8;
            }
            writer.Write(mTGISize);
            writer.Write((byte)bytes.Length);
            writer.Write(bytes);
            writer.Write(mUnknown);
            if (mVersion == 8)
            {
                writer.Write(mBGEOType);
                writer.Write(mBGEOGroup);
                writer.Write(mBGEOInstance);
            }
            writer.Write(mEntries.Length);
            for (var i = 0; i < mEntries.Length; i++)
            {
                mEntries[i].Write(writer);
            }
            writer.Write(mTGIList.Length);
            for (var i = 0; i < mTGIList.Length; i++)
            {
                mTGIList[i].Write(writer);
            }
        }
    }
}
