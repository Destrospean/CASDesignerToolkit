using System;
using System.Collections.Generic;
using System.IO;

namespace Destrospean.CmarNYCBorrowed
{
    public class WSO
    {
        int mBoneCount, mMeshCount, mVersion, mUnknown;

        Bone[] mBones;

        MeshGroup[] mMeshes;

        char[] mSoftwareName;

        public MeshGroup Base
        {
            get
            {
                var index = 0;
                if (MeshNames.Length > 1)
                {
                    index = Array.IndexOf(MeshNames, "group_base");
                }
                if (index == -1)
                {
                    index = Array.IndexOf(MeshNames, "group_0");
                }
                if (index > -1)
                {
                    return mMeshes[index];
                }
                return null;
            }
            set
            {
                var index = 0;
                if (MeshNames.Length > 1)
                {
                    index = Array.IndexOf(MeshNames, "group_base");
                }
                if (index == -1)
                {
                    index = Array.IndexOf(MeshNames, "group_0");
                }
                if (index > -1)
                {
                    mMeshes[index] = value;
                    return;
                }
                var meshGroups = new MeshGroup[mMeshes.Length + 1];
                meshGroups[0] = value;
                for (var i = 0; i < mMeshes.Length; i++)
                {
                    meshGroups[i + 1] = mMeshes[i];
                }
                mMeshes = meshGroups;
            }
        }

        public int BaseIndex
        {
            get
            {
                var index = 0;
                if (MeshNames.Length > 1)
                {
                    index = Array.IndexOf(MeshNames, "group_base");
                }
                if (index == -1)
                {
                    index = Array.IndexOf(MeshNames, "group_0");
                }
                if (index > -1)
                {
                    return index;
                }
                return 0;
            }
        }

        public Bone[] BoneList
        {
            get
            {
                return mBones;
            }
            set
            {
                var bones = new Bone[value.Length];
                for (var i = 0; i < value.Length; i++)
                {
                    bones[i] = new Bone(value[i]);
                }
                mBones = bones;
                mBoneCount = mBones.Length;
            }
        }

        public string[] BoneNameList
        {
            get
            {
                var bones = new string[BoneCount];
                for (var i = 0; i < BoneCount; i++)
                {
                    bones[i] = mBones[i].ToString();
                }
                return bones;
            }
        }

        public int EmptyBoneIndex
        {
            get
            {
                if (mMeshes.Length < 1)
                {
                    return -1;
                }
                var vertices = mMeshes[0].Vertices;
                for (var i = 0; i < vertices.Length; i++)
                {
                    var vertex = vertices[i];
                    for (var j = 0; j < vertex.BoneAssignments.Length; j++)
                    {
                        if (vertex.BoneWeights[j] == 0)
                        {
                            return vertex.BoneAssignments[j];
                        }
                    }
                }
                return Array.IndexOf(BoneNameList, Enum.GetName(typeof(BoneHash), BoneHash.b__ROOT_bind__));
            }
        }

        public MeshGroup Fat
        {
            get
            {
                var index = Array.IndexOf(MeshNames, "group_fat");
                if (index > -1)
                {
                    return mMeshes[index];
                }
                return null;
            }
            set
            {
                var index = Array.IndexOf(MeshNames, "group_fat");
                if (index > -1)
                {
                    mMeshes[index] = value;
                    return;
                }
                var meshGroups = new MeshGroup[mMeshes.Length + 1];
                for (var i = 0; i < mMeshes.Length; i++)
                {
                    meshGroups[i] = mMeshes[i];
                }
                meshGroups[mMeshes.Length] = value;
                mMeshes = meshGroups;
                mMeshCount = mMeshes.Length;
            }
        }

        public MeshGroup Fit
        {
            get
            {
                var index = Array.IndexOf(MeshNames, "group_fit");
                if (index > -1)
                {
                    return mMeshes[index];
                }
                return null;
            }
            set
            {
                var index = Array.IndexOf(MeshNames, "group_fit");
                if (index > -1)
                {
                    mMeshes[index] = value;
                    return;
                }
                var meshGroups = new MeshGroup[mMeshes.Length + 1];
                for (var i = 0; i < mMeshes.Length; i++)
                {
                    meshGroups[i] = mMeshes[i];
                }
                meshGroups[mMeshes.Length] = value;
                mMeshes = meshGroups;
                mMeshCount = mMeshes.Length;
            }
        }

        public bool HasBase
        {
            get
            {
                return Array.IndexOf(MeshNames, "group_base") >= 0 || Array.IndexOf(MeshNames, "group_0") >= 0;
            }
        }

        public bool HasFat
        {
            get
            {
                return Array.IndexOf(MeshNames, "group_fat") >= 0;
            }
        }

        public bool HasFit
        {
            get
            {
                return Array.IndexOf(MeshNames, "group_fit") >= 0;
            }
        }

        public bool HasMorphs
        {
            get
            {
                return MeshNames.Length > 1;
            }
        }

        public bool HasSpecial
        {
            get
            {
                return Array.IndexOf(MeshNames, "group_special") >= 0;
            }
        }

        public bool HasThin
        {
            get
            {
                return Array.IndexOf(MeshNames, "group_thin") >= 0;
            }
        }

        public string[] MeshNames
        {
            get
            {
                var meshNames = new string[MeshCount];
                for (var i = 0; i < meshNames.Length; i++)
                {
                    meshNames[i] = mMeshes[i].MeshName;
                }
                return meshNames;
            }
        }

        public int BoneCount
        {
            get
            {
                return mBoneCount;
            }
        }

        public int MeshCount
        {
            get
            {
                return mMeshCount;
            }
        }

        public MeshGroup Special
        {
            get
            {
                var index = Array.IndexOf(MeshNames, "group_special");
                if (index > -1)
                {
                    return mMeshes[index];
                }
                return null;
            }
            set
            {
                var index = Array.IndexOf(MeshNames, "group_special");
                if (index > -1)
                {
                    mMeshes[index] = value;
                    return;
                }
                var meshGroups = new MeshGroup[mMeshes.Length + 1];
                for (var i = 0; i < mMeshes.Length; i++)
                {
                    meshGroups[i] = mMeshes[i];
                }
                meshGroups[mMeshes.Length] = value;
                mMeshes = meshGroups;
                mMeshCount = mMeshes.Length;
            }
        }

        public MeshGroup Thin
        {
            get
            {
                var index = Array.IndexOf(MeshNames, "group_thin");
                if (index > -1)
                {
                    return mMeshes[index];
                }
                return null;
            }
            set
            {
                var index = Array.IndexOf(MeshNames, "group_thin");
                if (index > -1)
                {
                    mMeshes[index] = value;
                    return;
                }
                var meshGroups = new MeshGroup[mMeshes.Length + 1];
                for (var i = 0; i < mMeshes.Length; i++)
                {
                    meshGroups[i] = mMeshes[i];
                }
                meshGroups[mMeshes.Length] = value;
                mMeshes = meshGroups;
                mMeshCount = mMeshes.Length;
            }
        }

        public enum MeshName
        {
            group_0,
            group_base,
            group_fat,
            group_fit,
            group_thin,
            group_special
        }

        public class Bone
        {
            char[] mBoneName;

            float[] mBonePosition = new float[3],
            mBoneRotation = new float[3];

            byte mNameLength;

            public string Name
            {
                get
                {
                    return new string(mBoneName);
                }
                set
                {
                    mBoneName = value.ToCharArray();
                    mNameLength = (byte)mBoneName.Length;
                }
            }

            public Bone()
            {
            }

            public Bone(uint boneHash)
            {
                var name = Enum.GetName(typeof(BoneHash), boneHash);
                mNameLength = (byte)name.Length;
                mBoneName = name.ToCharArray();
                mBonePosition = new float[3];
                mBoneRotation = new float[3];
            }

            public Bone(string boneName)
            {
                mNameLength = (byte)boneName.Length;
                mBoneName = boneName.ToCharArray();
                mBonePosition = new float[3];
                mBoneRotation = new float[3];
            }

            public Bone(BinaryReader reader)
            {
                mNameLength = reader.ReadByte();
                mBoneName = new char[(int)mNameLength];
                mBoneName = reader.ReadChars((int)mNameLength);
                for (var i = 0; i < 3; i++)
                {
                    mBonePosition[i] = reader.ReadSingle();
                }
                for (var i = 0; i < 3; i++)
                {
                    mBoneRotation[i] = reader.ReadSingle();
                }
            }

            public Bone(Bone source)
            {
                mNameLength = source.mNameLength;
                mBoneName = new char[(int)source.mNameLength];
                for (var i = 0; i < (int)source.mNameLength; i++)
                {
                    mBoneName[i] = source.mBoneName[i];
                }
                for (var i = 0; i < 3; i++)
                {
                    mBonePosition[i] = source.mBonePosition[i];
                    mBoneRotation[i] = source.mBoneRotation[i];
                }
            }

