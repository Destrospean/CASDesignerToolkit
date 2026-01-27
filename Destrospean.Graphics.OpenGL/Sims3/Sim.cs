using System.Collections.Generic;
using System.IO;
using Destrospean.CmarNYCBorrowed;
using Destrospean.Common;
using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;
using s3pi.GenericRCOLResource;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace Destrospean.Graphics.OpenGL.Sims3
{
    public class Sim : SimBase
    {
        public class CASPartVolume : Volume
        {
            public List<Vector3> DeltaNormalsFat, DeltaNormalsFit, DeltaNormalsSpecial, DeltaNormalsThin, DeltaVerticesFat, DeltaVerticesFit, DeltaVerticesSpecial, DeltaVerticesThin;

            public SimBase ParentSim;
        }

        protected override void LoadMeshes(CASPart casPart, int presetIndex, int lodIndex, LoadTextureDelegate loadTextureCallback)
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
                    //PreloadedLODMorphed preloadedLODMorphed;
                    try
                    {
                        bblnKey = casPart.CASPartResource.TGIBlocks[bblnIndices[i]].ReverseEvaluateResourceKey();
                        evaluated = casPart.ParentPackage.EvaluateResourceKey(bblnKey);
                        bbln = new BBLN(new BinaryReader(((s3pi.Interfaces.APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)));
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
                                /*
                                preloadedLODMorphed = new PreloadedLODMorphed(bbln, new GEOM[]
                                    {
                                        new GEOM(geom, bgeo, 0, lod)
                                    });
                                PreloadedLODsMorphed.Add(bblnKey, preloadedLODMorphed);
                                */
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
                                    //preloadedLODMorphed = new PreloadedLODMorphed(bbln, geoms.ToArray());
                                    //PreloadedLODsMorphed.Add(bblnKey, preloadedLODMorphed);
                                    MeshUtils.GetDeltaVertices(geoms.ToArray(), deltaNormals, deltaVertices);
                                }
                                catch (ResourceIndexEntryNotFoundException)
                                {
                                }
                            }
                            //geom = geom.LoadGEOMMorph(preloadedLODMorphed.GEOMs, weights[i]);
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
                                        bond.Weight = new float[]
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
                    case CmarNYCBorrowed.Shader.SimAlphaTested:
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
                    foreach (var field in geom.Shader.GetFields())
                    {
                        int valueType;
                        var element = geom.Shader.GetFieldValue(field, out valueType);
                        if (valueType == 1 && (element.Length == 3 || element.Length == 4))
                        {
                            materialColors[(FieldType)field] = new Vector3((float)element[0], (float)element[1], (float)element[2]);
                        }
                        else if (valueType == 4)
                        {
                            materialMaps[(FieldType)field] = new ResourceKey(geom.TGIList[(uint)element[0]].Type, geom.TGIList[(uint)element[0]].Group, geom.TGIList[(uint)element[0]].Instance).ReverseEvaluateResourceKey();
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
                var currentPreset = casPart == CurrentCASPart ? casPart.AllPresets[presetIndex] : casPart.AllPresets[0];
                GlobalState.Meshes[geomAndKey.Key] = new CASPartVolume
                    {
                        AmbientMapID = loadTextureCallback(currentPreset.AmbientMap ?? material.AmbientMap, null),
                        ColorData = colors.ConvertAll(x => new Vector3(x[0], x[1], x[2])).ToArray(),
                        DeltaNormalsFat = FillMissingDeltas(normals, deltaNormalsFat).ConvertAll(x => new Vector3(x[0], x[1], x[2])),
                        DeltaNormalsFit = FillMissingDeltas(normals, deltaNormalsFit).ConvertAll(x => new Vector3(x[0], x[1], x[2])),
                        DeltaNormalsSpecial = FillMissingDeltas(normals, deltaNormalsSpecial).ConvertAll(x => new Vector3(x[0], x[1], x[2])),
                        DeltaNormalsThin = FillMissingDeltas(normals, deltaNormalsThin).ConvertAll(x => new Vector3(x[0], x[1], x[2])),
                        DeltaVerticesFat = FillMissingDeltas(vertices, deltaVerticesFat).ConvertAll(x => new Vector3(x[0], x[1], x[2])),
                        DeltaVerticesFit = FillMissingDeltas(vertices, deltaVerticesFit).ConvertAll(x => new Vector3(x[0], x[1], x[2])),
                        DeltaVerticesSpecial = FillMissingDeltas(vertices, deltaVerticesSpecial).ConvertAll(x => new Vector3(x[0], x[1], x[2])),
                        DeltaVerticesThin = FillMissingDeltas(vertices, deltaVerticesThin).ConvertAll(x => new Vector3(x[0], x[1], x[2])),
                        Faces = faces,
                        GroupID = ID,
                        MainTextureID = loadTextureCallback(geomAndKey.Key, currentPreset.Texture),
                        Material = material,
                        ParentSim = this,
                        Normals = normals.ConvertAll(x => new Vector3(x[0], x[1], x[2])).ToArray(),
                        SpecularMapID = loadTextureCallback(currentPreset.SpecularMap ?? material.SpecularMap, null),
                        TextureCoordinates = textureCoordinates.ConvertAll(x => new Vector2(x[0], x[1])).ToArray(),
                        Vertices = vertices.ConvertAll(x => new Vector3(x[0], x[1], x[2])).ToArray(),
                    };
            }
        }
    }
}
