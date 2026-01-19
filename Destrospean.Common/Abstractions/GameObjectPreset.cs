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
    public class GameObjectPreset : Preset
    {
        public CatalogResource.CatalogResource.MaterialBlock MaterialBlock
        {
            get
            {
                return ((PresetInternal)mInternal).MaterialBlock;
            }
        }

        protected class PresetInternal : PresetInternalBase
        {
            protected readonly IDictionary<string, object> mProperties = new SortedDictionary<string, object>(new PropertyNameComparer());

            public CatalogResource.CatalogResource.MaterialBlock MaterialBlock;

            public override Bitmap NewTexture
            {
                get
                {
                    uint[] maskArray = null;
                    Bitmap multiplier = null,
                    overlay = null;
                    int height = 1024,
                    width = 1024;
                    var stencils = new List<Bitmap>();
                    var stencilsEnabled = new List<bool>();
                    var stencilsRotation = new List<float>();
                    foreach (var propertyTypedKvp in PropertiesTyped)
                    {
                        var key = propertyTypedKvp.Key.ToLowerInvariant();
                        var value = Properties.ContainsKey(propertyTypedKvp.Key) ? Properties[propertyTypedKvp.Key] : GameObjectPreset.CreateComplateOverrideInstance(propertyTypedKvp.Key, propertyTypedKvp.Value.DefaultValue, propertyTypedKvp.Value.Type, MaterialBlock, ParentPackage);
                        if (key.StartsWith("stencil"))
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
                    }
                    return texture;
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

            public PresetInternal(GameObjectPreset preset, CatalogResource.CatalogResource.MaterialBlock materialBlock) : base()
            {
                Preset = preset;
                MaterialBlock = materialBlock;
                var evaluated = ParentPackage.EvaluateResourceKey(MaterialBlock.ParentTGIBlocks[MaterialBlock.ComplateXMLIndex].ReverseEvaluateResourceKey());
                mXmlDocument.LoadXml(new StreamReader(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)).ReadToEnd());
                Patterns = new List<Pattern>();
                foreach (var patternMaterialBlock in MaterialBlock.MaterialBlocks)
                {
                    Patterns.Add(new Pattern(preset, patternMaterialBlock, MaterialBlock));
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
                                var key = grandchildNode.Attributes["name"].Value;
                                PropertiesTyped.Add(key, new PropertyMeta(grandchildNode.Attributes["type"].Value, grandchildNode.Attributes["default"].Value));
                                if (!Properties.ContainsKey(key))
                                {
                                    Properties.Add(key, CreateComplateOverrideInstance(key, PropertiesTyped[key].DefaultValue, PropertiesTyped[key].Type, MaterialBlock, ParentPackage));
                                }
                            }
                        }
                    }
                }
            }

            public override string GetValue(string propertyName)
            {
                return GameObjectPreset.GetValue((GameObjectPreset)Preset, propertyName, PropertiesTyped[propertyName].Type, Properties);
            }

            public override void ReplacePresetComplate()
            {
                ReplacePresetComplate(ParentPackage.EvaluateResourceKey(MaterialBlock.ParentTGIBlocks[MaterialBlock.ComplateXMLIndex].ReverseEvaluateResourceKey()));
            }

            public override void SetValue(string propertyName, string newValue, Action beforeMarkUnsaved = null)
            {
                GameObjectPreset.SetValue((GameObjectPreset)Preset, MaterialBlock, propertyName, newValue, PropertiesTyped[propertyName].Type, Properties, beforeMarkUnsaved);
            }
        }

        public GameObjectPreset(CASTableObject castableObject, CatalogResource.CatalogResource.MaterialBlock materialBlock)
        {
            mCASTableObject = castableObject;
            mInternal = new PresetInternal(this, materialBlock);
        }

        public override void AddPattern(string patternSlotName, string newComplateName)
        {
            MaterialBlock.Name = newComplateName;
            MaterialBlock.ParentTGIBlocks[MaterialBlock.ComplateXMLIndex] = new TGIBlock(0, null, ResourceUtils.GetResourceType("_XML"), 0, System.Security.Cryptography.FNV64.GetHash(newComplateName));
            var lastPatternSlotName = Patterns[Patterns.Count - 1].SlotName;
            foreach (var complateOverride in new List<CatalogResource.CatalogResource.ComplateElement>(MaterialBlock.ComplateOverrides))
            {
                if (complateOverride.VariableName.StartsWith(lastPatternSlotName))
                {
                    var clonedComplateOverride = (CatalogResource.CatalogResource.ComplateElement)complateOverride.Clone((sender, e) =>
                        {
                        });
                    clonedComplateOverride.VariableName = clonedComplateOverride.VariableName.Replace(lastPatternSlotName, patternSlotName);
                    MaterialBlock.ComplateOverrides.Add(clonedComplateOverride);
                    ((PresetInternal)mInternal).Properties.Add(clonedComplateOverride.VariableName, clonedComplateOverride);
                }
            }
            foreach (var materialBlock in new List<CatalogResource.CatalogResource.MaterialBlock>(MaterialBlock.MaterialBlocks))
            {
                if (materialBlock.Pattern == lastPatternSlotName)
                {
                    var clonedMaterialBlock = (CatalogResource.CatalogResource.MaterialBlock)materialBlock.Clone((sender, e) =>
                        {
                        });
                    clonedMaterialBlock.Pattern = patternSlotName;
                    MaterialBlock.MaterialBlocks.Add(clonedMaterialBlock);
                    Patterns.Add(new Pattern(this, clonedMaterialBlock, MaterialBlock));
                }
            }
            mInternal.ReplacePresetComplate();
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
                    return new CatalogResource.CatalogResource.TC04_Single(0, null, 0, name, float.Parse(value, System.Globalization.CultureInfo.InvariantCulture));
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

        public static string GetValue(GameObjectPreset preset, string propertyName, string type, IDictionary<string, object> properties)
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
                    return preset.MaterialBlock.ParentTGIBlocks[((CatalogResource.CatalogResource.TC03_TGIIndex)properties[propertyName]).TGIIndex].ReverseEvaluateResourceKey();
                case "vec2":
                    return ((CatalogResource.CatalogResource.TC05_XY)properties[propertyName]).Unknown1.ToString() + "," + ((CatalogResource.CatalogResource.TC05_XY)properties[propertyName]).Unknown2.ToString();
                default:
                    return null;
            }
        }

        public override void ReplacePattern(string patternSlotName, string patternKey)
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

        public static void SetValue(GameObjectPreset preset, CatalogResource.CatalogResource.MaterialBlock materialBlock, string propertyName, string newValue, string type, IDictionary<string, object> properties, CmarNYCBorrowed.Action beforeMarkUnsaved = null)
        {
            if (!materialBlock.ComplateOverrides.Exists(x => x.VariableName == propertyName))
            {
                materialBlock.ComplateOverrides.Add((CatalogResource.CatalogResource.ComplateElement)properties[propertyName]);
            }
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
                        ((CatalogResource.CatalogResource.TC04_Single)properties[propertyName]).Unknown1 = float.Parse(newValue, System.Globalization.CultureInfo.InvariantCulture);
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
                    var index = preset.MaterialBlock.ParentTGIBlocks.FindIndex(x => x.ReverseEvaluateResourceKey() == newValue);
                    if (index == -1)
                    {
                        var evaluated = preset.ParentPackage.EvaluateImageResourceKey(newValue);
                        ((CatalogResource.CatalogResource.TC03_TGIIndex)properties[propertyName]).TGIIndex = (byte)preset.MaterialBlock.ParentTGIBlocks.Count;
                        preset.MaterialBlock.ParentTGIBlocks.Add(new s3pi.Interfaces.TGIBlock(0, null, evaluated.ResourceIndexEntry.ResourceType, evaluated.ResourceIndexEntry.ResourceGroup, evaluated.ResourceIndexEntry.Instance));
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
    }
}