            public bool Equals(Bone bone)
            {
                if (mNameLength != bone.mNameLength)
                {
                    return false;
                }
                for (var i = 0; i < (int)mNameLength; i++)
                {
                    if (!mBoneName[i].Equals(bone.mBoneName[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Bone);
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }

            public override string ToString()
            {
                return new string(mBoneName);
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(mNameLength);
                writer.Write(mBoneName);
                for (var i = 0; i < 3; i++)
                {
                    writer.Write(mBonePosition[i]);
                }
                for (var i = 0; i < 3; i++)
                {
                    writer.Write(mBoneRotation[i]);
                }
            }
        }

        public class FacePoint
        {
            float mNormalX, mNormalY, mNormalZ, mU, mV;

            short mVertexIndex;

            public float[] Normals
            {
                get
                {
                    return new float[]
                    {
                        mNormalX,
                        mNormalY,
                        mNormalZ
                    };
                }
                set
                {
                    mNormalX = value[0];
                    mNormalY = value[1];
                    mNormalZ = value[2];
                }
            }

            public float[] UVs
            {
                get
                {
                    return new float[]
                    {
                        mU,
                        mV
                    };
                }
                set
                {
                    mU = value[0];
                    mV = value[1];
                }
            }

            public short VertexIndex
            {
                get
                {
                    return mVertexIndex;
                }
            }

            public FacePoint()
            {
            }

            public FacePoint(FacePoint facePoint)
            {
                mVertexIndex = facePoint.mVertexIndex;
                mNormalX = facePoint.mNormalX;
                mNormalY = facePoint.mNormalY;
                mNormalZ = facePoint.mNormalZ;
                mU = facePoint.mU;
                mV = facePoint.mV;
            }

            public FacePoint(BinaryReader reader)
            {
                mVertexIndex = reader.ReadInt16();
                mNormalX = reader.ReadSingle();
                mNormalY = reader.ReadSingle();
                mNormalZ = reader.ReadSingle();
                mU = reader.ReadSingle();
                mV = reader.ReadSingle();
            }

            public FacePoint(int vertexIndex, float[] normals, float[] uvs, bool verticalFlip = false)
            {
                mVertexIndex = (short)vertexIndex;
                mNormalX = normals[0];
                mNormalY = normals[1];
                mNormalZ = normals[2];
                mU = uvs[0];
                mV = verticalFlip ? 1 - uvs[1] : uvs[1];
            }

            public override string ToString()
            {
                return string.Concat(new string[]
                    {
                        "Index: ",
                        VertexIndex.ToString(),
                        ", Normals X: ",
                        mNormalX.ToString(),
                        ", Y: ",
                        mNormalY.ToString(),
                        ", Z: ",
                        mNormalZ.ToString(),
                        ", UV X: ",
                        mU.ToString(),
                        ", Y: ",
                        mV.ToString()
                    });
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(mVertexIndex);
                writer.Write(mNormalX);
                writer.Write(mNormalY);
                writer.Write(mNormalZ);
                writer.Write(mU);
                writer.Write(mV);
            }
        }

        public class MeshGroup
        {
            byte mNameLength;

            char[] mMeshName;

            public int BonesUsedCount
            {
                get
                {
                    return BonesUsedIndices.Length;
                }
            }

            public int[] BonesUsedIndices
            {
                get
                {
                    var bonesUsedIndices = new List<int>();
                    for (var i = 0; i < VertexCount; i++)
                    {
                        var boneAssignments = Vertices[i].BoneAssignments;
                        var boneWeights = Vertices[i].BoneWeights;
                        for (var j = 0; j < 4; j++)
                        {
                            if (boneWeights[j] > 0 && bonesUsedIndices.IndexOf(boneAssignments[j]) < 0)
                            {
                                bonesUsedIndices.Add(boneAssignments[j]);
                            }
                        }
                    }
                    return bonesUsedIndices.ToArray();
                }
            }

            public int FaceCount;

            public int FacePointCount
            {
                get
                {
                    return FaceCount * 3;
                }
            }

            public FacePoint[] FacePoints;

            public int GeostateCount;

            public bool HasValidIDs
            {
                get
                {
                    for (var i = 0; i < VertexCount; i++)
                    {
                        if (Vertices[i].VertexID < 0)
                        {
                            return false;
                        }
                    }
                    return VertexIDRange[1] - VertexIDRange[0] > 0;
                }
            }

            public string MeshName
            {
                get
                {
                    return new string(mMeshName);
                }
                set
                {
                    mMeshName = value.ToCharArray();
                    mNameLength = (byte)mMeshName.Length;
                }
            }

            public int VertexCount;

            public int[] VertexIDRange
            {
                get
                {
                    int max = 0,
                    min = int.MaxValue;
                    foreach (var vertex in Vertices)
                    {
                        min = Math.Min(min, vertex.VertexID);
                        max = Math.Max(max, vertex.VertexID);
                    }
                    return new int[]
                    {
                        min,
                        max
                    };
                }
            }

            public Vertex[] Vertices;

            public MeshGroup()
            {
            }

            public MeshGroup(BinaryReader reader)
            {
                VertexCount = reader.ReadInt32();
                Vertices = new Vertex[VertexCount];
                for (var i = 0; i < VertexCount; i++)
                {
                    Vertices[i] = new Vertex(reader);
                }
                FaceCount = reader.ReadInt32();
                FacePoints = new FacePoint[FaceCount * 3];
                for (var i = 0; i < FaceCount * 3; i++)
                {
                    FacePoints[i] = new FacePoint(reader);
                }
                GeostateCount = reader.ReadInt32();
                mNameLength = reader.ReadByte();
                mMeshName = new char[(int)mNameLength];
                mMeshName = reader.ReadChars((int)mNameLength);
            }

            public MeshGroup(GEOM baseMesh, string meshName)
            {
                if (!baseMesh.IsValid || !baseMesh.IsBase)
                {
                    throw new WSOException("Input base mesh is not valid!");
                }
                VertexCount = baseMesh.VertexCount;
                Vertices = new Vertex[VertexCount];
                var index = Array.IndexOf<uint>(baseMesh.BoneHashList, 1468550073);
                if (index < 0)
                {
                    index = baseMesh.BoneHashList.Length;
                }
                for (var i = 0; i < VertexCount; i++)
                {
                    var tagValue = 0u;
                    if (baseMesh.HasTags)
                    {
                        tagValue = baseMesh.GetTagValue(i);
                    }
                    var bones = baseMesh.GetBones(i);
                    var tempBones = new int[bones.Length];
                    var boneWeights = baseMesh.GetBoneWeightsV5(i);
                    var tempWeights = new float[boneWeights.Length];
                    for (var j = 0; j < bones.Length; j++)
                    {
                        tempBones[j] = (int)bones[j];
                        tempWeights[j] = boneWeights[j] * 100;
                        if (tempWeights[j] < .5)
                        {
                            tempBones[j] = index;
                            tempWeights[j] = 0;
                        }
                    }
                    var totalWeight = 0f;
                    for (var j = 0; j < bones.Length; j++)
                    {
                        totalWeight += tempWeights[j];
                    }
                    if (!IsEqual(totalWeight, 100))
                    {
                        tempWeights[0] += 100 - totalWeight;
                    }
                    var vertexID = 0;
                    if (baseMesh.HasVertexIDs)
                    {
                        vertexID = baseMesh.GetVertexID(i);
                    }
                    Vertices[i] = new Vertex(baseMesh.GetPosition(i), vertexID, tagValue, tempBones, tempWeights);
                }
                FaceCount = baseMesh.FaceCount;
                FacePoints = new FacePoint[FaceCount * 3];
                for (var i = 0; i < FaceCount; i++)
                {
                    var indices = baseMesh.GetFaceIndices(i);
                    for (var j = 0; j < 3; j++)
                    {
                        FacePoints[i * 3 + j] = new FacePoint(indices[j], baseMesh.GetNormal(indices[j]), baseMesh.GetUV(indices[j], 0));
                    }
                }
                GeostateCount = 0;
                mMeshName = meshName.ToCharArray();
                mNameLength = (byte)mMeshName.Length;
            }

            public MeshGroup(GEOM baseMesh, GEOM morphMesh, string meshName)
            {
                if (!baseMesh.IsValid || !baseMesh.IsBase)
                {
                    throw new WSOException("Input base mesh is not valid!");
                }
                if (!morphMesh.IsValid || !morphMesh.IsMorph)
                {
                    throw new WSOException("Input morph mesh " + meshName + " is not valid!");
                }
                VertexCount = baseMesh.VertexCount;
                Vertices = new Vertex[VertexCount];
                var index = Array.IndexOf<uint>(baseMesh.BoneHashList, 1468550073);
                if (index < 0)
                {
                    index = baseMesh.BoneHashList.Length;
                }
                for (var i = 0; i < VertexCount; i++)
                {
                    var tagValue = 0u;
                    if (baseMesh.HasTags)
                    {
                        tagValue = baseMesh.GetTagValue(i);
                    }
                    var bones = baseMesh.GetBones(i);
                    var tempBones = new int[bones.Length];
                    var boneWeights = baseMesh.GetBoneWeightsV5(i);
                    var tempWeights = new float[boneWeights.Length];
                    for (var j = 0; j < bones.Length; j++)
                    {
                        tempBones[j] = (int)bones[j];
                        tempWeights[j] = boneWeights[j] * 100;
                        if (tempWeights[j] < .5)
                        {
                            tempBones[j] = index;
                            tempWeights[j] = 0;
                        }
                    }
                    var totalWeight = 0f;
                    for (var j = 0; j < bones.Length; j++)
                    {
                        totalWeight += tempWeights[j];
                    }
                    if (!IsEqual(totalWeight, 100))
                    {
                        tempWeights[0] += 100 - totalWeight;
                    }
                    var vertexID = 0;
                    if (baseMesh.HasVertexIDs)
                    {
                        vertexID = baseMesh.GetVertexID(i);
                    }
                    float[] basePosition = baseMesh.GetPosition(i),
                    morphDelta = morphMesh.GetPosition(i),
                    morphPosition = new float[basePosition.Length];
                    for (var j = 0; j < basePosition.Length; j++)
                    {
                        morphPosition[j] = basePosition[j] + morphDelta[j];
                    }
                    Vertices[i] = new Vertex(morphPosition, vertexID, tagValue, tempBones, tempWeights);
                }
                FaceCount = baseMesh.FaceCount;
                FacePoints = new FacePoint[FaceCount * 3];
                for (var i = 0; i < FaceCount; i++)
                {
                    var indices = baseMesh.GetFaceIndices(i);
                    for (var j = 0; j < 3; j++)
                    {
                        float[] baseNormal = baseMesh.GetNormal(indices[j]),
                        morphDelta = morphMesh.GetNormal(indices[j]),
                        morphNormal = new float[baseNormal.Length];
                        for (var k = 0; k < baseNormal.Length; k++)
                        {
                            morphNormal[k] = baseNormal[k] + morphDelta[k];
                        }
                        FacePoints[i * 3 + j] = new FacePoint(indices[j], morphNormal, baseMesh.GetUV(indices[j], 0));
                    }
                }
                GeostateCount = 0;
                mMeshName = meshName.ToCharArray();
                mNameLength = (byte)mMeshName.Length;
            }

            public MeshGroup(MeshGroup mesh, MeshName meshName) : this(mesh, Enum.GetName(typeof(MeshName), meshName))
            {
            }

            public MeshGroup(MeshGroup mesh, string meshName)
            {
                VertexCount = mesh.VertexCount;
                Vertices = new Vertex[VertexCount];
                for (var i = 0; i < VertexCount; i++)
                {
                    Vertices[i] = new Vertex(mesh.Vertices[i]);
                }
                FaceCount = mesh.FaceCount;
                FacePoints = new FacePoint[FacePointCount];
                for (var i = 0; i < FacePointCount; i++)
                {
                    FacePoints[i] = new FacePoint(mesh.FacePoints[i]);
                }
                GeostateCount = mesh.GeostateCount;
                mMeshName = meshName.ToCharArray();
                mNameLength = (byte)mMeshName.Length;
            }

            public MeshGroup(OBJ obj, string meshName, List<int[]> vertices, List<int[]> faces)
            {
                VertexCount = vertices.Count;
                Vertices = new Vertex[VertexCount];
                for (var i = 0; i < vertices.Count; i++)
                {
                    var tagValue = 0u;
                    var boneAssignments = new int[4];
                    var boneWeights = new float[4];
                    var vertexID = 0;
                    Vertices[i] = new Vertex(obj.VertexArray[vertices[i][0] - 1].Coordinates, vertexID, tagValue, boneAssignments, boneWeights);
                }
                FaceCount = faces.Count;
                FacePoints = new FacePoint[FaceCount * 3];
                for (var i = 0; i < faces.Count; i++)
                {
                    for (var j = 0; j < 3; j++)
                    {
                        FacePoints[i * 3 + j] = new FacePoint(faces[i][j], obj.NormalArray[vertices[faces[i][j]][2] - 1].Coordinates, obj.UVArray[vertices[faces[i][j]][1] - 1].Coordinates, true);
                    }
                }
                GeostateCount = 0;
                mMeshName = meshName.ToCharArray();
                mNameLength = (byte)mMeshName.Length;
            }

            public MeshGroup(int vertexCount, Vertex[] vertices, int faceCount, FacePoint[] facePoints, int geostateCount, string meshName)
            {
                VertexCount = vertexCount;
                Vertices = vertices;
                FaceCount = faceCount;
                FacePoints = facePoints;
                GeostateCount = geostateCount;
                mMeshName = meshName.ToCharArray();
                mNameLength = (byte)mMeshName.Length;
            }

            public void AppendMeshGroup(MeshGroup meshGroupToAppend, string[] oldBoneArray, string[] newBoneArray)
            {
                var vertices = new Vertex[VertexCount + meshGroupToAppend.VertexCount];
                var index = Array.IndexOf(newBoneArray, Enum.GetName(typeof(BoneHash), BoneHash.b__ROOT_bind__));
                for (var i = 0; i < VertexCount; i++)
                {
                    vertices[i] = Vertices[i];
                }
                for (var i = 0; i < meshGroupToAppend.VertexCount; i++)
                {
                    int[] bones = meshGroupToAppend.Vertices[i].BoneAssignments,
                    boneIndices = new int[bones.Length];
                    for (var j = 0; j < bones.Length; j++)
                    {
                        if (bones[j] >= 0 && bones[j] < oldBoneArray.Length)
                        {
                            boneIndices[j] = Array.IndexOf(newBoneArray, oldBoneArray[bones[j]]);
                        }
                        else
                        {
                            boneIndices[j] = index;
                        }
                    }
                    vertices[i + VertexCount] = new Vertex(meshGroupToAppend.Vertices[i].Position, meshGroupToAppend.Vertices[i].VertexID, meshGroupToAppend.Vertices[i].TagValue, boneIndices, meshGroupToAppend.Vertices[i].BoneWeights);
                }
                Vertices = vertices;
                var facePoints = new FacePoint[FacePointCount + meshGroupToAppend.FacePointCount];
                for (var i = 0; i < FacePointCount; i++)
                {
                    facePoints[i] = FacePoints[i];
                }
                for (var i = 0; i < meshGroupToAppend.FacePointCount; i++)
                {
                    facePoints[i + FacePointCount] = new FacePoint((int)meshGroupToAppend.FacePoints[i].VertexIndex + VertexCount, meshGroupToAppend.FacePoints[i].Normals, meshGroupToAppend.FacePoints[i].UVs);
                }
                FacePoints = facePoints;
                VertexCount += meshGroupToAppend.VertexCount;
                FaceCount += meshGroupToAppend.FaceCount;
            }

            public bool ApplyExtendedVertices(VertexExtended[] extendedVertices, bool applyNormals, bool applyUVs)
            {
                if (VertexCount != extendedVertices.Length)
                {
                    return false;
                }
                for (var i = 0; i < VertexCount; i++)
                {
                    Vertices[i] = new Vertex(extendedVertices[i].Position, extendedVertices[i].VertexID, extendedVertices[i].TagValue, extendedVertices[i].BoneAssignments, extendedVertices[i].BoneWeights);
                }
                for (var i = 0; i < FaceCount * 3; i++)
                {
                    var vertexIndex = (int)FacePoints[i].VertexIndex;
                    if (vertexIndex >= VertexCount)
                    {
                        return false;
                    }
                    if (applyNormals)
                    {
                        FacePoints[i].Normals = extendedVertices[vertexIndex].GetNormals();
                    }
                    if (applyUVs)
                    {
                        FacePoints[i].UVs = extendedVertices[vertexIndex].GetUVs();
                    }
                }
                return true;
            }

            public VertexExtended[] GetExtendedVertices()
            {
                var extendedVertices = new VertexExtended[VertexCount];
                for (var i = 0; i < VertexCount; i++)
                {
                    extendedVertices[i] = new VertexExtended(GetVertex(i));
                }
                for (var i = 0; i < FaceCount * 3; i++)
                {
                    var facePoint = GetFacePoint(i);
                    extendedVertices[(int)facePoint.VertexIndex].SetNormals(facePoint.Normals);
                    extendedVertices[(int)facePoint.VertexIndex].SetUVs(facePoint.UVs);
                }
                return extendedVertices;
            }

            public FacePoint GetFacePoint(int index)
            {
                return FacePoints[index];
            }

            public Vertex GetVertex(int index)
            {
                return Vertices[index];
            }

            public void SetFacePointNormal(int index, float[] normal)
            {
                FacePoints[index].Normals = new float[]
                    {
                        normal[0],
                        normal[1],
                        normal[2]
                    };
            }

            public void SetVertexPosition(int index, float[] position)
            {
                Vertices[index].Position = new float[]
                    {
                        position[0],
                        position[1],
                        position[2]
                    };
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(VertexCount);
                for (var i = 0; i < VertexCount; i++)
                {
                    Vertices[i].Write(writer);
                }
                writer.Write(FaceCount);
                for (var i = 0; i < FaceCount * 3; i++)
                {
                    FacePoints[i].Write(writer);
                }
                writer.Write(GeostateCount);
                writer.Write(mNameLength);
                writer.Write(mMeshName);
            }
        }

        public class Vertex
        {
            protected int[] mBoneAssignments = new int[4];

            protected float[] mBoneWeights = new float[4];

            public int[] BoneAssignments
            {
                get
                {
                    return mBoneAssignments;
                }
                set
                {
                    for (var i = 0; i < 4; i++)
                    {
                        mBoneAssignments[i] = value[i];
                    }
                }
            }

            public float[] BoneWeights
            {
                get
                {
                    return mBoneWeights;
                }
                set
                {
                    for (var i = 0; i < 4; i++)
                    {
                        mBoneWeights[i] = value[i];
                    }
                }
            }

            public float[] Position
            {
                get
                {
                    return new float[]
                    {
                        X,
                        Y,
                        Z
                    };
                }
                set
                {
                    X = value[0];
                    Y = value[1];
                    Z = value[2];
                }
            }

            public uint TagValue;

            public int VertexID;

            public float X, Y, Z;

            public Vertex()
            {
            }

            public Vertex(float[] position, int vertexID, uint tagValue, int[] boneAssignments, float[] boneWeights)
            {
                X = position[0];
                Y = position[1];
                Z = position[2];
                VertexID = vertexID;
                TagValue = tagValue;
                mBoneAssignments = boneAssignments;
                mBoneWeights = boneWeights;
            }

            public Vertex(BinaryReader reader)
            {
                X = reader.ReadSingle();
                Y = reader.ReadSingle();
                Z = reader.ReadSingle();
                VertexID = reader.ReadInt32();
                TagValue = reader.ReadUInt32();
                for (var i = 0; i < 4; i++)
                {
                    mBoneAssignments[i] = reader.ReadInt32();
                }
                for (var i = 0; i < 4; i++)
                {
                    mBoneWeights[i] = reader.ReadSingle();
                }
            }

            public Vertex(Vertex vertex)
            {
                X = vertex.X;
                Y = vertex.Y;
                Z = vertex.Z;
                VertexID = vertex.VertexID;
                TagValue = vertex.TagValue;
                mBoneAssignments = new int[vertex.mBoneAssignments.Length];
                mBoneWeights = new float[vertex.mBoneWeights.Length];
                for (var i = 0; i < vertex.mBoneAssignments.Length; i++)
                {
                    mBoneAssignments[i] = vertex.mBoneAssignments[i];
                    mBoneWeights[i] = vertex.mBoneWeights[i];
                }
            }

            public override string ToString()
            {
                return string.Concat(new string[] {
                    VertexID.ToString(),
                    ", X: ",
                    X.ToString(),
                    ", Y: ",
                    Y.ToString(),
                    ", Z: ",
                    Z.ToString(),
                    ", tag: ",
                    TagValue.ToString(),
                    ", Bones: ",
                    mBoneAssignments[0].ToString(),
                    ":",
                    mBoneWeights[0].ToString(),
                    ", ",
                    mBoneAssignments[1].ToString(),
                    ":",
                    mBoneWeights[1].ToString(),
                    ", ",
                    mBoneAssignments[2].ToString(),
                    ":",
                    mBoneWeights[2].ToString(),
                    ", ",
                    mBoneAssignments[3].ToString(),
                    ":",
                    mBoneWeights[3].ToString()
                });
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(X);
                writer.Write(Y);
                writer.Write(Z);
                writer.Write(VertexID);
                writer.Write(TagValue);
                for (var i = 0; i < 4; i++)
                {
                    writer.Write(mBoneAssignments[i]);
                }
                for (var i = 0; i < 4; i++)
                {
                    writer.Write(mBoneWeights[i]);
                }
            }
        }

        public class VertexExtended : Vertex
        {
            public float NormalX, NormalY, NormalZ, U, V;

            public VertexExtended()
            {
            }

            public VertexExtended(Vertex vertex)
            {
                X = vertex.Position[0];
                Y = vertex.Position[1];
                Z = vertex.Position[2];
                VertexID = vertex.VertexID;
                TagValue = vertex.TagValue;
                mBoneAssignments = vertex.BoneAssignments;
                mBoneWeights = vertex.BoneWeights;
            }

            public float[] GetNormals()
            {
                return new float[]
                {
                    NormalX,
                    NormalY,
                    NormalZ
                };
            }

            public float[] GetPosition()
            {
                return new float[]
                {
                    X,
                    Y,
                    Z
                };
            }

            public float[] GetUVs()
            {
                return new float[]
                {
                    U,
                    V
                };
            }

            public void SetNormals(float[] normals)
            {
                NormalX = normals[0];
                NormalY = normals[1];
                NormalZ = normals[2];
            }

            public void SetPosition(float[] position)
            {
                X = position[0];
                Y = position[1];
                Z = position[2];
            }

            public void SetUVs(float[] UVs)
            {
                U = UVs[0];
                V = UVs[1];
            }
        }

        [Serializable]
        public class WSOException : ApplicationException
        {
            public WSOException()
            {
            }

            public WSOException(string message) : base(message)
            {
            }

            public WSOException(string message, Exception inner) : base(message, inner)
            {
            }

            protected WSOException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
            {
            }
        }

        public WSO()
        {
        }

        public WSO(GEOM baseMesh, GEOM[] morphs) : this(baseMesh, Array.FindAll(morphs, x => x.IsValid), Array.FindAll(morphs, x => x.IsValid).Length == 1)
        {
        }

        public WSO(GEOM baseMesh, GEOM[] morphs, bool group0) : this(baseMesh, morphs.Length > 0 ? morphs[0] : null, morphs.Length > 1 ? morphs[1] : null, morphs.Length > 2 ? morphs[2] : null, morphs.Length > 3 ? morphs[3] : null, group0)
        {
        }

        public WSO(GEOM baseMesh, GEOM fatMorph, GEOM fitMorph, GEOM thinMorph, GEOM specialMorph, bool group0)
        {
            mVersion = 4;
            var count = 1;
            if (fatMorph != null)
            {
                count++;
            }
            if (thinMorph != null)
            {
                count++;
            }
            if (fitMorph != null)
            {
                count++;
            }
            if (specialMorph != null)
            {
                count++;
            }
            mMeshCount = count;
            mMeshes = new MeshGroup[count];
            if (group0)
            {
                mMeshes[0] = new MeshGroup(baseMesh, "group_0");
            }
            else
            {
                mMeshes[0] = new MeshGroup(baseMesh, "group_base");
            }
            count = 1;
            if (fatMorph != null)
            {
                mMeshes[count] = new MeshGroup(baseMesh, fatMorph, "group_fat");
                count++;
            }
            if (fitMorph != null)
            {
                mMeshes[count] = new MeshGroup(baseMesh, thinMorph, "group_thin");
                count++;
            }
            if (thinMorph != null)
            {
                mMeshes[count] = new MeshGroup(baseMesh, fitMorph, "group_fit");
                count++;
            }
            if (specialMorph != null)
            {
                mMeshes[count] = new MeshGroup(baseMesh, specialMorph, "group_special");
                count++;
            }
            mBoneCount = baseMesh.BoneHashList.Length;
            var hasRootBind = Array.IndexOf(baseMesh.BoneHashList, BoneHash.b__ROOT_bind__) > -1;
            if (!hasRootBind)
            {
                mBoneCount++;
            }
            mBones = new Bone[mBoneCount];
            for (var i = 0; i < baseMesh.BoneHashList.Length; i++)
            {
                mBones[i] = new Bone(baseMesh.BoneHashList[i]);
            }
            if (!hasRootBind)
            {
                mBones[baseMesh.BoneHashList.Length] = new Bone((uint)BoneHash.b__ROOT_bind__);
            }
        }

        public WSO(s3pi.GenericRCOLResource.GenericRCOLResource mlodResource, Rig rig, MeshGroup[] meshGroups)
        {
            mVersion = 4;
            mMeshCount = meshGroups.Length;
            mMeshes = meshGroups;
            var mlod = mlodResource.ChunkEntries.Find(x => x.RCOLBlock.Tag == "MLOD").RCOLBlock as meshExpImp.ModelBlocks.MLOD;
            var bones = new List<Bone>();
            foreach (var mesh in mlod.Meshes)
            {
                var skinController = mlodResource.ChunkEntries[mesh.SkinControllerIndex.TGIBlockIndex + mlodResource.PublicChunks].RCOLBlock as meshExpImp.ModelBlocks.SKIN;
                if (skinController != null && rig != null)
                {
                    foreach (var bone in skinController.Bones)
                    {
                        bones.Add(new Bone(Array.Find(rig.Bones, x => x.BoneHash == bone.NameHash).BoneName));
                    }
                }
            }
            if (!bones.Exists(x => x.Name == Enum.GetName(typeof(BoneHash), BoneHash.b__ROOT_bind__)))
            {
                bones.Add(new Bone((uint)BoneHash.b__ROOT_bind__));
            }
            mBoneCount = bones.Count;
            mBones = bones.ToArray();
        }

        public WSO(BinaryReader reader)
        {
            Read(reader);
        }

        public void AppendMesh(WSO meshToAppend)
        {
            if (meshToAppend.MeshCount != MeshCount)
            {
                throw new WSOException("Meshes do not have the same number of groups/morphs!");
            }
            var matched = true;
            for (var i = 0; i < MeshCount; i++)
            {
                if (Array.IndexOf(meshToAppend.MeshNames, mMeshes[i].MeshName) < 0)
                {
                    matched = false;
                }
            }
            if (!matched)
            {
                throw new WSOException("Meshes do not have the same mesh groups");
            }
            var tempBones = new List<Bone>();
            for (var i = 0; i < mBones.Length; i++)
            {
                tempBones.Add(mBones[i]);
            }
            for (var i = 0; i < meshToAppend.mBones.Length; i++)
            {
                if (Array.IndexOf(BoneNameList, meshToAppend.mBones[i].Name) < 0)
                {
                    tempBones.Add(meshToAppend.mBones[i]);
                }
            }
            mBones = tempBones.ToArray();
            for (var i = 0; i < MeshCount; i++)
            {
                mMeshes[i].AppendMeshGroup(meshToAppend.mMeshes[Array.IndexOf(meshToAppend.MeshNames, mMeshes[i].MeshName)], meshToAppend.BoneNameList, BoneNameList);
            }
        }

        public void AutoBone(WSO refMesh, bool unassignedVerticesOnly, bool interpolate, int interpolationPoints, float weightingFactor, bool restrictToFace, Action updateProgressCallback = null)
        {
            int emptyBone;
            string[] newBoneNameList,
            refBoneNameList = refMesh.BoneNameList;
            Bone[] newBoneList;
            if (unassignedVerticesOnly)
            {
                var tempBones = new List<Bone>(BoneList);
                var tempBoneNames = new List<string>(BoneNameList);
                for (var i = 0; i < refMesh.BoneNameList.Length; i++)
                {
                    if (Array.IndexOf(BoneNameList, refBoneNameList[i]) < 0)
                    {
                        tempBones.Add(refMesh.BoneList[i]);
                        tempBoneNames.Add(refMesh.BoneName(i));
                    }
                }
                newBoneNameList = tempBoneNames.ToArray();
                newBoneList = tempBones.ToArray();
                emptyBone = EmptyBoneIndex;
            }
            else
            {
                newBoneNameList = refMesh.BoneNameList;
                newBoneList = refMesh.BoneList;
                emptyBone = refMesh.EmptyBoneIndex;
            }
            BoneList = newBoneList;
            var refVertices = new Vector3[refMesh.Base.VertexCount];
            for (var i = 0; i < refVertices.Length; i++)
            {
                refVertices[i] = new Vector3(refMesh.Base.GetVertex(i).Position);
            }
            var refFaces = new int[refMesh.Base.FaceCount][];
            for (var i = 0; i < refMesh.Base.FaceCount; i++)
            {
                refFaces[i] = new int[]
                    {
                        (int)refMesh.Base.GetFacePoint(i * 3).VertexIndex,
                        (int)refMesh.Base.GetFacePoint(i * 3 + 1).VertexIndex,
                        (int)refMesh.Base.GetFacePoint(i * 3 + 2).VertexIndex
                    };
            }
            int baseIndex = BaseIndex,
            stepIt = 0;
            for (var i = 0; i < Base.VertexCount; i++)
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
                if (unassignedVerticesOnly && ValidBones(baseIndex, i))
                {
                    continue;
                }
                var position = new Vector3(Base.GetVertex(i).Position);
                var refPoints = position.GetReferenceMeshPoints(refVertices, refFaces, interpolate, restrictToFace, interpolationPoints);
                var refArray = new Vector3[refPoints.Length];
                for (var j = 0; j < refPoints.Length; j++)
                {
                    refArray[j] = new Vector3(refMesh.Base.GetVertex(refPoints[j]).Position);
                }
                var newBones = new List<int>();
                var newWeights = new List<float>();
                var valueWeights = position.GetInterpolationWeights(refArray, weightingFactor);
                for (var j = 0; j < refPoints.Length; j++)
                {
                    var refBones = refMesh.Base.GetVertex(refPoints[j]).BoneAssignments;
                    var refWeights = refMesh.Base.GetVertex(refPoints[j]).BoneWeights;
                    for (var k = 0; k < refBones.Length; k++)
                    {
                        if (refWeights[k] > 0 && refBones[k] < refBoneNameList.Length && refBones[k] >= 0)
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
                    newBones.Add(emptyBone);
                    newWeights.Add(0);
                }
                for (var j = 0; j < 4; j++)
                {
                    if (newBones[j] < refBoneNameList.Length && newBones[j] >= 0 && newWeights[j] > 0)
                    {
                        newBones[j] = Array.IndexOf(newBoneNameList, refBoneNameList[newBones[j]]);
                    }
                    else
                    {
                        newBones[j] = emptyBone;
                        newWeights[j] = 0;
                    }
                }
                for (var j = 0; j < MeshCount; j++)
                {
                    Mesh(j).GetVertex(i).BoneAssignments = newBones.GetRange(0, 4).ToArray();
                    Mesh(j).GetVertex(i).BoneWeights = newWeights.GetRange(0, 4).ToArray();
                }
            }
        }

        public void AutoMorph(WSO refMesh, bool interpolate, int interpolationPoints, bool restrictToFace, bool doFat, bool doThin, bool doFit, bool doSpecial, float weightingFactor, Action updateProgressCallback = null)
        {
            var refPoints = new int[Base.VertexCount][];
            var refVertices = new Vector3[refMesh.Base.VertexCount];
            var valueWeights = new float[Base.VertexCount][];
            for (var i = 0; i < refVertices.Length; i++)
            {
                refVertices[i] = new Vector3(refMesh.Base.GetVertex(i).Position);
            }
            var refFaces = new int[refMesh.Base.FaceCount][];
            for (var i = 0; i < refMesh.Base.FaceCount; i++)
            {
                refFaces[i] = new int[]
                    {
                        (int)refMesh.Base.GetFacePoint(i * 3).VertexIndex,
                        (int)refMesh.Base.GetFacePoint(i * 3 + 1).VertexIndex,
                        (int)refMesh.Base.GetFacePoint(i * 3 + 2).VertexIndex
                    };
            }
            var stepIt = 0;
            for (var i = 0; i < Base.VertexCount; i++)
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
                var position = new Vector3(Base.GetVertex(i).Position);
                refPoints[i] = position.GetReferenceMeshPoints(refVertices, refFaces, interpolate, restrictToFace, interpolationPoints);
                var refArray = new Vector3[refPoints[i].Length];
                for (var j = 0; j < refPoints[i].Length; j++)
                {
                    refArray[j] = new Vector3(refMesh.Base.GetVertex(refPoints[i][j]).Position);
                }
                valueWeights[i] = position.GetInterpolationWeights(refArray, weightingFactor);
            }
            if (doFat)
            {
                Fat = MakeWSOAutoMorph(Base, refMesh.Base, refMesh.Fat, refPoints, valueWeights, MeshName.group_fat);
                if (updateProgressCallback != null)
                {
                    updateProgressCallback();
                }
            }
            if (doThin)
            {
                Thin = MakeWSOAutoMorph(Base, refMesh.Base, refMesh.Thin, refPoints, valueWeights, MeshName.group_thin);
                if (updateProgressCallback != null)
                {
                    updateProgressCallback();
                }
            }
            if (doFit)
            {
                Fit = MakeWSOAutoMorph(Base, refMesh.Base, refMesh.Fit, refPoints, valueWeights, MeshName.group_fit);
                if (updateProgressCallback != null)
                {
                    updateProgressCallback();
                }
            }
            if (doSpecial)
            {
                Special = MakeWSOAutoMorph(Base, refMesh.Base, refMesh.Special, refPoints, valueWeights, MeshName.group_special);
                if (updateProgressCallback != null)
                {
                    updateProgressCallback();
                }
            }
        }

        public void AutoUV(WSO refMesh, bool unassignedOnly, float weightingFactor, Action updateProgressCallback = null)
        {
            var refVertices = new Vector3[refMesh.Base.VertexCount];
            for (var i = 0; i < refVertices.Length; i++)
            {
                refVertices[i] = new Vector3(refMesh.Base.GetVertex(i).Position);
            }
            var refVerticesExtended = refMesh.Base.GetExtendedVertices();
            var refFaces = new int[refMesh.Base.FaceCount][];
            for (var i = 0; i < refMesh.Base.FaceCount; i++)
            {
                refFaces[i] = new int[]
                    {
                        (int)refMesh.Base.GetFacePoint(i * 3).VertexIndex,
                        (int)refMesh.Base.GetFacePoint(i * 3 + 1).VertexIndex,
                        (int)refMesh.Base.GetFacePoint(i * 3 + 2).VertexIndex
                    };
            }
            var refFaceRefs = new int[refMesh.Base.VertexCount][];
            for (var i = 0; i < refVertices.Length; i++)
            {
                var temp = new List<int>();
                for (var j = 0; j < refFaces.Length; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        if (i == refFaces[j][k])
                        {
                            temp.Add(j);
                        }
                    }
                }
                refFaceRefs[i] = temp.ToArray();
            }
            var stepIt = 0;
            for (var i = 0; i < Base.FacePointCount; i++)
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
                if (!(unassignedOnly && (Base.FacePoints[i].UVs[0] > 0 || Base.FacePoints[i].UVs[1] > 0)))
                {
                    var currentVertexFaces = new List<Triangle>();
                    var position = new Vector3(Base.Vertices[(int)Base.FacePoints[i].VertexIndex].Position);
                    for (var j = 0; j < Base.FaceCount; j++)
                    {
                        if (Base.GetFacePoint(j * 3).VertexIndex == Base.FacePoints[i].VertexIndex || Base.GetFacePoint(j * 3 + 1).VertexIndex == Base.FacePoints[i].VertexIndex || Base.GetFacePoint(j * 3 + 2).VertexIndex == Base.FacePoints[i].VertexIndex)
                        {
                            currentVertexFaces.Add(new Triangle(Base.Vertices[(int)Base.FacePoints[j * 3].VertexIndex].Position, Base.Vertices[(int)Base.FacePoints[j * 3 + 1].VertexIndex].Position, Base.Vertices[(int)Base.FacePoints[j * 3 + 2].VertexIndex].Position));
                        }
                    }
                    var refPoints = position.GetFaceReferenceMeshPoints(refVertices, refFaces, refFaceRefs, currentVertexFaces.ToArray(), 5);
                    var refArray = new Vector3[refPoints.Length];
                    for (var j = 0; j < refPoints.Length; j++)
                    {
                        refArray[j] = new Vector3(refMesh.Base.GetVertex(refPoints[j]).Position);
                    }
                    float[] interpolationWeights = position.GetInterpolationWeights(refArray, weightingFactor),
                    newUV = new float[2];
                    for (var j = 0; j < refPoints.Length; j++)
                    {
                        var refUVs = refVerticesExtended[refPoints[j]].GetUVs();
                        for (var k = 0; k < refUVs.Length; k++)
                        {
                            newUV[k] += interpolationWeights[j] * refUVs[k];
                        }
                    }
                    for (var j = 0; j < MeshCount; j++)
                    {
                        Mesh(j).FacePoints[i].UVs = newUV;
                    }
                }
            }
        }

