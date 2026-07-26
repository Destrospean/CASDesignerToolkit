using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CASPartResource;
using Destrospean.CmarNYCBorrowed;
using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;

namespace Destrospean.Common
{
    public abstract class SimBase
    {
        readonly Dictionary<ClothingType, CASPart> mCASParts = new Dictionary<ClothingType, CASPart>();

        string mID;

        List<STPR> mSkinTonePresets;

        Bitmap mStackedBodyTexture, mStackedFaceTexture, mStackedScalpTexture;

        public readonly Dictionary<ClothingType, Dictionary<string, string>> CASPartOverrides = new Dictionary<ClothingType, Dictionary<string, string>>();

        public readonly List<ClothingType> CASPartOverridesDisabled = new List<ClothingType>();

        public Dictionary<ClothingType, CASPart> CASParts
        {
            get
            {
                var casParts = new Dictionary<ClothingType, CASPart>();
                foreach (var casPartKvp in mCASParts)
                {
                    casParts.Add(casPartKvp.Key, CurrentCASPart != null && casPartKvp.Key == CurrentCASPart.CASPartResource.Clothing ? CurrentCASPart : casPartKvp.Value);
                }
                return casParts;
            }
        }

        public CASPart CurrentCASPart = null;

        public Rig CurrentRig
        {
            get
            {
                return CurrentCASPart.CurrentRig;
            }
        }

        public float Fat = 0,
        Fit = 0,
        SkinDarkness = 0,
        Special = 0,
        Thin = 0;

        public string ID
        {
            get
            {
                if (mID == null)
                {
                    mID = Guid.NewGuid().ToString();
                }
                return mID;
            }
        }

        public delegate void LoadMeshOnMainThreadDelegate(object volume, Preset currentPreset, Bitmap presetTexture, Bitmap[] ambientAndSpecularMapTextures, object material, LoadTextureDelegate loadTextureCallback);

        public delegate int LoadTextureDelegate(string key, Bitmap image);

        public static object Lock = new object();

        public bool OverrideSkinColor = false,
        ShowMaternityPartsOnly = false;

        public delegate bool PresetXmlElementPredicate(CASPartPreset preset, System.Xml.XmlElement xmlElement);

        public float[] SkinColor =
            {
                140f / byte.MaxValue,
                100f / byte.MaxValue,
                80f / byte.MaxValue
            };

        public STPR SkinTonePreset;

        public List<STPR> SkinTonePresets
        {
            get
            {
                if (mSkinTonePresets == null)
                {
                    mSkinTonePresets = new List<STPR>();
                    foreach (var gamePackageKvp in ResourceUtils.GameContentPackages)
                    {
                        var resourceType = ResourceUtils.GetResourceType("STPR");
                        foreach (var resourceIndexEntry in gamePackageKvp.Value.FindAll(x => x.ResourceType == resourceType))
                        {
                            mSkinTonePresets.Add(new STPR(new BinaryReader(((s3pi.Interfaces.APackage)gamePackageKvp.Value).GetResource(resourceIndexEntry))));
                        }
                    }
                }
                return mSkinTonePresets;
            }
        }

        public SimBase()
        {
            foreach (ClothingType clothingType in Enum.GetValues(typeof(ClothingType)))
            {
                mCASParts[clothingType] = null;
            }
        }

        protected abstract void LoadMeshes(CASPart casPart, int presetIndex, int lodIndex, LoadTextureDelegate loadTextureCallback, LoadMeshOnMainThreadDelegate loadMeshOnMainThreadCallback);

        public static bool CASPartsConflict(CASPart a, CASPart b)
        {
            if (a == null || b == null || a == b)
            {
                return false;
            }
            return a.CASPartResource.Clothing == b.CASPartResource.Clothing || a.CASPartResource.Clothing == ClothingType.Body && (b.CASPartResource.Clothing == ClothingType.Bottom || b.CASPartResource.Clothing == ClothingType.Top) || (a.CASPartResource.Clothing == ClothingType.Bottom || a.CASPartResource.Clothing == ClothingType.Top) && b.CASPartResource.Clothing == ClothingType.Body;
        }

