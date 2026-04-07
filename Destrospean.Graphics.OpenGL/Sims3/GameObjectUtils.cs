using System;
using System.Collections.Generic;
using Destrospean.Common;
using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;
using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;

namespace Destrospean.Graphics.OpenGL.Sims3
{
    public static class GameObjectUtils
    {
        public static void LoadMeshes(this GameObject gameObject, int presetIndex, int lodIndex, uint materialState, SimBase.LoadTextureDelegate loadTextureCallback, SimBase.LoadMeshOnMainThreadDelegate loadMeshOnMainThreadCallback)
        {
            lock (Complate.Lock)
            {
                if (!PreloadedData.GameObjects.ContainsValue(gameObject) || gameObject.LODs.Count == 0)
                {
                    return;
                }
                var lodId = new List<LODId>(gameObject.LODs.Keys)[lodIndex];
                foreach (var meshGroup in gameObject.LODs[lodId].MeshGroups)
                {
                    if (meshGroup.VertexFormat == null && meshGroup.HasFlag(MeshFlags.ShadowCaster))
                    {
                        continue;
                    }
                    List<Vector3> colors = new List<Vector3>(),
                    normals = new List<Vector3>(),
                    vertices = new List<Vector3>();
                    var faces = new List<int[]>();
                    var indices = meshGroup.IndexBuffer.GetIndices(meshGroup.MeshGroup);
                    var textureCoordinates = new List<Vector2>();
                    for (var i = 0; i < indices.Length; i += 3)
                    {
                        faces.Add(new int[]
                            {
                                indices[i],
                                indices[i + 1],
                                indices[i + 2]
                            });
                    }
                    foreach (var vertex in meshGroup.VertexBuffer.GetVertices(meshGroup.MeshGroup, meshGroup.VertexFormat ?? VRTF.CreateDefaultForMesh(meshGroup.MeshGroup), meshGroup.UVScales))
                    {
                        colors.Add(vertex.Color == null ? Vector3.One : new Vector3(vertex.Color[0], vertex.Color[1], vertex.Color[2]));
                        if (vertex.Normal != null)
                        {
                            normals.Add(new Vector3(vertex.Normal[0], vertex.Normal[1], vertex.Normal[2]));
                        }
                        if (vertex.UV != null)
                        {
                            textureCoordinates.Add(new Vector2(vertex.UV[0][0], vertex.UV[0][1]));
                        }
                        if (vertex.Position != null)
                        {
                            vertices.Add(new Vector3(vertex.Position[0], vertex.Position[1], vertex.Position[2]));
                        }
                    }
                    var mlodResource = (GenericRCOLResource)gameObject.LODs[lodId].Resource;
                    var matd = mlodResource == null ? null : meshGroup.MaterialSet == null ? meshGroup.DirectMATD : mlodResource.ChunkEntries[meshGroup.MaterialSet.Entries.Find(x => (uint)x.MaterialState == materialState).Index.TGIBlockIndex + mlodResource.PublicChunks].RCOLBlock as MATD;
                    Material material;
                    if (!GlobalState.Materials.TryGetValue(matd.MaterialNameHash.ToString(), out material) && matd != null)
                    {
                        var materialColors = new Dictionary<FieldType, Vector3>();
                        var materialMaps = new Dictionary<FieldType, string>();
                        foreach (var element in matd.Mtnf.SData)
                        {
                            var elementFloat3 = element as ElementFloat3;
                            if (elementFloat3 != null)
                            {
                                materialColors[element.Field] = new Vector3(elementFloat3.Data0, elementFloat3.Data1, elementFloat3.Data2);
                                continue;
                            }
                            var elementTextureRef = element as ElementTextureRef;
                            if (elementTextureRef != null)
                            {
                                materialMaps[element.Field] = mlodResource.Resources[elementTextureRef.Data.TGIBlockIndex].ReverseEvaluateResourceKey();
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
                                HasTransparency = true,
                                NormalMap = materialMaps.TryGetValue(FieldType.NormalMap, out map) ? map : "",
                                Shader = meshGroup.HasFlag(MeshFlags.DropShadow) || matd.IsVideoSurface ? "textured" : "",
                                SpecularColor = materialColors.TryGetValue(FieldType.Specular, out color) ? color : Vector3.One,
                                SpecularMap = materialMaps.TryGetValue(FieldType.SpecularMap, out map) ? map : ""
                            };
                        GlobalState.Materials[matd.MaterialNameHash.ToString()] = material;
                    }
                    if (meshGroup.HasFlag(MeshFlags.DropShadow))
                    {
                        continue;
                    }
                    var currentPreset = gameObject.AllPresets[presetIndex];
                    loadMeshOnMainThreadCallback(new Volume
                        {
                            ColorData = colors.ToArray(),
                            Faces = faces,
                            Key = matd.MaterialNameHash.ToString(),
                            LODIndex = lodIndex,
                            Material = material,
                            Normals = normals.ToArray(),
                            Object = gameObject,
                            TextureCoordinates = textureCoordinates.ToArray(),
                            Vertices = vertices.ToArray()
                        }, currentPreset, (System.Drawing.Bitmap)currentPreset.Texture.Clone(), null, material, loadTextureCallback);
                }
            }
        }

        public static void LoadMeshOnMainThread(object volume, Preset currentPreset, System.Drawing.Bitmap presetTexture, System.Drawing.Bitmap[] ambientAndSpecularMapTextures, object material, SimBase.LoadTextureDelegate loadTextureCallback)
        {
            var materialCast = (Material)material;
            var volumeCast = (Volume)volume;
            volumeCast.AmbientMapID = loadTextureCallback(currentPreset.AmbientMap ?? materialCast.AmbientMap, null);
            volumeCast.SpecularMapID = loadTextureCallback(currentPreset.SpecularMap ?? materialCast.SpecularMap, null);
            volumeCast.MainTextureID = materialCast.DiffuseMap.Length > 0 && Convert.ToUInt32(materialCast.DiffuseMap.Substring(4, 8), 16) == ResourceUtils.GetResourceType("_IMG") ? loadTextureCallback(materialCast.DiffuseMap, null) : loadTextureCallback(volumeCast.Key, presetTexture);
            GlobalState.Meshes[volumeCast.Key] = volumeCast;
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
    }
}
