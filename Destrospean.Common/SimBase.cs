using System.Collections.Generic;
using System.Drawing;
using Destrospean.CmarNYCBorrowed;
using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;

namespace Destrospean.Common
{
    public abstract class SimBase
    {
        readonly Dictionary<CASPartResource.ClothingType, CASPart> mCASParts = new Dictionary<CASPartResource.ClothingType, CASPart>();

        string mID;

        Bitmap mStackedFaceTexture, mStackedScalpTexture;

        public Dictionary<CASPartResource.ClothingType, CASPart> CASParts
        {
            get
            {
                var casParts = new Dictionary<CASPartResource.ClothingType, CASPart>();
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
        Special = 0,
        Thin = 0;

        public string ID
        {
            get
            {
                if (mID == null)
                {
                    mID = System.Guid.NewGuid().ToString();
                }
                return mID;
            }
        }

        public delegate void LoadMeshOnMainThreadDelegate(object volume, Preset currentPreset, Bitmap presetTexture, object material, LoadTextureDelegate loadTextureCallback);

        public delegate int LoadTextureDelegate(string key, Bitmap image);

        public static object Lock = new object();

        public bool ShowMaternityPartsOnly = false;

        public float[] SkinColor =
            {
                140f / byte.MaxValue,
                100f / byte.MaxValue,
                80f / byte.MaxValue
            };

        public SimBase()
        {
            foreach (CASPartResource.ClothingType clothingType in System.Enum.GetValues(typeof(CASPartResource.ClothingType)))
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
            return a.CASPartResource.Clothing == b.CASPartResource.Clothing || a.CASPartResource.Clothing == CASPartResource.ClothingType.Body && (b.CASPartResource.Clothing == CASPartResource.ClothingType.Bottom || b.CASPartResource.Clothing == CASPartResource.ClothingType.Top) || (a.CASPartResource.Clothing == CASPartResource.ClothingType.Bottom || a.CASPartResource.Clothing == CASPartResource.ClothingType.Top) && b.CASPartResource.Clothing == CASPartResource.ClothingType.Body;
        }

        public static bool CASPartsConflict(CASPart a, Dictionary<string, uint> b)
        {
            return (uint)a.CASPartResource.Clothing == b["Clothing"] || a.CASPartResource.Clothing == CASPartResource.ClothingType.Body && (b["Clothing"] == (uint)CASPartResource.ClothingType.Bottom || b["Clothing"] == (uint)CASPartResource.ClothingType.Top) || (a.CASPartResource.Clothing == CASPartResource.ClothingType.Bottom || a.CASPartResource.Clothing == CASPartResource.ClothingType.Top) && b["Clothing"] == (uint)CASPartResource.ClothingType.Body;
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

        public Bitmap GetStackedFaceTexture(int presetIndex)
        {
            lock (Lock)
            {
                if (mStackedFaceTexture != null)
                {
                    mStackedFaceTexture.Dispose();
                }
                mStackedFaceTexture = new Bitmap(1024, 1024);
                using (var graphics = Graphics.FromImage(mStackedFaceTexture))
                {
                    foreach (var casPart in CASParts.Values)
                    {
                        if (casPart == null)
                        {
                            continue;
                        }
                        var preset = (CASPartPreset)casPart.AllPresets[casPart == CurrentCASPart ? presetIndex : 0];
                        if (casPart.CASPartResource.DataType == CASPartResource.DataTypeFlags.FaceOverlay)
                        {
                            graphics.DrawImage(preset.Texture, 0, 0);
                        }
                        if (preset.FaceTexture != null)
                        {
                            graphics.DrawImage(preset.FaceTexture, 0, 0);
                        }
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
                mStackedScalpTexture = new Bitmap(1024, 1024);
                using (var graphics = Graphics.FromImage(mStackedScalpTexture))
                {
                    foreach (var casPart in CASParts.Values)
                    {
                        if (casPart == null)
                        {
                            continue;
                        }
                        var preset = (CASPartPreset)casPart.AllPresets[casPart == CurrentCASPart ? presetIndex : 0];
                        if (casPart.CASPartResource.DataType == CASPartResource.DataTypeFlags.Scalp)
                        {
                            graphics.DrawImage(preset.Texture, 0, 0);
                        }
                        if (preset.ScalpTexture != null)
                        {
                            graphics.DrawImage(preset.ScalpTexture, 0, 0);
                        }
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
                foreach (CASPartResource.ClothingType clothingType in System.Enum.GetValues(typeof(CASPartResource.ClothingType)))
                {
                    if (mCASParts[clothingType] != null)
                    {
                        mCASParts[clothingType].Dispose();
                        mCASParts[clothingType] = null;
                    }
                }
                var random = new System.Random();
                foreach (CASPartResource.ClothingType clothingType in System.Enum.GetValues(typeof(CASPartResource.ClothingType)))
                {
                    var validCurrentTypePartKeys = new List<string>();
                    foreach (var casPartLookupKvp in CASPart.CASPartLookupCache)
                    {
                        if (((!ShowMaternityPartsOnly || casPartLookupKvp.Value["Clothing"] < (uint)CASPartResource.ClothingType.Body || casPartLookupKvp.Value["Clothing"] > (uint)CASPartResource.ClothingType.Bottom || (casPartLookupKvp.Value["ClothingCategory"] & (uint)CASPartResource.ClothingCategoryFlags.ValidForMaternity) != 0) && (casPartLookupKvp.Value["Age"] & (uint)CurrentCASPart.CASPartResource.AgeGender.Age) != 0 && casPartLookupKvp.Value["Clothing"] == (uint)clothingType && (casPartLookupKvp.Value["ClothingCategory"] & (uint.MaxValue - (uint)CASPartResource.ClothingCategoryFlags.ValidForMaternity - (uint)CASPartResource.ClothingCategoryFlags.ValidForRandom) & (uint)CurrentCASPart.CASPartResource.ClothingCategory) != 0 && (casPartLookupKvp.Value["ClothingCategory"] & (uint)CASPartResource.ClothingCategoryFlags.ValidForRandom) != 0 && (casPartLookupKvp.Value["Gender"] & (uint)CurrentCASPart.CASPartResource.AgeGender.Gender) != 0 && CASPart.GetAdjustedSpecies((CASPartResource.SpeciesType)casPartLookupKvp.Value["Species"]) == CASPart.GetAdjustedSpecies(CurrentCASPart.CASPartResource.AgeGender.Species)) && !CASPartsConflict(CurrentCASPart, casPartLookupKvp.Value))
                        {
                            var isValid = true;
                            foreach (var casPart in mCASParts.Values)
                            {
                                if (casPart == null)
                                {
                                    continue;
                                }
                                if (CASPartsConflict(casPart, casPartLookupKvp.Value) || (casPartLookupKvp.Value["Age"] & (uint)casPart.CASPartResource.AgeGender.Age) == 0 || (casPartLookupKvp.Value["ClothingCategory"] & (uint.MaxValue - (uint)CASPartResource.ClothingCategoryFlags.ValidForMaternity - (uint)CASPartResource.ClothingCategoryFlags.ValidForRandom) & (uint)casPart.CASPartResource.ClothingCategory) == 0 || (casPartLookupKvp.Value["Gender"] & (uint)casPart.CASPartResource.AgeGender.Gender) == 0 || CASPart.GetAdjustedSpecies((CASPartResource.SpeciesType)casPartLookupKvp.Value["Species"]) != CASPart.GetAdjustedSpecies(casPart.CASPartResource.AgeGender.Species))
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
                            case CASPartResource.ClothingType.Body:
                            case CASPartResource.ClothingType.Bottom:
                            case CASPartResource.ClothingType.Dental:
                            case CASPartResource.ClothingType.Earrings:
                            case CASPartResource.ClothingType.Eyebrow:
                            case CASPartResource.ClothingType.EyeColor:
                            case CASPartResource.ClothingType.Face:
                            case CASPartResource.ClothingType.Hair:
                            case CASPartResource.ClothingType.Scalp:
                            case CASPartResource.ClothingType.Shoes:
                            case CASPartResource.ClothingType.Top:
                                break;
                            default:
                                continue;
                        }
                        SetCASPart(clothingType, validCurrentTypePartKeys[random.Next(0, validCurrentTypePartKeys.Count - 1)]);
                    }
                }
                switch (random.Next(0, 2))
                {
                    case 0:
                        while (true)
                        {
                            SkinColor = new float[]
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
                            SkinColor = new float[]
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
                            SkinColor = new float[]
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
        }

        public void SetCASPart(CASPartResource.ClothingType clothingType, string key)
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
            if (PreloadedData.CASParts.ContainsKey(key))
            {
                mCASParts[clothingType] = PreloadedData.CASParts[key];
                return;
            }
            var evaluated = package.EvaluateResourceKey(key);
            (mCASParts[clothingType] = new CASPart(evaluated.Package, evaluated.ResourceIndexEntry, new Dictionary<string, GEOM>(), new Dictionary<string, s3pi.GenericRCOLResource.GenericRCOLResource>())).AllPresets[0].RegenerateTexture();

        }

        public void SetCASPart(CASPartResource.ClothingType clothingType, uint type, uint group, ulong instance)
        {
            SetCASPart(clothingType, new ResourceKey(type, group, instance).ReverseEvaluateResourceKey());
        }
    }
}
