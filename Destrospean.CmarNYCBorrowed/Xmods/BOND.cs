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
    public class BOND
    {
        uint mVersion = 1;

        public BoneAdjust[] Adjustments;

        public uint ContextVersion = 3;

        public TGI[] DelayLoadKey, ExternalKey, PublicKey;

        /// <summary>
        /// Tests for extreme scaling
        /// </summary>
        /// <returns></returns>
        public bool IsSizeMorph
        {
            get
            {
                foreach (var a in Adjustments)
                {
                    if ((a.SlotHash == 0xFEAE6981 || a.SlotHash == 0x57884BB9 || a.SlotHash == 0x556B181A || a.SlotHash == 0x6FA96266 || a.SlotHash == 0xAFAC05CF || a.SlotHash == 0x6FAF7238) && (Math.Abs(a.ScaleX) > .1f || Math.Abs(a.ScaleY) > .1f || Math.Abs(a.ScaleZ) > .1f))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public string Name = "unknown";

        public ObjectData[] ObjectKey;

        public float Weight = 1;

        public class BoneAdjust
        {
            public float OffsetX, OffsetY, OffsetZ, QuatW, QuatX, QuatY, QuatZ, ScaleX, ScaleY, ScaleZ;

            public uint SlotHash;

            public BoneAdjust()
            {
            }

            public BoneAdjust(BoneAdjust other)
            {
                SlotHash = other.SlotHash;
                OffsetX = other.OffsetX;
                OffsetY = other.OffsetY;
                OffsetZ = other.OffsetZ;
                ScaleX = other.ScaleX;
                ScaleY = other.ScaleY;
                ScaleZ = other.ScaleZ;
                QuatX = other.QuatX;
                QuatY = other.QuatY;
                QuatZ = other.QuatZ;
                QuatW = other.QuatW;
            }

            public BoneAdjust(BinaryReader reader)
            {
                SlotHash = reader.ReadUInt32();
                OffsetX = reader.ReadSingle();
                OffsetY = reader.ReadSingle();
                OffsetZ = reader.ReadSingle();
                ScaleX = reader.ReadSingle();
                ScaleY = reader.ReadSingle();
                ScaleZ = reader.ReadSingle();
                QuatX = reader.ReadSingle();
                QuatY = reader.ReadSingle();
                QuatZ = reader.ReadSingle();
                QuatW = reader.ReadSingle();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(SlotHash);
                writer.Write(OffsetX);
                writer.Write(OffsetY);
                writer.Write(OffsetZ);
                writer.Write(ScaleX);
                writer.Write(ScaleY);
                writer.Write(ScaleZ);
                writer.Write(QuatX);
                writer.Write(QuatY);
                writer.Write(QuatZ);
                if (QuatX > 0 || QuatY > 0 || QuatZ > 0)
                {
                    writer.Write(QuatW);
                }
                else
                {
                    writer.Write(0f);
                }
            }
        }

        public class ObjectData
        {
            uint mLength, mPosition;

            public ObjectData(uint position, uint length)
            {
                mPosition = position;
                mLength = length;
            }

            public ObjectData(BinaryReader reader)
            {
                mPosition = reader.ReadUInt32();
                mLength = reader.ReadUInt32();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(mPosition);
                writer.Write(mLength);
            }
        }

        public BOND()
        {
            PublicKey = new[]
                {
                    new TGI()
                };
            Adjustments = new BoneAdjust[0];
        }

        public BOND(BinaryReader reader)
        {
            reader.BaseStream.Position = 0;
            ContextVersion = reader.ReadUInt32();
            uint publicKeyCount = reader.ReadUInt32(),
            externalKeyCount = reader.ReadUInt32(),
            delayLoadKeyCount = reader.ReadUInt32(),
            objectKeyCount = reader.ReadUInt32();
            PublicKey = new TGI[publicKeyCount];
            for (var i = 0; i < publicKeyCount; i++)
            {
                PublicKey[i] = new TGI(reader, TGI.TGISequence.ITG);
            }
            ExternalKey = new TGI[externalKeyCount];
            for (var i = 0; i < externalKeyCount; i++)
            {
                ExternalKey[i] = new TGI(reader, TGI.TGISequence.ITG);
            }
            DelayLoadKey = new TGI[delayLoadKeyCount];
            for (var i = 0; i < delayLoadKeyCount; i++)
            {
                DelayLoadKey[i] = new TGI(reader, TGI.TGISequence.ITG);
            }
            ObjectKey = new ObjectData[objectKeyCount];
            ObjectKey[0] = new ObjectData(reader);
            mVersion = reader.ReadUInt32();
            Adjustments = new BoneAdjust[reader.ReadUInt32()];
            for (var i = 0; i < Adjustments.Length; i++)
            {
                Adjustments[i] = new BoneAdjust(reader);
            }
        }

        public void AddBoneAdjust(BoneAdjust adjust)
        {
            var temp = new BoneAdjust[Adjustments.Length + 1];
            Array.Copy(Adjustments, temp, Adjustments.Length);
            temp[temp.Length - 1] = new BoneAdjust(adjust);
            Adjustments = temp;
        }

        public void RemoveBoneAdjust(int index)
        {
            var temp = new BoneAdjust[Adjustments.Length - 1];
            Array.Copy(Adjustments, 0, temp, 0, index);
            Array.Copy(Adjustments, index + 1, temp, index, temp.Length - index);
            Adjustments = temp;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(ContextVersion);
            if (PublicKey == null)
            {
                PublicKey = new TGI[0];
            }
            writer.Write(PublicKey.Length);
            if (ExternalKey == null)
            {
                ExternalKey = new TGI[0];
            }
            writer.Write(ExternalKey.Length);
            if (DelayLoadKey == null)
            {
                DelayLoadKey = new TGI[0];
            }
            writer.Write(DelayLoadKey.Length);
            writer.Write(1);
            for (var i = 0; i < PublicKey.Length; i++)
            {
                PublicKey[i].Write(writer, TGI.TGISequence.ITG);
            }
            for (var i = 0; i < ExternalKey.Length; i++)
            {
                ExternalKey[i].Write(writer, TGI.TGISequence.ITG);
            }
            for (var i = 0; i < DelayLoadKey.Length; i++)
            {
                DelayLoadKey[i].Write(writer, TGI.TGISequence.ITG);
            }
            ObjectKey = new[]
                {
                    new ObjectData((uint)(20 + (PublicKey.Length << 4) + (ExternalKey.Length << 4) + (DelayLoadKey.Length << 4) + 8), (uint)(4 + Adjustments.Length * 44))
                };
            for (var i = 0; i < ObjectKey.Length; i++)
            {
                ObjectKey[i].Write(writer);
            }
            writer.Write(mVersion);
            if (Adjustments == null)
            {
                Adjustments = new BoneAdjust[0];
            }
            writer.Write(Adjustments.Length);
            for (var i = 0; i < Adjustments.Length; i++)
            {
                Adjustments[i].Write(writer);
            }
        }
    }
}
