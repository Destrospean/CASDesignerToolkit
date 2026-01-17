using System.Collections.Generic;

namespace Destrospean.Common
{
    public static class PreloadedData
    {
        public static readonly Dictionary<string, Destrospean.Common.Abstractions.CASPart> CASParts = new Dictionary<string, Destrospean.Common.Abstractions.CASPart>(System.StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, Destrospean.Common.Abstractions.GameObject> GameObjects = new Dictionary<string, Destrospean.Common.Abstractions.GameObject>(System.StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource> FTPTs = new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>(System.StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, CmarNYCBorrowed.GEOM> GEOMs = new Dictionary<string, CmarNYCBorrowed.GEOM>(System.StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource> LITEs = new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>(System.StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource> MLODs = new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>(System.StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource> MODLs = new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>(System.StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource> VPXYs = new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>(System.StringComparer.InvariantCultureIgnoreCase);
    }
}