        public string BoneName(int index)
        {
            return mBones[index].ToString();
        }

        public int BoneScan()
        {
            bool badBone = false,
            tooManyBones = false;
            var maxBone = 0;
            for (var i = 0; i < mMeshCount; i++)
            {
                for (var j = 0; j < Mesh(i).VertexCount; j++)
                {
                    var boneAssignments = Mesh(i).GetVertex(j).BoneAssignments;
                    var boneWeights = Mesh(i).GetVertex(j).BoneWeights;
                    for (var k = 0; k < 4; k++)
                    {
                        if (boneWeights[k] > 0 && boneAssignments[k] > maxBone)
                        {
                            maxBone = boneAssignments[k];
                        }
                    }
                    if (!ValidBones(i, j))
                    {
                        badBone = true;
                    }
                }
                if (Mesh(i).BonesUsedCount > 60)
                {
                    tooManyBones = true;
                }
            }
            var result = 0;
            if (maxBone > mBoneCount)
            {
                result += 8;
            }
            if (badBone)
            {
                result += 2;
            }
            if (tooManyBones)
            {
                result += 16;
            }
            return result;
        }

        public void FixBoneWeights()
        {
            for (var i = 0; i < mMeshCount; i++)
            {
                for (var j = 0; j < Mesh(i).VertexCount; j++)
                {
                    var boneWeights = Mesh(i).GetVertex(j).BoneWeights;
                    var totalWeight = 0;
                    for (var k = 0; k < 4; k++)
                    {
                        boneWeights[k] = (float)Math.Round((double)boneWeights[k]);
                        totalWeight += (int)boneWeights[k];
                    }
                    for (var k = 0; k < 4; k++)
                    {
                        if (boneWeights[k] >= (float)totalWeight - 100)
                        {
                            boneWeights[0] += 100 - (float)totalWeight;
                            break;
                        }
                    }
                    Mesh(i).GetVertex(j).BoneWeights = boneWeights;
                }
            }
        }

