using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;
using Destrospean.CmarNYCBorrowed;
using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;
using s3pi.Interfaces;

namespace Destrospean.Common
{
    public static class PatternUtils
    {
        public static readonly string CacheFilePath = string.Format("{0}{1}Destrospean{1}PatternThumbnailCache", System.Destrospean.Platform.CacheDirectoryPath, Path.DirectorySeparatorChar);

        public static readonly Dictionary<string, Bitmap> PreloadedPatternImages = new Dictionary<string, Bitmap>();

        public static readonly Dictionary<string, PatternInfo> PreloadedPatterns = new Dictionary<string, PatternInfo>();

        public static void GenerateCache(s3pi.Interfaces.IPackage package)
        {
            var categories = new List<string>();
            EvaluatedResourceKey gamePatternListEvaluated;
            try
            {
                gamePatternListEvaluated = package.EvaluateResourceKey("key:D4D9FBE5:00000000:1BDE14D18B416FEC");
            }
            catch (ResourceIndexEntryNotFoundException)
            {
                return;
            }
            var patternsByCategory = new Dictionary<string, List<string>>();
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(new StreamReader(((APackage)gamePatternListEvaluated.Package).GetResource(gamePatternListEvaluated.ResourceIndexEntry)).ReadToEnd());
            foreach (XmlNode childNode in xmlDocument.SelectSingleNode("patternlist").ChildNodes)
            {
                if (childNode.Name == "category")
                {
                    var category = childNode.Attributes["name"].Value;
                    patternsByCategory.Add(category, new List<string>());
                    foreach (XmlElement grandchildNode in childNode.ChildNodes)
                    {
                        if (grandchildNode.Name == "pattern")
                        {
                            var key = "";
                            if (grandchildNode.HasAttribute("reskey"))
                            {
                                key = grandchildNode.Attributes["reskey"].Value;
                            }
                            else if (grandchildNode.HasAttribute("name"))
                            {
                                key = "key:0333406C:00000000:" + System.Security.Cryptography.FNV64.GetHash(grandchildNode.Attributes["name"].Value).ToString("X16");
                            }
                            else
                            {
                                throw new AttributeNotFoundException("The pattern XML node given does not have a \"reskey\" or \"name\" attribute.");
                            }
                            patternsByCategory[category].Add(key);
                            patternsByCategory[category].Add(grandchildNode.HasAttribute("name") ? "Materials\\" + category + "\\" + grandchildNode.Attributes["name"].Value : key);
                        }
                    }
                }
            }
            categories.AddRange(patternsByCategory.Keys);
            categories.Sort();
            if (categories.Contains("Old"))
            {
                categories.Remove("Old");
            }
            for (var i = 0; i < categories.Count; i++)
            {
                var patternKeysPaths = patternsByCategory[categories[i]];
                var patternNamesKeysPaths = new List<string[]>();
                var uncachedPatternExists = false;
                for (var j = 0; j < patternKeysPaths.Count; j += 2)
                {
                    patternNamesKeysPaths.Add(new string[]
                        {
                            patternKeysPaths[j + 1].Substring(patternKeysPaths[j + 1].LastIndexOf("\\") + 1),
                            patternKeysPaths[j],
                            patternKeysPaths[j + 1]
                        });
                }
                patternNamesKeysPaths.Sort((a, b) => a[0].CompareTo(b[0]));
                foreach (var patternNameKeyPath in patternNamesKeysPaths)
                {
                    Bitmap image = null;
                    if (!PreloadedPatternImages.TryGetValue(patternNameKeyPath[1], out image))
                    {
                        Bitmap patternImage = null;
                        PatternInfo patternInfo;
                        if (!PreloadedPatterns.TryGetValue(patternNameKeyPath[1], out patternInfo))
                        {
                            patternInfo = PreloadedPatterns[patternNameKeyPath[1]] = GetPatternInfo(package, patternNameKeyPath[1]);
                        }
                        switch (patternInfo.Type)
                        {
                            case PatternType.Colored:
                                patternImage = package.GetRGBPatternImage(patternInfo, Complate.GetTextureCallback);
                                break;
                            case PatternType.HSV:
                                patternImage = package.GetHSVPatternImage(patternInfo, Complate.GetTextureCallback);
                                break;
                        }
                        if (patternInfo.Type == PatternType.Solid)
                        {
                            patternImage = new Bitmap(64, 64);
                            using (var graphics = Graphics.FromImage(patternImage))
                            {
                                graphics.Clear(Color.FromArgb((byte)(patternInfo.SolidColor[0] * byte.MaxValue), (byte)(patternInfo.SolidColor[1] * byte.MaxValue), (byte)(patternInfo.SolidColor[2] * byte.MaxValue)));
                            }
                        }
                        if (patternImage != null)
                        {
                            image = PreloadedPatternImages[patternNameKeyPath[1]] = new Bitmap(patternImage, 64, 64);
                        }
                        uncachedPatternExists = true;
                    }
                }
                if (uncachedPatternExists)
                {
                    SaveCache();
                }
            }
        }