        public static bool CASPartsConflict(CASPart a, Dictionary<string, string> b)
        {
            var bClothing = (ClothingType)Enum.Parse(typeof(ClothingType), b["Clothing"]);
            return a.CASPartResource.Clothing == bClothing || a.CASPartResource.Clothing == ClothingType.Body && (bClothing == ClothingType.Bottom || bClothing == ClothingType.Top) || (a.CASPartResource.Clothing == ClothingType.Bottom || a.CASPartResource.Clothing == ClothingType.Top) && bClothing == ClothingType.Body;
        }

        public static List<float[]> FillMissingDeltas(IEnumerable<float[]> vertices, IEnumerable<float[]> deltas)
        {
            var correctCount = new List<float[]>(vertices).Count;
            var newDeltas = new List<float[]>(deltas);
            if (newDeltas.Count > correctCount)
            {
                newDeltas.Clear();
                newDeltas.AddRange(new List<float[]>(deltas).GetRange(0, correctCount));
            }
            while (newDeltas.Count < correctCount)
            {
                newDeltas.Add(new float[]
                    {
                        0,
                        0,
                        0
                    });
            }
            return newDeltas;
        }

        public CASPart GetCASPart(ClothingType clothingType, string key)
        {
            s3pi.Interfaces.IPackage package;
            if (CurrentCASPart == null)
            {
                package = s3pi.Package.Package.NewPackage(0);
            }
            else
            {
                package = CurrentCASPart.ParentPackage;
            }
            var evaluated = package.EvaluateResourceKey(key);
            return new CASPart(evaluated.Package, evaluated.ResourceIndexEntry, new Dictionary<string, GEOM>(), new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>());
        }

        public List<CASPartPreset> GetCASPartPresetsWithXmlElement(int presetIndex, PresetXmlElementPredicate predicate)
        {
            var casPartsAndPresets = new List<Tuple<CASPart, CASPartPreset>>();
            foreach (var casPart in CASParts.Values)
            {
                if (casPart == null)
                {
                    continue;
                }
                var preset = (CASPartPreset)casPart.AllPresets[casPart == CurrentCASPart ? presetIndex : casPart.AllPresets.Count > 1 ? 1 : 0];
                var xmlDocument = new System.Xml.XmlDocument();
                xmlDocument.Load(preset.XmlFile);
                foreach (System.Xml.XmlElement element in xmlDocument.SelectSingleNode("preset").SelectSingleNode("complate").ChildNodes)
                {
                    if (predicate(preset, element))
                    {
                        casPartsAndPresets.Add(new Tuple<CASPart, CASPartPreset>(casPart, preset));
                        break;
                    }
                }
            }
            casPartsAndPresets.Sort((a, b) => a.Item1.CASPartResource.OverlayPriority.CompareTo(b.Item1.CASPartResource.OverlayPriority));
            return casPartsAndPresets.ConvertAll(x => x.Item2);
        }

        public Bitmap GetStackedBodyTexture(int presetIndex)
        {
            lock (Lock)
            {
                if (mStackedBodyTexture != null)
                {
                    mStackedBodyTexture.Dispose();
                }
                using (var graphics = Graphics.FromImage(mStackedBodyTexture = new Bitmap(1024, 1024)))
                {
                    var casPart = CASParts[ClothingType.Body] ?? CASParts[ClothingType.Bottom] ?? CASParts[ClothingType.Top];
                    graphics.DrawImage(CurrentCASPart.ParentPackage.GetSkinToneImage(new Tone(new BinaryReader(CurrentCASPart.ParentPackage.EvaluateResourceKey(new ResourceKey(SkinTonePreset.SkinToneKey.Type, SkinTonePreset.SkinToneKey.Group, SkinTonePreset.SkinToneKey.Instance).ReverseEvaluateResourceKey()).Stream)), (AgeGender)casPart.CASPartResource.AgeGender.Age, (AgeGender)((uint)casPart.CASPartResource.AgeGender.Gender << 12), PartType.Body, null, SkinDarkness, 0, 0, Complate.GetTextureCallback), 0, 0);
                    foreach (var preset in GetCASPartPresetsWithXmlElement(presetIndex, (preset, element) => element.Name.ToLowerInvariant() == "value" && (element.GetAttribute("key") ?? "").ToLowerInvariant() == "parttype" && (element.GetAttribute("value") ?? "").ToLowerInvariant() == "body"))
                    {
                        graphics.DrawImage(preset.Texture, 0, 0);
                    }
                }
                return mStackedBodyTexture;
            }
        }

