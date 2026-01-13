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
    public class Preset : Complate, IPreset
    {
        protected readonly CASTableObject mCASTableObject;

        protected readonly PresetInternal mInternal;

        protected new readonly XmlDocument mXmlDocument;

        public string AmbientMap
        {
            get
            {
                return mInternal.AmbientMap;
            }
        }

        public override CASTableObject CASTableObject
        {
            get
            {
                return mCASTableObject;
            }
        }

        public override IPackage ParentPackage
        {
            get
            {
                return CASTableObject.ParentPackage;
            }
        }

        public List<Pattern> Patterns
        {
            get
            {
                return mInternal.Patterns;
            }
        }

        public string[] PatternSlotNames
        {
            get
            {
                return mInternal.PatternSlotNames;
            }
        }

        public override IDictionary<string, PropertyMeta> PropertiesTyped
        {
            get
            {
                return mInternal.PropertiesTyped;
            }
        }

        public override string[] PropertyNames
        {
            get
            {
                return mInternal.PropertyNames;
            }
        }

        public string SpecularMap
        {
            get
            {
                return mInternal.SpecularMap;
            }
        }

        public Bitmap Texture
        {
            get
            {
                return mInternal.Texture;
            }
        }

        public StringReader XmlFile
        {
            get
            {
                var stream = new MemoryStream();
                mXmlDocument.Save(new XmlTextWriter(stream, System.Text.Encoding.UTF8)
                    {
                        Formatting = Formatting.Indented
                    });
                stream.Position = 0;
                var text = new StreamReader(stream).ReadToEnd();
                return new StringReader(text.Substring(text.IndexOf("<preset>")));
            }
        }

        protected class PresetInternal : Complate
        {
            protected Bitmap mTexture;

            public string AmbientMap
            {
                get;
                private set;
            }

            public override CASTableObject CASTableObject
            {
                get
                {
                    return Preset.CASTableObject;
                }
            }

            public XmlNode ComplateXmlNode
            {
                get;
                private set;
            }

            public Bitmap NewTexture
            {
                get
                {
                    uint[] controlMapArray = null,
                    maskArray = null;
                    Bitmap diffuseMap = null,
                    multiplier = null,
                    overlay = null;
                    float[] diffuseColor = null,
                    highlightColor = null,
                    rootColor = null,
                    tipColor = null;
                    int height = 1024,
                    width = 1024;
                    List<Bitmap> logos = new List<Bitmap>(),
                    stencils = new List<Bitmap>();
                    List<bool> logosEnabled = new List<bool>(),
                    stencilsEnabled = new List<bool>();
                    List<float[]> logosLowerRight = new List<float[]>(),
                    logosUpperLeft = new List<float[]>();
                    List<float> logosRotation = new List<float>(),
                    stencilsRotation = new List<float>();
                    foreach (var propertyXmlNodeKvp in PropertiesXmlNodes)
                    {
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
                        else
                        {
                            switch (key)
                            {
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
                                case "tip color":
                                    tipColor = ParseCommaSeparatedValues(value);
                                    break;
                            }
                        }
                    }
                    if (diffuseMap != null)
                    {
                        if (mXmlDocument.SelectSingleNode("complate").Attributes["name"].Value.ToLowerInvariant() == "hairuniversal")
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
                            using (var graphics = Graphics.FromImage(diffuseMap))
                            {
                                var attributes = new ImageAttributes();
                                var colorMatrix = new ColorMatrix(hairMatrix);
                                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                graphics.DrawImage(diffuseMap, new Rectangle(0, 0, diffuseMap.Width, diffuseMap.Height), 0, 0, diffuseMap.Width, diffuseMap.Height, GraphicsUnit.Pixel, attributes);
                            }
                            if (controlMapArray != null && highlightColor != null && rootColor != null && tipColor != null)
                            {
                                try
                                {
                                    diffuseMap = diffuseMap.GetWithPatternsApplied(controlMapArray, new List<object>
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
                    var patternImages = Patterns.ConvertAll(x => bool.Parse(GetValue(x.SlotName + " Enabled")) ? x.PatternImage : null);
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
                    return texture;
                }
            }

            public override IPackage ParentPackage
            {
                get
                {
                    return Preset.ParentPackage;
                }
            }

            public readonly List<Pattern> Patterns;

            public string[] PatternSlotNames
            {
                get
                {
                    return Patterns.ConvertAll(x => x.SlotName).ToArray();
                }
            }

            public readonly Preset Preset;

            public IDictionary<string, XmlNode> PropertiesXmlNodes
            {
                get
                {
                    return mPropertiesXmlNodes;
                }
            }

            public string SpecularMap
            {
                get;
                private set;
            }

            public Bitmap Texture
            {
                get
                {
                    if (mTexture == null)
                    {
                        mTexture = NewTexture;
                    }
                    return mTexture;
                }
                set
                {
                    mTexture = value;
                }
            }

            public PresetInternal(Preset preset, XmlNode complateXmlNode) : base()
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
                        Patterns.Add(new Pattern(Preset, childNode));
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

            public void ReplacePresetComplate()
            {
                PropertiesTyped.Clear();
                var evaluated = ParentPackage.EvaluateResourceKey(ComplateXmlNode);
                mXmlDocument.LoadXml(new StreamReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)).ReadToEnd());
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
        }

        public Preset(CASTableObject CASTableObject, TextReader xmlFile)
        {
            mCASTableObject = CASTableObject;
            mXmlDocument = new XmlDocument();
            mXmlDocument.LoadXml(xmlFile.ReadToEnd());
            mInternal = new PresetInternal(this, mXmlDocument.SelectSingleNode("preset").SelectSingleNode("complate"));
        }

        public void AddPattern(string patternSlotName, string newComplateName)
        {
            mInternal.ComplateXmlNode.Attributes["name"].Value = newComplateName;
            mInternal.ComplateXmlNode.Attributes["reskey"].Value = "key:0333406C:00000000:" + System.Security.Cryptography.FNV64.GetHash(newComplateName).ToString("X16");
            var lastPatternSlotName = Patterns.FindLast(x => x.SlotName != "Logo").SlotName;
            foreach (XmlNode childNode in mInternal.ComplateXmlNode.ChildNodes)
            {
                if (childNode.Name == "value" && childNode.Attributes["key"].Value.StartsWith(lastPatternSlotName))
                {
                    var clonedNode = childNode.CloneNode(true);
                    clonedNode.Attributes["key"].Value = clonedNode.Attributes["key"].Value.Replace(lastPatternSlotName, patternSlotName);
                    mInternal.ComplateXmlNode.AppendChild(clonedNode);
                    mInternal.PropertiesXmlNodes.Add(clonedNode.Attributes["key"].Value, clonedNode);
                }
                if (childNode.Name == "pattern" && childNode.Attributes["variable"].Value == lastPatternSlotName)
                {
                    var clonedNode = childNode.CloneNode(true);
                    clonedNode.Attributes["variable"].Value = patternSlotName;
                    mInternal.ComplateXmlNode.AppendChild(clonedNode);
                    Patterns.Add(new Pattern(this, clonedNode));
                }
            }
            mInternal.ReplacePresetComplate();
        }

        public override string GetValue(string propertyName)
        {
            return mInternal.GetValue(propertyName);
        }

        public void RegenerateTexture()
        {
            new System.Threading.Thread(() =>
                {
                    mInternal.Texture = mInternal.NewTexture;
                    MarkModelsNeedUpdatedCallback();
                }).Start();
        }

        public void ReplacePattern(string patternSlotName, string patternKey)
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

        public override void SetValue(string propertyName, string newValue, CmarNYCBorrowed.Action beforeMarkUnsaved = null)
        {
            mInternal.SetValue(propertyName, newValue, beforeMarkUnsaved ?? RegenerateTexture);
        }
    }
}
