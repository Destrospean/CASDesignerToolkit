using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;

namespace Destrospean.zoeoeBorrowed
{
    public struct MeshGroupData
    {
        public MATD DirectMATD;

        public IBUF IndexBuffer;

        public MTST MaterialSet;

        public MLOD.Mesh MeshGroup;

        public int PrimitiveCount
        {
            get
            {
                return MeshGroup.PrimitiveCount;
            }
        }

        public SKIN SkinController;

        public float[] UVScales;

        public VBUF VertexBuffer;

        public int VertexCount
        {
            get
            {
                return MeshGroup.VertexCount;
            }
        }

        public VRTF VertexFormat;

        public MeshGroupData(VRTF vertexFormat, VBUF vertexBuffer, IBUF indexBuffer, MATD directMATD, MLOD.Mesh mesh, SKIN skinController, float[] uvScales)
        {
            DirectMATD = directMATD;
            IndexBuffer = indexBuffer;
            MaterialSet = null;
            MeshGroup = mesh;
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
            MeshGroup = mesh;
            SkinController = skinController;
            UVScales = uvScales;
            VertexBuffer = vertexBuffer;
            VertexFormat = vertexFormat;
        }

        public bool HasFlag(MeshFlags flag)
        {
            return (MeshGroup.Flags & flag) != 0;
        }
    }
}
