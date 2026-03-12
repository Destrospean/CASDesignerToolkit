using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;
using Destrospean.CmarNYCBorrowed;
using Destrospean.S3PIExtensions;
using s3pi.Interfaces;

namespace Destrospean.Common.Abstractions
{
    public class CASPartPreset : Preset
    {
        public string BodyAmbientMap
        {
            get
            {
                return ((PresetInternal)mInternal).BodyAmbientMap;
            }
        }

        public string BodySpecularMap
        {
            get
            {
                return ((PresetInternal)mInternal).BodySpecularMap;
            }
        }

        public Bitmap FaceTexture
        {
            get
            {
                return ((PresetInternal)mInternal).FaceTexture;
            }
        }

        public Bitmap ScalpTexture
        {
            get
            {
                return ((PresetInternal)mInternal).ScalpTexture;
            }
        }

        public string SkinAmbientMap
        {
            get
            {
                return ((PresetInternal)mInternal).SkinAmbientMap;
            }
        }

        public string SkinSpecularMap
        {
            get
            {
                return ((PresetInternal)mInternal).SkinSpecularMap;
            }
        }

        public StringReader XmlFile
        {
            get
            {
                using (var stream = new MemoryStream())
                {
                    mXmlDocument.Save(new XmlTextWriter(stream, System.Text.Encoding.UTF8)
                        {
                            Formatting = Formatting.Indented
                        });
                    stream.Position = 0;
                    using (var reader = new StreamReader(stream))
                    {
                        var text = reader.ReadToEnd();
                        return new StringReader(text.Substring(text.IndexOf("<preset>")));
                    }
                }
            }
        }

        protected class PresetInternal : PresetInternalBase
        {
            public string BodyAmbientMap, BodySpecularMap, SkinAmbientMap, SkinSpecularMap;

            public XmlNode ComplateXmlNode
            {
                get;
                private set;
            }

            public Bitmap FaceTexture, ScalpTexture;

