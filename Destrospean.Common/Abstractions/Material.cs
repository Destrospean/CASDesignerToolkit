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
    public class Material : Complate, IPreset
    {
        protected readonly CASTableObject mCASTableObject;

        protected readonly MaterialInternal mInternal;

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

        public CatalogResource.CatalogResource.MaterialBlock MaterialBlock
        {
            get
            {
                return mInternal.MaterialBlock;
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

        public override IDictionary<string, string> PropertiesTyped
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

        protected class MaterialInternal : Complate
        {
            protected readonly IDictionary<string, object> mProperties = new SortedDictionary<string, object>(new PropertyNameComparer());

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
                    return Material.CASTableObject;
                }
            }

            public readonly Material Material;

            public CatalogResource.CatalogResource.MaterialBlock MaterialBlock;

            public Bitmap NewTexture
            {
                get
                {
                    uint[] maskArray = null;
                    Bitmap multiplier = null,
                    overlay = null;
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
                    foreach (var propertyKvp in Properties)
                    {
                        var key = propertyKvp.Key.ToLowerInvariant();
                        var value = propertyKvp.Value;
                        if (key.StartsWith("logo"))
                        {
                            if (key.EndsWith("enabled"))
                            {
                                logosEnabled.Add(((CatalogResource.CatalogResource.TC07_Boolean)value).Unknown1);
                            }
                            else if (key.EndsWith("lowerright"))
                            {
                                logosLowerRight.Add(new float[]
                                    {
                                        ((CatalogResource.CatalogResource.TC05_XY)value).Unknown1,
                                        ((CatalogResource.CatalogResource.TC05_XY)value).Unknown2
                                    });
                            }
                            else if (key.EndsWith("upperleft"))
                            {
                                logosUpperLeft.Add(new float[]
                                    {
                                        ((CatalogResource.CatalogResource.TC05_XY)value).Unknown1,
                                        ((CatalogResource.CatalogResource.TC05_XY)value).Unknown2
                                    });
                            }
                            else if (key.EndsWith("rotation"))
                            {
                                logosRotation.Add(((CatalogResource.CatalogResource.TC04_Single)value).Unknown1);
                            }
                            else if (key.EndsWith("texture"))
                            {
                                logos.Add(ParentPackage.GetTexture(MaterialBlock.ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)value).TGIIndex].ReverseEvaluateResourceKey(), GetTextureCallback, width, height));
                            }
                        }
                        else if (key.StartsWith("stencil"))
                        {
                            if (key.Length == 9)
                            {
                                stencils.Add(ParentPackage.GetTexture(MaterialBlock.ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)value).TGIIndex].ReverseEvaluateResourceKey(), GetTextureCallback, width, height));
                            }
                            else if (key.EndsWith("enabled"))
                            {
                                stencilsEnabled.Add(((CatalogResource.CatalogResource.TC07_Boolean)value).Unknown1);
                            }
                            else if (key.EndsWith("rotation"))
                            {
                                stencilsRotation.Add(((CatalogResource.CatalogResource.TC04_Single)value).Unknown1);
                            }
                        }
                        else
                        {
                            switch (key)
                            {
                                case "ambient":
                                    AmbientMap = MaterialBlock.ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)value).TGIIndex].ReverseEvaluateResourceKey();
                                    break;
                                case "mask":
                                    maskArray = ParentPackage.GetTextureARGBArray(MaterialBlock.ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)value).TGIIndex].ReverseEvaluateResourceKey(), GetTextureCallback, width, height);
                                    break;
                                case "multiplier":
                                    multiplier = ParentPackage.GetTexture(MaterialBlock.ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)value).TGIIndex].ReverseEvaluateResourceKey(), GetTextureCallback, width, height);
                                    break;
                                case "overlay":
                                    overlay = ParentPackage.GetTexture(MaterialBlock.ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)value).TGIIndex].ReverseEvaluateResourceKey(), GetTextureCallback, width, height);
                                    break;
                                case "specular":
                                    SpecularMap = MaterialBlock.ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)value).TGIIndex].ReverseEvaluateResourceKey();
                                    break;
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
                    return Material.ParentPackage;
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

            public IDictionary<string, object> Properties
            {
                get
                {
                    return mProperties;
                }
            }

            public override string[] PropertyNames
            {
                get
                {
                    return new List<string>(Properties.Keys).ToArray();
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

            public MaterialInternal(Material material, CatalogResource.CatalogResource.MaterialBlock materialBlock) : base()
            {
                Material = material;
                MaterialBlock = materialBlock;
                var evaluated = ParentPackage.EvaluateResourceKey(MaterialBlock.ParentTGIBlocks[MaterialBlock.ComplateXMLIndex].ReverseEvaluateResourceKey());
                mXmlDocument.LoadXml(new StreamReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)).ReadToEnd());
                Patterns = new List<Pattern>();
                foreach (var patternMaterialBlock in MaterialBlock.MaterialBlocks)
                {
                    Patterns.Add(new Pattern(material, patternMaterialBlock, MaterialBlock));
                }
                foreach (var complateElement in MaterialBlock.ComplateOverrides)
                {
                    Properties.Add(complateElement.VariableName, complateElement);
                }
                foreach (XmlNode childNode in mXmlDocument.SelectSingleNode("complate").ChildNodes)
                {
                    if (childNode.Name == "variables")
                    {
                        foreach (XmlNode grandchildNode in childNode.ChildNodes)
                        {
                            if (grandchildNode.Name == "param")
                            {
                                PropertiesTyped.Add(grandchildNode.Attributes["name"].Value, grandchildNode.Attributes["type"].Value);
                            }
                        }
                    }
                }
            }

            public override string GetValue(string propertyName)
            {
                return Material.GetValue(Material, propertyName, PropertiesTyped[propertyName], Properties);
            }

            public void ReplaceMaterialComplate()
            {
                PropertiesTyped.Clear();
                var evaluated = ParentPackage.EvaluateResourceKey(MaterialBlock.ParentTGIBlocks[MaterialBlock.ComplateXMLIndex]);
                mXmlDocument.LoadXml(new StreamReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)).ReadToEnd());
                foreach (XmlNode childNode in mXmlDocument.SelectSingleNode("complate").ChildNodes)
                {
                    if (childNode.Name == "variables")
                    {
                        foreach (XmlNode grandchildNode in childNode.ChildNodes)
                        {
                            if (grandchildNode.Name == "param")
                            {
                                PropertiesTyped.Add(grandchildNode.Attributes["name"].Value, grandchildNode.Attributes["type"].Value);
                            }
                        }
                    }
                }
            }

            public override void SetValue(string propertyName, string newValue, Action beforeMarkUnsaved = null)
            {
                Material.SetValue(Material, propertyName, newValue, PropertiesTyped[propertyName], Properties, beforeMarkUnsaved);
            }
        }

        public Material(CASTableObject castableObject, CatalogResource.CatalogResource.MaterialBlock materialBlock)
        {
            mCASTableObject = castableObject;
            mInternal = new MaterialInternal(this, materialBlock);
        }

        public void AddPattern(string patternSlotName, string newComplateName)
        {
            MaterialBlock.Name = newComplateName;
            MaterialBlock.ParentTGIBlocks[MaterialBlock.ComplateXMLIndex] = new TGIBlock(0, null, ResourceUtils.GetResourceType("_XML"), 0, System.Security.Cryptography.FNV64.GetHash(newComplateName));
            var lastPatternSlotName = Patterns.FindLast(x => x.SlotName != "Logo").SlotName;
            foreach (var complateOverride in MaterialBlock.ComplateOverrides)
            {
                if (complateOverride.VariableName.StartsWith(lastPatternSlotName))
                {
                    var clonedComplateOverride = (CatalogResource.CatalogResource.ComplateElement)complateOverride.Clone(null);
                    clonedComplateOverride.VariableName = clonedComplateOverride.VariableName.Replace(lastPatternSlotName, patternSlotName);
                    MaterialBlock.ComplateOverrides.Add(clonedComplateOverride);
                    mInternal.Properties.Add(clonedComplateOverride.VariableName, clonedComplateOverride);
                }
            }
            foreach (var materialBlock in MaterialBlock.MaterialBlocks)
            {
                if (materialBlock.Pattern == lastPatternSlotName)
                {
                    var clonedMaterialBlock = (CatalogResource.CatalogResource.MaterialBlock)materialBlock.Clone(null);
                    clonedMaterialBlock.Pattern = patternSlotName;
                    MaterialBlock.MaterialBlocks.Add(clonedMaterialBlock);
                    Patterns.Add(new Pattern(this, clonedMaterialBlock, MaterialBlock));
                }
            }
            mInternal.ReplaceMaterialComplate();
        }

        public static object CreateComplateOverrideInstance(string name, string value, string type, CatalogResource.CatalogResource.MaterialBlock materialBlock, IPackage package)
        {
            switch (type)
            {
                case "bool":
                    return new CatalogResource.CatalogResource.TC07_Boolean(0, null, 0, name, bool.Parse(value));
                case "color":
                    var rgba = System.Array.ConvertAll(ParseCommaSeparatedValues(value), x => (byte)(x * byte.MaxValue));
                    return new CatalogResource.CatalogResource.TC02_ARGB(0, null, 0, name, ((uint)rgba[3] << 24) + ((uint)rgba[0] << 16) + ((uint)rgba[1] << 8) + rgba[2]);
                case "float":
                    return new CatalogResource.CatalogResource.TC04_Single(0, null, 0, name, float.Parse(value));
                case "pattern":
                    return new CatalogResource.CatalogResource.TC01_String(0, null, 0, name, value);
                case "string":
                    try
                    {
                        var commaSeparatedValues = ParseCommaSeparatedValues(value);
                        return new CatalogResource.CatalogResource.TC06_XYZ(0, null, 0, name, commaSeparatedValues[0], commaSeparatedValues[1], commaSeparatedValues[2]);
                    }
                    catch (System.InvalidCastException)
                    {
                        return new CatalogResource.CatalogResource.TC01_String(0, null, 0, name, value);
                    }
                case "texture":
                    var key = value.StartsWith("($assetRoot)") ? "key:00B2D882:00000000:" + System.Security.Cryptography.FNV64.GetHash(value.Substring(value.LastIndexOf("\\") + 1, value.LastIndexOf(".") - value.LastIndexOf("\\") - 1)).ToString("X16") : value;
                    var index = materialBlock.ParentTGIBlocks.FindIndex(x => x.ReverseEvaluateResourceKey() == key);
                    if (index == -1)
                    {
                        var evaluated = package.EvaluateImageResourceKey(key);
                        materialBlock.ParentTGIBlocks.Add(new TGIBlock(0, null, evaluated.ResourceIndexEntry.ResourceType, evaluated.ResourceIndexEntry.ResourceGroup, evaluated.ResourceIndexEntry.Instance));
                        return new CatalogResource.CatalogResource.TC03_TGIIndex(0, null, 0, name, (byte)(materialBlock.ParentTGIBlocks.Count - 1));
                    }
                    return new CatalogResource.CatalogResource.TC03_TGIIndex(0, null, 0, name, (byte)index);
                case "vec2":
                    var coordinates = ParseCommaSeparatedValues(value);
                    return new CatalogResource.CatalogResource.TC05_XY(0, null, 0, name, coordinates[0], coordinates[1]);
                default:
                    return null;
            }
        }

        public static string GetValue(Material material, string propertyName, string type, IDictionary<string, object> properties)
        {
            switch (type)
            {
                case "bool":
                    return ((CatalogResource.CatalogResource.TC07_Boolean)properties[propertyName]).Unknown1.ToString();
                case "color":
                    var argb = System.Array.ConvertAll(System.BitConverter.GetBytes(((CatalogResource.CatalogResource.TC02_ARGB)properties[propertyName]).ARGB), x => ((float)x / byte.MaxValue).ToString());
                    return string.Join(",", new string[]
                        {
                            argb[2],
                            argb[1],
                            argb[0],
                            argb[3]
                        });
                case "float":
                    try
                    {
                        return ((CatalogResource.CatalogResource.TC04_Single)properties[propertyName]).Unknown1.ToString();
                    }
                    catch (System.InvalidCastException)
                    {
                        return ((CatalogResource.CatalogResource.TC01_String)properties[propertyName]).Data;
                    }
                case "pattern":
                    return ((CatalogResource.CatalogResource.TC01_String)properties[propertyName]).Data;
                case "string":
                    try
                    {
                        var complateElement = ((CatalogResource.CatalogResource.TC06_XYZ)properties[propertyName]);
                        return string.Join(",", new string[]
                            {
                                complateElement.Unknown1.ToString(),
                                complateElement.Unknown2.ToString(),
                                complateElement.Unknown3.ToString()
                            });
                    }
                    catch (System.InvalidCastException)
                    {
                        return ((CatalogResource.CatalogResource.TC01_String)properties[propertyName]).Data;
                    }
                case "texture":
                    return material.MaterialBlock.ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)properties[propertyName]).TGIIndex].ReverseEvaluateResourceKey();
                case "vec2":
                    return ((CatalogResource.CatalogResource.TC05_XY)properties[propertyName]).Unknown1.ToString() + "," + ((CatalogResource.CatalogResource.TC05_XY)properties[propertyName]).Unknown2.ToString();
                default:
                    return null;
            }
        }

        public override string GetValue(string propertyName)
        {
            return mInternal.GetValue(propertyName);
        }

        public void RegenerateTexture()
        {
            new System.Threading.Thread(() =>
                {
                    System.Threading.Thread.Sleep(1);
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
            foreach (var patternMaterialBlock in MaterialBlock.MaterialBlocks)
            {
                if (i++ == patternIndex)
                {
                    patternMaterialBlock.Name = patternXmlDocument.SelectSingleNode("complate").Attributes["name"].Value;
                    var index = patternMaterialBlock.ParentTGIBlocks.FindIndex(x => x.ReverseEvaluateResourceKey() == patternKey);
                    if (index == -1)
                    {
                        patternMaterialBlock.ComplateXMLIndex = (byte)patternMaterialBlock.ParentTGIBlocks.Count;
                        patternMaterialBlock.ParentTGIBlocks.Add(new TGIBlock(0, null, evaluated.ResourceIndexEntry.ResourceType, evaluated.ResourceIndexEntry.ResourceGroup, evaluated.ResourceIndexEntry.Instance));
                    }
                    else
                    {
                        patternMaterialBlock.ComplateXMLIndex = (byte)index;
                    }
                    for (var j = patternMaterialBlock.ComplateOverrides.Count - 1; j > -1; j--)
                    {
                        switch (patternMaterialBlock.ComplateOverrides[j].VariableName.ToLowerInvariant())
                        {
                            case "assetroot":
                            case "filename":
                                break;
                            default:
                                patternMaterialBlock.ComplateOverrides.RemoveAt(j);
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
                                    patternMaterialBlock.ComplateOverrides.Add((CatalogResource.CatalogResource.ComplateElement)CreateComplateOverrideInstance(patternGrandchildNode.Attributes["name"].Value, patternGrandchildNode.Attributes["default"].Value, patternGrandchildNode.Attributes["type"].Value, patternMaterialBlock, ParentPackage));
                                }
                            }
                        }
                    }
                    Patterns[patternIndex] = new Pattern(this, patternMaterialBlock, MaterialBlock);
                    break;
                }
            }
        }

        public static void SetValue(Material material, string propertyName, string newValue, string type, IDictionary<string, object> properties, CmarNYCBorrowed.Action beforeMarkUnsaved = null)
        {
            switch (type)
            {
                case "bool":
                    ((CatalogResource.CatalogResource.TC07_Boolean)properties[propertyName]).Unknown1 = bool.Parse(newValue);
                    break;
                case "color":
                    var rgba = System.Array.ConvertAll(ParseCommaSeparatedValues(newValue), x => (byte)(x * byte.MaxValue));
                    ((CatalogResource.CatalogResource.TC02_ARGB)properties[propertyName]).ARGB = ((uint)rgba[3] << 24) + ((uint)rgba[0] << 16) + ((uint)rgba[1] << 8) + rgba[2];
                    break;
                case "float":
                    try
                    {
                        ((CatalogResource.CatalogResource.TC04_Single)properties[propertyName]).Unknown1 = float.Parse(newValue);
                    }
                    catch (System.InvalidCastException)
                    {
                        ((CatalogResource.CatalogResource.TC01_String)properties[propertyName]).Data = newValue;
                    }
                    break;
                case "pattern":
                    ((CatalogResource.CatalogResource.TC01_String)properties[propertyName]).Data = newValue;
                    break;
                case "string":
                    try
                    {
                        var commaSeparatedValues = ParseCommaSeparatedValues(newValue);
                        var complateElement = ((CatalogResource.CatalogResource.TC06_XYZ)properties[propertyName]);
                        complateElement.Unknown1 = commaSeparatedValues[0];
                        complateElement.Unknown2 = commaSeparatedValues[1];
                        complateElement.Unknown3 = commaSeparatedValues[2];
                    }
                    catch (System.InvalidCastException)
                    {
                        ((CatalogResource.CatalogResource.TC01_String)properties[propertyName]).Data = newValue;
                    }
                    break;
                case "texture":
                    var index = material.MaterialBlock.ParentTGIBlocks.FindIndex(x => x.ReverseEvaluateResourceKey() == newValue);
                    if (index == -1)
                    {
                        var evaluated = material.ParentPackage.EvaluateImageResourceKey(newValue);
                        ((CatalogResource.CatalogResource.TC03_TGIIndex)properties[propertyName]).TGIIndex = (byte)material.MaterialBlock.ParentTGIBlocks.Count;
                        material.MaterialBlock.ParentTGIBlocks.Add(new s3pi.Interfaces.TGIBlock(0, null, evaluated.ResourceIndexEntry.ResourceType, evaluated.ResourceIndexEntry.ResourceGroup, evaluated.ResourceIndexEntry.Instance));
                        break;
                    }
                    ((CatalogResource.CatalogResource.TC03_TGIIndex)properties[propertyName]).TGIIndex = (byte)index;
                    break;
                case "vec2":
                    var coordinates = ParseCommaSeparatedValues(newValue);
                    ((CatalogResource.CatalogResource.TC05_XY)properties[propertyName]).Unknown1 = coordinates[0];
                    ((CatalogResource.CatalogResource.TC05_XY)properties[propertyName]).Unknown2 = coordinates[1];
                    break;
            }
            if (beforeMarkUnsaved != null)
            {
                beforeMarkUnsaved();
            }
            MarkUnsavedChangesCallback();
        }

        public override void SetValue(string propertyName, string newValue, CmarNYCBorrowed.Action beforeMarkUnsaved = null)
        {
            mInternal.SetValue(propertyName, newValue, beforeMarkUnsaved ?? RegenerateTexture);
        }
    }
}
