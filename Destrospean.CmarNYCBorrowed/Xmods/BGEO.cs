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
    public class BGEO
    {
        char[] mMagic;

        Section1[] mSection1;

        int mSection1Count, mSection1HeaderSize, mSection1LODCount, mSection1LODSize, mSection1Offset, mSection2Count, mSection2Offset, mSection3Count, mSection3Offset, mVersion;

        Section2[] mSection2;

        Section3[] mSection3;

        public int Section1Count
        {
            get
            {
                return mSection1Count;
            }
        }

        public int Section1LODCount
        {
            get
            {
                return mSection1LODCount;
            }
        }

        public float Weight;

        [Serializable]
        public class BlendException : ApplicationException
        {
            public BlendException()
            {
            }

            public BlendException(string message) : base(message)
            {
            }

            public BlendException(string message, Exception inner) : base(message, inner)
            {
            }

            protected BlendException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
            {
            }
        }

        public class Section1
        {
            uint mAgeGenderSpecies, mRegion;

            int[] mEntryCount, mFirstVertexID, mOriginalEntryCount, mVertexIDCount;

            public uint AgeGenderSpecies
            {
                get
                {
                    var temp = mAgeGenderSpecies;
                    if ((temp & 0xF00) == 0)
                    {
                        temp = temp | (uint)Species.Human;
                    }
                    return temp;
                }
            }

            public uint Region
            {
                get
                {
                    return mRegion;
                }
            }

            public Section1()
            {
            }

            public Section1(int age, int gender, int species, int region, int[] firstVertexID, int[] vertexIDCount, int[] entryCount) : this((uint)(age + (gender << 12) + (species << 8)) + (1 << 16), region, firstVertexID, vertexIDCount, entryCount)
            {
            }

            public Section1(uint ageGenderSpecies, int region, int[] firstVertexID, int[] vertexIDCount, int[] entryCount)
            {
                mAgeGenderSpecies = ageGenderSpecies;
                mRegion = (uint)region;
                mFirstVertexID = firstVertexID;
                mVertexIDCount = vertexIDCount;
                mEntryCount = entryCount;
                mOriginalEntryCount = entryCount;
                if (firstVertexID.Length != 4 || vertexIDCount.Length != 4 || entryCount.Length != 4)
                {
                    throw new BlendException("Section 1 constructor: LOD information arrays must have four elements.");
                }
            }

            public Section1(BinaryReader reader, int section1LODCount)
            {
                mAgeGenderSpecies = reader.ReadUInt32();
                mRegion = reader.ReadUInt32();
                mFirstVertexID = new int[section1LODCount];
                mVertexIDCount = new int[section1LODCount];
                mEntryCount = new int[section1LODCount];
                mOriginalEntryCount = new int[section1LODCount];
                for (var i = 0; i < section1LODCount; i++)
                {
                    mFirstVertexID[i] = reader.ReadInt32();
                    mVertexIDCount[i] = reader.ReadInt32();
                    mEntryCount[i] = reader.ReadInt32();
                    mOriginalEntryCount[i] = mEntryCount[i];
                }
            }

            public int FixSection3Count(Section2[] section2, int section1LODCount, int Section2Index)
            {
                var index = Section2Index;
                for (var i = 0; i < section1LODCount; i++)
                {
                    var fixedCount = 0;
                    for (var j = index; j < index + mVertexIDCount[i]; j++)
                    {
                        if (section2[j].HasPosition)
                        {
                            fixedCount++;
                        }
                        if (section2[j].HasNormals)
                        {
                            fixedCount++;
                        }
                    }
                    mEntryCount[i] = Math.Max(fixedCount, mOriginalEntryCount[i]);
                    index += mVertexIDCount[i];
                }
                return index;
            }

            public int[] GetLODData(int lod)
            {
                return new int[]
                {
                    mFirstVertexID[lod],
                    mVertexIDCount[lod],
                    mEntryCount[lod],
                    mOriginalEntryCount[lod]
                };
            }

            public string GetLODDataAsString(int lod)
            {
                return mFirstVertexID[lod].ToString() + ", " + mVertexIDCount[lod].ToString() + ", " + mEntryCount[lod].ToString();
            }

            public override string ToString()
            {
                return ((Species)mAgeGenderSpecies).ToString() + ((AgeGender)mAgeGenderSpecies).ToString() + Environment.NewLine + "LOD 0: First vertex " + mFirstVertexID[0].ToString() + ", Number vertices " + mVertexIDCount[0].ToString() + ", Number entries " + mEntryCount[0] + Environment.NewLine + "LOD 1: First vertex " + mFirstVertexID[1].ToString() + ", Number vertices " + mVertexIDCount[1].ToString() + ", Number entries " + mEntryCount[1] + Environment.NewLine + "LOD 2: First vertex " + mFirstVertexID[2].ToString() + ", Number vertices " + mVertexIDCount[2].ToString() + ", Number entries " + mEntryCount[2] + Environment.NewLine + "LOD 3: First vertex " + mFirstVertexID[3].ToString() + ", Number vertices " + mVertexIDCount[3].ToString() + ", Number entries " + mEntryCount[3];
            }

            public void Write(BinaryWriter writer, int section1LODCount)
            {
                writer.Write(mAgeGenderSpecies);
                writer.Write(mRegion);
                for (var i = 0; i < section1LODCount; i++)
                {
                    writer.Write(mFirstVertexID[i]);
                    writer.Write(mVertexIDCount[i]);
                    writer.Write(mEntryCount[i]);
                }
            }
        }

        public class Section2
        {
            bool mHasNormals, mHasPosition;

            int mOffset;

            public bool HasNormals
            {
                get
                {
                    return mHasNormals;
                }
            }

            public bool HasPosition
            {
                get
                {
                    return mHasPosition;
                }
            }

            public int Offset
            {
                get
                {
                    return mOffset;
                }
            }

            public Section2()
            {
                mHasPosition = false;
                mHasNormals = false;
                mOffset = 0;
            }

            public Section2(bool hasPosition, bool hasNormals, int offset)
            {
                mHasPosition = hasPosition;
                mHasNormals = hasNormals;
                mOffset = offset;
            }

            public Section2(BinaryReader reader)
            {
                var temp = reader.ReadInt16();
                mHasPosition = (temp & 1) == 1;
                mHasNormals = (temp & 2) == 2;
                mOffset = temp >> 2;
            }

            public override string ToString()
            {
                return "Position: " + mHasPosition.ToString() + ", Normals: " + mHasNormals.ToString() + ", Offset: " + mOffset.ToString();
            }

            public void Write(BinaryWriter writer)
            {
                var temp = (short)(mOffset << 2);
                if (mHasPosition)
                {
                    temp += 1;
                }
                if (mHasNormals)
                {
                    temp += 2;
                }
                writer.Write(temp);
            }
        }

        public class Section3
        {
            float mX, mY, mZ;

            public float[] DeltaValues
            {
                get
                {
                    return new float[]
                    {
                        mX,
                        mY,
                        mZ
                    };
                }
            }

            public Section3(BinaryReader reader)
            {
                mX = ConvertIn(reader.ReadUInt16());
                mY = ConvertIn(reader.ReadUInt16());
                mZ = ConvertIn(reader.ReadUInt16());
            }

            public Section3(float x, float y, float z)
            {
                mX = x;
                mY = y;
                mZ = z;
            }

            public float ConvertIn(ushort encode)
            {
                return (((int)encode ^ 0x8000) << 16 >> 16) * .0005f;
            }

            public ushort ConvertOut(float decode)
            {
                return BitConverter.ToUInt16(BitConverter.GetBytes(Convert.ToInt32(decode * 2000) ^ 0x8000), 0);
            }

            public override string ToString()
            {
                return "X: " + mX.ToString() + ", Y: " + mY.ToString() + ", Z: " + mZ.ToString();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(ConvertOut(mX));
                writer.Write(ConvertOut(mY));
                writer.Write(ConvertOut(mZ));
            }
        }

        public class VertexData
        {
            public Vector3 Normals, Position;

            public int VertexID;

            public VertexData(int id, Vector3 position, Vector3 normals)
            {
                VertexID = id;
                Position = new Vector3(position);
                Normals = new Vector3(normals);
            }
        }

        public BGEO()
        {
        }

        public BGEO(GEOM[][][] ageGenderLODMorphMeshes, int[] ageArray, int[] genderArray, int[] speciesArray, int[] regionArray) //dimensions: age/gender group, lod, mesh - for slider morphs
        {
            if (ageGenderLODMorphMeshes.Length != ageArray.Length || ageGenderLODMorphMeshes.Length != genderArray.Length || ageGenderLODMorphMeshes.Length != speciesArray.Length || ageGenderLODMorphMeshes.Length != regionArray.Length)
            {
                throw new BlendException("Age/gender/species/region array lengths do not match GEOM array length!");
            }
            mMagic = new char[]
                {
                    'B',
                    'G',
                    'E',
                    'O'
                };
            mVersion = 768;
            mSection1Count = ageGenderLODMorphMeshes.Length;
            mSection1LODCount = 4;
            mSection1 = new Section1[Section1Count];
            var section2 = new List<Section2>();
            var section3 = new List<Section3>();
            int indexIn = 0,
            indexOut = 0;
            for (var i = 0; i < mSection1Count; i++)
            {
                var section1Info = new int[3][];
                for (var j = 0; j < 3; j++)
                {
                    section1Info[j] = new int[4];
                }
                for (var j = 0; j < ageGenderLODMorphMeshes[i].Length; j++)
                {
                    if (ageGenderLODMorphMeshes[i][j] == null || ageGenderLODMorphMeshes[i][j].Length == 0)
                    {
                        continue;
                    }
                    Section2[] tempSection2 = null;
                    Section3[] tempSection3 = null;
                    var firstVertex = 0;
                    BGEOLODConstructor(ageGenderLODMorphMeshes[i][j], indexIn, out firstVertex, out tempSection2, out tempSection3, out indexOut);
                    section1Info[0][j] = firstVertex;
                    section1Info[1][j] = tempSection2.Length;
                    section1Info[2][j] = tempSection3.Length;
                    section2.AddRange(tempSection2);
                    section3.AddRange(tempSection3);
                    indexIn = indexOut;
                }
                mSection1[i] = new Section1(ageArray[i], genderArray[i], speciesArray[i], regionArray[i], section1Info[0], section1Info[1], section1Info[2]);
            }
            mSection1HeaderSize = 8;
            mSection1LODSize = 12;
            mSection1Offset = 44;
            mSection2 = section2.ToArray();
            mSection2Offset = 44 + mSection1Count * 56;
            mSection2Count = mSection2.Length;
            mSection3 = section3.ToArray();
            mSection3Offset = mSection2Offset + mSection2Count * 2;
            mSection3Count = mSection3.Length;
        }

        public BGEO(GEOM[][][] ageGenderLODMorphMeshes, uint[] ageGenderSpeciesArray, uint[] regionArray) //dimensions: age/gender group, lod, mesh - for slider morphs
        {
            if (ageGenderLODMorphMeshes.Length != ageGenderSpeciesArray.Length || ageGenderLODMorphMeshes.Length != regionArray.Length)
            {
                throw new BlendException("Age/gender/species/region array lengths do not match GEOM array length!");
            }
            mMagic = new char[]
                {
                    'B',
                    'G',
                    'E',
                    'O'
                };
            mVersion = 768;
            mSection1Count = ageGenderLODMorphMeshes.Length;
            mSection1LODCount = 4;
            mSection1 = new Section1[Section1Count];
            var section2 = new List<Section2>();
            var section3 = new List<Section3>();
            int indexIn = 0,
            indexOut = 0;
            for (var i = 0; i < mSection1Count; i++)
            {
                var section1Info = new int[3][];
                for (var j = 0; j < 3; j++)
                {
                    section1Info[j] = new int[4];
                }
                for (var j = 0; j < ageGenderLODMorphMeshes[i].Length; j++)
                {
                    if (ageGenderLODMorphMeshes[i][j] == null || ageGenderLODMorphMeshes[i][j].Length == 0)
                    {
                        continue;
                    }
                    Section2[] tempSection2 = null;
                    Section3[] tempSection3 = null;
                    var firstVertex = 0;
                    BGEOLODConstructor(ageGenderLODMorphMeshes[i][j], indexIn, out firstVertex, out tempSection2, out tempSection3, out indexOut);
                    section1Info[0][j] = firstVertex;
                    section1Info[1][j] = tempSection2.Length;
                    section1Info[2][j] = tempSection3.Length;
                    section2.AddRange(tempSection2);
                    section3.AddRange(tempSection3);
                    indexIn = indexOut;
                }
                mSection1[i] = new Section1(ageGenderSpeciesArray[i], (int)regionArray[i], section1Info[0], section1Info[1], section1Info[2]);
            }
            mSection1HeaderSize = 8;
            mSection1LODSize = 12;
            mSection1Offset = 44;
            mSection2 = section2.ToArray();
            mSection2Offset = 44 + mSection1Count * 56;
            mSection2Count = mSection2.Length;
            mSection3 = section3.ToArray();
            mSection3Offset = mSection2Offset + mSection2Count * 2;
            mSection3Count = mSection3.Length;
        }

        public BGEO(GEOM[][] lodMorphMeshes) //dimensions: lod, mesh - for clothing fat/fit/thin/special morphs
        {
            mMagic = new char[]
                {
                    'B',
                    'G',
                    'E', 
                    'O'
                };
            mVersion = 768;
            mSection1Count = 1;
            mSection1LODCount = 4;
            mSection1 = new Section1[1];
            var section2 = new List<Section2>();
            var section3 = new List<Section3>();
            int indexIn = 0,
            indexOut = 0;
            var section1Info = new int[3][];
            for (var i = 0; i < 3; i++)
            {
                section1Info[i] = new int[4];
            }
            for (var i = 0; i < lodMorphMeshes.Length; i++)
            {
                if (lodMorphMeshes[i] == null || lodMorphMeshes[i].Length == 0)
                {
                    continue;
                }
                Section2[] tempSection2 = null;
                Section3[] tempSection3 = null;
                var firstVertex = 0;
                BGEOLODConstructor(lodMorphMeshes[i], indexIn, out firstVertex, out tempSection2, out tempSection3, out indexOut);
                section1Info[0][i] = firstVertex;
                section1Info[1][i] = tempSection2.Length;
                section1Info[2][i] = tempSection3.Length;
                section2.AddRange(tempSection2);
                section3.AddRange(tempSection3);
                indexIn = indexOut;
            }
            mSection1[0] = new Section1(127, 3, 1, (int)CASRegions.Body, section1Info[0], section1Info[1], section1Info[2]);
            mSection1HeaderSize = 8;
            mSection1LODSize = 12;
            mSection1Offset = 44;
            mSection2Offset = 100;
            mSection2 = section2.ToArray();
            mSection2Count = mSection2.Length;
            mSection3Offset = 100 + mSection2Count * 2;
            mSection3 = section3.ToArray();
            mSection3Count = mSection3.Length;
        }

        public BGEO(BinaryReader reader)
        {
            reader.BaseStream.Position = 0;
            mMagic = reader.ReadChars(4);
            if (new string(mMagic) != "BGEO")
            {
                throw new BlendException("Not a valid BGEO file.");
            }
            mVersion = reader.ReadInt32();
            if (mVersion != 768)
            {
                throw new BlendException("Not a recognized BGEO version.");
            }
            mSection1Count = reader.ReadInt32();
            mSection1LODCount = reader.ReadInt32();
            mSection2Count = reader.ReadInt32();
            mSection3Count = reader.ReadInt32();
            mSection1HeaderSize = reader.ReadInt32();
            mSection1LODSize = reader.ReadInt32();
            mSection1Offset = reader.ReadInt32();
            mSection2Offset = reader.ReadInt32();
            mSection3Offset = reader.ReadInt32();
            mSection1 = new Section1[mSection1Count];
            for (var i = 0; i < mSection1Count; i++)
            {
                mSection1[i] = new Section1(reader, mSection1LODCount);
            }
            mSection2 = new Section2[mSection2Count];
            for (var i = 0; i < mSection2Count; i++)
            {
                mSection2[i] = new Section2(reader);
            }
            mSection3 = new Section3[mSection3Count];
            for (var i = 0; i < mSection3Count; i++)
            {
                mSection3[i] = new Section3(reader);
            }
            var section2Index = 0;
            for (var i = 0; i < mSection1Count; i++)
            {
                section2Index = mSection1[i].FixSection3Count(mSection2, mSection1LODCount, section2Index);
            }
        }

        public void BGEOLODConstructor(GEOM[] lodMorphMeshes, int indexIn, out int firstVertexID, out Section2[] outSection2, out Section3[] outSection3, out int indexOut)
        {
            int maxVertexID = 0,
            minVertexID = Int32.MaxValue;
            float normalsLimit = 0,
            positionLimit = 0;
            for (var i = 0; i < lodMorphMeshes.Length; i++)
            {
                if (!lodMorphMeshes[i].IsMorph)
                {
                    throw new BlendException("Not a valid morph mesh!");
                }
                minVertexID = Math.Min(lodMorphMeshes[i].MinVertexID, minVertexID);
                maxVertexID = Math.Max(lodMorphMeshes[i].MaxVertexID, maxVertexID);
            }
            /*
            var overlap = false;
            if (lodMorphMeshes.Length > 1 && (lodMorphMeshes[1].MinVertexID > lodMorphMeshes[0].MinVertexID && lodMorphMeshes[1].MinVertexID < lodMorphMeshes[0].MaxVertexID || lodMorphMeshes[1].MaxVertexID > lodMorphMeshes[0].MinVertexID && lodMorphMeshes[1].MaxVertexID < lodMorphMeshes[0].MaxVertexID))
            {
                overlap = true;
            }
            if (lodMorphMeshes.Length > 2 && (lodMorphMeshes[2].MinVertexID > lodMorphMeshes[0].MinVertexID && lodMorphMeshes[2].MinVertexID < lodMorphMeshes[0].MaxVertexID || lodMorphMeshes[2].MaxVertexID > lodMorphMeshes[0].MinVertexID && lodMorphMeshes[2].MaxVertexID < lodMorphMeshes[0].MaxVertexID))
            {
                overlap = true;
            }
            if (lodMorphMeshes.Length > 2 && (lodMorphMeshes[2].MinVertexID > lodMorphMeshes[1].MinVertexID && lodMorphMeshes[2].MinVertexID < lodMorphMeshes[1].MaxVertexID || lodMorphMeshes[2].MaxVertexID > lodMorphMeshes[1].MinVertexID && lodMorphMeshes[2].MaxVertexID < lodMorphMeshes[1].MaxVertexID))
            {
                overlap = true;
            }
            if (overlap)
            {
                if (MessageBox.Show("Your meshes have overlapping vertex IDs within\na LOD and the morph may not work correctly.\nDo you want to continue anyway?", "Vertex numbering alert", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    throw new BlendException("Vertex numbering error");
                }
            }
            */
            var vertexIDCount = maxVertexID - minVertexID + 1;
            var vertexList = new VertexData[vertexIDCount];
            for (var i = 0; i < lodMorphMeshes.Length; i++)
            {
                for (var j = 0; j < lodMorphMeshes[i].VertexCount; j++)
                {
                    var id = lodMorphMeshes[i].GetVertexID(j);
                    vertexList[id - minVertexID] = new VertexData(id, new Vector3(lodMorphMeshes[i].GetPosition(j)), new Vector3(lodMorphMeshes[i].GetNormal(j)));
                }
            }
            //var gap = false;
            var nothing = new Vector3();
            for (var i = 0; i < vertexList.Length; i++)
            {
                if (vertexList[i] == null)
                {
                    //gap = true;
                    vertexList[i] = new VertexData(i + minVertexID, nothing, nothing);
                }
            }
            /*
            if (gap)
            {
                if (MessageBox.Show("Your meshes have a gap in vertex IDs within\na LOD but the morph will probably work.\nDo you want to continue anyway?", "Vertex numbering alert", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    throw new BlendException("Vertex numbering error");
                }
            }
            */
            var newSection2 = new Section2[vertexIDCount];
            var newSection3 = new List<Section3>();
            var offset = 0 - indexIn;
            for (var i = 0; i < vertexList.Length; i++)
            {
                bool hasNormals = false,
                hasPosition = false;
                if (Math.Abs(vertexList[i].Position.X) > positionLimit || Math.Abs(vertexList[i].Position.Y) > positionLimit || Math.Abs(vertexList[i].Position.Z) > positionLimit)
                {
                    newSection3.Add(new Section3(vertexList[i].Position.X, vertexList[i].Position.Y, vertexList[i].Position.Z));
                    hasPosition = true;
                }
                if (Math.Abs(vertexList[i].Normals.X) > normalsLimit || Math.Abs(vertexList[i].Normals.Y) > normalsLimit || Math.Abs(vertexList[i].Normals.Z) > normalsLimit)
                {
                    newSection3.Add(new Section3(vertexList[i].Normals.X, vertexList[i].Normals.Y, vertexList[i].Normals.Z));
                    hasNormals = true;
                }
                if (hasNormals || hasPosition)
                {
                    newSection2[i] = new Section2(hasPosition, hasNormals, offset);
                    offset = 0;
                    if (hasNormals || hasPosition)
                    {
                        offset++;
                    }
                }
                else
                {
                    newSection2[i] = new Section2(false, false, 0);
                }
            }
            firstVertexID = minVertexID;
            outSection2 = newSection2;
            outSection3 = newSection3.ToArray();
            indexOut = outSection3.Length - offset;
        }

        public List<VertexData> GetDeltas(int entry, int lod)
        {
            var vertexDeltas = new List<VertexData>();
            int runningOffset = GetLODInitialOffset(entry, lod),
            section2Count = GetSection2Count(entry, lod),
            section2Start = GetSection2StartIndex(entry, lod),
            section2StartVertexID = GetLODStartVertexID(entry, lod),
            section3Start = GetSection3StartIndex(entry, lod);
            for (var i = 0; i < section2Count; i++)
            {
                var section2 = GetSection2(section2Start + i);
                runningOffset += section2.Offset;
                Vector3 normal = new Vector3(),
                position = new Vector3();
                var tempIndex = runningOffset;
                if (section2.HasPosition)
                {
                    position = new Vector3(GetSection3(tempIndex + section3Start));
                    tempIndex++;
                }
                if (section2.HasNormals)
                {
                    normal = new Vector3(GetSection3(tempIndex + section3Start));
                }
                var vertexData = new VertexData(section2StartVertexID + i, position, normal);
                vertexDeltas.Add(vertexData);
            }
            return vertexDeltas;
        }

        public int GetLODInitialOffset(int section1EntryIndex, int lod)
        {
            if (section1EntryIndex == 0 && lod == 0)
            {
                return 0;
            }
            var offset = 0;
            for (var i = 0; i < section1EntryIndex; i++)
            {
                int count = GetSection2Count(i),
                start = GetSection2StartIndex(i, 0);
                for (var j = start; j < start + count; j++)
                {
                    offset += GetSection2(j).Offset;
                }
            }
            for (var i = 0; i < lod; i++)
            {
                int lodCount = GetSection2Count(section1EntryIndex, i),
                lodStart = GetSection2StartIndex(section1EntryIndex, i);
                for (var j = lodStart; j < lodStart + lodCount; j++)
                {
                    offset += GetSection2(j).Offset;
                }
            }
            return offset;
        }

        public int GetLODStartVertexID(int section1EntryIndex, int lod)
        {
            return mSection1[section1EntryIndex].GetLODData(lod)[0];
        }

        public Section1 GetSection1(int section1EntryIndex)
        {
            return mSection1[section1EntryIndex];
        }

        public int GetSection1EntryIndex(Species species, AgeGender age, AgeGender gender)
        {
            for (var i = 0; i < mSection1Count; i++)
            {
                if ((mSection1[i].AgeGenderSpecies & (uint)species) > 0 && (mSection1[i].AgeGenderSpecies & (uint)age) > 0 && (mSection1[i].AgeGenderSpecies & (uint)gender) > 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public Section2 GetSection2(int section2Index)
        {
            return mSection2[section2Index];
        }

        public int GetSection2Count()
        {
            return mSection2Count;
        }

        public int GetSection2Count(int section1EntryIndex)
        {
            var section1 = GetSection1(section1EntryIndex);
            var temp = 0;
            for (var i = 0; i < mSection1LODCount; i++)
            {
                temp += section1.GetLODData(i)[1];
            }
            return temp;
        }

        public int GetSection2Count(int section1EntryIndex, int lod)
        {
            return GetSection1(section1EntryIndex).GetLODData(lod)[1];
        }

        public int GetSection2StartIndex(int section1EntryIndex, int lod)
        {
            var temp = 0;
            for (var i = 0; i < section1EntryIndex; i++)
            {
                for (var j = 0; j < Section1LODCount; j++)
                {
                    temp += mSection1[i].GetLODData(j)[1];
                }
            }
            for (var j = 0; j < lod; j++)
            {
                temp += mSection1[section1EntryIndex].GetLODData(j)[1];
            }
            return temp;
        }

        public int GetSection3Count()
        {
            return mSection3Count;
        }

        public int GetSection3Count(int section1EntryIndex)
        {
            var section1 = GetSection1(section1EntryIndex);
            var temp = 0;
            for (var i = 0; i < mSection1LODCount; i++)
            {
                temp += section1.GetLODData(i)[2];
            }
            return temp;
        }

        public float[] GetSection3(int section3Index)
        {
            return section3Index < mSection3.Length ? mSection3[section3Index].DeltaValues : new float[]
            {
                0,
                0,
                0
            };
        }

        public int GetSection3Count(int section1EntryIndex, int lod)
        {
            return GetSection1(section1EntryIndex).GetLODData(lod)[2];
        }

        public int GetSection3StartIndex(int section1EntryIndex, int lod)
        {
            var temp = 0;
            for (var i = 0; i < section1EntryIndex; i++)
            {
                for (var j = 0; j < Section1LODCount; j++)
                {
                    temp += mSection1[i].GetLODData(j)[2];
                }
            }
            for (var j = 0; j < lod; j++)
            {
                temp += mSection1[section1EntryIndex].GetLODData(j)[2];
            }
            return temp;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(mMagic);
            writer.Write(mVersion);
            writer.Write(mSection1Count);
            writer.Write(mSection1LODCount);
            writer.Write(mSection2Count);
            writer.Write(mSection3Count);
            writer.Write(mSection1HeaderSize);
            writer.Write(mSection1LODSize);
            writer.Write(mSection1Offset);
            writer.Write(mSection2Offset);
            writer.Write(mSection3Offset);
            for (var i = 0; i < mSection1Count; i++)
            {
                mSection1[i].Write(writer, mSection1LODCount);
            }
            for (var i = 0; i < mSection2Count; i++)
            {
                mSection2[i].Write(writer);
            }
            for (var i = 0; i < mSection3Count; i++)
            {
                mSection3[i].Write(writer);
            }
        }
    }
}
