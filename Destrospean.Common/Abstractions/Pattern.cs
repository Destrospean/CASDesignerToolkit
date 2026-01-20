using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Destrospean.CmarNYCBorrowed;
using Destrospean.S3PIExtensions;

namespace Destrospean.Common.Abstractions
{
    public class Pattern : Complate
    {
        protected object mPatternImage;

        protected readonly IDictionary<string, object> mProperties = new SortedDictionary<string, object>(new PropertyNameComparer());

        public override CASTableObject CASTableObject
        {
            get
            {
                return Preset.CASTableObject;
            }
        }

        public override s3pi.Interfaces.IPackage ParentPackage
        {
            get
            {
                return Preset.ParentPackage;
            }
        }

        public object PatternImage
        {
            get
            {
                if (mPatternImage == null)
                {
                    switch (PatternInfo.Type)
                    {
                        case PatternType.Colored:
                            mPatternImage = ParentPackage.GetRGBPatternImage(PatternInfo, GetTextureCallback);
                            break;
                        case PatternType.HSV:
                            mPatternImage = ParentPackage.GetHSVPatternImage(PatternInfo, GetTextureCallback);
                            break;
                        case PatternType.Solid:
                            mPatternImage = PatternInfo.SolidColor;
                            break;
                    }
                }
                return mPatternImage;
            }
            private set
            {
                mPatternImage = value;
            }
        }

        public PatternInfo PatternInfo
        {
            get;
            private set;
        }

        public readonly Preset Preset;

        public override string[] PropertyNames
        {
            get
            {
                if (Preset is GameObjectPreset)
                {
                    return new List<string>(mProperties.Keys).ToArray();
                }
                return base.PropertyNames;
            }
        }

        public readonly string SlotName;

        public Pattern(GameObjectPreset preset, object patternMaterialBlock, object presetMaterialBlock) : base()
        {
            var patternMaterialBlockCast = (CatalogResource.CatalogResource.MaterialBlock)patternMaterialBlock;
            SlotName = patternMaterialBlockCast.Pattern;
            PatternInfo = new PatternInfo
                {
                    Name = patternMaterialBlockCast.Name
                };
            Preset = preset;
            var evaluated = ParentPackage.EvaluateResourceKey(patternMaterialBlockCast.ParentTGIBlocks[patternMaterialBlockCast.ComplateXMLIndex].ReverseEvaluateResourceKey());
            mXmlDocument.LoadXml(new System.IO.StreamReader(s3pi.WrapperDealer.WrapperDealer.GetResource(0, evaluated.Package, evaluated.ResourceIndexEntry).Stream).ReadToEnd());
            foreach (var complateOverride in patternMaterialBlockCast.ComplateOverrides)
            {
                mProperties.Add(complateOverride.VariableName, complateOverride);
            }
            foreach (XmlNode childNode in mXmlDocument.SelectSingleNode("complate").ChildNodes)
            {
                if (childNode.Name == "variables")
                {
                    foreach (XmlNode grandchildNode in childNode.ChildNodes)
                    {
                        if (grandchildNode.Name == "param")
                        {
                            var key = grandchildNode.Attributes["name"].Value;
                            PropertiesTyped.Add(key, new PropertyMeta(grandchildNode.Attributes["type"].Value, grandchildNode.Attributes["default"].Value));
                            if (!mProperties.ContainsKey(key))
                            {
                                mProperties.Add(key, GameObjectPreset.CreateComplateOverrideInstance(key, PropertiesTyped[key].DefaultValue, PropertiesTyped[key].Type, (CatalogResource.CatalogResource.MaterialBlock)patternMaterialBlock, ParentPackage));
                            }
                        }
                    }
                }
            }
            RefreshPatternInfo(false, presetMaterialBlock, ParentPackage);
        }

