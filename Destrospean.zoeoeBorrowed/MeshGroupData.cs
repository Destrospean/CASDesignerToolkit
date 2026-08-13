using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;

namespace Destrospean.zoeoeBorrowed
{
    public class MeshGroupData
    {
        public MATD DirectMATD;

        public int CurrentGeoStateIndex = 0;

        public IBUF IndexBuffer;

        public object Lock = new object();

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
                var defaultScalar = 1f / short.MaxValue;
                return new[]
                {   
                    defaultScalar,
                    defaultScalar,
                    defaultScalar
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
            DirectMATD = directMATD;
            IndexBuffer = indexBuffer;
            MeshGroup = mesh;
            ParentResource = parentResource;
            SkinController = skinController;
            VertexBuffer = vertexBuffer;
            VertexFormat = vertexFormat;
        }

        public MeshGroupData(GenericRCOLResource parentResource, VRTF vertexFormat, VBUF vertexBuffer, IBUF indexBuffer, MTST materialSet, MLOD.Mesh mesh, SKIN skinController)
        {
            IndexBuffer = indexBuffer;
            MaterialSet = materialSet;
            MeshGroup = mesh;
            ParentResource = parentResource;
            SkinController = skinController;
            VertexBuffer = vertexBuffer;
            VertexFormat = vertexFormat;
        }

        public bool HasFlag(MeshFlags flag)
        {
            return (MeshGroup.Flags & flag) != 0;
        }
    }
}
