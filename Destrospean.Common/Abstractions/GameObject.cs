using System;
using System.Collections.Generic;
using System.IO;
using Destrospean.CmarNYCBorrowed;
using Destrospean.S3PIExtensions;
using Destrospean.zoeoeBorrowed;
using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace Destrospean.Common.Abstractions
{
    public class GameObject : CASTableObject
    {
        ObjKeyResource.ObjKeyResource mObjKeyResource;

        public readonly CatalogResource.CatalogResource CatalogResource;

        public override Rig CurrentRig
        {
            get
            {
                return mCurrentRig;
            }
        }

        public readonly Dictionary<LODId, LODData> LODs = new Dictionary<LODId, LODData>();

        public CatalogResource.ObjectCatalogResource ObjectCatalogResource
        {
            get
            {
                return CatalogResource as CatalogResource.ObjectCatalogResource;
            }
        }

        public ObjKeyResource.ObjKeyResource ObjKeyResource
        {
            get
            {
                if (ObjectCatalogResource == null)
                {
                    return null;
                }
                if (mObjKeyResource == null)
                {
                    mObjKeyResource = ParentPackage.EvaluateResourceKey(ObjectCatalogResource.TGIBlocks[(int)ObjectCatalogResource.OBJKIndex].ReverseEvaluateResourceKey()).GetResource<ObjKeyResource.ObjKeyResource>();
                }
                return mObjKeyResource;
            }
        }

        public new delegate void UpdateUIDelegate(GameObject gameObject, int lodIndex, int groupIndex, uint materialState);

        public GameObject(IPackage package, IResourceIndexEntry resourceIndexEntry, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources) : base(package, resourceIndexEntry)
        {
            CatalogResource = new PackageResourceIndexEntryTuple(package, resourceIndexEntry).GetResource<CatalogResource.CatalogResource>();
            var propertyInfo = CatalogResource.GetType().GetProperty("Materials", typeof(CatalogResource.CatalogResource.MaterialList));
            if (propertyInfo != null)
            {
                Presets.AddRange(((CatalogResource.CatalogResource.MaterialList)propertyInfo.GetValue(CatalogResource, null)).ConvertAll(x => new GameObjectPreset(this, x.MaterialBlock) as Preset));
            }
            LoadLODs(mlodResources, modlResources, vpxyResources);
            LODData lodData;
            if (LODs.TryGetValue(LODId.MediumDetail, out lodData))
            {
                DefaultPresetKey = "key:" + ResourceUtils.GetResourceType("_XML").ToString("X8") + lodData.ResourceKey.Substring(12);
                try
                {   
                    using (var reader = new StreamReader(ParentPackage.EvaluateResourceKey(DefaultPresetKey).Stream))
                    {
                        DefaultPreset = new CASPartPreset(this, reader);
                    }
                }
                catch (ResourceIndexEntryNotFoundException)
                {
                }
            }
        }

        public void AddCASPartPreset(CASPartPreset casPartPreset)
        {
            var materialBlock = new CatalogResource.CatalogResource.MaterialBlock(0, (sender, e) =>
                {
                }, (TGIBlockList)CatalogResource.GetType().GetProperty("TGIBlocks").GetValue(CatalogResource, null))
                {
                    Name = casPartPreset.Patterns.Exists(x => x.SlotName == "Pattern D") ? "ObjectRgbaMask" : "ObjectRgbMask"
                };
            materialBlock.ComplateXMLIndex = (byte)materialBlock.ParentTGIBlocks.Count;
            materialBlock.ParentTGIBlocks.Add(new TGIBlock(0, null, ResourceUtils.GetResourceType("_XML"), 0, System.Security.Cryptography.FNV64.GetHash(materialBlock.Name)));
            foreach (var name in casPartPreset.PropertiesTyped.Keys)
            {
                materialBlock.ComplateOverrides.Add((CatalogResource.CatalogResource.ComplateElement)GameObjectPreset.CreateComplateOverrideInstance(name, casPartPreset[name], casPartPreset.PropertiesTyped[name].Type, materialBlock, ParentPackage));
            }
            foreach (var pattern in casPartPreset.Patterns)
            {
                var patternMaterialBlock = new CatalogResource.CatalogResource.MaterialBlock(0, (sender, e) =>
                    {
                    }, materialBlock.ParentTGIBlocks)
                    {
                        ComplateXMLIndex = (byte)materialBlock.ParentTGIBlocks.Count,
                        Name = pattern.PatternInfo.Name,
                        Pattern = pattern.SlotName
                    };
                patternMaterialBlock.ParentTGIBlocks.Add(new TGIBlock(0, null, ResourceUtils.GetResourceType("_XML"), 0, System.Security.Cryptography.FNV64.GetHash(patternMaterialBlock.Name)));
                var gameObjectPattern = new Pattern(new GameObjectPreset(this, materialBlock), patternMaterialBlock, materialBlock);
                foreach (var name in gameObjectPattern.PropertiesTyped.Keys)
                {
                    patternMaterialBlock.ComplateOverrides.Add((CatalogResource.CatalogResource.ComplateElement)GameObjectPreset.CreateComplateOverrideInstance(name, gameObjectPattern[name], gameObjectPattern.PropertiesTyped[name].Type, patternMaterialBlock, gameObjectPattern.ParentPackage));
                }
                materialBlock.MaterialBlocks.Add(patternMaterialBlock);
            }
            var materials = (CatalogResource.CatalogResource.MaterialList)CatalogResource.GetType().GetProperty("Materials").GetValue(CatalogResource, null);
            var material = new CatalogResource.CatalogResource.Material(0, (sender, e) =>
                {
                }, 1, 0, (ushort)0x42, materialBlock, materialBlock.ParentTGIBlocks, (uint)materials.Count);
            materials.Add(material);
            var preset = new GameObjectPreset(this, material.MaterialBlock);
            Presets.Add(preset);
            for (var i = 0; i < preset.Patterns.Count; i++)
            {
                foreach (var name in preset.Patterns[i].PropertyNames)
                {
                    preset.Patterns[i].SetValue(name, casPartPreset.Patterns[i][name], () =>
                        {
                        });
                }
            }
        }

        public void CloneMeshGroup(LODId lod, int groupIndex, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources, bool shareMaterial = false)
        {
            var lodData = LODs[lod];
            lodData.CloneMeshGroup(groupIndex, shareMaterial);
            ParentPackage.ReplaceResource(ParentPackage.EvaluateResourceKey(lodData.ResourceKey).ResourceIndexEntry, lodData.Resource);
            if (lodData.MLODChunk is MLOD)
            {
                mlodResources[lodData.ResourceKey] = (GenericRCOLResource)lodData.Resource;
            }
            else
            {
                modlResources[lodData.ResourceKey] = (GenericRCOLResource)lodData.Resource;
            }
        }

        public void DeleteMeshGroup(LODId lod, int groupIndex, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            var lodData = LODs[lod];
            lodData.DeleteMeshGroup(groupIndex);
            ParentPackage.ReplaceResource(ParentPackage.EvaluateResourceKey(lodData.ResourceKey).ResourceIndexEntry, lodData.Resource);
            if (lodData.MLODChunk is MLOD)
            {
                mlodResources[lodData.ResourceKey] = (GenericRCOLResource)lodData.Resource;
            }
            else
            {
                modlResources[lodData.ResourceKey] = (GenericRCOLResource)lodData.Resource;
            }
        }

        public override void Dispose()
        {
            CatalogResource.Stream.Close();
            if (ObjKeyResource != null)
            {
                ObjKeyResource.Stream.Close();
            }
            base.Dispose();
        }

        public void ExportMeshGroup(LODId lod, int groupIndex, MeshFileType meshFileType, string filename, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            switch (meshFileType)
            {
                case MeshFileType.MLOD:
                    if (filename.ToLowerInvariant().EndsWith(".lod"))
                    {
                        filename.Remove(filename.LastIndexOf('.'));
                    }
                    using (var fileStream = File.Create(filename + ".lod"))
                    {
                        new BinaryWriter(fileStream).Write(LODs[lod].Resource.AsBytes);
                    }
                    break;
                case MeshFileType.OBJ:
                    if (filename.ToLowerInvariant().EndsWith(".obj"))
                    {
                        filename.Remove(filename.LastIndexOf('.'));
                    }
                    using (var fileStream = File.Create(filename + ".obj"))
                    {
                        var groups = new List<OBJ.Group>();
                        var normals = new List<OBJ.Normal>();
                        var textureCoordinates = new List<OBJ.UV>();
                        var vertices = new List<OBJ.Vertex>();
                        foreach (var meshGroup in LODs[lod].MeshGroups)
                        {
                            if (meshGroup.VertexFormat == null && meshGroup.HasFlag(MeshFlags.ShadowCaster) || groupIndex > -1 && !meshGroup.Equals(LODs[lod].MeshGroups[groupIndex]))
                            {
                                continue;
                            }
                            foreach (var vertex in meshGroup.VertexBuffer.GetVertices(meshGroup.MeshGroup, meshGroup.VertexFormat ?? VRTF.CreateDefaultForMesh(meshGroup.MeshGroup), meshGroup.UVScales))
                            {
                                if (vertex.Normal != null)
                                {
                                    normals.Add(new OBJ.Normal(vertex.Normal));
                                }
                                if (vertex.Position != null)
                                {
                                    vertices.Add(new OBJ.Vertex(vertex.Position));
                                }
                                if (vertex.UV != null)
                                {
                                    textureCoordinates.Add(new OBJ.UV(vertex.UV[0], true));
                                }
                            }
                            var indices = meshGroup.IndexBuffer.GetIndices(meshGroup.MeshGroup);
                            var group = new OBJ.Group("group_" + (groupIndex == -1 ? LODs[lod].MeshGroups.IndexOf(meshGroup) : 0));
                            for (var i = 0; i < indices.Length; i += 3)
                            {
                                group.AddFace(new OBJ.Face(new[]
                                    {
                                        indices[i],
                                        indices[i + 1],
                                        indices[i + 2]
                                    }, 1, OBJ.MeshType.Base));
                            }
                            groups.Add(group);
                        }
                        new OBJ
                        {
                            GroupArray = groups.ToArray(),
                            NormalArray = normals.ToArray(),
                            UVArray = textureCoordinates.ToArray(),
                            VertexArray = vertices.ToArray()
                        }.Write(new StreamWriter(fileStream));
                        break;
                    }
                case MeshFileType.WSO:
                    if (filename.ToLowerInvariant().EndsWith(".wso"))
                    {
                        filename.Remove(filename.LastIndexOf('.'));
                    }
                    using (var fileStream = File.Create(filename + ".wso"))
                    {
                        var groups = new List<WSO.MeshGroup>();
                        foreach (var meshGroup in LODs[lod].MeshGroups)
                        {
                            if (meshGroup.VertexFormat == null && meshGroup.HasFlag(MeshFlags.ShadowCaster) || groupIndex > -1 && !meshGroup.Equals(LODs[lod].MeshGroups[groupIndex]))
                            {
                                continue;
                            }
                            var extendedVertices = new List<WSO.VertexExtended>();
                            foreach (var vertex in meshGroup.VertexBuffer.GetVertices(meshGroup.MeshGroup, meshGroup.VertexFormat ?? VRTF.CreateDefaultForMesh(meshGroup.MeshGroup), meshGroup.UVScales))
                            {
                                var extendedVertex = new WSO.VertexExtended();
                                if (vertex.Normal != null)
                                {
                                    extendedVertex.SetNormals(vertex.Normal);
                                }
                                if (vertex.Position != null)
                                {
                                    extendedVertex.SetPosition(vertex.Position);
                                }
                                if (vertex.UV != null)
                                {
                                    extendedVertex.SetUVs(vertex.UV[0]);
                                }
                                extendedVertices.Add(extendedVertex);
                            }
                            var facePoints = new List<WSO.FacePoint>();
                            var indices = meshGroup.IndexBuffer.GetIndices(meshGroup.MeshGroup);
                            for (var i = 0; i < indices.Length; i++)
                            {
                                facePoints.Add(new WSO.FacePoint(indices[i], extendedVertices[indices[i]].GetNormals(), extendedVertices[indices[i]].GetUVs(), false));
                            }
                            groups.Add(new WSO.MeshGroup(meshGroup.VertexCount, extendedVertices.ToArray(), indices.Length / 3, facePoints.ToArray(), 0, "group_" + (groupIndex == -1 ? LODs[lod].MeshGroups.IndexOf(meshGroup) : 0)));
                        }
                        new WSO(LODs[lod].Resource, CurrentRig, groups.ToArray()).Write(new BinaryWriter(fileStream));
                        break;
                    }
            }
        }

        public void ImportMeshGroup(LODId lod, int groupIndex, string filename, UpdateUIDelegate updateUICallback, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            /*
            foreach (var geometryResourceKvp in geometryResources)
            {
                if (geometryResourceKvp.Value == LODs[lod][groupIndex])
                {
                    var evaluated = ParentPackage.EvaluateResourceKey(geometryResourceKvp.Key);
                    ParentPackage.AddResource(filename, evaluated.ResourceIndexEntry, false);
                    ParentPackage.DeleteResource(evaluated.ResourceIndexEntry);
                    geometryResources[geometryResourceKvp.Key] = new GEOM(new BinaryReader(File.OpenRead(filename)));
                    LoadLODs(geometryResources, vpxyResources);
                    updateUICallback(this, new List<int>(LODs.Keys).IndexOf(lod), groupIndex);
                    break;
                }
            }
            */
        }

        public void ImportMeshGroup(LODId lod, int groupIndex, MeshFileType meshFileType, string filename, UpdateUIDelegate updateUICallback, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            using (var fileStream = File.OpenRead(filename))
            {
                switch (meshFileType)
                {
                    case MeshFileType.OBJ:
                        {
                            var obj = new OBJ(new StreamReader(fileStream));
                            var indexOffset = 0;
                            for (var i = 0; i < obj.GroupCount; i++)
                            {
                                var group = obj.GroupArray[i];
                                if (i >= LODs[lod].MeshGroups.Count || groupIndex > -1 && groupIndex != i)
                                {
                                    continue;
                                }
                                var meshGroup = LODs[lod].MeshGroups[i + indexOffset];
                                if (meshGroup.VertexFormat == null && meshGroup.HasFlag(MeshFlags.ShadowCaster))
                                {
                                    meshGroup = LODs[lod].MeshGroups[i + ++indexOffset];
                                }
                                List<int[]> faces = new List<int[]>(),
                                vertexIndices = new List<int[]>();
                                foreach (var face in group.Faces)
                                {
                                    var temp = new int[3];
                                    int j = 0,
                                    vertexIndex = 0;
                                    foreach (var facePoint in face.FacePoints)
                                    {
                                        if (!obj.TryGetVertexIndex(facePoint, vertexIndices, out vertexIndex, false))
                                        {
                                            temp[j] = vertexIndices.Count;
                                            vertexIndices.Add(facePoint);
                                        }
                                        else
                                        {
                                            temp[j] = vertexIndex;
                                        }
                                        j++;
                                    }
                                    faces.Add(temp);
                                }
                                var vertices = new meshExpImp.ModelBlocks.Vertex[vertexIndices.Count];
                                for (var j = 0; j < vertexIndices.Count; j++)
                                {
                                    var vertexIndex = vertexIndices[j];
                                    vertices[j] = new meshExpImp.ModelBlocks.Vertex
                                        {
                                            Normal = new[]
                                                {
                                                    obj.NormalArray[vertexIndex[2] - 1].Coordinates[0],
                                                    obj.NormalArray[vertexIndex[2] - 1].Coordinates[1],
                                                    obj.NormalArray[vertexIndex[2] - 1].Coordinates[2],
                                                    0 // Needed because of a bug with S3PI where the last value is truncated
                                                },
                                            Position = obj.VertexArray[vertexIndex[0] - 1].Coordinates,
                                            UV = new float[][]
                                                {
                                                    new[]
                                                    {
                                                        obj.UVArray[vertexIndex[1] - 1].Coordinates[0],
                                                        1 - obj.UVArray[vertexIndex[1] - 1].Coordinates[1]
                                                    }
                                                }
                                        };
                                }
                                var faceIndices = new List<int>();
                                foreach (var face in faces)
                                {
                                    faceIndices.AddRange(face);
                                }
                                meshGroup.IndexBuffer.SetIndices(LODs[lod].MLODChunk, meshGroup.MeshGroup, faceIndices.ToArray());
                                meshGroup.VertexBuffer.SetVertices(LODs[lod].MLODChunk, meshGroup.MeshGroup, meshGroup.VertexFormat ?? VRTF.CreateDefaultForMesh(meshGroup.MeshGroup), vertices, meshGroup.UVScales);
                            }
                            break;
                        }
                    case MeshFileType.WSO:
                        {
                            var wso = new WSO(new BinaryReader(fileStream));
                            var indexOffset = 0;
                            for (var i = 0; i < wso.MeshCount; i++)
                            {
                                var group = wso.GetMesh(i);
                                if (i >= LODs[lod].MeshGroups.Count || groupIndex > -1 && groupIndex != i)
                                {
                                    continue;
                                }
                                var meshGroup = LODs[lod].MeshGroups[i + indexOffset];
                                if (meshGroup.VertexFormat == null && meshGroup.HasFlag(MeshFlags.ShadowCaster))
                                {
                                    meshGroup = LODs[lod].MeshGroups[i + ++indexOffset];
                                }
                                var vertices = new meshExpImp.ModelBlocks.Vertex[group.VertexCount];
                                var indices = new int[group.FacePointCount];
                                for (var j = 0; j < indices.Length; j++)
                                {
                                    var facePoint = group.GetFacePoint(j);
                                    indices[j] = facePoint.VertexIndex;
                                    vertices[facePoint.VertexIndex] = new meshExpImp.ModelBlocks.Vertex
                                        {
                                            Normal = new[]
                                                {
                                                    facePoint.Normals[0],
                                                    facePoint.Normals[1],
                                                    facePoint.Normals[2],
                                                    0 // Needed because of a bug with S3PI where the last value is truncated
                                                },
                                            Position = group.GetVertex(facePoint.VertexIndex).Position,
                                            UV = new float[][]
                                                {
                                                    facePoint.UVs
                                                }
                                        };
                                }
                                meshGroup.IndexBuffer.SetIndices(LODs[lod].MLODChunk, meshGroup.MeshGroup, indices);
                                meshGroup.VertexBuffer.SetVertices(LODs[lod].MLODChunk, meshGroup.MeshGroup, meshGroup.VertexFormat ?? VRTF.CreateDefaultForMesh(meshGroup.MeshGroup), vertices, meshGroup.UVScales);
                            }
                            break;
                        }
                }
                LoadLODs(mlodResources, modlResources, vpxyResources);
                updateUICallback(this, new List<LODId>(LODs.Keys).IndexOf(lod), groupIndex, (uint)MTST.State.Default);
            }
        }

        public void LoadLODs(Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            GenericRCOLResource vpxyResource = null;
            if (ObjKeyResource != null)
            {
                var vpxyResourceIndexEntry = ParentPackage.GetResourceIndexEntry(ObjKeyResource.TGIBlocks[0]);
                var vpxyKey = vpxyResourceIndexEntry.ReverseEvaluateResourceKey();
                if (!vpxyResources.TryGetValue(vpxyKey, out vpxyResource))
                {
                    vpxyResources.Add(vpxyKey, new GenericRCOLResource(0, ((APackage)ParentPackage).GetResource(vpxyResourceIndexEntry)));
                    vpxyResource = vpxyResources[vpxyKey];
                }
            }
            if (vpxyResource == null)
            {
                return;
            }
            foreach (var entry in ((s3pi.GenericRCOLResource.VPXY)vpxyResource.ChunkEntries[0].RCOLBlock).Entries)
            {
                var entry01 = entry as s3pi.GenericRCOLResource.VPXY.Entry01;
                if (entry01 == null)
                {
                    continue;
                }
                switch (entry01.ParentTGIBlocks[entry01.TGIIndex].GetResourceTypeTag())
                {
                    case "_RIG":
                        using (var reader = new BinaryReader(ParentPackage.EvaluateResourceKey(entry01.ParentTGIBlocks[entry01.TGIIndex].ReverseEvaluateResourceKey()).Stream))
                        {
                            mCurrentRig = new Rig(reader);
                        }
                        break;
                    case "MODL":
                        var modlResourceIndexEntry = ParentPackage.GetResourceIndexEntry(entry01.ParentTGIBlocks[entry01.TGIIndex]);
                        var modlKey = modlResourceIndexEntry.ReverseEvaluateResourceKey();
                        GenericRCOLResource modlResource;
                        if (!modlResources.TryGetValue(modlKey, out modlResource))
                        {
                            modlResources.Add(modlKey, new GenericRCOLResource(0, ((APackage)ParentPackage).GetResource(modlResourceIndexEntry)));
                            modlResource = modlResources[modlKey];
                        }
                        LODs.Clear();
                        foreach (var lodEntry in ((MODL)modlResource.ChunkEntries.Find(x => x.RCOLBlock.Tag == "MODL").RCOLBlock).Entries)
                        {
                            if (lodEntry.ModelLodIndex.RefType == GenericRCOLResource.ReferenceType.Public)
                            {
                                LODs.Add(lodEntry.Id, new LODData(lodEntry.Id, modlKey, modlResource, (MLOD)modlResource.ChunkEntries[lodEntry.ModelLodIndex.TGIBlockIndex].RCOLBlock));
                                continue;
                            }
                            if (lodEntry.ModelLodIndex.RefType == GenericRCOLResource.ReferenceType.Delayed)
                            {
                                var mlodResourceIndexEntry = ParentPackage.GetResourceIndexEntry(modlResource.Resources[lodEntry.ModelLodIndex.TGIBlockIndex]);
                                var mlodKey = mlodResourceIndexEntry.ReverseEvaluateResourceKey();
                                GenericRCOLResource mlodResource;
                                if (!mlodResources.TryGetValue(mlodKey, out mlodResource))
                                {
                                    mlodResources.Add(mlodKey, new GenericRCOLResource(0, ((APackage)ParentPackage).GetResource(mlodResourceIndexEntry)));
                                    mlodResource = mlodResources[mlodKey];
                                }
                                LODs.Add(lodEntry.Id, new LODData(lodEntry.Id, mlodKey, mlodResource, (MLOD)mlodResource.ChunkEntries.Find(x => x.RCOLBlock.Tag == "MLOD").RCOLBlock));
                                continue;
                            }
                            break;
                        }
                        break;
                }
            }
        }

        public override void SavePresets()
        {
            SaveDefaultPreset();
            var propertyInfo = CatalogResource.GetType().GetProperty("Materials", typeof(CatalogResource.CatalogResource.MaterialList));
            if (propertyInfo != null)
            {
                var materials = ((CatalogResource.CatalogResource.MaterialList)propertyInfo.GetValue(CatalogResource, null));
                var materialsReordered = new List<CatalogResource.CatalogResource.Material>();
                for (var i = 0; i < Presets.Count; i++)
                {
                    materialsReordered.Add(materials.Find(x => x.MaterialBlock == ((GameObjectPreset)Presets[i]).MaterialBlock));
                }
                materials.Clear();
                materials.AddRange(materialsReordered);
            }
        }
    }
}
