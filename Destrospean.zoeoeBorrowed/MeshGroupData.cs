using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;

namespace Destrospean.zoeoeBorrowed
{
    public struct MeshGroupData
    {
        public MATD DirectMATD;

        public string ID;

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
            ID = System.Guid.NewGuid().ToString();
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
            ID = System.Guid.NewGuid().ToString();
            DirectMATD = null;
            IndexBuffer = indexBuffer;
            MaterialSet = materialSet;
            MeshGroup = mesh;
            SkinController = skinController;
            UVScales = uvScales;
            VertexBuffer = vertexBuffer;
            VertexFormat = vertexFormat;
        }

        public static bool operator ==(MeshGroupData a, MeshGroupData b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(MeshGroupData a, MeshGroupData b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(object other)
        {
            return Equals((MeshGroupData)other);
        }

        public bool Equals(MeshGroupData other)
        {
            return ID == other.ID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool HasFlag(MeshFlags flag)
        {
            return (MeshGroup.Flags & flag) != 0;
        }
    }
}
