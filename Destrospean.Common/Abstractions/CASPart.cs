using System;
using System.Collections.Generic;
using System.IO;
using Destrospean.CmarNYCBorrowed;
using Destrospean.S3PIExtensions;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3pi.WrapperDealer;

namespace Destrospean.Common.Abstractions
{
    public class CASPart : CASTableObject
    {
        public AgeGender AdjustedAge
        {
            get
            {
                var age = (AgeGender)(uint)CASPartResource.AgeGender.Age;
                return age >= AgeGender.Teen && age <= AgeGender.Elder ? AgeGender.Adult : age;
            }
        }

        public Species AdjustedSpecies
        {
            get
            {
                var species = (Species)((uint)CASPartResource.AgeGender.Species << 8);
                return species == 0 ? Species.Human : species;
            }
        }

        public static readonly string CacheFilePath = string.Format("{0}{1}Destrospean{1}CASPartLookupCache", Platform.CacheDirectoryPath, Path.DirectorySeparatorChar);
            
        public static Dictionary<string, Dictionary<string, uint>> CASPartLookupCache;

        public readonly CASPartResource.CASPartResource CASPartResource;

        public override Rig CurrentRig
        {
            get
            {
                if (mCurrentRig == null)
                {
                    mCurrentRig = MeshUtils.GetRig(ParentPackage, AdjustedSpecies, AdjustedAge);
                }
                return mCurrentRig;
            }
        }

        public readonly Dictionary<int, List<GEOMAndKey>> LODs = new Dictionary<int, List<GEOMAndKey>>();

        public struct GEOMAndKey
        {
            public GEOM GEOM;

            public string Key;

            public GEOMAndKey(string key, GEOM geom)
            {
                GEOM = geom;
                Key = key;
            }
        }

        public CASPart(IPackage package, IResourceIndexEntry resourceIndexEntry, Dictionary<string, GEOM> geometryResources, Dictionary<string, GenericRCOLResource> vpxyResources) : base(package, resourceIndexEntry)
        {
            var defaultPresetResourceIndexEntries = ParentPackage.FindAll(x => x.ResourceType == ResourceUtils.GetResourceType("_XML") && x.Instance == resourceIndexEntry.Instance);
            if (defaultPresetResourceIndexEntries.Count > 0)
            {
                var stream = ((APackage)ParentPackage).GetResource(defaultPresetResourceIndexEntries[0]);
                {
                    DefaultPresetKey = defaultPresetResourceIndexEntries[0].ReverseEvaluateResourceKey();
                    using (var reader = new StreamReader(stream))
                    {
                        DefaultPreset = new CASPartPreset(this, reader);
                    }
                }
            }
            CASPartResource = (CASPartResource.CASPartResource)WrapperDealer.GetResource(0, package, resourceIndexEntry);
            Presets.AddRange(CASPartResource.Presets.ConvertAll(x => new CASPartPreset(this, x.XmlFile) as Preset));
            LoadLODs(geometryResources, vpxyResources);
            if (LODs.Count == 0)
            {
                for (var i = 0; i < 4; i++)
                {
                    LODs[i] = new List<GEOMAndKey>();
                }
            }
        }

