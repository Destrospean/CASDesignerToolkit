using System;
using System.Collections.Generic;
using s3pi.GenericRCOLResource;

namespace Destrospean.Common
{
    public static class PreloadedData
    {
        public static readonly Dictionary<string, Abstractions.CASPart> CASParts = new Dictionary<string, Abstractions.CASPart>(StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, Abstractions.GameObject> GameObjects = new Dictionary<string, Abstractions.GameObject>(StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, GenericRCOLResource> FTPTs = new Dictionary<string, GenericRCOLResource>(StringComparer.InvariantCultureIgnoreCase),
        LITEs = new Dictionary<string, GenericRCOLResource>(StringComparer.InvariantCultureIgnoreCase),
        MLODs = new Dictionary<string, GenericRCOLResource>(StringComparer.InvariantCultureIgnoreCase),
        MODLs = new Dictionary<string, GenericRCOLResource>(StringComparer.InvariantCultureIgnoreCase),
        VPXYs = new Dictionary<string, GenericRCOLResource>(StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, CmarNYCBorrowed.GEOM> GEOMs = new Dictionary<string, CmarNYCBorrowed.GEOM>(StringComparer.InvariantCultureIgnoreCase);
    }
}