        public static PatternInfo GetPatternInfo(IPackage package, string patternKey)
        {
            var evaluated = package.EvaluateResourceKey(patternKey);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(new StreamReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)).ReadToEnd());
            var properties = new Dictionary<string, string>();
            foreach (XmlNode childNode in xmlDocument.SelectSingleNode("complate").ChildNodes)
            {
                if (childNode.Name == "variables")
                {
                    foreach (XmlNode grandchildNode in childNode.ChildNodes)
                    {
                        if (grandchildNode.Name == "param")
                        {
                            var defaultValue = grandchildNode.Attributes["default"].Value;
                            properties.Add(grandchildNode.Attributes["name"].Value, grandchildNode.Attributes["type"].Value == "texture" && defaultValue.StartsWith("($assetRoot)") ? "key:00B2D882:00000000:" + System.Security.Cryptography.FNV64.GetHash(defaultValue.Substring(defaultValue.LastIndexOf("\\") + 1, defaultValue.LastIndexOf(".") - defaultValue.LastIndexOf("\\") - 1)).ToString("X16") : defaultValue);
                        }
                    }
                }
            }
            string background = null,
            rgbMask = null;
            float baseHueBackground = float.MinValue,
            baseSaturationBackground = float.MinValue,
            baseValueBackground = float.MinValue,
            hueBackground = float.MinValue,
            saturationBackground = float.MinValue,
            valueBackground = float.MinValue;
            List<float> baseHues = new List<float>(),
            baseSaturations = new List<float>(),
            baseValues = new List<float>(),
            hues = new List<float>(),
            saturations = new List<float>(),
            values = new List<float>();
            var channels = new List<string>();
            var channelsEnabled = new List<bool>();
            List<float[]> baseHSVColors = new List<float[]>(),
            hsvColors = new List<float[]>(),
            hsvShift = new List<float[]>(),
            rgbColors = new List<float[]>();
            float[] hsvShiftBackground = null;
            foreach (var propertyKvp in properties)
            {
                string key = propertyKvp.Key.ToLower(),
                value = propertyKvp.Value;
                if (key.StartsWith("channel"))
                {
                    if (key.EndsWith("enabled"))
                    {
                        channelsEnabled.Add(bool.Parse(value));
                    }
                    else
                    {
                        channels.Add(value);
                    }
                }
                else if (key.StartsWith("color"))
                {
                    var color = Pattern.ParseCommaSeparatedValues(value);
                    rgbColors.Add(new float[]
                        {
                            color[0],
                            color[1],
                            color[2]
                        });
                }
                else if (key.StartsWith("base h"))
                {
                    if (key.EndsWith("bg"))
                    {
                        baseHueBackground = float.Parse(value, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        baseHues.Add(float.Parse(value, CultureInfo.InvariantCulture));
                    }
                }
                else if (key.StartsWith("base s"))
                {
                    if (key.EndsWith("bg"))
                    {
                        baseSaturationBackground = float.Parse(value, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        baseSaturations.Add(float.Parse(value, CultureInfo.InvariantCulture));
                    }
                }
                else if (key.StartsWith("base v"))
                {
                    if (key.EndsWith("bg"))
                    {
                        baseValueBackground = float.Parse(value, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        baseValues.Add(float.Parse(value, CultureInfo.InvariantCulture));
                    }
                }
                else if (key.StartsWith("h "))
                {
                    if (key.EndsWith("bg"))
                    {
                        hueBackground = float.Parse(value, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        hues.Add(float.Parse(value, CultureInfo.InvariantCulture));
                    }
                }
                else if (key.StartsWith("hsvshift"))
                {
                    if (key.EndsWith("bg"))
                    {
                        hsvShiftBackground = Pattern.ParseCommaSeparatedValues(value);
                    }
                    else
                    {
                        hsvShift.Add(Pattern.ParseCommaSeparatedValues(value));
                    }
                }
                else if (key.StartsWith("s "))
                {
                    if (key.EndsWith("bg"))
                    {
                        saturationBackground = float.Parse(value, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        saturations.Add(float.Parse(value, CultureInfo.InvariantCulture));
                    }
                }
                else if (key.StartsWith("v "))
                {
                    if (key.EndsWith("bg"))
                    {
                        valueBackground = float.Parse(value, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        values.Add(float.Parse(value, CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    switch (key)
                    {
                        case "background image":
                            background = value;
                            break;
                        case "rgbmask":
                            rgbMask = value;
                            break;
                    }
                }
            }
            for (var i = 0; i < baseHues.Count && baseHues.Count == baseSaturations.Count && baseHues.Count == baseValues.Count; i++)
            {
                baseHSVColors.Add(new float[]
                    {
                        baseHues[i],
                        baseSaturations[i],
                        baseValues[i]
                    });
            }
            for (var i = 0; i < hues.Count && hues.Count == saturations.Count && hues.Count == values.Count; i++)
            {
                hsvColors.Add(new float[]
                    {
                        hues[i],
                        saturations[i],
                        values[i]
                    });
            }
            return new PatternInfo
            {
                Background = background,
                Channels = channels.Count == 0 ? null : channels.ToArray(),
                ChannelsEnabled = channelsEnabled.Count == 0 ? null : channelsEnabled.ToArray(),
                HSV = hsvColors.Count == 0 ? null : hsvColors.ToArray(),
                HSVBase = baseHSVColors.Count == 0 ? null : baseHSVColors.ToArray(),
                HSVBaseBG = baseHueBackground == float.MinValue || baseSaturationBackground == float.MinValue || baseValueBackground == float.MinValue ? null : new float[]
                    {
                        baseHueBackground,
                        baseSaturationBackground,
                        baseValueBackground
                    },
                HSVBG = hueBackground == float.MinValue || saturationBackground == float.MinValue || valueBackground == float.MinValue ? null : new float[]
                    {
                        hueBackground,
                        saturationBackground,
                        valueBackground
                    },
                HSVShift = hsvShift.Count == 0 ? null : hsvShift.ToArray(),
                HSVShiftBG = hsvShiftBackground,
                RGBColors = rgbColors.ToArray(),
                RGBMask = rgbMask,
                SolidColor = rgbColors.Count == 1 ? rgbColors[0] : null
            };
        }

        public static void LoadCache()
        {
            if (File.Exists(CacheFilePath))
            {
                using (var reader = new Newtonsoft.Json.Bson.BsonReader(new FileStream(CacheFilePath, FileMode.Open)))
                {
                    foreach (var patternImageBase64StringKvp in new Newtonsoft.Json.JsonSerializer().Deserialize<Dictionary<string, string>>(reader))
                    {
                        using (var stream = new MemoryStream(System.Convert.FromBase64String(patternImageBase64StringKvp.Value)))
                        {
                            PreloadedPatternImages.Add(patternImageBase64StringKvp.Key, (Bitmap)Bitmap.FromStream(stream));
                        }
                    }
                }
            }
        }

        public static void SaveCache()
        {
            var patternThumbnailCache = new Dictionary<string, string>();
            foreach (var patternImageKvp in PreloadedPatternImages)
            {
                using (var stream = new MemoryStream())
                {
                    patternImageKvp.Value.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    patternThumbnailCache.Add(patternImageKvp.Key, System.Convert.ToBase64String(stream.ToArray()));
                }
            }
            Directory.CreateDirectory(Path.GetDirectoryName(CacheFilePath));
            using (var writer = new Newtonsoft.Json.Bson.BsonWriter(new FileStream(CacheFilePath, FileMode.Create)))
            {
                new Newtonsoft.Json.JsonSerializer().Serialize(writer, patternThumbnailCache);
            }
        }
    }
}
