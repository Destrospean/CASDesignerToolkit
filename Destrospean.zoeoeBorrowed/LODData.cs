using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;

namespace Destrospean.zoeoeBorrowed
{
    public struct LODData
    {
        public System.Collections.Generic.List<MeshGroupData> MeshGroups;

        public s3pi.Interfaces.IResource Resource;

        public string ResourceKey;

        public LODData(LODId id, string key, GenericRCOLResource resource, MLOD mlodChunk)
        {
            Resource = resource;
            ResourceKey = key;
            MeshGroups = new System.Collections.Generic.List<MeshGroupData>();
            foreach (var meshGroup in mlodChunk.Meshes)
            {
                var materialIndexBlock = resource.ChunkEntries[meshGroup.MaterialIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock;
                IBUF indexBuffer = null;
                try
                {
                    indexBuffer = new IBUF(0, (sender, e) =>
                        {
                        }, resource.ChunkEntries[meshGroup.IndexBufferIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock.Stream);
                }
                catch
                {
                }
                SKIN skinController = null;
                try
                {
                    skinController = new SKIN(0, (sender, e) =>
                        {
                        }, resource.ChunkEntries[meshGroup.SkinControllerIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock.Stream);
                }
                catch
                {
                }
                VBUF vertexBuffer = null;
                try
                {
                    vertexBuffer = new VBUF(0, (sender, e) =>
                        {
                        }, resource.ChunkEntries[meshGroup.VertexBufferIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock.Stream);
                }
                catch
                {
                }
                VRTF vertexFormat = null;
                try
                {
                    vertexFormat = new VRTF(0, (sender, e) =>
                        {
                        }, resource.ChunkEntries[meshGroup.VertexFormatIndex.TGIBlockIndex + resource.PublicChunks].RCOLBlock.Stream);
                }
                catch
                {
                }
                MTST mtst = null;
                try
                {
                    mtst = new MTST(0, (sender, e) =>
                        {
                        }, materialIndexBlock.Stream);
                }
                catch
                {
                }
                MATD matd = null;
                try
                {
                    matd = new MATD(0, (sender, e) =>
                        {
                        }, mtst == null ? materialIndexBlock.Stream : resource.ChunkEntries[mtst.Entries.Find(x => x.MaterialState == MTST.State.Default).Index.TGIBlockIndex + resource.PublicChunks].RCOLBlock.Stream);
                }
                catch
                {
                }
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
    }
}
