using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Destrospean.CmarNYCBorrowed;
using Destrospean.Common;
using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;
using s3pi.GenericRCOLResource;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;

namespace Destrospean.Graphics.OpenGL.Sims3
{
    public class Sim : SimBase
    {
        public int SkinAmbientMapID = -1,
        SkinSpecularMapID = -1;

        public class CASPartVolume : Volume
        {
            public int BodyAmbientMapID
            {
                set
                {
                    ParentSim.SkinAmbientMapID = value;
                }
                get
                {
                    return ParentSim.SkinAmbientMapID;
                }
            }

            public int BodySpecularMapID
            {
                set
                {
                    ParentSim.SkinSpecularMapID = value;
                }
                get
                {
                    return ParentSim.SkinSpecularMapID;
                }
            }

            public List<Vector3> DeltaNormalsFat, DeltaNormalsFit, DeltaNormalsSpecial, DeltaNormalsThin, DeltaVerticesFat, DeltaVerticesFit, DeltaVerticesSpecial, DeltaVerticesThin;

            public Sim ParentSim;

            public int SkinAmbientMapID
            {
                set
                {
                    if (ParentSim.SkinAmbientMapID == -1)
                    {
                        ParentSim.SkinAmbientMapID = value;
                    }
                }
                get
                {
                    return ParentSim.SkinAmbientMapID;
                }
            }

            public int SkinSpecularMapID
            {
                set
                {
                    if (ParentSim.SkinSpecularMapID == -1)
                    {
                        ParentSim.SkinSpecularMapID = value;
                    }
                }
                get
                {
                    return ParentSim.SkinSpecularMapID;
                }
            }
        }

