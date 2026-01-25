using System.Collections.Generic;
using Destrospean.CmarNYCBorrowed;
using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;

namespace Destrospean.Common
{
    public abstract class SimBase
    {
        readonly Dictionary<CASPartResource.ClothingType, CASPart> mCASParts = new Dictionary<CASPartResource.ClothingType, CASPart>();

        string mID;

        static object sLock = new object();

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

        public delegate int LoadTextureDelegate(string key, System.Drawing.Bitmap image);

        public readonly Dictionary<string, PreloadedLODMorphed> PreloadedLODsMorphed = new Dictionary<string, PreloadedLODMorphed>(System.StringComparer.InvariantCultureIgnoreCase);

        public struct PreloadedLODMorphed
        {
            public BBLN BBLN;

            public GEOM[] GEOMs;

            public PreloadedLODMorphed(BBLN bbln, GEOM[] geoms)
            {
                BBLN = bbln;
                GEOMs = geoms;
            }
        }

        public SimBase()
        {
            foreach (CASPartResource.ClothingType clothingType in System.Enum.GetValues(typeof(CASPartResource.ClothingType)))
            {
                mCASParts[clothingType] = null;
            }
        }

        protected abstract void LoadMeshes(CASPart casPart, int presetIndex, int lodIndex, LoadTextureDelegate loadTextureCallback);

        public static bool CASPartsConflict(CASPart a, CASPart b)
        {
            if (a == null || b == null || a == b)
            {
                return false;
            }
            if (a.CASPartResource.Clothing == b.CASPartResource.Clothing)
            {
                return true;
            }
            if (a.CASPartResource.Clothing == CASPartResource.ClothingType.Body && (b.CASPartResource.Clothing == CASPartResource.ClothingType.Bottom || b.CASPartResource.Clothing == CASPartResource.ClothingType.Top))
            {
                return true;
            }
            if ((a.CASPartResource.Clothing == CASPartResource.ClothingType.Bottom || a.CASPartResource.Clothing == CASPartResource.ClothingType.Top) && b.CASPartResource.Clothing == CASPartResource.ClothingType.Body)
            {
                return true;
            }
            return false;
        }

        public static List<float[]> FillMissingDeltas(IEnumerable<float[]> vertices, List<float[]> deltas)
        {
            var newDeltas = new List<float[]>(deltas);
            var correctCount = new List<float[]>(vertices).Count;
            if (newDeltas.Count > correctCount)
            {
                newDeltas.Clear();
                newDeltas.AddRange(deltas.GetRange(0, correctCount));
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

        public void LoadMeshes(int presetIndex, int lodIndex, LoadTextureDelegate loadTextureCallback)
        {
            lock (sLock)
            {
                new List<CASPart>(CASParts.Values).FindAll(x => x != null && !CASPartsConflict(x, CurrentCASPart)).ForEach(x => LoadMeshes(x, presetIndex, lodIndex, loadTextureCallback));
            }
        }

        public void RandomizeCASParts()
        {
            lock (sLock)
            {
                var random = new System.Random();
                foreach (CASPartResource.ClothingType clothingType in System.Enum.GetValues(typeof(CASPartResource.ClothingType)))
                {
                    var validCurrentTypePartKeys = new List<string>();
                    foreach (var casPartLookupKvp in CASPart.CASPartLookupCache)
                    {
                        var flags = casPartLookupKvp.Value;
                        if ((flags["Age"] & (uint)CurrentCASPart.CASPartResource.AgeGender.Age) != 0 && flags["Clothing"] == (uint)clothingType && (flags["ClothingCategory"] & (uint.MaxValue - (uint)CASPartResource.ClothingCategoryFlags.ValidForRandom) & (uint)CurrentCASPart.CASPartResource.ClothingCategory) != 0 && (flags["ClothingCategory"] & (uint)CASPartResource.ClothingCategoryFlags.ValidForRandom) != 0 && (flags["Gender"] & (uint)CurrentCASPart.CASPartResource.AgeGender.Gender) != 0 && flags["Species"] == (uint)CurrentCASPart.CASPartResource.AgeGender.Species)
                        {
                            validCurrentTypePartKeys.Add(casPartLookupKvp.Key);
                        }
                    }
                    if (validCurrentTypePartKeys.Count > 0)
                    {
                        SetCASPart(clothingType, validCurrentTypePartKeys[random.Next(0, validCurrentTypePartKeys.Count - 1)]);
                    }
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
            mCASParts[clothingType] = new CASPart(evaluated.Package, evaluated.ResourceIndexEntry, PreloadedData.GEOMs, PreloadedData.VPXYs);
        }

        public void SetCASPart(CASPartResource.ClothingType clothingType, uint type, uint group, ulong instance)
        {
            SetCASPart(clothingType, new ResourceKey(type, group, instance).ReverseEvaluateResourceKey());
        }
    }
}
