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
                            uvScales = new float[]
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