        public Pattern(CASPartPreset preset, XmlNode patternXmlNode) : base()
        {
            SlotName = patternXmlNode.Attributes["variable"].Value;
            PatternInfo = new PatternInfo
                {
                    Name = patternXmlNode.Attributes["name"].Value
                };
            Preset = preset;
            var evaluated = ParentPackage.EvaluateResourceKey(patternXmlNode);
            mXmlDocument.LoadXml(new System.IO.StreamReader(s3pi.WrapperDealer.WrapperDealer.GetResource(0, evaluated.Package, evaluated.ResourceIndexEntry).Stream).ReadToEnd());
            foreach (XmlNode childNode in patternXmlNode.ChildNodes)
            {
                if (childNode.Name == "value")
                {
                    mPropertiesXmlNodes.Add(childNode.Attributes["key"].Value, childNode);
                }
            }
            foreach (XmlNode childNode in mXmlDocument.SelectSingleNode("complate").ChildNodes)
            {
                if (childNode.Name == "variables")
                {
                    foreach (XmlNode grandchildNode in childNode.ChildNodes)
                    {
                        if (grandchildNode.Name == "param")
                        {
                            PropertiesTyped.Add(grandchildNode.Attributes["name"].Value, new PropertyMeta(grandchildNode.Attributes["type"].Value, grandchildNode.Attributes["default"].Value));
                        }
                    }
                }
            }
            RefreshPatternInfo(false);
        }

        float GetFloatValue(object value)
        {
            try
            {
                return ((CatalogResource.CatalogResource.TC04_Single)value).Unknown1;
            }
            catch (System.InvalidCastException)
            {
                return float.Parse(((CatalogResource.CatalogResource.TC01_String)value).Data, CultureInfo.InvariantCulture);
            }
        }

