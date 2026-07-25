using System;
using System.Collections.Generic;
using System.Drawing;
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

        Bitmap mStackedBodyTexture, mStackedFaceTexture, mStackedScalpTexture;

        public Bitmap BodyMultiplier
        {
            get
            {
                var agePrefix = "a";
                var bodyCASPart = CASParts[ClothingType.Body] ?? CASParts[ClothingType.Bottom] ?? CASParts[ClothingType.Top];
                if (bodyCASPart != null)
                {
                    switch (bodyCASPart.AdjustedAge)
                    {
                        case AgeGender.Baby:
                            agePrefix = "b";
                            break;
                        case AgeGender.Toddler:
                            agePrefix = "p";
                            break;
                        case AgeGender.Child:
                            agePrefix = "c";
                            break;
                        case AgeGender.Elder:
                            agePrefix = "e";
                            break;
                        default:
                            agePrefix = "a";
                            break;
                    }
                }
                return (Bitmap)CurrentCASPart.ParentPackage.GetTexture("key:00B2D882:00000000:" + System.Security.Cryptography.FNV64.GetHash(agePrefix + (bodyCASPart.AdjustedAge < AgeGender.Teen ? "u" : bodyCASPart.CASPartResource.AgeGender.Gender == GenderFlags.Male ? "m" : "f") + "Body_m").ToString("X16"), Complate.GetTextureCallback, 1024, 1024)?.Clone() ?? new Bitmap(1024, 1024);
            }
        }

        public readonly Dictionary<ClothingType, CASPart> CASPartOverrides = new Dictionary<ClothingType, CASPart>();

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

        public Bitmap FaceMultiplier
        {
            get
            {
                var agePrefix = "a";
                var faceCASPart = CASParts[ClothingType.Face];
                if (faceCASPart != null)
                {
                    switch (faceCASPart.CASPartResource.AgeGender.Age)
                    {
                        case AgeFlags.Toddler:
                            agePrefix = "p";
                            break;
                        case AgeFlags.Child:
                            agePrefix = "c";
                            break;
                        case AgeFlags.YoungAdult:
                            agePrefix = "y";
                            break;
                        case AgeFlags.Elder:
                            agePrefix = "e";
                            break;
                        default:
                            agePrefix = "a";
                            break;
                    }
                }
                return (Bitmap)CurrentCASPart.ParentPackage.GetTexture("key:00B2D882:00000000:" + System.Security.Cryptography.FNV64.GetHash(agePrefix + (faceCASPart.AdjustedAge < AgeGender.Teen ? "u" : faceCASPart.CASPartResource.AgeGender.Gender == GenderFlags.Male ? "m" : "f") + "Face_m").ToString("X16"), Complate.GetTextureCallback, 1024, 1024)?.Clone() ?? new Bitmap(1024, 1024);
            }
        }

        public float Fat = 0,
        Fit = 0,
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

        public Bitmap ScalpMultiplier
        {
            get
            {
                var agePrefix = "a";
                var scalpCASPart = CASParts[ClothingType.Scalp];
                if (scalpCASPart != null)
                {
                    switch (scalpCASPart.AdjustedAge)
                    {
                        case AgeGender.Toddler:
                            agePrefix = "p";
                            break;
                        case AgeGender.Child:
                            agePrefix = "c";
                            break;
                        case AgeGender.Elder:
                            agePrefix = "e";
                            break;
                        default:
                            agePrefix = "a";
                            break;
                    }
                }
                return (Bitmap)CurrentCASPart.ParentPackage.GetTexture("key:00B2D882:00000000:" + System.Security.Cryptography.FNV64.GetHash(agePrefix + (scalpCASPart.AdjustedAge < AgeGender.Teen ? "u" : scalpCASPart.CASPartResource.AgeGender.Gender == GenderFlags.Male ? "m" : "f") + "Scalp_m").ToString("X16"), Complate.GetTextureCallback, 1024, 1024)?.Clone() ?? new Bitmap(1024, 1024);
            }
        }

        public float[] SkinColor =
            {
                140f / byte.MaxValue,
                100f / byte.MaxValue,
                80f / byte.MaxValue
            };

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
                var preset = (CASPartPreset)casPart.AllPresets[casPart == CurrentCASPart ? presetIndex : 0];
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
                    foreach (var preset in GetCASPartPresetsWithXmlElement(0, (preset, element) => element.Name.ToLowerInvariant() == "value" && (element.GetAttribute("key") ?? "").ToLowerInvariant() == "parttype" && ((element.GetAttribute("value") ?? "").ToLowerInvariant() == "face" || (element.GetAttribute("value") ?? "").ToLowerInvariant() == "hair" && bool.Parse(preset["DrawsOnFace"] ?? "false"))))
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
                    foreach (var preset in GetCASPartPresetsWithXmlElement(0, (preset, element) => element.Name.ToLowerInvariant() == "value" && (element.GetAttribute("key") ?? "").ToLowerInvariant() == "parttype" && ((element.GetAttribute("value") ?? "").ToLowerInvariant() == "scalp" || (element.GetAttribute("value") ?? "").ToLowerInvariant() == "hair" && bool.Parse(preset["DrawsOnScalp"] ?? "false"))))
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
                    var isValid = true;
                    foreach (var casPart in mCASParts.Values)
                    {
                        if (casPart == null)
                        {
                            continue;
                        }
                        if (CASPartsConflict(casPart, casPartOverrideKvp.Value) || (casPartOverrideKvp.Value.CASPartResource.AgeGender.Age & casPart.CASPartResource.AgeGender.Age) == 0 || ((uint)casPartOverrideKvp.Value.CASPartResource.ClothingCategory & (uint.MaxValue - (uint)ClothingCategoryFlags.ValidForMaternity - (uint)ClothingCategoryFlags.ValidForRandom) & (uint)casPart.CASPartResource.ClothingCategory) == 0 || (casPartOverrideKvp.Value.CASPartResource.AgeGender.Gender & casPart.CASPartResource.AgeGender.Gender) == 0 || CASPart.GetAdjustedSpecies(casPartOverrideKvp.Value.CASPartResource.AgeGender.Species) != CASPart.GetAdjustedSpecies(casPart.CASPartResource.AgeGender.Species))
                        {
                            isValid = false;
                            break;
                        }
                    }
                    if (isValid)
                    {
                        mCASParts[casPartOverrideKvp.Key] = casPartOverrideKvp.Value;
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
            switch (random.Next(0, 2))
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
            (mCASParts[clothingType] = new CASPart(evaluated.Package, evaluated.ResourceIndexEntry, new Dictionary<string, GEOM>(), new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>())).AllPresets[0].RegenerateTexture();
        }

        public void SetCASPart(ClothingType clothingType, uint type, uint group, ulong instance)
        {
            SetCASPart(clothingType, new ResourceKey(type, group, instance).ReverseEvaluateResourceKey());
        }

        public void SetCASPartOverride(ClothingType clothingType, string key)
        {
            var package = CurrentCASPart?.ParentPackage ?? s3pi.Package.Package.NewPackage(0);
            if (PreloadedData.CASParts.ContainsKey(key))
            {
                CASPartOverrides[clothingType] = PreloadedData.CASParts[key];
                return;
            }
            var evaluated = package.EvaluateResourceKey(key);
            (CASPartOverrides[clothingType] = new CASPart(evaluated.Package, evaluated.ResourceIndexEntry, new Dictionary<string, GEOM>(), new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>())).AllPresets[0].RegenerateTexture();
        }
    }
}