        public Bitmap GetStackedFaceTexture(int presetIndex)
        {
            lock (Lock)
            {
                if (mStackedFaceTexture != null)
                {
                    mStackedFaceTexture.Dispose();
                }
                using (var graphics = Graphics.FromImage(mStackedFaceTexture = new Bitmap(1024, 1024)))
                {
                    graphics.DrawImage(CurrentCASPart.ParentPackage.GetSkinToneImage(new Tone(new BinaryReader(CurrentCASPart.ParentPackage.EvaluateResourceKey(new ResourceKey(SkinTonePreset.SkinToneKey.Type, SkinTonePreset.SkinToneKey.Group, SkinTonePreset.SkinToneKey.Instance).ReverseEvaluateResourceKey()).Stream)), (AgeGender)CASParts[ClothingType.Face].CASPartResource.AgeGender.Age, (AgeGender)((uint)CASParts[ClothingType.Face].CASPartResource.AgeGender.Gender << 12), PartType.Face, null, SkinDarkness, 0, 0, Complate.GetTextureCallback), 0, 0);
                    bool drawsOnFace;
                    foreach (var preset in GetCASPartPresetsWithXmlElement(0, (preset, element) => element.Name.ToLowerInvariant() == "value" && (element.GetAttribute("key") ?? "").ToLowerInvariant() == "parttype" && ((element.GetAttribute("value") ?? "").ToLowerInvariant() == "face" || (element.GetAttribute("value") ?? "").ToLowerInvariant() == "hair" && bool.TryParse(preset["DrawsOnFace"], out drawsOnFace) && drawsOnFace)))
                    {
                        graphics.DrawImage(preset.FaceTexture ?? preset.Texture ?? new Bitmap(1024, 1024), 0, 0);
                    }
                }
                return mStackedFaceTexture;
            }
        }

        public Bitmap GetStackedScalpTexture(int presetIndex)
        {
            lock (Lock)
            {
                if (mStackedScalpTexture != null)
                {
                    mStackedScalpTexture.Dispose();
                }
                using (var graphics = Graphics.FromImage(mStackedScalpTexture = new Bitmap(1024, 1024)))
                {
                    graphics.DrawImage(CurrentCASPart.ParentPackage.GetSkinToneImage(new Tone(new BinaryReader(CurrentCASPart.ParentPackage.EvaluateResourceKey(new ResourceKey(SkinTonePreset.SkinToneKey.Type, SkinTonePreset.SkinToneKey.Group, SkinTonePreset.SkinToneKey.Instance).ReverseEvaluateResourceKey()).Stream)), (AgeGender)CASParts[ClothingType.Scalp].CASPartResource.AgeGender.Age, (AgeGender)((uint)CASParts[ClothingType.Scalp].CASPartResource.AgeGender.Gender << 12), PartType.Scalp, null, SkinDarkness, 0, 0, Complate.GetTextureCallback), 0, 0);
                    bool drawsOnScalp;
                    foreach (var preset in GetCASPartPresetsWithXmlElement(0, (preset, element) => element.Name.ToLowerInvariant() == "value" && (element.GetAttribute("key") ?? "").ToLowerInvariant() == "parttype" && ((element.GetAttribute("value") ?? "").ToLowerInvariant() == "scalp" || (element.GetAttribute("value") ?? "").ToLowerInvariant() == "hair" && bool.TryParse(preset["DrawsOnScalp"], out drawsOnScalp) && drawsOnScalp)))
                    {
                        graphics.DrawImage(preset.ScalpTexture ?? preset.Texture ?? new Bitmap(1024, 1024), 0, 0);
                    }
                }
                return mStackedScalpTexture;
            }
        }

        public virtual void LoadMeshes(int presetIndex, int lodIndex, LoadTextureDelegate loadTextureCallback, LoadMeshOnMainThreadDelegate loadMeshOnMainThreadCallback)
        {
            lock (Lock)
            {
                new List<CASPart>(CASParts.Values).FindAll(x => x != null && !CASPartsConflict(x, CurrentCASPart)).ForEach(x => LoadMeshes(x, presetIndex, lodIndex, loadTextureCallback, loadMeshOnMainThreadCallback));
            }
        }

