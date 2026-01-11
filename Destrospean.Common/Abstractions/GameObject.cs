using System;
using System.Collections.Generic;
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
                Presets.AddRange(((CatalogResource.CatalogResource.MaterialList)propertyInfo.GetValue(CatalogResource, null)).ConvertAll(x => new Material(this, x.MaterialBlock) as IPreset));
            }
            LoadLODs(mlodResources, modlResources, vpxyResources);
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
                        if (lodEntry.ModelLodIndex.RefType == GenericRCOLResource.ReferenceType.Public) //MLOD is internal, assuming for low LOD. Note: public means don't use public chunk count
                        {
                            resourceWithMLOD = modlResource;
                            mlodToLoad = (MLOD)resourceWithMLOD.ChunkEntries[lodEntry.ModelLodIndex.TGIBlockIndex].RCOLBlock;
                        }
                        else if (lodEntry.ModelLodIndex.RefType == GenericRCOLResource.ReferenceType.Delayed) //MLOD in external resource, assuming for shadows and high LOD
                        {
                            var mlodResourceIndexEntry = ParentPackage.GetResourceIndexEntry(modlResource.Resources[lodEntry.ModelLodIndex.TGIBlockIndex]);
                            var mlodKey = mlodResourceIndexEntry.ReverseEvaluateResourceKey();
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
                        var lodData = zoeoeBorrowed.MeshUtils.LoadMLODData(resourceWithMLOD, resourceWithMLOD.PublicChunks, mlodToLoad);
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
