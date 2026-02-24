using System;
using System.Collections.Generic;
using System.IO;
using Destrospean.CmarNYCBorrowed;
using Destrospean.S3PIExtensions;
using Destrospean.zoeoeBorrowed;
using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3pi.WrapperDealer;

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
                    var evaluated = ParentPackage.EvaluateResourceKey(ObjectCatalogResource.TGIBlocks[(int)ObjectCatalogResource.OBJKIndex].ReverseEvaluateResourceKey());
                    mObjKeyResource = (ObjKeyResource.ObjKeyResource)WrapperDealer.GetResource(0, evaluated.Package, evaluated.ResourceIndexEntry);
                }
                return mObjKeyResource;
            }
        }

        public GameObject(IPackage package, IResourceIndexEntry resourceIndexEntry, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources) : base(package, resourceIndexEntry)
        {
            CatalogResource = (CatalogResource.CatalogResource)WrapperDealer.GetResource(0, package, resourceIndexEntry);
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
                    var defaultPresetResourceEvaluated = ParentPackage.EvaluateResourceKey(DefaultPresetKey);
                    using (var reader = new StreamReader(((APackage)defaultPresetResourceEvaluated.Package).GetResource(defaultPresetResourceEvaluated.ResourceIndexEntry)))
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
                    preset.Patterns[i][name] = casPartPreset.Patterns[i][name];
                }
            }
        }

        public void AddMeshGroup(LODId lod, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            /*
            var vpxyResourceIndexEntry = ParentPackage.GetResourceIndexEntry(CASPartResource.TGIBlocks[CASPartResource.VPXYIndexes[0]]);
            var vpxyKey = vpxyResourceIndexEntry.ReverseEvaluateResourceKey();
            GenericRCOLResource vpxyResource;
            if (!vpxyResources.TryGetValue(vpxyKey, out vpxyResource))
            {
                vpxyResources.Add(vpxyKey, (GenericRCOLResource)WrapperDealer.GetResource(0, ParentPackage, vpxyResourceIndexEntry));
                vpxyResource = vpxyResources[vpxyKey];
            }
            var vpxy = new CmarNYCBorrowed.VPXY(new BinaryReader(vpxyResource.Stream));
            var geomTGIs = new TGI[4][];
            for (var i = 0; i < geomTGIs.GetLength(0); i++)
            {
                var geomTGIList = new List<TGI>(vpxy.GetMeshLinks(i));
                if (i == lod || lod == -1)
                {
                    var temp = "_lod" + i.ToString() + "-" + (geomTGIList.Count + 1).ToString();
                    var newGEOMTGI = new TGI(ResourceUtils.GetResourceType("GEOM"), geomTGIList[geomTGIList.Count - 1].Group, System.Security.Cryptography.FNV64.GetHash(CASPartResource.Unknown1 + temp + Environment.UserName + Environment.TickCount.ToString() + temp));
                    var geomStream = new MemoryStream();
                    var geom = geometryResources[new ResourceKey(geomTGIList[geomTGIList.Count - 1].Type, geomTGIList[geomTGIList.Count - 1].Group, geomTGIList[geomTGIList.Count - 1].Instance).ReverseEvaluateResourceKey()];
                    geom.Write(new BinaryWriter(geomStream));
                    var newGEOMResourceIndexEntry = ParentPackage.AddResource(new ResourceKey(newGEOMTGI.Type, newGEOMTGI.Group, newGEOMTGI.Instance), geomStream, true);
                    geometryResources.Add(newGEOMResourceIndexEntry.ReverseEvaluateResourceKey(), new GEOM(new BinaryReader(geomStream)));
                    geomTGIList.Add(newGEOMTGI);
                }
                geomTGIs[i] = geomTGIList.ToArray();
            }
            var vpxyStream = new MemoryStream();
            new CmarNYCBorrowed.VPXY(new TGI(vpxyResourceIndexEntry.ResourceType, vpxyResourceIndexEntry.ResourceGroup, vpxyResourceIndexEntry.Instance), vpxy.BondLinks, geomTGIs).Write(new BinaryWriter(vpxyStream));
            vpxyResource = new GenericRCOLResource(0, vpxyStream);
            ParentPackage.ReplaceResource(vpxyResourceIndexEntry, vpxyResource);
            vpxyResources[vpxyKey] = vpxyResource;
            */
        }

        public void DeleteMeshGroup(LODId lod, int groupIndex, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            /*
            var vpxyResourceIndexEntry = ParentPackage.GetResourceIndexEntry(CASPartResource.TGIBlocks[CASPartResource.VPXYIndexes[0]]);
            var vpxyKey = vpxyResourceIndexEntry.ReverseEvaluateResourceKey();
            GenericRCOLResource vpxyResource;
            if (!vpxyResources.TryGetValue(vpxyKey, out vpxyResource))
            {
                vpxyResources.Add(vpxyKey, (GenericRCOLResource)WrapperDealer.GetResource(0, ParentPackage, vpxyResourceIndexEntry));
                vpxyResource = vpxyResources[vpxyKey];
            }
            var vpxy = new CmarNYCBorrowed.VPXY(new BinaryReader(vpxyResource.Stream));
            var geomTGIs = new TGI[4][];
            for (var i = 0; i < geomTGIs.GetLength(0); i++)
            {
                var geomTGIList = new List<TGI>(vpxy.GetMeshLinks(i));
                if (i == lod || lod == -1)
                {
                    var geomKey = new ResourceKey(geomTGIList[groupIndex].Type, geomTGIList[groupIndex].Group, geomTGIList[groupIndex].Instance).ReverseEvaluateResourceKey();
                    ParentPackage.DeleteResource(ParentPackage.EvaluateResourceKey(geomKey).ResourceIndexEntry);
                    geometryResources.Remove(geomKey);
                    geomTGIList.RemoveAt(groupIndex);
                }
                geomTGIs[i] = geomTGIList.ToArray();
            }
            var vpxyStream = new MemoryStream();
            new CmarNYCBorrowed.VPXY(new TGI(vpxyResourceIndexEntry.ResourceType, vpxyResourceIndexEntry.ResourceGroup, vpxyResourceIndexEntry.Instance), vpxy.BondLinks, geomTGIs).Write(new BinaryWriter(vpxyStream));
            vpxyResource = new GenericRCOLResource(0, vpxyStream);
            ParentPackage.ReplaceResource(vpxyResourceIndexEntry, vpxyResource);
            vpxyResources[vpxyKey] = vpxyResource;
            */
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
            var extension = "";
            switch (meshFileType)
            {
                case MeshFileType.MLOD:
                    extension = ".lod";
                    if (filename.ToLowerInvariant().EndsWith(extension))
                    {
                        filename.Remove(filename.LastIndexOf('.'));
                    }
                    using (var fileStream = File.Create(filename + extension))
                    {
                        using (var writer = new BinaryWriter(fileStream))
                        {
                            writer.Write(LODs[lod].Resource.AsBytes);
                        }
                    }
                    break;
                case MeshFileType.OBJ:
                    extension = ".obj";
                    goto case MeshFileType.WSO;
                case MeshFileType.WSO:
                    if (string.IsNullOrEmpty(extension))
                    {
                        extension = ".wso";
                    }
                    using (var fileStream = File.Create(filename + (filename.ToLowerInvariant().EndsWith(extension) ? "" : extension)))
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
                        var wso = new WSO(LODs[lod].Resource, CurrentRig, groups.ToArray());
                        switch (meshFileType)
                        {
                            case MeshFileType.OBJ:
                                using (var writer = new StreamWriter(fileStream))
                                {
                                    new OBJ(wso).Write(writer);
                                }
                                break;
                            case MeshFileType.WSO:
                                using (var writer = new BinaryWriter(fileStream))
                                {
                                    wso.Write(writer);
                                }
                                break;
                        }
                    }
                    break;
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
            /*
            var mlod = (MLOD)((GenericRCOLResource)LODs[lod].Resource).ChunkEntries.Find(x => x.RCOLBlock.Tag == "MLOD").RCOLBlock;
            using (var fileStream = File.OpenRead(filename))
            {
                var wso = new WSO(new BinaryReader(fileStream));
                for (var i = 0; i < LODs[lod].MeshGroups.Count; i++)
                {
                    var meshGroup = LODs[lod].MeshGroups[i];
                    if (groupIndex > -1 && groupIndex != i)
                    {
                        continue;
                    }
                    var group = wso.GetMesh(groupIndex == -1 ? i : 0);
                    var vertices = new List<meshExpImp.ModelBlocks.Vertex>();
                    foreach (var extendedVertex in group.GetExtendedVertices())
                    {
                        vertices.Add(new meshExpImp.ModelBlocks.Vertex 
                            {
                                Normal = extendedVertex.GetNormals(),
                                Position = extendedVertex.GetPosition(),
                                UV = new float[][]
                                    {
                                        extendedVertex.GetUVs()
                                    }
                            });
                    }
                    meshGroup.VertexBuffer.SetVertices(mlod, meshGroup.MeshGroup, meshGroup.VertexFormat, vertices.ToArray(), meshGroup.UVScales);
                    var indices = new int[group.FacePointCount];
                    for (var j = 0; j < indices.Length; j += 3)
                    {
                        indices[j] = group.GetFacePoint(j).VertexIndex;
                    }
                    meshGroup.IndexBuffer.SetIndices(mlod, meshGroup.MeshGroup, indices);
                }
                LoadLODs(mlodResources, modlResources, vpxyResources);
                updateUICallback(this, new List<LODId>(LODs.Keys).IndexOf(lod), groupIndex);
            }
            */
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
                    vpxyResources.Add(vpxyKey, (GenericRCOLResource)WrapperDealer.GetResource(0, ParentPackage, vpxyResourceIndexEntry));
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
                        var evaluated = ParentPackage.EvaluateResourceKey(entry01.ParentTGIBlocks[entry01.TGIIndex].ReverseEvaluateResourceKey());
                        using (var reader = new BinaryReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)))
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
                            modlResources.Add(modlKey, (GenericRCOLResource)WrapperDealer.GetResource(0, ParentPackage, modlResourceIndexEntry));
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
                                    mlodResources.Add(mlodKey, (GenericRCOLResource)WrapperDealer.GetResource(0, ParentPackage, mlodResourceIndexEntry));
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
