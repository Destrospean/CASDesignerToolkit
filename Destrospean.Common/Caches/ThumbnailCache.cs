using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Destrospean.Common
{
    public abstract class ThumbnailCache
    {
        public abstract string CacheFilePath
        {
            get;
        }

        public const uint CacheVersion = 0;

        public Dictionary<string, Bitmap> PreloadedThumbnails = new Dictionary<string, Bitmap>();

        public virtual bool LoadCache()
        {
            if (File.Exists(CacheFilePath))
            {
                using (var reader = new Newtonsoft.Json.Bson.BsonReader(new FileStream(CacheFilePath, FileMode.Open)))
                {
                    var cache = new Newtonsoft.Json.JsonSerializer().Deserialize<Newtonsoft.Json.Linq.JObject>(reader);
                    Newtonsoft.Json.Linq.JToken version;
                    if (!cache.TryGetValue("Version", out version) || (uint)version != CacheVersion)
                    {
                        File.Delete(CacheFilePath);
                        return false;
                    }
                    foreach (var thumbnailKvp in (Newtonsoft.Json.Linq.JObject)cache["Data"])
                    {
                        using (var stream = new MemoryStream((byte[])thumbnailKvp.Value))
                        {
                            PreloadedThumbnails.Add(thumbnailKvp.Key, new Bitmap(stream));
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public virtual void SaveCache()
        {
            var cache = new Dictionary<string, object>();
            var data = new Dictionary<string, byte[]>();
            foreach (var thumbnailKvp in PreloadedThumbnails)
            {
                using (var stream = new MemoryStream())
                {
                    thumbnailKvp.Value.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    data.Add(thumbnailKvp.Key, stream.ToArray());
                }
            }
            cache.Add("Version", CacheVersion);
            cache.Add("Data", data);
            Directory.CreateDirectory(Path.GetDirectoryName(CacheFilePath));
            using (var writer = new Newtonsoft.Json.Bson.BsonWriter(new FileStream(CacheFilePath, FileMode.Create)))
            {
                new Newtonsoft.Json.JsonSerializer().Serialize(writer, cache);
            }
        }
    }
}

