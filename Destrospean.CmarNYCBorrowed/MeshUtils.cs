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

using System.Collections.Generic;
using Destrospean.S3PIExtensions;

namespace Destrospean.CmarNYCBorrowed
{
    public static class MeshUtils
    {
        public static void GetDeltaVertices(this GEOM baseMesh, BGEO morph, int lod, int bgeoSection1EntryNumber, List<float[]> deltaNormals, List<float[]> deltaPositions)
        {
            if (baseMesh == null || !baseMesh.HasVertexIDs || bgeoSection1EntryNumber < 0 || morph == null)
            {
                return;
            }
            var vertexDeltas = morph.GetDeltas(bgeoSection1EntryNumber, lod);
            for (var i = 0; i < baseMesh.VertexCount; i++)
            {
                var vertexDeltaIndex = vertexDeltas.FindIndex(x => x.VertexID == baseMesh.GetVertexID(i));
                if (vertexDeltaIndex > -1)
                {
                    var delta = vertexDeltas[vertexDeltaIndex].Position;
                    deltaNormals.Add(delta.Coordinates);
                    deltaPositions.Add(delta.Coordinates);
                }
            }
        }

        public static void GetDeltaVertices(this GEOM baseMesh, BGEO morph, int lod, Species species, AgeGender age, AgeGender gender, List<float[]> deltaNormals, List<float[]> deltaPositions)
        {
            GetDeltaVertices(baseMesh, morph, lod, morph.GetSection1EntryIndex(species, age, gender), deltaNormals, deltaPositions);
        }

        public static void GetDeltaVertices(GEOM[] morphs, List<float[]> deltaNormals, List<float[]> deltaPositions)
        {
            if (morphs == null)
            {
                return;
            }
            for (var i = 0; i < morphs.Length; i++)
            {
                for (var j = 0; j < morphs[i].VertexCount; j++)
                {
                    try
                    {
                        morphs[i].GetVertexID(j);
                        deltaNormals.Add(morphs[i].GetNormal(j));
                        deltaPositions.Add(morphs[i].GetPosition(j));
                    }
                    catch (System.NullReferenceException)
                    {
                    }
                }
            }
        }

