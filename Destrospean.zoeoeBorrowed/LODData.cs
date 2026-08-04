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

        enum ChunkReferenceIndices
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
                float[] uvScales =
                    {   
                        -1,
                        -1,
                        -1
                    };
                if (matd != null)
                {
                    foreach (var element in matd.Mtnf.SData)
                    {
                        if (element.Field == FieldType.UVScales)
                        {
                            var elementFloat3 = ((ElementFloat3)element);
                            uvScales = new[]
                                {
                                    elementFloat3.Data0,
                                    elementFloat3.Data1,
                                    elementFloat3.Data2
                                };
                        }
                    }
                }
                MeshGroups.Add(mtst == null ? new MeshGroupData(vertexFormat, vertexBuffer, indexBuffer, matd, meshGroup, skinController, uvScales) : new MeshGroupData(vertexFormat, vertexBuffer, indexBuffer, mtst, meshGroup, skinController, uvScales));
            }
        }

        public void CloneMeshGroup(int groupIndex)
        {
            var resource = (GenericRCOLResource)Resource;
            var meshGroup = MeshGroups[groupIndex];
            var mesh = (MLOD.Mesh)meshGroup.MeshGroup.Clone((sender, e) =>
                {
                });
            GenericRCOLResource.ChunkEntry indexBuffer = (GenericRCOLResource.ChunkEntry)resource.ChunkEntries[mesh.IndexBufferIndex.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
                {
                }),
            material = (GenericRCOLResource.ChunkEntry)resource.ChunkEntries[mesh.MaterialIndex.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
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
            mesh.MaterialIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
            resource.ChunkEntries.Add(material);
            mesh.SkinControllerIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
            resource.ChunkEntries.Add(skinController);
            mesh.VertexBufferIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
            resource.ChunkEntries.Add(vertexBuffer);
            mesh.VertexFormatIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
            resource.ChunkEntries.Add(vertexFormat);
            if (meshGroup.DirectMATD == null)
            {
                foreach (var entry in ((MTST)material.RCOLBlock).Entries)
                {
                    resource.ChunkEntries.Add((GenericRCOLResource.ChunkEntry)resource.ChunkEntries[entry.Index.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
                        {
                        }));
                    entry.Index.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks - 1;
                }
            }
            var uvScales = (float[])meshGroup.UVScales.Clone();
            MLODChunk.Meshes.Add(mesh);
            MeshGroups.Add(meshGroup.MaterialSet == null ? new MeshGroupData((VRTF)vertexFormat.RCOLBlock, (VBUF)vertexBuffer.RCOLBlock, (IBUF)indexBuffer.RCOLBlock, (MATD)material.RCOLBlock, mesh, (SKIN)skinController.RCOLBlock, uvScales) : new MeshGroupData((VRTF)vertexFormat.RCOLBlock, (VBUF)vertexBuffer.RCOLBlock, (IBUF)indexBuffer.RCOLBlock, (MTST)material.RCOLBlock, mesh, (SKIN)skinController.RCOLBlock, uvScales));
        }

        public void DeleteMeshGroup(int groupIndex)
        {
            var resource = (GenericRCOLResource)Resource;
            var chunkReferenceMap = new Dictionary<string, Dictionary<ChunkReferenceIndices, TGIBlock>>();
            var mtstEntryMap = new Dictionary<string, Dictionary<MTST.Entry, TGIBlock>>();
            foreach (var meshGroup in MeshGroups)
            {
                var material = resource.ChunkEntries[meshGroup.MeshGroup.MaterialIndex.TGIBlockIndex + resource.PublicChunks];
                chunkReferenceMap[meshGroup.ID] = new Dictionary<ChunkReferenceIndices, TGIBlock>
                {
                    {
                        ChunkReferenceIndices.IndexBufferIndex,
                        resource.ChunkEntries[meshGroup.MeshGroup.IndexBufferIndex.TGIBlockIndex + resource.PublicChunks].TGIBlock
                    },
                    {
                        ChunkReferenceIndices.MaterialIndex,
                        material.TGIBlock
                    },
                    {
                        ChunkReferenceIndices.SkinControllerIndex,
                        resource.ChunkEntries[meshGroup.MeshGroup.SkinControllerIndex.TGIBlockIndex + resource.PublicChunks].TGIBlock
                    },
                    {
                        ChunkReferenceIndices.VertexBufferIndex,
                        resource.ChunkEntries[meshGroup.MeshGroup.VertexBufferIndex.TGIBlockIndex + resource.PublicChunks].TGIBlock
                    },
                    {
                        ChunkReferenceIndices.VertexFormatIndex,
                        resource.ChunkEntries[meshGroup.MeshGroup.VertexFormatIndex.TGIBlockIndex + resource.PublicChunks].TGIBlock
                    }
                };
                if (meshGroup.DirectMATD == null)
                {
                    mtstEntryMap[meshGroup.ID] = new Dictionary<MTST.Entry, TGIBlock>();
                    foreach (var entry in meshGroup.MaterialSet.Entries)
                    {
                        mtstEntryMap[meshGroup.ID][entry] = resource.ChunkEntries[entry.Index.TGIBlockIndex + resource.PublicChunks].TGIBlock;
                    }
                }
            }
            var indices = new List<int>();
            foreach (var name in System.Enum.GetNames(typeof(ChunkReferenceIndices)))
            {
                var id = MeshGroups[groupIndex].ID;
                var index = ((GenericRCOLResource.ChunkReference)typeof(MLOD.Mesh).GetProperty(name).GetValue(MeshGroups[groupIndex].MeshGroup)).TGIBlockIndex;
                if (!MeshGroups.Exists(x => x.ID != id && ((GenericRCOLResource.ChunkReference)typeof(MLOD.Mesh).GetProperty(name).GetValue(x.MeshGroup)).TGIBlockIndex == index))
                {
                    indices.Add(index + resource.PublicChunks);
                }
            }
            indices.Sort((a, b) => b.CompareTo(a));
            foreach (var index in indices)
            {
                resource.ChunkEntries.RemoveAt(index);
            }
            MeshGroups.RemoveAt(groupIndex);
            MLODChunk.Meshes.RemoveAt(groupIndex);
            foreach (var meshGroup in MeshGroups)
            {
                meshGroup.MeshGroup.IndexBufferIndex.TGIBlockIndex = resource.ChunkEntries.FindIndex(x => x.TGIBlock.Equals(chunkReferenceMap[meshGroup.ID][ChunkReferenceIndices.IndexBufferIndex])) - resource.PublicChunks;
                meshGroup.MeshGroup.MaterialIndex.TGIBlockIndex = resource.ChunkEntries.FindIndex(x => x.TGIBlock.Equals(chunkReferenceMap[meshGroup.ID][ChunkReferenceIndices.MaterialIndex])) - resource.PublicChunks;
                meshGroup.MeshGroup.SkinControllerIndex.TGIBlockIndex = resource.ChunkEntries.FindIndex(x => x.TGIBlock.Equals(chunkReferenceMap[meshGroup.ID][ChunkReferenceIndices.SkinControllerIndex])) - resource.PublicChunks;
                meshGroup.MeshGroup.VertexBufferIndex.TGIBlockIndex = resource.ChunkEntries.FindIndex(x => x.TGIBlock.Equals(chunkReferenceMap[meshGroup.ID][ChunkReferenceIndices.VertexBufferIndex])) - resource.PublicChunks;
                meshGroup.MeshGroup.VertexFormatIndex.TGIBlockIndex = resource.ChunkEntries.FindIndex(x => x.TGIBlock.Equals(chunkReferenceMap[meshGroup.ID][ChunkReferenceIndices.VertexFormatIndex])) - resource.PublicChunks;
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