            public override Bitmap NewTexture
            {
                get
                {
                    uint[] controlMapArray = null,
                    faceControlMapArray = null,
                    maskArray = null,
                    scalpControlMapArray = null;
                    Bitmap diffuseMap = null,
                    faceDiffuseMap = null,
                    multiplier = null,
                    overlay = null,
                    scalpDiffuseMap = null;
                    float[] diffuseColor = null,
                    highlightColor = null,
                    rootColor = null,
                    tintColor =
                        {
                            1,
                            1,
                            1
                        },
                    tipColor = null;
                    bool drawsOnFace = false, drawsOnScalp = false;
                    int height = 1024,
                    width = 1024;
                    List<Bitmap> logos = new List<Bitmap>(),
                    stencils = new List<Bitmap>();
                    List<bool> logosEnabled = new List<bool>(),
                    stencilsEnabled = new List<bool>(),
                    tintColorsEnabled = new List<bool>();
                    List<float[]> logosLowerRight = new List<float[]>(),
                    logosUpperLeft = new List<float[]>(),
                    tintColors = new List<float[]>();
                    List<float> logosRotation = new List<float>(),
                    stencilsRotation = new List<float>();
                    foreach (var propertyXmlNodeKvp in PropertiesXmlNodes)
                    {
                        if (!PropertiesTyped.ContainsKey(propertyXmlNodeKvp.Key))
                        {
                            continue;
                        }
                        string key = propertyXmlNodeKvp.Key.ToLowerInvariant(),
                        value = propertyXmlNodeKvp.Value.Attributes["value"].Value;
                        if (key.StartsWith("logo"))
                        {
                            if (key.EndsWith("enabled"))
                            {
                                logosEnabled.Add(bool.Parse(value));
                            }
                            else if (key.EndsWith("lowerright"))
                            {
                                logosLowerRight.Add(ParseCommaSeparatedValues(value));
                            }
                            else if (key.EndsWith("upperleft"))
                            {
                                logosUpperLeft.Add(ParseCommaSeparatedValues(value));
                            }
                            else if (key.EndsWith("rotation"))
                            {
                                logosRotation.Add(float.Parse(value, System.Globalization.CultureInfo.InvariantCulture));
                            }
                            else if (key.EndsWith("texture"))
                            {
                                logos.Add(ParentPackage.GetTexture(value, GetTextureCallback, width, height));
                            }
                        }
                        else if (key.StartsWith("stencil"))
                        {
                            if (key.Length == 9)
                            {
                                stencils.Add(ParentPackage.GetTexture(value, GetTextureCallback, width, height));
                            }
                            else if (key.EndsWith("enabled"))
                            {
                                stencilsEnabled.Add(bool.Parse(value));
                            }
                            else if (key.EndsWith("rotation"))
                            {
                                stencilsRotation.Add(float.Parse(value, System.Globalization.CultureInfo.InvariantCulture));
                            }
                        }
                        else if (key.StartsWith("tint color"))
                        {
                            if (key.Length == 10)
                            {
                                tintColor = ParseCommaSeparatedValues(value);
                            }
                            else if (key.Length == 12)
                            {
                                tintColors.Add(ParseCommaSeparatedValues(value));
                            }
                            else if (key.EndsWith("enabled"))
                            {
                                tintColorsEnabled.Add(bool.Parse(value));
                            }
                        }
                        else
                        {
                            switch (key)
                            {
                                case "ambient":
                                    AmbientMap = value;
                                    goto case "skin ambient";
                                case "body ambient":
                                    BodyAmbientMap = value;
                                    break;
                                case "body specular":
                                    BodySpecularMap = value;
                                    break;
                                case "clothing ambient":
                                    AmbientMap = value;
                                    break;
                                case "clothing specular":
                                    SpecularMap = value;
                                    break;
                                case "control map":
                                    controlMapArray = ParentPackage.GetTextureARGBArray(value, GetTextureCallback, width, height);
                                    break;
                                case "diffuse color":
                                    diffuseColor = ParseCommaSeparatedValues(value);
                                    break;
                                case "diffuse map":
                                    diffuseMap = ParentPackage.GetTexture(value, GetTextureCallback, width, height);
                                    break;
                                case "drawsonface":
                                    drawsOnFace = bool.Parse(value);
                                    break;
                                case "drawsonscalp":
                                    drawsOnScalp = bool.Parse(value);
                                    break;
                                case "face ambient":
                                    goto case "ambient";
                                case "face control map":
                                    faceControlMapArray = ParentPackage.GetTextureARGBArray(value, GetTextureCallback, width, height);
                                    break;
                                case "face diffuse map":
                                    faceDiffuseMap = ParentPackage.GetTexture(value, GetTextureCallback, width, height);
                                    break;
                                case "face overlay":
                                    goto case "overlay";
                                case "face specular":
                                    goto case "specular";
                                case "highlight color":
                                    highlightColor = ParseCommaSeparatedValues(value);
                                    break;
                                case "mask":
                                    maskArray = ParentPackage.GetTextureARGBArray(value, GetTextureCallback, width, height);
                                    break;
                                case "multiplier":
                                    multiplier = ParentPackage.GetTexture(value, GetTextureCallback, width, height);
                                    break;
                                case "overlay":
                                    overlay = ParentPackage.GetTexture(value, GetTextureCallback, width, height);
                                    break;
                                case "root color":
                                    rootColor = ParseCommaSeparatedValues(value);
                                    break;
                                case "scalp control map":
                                    scalpControlMapArray = ParentPackage.GetTextureARGBArray(value, GetTextureCallback, width, height);
                                    break;
                                case "scalp diffuse map":
                                    scalpDiffuseMap = ParentPackage.GetTexture(value, GetTextureCallback, width, height);
                                    break;
                                case "skin ambient":
                                    SkinAmbientMap = value;
                                    break;
                                case "skin specular":
                                    SkinSpecularMap = value;
                                    break;
                                case "specular":
                                    SpecularMap = value;
                                    goto case "skin specular";
                                case "tip color":
                                    tipColor = ParseCommaSeparatedValues(value);
                                    break;
                            }
                        }
                    }
                    var complateName = mXmlDocument.SelectSingleNode("complate").Attributes["name"].Value.ToLowerInvariant();
                    var diffuseMaps = new Bitmap[]
                        {
                            diffuseMap,
                            faceDiffuseMap,
                            scalpDiffuseMap
                        };
                    var controlMapArrays = new uint[][]
                        {
                            controlMapArray,
                            faceControlMapArray,
                            scalpControlMapArray
                        };
                    for (var i = 0; i < diffuseMaps.Length; i++)
                    {
                        if (diffuseMaps[i] != null && complateName == "hairuniversal")
                        {
                            float[][] hairMatrix =
                                {
                                    new float[]
                                    {
                                        diffuseColor[0],
                                        0,
                                        0,
                                        0,
                                        0
                                    },
                                    new float[]
                                    {
                                        0,
                                        diffuseColor[1],
                                        0,
                                        0,
                                        0
                                    },
                                    new float[]
                                    {
                                        0,
                                        0,
                                        diffuseColor[2],
                                        0,
                                        0
                                    },
                                    new float[]
                                    {
                                        0,
                                        0,
                                        0,
                                        1,
                                        0
                                    },
                                    new float[]
                                    {
                                        0,
                                        0,
                                        0,
                                        0,
                                        1
                                    }
                                };
                            using (var graphics = Graphics.FromImage(diffuseMaps[i]))
                            {
                                var attributes = new ImageAttributes();
                                var colorMatrix = new ColorMatrix(hairMatrix);
                                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                graphics.DrawImage(diffuseMaps[i], new Rectangle(0, 0, diffuseMaps[i].Width, diffuseMaps[i].Height), 0, 0, diffuseMaps[i].Width, diffuseMaps[i].Height, GraphicsUnit.Pixel, attributes);
                            }
                            if (controlMapArrays[i] != null)
                            {
                                try
                                {
                                    diffuseMaps[i] = diffuseMaps[i].GetWithPatternsApplied(controlMapArrays[i], new List<object>
                                        {
                                            rootColor,
                                            highlightColor,
                                            tipColor
                                        }, false);
                                }
                                catch (System.IndexOutOfRangeException)
                                {
                                }
                            }
                        }
                    }
                    diffuseMap = diffuseMaps[0];
                    faceDiffuseMap = diffuseMaps[1];
                    scalpDiffuseMap = diffuseMaps[2];
                    if (complateName.StartsWith("casoverlay") || complateName.StartsWith("casskinoverlay"))
                    {
                        float[][] faceMatrix =
                            {
                                new float[]
                                {
                                    tintColor[0],
                                    0,
                                    0,
                                    0,
                                    0
                                },
                                new float[]
                                {
                                    0,
                                    tintColor[1],
                                    0,
                                    0,
                                    0
                                },
                                new float[]
                                {
                                    0,
                                    0,
                                    tintColor[2],
                                    0,
                                    0
                                },
                                new float[]
                                {
                                    0,
                                    0,
                                    0,
                                    1,
                                    0
                                },
                                new float[]
                                {
                                    0,
                                    0,
                                    0,
                                    0,
                                    1
                                }
                            };
                        using (var graphics = Graphics.FromImage(overlay))
                        {
                            var attributes = new ImageAttributes();
                            var colorMatrix = new ColorMatrix(faceMatrix);
                            attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            graphics.DrawImage(overlay, new Rectangle(0, 0, overlay.Width, overlay.Height), 0, 0, overlay.Width, overlay.Height, GraphicsUnit.Pixel, attributes);
                        }
                        if (maskArray == null)
                        {
                            maskArray = new uint[overlay.Height * overlay.Width >> 2];
                            for (var i = 0; i < maskArray.Length; i++)
                            {
                                maskArray[i] = 0;
                            }
                        }
                        while (tintColorsEnabled.Count < tintColors.Count)
                        {
                            tintColorsEnabled.Add(false);
                        }
                        for (var i = 0; i < tintColors.Count; i++)
                        {
                            if (!tintColorsEnabled[i])
                            {
                                tintColors[i] = null;
                            }
                        }
                        try
                        {
                            overlay = overlay.GetWithPatternsApplied(maskArray, tintColors.ConvertAll(x => (object)x), true);
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                        }
                    }
                    bool patternEnabled;
                    var patternImages = Patterns.FindAll(x => x.SlotName != "Logo").ConvertAll(x => bool.TryParse(GetValue(x.SlotName + " Enabled"), out patternEnabled) && patternEnabled ? x.PatternImage : null);
                    if (maskArray != null)
                    {
                        if (multiplier != null)
                        {
                            try
                            {
                                multiplier = multiplier.GetWithPatternsApplied(maskArray, patternImages, false);
                            }
                            catch (System.IndexOutOfRangeException)
                            {
                            }
                        }
                        if (overlay != null)
                        {
                            try
                            {
                                overlay = overlay.GetWithPatternsApplied(maskArray, patternImages, true);
                            }
                            catch (System.IndexOutOfRangeException)
                            {
                            }
                        }
                    }
                    var texture = new Bitmap(width, height);
                    using (var graphics = Graphics.FromImage(texture))
                    {
                        /*
                        var casPart = CASTableObject as CASPart;
                        if (multiplier == null && diffuseMap == null && casPart != null)
                        {
                            foreach (var geomAndKey in new List<List<CASPart.GEOMAndKey>>(casPart.LODs.Values)[0])
                            {
                                foreach (var field in geomAndKey.GEOM.Shader.GetFields())
                                {
                                    if (field != (uint)s3pi.GenericRCOLResource.FieldType.DiffuseMap)
                                    {
                                        continue;
                                    }
                                    int valueType;
                                    var tgi = geomAndKey.GEOM.TGIList[(uint)geomAndKey.GEOM.Shader.GetFieldValue(field, out valueType)[0]];
                                    graphics.DrawImage(ParentPackage.GetTexture(new ResourceKey(tgi.Type, tgi.Group, tgi.Instance).ReverseEvaluateResourceKey(), GetTextureCallback, width, height), 0, 0);
                                    break;
                                }
                            }
                        }
                        */
                        if (diffuseMap != null)
                        {
                            graphics.DrawImage(diffuseMap, 0, 0);
                        }
                        if (multiplier != null)
                        {
                            graphics.DrawImage(multiplier, 0, 0);
                        }
                        if (overlay != null)
                        {
                            graphics.DrawImage(overlay, 0, 0);
                        }
                        for (var i = 0; i < stencils.Count; i++)
                        {
                            if (stencilsEnabled[i])
                            {
                                graphics.DrawImage(RotateImage(QuadrupleCanvasSize(stencils[i]), stencilsRotation[i] * 360), -stencils[i].Width >> 1, -stencils[i].Height >> 1);
                            }
                        }
                        for (var i = 0; i < logos.Count; i++)
                        {
                            if (logosEnabled[i])
                            {
                                int logoHeight = (int)((logosLowerRight[i][1] - logosUpperLeft[i][1]) * height),
                                logoWidth = (int)((logosLowerRight[i][0] - logosUpperLeft[i][0]) * width);
                                graphics.DrawImage(RotateImage(QuadrupleCanvasSize(logos[i]), logosRotation[i] * 360), logosUpperLeft[i][0] * width - (logoWidth >> 1), logosUpperLeft[i][1] * height - (logoHeight >> 1), logoWidth << 1, logoHeight << 1);
                            }
                        }
                    }
                    if (FaceTexture != null)
                    {
                        FaceTexture.Dispose();
                    }
                    if (ScalpTexture != null)
                    {
                        ScalpTexture.Dispose();
                    }
                    if (drawsOnFace)
                    {
                        FaceTexture = faceDiffuseMap;
                    }
                    else
                    {
                        FaceTexture = null;
                    }
                    if (drawsOnScalp)
                    {
                        ScalpTexture = scalpDiffuseMap;
                    }
                    else
                    {
                        ScalpTexture = null;
                    }
                    return texture;
                }
            }

