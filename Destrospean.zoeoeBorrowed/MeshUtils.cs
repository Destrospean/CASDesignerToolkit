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

            public LODData(LODId id, List<MeshGroupData> meshGroups, IResource mlodResource)
            {
                ID = id;
                MeshGroups = meshGroups;
                MLODResource = mlodResource;
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

            public int PrimitiveCount;

            //public SKIN SkinController; //not implemented

            public float UVScales;

            public VBUF VertexBuffer;

            public VRTF VertexFormat;

            public MeshGroupData(VRTF vertexFormat, VBUF vertexBuffer, IBUF indexBuffer, MATD directMATD, int primitiveCount, float uvScales)
            {
                DirectMATD = directMATD;
                IndexBuffer = indexBuffer;
                MaterialSet = null;
                PrimitiveCount = primitiveCount;
                UVScales = uvScales;
                VertexBuffer = vertexBuffer;
                VertexFormat = vertexFormat;
            }

            public MeshGroupData(VRTF vertexFormat, VBUF vertexBuffer, IBUF indexBuffer, MTST materialSet, int primitiveCount, float uvScales)
            {
                DirectMATD = null;
                IndexBuffer = indexBuffer;
                MaterialSet = materialSet;
                PrimitiveCount = primitiveCount;
                UVScales = uvScales;
                VertexBuffer = vertexBuffer;
                VertexFormat = vertexFormat;
            }
        }

        static float[][] GetBlendWeightsColorsNormalsOrTangents(byte[] vertexBuffer, int stride, int offset)
        {
            var values = new List<float[]>();
            for (var i = offset; i < vertexBuffer.Length; i += stride)
            {
                float scalar = byte.MaxValue - vertexBuffer[i + 3];
                if (scalar == 0)
                {
                    scalar = offset == 20 ? 256 : 128;
                }
                var subtrahend = offset == 20 ? 0 : 128;
                values.Add(new float[]
                    {
                        (vertexBuffer[i + 2] - subtrahend) / scalar,
                        (vertexBuffer[i + 1] - subtrahend) / scalar,
                        (vertexBuffer[i] - subtrahend) / scalar
                    });
            }
            return values.ToArray();
        }

        public static float[][] GetBlendWeights(byte[] vertexBuffer, int stride)
        {
            return GetBlendWeightsColorsNormalsOrTangents(vertexBuffer, stride, 20); 
        }

        public static int[][] GetFaces(int[] indexBuffer, int stride)
        {
            var faces = new List<int[]>();
            for (var i = 0; i < indexBuffer.Length; i += 3)
            {
                faces.Add(new int[]
                    {
                        indexBuffer[i] + 1,
                        indexBuffer[i + 1] + 1,
                        indexBuffer[i + 2] + 1
                    });
            }
            return faces.ToArray();
        }

        public static float[][] GetNormals(byte[] vertexBuffer, int stride)
        {
            return GetBlendWeightsColorsNormalsOrTangents(vertexBuffer, stride, 8); 
        }

        public static float[][] GetTangents(byte[] vertexBuffer, int stride)
        {
            return GetBlendWeightsColorsNormalsOrTangents(vertexBuffer, stride, 24); 
        }

        public static float[][] GetTextureCoordinates(byte[] vertexBuffer, int stride, float uvScales)
        {
            var textureCoordinates = new List<float[]>();
            for (var i = 12; i < vertexBuffer.Length; i += stride)
            {
                textureCoordinates.Add(uvScales == -1 ? new float[]
                    {
                        (float)BitConverter.ToInt16(vertexBuffer, i) / short.MaxValue,
                        1 - (float)BitConverter.ToInt16(vertexBuffer, i + 2) / short.MaxValue
                    } : new float[]
                    {
                        (float)BitConverter.ToInt16(vertexBuffer, i) * uvScales,
                        1 - (float)BitConverter.ToInt16(vertexBuffer, i + 2) * uvScales
                    });
            }
            return textureCoordinates.ToArray();
        }

        public static float[][] GetVertices(byte[] vertexBuffer, int stride)
        {
            var vertices = new List<float[]>();
            for (var i = 0; i < vertexBuffer.Length; i += stride)
            {
                float scalar = BitConverter.ToUInt16(vertexBuffer, i + 6);
                vertices.Add(new float[]
                    {
                        BitConverter.ToInt16(vertexBuffer, i) / scalar,
                        BitConverter.ToInt16(vertexBuffer, i + 2) / scalar,
                        BitConverter.ToInt16(vertexBuffer, i + 4) / scalar
                    });
            }
            return vertices.ToArray();
        }

        public static LODData LoadMLODData(GenericRCOLResource outerResource, int publicChunkCount, MLOD mlodChunk)
        {
            var meshGroups = new List<MeshGroupData>();
            foreach (var meshGroup in mlodChunk.Meshes)
            {
                var indexBuffer = outerResource.ChunkEntries[meshGroup.IndexBufferIndex.TGIBlockIndex + publicChunkCount].RCOLBlock as IBUF;
                var vertexBuffer = outerResource.ChunkEntries[meshGroup.VertexBufferIndex.TGIBlockIndex + publicChunkCount].RCOLBlock as VBUF;
                var vertexFormat = outerResource.ChunkEntries[meshGroup.VertexFormatIndex.TGIBlockIndex + publicChunkCount].RCOLBlock as VRTF;
                var materialIndex = meshGroup.MaterialIndex.TGIBlockIndex;
                var materialIndexBlock = outerResource.ChunkEntries[materialIndex + publicChunkCount].RCOLBlock;
                var mtst = outerResource.ChunkEntries[meshGroup.MaterialIndex.TGIBlockIndex + publicChunkCount].RCOLBlock as MTST;
                MATD matd = null;
                if (mtst != null) 
                {
                    matd = outerResource.ChunkEntries[mtst.Entries[0].Index.TGIBlockIndex + publicChunkCount].RCOLBlock as MATD;
                }
                //uv scales - eventually not necessary
                var uvScales = -1f;
                if (matd != null)
                {
                    foreach (var shaderData in matd.Mtnf.SData)
                    {
                        if (shaderData.Field == FieldType.UVScales)
                        {
                            uvScales = ((ElementFloat3)shaderData).Data0;
                        }
                    }
                }
                meshGroups.Add(mtst == null ? new MeshGroupData(vertexFormat, vertexBuffer, indexBuffer, materialIndexBlock as MATD, meshGroup.PrimitiveCount, uvScales) : new MeshGroupData(vertexFormat, vertexBuffer, indexBuffer, materialIndexBlock as MTST, meshGroup.PrimitiveCount, uvScales));
            }
            return new LODData(LODId.HighDetail, meshGroups, outerResource);
        }
    }
}