        public void RandomizeCASParts()
        {
            lock (Lock)
            {
                foreach (ClothingType clothingType in Enum.GetValues(typeof(ClothingType)))
                {
                    if (mCASParts[clothingType] != null)
                    {
                        mCASParts[clothingType].Dispose();
                        mCASParts[clothingType] = null;
                    }
                }
                foreach (var casPartOverrideKvp in CASPartOverrides)
                {
                    if (CASPartOverridesDisabled.Contains(casPartOverrideKvp.Key))
                    {
                        continue;
                    }
                    var evaluated = (CurrentCASPart?.ParentPackage ?? s3pi.Package.Package.NewPackage(0)).EvaluateResourceKey(casPartOverrideKvp.Value["ResourceKey"]);
                    var casPartOverride = new CASPart(evaluated.Package, evaluated.ResourceIndexEntry, new Dictionary<string, GEOM>(), new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>());
                    var isValid = true;
                    foreach (var casPart in mCASParts.Values)
                    {
                        if (casPart == null)
                        {
                            continue;
                        }
                        if (CASPartsConflict(casPart, casPartOverride) || (casPartOverride.CASPartResource.AgeGender.Age & casPart.CASPartResource.AgeGender.Age) == 0 || ((uint)casPartOverride.CASPartResource.ClothingCategory & (uint.MaxValue - (uint)ClothingCategoryFlags.ValidForMaternity - (uint)ClothingCategoryFlags.ValidForRandom) & (uint)casPart.CASPartResource.ClothingCategory) == 0 || (casPartOverride.CASPartResource.AgeGender.Gender & casPart.CASPartResource.AgeGender.Gender) == 0 || CASPart.GetAdjustedSpecies(casPartOverride.CASPartResource.AgeGender.Species) != CASPart.GetAdjustedSpecies(casPart.CASPartResource.AgeGender.Species))
                        {
                            isValid = false;
                            break;
                        }
                    }
                    if (isValid)
                    {
                        casPartOverrideKvp.Value["Age"] = casPartOverride.CASPartResource.AgeGender.Age.ToString();
                        casPartOverrideKvp.Value["Clothing"] = casPartOverride.CASPartResource.Clothing.ToString();
                        casPartOverrideKvp.Value["Gender"] = casPartOverride.CASPartResource.AgeGender.Gender.ToString();
                        casPartOverrideKvp.Value["Species"] = casPartOverride.CASPartResource.AgeGender.Species.ToString();
                        casPartOverrideKvp.Value["Unknown1"] = casPartOverride.CASPartResource.Unknown1;
                        mCASParts[casPartOverrideKvp.Key] = casPartOverride;
                    }
                }
                var random = new Random();
                foreach (ClothingType clothingType in Enum.GetValues(typeof(ClothingType)))
                {
                    var validCurrentTypePartKeys = new List<string>();
                    foreach (var casPartLookupKvp in CASPart.CASPartLookupCache)
                    {
                        var age = (AgeFlags)Enum.Parse(typeof(AgeFlags), casPartLookupKvp.Value["Age"]);
                        var category = (ClothingCategoryFlags)Enum.Parse(typeof(ClothingCategoryFlags), casPartLookupKvp.Value["ClothingCategory"]);
                        var clothing = (ClothingType)Enum.Parse(typeof(ClothingType), casPartLookupKvp.Value["Clothing"]);
                        var gender = (GenderFlags)Enum.Parse(typeof(GenderFlags), casPartLookupKvp.Value["Gender"]);
                        var species = (SpeciesType)Enum.Parse(typeof(SpeciesType), casPartLookupKvp.Value["Species"]);
                        if (((!ShowMaternityPartsOnly || clothing < ClothingType.Body || clothing > ClothingType.Bottom || (category & ClothingCategoryFlags.ValidForMaternity) != 0) && (age & CurrentCASPart.CASPartResource.AgeGender.Age) != 0 && clothing == clothingType && ((uint)category & (uint.MaxValue - (uint)ClothingCategoryFlags.ValidForMaternity - (uint)ClothingCategoryFlags.ValidForRandom) & (uint)CurrentCASPart.CASPartResource.ClothingCategory) != 0 && ((uint)category & (uint)ClothingCategoryFlags.ValidForRandom) != 0 && (gender & CurrentCASPart.CASPartResource.AgeGender.Gender) != 0 && CASPart.GetAdjustedSpecies(species) == CASPart.GetAdjustedSpecies(CurrentCASPart.CASPartResource.AgeGender.Species)) && !CASPartsConflict(CurrentCASPart, casPartLookupKvp.Value))
                        {
                            var isValid = true;
                            foreach (var casPart in mCASParts.Values)
                            {
                                if (casPart == null)
                                {
                                    continue;
                                }
                                if (CASPartsConflict(casPart, casPartLookupKvp.Value) || (age & casPart.CASPartResource.AgeGender.Age) == 0 || ((uint)category & (uint.MaxValue - (uint)ClothingCategoryFlags.ValidForMaternity - (uint)ClothingCategoryFlags.ValidForRandom) & (uint)casPart.CASPartResource.ClothingCategory) == 0 || (gender & casPart.CASPartResource.AgeGender.Gender) == 0 || CASPart.GetAdjustedSpecies(species) != CASPart.GetAdjustedSpecies(casPart.CASPartResource.AgeGender.Species))
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                            if (isValid)
                            {
                                validCurrentTypePartKeys.Add(casPartLookupKvp.Key);
                            }
                        }
                    }
                    if (validCurrentTypePartKeys.Count > 0)
                    {
                        switch (clothingType)
                        {
                            case ClothingType.Bottom:
                            case ClothingType.Eyebrow:
                            case ClothingType.EyeColor:
                            case ClothingType.Face:
                            case ClothingType.Hair:
                            case ClothingType.Scalp:
                            case ClothingType.Shoes:
                            case ClothingType.Top:
                                break;
                            default:
                                continue;
                        }
                        SetCASPart(clothingType, validCurrentTypePartKeys[random.Next(0, validCurrentTypePartKeys.Count - 1)]);
                    }
                }
                if (!OverrideSkinColor)
                {
                    RandomizeSkinColor();
                }
            }
        }

        public void RandomizeSkinColor()
        {
            var random = new Random();
            SkinDarkness = random.Next(101) / 100f;
            SkinTonePreset = SkinTonePresets[random.Next(SkinTonePresets.Count)];
            /*
            switch (random.Next(3))
            {
                case 0:
                    while (true)
                    {
                        SkinColor = new[]
                            {
                                (float)random.Next(240, 255) / byte.MaxValue,
                                (float)random.Next(200, 230) / byte.MaxValue,
                                (float)random.Next(160, 200) / byte.MaxValue
                            };
                        if (SkinColor[0] >= SkinColor[1] && SkinColor[1] >= SkinColor[2])
                        {
                            break;
                        }
                    }
                    break;
                case 1:
                    while (true)
                    {
                        SkinColor = new[]
                            {
                                (float)random.Next(180, 220) / byte.MaxValue,
                                (float)random.Next(130, 180) / byte.MaxValue,
                                (float)random.Next(90, 140) / byte.MaxValue
                            };
                        if (SkinColor[0] >= SkinColor[1] && SkinColor[1] >= SkinColor[2])
                        {
                            break;
                        }
                    }
                    break;
                case 2:
                    while (true)
                    {
                        SkinColor = new[]
                            {
                                (float)random.Next(50, 150) / byte.MaxValue,
                                (float)random.Next(20, 100) / byte.MaxValue,
                                (float)random.Next(10, 70) / byte.MaxValue
                            };
                        if (SkinColor[0] >= SkinColor[1] && SkinColor[1] >= SkinColor[2])
                        {
                            break;
                        }
                    }
                    break;
            }
            */
        }

        public void SetCASPart(ClothingType clothingType, string key)
        {
            var package = CurrentCASPart?.ParentPackage ?? s3pi.Package.Package.NewPackage(0);
            if (PreloadedData.CASParts.ContainsKey(key))
            {
                mCASParts[clothingType] = PreloadedData.CASParts[key];
                return;
            }
            var evaluated = package.EvaluateResourceKey(key);
            var presets = (mCASParts[clothingType] = new CASPart(evaluated.Package, evaluated.ResourceIndexEntry, new Dictionary<string, GEOM>(), new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>())).AllPresets;
            presets[presets.Count > 1 ? 1 : 0].RegenerateTexture();
        }

        public void SetCASPart(ClothingType clothingType, uint type, uint group, ulong instance)
        {
            SetCASPart(clothingType, new ResourceKey(type, group, instance).ReverseEvaluateResourceKey());
        }

        public void SetCASPartOverride(ClothingType clothingType, string key)
        {
            CASPartOverrides[clothingType] = new Dictionary<string, string>
            {
                {
                    "ResourceKey",
                    key
                }
            };
        }
    }
}
