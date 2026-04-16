using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;

namespace Destrospean.Common
{
    public class CASPartThumbnailCache : ThumbnailCache
    {
        public override string CacheFilePath
        {
            get
            {
                return string.Format("{0}{1}Destrospean{1}CASPartThumbnailCache", System.Destrospean.Platform.CacheDirectoryPath, Path.DirectorySeparatorChar);
            }
        }

        public static CASPartThumbnailCache Singleton = new CASPartThumbnailCache();

        public void GenerateCache(s3pi.Interfaces.IPackage package)
        {
            PreloadedThumbnails.Clear();
            List<string> keys = new List<string>(),
            names = new List<string>();
            var casPartsNamesKeysThumbnailKeys = new List<string[]>();
            var uncachedCASPartExists = false;
            foreach (var casPartLookupKvp in CASPart.CASPartLookupCache)
            {
                casPartsNamesKeysThumbnailKeys.Add(new[]
                    {
                        casPartLookupKvp.Value["Unknown1"],
                        casPartLookupKvp.Key,
                        "key:626F60CE" + casPartLookupKvp.Key.Substring(12)
                    });
            }
            casPartsNamesKeysThumbnailKeys.Sort((a, b) => a[0].CompareTo(b[0]));
            foreach (var casPartNameKeyThumbnailKey in casPartsNamesKeysThumbnailKeys)
            {
                Bitmap casPartImage = null;
                if (!PreloadedThumbnails.TryGetValue(casPartNameKeyThumbnailKey[1], out casPartImage))
                {
                    try
                    {
                        var evaluated = package.EvaluateThumbnailResourceKey(casPartNameKeyThumbnailKey[2]);
                        casPartImage = new Bitmap(((s3pi.Interfaces.APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry));
                    }
                    catch (ResourceIndexEntryNotFoundException)
                    {
                        casPartImage = new Bitmap(64, 64);
                        using (var graphics = Graphics.FromImage(casPartImage))
                        {
                            graphics.Clear(Color.Transparent);
                        }
                    }
                    casPartImage = PreloadedThumbnails[casPartNameKeyThumbnailKey[1]] = new Bitmap(casPartImage, 64, 64);
                    uncachedCASPartExists = true;
                }
                keys.Add(casPartNameKeyThumbnailKey[1]);
                names.Add(casPartNameKeyThumbnailKey[0]);
            }
            if (uncachedCASPartExists)
            {
                SaveCache();
            }
        }
    }
}