        public Bone GetBone(int index)
        {
            return mBones[index];
        }

        public static bool IsEqual(float x, float y)
        {
            return Math.Abs(x - y) <= (Math.Abs(x) + Math.Abs(y)) / 2000000;
        }

        public MeshGroup MakeWSOAutoMorph(MeshGroup baseMesh, MeshGroup refBase, MeshGroup refMorph, int[][] refPoints, float[][] valueWeights, MeshName name)
        {
            VertexExtended[] refBaseVertices = refBase.GetExtendedVertices(),
            refMorphVertices = refMorph.GetExtendedVertices();
            var newMorph = new MeshGroup(baseMesh, name);
            var deltaNormals = new Vector3[newMorph.VertexCount];
            for (var i = 0; i < newMorph.VertexCount; i++)
            {
                var newPosition = new Vector3(newMorph.GetVertex(i).Position);
                for (var j = 0; j < refPoints[i].Length; j++)
                {
                    newPosition += valueWeights[i][j] * (new Vector3(refMorph.GetVertex(refPoints[i][j]).Position) - new Vector3(refBase.GetVertex(refPoints[i][j]).Position));
                    deltaNormals[i] += valueWeights[i][j] * (new Vector3(refMorphVertices[refPoints[i][j]].GetNormals()) - new Vector3(refBaseVertices[refPoints[i][j]].GetNormals()));
                }
                newMorph.SetVertexPosition(i, newPosition.Coordinates);
            }
            for (var i = 0; i < newMorph.FacePointCount; i++)
            {
                newMorph.SetFacePointNormal(i, (new Vector3(newMorph.GetFacePoint(i).Normals) + deltaNormals[(int)newMorph.GetFacePoint(i).VertexIndex]).Coordinates);
            }
            return newMorph;
        }