            public IDictionary<string, XmlNode> PropertiesXmlNodes
            {
                get
                {
                    return mPropertiesXmlNodes;
                }
            }

            public PresetInternal(CASPartPreset preset, XmlNode complateXmlNode) : base()
            {
                Preset = preset;
                ComplateXmlNode = complateXmlNode;
                var evaluated = ParentPackage.EvaluateResourceKey(ComplateXmlNode);
                mXmlDocument.LoadXml(new StreamReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)).ReadToEnd());
                Patterns = new List<Pattern>();
                foreach (XmlNode childNode in ComplateXmlNode.ChildNodes)
                {
                    if (childNode.Name == "value")
                    {
                        PropertiesXmlNodes.Add(childNode.Attributes["key"].Value, childNode);
                    }
                    if (childNode.Name == "pattern")
                    {
                        Patterns.Add(new Pattern((CASPartPreset)Preset, childNode));
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
            }

            public override void ReplacePresetComplate()
            {
                ReplacePresetComplate(ParentPackage.EvaluateResourceKey(ComplateXmlNode));
            }
        }

        public CASPartPreset(CASTableObject CASTableObject, TextReader xmlFile)
        {
            mCASTableObject = CASTableObject;
            mXmlDocument.LoadXml(xmlFile.ReadToEnd());
            mInternal = new PresetInternal(this, mXmlDocument.SelectSingleNode("preset").SelectSingleNode("complate"));
        }

