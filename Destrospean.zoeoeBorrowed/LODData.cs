using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;

namespace Destrospean.zoeoeBorrowed
{
    public struct LODData
    {
        public System.Collections.Generic.List<MeshGroupData> MeshGroups;

        public MLOD MLODChunk;

        public s3pi.Interfaces.IResource Resource;

        public string ResourceKey;

        public LODData(LODId id, string key, GenericRCOLResource resource, MLOD mlodChunk)
        {
            Resource = resource;
            ResourceKey = key;
            MeshGroups = new System.Collections.Generic.List<MeshGroupData>();
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

        public void CloneMeshGroup(int index)
        {
            var resource = (GenericRCOLResource)Resource;
            var meshGroup = MeshGroups[index];
            var mesh = (MLOD.Mesh)meshGroup.MeshGroup.Clone((sender, e) =>
                {
                });
            GenericRCOLResource.ChunkEntry indexBuffer = (GenericRCOLResource.ChunkEntry)resource.ChunkEntries[mesh.IndexBufferIndex.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
                {
                }),
            materialIndexChunkEntry = (GenericRCOLResource.ChunkEntry)resource.ChunkEntries[mesh.MaterialIndex.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
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
            mesh.SkinControllerIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
            resource.ChunkEntries.Add(skinController);
            mesh.VertexBufferIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
            resource.ChunkEntries.Add(vertexBuffer);
            mesh.VertexFormatIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
            resource.ChunkEntries.Add(vertexFormat);
            if (meshGroup.DirectMATD == null)
            {
                mesh.MaterialIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
                resource.ChunkEntries.Add(materialIndexChunkEntry);
                foreach (var entry in ((MTST)materialIndexChunkEntry.RCOLBlock).Entries)
                {
                    mesh.MaterialIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
                    resource.ChunkEntries.Add((GenericRCOLResource.ChunkEntry)resource.ChunkEntries[entry.Index.TGIBlockIndex + resource.PublicChunks].Clone((sender, e) =>
                        {
                        }));
                }
            }
            else
            {
                mesh.MaterialIndex.TGIBlockIndex = resource.ChunkEntries.Count - resource.PublicChunks;
                resource.ChunkEntries.Add(materialIndexChunkEntry);
            }
            var uvScales = (float[])meshGroup.UVScales.Clone();
            MLODChunk.Meshes.Add(mesh);
            MeshGroups.Add(meshGroup.DirectMATD == null ? new MeshGroupData((VRTF)vertexFormat.RCOLBlock, (VBUF)vertexBuffer.RCOLBlock, (IBUF)indexBuffer.RCOLBlock, meshGroup.MaterialSet, mesh, (SKIN)skinController.RCOLBlock, uvScales) : new MeshGroupData((VRTF)vertexFormat.RCOLBlock, (VBUF)vertexBuffer.RCOLBlock, (IBUF)indexBuffer.RCOLBlock, (MATD)materialIndexChunkEntry.RCOLBlock, mesh, (SKIN)skinController.RCOLBlock, uvScales));
        }

        public void DeleteMeshGroup(int index)
        {
            var resource = (GenericRCOLResource)Resource;
            var indices = new System.Collections.Generic.List<int>();
            foreach (var property in typeof(MLOD.Mesh).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                switch (property.Name)
                {
                    case "IndexBufferIndex":
                    case "MaterialIndex":
                    case "SkinControllerIndex":
                    case "VertexBufferIndex":
                    case "VertexFormatIndex":
                        break;
                    default:
                        continue;
                }
                var chunkReference = property.GetValue(MeshGroups[index].MeshGroup) as GenericRCOLResource.ChunkReference;
                if (chunkReference != null)
                {
                    indices.Add(chunkReference.TGIBlockIndex + resource.PublicChunks);
                }
            }
            indices.Sort((a, b) => b.CompareTo(a));
            foreach (var i in indices)
            {
                resource.ChunkEntries.RemoveAt(i);
            }
            MLODChunk.Meshes.RemoveAt(index);
            MeshGroups.RemoveAt(index);
        }
    }
}
