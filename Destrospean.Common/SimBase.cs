using System.Collections.Generic;
using Destrospean.CmarNYCBorrowed;
using Destrospean.Common.Abstractions;

namespace Destrospean.Common
{
    public abstract class SimBase
    {
        readonly Dictionary<CASPartResource.ClothingType, CASPart> mCASParts = new Dictionary<CASPartResource.ClothingType, CASPart>();

        string mID;

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

        public void LoadMeshes(int presetIndex, int lodIndex, LoadTextureDelegate loadTextureCallback)
        {
            System.Array.ForEach(new List<CASPart>(CASParts.Values).FindAll(x => x != null).ToArray(), x => LoadMeshes(x, presetIndex, lodIndex, loadTextureCallback));
        }
    }
}
