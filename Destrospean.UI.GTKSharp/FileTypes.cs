namespace Destrospean.DestrospeanCASPEditor
{
    public static class FileTypes
    {
        public const string DBPFPackage = "The Sims 3 DBPF Package",
        GEOM = "The Sims 3 Body Geometry Resource",
        MLOD = "The Sims 3 Object Geometry Resource",
        OBJ = "Wavefront OBJ",
        WSO = "The Sims Resource Workshop Object";
    
        public static string GetName(Common.MeshFileType meshFileType)
        {
            switch (meshFileType)
            {
                case Common.MeshFileType.GEOM:
                    return GEOM;
                case Common.MeshFileType.MLOD:
                    return MLOD;
                case Common.MeshFileType.OBJ:
                    return OBJ;
                case Common.MeshFileType.WSO:
                    return WSO;
                default:
                    return null;
            }
        }
    }
}
