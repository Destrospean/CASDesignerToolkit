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

        public GenericRCOLResource ParentResource;

        public int PrimitiveCount
        {
            get
            {
                return MeshGroup.PrimitiveCount;
            }
        }

        public SKIN SkinController;

        public float[] UVScales
        {
            get
            {
                foreach (var element in (DirectMATD ?? (MATD)ParentResource.ChunkEntries[MaterialSet.Entries.Find(x => x.MaterialState == MTST.State.Default).Index.TGIBlockIndex + ParentResource.PublicChunks].RCOLBlock).Mtnf.SData)
                {
                    if (element.Field == FieldType.UVScales)
                    {
                        var elementFloat3 = ((ElementFloat3)element);
                        return new[]
                        {
                            elementFloat3.Data0,
                            elementFloat3.Data1,
                            elementFloat3.Data2
                        };
                    }
                }
                return new[]
                {   
                    3.051851E-05f,
                    3.051851E-05f,
                    3.051851E-05f
                };
            }
        }

        public VBUF VertexBuffer;

        public int VertexCount
        {
            get
            {
                return MeshGroup.VertexCount;
            }
        }

        public VRTF VertexFormat;

        public MeshGroupData(GenericRCOLResource parentResource, VRTF vertexFormat, VBUF vertexBuffer, IBUF indexBuffer, MATD directMATD, MLOD.Mesh mesh, SKIN skinController)
        {
            ID = System.Guid.NewGuid().ToString();
            DirectMATD = directMATD;
            IndexBuffer = indexBuffer;
            MaterialSet = null;
            MeshGroup = mesh;
            ParentResource = parentResource;
            SkinController = skinController;
            VertexBuffer = vertexBuffer;
            VertexFormat = vertexFormat;
        }

        public MeshGroupData(GenericRCOLResource parentResource, VRTF vertexFormat, VBUF vertexBuffer, IBUF indexBuffer, MTST materialSet, MLOD.Mesh mesh, SKIN skinController)
        {
            ID = System.Guid.NewGuid().ToString();
            DirectMATD = null;
            IndexBuffer = indexBuffer;
            MaterialSet = materialSet;
            MeshGroup = mesh;
            ParentResource = parentResource;
            SkinController = skinController;
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