        public void AddMeshGroup(int lod, Dictionary<string, GEOM> geometryResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            var vpxyResourceIndexEntry = ParentPackage.GetResourceIndexEntry(CASPartResource.TGIBlocks[CASPartResource.VPXYIndexes[0]]);
            var vpxyKey = vpxyResourceIndexEntry.ReverseEvaluateResourceKey();
            GenericRCOLResource vpxyResource;
            if (!vpxyResources.TryGetValue(vpxyKey, out vpxyResource))
            {
                vpxyResources.Add(vpxyKey, (GenericRCOLResource)WrapperDealer.GetResource(0, ParentPackage, vpxyResourceIndexEntry));
                vpxyResource = vpxyResources[vpxyKey];
            }
            using (var reader = new BinaryReader(vpxyResource.Stream))
            {
                var vpxy = new CmarNYCBorrowed.VPXY(reader);
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
                        using (var geomStreamWriter = new BinaryWriter(geomStream))
                        {
                            geom.Write(geomStreamWriter);
                        }
                        var newGEOMResourceIndexEntry = ParentPackage.AddResource(new ResourceKey(newGEOMTGI.Type, newGEOMTGI.Group, newGEOMTGI.Instance), geomStream, true);
                        using (var geomStreamReader = new BinaryReader(geomStream))
                        {
                            geometryResources.Add(newGEOMResourceIndexEntry.ReverseEvaluateResourceKey(), new GEOM(geomStreamReader));
                        }
                        geomTGIList.Add(newGEOMTGI);
                    }
                    geomTGIs[i] = geomTGIList.ToArray();
                }
                var vpxyStream = new MemoryStream();
                using (var vpxyStreamWriter = new BinaryWriter(vpxyStream))
                {
                    new CmarNYCBorrowed.VPXY(new TGI(vpxyResourceIndexEntry.ResourceType, vpxyResourceIndexEntry.ResourceGroup, vpxyResourceIndexEntry.Instance), vpxy.BondLinks, geomTGIs).Write(vpxyStreamWriter);
                }
                vpxyResource = new GenericRCOLResource(0, vpxyStream);
                ParentPackage.ReplaceResource(vpxyResourceIndexEntry, vpxyResource);
                vpxyResources[vpxyKey] = vpxyResource;
            }
        }

        public void AdjustPresetCount()
        {
            while (CASPartResource.Presets.Count < Presets.Count)
            {
                CASPartResource.Presets.Add(new CASPartResource.CASPartResource.Preset(0, null));
            }
            while (CASPartResource.Presets.Count > Presets.Count)
            {
                CASPartResource.Presets.RemoveAt(0);
            }
        }

        public void DeleteMeshGroup(int lod, int groupIndex, Dictionary<string, GEOM> geometryResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            var vpxyResourceIndexEntry = ParentPackage.GetResourceIndexEntry(CASPartResource.TGIBlocks[CASPartResource.VPXYIndexes[0]]);
            var vpxyKey = vpxyResourceIndexEntry.ReverseEvaluateResourceKey();
            GenericRCOLResource vpxyResource;
            if (!vpxyResources.TryGetValue(vpxyKey, out vpxyResource))
            {
                vpxyResources.Add(vpxyKey, (GenericRCOLResource)WrapperDealer.GetResource(0, ParentPackage, vpxyResourceIndexEntry));
                vpxyResource = vpxyResources[vpxyKey];
            }
            using (var reader = new BinaryReader(vpxyResource.Stream))
            {
                var vpxy = new CmarNYCBorrowed.VPXY(reader);
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
                using (var vpxyStreamWriter = new BinaryWriter(vpxyStream))
                {
                    new CmarNYCBorrowed.VPXY(new TGI(vpxyResourceIndexEntry.ResourceType, vpxyResourceIndexEntry.ResourceGroup, vpxyResourceIndexEntry.Instance), vpxy.BondLinks, geomTGIs).Write(vpxyStreamWriter);
                }
                vpxyResource = new GenericRCOLResource(0, vpxyStream);
                ParentPackage.ReplaceResource(vpxyResourceIndexEntry, vpxyResource);
                vpxyResources[vpxyKey] = vpxyResource;
            }
        }

        public override void Dispose()
        {
            CASPartResource.Stream.Close();
            base.Dispose();
        }

        public void ExportMeshGroup(int lod, int groupIndex, MeshFileType meshFileType, string filename, Dictionary<string, GEOM> geometryResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            var geom = LODs[lod][groupIndex].GEOM;
            byte[] bblnIndices =
                {
                    CASPartResource.BlendInfoFatIndex,
                    CASPartResource.BlendInfoFitIndex,
                    CASPartResource.BlendInfoThinIndex,
                    CASPartResource.BlendInfoSpecialIndex
                };
            var morphs = new GEOM[bblnIndices.Length];
            for (var i = 0; i < bblnIndices.Length; i++)
            {
                BBLN bbln;
                EvaluatedResourceKey evaluated;
                try
                {
                    evaluated = ParentPackage.EvaluateResourceKey(CASPartResource.TGIBlocks[bblnIndices[i]].ReverseEvaluateResourceKey());
                    using (var reader = new BinaryReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)))
                    {
                        bbln = new BBLN(reader);
                    }
                }
                catch (ResourceIndexEntryNotFoundException)
                {
                    morphs[i] = null;
                    continue;
                }
                BGEO bgeo = null;
                try
                {
                    evaluated = ParentPackage.EvaluateResourceKey(new ResourceKey(bbln.BGEOTGI.Type, bbln.BGEOTGI.Group, bbln.BGEOTGI.Instance).ReverseEvaluateResourceKey());
                    using (var reader = new BinaryReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)))
                    {
                        bgeo = new BGEO(reader);
                    }
                }
                catch (ResourceIndexEntryNotFoundException)
                {
                }
                foreach (var entry in bbln.Entries)
                {
                    foreach (var geomMorph in entry.GEOMMorphs)
                    {
                        if (bgeo != null)
                        {
                            morphs[i] = new GEOM(geom, bgeo, bgeo.GetSection1EntryIndex(AdjustedSpecies, (AgeGender)(uint)CASPartResource.AgeGender.Age, (AgeGender)((uint)CASPartResource.AgeGender.Gender << 12)), lod);
                        }
                        else if (bbln.TGIList != null && bbln.TGIList.Length > geomMorph.TGIIndex && geom.HasVertexIDs)
                        {
                            try
                            {
                                evaluated = ParentPackage.EvaluateResourceKey(new ResourceKey(bbln.TGIList[geomMorph.TGIIndex].Type, bbln.TGIList[geomMorph.TGIIndex].Group, bbln.TGIList[geomMorph.TGIIndex].Instance).ReverseEvaluateResourceKey());
                                using (var reader = new BinaryReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)))
                                {
                                    var vpxy = new CmarNYCBorrowed.VPXY(reader);
                                    foreach (var link in vpxy.GetMeshLinks(lod))
                                    {
                                        try
                                        {
                                            using (var geomStreamReader = new BinaryReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)))
                                            {
                                                evaluated = ParentPackage.EvaluateResourceKey(new ResourceKey(link.Type, link.Group, link.Instance).ReverseEvaluateResourceKey());
                                                morphs[i] = new GEOM(geomStreamReader);
                                            }
                                        }
                                        catch (ResourceIndexEntryNotFoundException)
                                        {
                                            morphs[i] = null;
                                        }
                                    }
                                }

                            }
                            catch (ResourceIndexEntryNotFoundException)
                            {
                                morphs[i] = null;
                            }
                        }
                    }
                }
            }
            switch (meshFileType)
            {
                case MeshFileType.GEOM:
                    if (filename.ToLowerInvariant().EndsWith(".simgeom"))
                    {
                        filename.Remove(filename.LastIndexOf('.'));
                    }
                    using (var fileStream = File.Create(filename + ".simgeom"))
                    {
                        using (var writer = new BinaryWriter(fileStream))
                        {
                            geom.Write(writer);
                        }
                    }
                    for (var i = 0; i < Array.FindAll(morphs, x => x.IsValid).Length; i++)
                    {
                        if (morphs[i] != null)
                        {
                            using (var fileStream = File.Create(filename + "_" + "fat fit thin special".Split(' ')[i] + ".simgeom"))
                            {
                                using (var writer = new BinaryWriter(fileStream))
                                {
                                    morphs[i].Write(writer);
                                }
                            }
                        }
                    }
                    break;
                case MeshFileType.OBJ:
                    using (var fileStream = File.Create(filename + (filename.ToLowerInvariant().EndsWith(".obj") ? "" : ".obj")))
                    {
                        using (var writer = new StreamWriter(fileStream))
                        {
                            new OBJ(geom, Array.ConvertAll(morphs, x => x.IsValid ? x : null)).Write(writer);
                        }
                    }
                    break;
                case MeshFileType.WSO:
                    using (var fileStream = File.Create(filename + (filename.ToLowerInvariant().EndsWith(".wso") ? "" : ".wso")))
                    {
                        using (var writer = new BinaryWriter(fileStream))
                        {
                            new WSO(geom, morphs).Write(writer);
                        }
                    }
                    break;
            }
        }

        public static void GenerateLookupCache()
        {
            CASPartLookupCache = new Dictionary<string, Dictionary<string, uint>>();
            foreach (var gamePackageKvp in ResourceUtils.GameContentPackages)
            {
                var resourceType = ResourceUtils.GetResourceType("CASP");
                foreach (var casPartResourceIndexEntry in gamePackageKvp.Value.FindAll(x => x.ResourceType == resourceType))
                {
                    var key = casPartResourceIndexEntry.ReverseEvaluateResourceKey();
                    var casPartResource = (CASPartResource.CASPartResource)WrapperDealer.GetResource(0, gamePackageKvp.Value, casPartResourceIndexEntry);
                    if (CASPartLookupCache.ContainsKey(key))
                    {
                        continue;
                    }
                    CASPartLookupCache[key] = new Dictionary<string, uint>
                    {
                        {
                            "Age",
                            (uint)casPartResource.AgeGender.Age
                        },
                        {
                            "Clothing",
                            (uint)casPartResource.Clothing
                        },
                        {
                            "ClothingCategory",
                            (uint)casPartResource.ClothingCategory
                        },
                        {
                            "DataType",
                            (uint)casPartResource.DataType
                        },
                        {
                            "Gender",
                            (uint)casPartResource.AgeGender.Gender
                        },
                        {
                            "Handedness",
                            (uint)casPartResource.AgeGender.Handedness
                        },
                        {
                            "Species",
                            (uint)casPartResource.AgeGender.Species
                        }
                    };
                }
            }
            SaveLookupCache();
        }

        public static CASPartResource.SpeciesType GetAdjustedSpecies(CASPartResource.SpeciesType species)
        {
            return (uint)species == 0 ? (CASPartResource.SpeciesType)1 : species;
        }

        public void ImportMeshGroup(int lod, int groupIndex, string filename, UpdateUIDelegate updateUICallback, Dictionary<string, GEOM> geometryResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            var evaluated = ParentPackage.EvaluateResourceKey(LODs[lod][groupIndex].Key);
            ParentPackage.AddResource(filename, evaluated.ResourceIndexEntry, false);
            ParentPackage.DeleteResource(evaluated.ResourceIndexEntry);
            using (var reader = new BinaryReader(File.OpenRead(filename)))
            {
                geometryResources[LODs[lod][groupIndex].Key] = new GEOM(reader);
            }
            LoadLODs(geometryResources, vpxyResources);
            updateUICallback(this, new List<int>(LODs.Keys).IndexOf(lod), groupIndex);
        }

        public void ImportMeshGroup(int lod, int groupIndex, MeshFileType meshFileType, string filename, UpdateUIDelegate updateUICallback, Dictionary<string, GEOM> geometryResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            var geom = LODs[lod][groupIndex].GEOM;
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
                    using (var reader = new BinaryReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)))
                    {
                        bbln = new BBLN(reader);
                        bblnResourceIndexEntries[i] = evaluated.ResourceIndexEntry;
                    }
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
                GEOM[] newGEOMPlusMorphs = null;
                switch (meshFileType)
                {
                    case MeshFileType.OBJ:
                        using (var reader = new StreamReader(fileStream))
                        {
                            newGEOMPlusMorphs = GEOM.GEOMsFromOBJ(new OBJ(reader), geom, new TGI(), false, false);
                        }
                        break;
                    case MeshFileType.WSO:
                        using (var reader = new BinaryReader(fileStream))
                        {
                            newGEOMPlusMorphs = GEOM.GEOMsFromWSO(new WSO(reader), geom, new TGI());
                        }
                        break;
                }
                for (var i = newGEOMPlusMorphs.Length - 1; i > -1 ; i--)
                {
                    var stream = new MemoryStream();
                    using (var writer = new BinaryWriter(stream))
                    {
                        newGEOMPlusMorphs[i].Write(writer);
                    }
                    if (i == 0)
                    {
                        var evaluated = ParentPackage.EvaluateResourceKey(LODs[lod][groupIndex].Key);
                        ParentPackage.AddResource(evaluated.ResourceIndexEntry, stream, false);
                        ParentPackage.DeleteResource(evaluated.ResourceIndexEntry);
                        geometryResources[LODs[lod][groupIndex].Key] = newGEOMPlusMorphs[i];
                        LoadLODs(geometryResources, vpxyResources);
                        updateUICallback(this, new List<int>(LODs.Keys).IndexOf(lod), groupIndex);
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
                                using (var reader = new BinaryReader(((APackage)morphEvaluated.Package).GetResource(morphEvaluated.ResourceIndexEntry)))
                                {
                                    lodMorphMeshes[j] = LODs.ContainsKey(j) ? new GEOM[]
                                        {
                                            j == lod ? newGEOMPlusMorphs[i] : new GEOM(LODs[j][groupIndex].GEOM, new BGEO(reader), 0, j)
                                        } : new GEOM[0];
                                }
                            }
                        }
                        else
                        {
                            using (var reader = new BinaryReader(((APackage)morphEvaluated.Package).GetResource(morphEvaluated.ResourceIndexEntry)))
                            {
                                var vpxy = new CmarNYCBorrowed.VPXY(reader);
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
                        }
                        var vpxyResourceKey = morphEvaluated.ResourceIndexEntry.ReverseEvaluateResourceKey();
                        if (vpxyResources.ContainsKey(vpxyResourceKey))
                        {
                            vpxyResources.Remove(vpxyResourceKey);
                        }
                        var bgeoTGI = new TGI(ResourceUtils.GetResourceType("BGEO"), 0, bblnResourceIndexEntries[i - 1].Instance);
                        var newBBLN = new BBLN(8, CASPartResource.Unknown1 + morphName, bgeoTGI);
                        var newBGEO = new BGEO(lodMorphMeshes);
                        var resourceStream = new MemoryStream();
                        using (var writer = new BinaryWriter(resourceStream))
                        {
                            newBBLN.Write(writer);
                        } 
                        ParentPackage.DeleteResource(morphEvaluated.ResourceIndexEntry);
                        ParentPackage.DeleteResource(bblnResourceIndexEntries[i - 1]);
                        ParentPackage.AddResource(bblnResourceIndexEntries[i - 1], resourceStream, true);
                        resourceStream = new MemoryStream();
                        using (var writer = new BinaryWriter(resourceStream))
                        {
                            newBGEO.Write(writer);
                        } 
                        ParentPackage.AddResource(new TGIBlock(0, null, bgeoTGI.Type, bgeoTGI.Group, bgeoTGI.Instance), resourceStream, true);
                        CASPartResource.TGIBlocks[bblnIndices[i - 1]].ResourceGroup = bblnResourceIndexEntries[i - 1].ResourceGroup;
                        CASPartResource.TGIBlocks[bblnIndices[i - 1]].Instance = bblnResourceIndexEntries[i - 1].Instance;
                    }
                }
            }
        }

        public void LoadLODs(Dictionary<string, GEOM> geometryResources, Dictionary<string, GenericRCOLResource> vpxyResources)
        {
            foreach (var vpxyIndex in CASPartResource.VPXYIndexes)
            {
                var vpxyKey = CASPartResource.TGIBlocks[vpxyIndex].ReverseEvaluateResourceKey();
                GenericRCOLResource vpxyResource;
                if (!vpxyResources.TryGetValue(vpxyKey, out vpxyResource))
                {
                    EvaluatedResourceKey evaluated;
                    try
                    {
                        evaluated = ParentPackage.EvaluateResourceKey(vpxyKey);
                    }
                    catch (ResourceIndexEntryNotFoundException)
                    {
                        continue;
                    }
                    vpxyResources.Add(vpxyKey, (GenericRCOLResource)WrapperDealer.GetResource(0, evaluated.Package, evaluated.ResourceIndexEntry));
                    vpxyResource = vpxyResources[vpxyKey];
                }
                foreach (var entry in ((s3pi.GenericRCOLResource.VPXY)vpxyResource.ChunkEntries[0].RCOLBlock).Entries)
                {
                    var entry00 = entry as s3pi.GenericRCOLResource.VPXY.Entry00;
                    if (entry00 != null)
                    {
                        LODs[entry00.EntryID] = new List<GEOMAndKey>();
                        foreach (var tgiIndex in entry00.TGIIndexes)
                        {
                            var geometryResourceKey = entry00.ParentTGIBlocks[tgiIndex].ReverseEvaluateResourceKey();
                            GEOM geometryResource;
                            if (!geometryResources.TryGetValue(geometryResourceKey, out geometryResource))
                            {
                                var evaluated = ParentPackage.EvaluateResourceKey(geometryResourceKey);
                                using (var reader = new BinaryReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)))
                                {
                                    geometryResources.Add(geometryResourceKey, new GEOM(reader));
                                    geometryResource = geometryResources[geometryResourceKey];
                                }
                            }
                            LODs[entry00.EntryID].Add(new GEOMAndKey(geometryResourceKey, geometryResource));
                        }
                    }
                }
            }
        }

        public static void LoadLookupCache()
        {
            if (File.Exists(CacheFilePath))
            {
                using (var reader = new Newtonsoft.Json.Bson.BsonReader(new FileStream(CacheFilePath, FileMode.Open)))
                {
                    CASPartLookupCache = new Newtonsoft.Json.JsonSerializer().Deserialize<Dictionary<string, Dictionary<string, uint>>>(reader);
                }
            }
        }

        public static void SaveLookupCache()
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(CacheFilePath));
            using (var writer = new Newtonsoft.Json.Bson.BsonWriter(new FileStream(CacheFilePath, FileMode.Create)))
            {
                new Newtonsoft.Json.JsonSerializer().Serialize(writer, CASPartLookupCache);
            }
        }

        public void SavePreset(int index)
        {
            CASPartResource.Presets[index].Unknown1 = (uint)index;
            CASPartResource.Presets[index].XmlFile = ((CASPartPreset)Presets[index]).XmlFile;
        }

        public override void SavePresets()
        {
            SaveDefaultPreset();
            AdjustPresetCount();
            for (var i = 0; i < CASPartResource.Presets.Count; i++)
            {
                SavePreset(i);
            }
        }
    }
}
