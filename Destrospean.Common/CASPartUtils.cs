using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;

namespace Destrospean.Common
{
    public static class CASPartUtils
    {
        public static readonly string CacheFilePath = string.Format("{0}{1}Destrospean{1}CASPartThumbnailCache", System.Destrospean.Platform.CacheDirectoryPath, Path.DirectorySeparatorChar);

        public static readonly Dictionary<string, Bitmap> PreloadedCASPartImages = new Dictionary<string, Bitmap>();

        public static void GenerateCache(s3pi.Interfaces.IPackage package)
        {
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
                if (!PreloadedCASPartImages.TryGetValue(casPartNameKeyThumbnailKey[1], out casPartImage))
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
                    casPartImage = PreloadedCASPartImages[casPartNameKeyThumbnailKey[1]] = new Bitmap(casPartImage, 64, 64);
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

        public static void LoadCache()
        {
            if (File.Exists(CacheFilePath))
            {
                using (var reader = new Newtonsoft.Json.Bson.BsonReader(new FileStream(CacheFilePath, FileMode.Open)))
                {
                    foreach (var casPartImageBase64StringKvp in new Newtonsoft.Json.JsonSerializer().Deserialize<Dictionary<string, string>>(reader))
                    {
                        using (var stream = new MemoryStream(System.Convert.FromBase64String(casPartImageBase64StringKvp.Value)))
                        {
                            PreloadedCASPartImages.Add(casPartImageBase64StringKvp.Key, new Bitmap(stream));
                        }
                    }
                }
            }
        }

        public static void SaveCache()
        {
            var casPartThumbnailCache = new Dictionary<string, string>();
            foreach (var casPartImageKvp in PreloadedCASPartImages)
            {
                using (var stream = new MemoryStream())
                {
                    casPartImageKvp.Value.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    casPartThumbnailCache.Add(casPartImageKvp.Key, System.Convert.ToBase64String(stream.ToArray()));
                }
            }
            Directory.CreateDirectory(Path.GetDirectoryName(CacheFilePath));
            using (var writer = new Newtonsoft.Json.Bson.BsonWriter(new FileStream(CacheFilePath, FileMode.Create)))
            {
                new Newtonsoft.Json.JsonSerializer().Serialize(writer, casPartThumbnailCache);
            }
        }
    }
}