        protected override void LoadMeshes(CASPart casPart, int presetIndex, int lodIndex, LoadTextureDelegate loadTextureCallback, LoadMeshOnMainThreadDelegate loadMeshOnMainThreadCallback)
        {
            lock (Complate.Lock)
            {
                if (!CASParts.ContainsValue(casPart) || casPart.LODs.Count == 0)
                {
                    return;
                }
                var adjustedLODIndex = lodIndex < casPart.LODs.Count ? lodIndex : casPart.LODs.Count - 1;
                if (adjustedLODIndex < 0)
                {
                    return;
                }
                var lod = new List<int>(casPart.LODs.Keys)[adjustedLODIndex];
                foreach (var geomAndKey in new List<List<CASPart.GEOMAndKey>>(casPart.LODs.Values)[adjustedLODIndex])
                {
                    var geom = geomAndKey.GEOM;
                    byte[] bblnIndices =
                        {
                            casPart.CASPartResource.BlendInfoFatIndex,
                            casPart.CASPartResource.BlendInfoFitIndex,
                            casPart.CASPartResource.BlendInfoThinIndex,
                            casPart.CASPartResource.BlendInfoSpecialIndex
                        };
                    List<float[]> deltaNormalsFat = new List<float[]>(),
                    deltaNormalsFit = new List<float[]>(),
                    deltaNormalsSpecial = new List<float[]>(),
                    deltaNormalsThin = new List<float[]>(),
                    deltaVerticesFat = new List<float[]>(),
                    deltaVerticesFit = new List<float[]>(),
                    deltaVerticesSpecial = new List<float[]>(),
                    deltaVerticesThin = new List<float[]>();
                    for (var i = 0; i < bblnIndices.Length; i++)
                    {
                        BBLN bbln;
                        string bblnKey;
                        EvaluatedResourceKey evaluated;
                        try
                        {
                            bblnKey = casPart.CASPartResource.TGIBlocks[bblnIndices[i]].ReverseEvaluateResourceKey();
                            evaluated = casPart.ParentPackage.EvaluateResourceKey(bblnKey);
                            var stream = ((s3pi.Interfaces.APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry);
                            stream.Position = 0;
                            bbln = new BBLN(new BinaryReader(stream));
                            stream.Position = 0;
                        }
                        catch (ResourceIndexEntryNotFoundException)
                        {
                            continue;
                        }
                        BGEO bgeo = null;
                        try
                        {
                            evaluated = casPart.ParentPackage.EvaluateResourceKey(new ResourceKey(bbln.BGEOTGI.Type, bbln.BGEOTGI.Group, bbln.BGEOTGI.Instance).ReverseEvaluateResourceKey());
                            bgeo = new BGEO(new BinaryReader(((s3pi.Interfaces.APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)));
                        }
                        catch (ResourceIndexEntryNotFoundException)
                        {
                        }
                        foreach (var entry in bbln.Entries)
                        {
                            foreach (var geomMorph in entry.GEOMMorphs)
                            {
                                List<float[]> deltaNormals, deltaVertices;
                                switch (i)
                                {
                                    case 0:
                                        deltaNormals = deltaNormalsFat;
                                        deltaVertices = deltaVerticesFat;
                                        break;
                                    case 1:
                                        deltaNormals = deltaNormalsFit;
                                        deltaVertices = deltaVerticesFit;
                                        break;
                                    case 2:
                                        deltaNormals = deltaNormalsThin;
                                        deltaVertices = deltaVerticesThin;
                                        break;
                                    default:
                                        deltaNormals = deltaNormalsSpecial;
                                        deltaVertices = deltaVerticesSpecial;
                                        break;
                                }
                                if (bgeo != null)
                                {
                                    geom.GetDeltaVertices(bgeo, lod, 0, deltaNormals, deltaVertices);
                                }
                                else if (bbln.TGIList != null && bbln.TGIList.Length > geomMorph.TGIIndex && geom.HasVertexIDs)
                                {
                                    try
                                    {
                                        var geoms = new List<GEOM>();
                                        foreach (var link in new CmarNYCBorrowed.VPXY(new BinaryReader(PreloadedData.VPXYs[new ResourceKey(bbln.TGIList[geomMorph.TGIIndex].Type, bbln.TGIList[geomMorph.TGIIndex].Group, bbln.TGIList[geomMorph.TGIIndex].Instance).ReverseEvaluateResourceKey()].Stream)).GetMeshLinks(lod))
                                        {
                                            try
                                            {
                                                geoms.Add(PreloadedData.GEOMs[new ResourceKey(link.Type, link.Group, link.Instance).ReverseEvaluateResourceKey()]);
                                            }
                                            catch (ResourceIndexEntryNotFoundException)
                                            {
                                            }
                                        }
                                        MeshUtils.GetDeltaVertices(geoms.ToArray(), deltaNormals, deltaVertices);
                                    }
                                    catch (ResourceIndexEntryNotFoundException)
                                    {
                                    }
                                }
                            }
                            foreach (var boneMorph in entry.BoneMorphs)
                            {
                                try
                                {   
                                    foreach (var link in new CmarNYCBorrowed.VPXY(new BinaryReader(PreloadedData.VPXYs[new ResourceKey(bbln.TGIList[boneMorph.TGIIndex].Type, bbln.TGIList[boneMorph.TGIIndex].Group, bbln.TGIList[boneMorph.TGIIndex].Instance).ReverseEvaluateResourceKey()].Stream)).AllLinks)
                                    {
                                        try
                                        {   
                                            evaluated = casPart.ParentPackage.EvaluateResourceKey(new ResourceKey(link.Type, link.Group, link.Instance).ReverseEvaluateResourceKey());
                                            var bond = new BOND(new BinaryReader(((s3pi.Interfaces.APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)));
                                            bond.Weight = new[]
                                                {
                                                    Fat,
                                                    Fit,
                                                    Thin,
                                                    Special
                                                }[i] * boneMorph.Amount;
                                            geom = geom.LoadBONDMorph(bond, casPart.CurrentRig);
                                        }
                                        catch (ResourceIndexEntryNotFoundException)
                                        {
                                        }
                                    }
                                }
                                catch (ResourceIndexEntryNotFoundException)
                                {
                                }
                            }
                        }
                    }
                    List<float[]> colors = new List<float[]>(),
                    normals = new List<float[]>(),
                    vertices = new List<float[]>();
                    var faces = new List<int[]>();
                    var textureCoordinates = new List<float[]>();
                    for (var i = 0; i < geom.FaceCount; i++)
                    {
                        var indices = geom.GetFaceIndices(i);
                        faces.Add(new int[]
                            {
                                indices[0],
                                indices[1],
                                indices[2]
                            });
                    }
                    for (var i = 0; i < geom.VertexCount; i++)
                    {
                        colors.Add(new float[]
                            {
                                1,
                                1,
                                1
                            });
                        normals.Add(geom.GetNormal(i));
                        vertices.Add(geom.GetPosition(i));
                        for (var j = 0; j < geom.UVCount; j++)
                        {
                            var uv = geom.GetUV(i, j);
                            textureCoordinates.Add(uv);
                        }
                    }
                    var hasTransparency = false;
                    switch ((CmarNYCBorrowed.Shader)geom.ShaderHash)
                    {
                        case CmarNYCBorrowed.Shader.CasSimHair:
                        case CmarNYCBorrowed.Shader.CasSimHairSimple:
                        case CmarNYCBorrowed.Shader.SimAlphaBlended:
                        case CmarNYCBorrowed.Shader.SimAlphaTested:
                        case CmarNYCBorrowed.Shader.SimEyelashes:
                        case CmarNYCBorrowed.Shader.SimGlass:
                        case CmarNYCBorrowed.Shader.SimHair:
                            hasTransparency = true;
                            break;
                    }
                    Material material;
                    if (!GlobalState.Materials.TryGetValue(geomAndKey.Key, out material))
                    {
                        var materialColors = new Dictionary<FieldType, Vector3>();
                        var materialMaps = new Dictionary<FieldType, string>();
                        for (var i = 0; i < geom.Shader.FieldCount; i++)
                        {
                            uint fieldType, valueType;
                            var field = geom.Shader.GetField(i, out fieldType, out valueType);
                            switch ((MeshFormatDataType)valueType)
                            {
                                case MeshFormatDataType.Float:
                                    if (field.Length > 2)
                                    {
                                        materialColors[(FieldType)fieldType] = ToVector3(System.Array.ConvertAll(field, x => (float)x));
                                    }
                                    break;
                                case MeshFormatDataType.Uint:
                                    materialMaps[(FieldType)fieldType] = new ResourceKey(geom.TGIList[(uint)field[0]].Type, geom.TGIList[(uint)field[0]].Group, geom.TGIList[(uint)field[0]].Instance).ReverseEvaluateResourceKey();
                                    break;
                            }
                        }
                        Vector3 color;
                        string map;
                        material = new Material
                            {
#pragma warning disable 0618
                                AmbientColor = materialColors.TryGetValue(FieldType.Ambient, out color) ? color : Vector3.One,
#pragma warning restore 0618
                                AmbientMap = materialMaps.TryGetValue(FieldType.AmbientOcclusionMap, out map) ? map : "",
                                DiffuseColor = materialColors.TryGetValue(FieldType.Diffuse, out color) ? color : Vector3.One,
                                DiffuseMap = materialMaps.TryGetValue(FieldType.DiffuseMap, out map) ? map : "",
                                HasTransparency = hasTransparency,
                                NormalMap = materialMaps.TryGetValue(FieldType.NormalMap, out map) ? map : "",
                                SpecularColor = materialColors.TryGetValue(FieldType.Specular, out color) ? color : Vector3.One,
                                SpecularMap = materialMaps.TryGetValue(FieldType.SpecularMap, out map) ? map : ""
                            };
                        GlobalState.Materials[geomAndKey.Key] = material;
                    }
                    var currentPreset = casPart.AllPresets[casPart == CurrentCASPart ? presetIndex : 0];
                    string ambientMap = currentPreset.AmbientMap ?? material.AmbientMap,
                    specularMap = currentPreset.SpecularMap ?? material.SpecularMap,
                    skinAmbientMap = ((CASPartPreset)currentPreset).SkinAmbientMap ?? material.AmbientMap,
                    skinSpecularMap = ((CASPartPreset)currentPreset).SkinSpecularMap ?? material.SpecularMap;
                    Bitmap ambientMapImage = null,
                    specularMapImage = null,
                    skinAmbientMapImage = null,
                    skinSpecularMapImage = null;
                    if (!string.IsNullOrEmpty(ambientMap))
                    {
                        ambientMapImage = (Bitmap)CurrentCASPart.ParentPackage.GetTexture(ambientMap, Complate.GetTextureCallback).Clone();
                    }
                    if (!string.IsNullOrEmpty(specularMap))
                    {
                        specularMapImage = (Bitmap)CurrentCASPart.ParentPackage.GetTexture(specularMap, Complate.GetTextureCallback).Clone();
                    }
                    if (!string.IsNullOrEmpty(skinAmbientMap))
                    {
                        skinAmbientMapImage = (Bitmap)CurrentCASPart.ParentPackage.GetTexture(skinAmbientMap, Complate.GetTextureCallback).Clone();
                    }
                    if (!string.IsNullOrEmpty(skinSpecularMap))
                    {
                        skinSpecularMapImage = (Bitmap)CurrentCASPart.ParentPackage.GetTexture(skinSpecularMap, Complate.GetTextureCallback).Clone();
                    }
                    loadMeshOnMainThreadCallback(new CASPartVolume
                        {
                            ColorData = colors.ConvertAll(ToVector3).ToArray(),
                            DeltaNormalsFat = FillMissingDeltas(normals, deltaNormalsFat).ConvertAll(ToVector3),
                            DeltaNormalsFit = FillMissingDeltas(normals, deltaNormalsFit).ConvertAll(ToVector3),
                            DeltaNormalsSpecial = FillMissingDeltas(normals, deltaNormalsSpecial).ConvertAll(ToVector3),
                            DeltaNormalsThin = FillMissingDeltas(normals, deltaNormalsThin).ConvertAll(ToVector3),
                            DeltaVerticesFat = FillMissingDeltas(vertices, deltaVerticesFat).ConvertAll(ToVector3),
                            DeltaVerticesFit = FillMissingDeltas(vertices, deltaVerticesFit).ConvertAll(ToVector3),
                            DeltaVerticesSpecial = FillMissingDeltas(vertices, deltaVerticesSpecial).ConvertAll(ToVector3),
                            DeltaVerticesThin = FillMissingDeltas(vertices, deltaVerticesThin).ConvertAll(ToVector3),
                            Faces = faces,
                            GroupID = ID,
                            Key = geomAndKey.Key,
                            LODIndex = lodIndex,
                            Material = material,
                            Object = casPart,
                            ParentSim = this,
                            Normals = normals.ConvertAll(ToVector3).ToArray(),
                            TextureCoordinates = textureCoordinates.ConvertAll(x => new Vector2(x[0], x[1])).ToArray(),
                            Vertices = vertices.ConvertAll(ToVector3).ToArray(),
                        }, currentPreset, (Bitmap)(casPart.CASPartResource.Clothing >= CASPartResource.ClothingType.Body && casPart.CASPartResource.Clothing <= CASPartResource.ClothingType.Bottom ? GetStackedBodyTexture(presetIndex) : casPart.CASPartResource.Clothing == CASPartResource.ClothingType.Face ? GetStackedFaceTexture(presetIndex) : casPart.CASPartResource.Clothing == CASPartResource.ClothingType.Scalp ? GetStackedScalpTexture(presetIndex) : casPart.CASPartResource.Clothing == CASPartResource.ClothingType.Shoes ? GetStackedShoesTexture(presetIndex) : currentPreset.Texture).Clone(), new[]
                        {
                            ambientMapImage,
                            specularMapImage,
                            skinAmbientMapImage,
                            skinSpecularMapImage
                        }, material, loadTextureCallback);
                }
            }
        }

        public static void LoadMeshOnMainThread(object volume, Preset currentPreset, Bitmap presetTexture, Bitmap[] ambientAndSpecularMapTextures, object material, LoadTextureDelegate loadTextureCallback)
        {
            var casPartPreset = (CASPartPreset)currentPreset;
            var casPartVolume = (CASPartVolume)volume;
            var materialCast = (Material)material;
            casPartVolume.AmbientMapID = loadTextureCallback(currentPreset.AmbientMap ?? materialCast.AmbientMap, ambientAndSpecularMapTextures[0]);
            casPartVolume.SpecularMapID = loadTextureCallback(currentPreset.SpecularMap ?? materialCast.SpecularMap, ambientAndSpecularMapTextures[1]);
            if (!string.IsNullOrEmpty(casPartPreset.BodyAmbientMap))
            {
                casPartVolume.BodyAmbientMapID = loadTextureCallback(casPartPreset.BodyAmbientMap, null);
            }
            if (!string.IsNullOrEmpty(casPartPreset.BodySpecularMap))
            {
                casPartVolume.BodySpecularMapID = loadTextureCallback(casPartPreset.BodySpecularMap, null);
            }
            if (!string.IsNullOrEmpty(casPartPreset.SkinAmbientMap))
            {
                casPartVolume.SkinAmbientMapID = loadTextureCallback(casPartPreset.SkinAmbientMap, ambientAndSpecularMapTextures[2]);
            }
            if (!string.IsNullOrEmpty(casPartPreset.SkinSpecularMap))
            {
                casPartVolume.SkinSpecularMapID = loadTextureCallback(casPartPreset.SkinSpecularMap, ambientAndSpecularMapTextures[3]);
            }
            casPartVolume.MainTextureID = loadTextureCallback(casPartVolume.Key, presetTexture);
            GlobalState.Meshes[casPartVolume.Key] = casPartVolume;
            foreach (var texture in ambientAndSpecularMapTextures)
            {
                if (texture != null)
                {
                    texture.Dispose();
                }
            }
            presetTexture.Dispose();
            foreach (var meshKey in new List<string>(GlobalState.Meshes.Keys))
            {
                Volume mesh;
                if (GlobalState.Meshes.TryGetValue(meshKey, out mesh) && mesh.LODIndex != GlobalState.CurrentLODIndex)
                {
                    lock (GlobalState.Lock)
                    {
                        GlobalState.Meshes.Remove(meshKey);
                    }
                }
            }
        }

        public static Vector3 ToVector3(float[] coordinates)
        {
            return new Vector3(coordinates[0], coordinates[1], coordinates[2]);
        }
    }
}