        public MeshGroup Mesh(string name)
        {
            var index = Array.IndexOf(MeshNames, name);
            if (index > -1)
            {
                return mMeshes[index];
            }
            return null;
        }

        public MeshGroup Mesh(int index)
        {
            if (index < mMeshes.Length)
            {
                return mMeshes[index];
            }
            return null;
        }

        public int MeshIndex(string name)
        {
            return Array.IndexOf(MeshNames, name);
        }

        public bool MorphMatch()
        {
            if (!Base.HasValidIDs)
            {
                throw new WSOException("Mesh does not have valid ID numbering and morphs cannot be matched!");
            }
            var baseMesh = Base;
            var newMorphs = new MeshGroup[mMeshCount - 1];
            for (var i = 0; i < mMeshCount - 1; i++)
            {
                var meshGroup = Mesh(i + 1);
                VertexExtended[] oldMorphVertices = meshGroup.GetExtendedVertices(),
                newMorphVertices = new VertexExtended[baseMesh.VertexCount];
                var oldMorphVertexIDs = new int[meshGroup.VertexCount];
                for (var j = 0; j < meshGroup.VertexCount; j++)
                {
                    oldMorphVertexIDs[j] = meshGroup.GetVertex(j).VertexID;
                }
                var startIndex = 0;
                for (var j = 0; j < baseMesh.VertexCount; j++)
                {
                    var index = Array.IndexOf<int>(oldMorphVertexIDs, baseMesh.GetVertex(j).VertexID, startIndex);
                    if (index > -1)
                    {
                        newMorphVertices[j] = oldMorphVertices[index];
                        startIndex = index;
                    }
                    else
                    {
                        index = Array.IndexOf<int>(oldMorphVertexIDs, baseMesh.GetVertex(j).VertexID);
                        if (index > -1)
                        {
                            newMorphVertices[j] = oldMorphVertices[index];
                        }
                        else
                        {
                            newMorphVertices[j] = new VertexExtended(baseMesh.GetVertex(j));
                            for (var k = 0; k < baseMesh.FaceCount; k++)
                            {
                                if ((int)baseMesh.GetFacePoint(k).VertexIndex == j)
                                {
                                    newMorphVertices[j].SetNormals(baseMesh.GetFacePoint(k).Normals);
                                    newMorphVertices[j].SetUVs(baseMesh.GetFacePoint(k).UVs);
                                }
                            }
                        }
                    }
                }
                newMorphs[i] = new MeshGroup(baseMesh, Mesh(i + 1).MeshName);
                if (!newMorphs[i].ApplyExtendedVertices(newMorphVertices, true, false))
                {
                    return false;
                }
            }
            for (var i = 1; i < MeshCount; i++)
            {
                mMeshes[i] = newMorphs[i - 1];
            }
            return true;
        }

