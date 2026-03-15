using System.Collections.Generic;

namespace Destrospean.Common
{
    public static class PreloadedData
    {
        public static readonly Dictionary<string, Abstractions.CASPart> CASParts = new Dictionary<string, Abstractions.CASPart>(System.StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, Abstractions.GameObject> GameObjects = new Dictionary<string, Abstractions.GameObject>(System.StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource> FTPTs = new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>(System.StringComparer.InvariantCultureIgnoreCase),
        LITEs = new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>(System.StringComparer.InvariantCultureIgnoreCase),
        MLODs = new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>(System.StringComparer.InvariantCultureIgnoreCase),
        MODLs = new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>(System.StringComparer.InvariantCultureIgnoreCase),
        VPXYs = new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>(System.StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, CmarNYCBorrowed.GEOM> GEOMs = new Dictionary<string, CmarNYCBorrowed.GEOM>(System.StringComparer.InvariantCultureIgnoreCase);
    }
}
