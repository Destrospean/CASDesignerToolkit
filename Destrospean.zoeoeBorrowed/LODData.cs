using System.Collections.Generic;
using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace Destrospean.zoeoeBorrowed
{
    public struct LODData
    {
        public List<MeshGroupData> MeshGroups;

        public MLOD MLODChunk;

        public IResource Resource;

        public string ResourceKey;

        enum ChunkReferences
        {
            IndexBufferIndex,
            MaterialIndex,
            SkinControllerIndex,
            VertexBufferIndex,
            VertexFormatIndex
        }

        public LODData(LODId id, string key, GenericRCOLResource resource, MLOD mlodChunk)
        {
            Resource = resource;
            ResourceKey = key;
            MeshGroups = new List<MeshGroupData>();
            MLODChunk = mlodChunk;
            foreach (var meshGroup in MLODChunk.Meshes)
            {
                var indexBuffer = resource.ChunkEntries[meshGroup.IndexBufferIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock as IBUF;
                var materialIndexBlock = resource.ChunkEntries[meshGroup.MaterialIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock;
                var skinController = resource.ChunkEntries[meshGroup.SkinControllerIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock as SKIN;
                var vertexBuffer = resource.ChunkEntries[meshGroup.VertexBufferIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock as VBUF;
                var vertexFormat = resource.ChunkEntries[meshGroup.VertexFormatIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock as VRTF;
                var mtst = materialIndexBlock as MTST;
                var matd = mtst == null ? materialIndexBlock as MATD : resource.ChunkEntries[mtst.Entries.Find(x => x.MaterialState == MTST.State.Default).Index.TGIBlockIndex + resource.PublicChunks].RCOLBlock as MATD;
                MeshGroups.Add(mtst == null ? new MeshGroupData(resource, vertexFormat, vertexBuffer, indexBuffer, matd, meshGroup, skinController) : new MeshGroupData(resource, vertexFormat, vertexBuffer, indexBuffer, mtst, meshGroup, skinController));
            }
        }

        public void CloneMeshGroup(int groupIndex, bool shareMaterial = false)
        {
            var resource = (GenericRCOLResource)Resource;
            var meshGroup = MeshGroups[groupIndex];
            var mesh = (MLOD.Mesh)meshGroup.MeshGroup.Clone((sender, e) =>
                {
                });
            GenericRCOLResource.ChunkEntry indexBuffer = (GenericRCOLResource.ChunkEntry)resource.ChunkEntries[mesh.IndexBufferIndex.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
                {
                }),
            material = shareMaterial ? null : (GenericRCOLResource.ChunkEntry)resource.ChunkEntries[mesh.MaterialIndex.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
                {
                }),
            skinController = (GenericRCOLResource.ChunkEntry)resource.ChunkEntries[mesh.SkinControllerIndex.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
                {
                }),
            vertexBuffer = (GenericRCOLResource.ChunkEntry)resource.ChunkEntries[mesh.VertexBufferIndex.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
                {
                }),
            vertexFormat = (GenericRCOLResource.ChunkEntry)resource.ChunkEntries[mesh.VertexFormatIndex.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
                {
                });
            mesh.IndexBufferIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
            resource.ChunkEntries.Add(indexBuffer);
            if (!shareMaterial)
            {
                mesh.MaterialIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
                resource.ChunkEntries.Add(material);
            }
            mesh.SkinControllerIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
            resource.ChunkEntries.Add(skinController);
            mesh.VertexBufferIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
            resource.ChunkEntries.Add(vertexBuffer);
            mesh.VertexFormatIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
            resource.ChunkEntries.Add(vertexFormat);
            if (!shareMaterial && meshGroup.DirectMATD == null)
            {
                foreach (var entry in ((MTST)material.RCOLBlock).Entries)
                {
                    resource.ChunkEntries.Add((GenericRCOLResource.ChunkEntry)resource.ChunkEntries[entry.Index.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
                        {
                        }));
                    entry.Index.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks - 1;
                }
            }
            MLODChunk.Meshes.Add(mesh);
            MeshGroups.Add(meshGroup.MaterialSet == null ? new MeshGroupData(resource, vertexFormat.RCOLBlock as VRTF, vertexBuffer.RCOLBlock as VBUF, indexBuffer.RCOLBlock as IBUF, material?.RCOLBlock as MATD ?? meshGroup.DirectMATD, mesh, skinController.RCOLBlock as SKIN) : new MeshGroupData(resource, vertexFormat.RCOLBlock as VRTF, vertexBuffer.RCOLBlock as VBUF, indexBuffer.RCOLBlock as IBUF, material?.RCOLBlock as MTST ?? meshGroup.MaterialSet, mesh, skinController.RCOLBlock as SKIN));
        }

        public void DeleteMeshGroup(int groupIndex)
        {
            var resource = (GenericRCOLResource)Resource;
            var chunkReferenceMap = new Dictionary<string, Dictionary<ChunkReferences, TGIBlock>>();
            var mtstEntryMap = new Dictionary<string, Dictionary<MTST.Entry, TGIBlock>>();
            foreach (var meshGroup in MeshGroups)
            {
                chunkReferenceMap[meshGroup.ID] = new Dictionary<ChunkReferences, TGIBlock>();
                foreach (ChunkReferences chunkReference in System.Enum.GetValues(typeof(ChunkReferences)))
                {
                    chunkReferenceMap[meshGroup.ID][chunkReference] = resource.ChunkEntries[((GenericRCOLResource.ChunkReference)meshGroup.MeshGroup.GetType().GetProperty(chunkReference.ToString()).GetValue(meshGroup.MeshGroup)).TGIBlockIndex + resource.PublicChunks].TGIBlock;
                }
                if (meshGroup.DirectMATD == null)
                {
                    mtstEntryMap[meshGroup.ID] = new Dictionary<MTST.Entry, TGIBlock>();
                    foreach (var entry in meshGroup.MaterialSet.Entries)
                    {
                        mtstEntryMap[meshGroup.ID][entry] = resource.ChunkEntries[entry.Index.TGIBlockIndex + resource.PublicChunks].TGIBlock;
                    }
                }
            }
            var chunkEntryIndicesToRemove = new List<int>();
            foreach (var chunkReferenceName in System.Enum.GetNames(typeof(ChunkReferences)))
            {
                var meshGroup = MeshGroups[groupIndex];
                var index = ((GenericRCOLResource.ChunkReference)meshGroup.MeshGroup.GetType().GetProperty(chunkReferenceName).GetValue(meshGroup.MeshGroup)).TGIBlockIndex;
                if (!MeshGroups.Exists(x => x != meshGroup && ((GenericRCOLResource.ChunkReference)x.MeshGroup.GetType().GetProperty(chunkReferenceName).GetValue(x.MeshGroup)).TGIBlockIndex == index))
                {
                    chunkEntryIndicesToRemove.Add(index + resource.PublicChunks);
                }
            }
            if (MeshGroups[groupIndex].DirectMATD == null)
            {
                var meshGroup = MeshGroups[groupIndex];
                foreach (var tgiBlock in mtstEntryMap[meshGroup.ID].Values)
                {
                    var index = resource.ChunkEntries.FindIndex(x => x.TGIBlock.Equals(tgiBlock));
                    if (index > -1 && !MeshGroups.Exists(x => x != meshGroup && x.DirectMATD == null && ((MTST)resource.ChunkEntries[x.MeshGroup.MaterialIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock).Entries.Exists(y => y.Index.TGIBlockIndex == index - resource.PublicChunks)))
                    {
                        chunkEntryIndicesToRemove.Add(index);
                    }
                }
            }
            chunkEntryIndicesToRemove.Sort((a, b) => b.CompareTo(a));
            foreach (var index in chunkEntryIndicesToRemove)
            {
                resource.ChunkEntries.RemoveAt(index);
            }
            MeshGroups.RemoveAt(groupIndex);
            MLODChunk.Meshes.RemoveAt(groupIndex);
            foreach (var meshGroup in MeshGroups)
            {
                foreach (ChunkReferences chunkReference in System.Enum.GetValues(typeof(ChunkReferences)))
                {
                    ((GenericRCOLResource.ChunkReference)meshGroup.MeshGroup.GetType().GetProperty(chunkReference.ToString()).GetValue(meshGroup.MeshGroup)).TGIBlockIndex = resource.ChunkEntries.FindIndex(x => x.TGIBlock.Equals(chunkReferenceMap[meshGroup.ID][chunkReference])) - resource.PublicChunks;
                }
                if (meshGroup.DirectMATD == null)
                {
                    foreach (var entry in ((MTST)resource.ChunkEntries[meshGroup.MeshGroup.MaterialIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock).Entries)
                    {
                        entry.Index.TGIBlockIndex = resource.ChunkEntries.FindIndex(x => x.TGIBlock.Equals(mtstEntryMap[meshGroup.ID][entry])) - resource.PublicChunks;
                    }
                }
            }
        }
    }
}