        public static Rig GetRig(this s3pi.Interfaces.IPackage package, Species species, AgeGender age)
        {
            var rigName = GetRigPrefix(species, age, AgeGender.Unisex) + "Rig";
            var evaluated = package.EvaluateResourceKey(new ResourceKey(ResourceUtils.GetResourceType("_RIG"), 0, System.Security.Cryptography.FNV64.GetHash(rigName)).ReverseEvaluateResourceKey());
            return new Rig(new System.IO.BinaryReader(((s3pi.Interfaces.APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)));
        }

        public static string GetRigPrefix(Species species, AgeGender age, AgeGender gender)
        {
            var specifier = "";
            switch (age)
            {
                case AgeGender.Toddler:
                    specifier = (species == Species.Human ? "p" : "c");
                    break;
                case AgeGender.Child:
                    specifier = "c";
                    break;
                default:
                    specifier = "a";
                    break;
            }
            switch (species)
            {
                case Species.Human:
                    specifier += "u";
                    break;
                default:
                    specifier += (age == AgeGender.Child && species == Species.LittleDog) ? "d" : species.ToString().Substring(0, 1).ToLower();
                    break;
            }
            return specifier;
        }

        public static GEOM LoadBGEOMorph(this GEOM baseMesh, BGEO morph, int lod, Species species, AgeGender age, AgeGender gender)
        {
            if (baseMesh == null || !baseMesh.HasVertexIDs)
            {
                return baseMesh;
            }
            if (morph == null)
            {
                return new GEOM(baseMesh);
            }
            var morphMesh = new GEOM(baseMesh);
            var entry = morph.GetSection1EntryIndex(species, age, gender);
            if (entry < 0)
            {
                return new GEOM(baseMesh);
            }
            var vertexDeltas = morph.GetDeltas(entry, lod);
            for (var i = 0; i < morphMesh.VertexCount; i++)
            {
                var vertexDeltaIndex = vertexDeltas.FindIndex(x => x.VertexID == morphMesh.GetVertexID(i));
                if (vertexDeltaIndex > -1)
                {
                    Vector3 delta = vertexDeltas[vertexDeltaIndex].Position,
                    normal = new Vector3(morphMesh.GetNormal(i)),
                    position = new Vector3(morphMesh.GetPosition(i));
                    morphMesh.SetPosition(i, (position + delta * morph.Weight).Coordinates);
                    morphMesh.SetNormal(i, (normal + delta * morph.Weight).Coordinates);
                }
            }
            return morphMesh;
        }

        public static GEOM LoadBONDMorph(this GEOM baseMesh, BOND boneDelta, Rig rig)
        {
            if (baseMesh == null)
            {
                return null;
            }
            if (boneDelta == null)
            {
                return baseMesh;
            }
            var missingBones = "";
            var morphMesh = new GEOM(baseMesh);
            var unit = new Vector3(1, 1, 1);
            morphMesh.SetupDeltas();
            foreach (var delta in boneDelta.Adjustments)
            {
                var bone = rig.GetBone(delta.SlotHash);
                if (bone == null)
                {
                    missingBones += "Bone not found: " + delta.SlotHash.ToString("X8") + ", ";
                    continue;
                }
                Vector3 localOffset = new Vector3(delta.OffsetX, delta.OffsetY, delta.OffsetZ),
                localScale = new Vector3(delta.ScaleX, delta.ScaleY, delta.ScaleZ);
                var localRotation = new Quaternion(delta.QuatX, delta.QuatY, delta.QuatZ, delta.QuatW);
                if (localRotation.IsEmpty)
                {
                    localRotation = Quaternion.Identity;
                }
                if (!localRotation.IsNormalized)
                {
                    localRotation.Balance();
                }           
                morphMesh.BoneMorpher(bone, boneDelta.Weight, (bone.MorphRotation * localOffset * bone.MorphRotation.Conjugate()).ToVector3(), (bone.MorphRotation.ToMatrix3D() * Matrix3D.FromScale(localScale + unit)).Scale - unit, bone.MorphRotation * localRotation * bone.MorphRotation.Conjugate());
            }
            morphMesh.UpdatePositions();
            foreach (var delta in boneDelta.Adjustments)
            {
                var bone = rig.GetBone(delta.SlotHash);
                if (bone == null)
                {
                    continue;
                }
                Vector3 localOffset = new Vector3(delta.OffsetX, delta.OffsetY, delta.OffsetZ),
                localScale = new Vector3(delta.ScaleX, delta.ScaleY, delta.ScaleZ);
                var localRotation = new Quaternion(delta.QuatX, delta.QuatY, delta.QuatZ, delta.QuatW);
                if (localRotation.IsEmpty)
                {
                    localRotation = Quaternion.Identity;
                }
                if (!localRotation.IsNormalized)
                {
                    localRotation.Balance();
                }
                rig.BoneMorpher(bone, boneDelta.Weight, localScale, localOffset, localRotation);
            }
            return morphMesh;
        }

        /// <summary>
        /// Apply morph meshes
        /// </summary>
        /// <param name="baseMesh">base</param>
        /// <param name="morphs">morph meshes for one morph: fat, fit, thin or special</param>
        /// <param name="weight">morph weight</param>
        /// <returns></returns>
        public static GEOM LoadGEOMMorph(this GEOM baseMesh, GEOM[] morphs, float weight)
        {
            if (baseMesh == null)
            {
                return baseMesh;
            }
            if (morphs == null || morphs.Length == 0)
            {
                return new GEOM(baseMesh);
            }
            var morphMesh = new GEOM(baseMesh);
            Dictionary<int, Vector3> deltaNormals = new Dictionary<int, Vector3>(),
            deltaPositions = new Dictionary<int, Vector3>();
            for (var i = 0; i < morphs.Length; i++)
            {
                for (var j = 0; j < morphs[i].VertexCount; j++)
                {
                    int vertexID;
                    try
                    {
                        vertexID = morphs[i].GetVertexID(j);
                    }
                    catch (System.NullReferenceException)
                    {
                        continue;
                    }
                    if (!deltaNormals.ContainsKey(vertexID))
                    {
                        deltaNormals.Add(vertexID, new Vector3(morphs[i].GetNormal(j)));
                    }
                    if (!deltaPositions.ContainsKey(vertexID))
                    {
                        deltaPositions.Add(vertexID, new Vector3(morphs[i].GetPosition(j)));
                    }
                }
            }
            for (var i = 0; i < morphMesh.VertexCount; i++)
            {
                int vertexID;
                try
                {
                    vertexID = morphMesh.GetVertexID(i);
                }
                catch (System.NullReferenceException)
                {
                    continue;
                }
                Vector3 delta = new Vector3(),
                normal = new Vector3(morphMesh.GetNormal(i)),
                position = new Vector3(morphMesh.GetPosition(i));
                deltaPositions.TryGetValue(vertexID, out delta);
                morphMesh.SetPosition(i, (position + delta * weight).Coordinates);
                morphMesh.SetNormal(i, (normal + delta * weight).Coordinates);
            }
            return morphMesh;
        }
    }
}
