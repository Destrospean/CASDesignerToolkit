using System;
using System.Collections.Generic;
using System.IO;
using Destrospean.CmarNYCBorrowed;
using Destrospean.S3PIExtensions;
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

        public readonly Dictionary<LODId, zoeoeBorrowed.MeshUtils.LODData> LODs = new Dictionary<LODId, zoeoeBorrowed.MeshUtils.LODData>();

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

        public void ExportMeshGroup(LODId lod, int groupIndex, MeshFileType meshFileType, string filename, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            switch (meshFileType)
            {
                case MeshFileType.MODL:
                    if (filename.ToLowerInvariant().EndsWith(".model"))
                    {
                        filename.Remove(filename.LastIndexOf('.'));
                    }
                    using (var fileStream = File.Create(filename + ".model"))
                    {
                        new BinaryWriter(fileStream).Write(LODs[lod].MLODResource.AsBytes);
                    }
                    break;
                case MeshFileType.OBJ:
                    using (var fileStream = File.Create(filename + (filename.ToLowerInvariant().EndsWith(".obj") ? "" : ".obj")))
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
                                if (vertex.UV != null)
                                {
                                    foreach (var uvSet in vertex.UV)
                                    {
                                        textureCoordinates.Add(new OBJ.UV(uvSet));
                                    }
                                }
                                if (vertex.Position != null)
                                {
                                    vertices.Add(new OBJ.Vertex(vertex.Position));
                                }
                            }
                            groups.Add(new OBJ.Group("group_" + (groupIndex == -1 ? LODs[lod].MeshGroups.IndexOf(meshGroup) : 0)));
                            var indices = meshGroup.IndexBuffer.GetIndices(meshGroup.MeshGroup);
                            for (var i = 0; i < indices.Length; i += 3)
                            {
                                groups[groups.Count - 1].AddFace(new OBJ.Face
                                    {
                                        Point1 = new int[]
                                            {
                                                indices[i] + 1,
                                                indices[i] + 1,
                                            },
                                        Point2 = new int[]
                                            {
                                                indices[i + 1] + 1,
                                                indices[i + 1] + 1,
                                            },
                                        Point3 = new int[]
                                            {
                                                indices[i + 2] + 1,
                                                indices[i + 2] + 1,
                                            }
                                    });
                            }
                        }
                        var obj = new OBJ
                            {
                                GroupArray = groups.ToArray(),
                                NormalArray = normals.ToArray(),
                                UVArray = textureCoordinates.ToArray(),
                                VertexArray = vertices.ToArray()
                            };
                        obj.FlipUV(true, false);
                        obj.Write(new StreamWriter(fileStream));
                    }
                    break;
                case MeshFileType.WSO:
                    /*
                    using (var fileStream = File.Create(filename + (filename.ToLowerInvariant().EndsWith(".wso") ? "" : ".wso")))
                    {
                        new WSO(meshGroup, morphs).Write(new BinaryWriter(fileStream));
                    }
                    */
                    break;
            }
        }

        public void ImportMesh(LODId lod, int groupIndex, string filename, System.Action<CASPart, int, int> updateUICallback, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources)
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

        public void ImportMeshGroup(LODId lod, int groupIndex, MeshFileType meshFileType, string filename, System.Action<CASPart, int, int> updateUICallback, Dictionary<string, GenericRCOLResource> mlodResources, Dictionary<string, GenericRCOLResource> modlResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            /*
            var geom = LODs[lod][groupIndex];
            byte[] bblnIndices =
                {
                    CASPartResource.BlendInfoFatIndex,
                    CASPartResource.BlendInfoFitIndex,
                    CASPartResource.BlendInfoThinIndex,
                    CASPartResource.BlendInfoSpecialIndex
                };
            var bblnResourceIndexEntries = new IResourceIndexEntry[bblnIndices.Length];
            var morphsEvaluated = new EvaluatedResourceKey?[bblnIndices.Length];
            for (var i = 0; i < bblnIndices.Length; i++)
            {
                BBLN bbln;
                EvaluatedResourceKey evaluated;
                try
                {
                    evaluated = ParentPackage.EvaluateResourceKey(CASPartResource.TGIBlocks[bblnIndices[i]].ReverseEvaluateResourceKey());
                    bbln = new BBLN(new BinaryReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)));
                    bblnResourceIndexEntries[i] = evaluated.ResourceIndexEntry;
                }
                catch (ResourceIndexEntryNotFoundException)
                {
                    morphsEvaluated[i] = null;
                    continue;
                }
                try
                {
                    morphsEvaluated[i] = ParentPackage.EvaluateResourceKey(new ResourceKey(bbln.BGEOTGI.Type, bbln.BGEOTGI.Group, bbln.BGEOTGI.Instance).ReverseEvaluateResourceKey());
                    continue;
                }
                catch (ResourceIndexEntryNotFoundException)
                {
                }
                foreach (var entry in bbln.Entries)
                {
                    foreach (var geomMorph in entry.GEOMMorphs)
                    {
                        if (bbln.TGIList != null && bbln.TGIList.Length > geomMorph.TGIIndex && geom.HasVertexIDs)
                        {
                            try
                            {
                                morphsEvaluated[i] = ParentPackage.EvaluateResourceKey(new ResourceKey(bbln.TGIList[geomMorph.TGIIndex].Type, bbln.TGIList[geomMorph.TGIIndex].Group, bbln.TGIList[geomMorph.TGIIndex].Instance).ReverseEvaluateResourceKey());
                            }
                            catch (ResourceIndexEntryNotFoundException)
                            {
                                morphsEvaluated[i] = null;
                            }
                        }
                    }
                }
            }
            using (var fileStream = File.OpenRead(filename))
            {
                var newGEOMPlusMorphs = GEOM.GEOMsFromOBJ(meshFileType == MeshFileType.OBJ ? new OBJ(new StreamReader(fileStream)) : meshFileType == MeshFileType.WSO ? new OBJ(new WSO(new BinaryReader(fileStream))) : null, geom, new TGI(), false, false);
                for (var i = newGEOMPlusMorphs.Length - 1; i > -1 ; i--)
                {
                    var stream = new MemoryStream();
                    newGEOMPlusMorphs[i].Write(new BinaryWriter(stream));
                    if (i == 0)
                    {
                        foreach (var geometryResourceKvp in geometryResources)
                        {
                            if (geometryResourceKvp.Value == LODs[lod][groupIndex])
                            {
                                var evaluated = ParentPackage.EvaluateResourceKey(geometryResourceKvp.Key);
                                ParentPackage.AddResource(evaluated.ResourceIndexEntry, stream, false);
                                ParentPackage.DeleteResource(evaluated.ResourceIndexEntry);
                                geometryResources[geometryResourceKvp.Key] = newGEOMPlusMorphs[i];
                                LoadLODs(geometryResources, vpxyResources);
                                updateUICallback(this, new List<int>(LODs.Keys).IndexOf(lod), groupIndex);
                                break;
                            }
                        }
                    }
                    else if (morphsEvaluated[i - 1].HasValue)
                    {
                        var lodMorphMeshes = new GEOM[4][];
                        var morphEvaluated = morphsEvaluated[i - 1].Value;
                        var morphName = "_fat _fit _thin _special".Split(' ')[i - 1];
                        if (morphEvaluated.ResourceIndexEntry.GetResourceTypeTag() == "BGEO")
                        {
                            for (var j = 0; j < lodMorphMeshes.Length; j++)
                            {
                                lodMorphMeshes[j] =  LODs.ContainsKey(j) ? new GEOM[]
                                    {
                                        j == lod ? newGEOMPlusMorphs[i] : new GEOM(LODs[j][groupIndex], new BGEO(new BinaryReader(((APackage)morphEvaluated.Package).GetResource(morphEvaluated.ResourceIndexEntry))), 0, j)
                                    } : new GEOM[0];
                            }
                        }
                        else
                        {
                            var vpxy = new CmarNYCBorrowed.VPXY(new BinaryReader(((APackage)morphEvaluated.Package).GetResource(morphEvaluated.ResourceIndexEntry)));
                            for (var j = 0; j < lodMorphMeshes.Length; j++)
                            {
                                lodMorphMeshes[j] = j == lod ? new GEOM[]
                                    {
                                        newGEOMPlusMorphs[i]
                                    } : Array.ConvertAll(vpxy.GetMeshLinks(j), x => geometryResources[new ResourceKey(x.Type, x.Group, x.Instance).ReverseEvaluateResourceKey()]);
                            }
                            for (var j = 0; j < lodMorphMeshes.Length; j++)
                            {
                                var meshLinks = vpxy.GetMeshLinks(j);
                                for (var k = 0; k < meshLinks.Length; k++)
                                {
                                    var key = new ResourceKey(meshLinks[k].Type, meshLinks[k].Group, meshLinks[k].Instance).ReverseEvaluateResourceKey();
                                    var evaluated = ParentPackage.EvaluateResourceKey(key);
                                    evaluated.Package.DeleteResource(evaluated.ResourceIndexEntry);
                                    geometryResources.Remove(key);
                                }
                            }
                        }
                        var geomTGIs = new TGI[lodMorphMeshes.Length][];
                        var group = 0u;
                        foreach (var j in CASPartResource.Diffuse1Indexes)
                        {
                            group = CASPartResource.TGIBlocks[j].ResourceGroup;
                        }
                        foreach (var j in CASPartResource.Specular1Indexes)
                        {
                            group = CASPartResource.TGIBlocks[j].ResourceGroup;
                        }
                        for (var j = 0; j < lodMorphMeshes.Length; j++)
                        {
                            geomTGIs[j] = new TGI[lodMorphMeshes[j].Length];
                            for (var k = 0; k < geomTGIs[j].Length; k++)
                            {
                                var temp = "_lod" + j.ToString();
                                if (k > 0)
                                {
                                    temp += "-" + (k + 1).ToString();
                                }
                                temp += morphName;
                                geomTGIs[j][k] = new TGI(ResourceUtils.GetResourceType("GEOM"), group, System.Security.Cryptography.FNV64.GetHash(CASPartResource.Unknown1 + morphName + Environment.UserName + Environment.TickCount.ToString() + temp));
                                var geomResourceKey = new TGIBlock(0, null, geomTGIs[j][k].Type, geomTGIs[j][k].Group, geomTGIs[j][k].Instance);
                                var geomStream = new MemoryStream();
                                lodMorphMeshes[j][k].Write(new BinaryWriter(geomStream));
                                var geomResourceIndexEntry = ParentPackage.AddResource(geomResourceKey, geomStream, true);
                                geometryResources[geomResourceIndexEntry.ReverseEvaluateResourceKey()] = new GEOM(lodMorphMeshes[j][k]);
                            }
                        }
                        var vpxyTGI = new TGI(ResourceUtils.GetResourceType("VPXY"), 1, bblnResourceIndexEntries[i - 1].Instance);
                        var newVPXY = new CmarNYCBorrowed.VPXY(vpxyTGI, geomTGIs);
                        var newBBLN = new BBLN(7, CASPartResource.Unknown1 + morphName, vpxyTGI);
                        var vpxyResourceKey = new TGIBlock(0, null, bblnResourceIndexEntries[i - 1].ResourceType, bblnResourceIndexEntries[i - 1].ResourceGroup, bblnResourceIndexEntries[i - 1].Instance);
                        var vpxyStream = new MemoryStream();
                        newBBLN.Write(new BinaryWriter(vpxyStream));
                        ParentPackage.DeleteResource(morphEvaluated.ResourceIndexEntry);
                        ParentPackage.DeleteResource(bblnResourceIndexEntries[i - 1]);
                        ParentPackage.AddResource(vpxyResourceKey, vpxyStream, true);
                        vpxyResourceKey = new TGIBlock(0, null, vpxyTGI.Type, vpxyTGI.Group, vpxyTGI.Instance);
                        vpxyStream = new MemoryStream();
                        newVPXY.Write(new BinaryWriter(vpxyStream));
                        var vpxyResourceIndexEntry = ParentPackage.AddResource(vpxyResourceKey, vpxyStream, true);
                        vpxyResources[vpxyResourceIndexEntry.ReverseEvaluateResourceKey()] = (GenericRCOLResource)WrapperDealer.GetResource(0, ParentPackage, vpxyResourceIndexEntry);
                        CASPartResource.TGIBlocks[bblnIndices[i - 1]].ResourceGroup = bblnResourceIndexEntries[i - 1].ResourceGroup;
                        CASPartResource.TGIBlocks[bblnIndices[i - 1]].Instance = bblnResourceIndexEntries[i - 1].Instance;
                    }
                }
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
                if (entry01 != null && entry01.ParentTGIBlocks[entry01.TGIIndex].ResourceType == ResourceUtils.GetResourceType("MODL"))
                {
                    var modlResourceIndexEntry = ParentPackage.GetResourceIndexEntry(entry01.ParentTGIBlocks[entry01.TGIIndex]);
                    var modlKey = modlResourceIndexEntry.ReverseEvaluateResourceKey();
                    GenericRCOLResource modlResource;
                    if (!modlResources.TryGetValue(modlKey, out modlResource))
                    {
                        modlResources.Add(modlKey, (GenericRCOLResource)WrapperDealer.GetResource(0, ParentPackage, modlResourceIndexEntry));
                        modlResource = modlResources[modlKey];
                    }
                    LODs.Clear();
                    foreach (var lodEntry in ((MODL)modlResource.ChunkEntries[0].RCOLBlock).Entries)
                    {
                        //outer resource that MLOD belongs to & public chunk count for it
                        GenericRCOLResource resourceWithMLOD = null;
                        MLOD mlodToLoad = null;
                        var key = "";
                        if (lodEntry.ModelLodIndex.RefType == GenericRCOLResource.ReferenceType.Public) //MLOD is internal, assuming for low LOD. Note: public means don't use public chunk count
                        {
                            resourceWithMLOD = modlResource;
                            mlodToLoad = (MLOD)resourceWithMLOD.ChunkEntries[lodEntry.ModelLodIndex.TGIBlockIndex].RCOLBlock;
                        }
                        else if (lodEntry.ModelLodIndex.RefType == GenericRCOLResource.ReferenceType.Delayed) //MLOD in external resource, assuming for shadows and high LOD
                        {
                            var mlodResourceIndexEntry = ParentPackage.GetResourceIndexEntry(modlResource.Resources[lodEntry.ModelLodIndex.TGIBlockIndex]);
                            var mlodKey = key = mlodResourceIndexEntry.ReverseEvaluateResourceKey();
                            GenericRCOLResource mlodResource;
                            if (!mlodResources.TryGetValue(mlodKey, out mlodResource))
                            {
                                mlodResources.Add(mlodKey, (GenericRCOLResource)WrapperDealer.GetResource(0, ParentPackage, mlodResourceIndexEntry));
                                mlodResource = mlodResources[mlodKey];
                            }
                            using (var mlodStream = ((APackage)ParentPackage).GetResource(mlodResourceIndexEntry))
                            {
                                resourceWithMLOD = new GenericRCOLResource(0, mlodStream);
                            }
                            mlodToLoad = (MLOD)resourceWithMLOD.ChunkEntries[0].RCOLBlock; //internal MLOD is chunk 0 of external MLOD resource
                        }
                        else
                        {
                            return;
                        }
                        var lodData = zoeoeBorrowed.MeshUtils.LoadMLODData(key, resourceWithMLOD, resourceWithMLOD.PublicChunks, mlodToLoad);
                        lodData.ID = lodEntry.Id;
                        LODs.Add(lodData.ID, lodData);
                    }
                    break;
                }
            }
        }

        public override void SavePresets()
        {
            SaveDefaultPreset();
        }
    }
}