        public void Read(BinaryReader reader)
        {
            mVersion = reader.ReadInt32();
            if (mVersion == 5)
            {
                var count = reader.ReadInt32();
                mSoftwareName = reader.ReadChars(count);
                mUnknown = reader.ReadInt32();
            }
            mMeshCount = reader.ReadInt32();
            mMeshes = new MeshGroup[mMeshCount];
            for (var i = 0; i < mMeshCount; i++)
            {
                mMeshes[i] = new MeshGroup(reader);
            }
            mBoneCount = reader.ReadInt32();
            mBones = new Bone[mBoneCount];
            for (var i = 0; i < mBoneCount; i++)
            {
                mBones[i] = new Bone(reader);
            }
        }

        public void ReplaceBones(WSO wso)
        {
            if (Base.VertexCount != wso.Base.VertexCount)
            {
                throw new WSOException("Source number of vertices does not equal target number of vertices!");
            }
            for (var i = 0; i < mMeshes.Length; i++)
            {
                var meshGroup = mMeshes[i];
                for (var j = 0; j < meshGroup.VertexCount; j++)
                {
                    meshGroup.Vertices[j].BoneAssignments = wso.Base.Vertices[j].BoneAssignments;
                    meshGroup.Vertices[j].BoneWeights = wso.Base.Vertices[j].BoneWeights;
                }
            }
            BoneList = wso.BoneList;
        }