        public override void AddPattern(string patternSlotName, string newComplateName)
        {
            var presetInternal = (PresetInternal)mInternal;
            presetInternal.ComplateXmlNode.Attributes["name"].Value = newComplateName;
            presetInternal.ComplateXmlNode.Attributes["reskey"].Value = "key:0333406C:00000000:" + System.Security.Cryptography.FNV64.GetHash(newComplateName).ToString("X16");
            var lastPatternSlotName = Patterns.FindLast(x => x.SlotName != "Logo").SlotName;
            foreach (XmlNode childNode in presetInternal.ComplateXmlNode.ChildNodes)
            {
                if (childNode.Name == "value" && childNode.Attributes["key"].Value.StartsWith(lastPatternSlotName))
                {
                    var clonedNode = childNode.CloneNode(true);
                    clonedNode.Attributes["key"].Value = clonedNode.Attributes["key"].Value.Replace(lastPatternSlotName, patternSlotName);
                    presetInternal.ComplateXmlNode.AppendChild(clonedNode);
                    presetInternal.PropertiesXmlNodes.Add(clonedNode.Attributes["key"].Value, clonedNode);
                }
                if (childNode.Name == "pattern" && childNode.Attributes["variable"].Value == lastPatternSlotName)
                {
                    var clonedNode = childNode.CloneNode(true);
                    clonedNode.Attributes["variable"].Value = patternSlotName;
                    presetInternal.ComplateXmlNode.AppendChild(clonedNode);
                    Patterns.Add(new Pattern(this, clonedNode));
                }
            }
            mInternal.ReplacePresetComplate();
        }

