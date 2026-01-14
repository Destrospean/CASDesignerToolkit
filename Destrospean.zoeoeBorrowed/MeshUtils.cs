using System;
using System.Collections.Generic;
using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace Destrospean.zoeoeBorrowed
{
    public class MeshUtils
    {
        public struct LODData
        {
            public LODId ID;

            public List<MeshGroupData> MeshGroups;

            public IResource MLODResource;

            public string MLODResourceKey;

            public LODData(LODId id, List<MeshGroupData> meshGroups, IResource mlodResource, string mlodResourceKey)
            {
                ID = id;
                MeshGroups = meshGroups;
                MLODResource = mlodResource;
                MLODResourceKey = mlodResourceKey;
            }

            public override string ToString()
            {
                return ID.ToString();
            }
        }

        public struct MeshGroupData
        {
            public MATD DirectMATD;

            public IBUF IndexBuffer;

            public MTST MaterialSet;

            public MLOD.Mesh Mesh;

            public int PrimitiveCount
            {
                get
                {
                    return Mesh.PrimitiveCount;
                }
            }

            public SKIN SkinController;

            public float[] UVScales;

            public VBUF VertexBuffer;

            public int VertexCount
            {
                get
                {
                    return Mesh.VertexCount;
                }
            }

            public VRTF VertexFormat;

            public MeshGroupData(VRTF vertexFormat, VBUF vertexBuffer, IBUF indexBuffer, MATD directMATD, MLOD.Mesh mesh, SKIN skinController, float[] uvScales)
            {
                DirectMATD = directMATD;
                IndexBuffer = indexBuffer;
                MaterialSet = null;
                Mesh = mesh;
                SkinController = skinController;
                UVScales = uvScales;
                VertexBuffer = vertexBuffer;
                VertexFormat = vertexFormat;
            }

            public MeshGroupData(VRTF vertexFormat, VBUF vertexBuffer, IBUF indexBuffer, MTST materialSet, MLOD.Mesh mesh, SKIN skinController, float[] uvScales)
            {
                DirectMATD = null;
                IndexBuffer = indexBuffer;
                MaterialSet = materialSet;
                Mesh = mesh;
                SkinController = skinController;
                UVScales = uvScales;
                VertexBuffer = vertexBuffer;
                VertexFormat = vertexFormat;
            }
        }

        public static LODData LoadMLODData(string outerResourceKey, GenericRCOLResource outerResource, int publicChunkCount, MLOD mlodChunk)
        {
            var meshGroups = new List<MeshGroupData>();
            foreach (var meshGroup in mlodChunk.Meshes)
            {
                var indexBuffer = outerResource.ChunkEntries[meshGroup.IndexBufferIndex.TGIBlockIndex + publicChunkCount].RCOLBlock as IBUF;
                var skinController = outerResource.ChunkEntries[meshGroup.SkinControllerIndex.TGIBlockIndex + publicChunkCount].RCOLBlock as SKIN;
                var vertexBuffer = outerResource.ChunkEntries[meshGroup.VertexBufferIndex.TGIBlockIndex + publicChunkCount].RCOLBlock as VBUF;
                var vertexFormat = outerResource.ChunkEntries[meshGroup.VertexFormatIndex.TGIBlockIndex + publicChunkCount].RCOLBlock as VRTF;
                var materialIndexBlock = outerResource.ChunkEntries[meshGroup.MaterialIndex.TGIBlockIndex + publicChunkCount].RCOLBlock;
                var mtst = materialIndexBlock as MTST;
                var matd = mtst == null ? materialIndexBlock as MATD : outerResource.ChunkEntries[mtst.Entries[0].Index.TGIBlockIndex + publicChunkCount].RCOLBlock as MATD;
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
                meshGroups.Add(mtst == null ? new MeshGroupData(vertexFormat, vertexBuffer, indexBuffer, matd, meshGroup, skinController, uvScales) : new MeshGroupData(vertexFormat, vertexBuffer, indexBuffer, mtst, meshGroup, skinController, uvScales));
            }
            return new LODData(LODId.HighDetail, meshGroups, outerResource, outerResourceKey);
        }
    }
}
