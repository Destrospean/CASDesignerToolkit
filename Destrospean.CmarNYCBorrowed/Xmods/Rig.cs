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
    public class Rig
    {
        Bone[] mBones;

        int mBoneCount, mIKChainCount, mMinorVersion, mVersion;

        IKChain[] mIKChains;

        string mRigName;

        Quaternion mRootRotation = Quaternion.Identity;

        public int BoneCount
        {
            get
            {
                return mBoneCount;
            }
        }

        public Bone[] Bones
        {
            get
            {
                return mBones;
            }
        }

        public Quaternion RootBindRotation
        {
            get
            {
                return mRootRotation;
            }
            set
            {
                mRootRotation = value;
            }
        }

        public class Bone
        {
            uint mBoneHash;

            string mBoneName;

            Quaternion mGlobalRotation, mLocalRotation;

            Matrix4D mGlobalTransform, mLocalTransform;

            int mIndex, mOpposingBoneIndex, mParentBoneIndex;

            float[] mOrientation = new float[4],
            mPosition = new float[3],
            mScaling = new float[3];

            Rig mRig;

            Vector3 mWorldPosition;

            public uint BoneHash
            {
                get
                {
                    return mBoneHash;
                }
            }

            public string BoneName
            {
                get
                {
                    return mBoneName;
                }
            }

            public uint Flags;

            public Quaternion GlobalRotation
            {
                get
                {
                    return new Quaternion(mGlobalRotation.Coordinates);
                }
            }

            public Matrix4D GlobalTransform
            {
                get
                {
                    return new Matrix4D(mGlobalTransform.Matrix);
                }
            }

            public Quaternion LocalRotation
            {
                get
                {
                    return new Quaternion(mOrientation);
                }
                set
                {
                    mOrientation = value.Coordinates;
                }
            }

            public Matrix4D LocalTransform
            {
                get
                {
                    return new Matrix4D(mLocalTransform.Matrix);
                }
            }

            public Quaternion MorphRotation
            {
                get
                {
                    return mParentBoneIndex >= 0 ? new Quaternion(mRig.mBones[mParentBoneIndex].mGlobalRotation.Coordinates) : mGlobalRotation;
                }
            }

            public string OpposingBoneName
            {
                get
                {
                    if (mOpposingBoneIndex != mIndex)
                    {
                        return mRig.mBones[mOpposingBoneIndex].mBoneName;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            public Bone ParentBone
            {
                get
                {
                    if (mParentBoneIndex >= 0)
                    {
                        return mRig.mBones[mParentBoneIndex];
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public int ParentBoneIndex
            {
                get
                {
                    return mParentBoneIndex;
                }

            }

            public string ParentName
            {
                get
                {
                    if (mParentBoneIndex >= 0)
                    {
                        return mRig.mBones[mParentBoneIndex].mBoneName;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            /// <summary>
            /// Returns position relative to parent bone
            /// </summary>
            public Vector3 PositionVector
            {
                get
                {
                    return new Vector3(mPosition);
                }
                set
                {
                    mPosition = value.Coordinates;
                }
            }

            public Vector3 ScalingVector
            {
                get
                {
                    return new Vector3(mScaling);
                }
                set
                {
                    mScaling = value.Coordinates;
                }
            }

            public Vector3 WorldPosition
            {
                get
                {
                    return mWorldPosition;
                }
                set
                {
                    mWorldPosition = new Vector3(value);
                }
            }

            public Rig Rig
            {
                get
                {
                    return mRig;
                }
            }

            public Bone(Bone other, Rig rig)
            {
                mIndex = other.mIndex;
                mPosition = new[]
                    {
                        other.mPosition[0],
                        other.mPosition[1],
                        other.mPosition[2]
                    };
                mOrientation = new[]
                    {
                        other.mOrientation[0],
                        other.mOrientation[1],
                        other.mOrientation[2],
                        other.mOrientation[3]
                    };
                mScaling = new[]
                    {
                        other.mScaling[0],
                        other.mScaling[1],
                        other.mScaling[2]
                    };
                mBoneName = other.mBoneName;
                mOpposingBoneIndex = other.mOpposingBoneIndex;
                mParentBoneIndex = other.mParentBoneIndex;
                mBoneHash = other.mBoneHash;
                Flags = other.Flags;
                mRig = rig;
                mLocalRotation = new Quaternion(other.mLocalRotation.Coordinates);
                mGlobalRotation = new Quaternion(other.mGlobalRotation.Coordinates);
                mLocalTransform = new Matrix4D(other.mLocalTransform.Matrix);
                mGlobalTransform = new Matrix4D(other.mGlobalTransform.Matrix);
                mWorldPosition = new Vector3(other.mWorldPosition);
            }

            public Bone(BinaryReader reader, Rig rig, int index)
            {
                mRig = rig;
                mIndex = index;
                for (var i = 0; i < 3; i++)
                {
                    mPosition[i] = reader.ReadSingle();
                }
                for (var i = 0; i < 4; i++)
                {
                    mOrientation[i] = reader.ReadSingle();
                }
                for (var i = 0; i < 3; i++)
                {
                    mScaling[i] = reader.ReadSingle();
                }
                char[] temp = reader.ReadChars(reader.ReadInt32());
                mBoneName = new string(temp);
                mOpposingBoneIndex = reader.ReadInt32();
                mParentBoneIndex = reader.ReadInt32();
                mBoneHash = reader.ReadUInt32();
                Flags = reader.ReadUInt32();
                CalculateTransforms();
            }

            public void BoneMover(Vector3 scale, Vector3 offset, Quaternion rotation, float weight)
            {
                Vector3 bonePosition = WorldPosition, weightedOffset = offset * weight,
                weightedScale = (scale * weight) + new Vector3(1, 1, 1);
                var weightedRotation = rotation * weight;
                var transform = weightedRotation.ToMatrix4D(weightedOffset, weightedScale);
                if (ParentBone != null)
                {
                    bonePosition -= ParentBone.WorldPosition;
                }
                bonePosition = transform * bonePosition;
                if (ParentBone != null)
                {
                    bonePosition += ParentBone.WorldPosition;
                }
                WorldPosition = bonePosition;
                var childBones = mRig.GetChildren(mBoneHash);
                foreach (var child in childBones)
                {
                    child.BoneMover(scale, offset, rotation, weight);
                }
            }

            public void CalculateTransforms()
            {
                mLocalRotation = new Quaternion(mOrientation);
                if (mLocalRotation.IsEmpty)
                {
                    mLocalRotation = Quaternion.Identity;
                }
                if (!mLocalRotation.IsNormalized)
                {
                    mLocalRotation.Balance();
                }
                mLocalTransform = mLocalRotation.ToMatrix4D(new Vector3(mPosition), new Vector3(mScaling));
                if (mBoneName.Contains("ROOT_bind"))
                {
                    mRig.mRootRotation = mLocalRotation;
                }
                if (mParentBoneIndex >= 0 && mParentBoneIndex < mRig.BoneCount)
                {
                    mGlobalTransform = mRig.mBones[mParentBoneIndex].mGlobalTransform * mLocalTransform;
                    mGlobalRotation = mRig.mBones[mParentBoneIndex].mGlobalRotation * mLocalRotation;
                }
                else
                {
                    mGlobalTransform = mLocalTransform;
                    mGlobalRotation = mLocalRotation;
                }
                mWorldPosition = mGlobalTransform * new Vector3();
            }

            public void ScaleBone(Vector3 scale, float weight)
            {
                Vector3 position = new Vector3(mPosition),
                temp = position.Scale((scale * weight) + new Vector3(1, 1, 1));  
                mPosition = temp.Coordinates;
            }

            public override string ToString()
            {
                var text = "Position: " + mPosition[0].ToString() + "," + mPosition[1].ToString() + "," + mPosition[2].ToString() + Environment.NewLine;
                text += "Rotation: " + mOrientation[0].ToString() + "," + mOrientation[1].ToString() + "," + mOrientation[2].ToString() + Environment.NewLine;
                text += "Scaling: " + mScaling[0].ToString() + "," + mScaling[1].ToString() + "," + mScaling[2].ToString() + Environment.NewLine;
                text += "Bone Name: " + mBoneName + Environment.NewLine;
                text += "Opposing Bone Index: " + mOpposingBoneIndex.ToString() + " (" + mRig.mBones[mOpposingBoneIndex].mBoneName + ")" + Environment.NewLine;
                text += "Parent Bone Index: " + mParentBoneIndex.ToString() + ((mParentBoneIndex >= 0 && mParentBoneIndex < mRig.mBoneCount) ? " (" + mRig.mBones[mParentBoneIndex].mBoneName + ")" : "") + Environment.NewLine;
                text += "Bone Hash : " + mBoneHash.ToString("X8") + Environment.NewLine;
                text += "Flags : " + Flags.ToString("X8");
                return text;
            }

            public void UpdateLocalData(float weight, Vector3 morphOffset, Quaternion morphRotation, Vector3 morphScale)
            {
                var weightedOffset = morphOffset * weight;
                var weightedRotation = morphRotation * weight;
                mPosition = (PositionVector + weightedOffset).Coordinates;
                mOrientation = (mLocalRotation * weightedRotation).Coordinates;
            }
        }

        public class IKChain
        {
            int[] mBoneIndex, mInfoNode;

            int mBoneListLength, mPoleVectorIndex, mSlotInfo, mSlotOffsetIndex, mRootIndex;              

            Rig mRig;

            public IKChain(IKChain other, Rig rig)
            {
                mBoneListLength = other.mBoneListLength;
                mBoneIndex = new int[mBoneListLength];
                Array.Copy(other.mBoneIndex, mBoneIndex, mBoneListLength);
                mInfoNode = new int[11];
                Array.Copy(other.mInfoNode, mInfoNode, 11);
                mPoleVectorIndex = other.mPoleVectorIndex;
                mSlotInfo = other.mSlotInfo;
                mSlotOffsetIndex = other.mSlotOffsetIndex;
                mRootIndex = other.mRootIndex;
                mRig = rig;
            }

            public IKChain(BinaryReader reader, Rig rig)
            {
                mBoneListLength = reader.ReadInt32();
                mBoneIndex = new int[mBoneListLength];
                for (var i = 0; i < mBoneListLength; i++)
                {
                    mBoneIndex[i] = reader.ReadInt32();
                }
                mInfoNode = new int[11];
                for (var i = 0; i < 11; i++)
                {
                    mInfoNode[i] = reader.ReadInt32();
                }
                mPoleVectorIndex = reader.ReadInt32();
                mSlotInfo = reader.ReadInt32();
                mSlotOffsetIndex = reader.ReadInt32();
                mRootIndex = reader.ReadInt32();
                mRig = rig;
            }

            public override string ToString()
            {
                var text = "Bones: ";
                for (var i = 0; i < mBoneListLength; i++)
                {
                    text += mRig.mBones[mBoneIndex[i]].BoneName + " ";
                }
                text += Environment.NewLine + "PoleVectorIndex: " + mPoleVectorIndex.ToString() + Environment.NewLine;
                text += "SlotOffsetIndex: " + mSlotOffsetIndex.ToString() + Environment.NewLine;
                text += "RootIndex: " + mRootIndex.ToString();
                return text;
            }
        }

        public Rig(Rig other)
        {
            mVersion = other.mVersion;
            mMinorVersion = other.mMinorVersion;
            mBoneCount = other.mBoneCount;
            mBones = new Bone[mBoneCount];
            for (var i = 0; i < mBoneCount; i++)
            {
                mBones[i] = new Bone(other.mBones[i], this);
            }
            mIKChainCount = other.mIKChainCount;
            mIKChains = new IKChain[mIKChainCount];
            for (var i = 0; i < mIKChainCount; i++)
            {
                mIKChains[i] = new IKChain(other.mIKChains[i], this);
            }
        }

        public Rig(BinaryReader reader)
        {
            reader.BaseStream.Position = 0;
            mVersion = reader.ReadInt32();
            mMinorVersion = reader.ReadInt32();
            mBoneCount = reader.ReadInt32();
            mBones = new Bone[mBoneCount];
            for (var i = 0; i < mBoneCount; i++)
            {
                mBones[i] = new Bone(reader, this, i);
            }
            mRigName = new String(reader.ReadChars(reader.ReadInt32()));
            if (mVersion >= 4)
            {
                mIKChainCount = reader.ReadInt32();
                mIKChains = new IKChain[mIKChainCount];
                for (var i = 0; i < mIKChainCount; i++)
                {
                    mIKChains[i] = new IKChain(reader, this);
                }
            }
        }

        public void BoneMorpher(Bone bone, float weight, Vector3 scale, Vector3 offset, Quaternion rotation)
        {
            bone.UpdateLocalData(weight, offset, rotation, scale);
            ScaleBone(bone.BoneHash, scale, weight);
            for (var i = GetIndex(bone.BoneHash); i < mBones.Length; i++)
            {
                mBones[i].CalculateTransforms();
            }
        }

        public void BoneMorpher2(Bone bone, float weight, Vector3 scale, Vector3 offset, Quaternion rotation)
        {
            bone.UpdateLocalData(weight, offset, rotation, scale);
            var childBones = GetChildren(bone.BoneHash);
            foreach (var child in childBones)
            {
                child.ScaleBone(scale, weight);
                ScaleBone(child.BoneHash, scale, weight);
            }
        }

        public Bone GetBone(uint boneHash)
        {
            foreach (var bone in mBones)
            {
                if (boneHash == bone.BoneHash)
                {
                    return bone;
                }
            }
            return null;
        }

        public int[] GetChildIndices(int index)
        {
            var childList = new List<int>();
            if (index >= 0 && index < BoneCount)
            {
                for (var i = 0; i < BoneCount; i++)
                {
                    if (mBones[i].ParentBoneIndex == index)
                    {
                        childList.Add(i);
                    }
                }
            }
            return childList.ToArray();
        }

        public Bone[] GetChildren(uint boneHash)
        {
            var index = GetIndex(boneHash);
            if (index < 0)
            {
                return null;
            }
            var childBones = new List<Bone>();
            foreach (var i in GetChildIndices(index))
            {
                childBones.Add(mBones[i]);
            }
            return childBones.ToArray();
        }

        public void GetDescendants(uint boneHash, ref List<uint> allBones)
        {
            var index = GetIndex(boneHash);
            if (index < 0)
            {
                return;
            }
            allBones.Add(boneHash);
            foreach (var i in GetChildIndices(index))
            {
                GetDescendants(mBones[i].BoneHash, ref allBones);
            }
        }

        public int GetIndex(uint boneHash)
        {
            for (var i = 0; i < BoneCount; i++)
            {
                if (boneHash == mBones[i].BoneHash)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool GetPosition(uint boneHash, out Vector3 position)
        {
            foreach (var bone in mBones)
            {
                if (boneHash == bone.BoneHash)
                {
                    position = bone.PositionVector;
                    return true;
                }
            }
            position = new Vector3();
            return false;
        }

        public bool GetPosition(int index, out Vector3 position)
        {
            if (index >= 0 && index < BoneCount)
            {
                position = mBones[index].PositionVector;
                return true;
            }
            position = new Vector3();
            return false;
        }

        public bool GetWorldPosition(uint boneHash, out Vector3 position)
        {
            foreach (var bone in mBones)
            {
                if (boneHash == bone.BoneHash)
                {
                    position = bone.WorldPosition;
                    return true;
                }
            }
            position = new Vector3();
            return false;
        }

        public bool GetWorldPosition(int index, out Vector3 position)
        {
            if (index >= 0 && index < BoneCount)
            {
                position = mBones[index].WorldPosition;
                return true;
            }
            position = new Vector3();
            return false;
        }

        public int[] GetParentIndices(int index)
        {
            var parentList = new List<int>();
            var i = index;
            while (mBones[i].ParentBoneIndex >= 0 && mBones[i].ParentBoneIndex < BoneCount)
            {
                parentList.Add(mBones[i].ParentBoneIndex);
                i = mBones[i].ParentBoneIndex;
            }
            parentList.Reverse();
            return parentList.ToArray();
        }

        public string ListBonesByFlags(uint flag)
        {
            var text = "";
            foreach (var bone in mBones)
            {
                if (bone.Flags == flag)
                {
                    text += bone.BoneName + Environment.NewLine;
                }
            }
            return text;
        }

        public void ScaleBone(uint boneHash, Vector3 scale, float weight)
        {
            var childBones = GetChildren(boneHash);
            foreach (var child in childBones)
            {
                child.ScaleBone(scale, weight);
                ScaleBone(child.BoneHash, scale, weight);
            }
        }

        public override string ToString()
        {
            var text = "Version: " + mVersion.ToString() + ", Minor Version: " + mMinorVersion.ToString() + Environment.NewLine;
            text += mBoneCount.ToString() + " Bones:" + Environment.NewLine + Environment.NewLine;
            for (var i = 0; i < mBoneCount; i++)
            {
                text += mBones[i].ToString() + Environment.NewLine + Environment.NewLine;
            }
            text += mRigName + Environment.NewLine + Environment.NewLine;
            text += mIKChainCount.ToString() + " IK Chains:" + Environment.NewLine + Environment.NewLine;
            for (var i = 0; i < mIKChainCount; i++)
            {
                text += mIKChains[i].ToString() + Environment.NewLine + Environment.NewLine;
            }
            return text;
        }
    }
}