        public override void ReplacePattern(string patternSlotName, string patternKey)
        {
            var evaluated = ParentPackage.EvaluateResourceKey(patternKey);
            int i = 0,
            patternIndex = Patterns.FindIndex(x => x.SlotName == patternSlotName);
            var patternXmlDocument = new XmlDocument();
            patternXmlDocument.LoadXml(new StreamReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)).ReadToEnd());
            foreach (XmlNode presetChildNode in mXmlDocument.SelectSingleNode("preset").SelectSingleNode("complate").ChildNodes)
            {
                if (presetChildNode.Name == "pattern" && i++ == patternIndex)
                {
                    presetChildNode.Attributes["name"].Value = patternXmlDocument.SelectSingleNode("complate").Attributes["name"].Value;
                    presetChildNode.Attributes["reskey"].Value = patternKey;
                    for (var j = presetChildNode.ChildNodes.Count - 1; j > -1; j--)
                    {
                        switch (presetChildNode.ChildNodes[j].Attributes["key"].Value.ToLowerInvariant())
                        {
                            case "assetroot":
                            case "filename":
                                break;
                            default:
                                presetChildNode.RemoveChild(presetChildNode.ChildNodes[j]);
                                break;
                        }
                    }
                    foreach (XmlNode patternChildNode in patternXmlDocument.SelectSingleNode("complate").ChildNodes)
                    {
                        if (patternChildNode.Name == "variables")
                        {
                            foreach (XmlNode patternGrandchildNode in patternChildNode.ChildNodes)
                            {
                                if (patternGrandchildNode.Name == "param")
                                {
                                    var defaultValue = patternGrandchildNode.Attributes["default"].Value;
                                    var valueElement = mXmlDocument.CreateElement("value");
                                    valueElement.SetAttribute("key", patternGrandchildNode.Attributes["name"].Value);
                                    valueElement.SetAttribute("value", patternGrandchildNode.Attributes["type"].Value == "texture" && defaultValue.StartsWith("($assetRoot)") ? "key:00B2D882:00000000:" + System.Security.Cryptography.FNV64.GetHash(defaultValue.Substring(defaultValue.LastIndexOf("\\") + 1, defaultValue.LastIndexOf(".") - defaultValue.LastIndexOf("\\") - 1)).ToString("X16") : defaultValue);
                                    presetChildNode.AppendChild(valueElement);
                                }
                            }
                        }
                    }
                    Patterns[patternIndex] = new Pattern(this, presetChildNode);
                    break;
                }
            }
        }
    }
}