        void PopulateVariablesForGameObjectPatterns(s3pi.Interfaces.IPackage package, object presetMaterialBlock, ref string background, ref string rgbMask, List<string> channels, List<bool> channelsEnabled, ref float baseHueBackground, ref float baseSaturationBackground, ref float baseValueBackground, ref float hueBackground, ref float saturationBackground, ref float valueBackground, List<float> baseHues, List<float> baseSaturations, List<float> baseValues, List<float> hues, List<float> saturations, List<float> values, ref float[] hsvShiftBackground, List<float[]> hsvShift, List<float[]> rgbColors)
        {
            foreach (var propertyTypedKvp in PropertiesTyped)
            {
                var key = propertyTypedKvp.Key.ToLowerInvariant();
                var value = mProperties.ContainsKey(propertyTypedKvp.Key) ? mProperties[propertyTypedKvp.Key] : GameObjectPreset.CreateComplateOverrideInstance(propertyTypedKvp.Key, propertyTypedKvp.Value.DefaultValue, propertyTypedKvp.Value.Type, (CatalogResource.CatalogResource.MaterialBlock)presetMaterialBlock, package);
                if (key.StartsWith("channel"))
                {
                    if (key.EndsWith("enabled"))
                    {
                        channelsEnabled.Add(((CatalogResource.CatalogResource.TC07_Boolean)value).Unknown1);
                    }
                    else
                    {
                        channels.Add(((CatalogResource.CatalogResource.MaterialBlock)presetMaterialBlock).ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)value).TGIIndex].ReverseEvaluateResourceKey());
                    }
                }
                else if (key.StartsWith("color"))
                {
                    var color = System.Array.ConvertAll(System.BitConverter.GetBytes(((CatalogResource.CatalogResource.TC02_ARGB)value).ARGB), x => (float)x / byte.MaxValue);
                    rgbColors.Add(new float[]
                        {
                            color[2],
                            color[1],
                            color[0]
                        });
                }
                else if (key.StartsWith("base h"))
                {
                    if (key.EndsWith("bg"))
                    {
                        baseHueBackground = GetFloatValue(value);
                    }
                    else
                    {
                        baseHues.Add(GetFloatValue(value));
                    }
                }
                else if (key.StartsWith("base s"))
                {
                    if (key.EndsWith("bg"))
                    {
                        baseSaturationBackground = GetFloatValue(value);
                    }
                    else
                    {
                        baseSaturations.Add(GetFloatValue(value));
                    }
                }
                else if (key.StartsWith("base v"))
                {
                    if (key.EndsWith("bg"))
                    {
                        baseValueBackground = GetFloatValue(value);
                    }
                    else
                    {
                        baseValues.Add(GetFloatValue(value));
                    }
                }
                else if (key.StartsWith("h "))
                {
                    if (key.EndsWith("bg"))
                    {
                        hueBackground = GetFloatValue(value);
                    }
                    else
                    {
                        hues.Add(GetFloatValue(value));
                    }
                }
                else if (key.StartsWith("hsvshift"))
                {
                    float[] color;
                    try
                    {
                        var complateElement = (CatalogResource.CatalogResource.TC06_XYZ)value;
                        color = new float[]
                            {
                                complateElement.Unknown1,
                                complateElement.Unknown2,
                                complateElement.Unknown3
                            };
                    }
                    catch (System.InvalidCastException)
                    {
                        color = ParseCommaSeparatedValues(((CatalogResource.CatalogResource.TC01_String)value).Data);
                    }
                    if (key.EndsWith("bg"))
                    {
                        hsvShiftBackground = color;
                    }
                    else
                    {
                        hsvShift.Add(color);
                    }
                }
                else if (key.StartsWith("s "))
                {
                    if (key.EndsWith("bg"))
                    {
                        saturationBackground = GetFloatValue(value);
                    }
                    else
                    {
                        saturations.Add(GetFloatValue(value));
                    }
                }
                else if (key.StartsWith("v "))
                {
                    if (key.EndsWith("bg"))
                    {
                        valueBackground = GetFloatValue(value);
                    }
                    else
                    {
                        values.Add(GetFloatValue(value));
                    }
                }
                else
                {
                    switch (key)
                    {
                        case "background image":
                            background = ((CatalogResource.CatalogResource.MaterialBlock)presetMaterialBlock).ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)value).TGIIndex].ReverseEvaluateResourceKey();
                            break;
                        case "rgbmask":
                            rgbMask = ((CatalogResource.CatalogResource.MaterialBlock)presetMaterialBlock).ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)value).TGIIndex].ReverseEvaluateResourceKey();
                            break;
                    }
                }
            }
        }

        void SetValue(GameObjectPreset preset, string propertyName, string newValue, CmarNYCBorrowed.Action beforeMarkUnsaved = null)
        {
            GameObjectPreset.SetValue(preset, preset.MaterialBlock.MaterialBlocks.Find(x => x.Pattern == SlotName), propertyName, newValue, PropertiesTyped[propertyName].Type, mProperties, beforeMarkUnsaved ?? (() => RefreshPatternInfo(true, preset.MaterialBlock, preset.ParentPackage)));
        }

        public override string GetValue(string propertyName)
        {
            var material = Preset as GameObjectPreset;
            return material == null ? base.GetValue(propertyName) : GameObjectPreset.GetValue(material, propertyName, PropertiesTyped[propertyName].Type, mProperties);
        }

        public void RefreshPatternInfo(bool regeneratePresetTexture = true, object presetMaterialBlock = null, s3pi.Interfaces.IPackage package = null)
        {
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
            if (mProperties.Count > 0)
            {
                PopulateVariablesForGameObjectPatterns(package, presetMaterialBlock, ref background, ref rgbMask, channels, channelsEnabled, ref baseHueBackground, ref baseSaturationBackground, ref baseValueBackground, ref hueBackground, ref saturationBackground, ref valueBackground, baseHues, baseSaturations, baseValues, hues, saturations, values, ref hsvShiftBackground, hsvShift, rgbColors);
            }
            foreach (var propertyXmlNodeKvp in mPropertiesXmlNodes)
            {
                string key = propertyXmlNodeKvp.Key.ToLowerInvariant(),
                value = propertyXmlNodeKvp.Value.Attributes["value"].Value;
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
                    var color = ParseCommaSeparatedValues(value);
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
                        hsvShiftBackground = ParseCommaSeparatedValues(value);
                    }
                    else
                    {
                        hsvShift.Add(ParseCommaSeparatedValues(value));
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
            PatternInfo = new PatternInfo
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
                    Name = PatternInfo.Name,
                    RGBColors = rgbColors.ToArray(),
                    RGBMask = rgbMask,
                    SolidColor = rgbColors.Count == 1 ? rgbColors[0] : null
                };
            PatternImage = null;
            if (regeneratePresetTexture)
            {
                Preset.RegenerateTexture();
            }
        }

        public override void SetValue(string propertyName, string newValue, CmarNYCBorrowed.Action beforeMarkUnsaved = null)
        {
            var preset = Preset as GameObjectPreset;
            if (preset == null)
            {
                base.SetValue(propertyName, newValue, beforeMarkUnsaved ?? (() => RefreshPatternInfo()));
                return;
            }
            SetValue(preset, propertyName, newValue, beforeMarkUnsaved);
        }
    }
}