        public void ReplaceBonesByID(WSO wso)
        {
            var bones = new List<Bone>(BoneList);
            var ids = new int[wso.Base.VertexCount];
            int index0 = 0,
            index1 = 0,
            maxID = 0,
            minID = 0;
            for (var i = 0; i < wso.Base.VertexCount; i++)
            {
                minID = Math.Max(minID, wso.Base.Vertices[i].VertexID);
                maxID = Math.Max(maxID, wso.Base.Vertices[i].VertexID);
                ids[i] = wso.Base.Vertices[i].VertexID;
            }
            if (maxID == 0 && minID == 0)
            {
                throw new WSOException("No vertex IDs in source mesh!");
            }
            minID = 0;
            maxID = 0;
            for (var i = 0; i < Base.VertexCount; i++)
            {
                minID = Math.Max(minID, Base.Vertices[i].VertexID);
                maxID = Math.Max(maxID, Base.Vertices[i].VertexID);
                if (VertexIDSearch(Base.Vertices[i].VertexID, ids, index0, out index1))
                {
                    int[] temp0 = wso.Base.Vertices[index1].BoneAssignments,
                    temp1 = new int[temp0.Length];
                    var boneWeights = wso.Base.Vertices[index1].BoneWeights;
                    for (var j = 0; j < temp0.Length; j++)
                    {
                        if (temp0[j] < 0)
                        {
                            temp1[j] = temp0[j];
                        }
                        else
                        {
                            temp1[j] = bones.IndexOf(wso.BoneList[temp0[j]]);
                            if (temp1[j] < 0)
                            {
                                if (boneWeights[j] > 0)
                                {
                                    temp1[j] = (int)((byte)bones.Count);
                                    bones.Add(wso.BoneList[temp0[j]]);
                                }
                                else
                                {
                                    temp1[j] = -1;
                                }
                            }
                        }
                    }
                    Base.Vertices[i].BoneAssignments = temp1;
                    Base.Vertices[i].BoneWeights = wso.Base.Vertices[index1].BoneWeights;
                    index0 = index1;
                }
            }
            if (maxID == 0 && minID == 0)
            {
                throw new WSOException("No vertex IDs in target mesh!");
            }
            BoneList = bones.ToArray();
        }

        public void ReplaceNormals(WSO wso)
        {
            if (Base.FacePointCount != wso.Base.FacePointCount)
            {
                throw new WSOException("Source number of faces does not equal target number of faces!");
            }
            for (var i = 0; i < mMeshes.Length; i++)
            {
                var meshGroup = mMeshes[i];
                for (var j = 0; j < meshGroup.FacePointCount; j++)
                {
                    meshGroup.FacePoints[j].Normals = wso.Base.FacePoints[j].Normals;
                }
            }
        }

        public void ReplaceNormalsByID(WSO wso)
        {
            throw new WSOException("Not yet implemented!");
        }

        public void ReplacePositions(WSO wso)
        {
            if (Base.VertexCount != wso.Base.VertexCount)
            {
                throw new WSOException("Source number of vertices does not equal target number of vertices!");
            }
            for (var i = 0; i < mMeshes.Length; i++)
            {
                var meshGroup = mMeshes[i];
                for (var j = 0; j < meshGroup.VertexCount; j++)
                {
                    meshGroup.Vertices[j].Position = wso.Base.Vertices[j].Position;
                }
            }
        }

        public void ReplacePositionsByID(WSO wso)
        {
            var ids = new int[wso.Base.VertexCount];
            int index0 = 0,
            index1 = 0,
            maxID = 0,
            minID = 0;
            for (var i = 0; i < wso.Base.VertexCount; i++)
            {
                minID = Math.Max(minID, wso.Base.Vertices[i].VertexID);
                maxID = Math.Max(maxID, wso.Base.Vertices[i].VertexID);
                ids[i] = wso.Base.Vertices[i].VertexID;
            }
            if (maxID == 0 && minID == 0)
            {
                throw new WSOException("No vertex IDs in source mesh!");
            }
            minID = 0;
            maxID = 0;
            for (var i = 0; i < Base.VertexCount; i++)
            {
                minID = Math.Max(minID, Base.Vertices[i].VertexID);
                maxID = Math.Max(maxID, Base.Vertices[i].VertexID);
                if (VertexIDSearch(Base.Vertices[i].VertexID, ids, index0, out index1))
                {
                    Base.Vertices[i].Position = wso.Base.Vertices[index1].Position;
                    index0 = index1;
                }
            }
            if (maxID == 0 && minID == 0)
            {
                throw new WSOException("No vertex IDs in target mesh!");
            }
        }

        public void ReplaceTagValues(WSO wso)
        {
            if (Base.VertexCount != wso.Base.VertexCount)
            {
                throw new WSOException("Source number of vertices does not equal target number of vertices!");
            }
            
            for (var i = 0; i < mMeshes.Length; i++)
            {
                var meshGroup = mMeshes[i];
                for (var j = 0; j < meshGroup.VertexCount; j++)
                {
                    meshGroup.Vertices[j].TagValue = wso.Base.Vertices[j].TagValue;
                }
            }
        }

        public void ReplaceTagValuesByID(WSO wso)
        {
            var ids = new int[wso.Base.VertexCount];
            int index0 = 0,
            index1 = 0,
            maxID = 0,
            minID = 0;
            for (var i = 0; i < wso.Base.VertexCount; i++)
            {
                minID = Math.Max(minID, wso.Base.Vertices[i].VertexID);
                maxID = Math.Max(maxID, wso.Base.Vertices[i].VertexID);
                ids[i] = wso.Base.Vertices[i].VertexID;
            }
            if (maxID == 0 && minID == 0)
            {
                throw new WSOException("No vertex IDs in source mesh!");
            }
            minID = 0;
            maxID = 0;
            for (var i = 0; i < Base.VertexCount; i++)
            {
                minID = Math.Max(minID, Base.Vertices[i].VertexID);
                maxID = Math.Max(maxID, Base.Vertices[i].VertexID);
                if (VertexIDSearch(Base.Vertices[i].VertexID, ids, index0, out index1))
                {
                    Base.Vertices[i].TagValue = wso.Base.Vertices[index1].TagValue;
                    index0 = index1;
                }
            }
            if (maxID == 0 && minID == 0)
            {
                throw new WSOException("No vertex IDs in target mesh!");
            }
        }

