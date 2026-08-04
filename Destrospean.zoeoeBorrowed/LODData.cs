using System.Collections.Generic;
using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;

namespace Destrospean.zoeoeBorrowed
{
    public struct LODData
    {
        public List<MeshGroupData> MeshGroups;

        public MLOD MLODChunk;

        public s3pi.Interfaces.IResource Resource;

        public string ResourceKey;

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
            MeshGroups.Add(meshGroup.DirectMATD == null ? new MeshGroupData((VRTF)vertexFormat.RCOLBlock, (VBUF)vertexBuffer.RCOLBlock, (IBUF)indexBuffer.RCOLBlock, (MTST)material.RCOLBlock, mesh, (SKIN)skinController.RCOLBlock, uvScales) : new MeshGroupData((VRTF)vertexFormat.RCOLBlock, (VBUF)vertexBuffer.RCOLBlock, (IBUF)indexBuffer.RCOLBlock, (MATD)material.RCOLBlock, mesh, (SKIN)skinController.RCOLBlock, uvScales));
        }

        public void DeleteMeshGroup(int groupIndex)
        {
            /*
            var resource = (GenericRCOLResource)Resource;
            var indices = new List<int>();
            foreach (var property in typeof(MLOD.Mesh).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                switch (property.Name)
                {
                    case "IndexBufferIndex":
                    case "MaterialIndex":
                    case "SkinControllerIndex":
                    case "VertexBufferIndex":
                    case "VertexFormatIndex":
                        var chunkReference = property.GetValue(MeshGroups[groupIndex].MeshGroup) as GenericRCOLResource.ChunkReference;
                        if (chunkReference != null)
                        {
                            indices.Add(chunkReference.TGIBlockIndex + resource.PublicChunks);
                        }
                        break;
                }

            }
            indices.Sort((a, b) => b.CompareTo(a));
            var chunkReferenceMap = new Dictionary<string, Dictionary<GenericRCOLResource.ChunkReference, GenericRCOLResource.ChunkEntry>>();
            var mtstEntryMap = new Dictionary<MTST.Entry, GenericRCOLResource.ChunkEntry>();
            foreach (var meshGroup in MeshGroups)
            {
                chunkReferenceMap[meshGroup.ID] = new Dictionary<GenericRCOLResource.ChunkReference, GenericRCOLResource.ChunkEntry>
                {
                    {
                        meshGroup.MeshGroup.IndexBufferIndex,
                        resource.ChunkEntries[meshGroup.MeshGroup.IndexBufferIndex.TGIBlockIndex + resource.PublicChunks]
                    },
                    {
                        meshGroup.MeshGroup.SkinControllerIndex,
                        resource.ChunkEntries[meshGroup.MeshGroup.SkinControllerIndex.TGIBlockIndex + resource.PublicChunks]
                    },
                    {
                        meshGroup.MeshGroup.VertexBufferIndex,
                        resource.ChunkEntries[meshGroup.MeshGroup.VertexBufferIndex.TGIBlockIndex + resource.PublicChunks]
                    },
                    {
                        meshGroup.MeshGroup.VertexFormatIndex,
                        resource.ChunkEntries[meshGroup.MeshGroup.VertexFormatIndex.TGIBlockIndex + resource.PublicChunks]
                    }
                };
                var material = chunkReferenceMap[meshGroup.ID][meshGroup.MeshGroup.MaterialIndex] = resource.ChunkEntries[meshGroup.MeshGroup.MaterialIndex.TGIBlockIndex + resource.PublicChunks];
                if (meshGroup.DirectMATD == null)
                {
                    foreach (var entry in ((MTST)material.RCOLBlock).Entries)
                    {
                        mtstEntryMap[entry] = (GenericRCOLResource.ChunkEntry)resource.ChunkEntries[entry.Index.TGIBlockIndex + resource.PublicChunks];
                    }
                }
            }
            foreach (var index in indices)
            {
                resource.ChunkEntries.RemoveAt(index);
            }
            */
            MeshGroups.RemoveAt(groupIndex);
            MLODChunk.Meshes.RemoveAt(groupIndex);
            /*
            foreach (var meshGroup in MeshGroups)
            {
                meshGroup.MeshGroup.IndexBufferIndex.TGIBlockIndex = resource.ChunkEntries.IndexOf(chunkReferenceMap[meshGroup.ID][meshGroup.MeshGroup.IndexBufferIndex]) - resource.PublicChunks;
                meshGroup.MeshGroup.MaterialIndex.TGIBlockIndex = resource.ChunkEntries.IndexOf(chunkReferenceMap[meshGroup.ID][meshGroup.MeshGroup.MaterialIndex]) - resource.PublicChunks;
                meshGroup.MeshGroup.SkinControllerIndex.TGIBlockIndex = resource.ChunkEntries.IndexOf(chunkReferenceMap[meshGroup.ID][meshGroup.MeshGroup.SkinControllerIndex]) - resource.PublicChunks;
                meshGroup.MeshGroup.VertexBufferIndex.TGIBlockIndex = resource.ChunkEntries.IndexOf(chunkReferenceMap[meshGroup.ID][meshGroup.MeshGroup.VertexBufferIndex]) - resource.PublicChunks;
                meshGroup.MeshGroup.VertexFormatIndex.TGIBlockIndex = resource.ChunkEntries.IndexOf(chunkReferenceMap[meshGroup.ID][meshGroup.MeshGroup.VertexFormatIndex]) - resource.PublicChunks;
                if (meshGroup.DirectMATD == null)
                {
                    foreach (var entry in ((MTST)((GenericRCOLResource.ChunkEntry)resource.ChunkEntries[meshGroup.MeshGroup.MaterialIndex.TGIBlockIndex + resource.PublicChunks]).RCOLBlock).Entries)
                    {
                        entry.Index.TGIBlockIndex = resource.ChunkEntries.IndexOf(mtstEntryMap[entry]) - resource.PublicChunks;
                    }
                }
            }
            */
        }
    }
}