        public void ReplaceUV(WSO wso)
        {
            if (Base.FacePointCount != wso.Base.FacePointCount)
            {
                throw new WSOException("Source number of faces does not equal target number of faces!");
            }
            for (var i = 0; i < mMeshes.Length; i++)
            {
                var meshGroup = mMeshes[i];
                for (var j = 0; j < meshGroup.FacePointCount; j++)
                {
                    meshGroup.FacePoints[j].UVs = wso.Base.FacePoints[j].UVs;
                }
            }
        }

        public void ReplaceUVByID(WSO wso)
        {
            throw new WSOException("Not yet implemented!");
        }

        public void ReplaceVertexIDs(WSO wso)
        {
            if (Base.VertexCount != wso.Base.VertexCount)
            {
                throw new WSOException("Source number of vertices does not equal target number of vertices!");
            }
            for (var i = 0; i < mMeshes.Length; i++)
            {
                var meshGroup = mMeshes[i];
                for (var j = 0; j < meshGroup.VertexCount; j++)
                {
                    meshGroup.Vertices[j].VertexID = wso.Base.Vertices[j].VertexID;
                }
            }
        }

        public bool SeamFixer(string ageGender, bool fixBones, bool fixNormals)
        {
            var boneNameList = new List<string>(BoneNameList);
            var emptyIndex = (byte)boneNameList.IndexOf(Enum.GetName(typeof(BoneHash), BoneHash.b__ROOT_bind__));
            if (emptyIndex < 0)
            {
                emptyIndex = (byte)EmptyBoneIndex;
            }
            var refSeams = new Seams(ageGender);
            if (refSeams.mPosition == null)
            {
                //MessageBox.Show("Invalid age/gender selected: " + ageGender);
                return false;
            }
            for (var i = 0; i < Base.VertexCount; i++)
            {
                var j = 0;
                while (j < refSeams.mPosition.Length)
                {
                    if (new Vector3(Base.Vertices[i].Position) == refSeams.mPosition[j])
                    {
                        if (fixBones)
                        {
                            var boneNames = refSeams.mBoneName[j];
                            var indices = new int[boneNames.Length];
                            var boneWeight = refSeams.mBoneWeight[j];
                            for (var k = 0; k < boneNames.Length; k++)
                            {
                                indices[k] = boneNameList.IndexOf(boneNames[k]);
                                if (indices[k] < 0)
                                {
                                    if (boneWeight[k] > 0)
                                    {
                                        indices[k] = boneNameList.Count;
                                        boneNameList.Add(boneNames[k]);
                                    }
                                    else
                                    {
                                        indices[k] = (int)emptyIndex;
                                    }
                                }
                            }
                            var boneWeightsTSRW = new float[4];
                            for (var k = 0; k < 4; k++)
                            {
                                boneWeightsTSRW[k] = boneWeight[k] * 100;
                            }
                            
                            for (var k = 0; k < mMeshes.Length; k++)
                            {
                                var meshGroup = mMeshes[k];
                                meshGroup.Vertices[i].BoneAssignments = indices;
                                meshGroup.Vertices[i].BoneWeights = boneWeightsTSRW;
                            }
                        }
                        if (fixNormals)
                        {
                            for (var k = 0; k < Base.FacePointCount; k++)
                            {
                                if ((int)Base.FacePoints[k].VertexIndex == i)
                                {
                                    Base.FacePoints[k].Normals = refSeams.mNormal[j].Coordinates;
                                }
                            }
                            break;
                        }
                        break;
                    }
                    else
                    {
                        j++;
                    }
                }
            }
            if (fixBones)
            {
                mBones = new Bone[boneNameList.Count];
                mBoneCount = boneNameList.Count;
                for (var i = 0; i < boneNameList.Count; i++)
                {
                    mBones[i] = new Bone(boneNameList[i]);
                }
            }
            return true;
        }

        public void SetBone(int index, Bone newBone)
        {
            mBones[index] = new Bone(newBone);
        }

        public void SortBones(ref List<int> newBones, ref List<float> newWeights)
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

        public bool ValidBones(int meshGroupIndex, int vertexSequenceNumber)
        {
            var boneAssignents = mMeshes[meshGroupIndex].Vertices[vertexSequenceNumber].BoneAssignments;
            var boneWeights = mMeshes[meshGroupIndex].Vertices[vertexSequenceNumber].BoneWeights;
            var totalWeight = 0f;
            for (var i = 0; i < 4; i++)
            {
                if (boneWeights[i] > 0 && (boneAssignents[i] < 0 || boneAssignents[i] >= BoneCount))
                {
                    return false;
                }
                totalWeight += boneWeights[i];
            }
            return (double)totalWeight == 100;
        }

        public bool VertexIDSearch(int vertexID, int[] vertexIDArray, int startIndex, out int foundIndex)
        {
            var index = Array.IndexOf<int>(vertexIDArray, vertexID, startIndex);
            if (index > -1)
            {
                foundIndex = index;
                return true;
            }
            index = Array.IndexOf<int>(vertexIDArray, vertexID);
            if (index > -1)
            {
                foundIndex = index;
                return true;
            }
            foundIndex = startIndex;
            return false;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(mVersion);
            if (mVersion == 5)
            {
                writer.Write(mSoftwareName.Length);
                writer.Write(mSoftwareName);
                writer.Write(mUnknown);
            }
            writer.Write(mMeshCount);
            for (var i = 0; i < mMeshCount; i++)
            {
                mMeshes[i].Write(writer);
            }
            writer.Write(mBoneCount);
            for (var i = 0; i < mBoneCount; i++)
            {
                mBones[i].Write(writer);
            }
        }

        public static WSO[] WSOsfromOBJ(OBJ obj, WSO refMesh, bool smoothModel, bool cleanModel, bool flipUV, Action updateProgressCallback = null)
        {
            if (obj.UVArray.Length == 0)
            {
                /*
                if (MessageBox.Show("This OBJ mesh has no UV mapping. Continue with a blank UV map?", "No UV mapping found", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return null;
                }
                */
                obj.AddEmptyUV();
            }
            else if (flipUV)
            {
                var uvs = new OBJ.UV[obj.UVArray.Length];
                for (var i = 0; i < obj.UVArray.Length; i++)
                {
                    uvs[i] = new OBJ.UV(obj.UVArray[i].Coordinates, true);
                }
                obj.UVArray = uvs;
            }
            if (obj.NormalArray.Length == 0 && !smoothModel)
            {
                /*
                if (MessageBox.Show("This OBJ mesh has no normals. Continue and calculate normals?", "No normals found", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                   return null;
                }
                */
                smoothModel = true;
            }
            if (smoothModel)
            {
                obj.CalculateNormals(true);
            }
            var wsos = new List<WSO>();
            var index = 0;
            wsos.Add(new WSO());
            wsos[index].mVersion = 4;
            var meshGroups = new List<MeshGroup>();
            for (var i = 0; i < obj.GroupCount; i++)
            {
                if (i > 0 && (string.Compare(obj.GroupArray[i].GroupName, "group_base", true) == 0 || string.Compare(obj.GroupArray[i].GroupName, "group_0", true) == 0))
                {
                    wsos[index].mMeshes = meshGroups.ToArray();
                    wsos[index].mMeshCount = meshGroups.Count;
                    wsos[index].AutoBone(refMesh, false, true, 3, 2, false, null);
                    index++;
                    wsos.Add(new WSO());
                    wsos[index].mVersion = 4;
                    meshGroups.Clear();
                }
                List<int[]> faces = new List<int[]>(),
                vertices = new List<int[]>();
                foreach (var face in obj.GroupArray[i].Faces)
                {
                    if (updateProgressCallback != null)
                    {
                        updateProgressCallback();
                    }
                    var temp = new int[3];
                    int j = 0,
                    vertexIndex = 0;
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
                meshGroups.Add(new MeshGroup(obj, obj.GroupArray[i].GroupName, vertices, faces));
            }
            wsos[index].mMeshes = meshGroups.ToArray();
            wsos[index].mMeshCount = meshGroups.Count;
            wsos[index].AutoBone(refMesh, false, true, 3, 2, false, null);
            return wsos.ToArray();
        }
    }
}
