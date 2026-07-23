using System.Collections.Generic;
using Destrospean.CmarNYCBorrowed;
using Destrospean.S3PIExtensions;

namespace Destrospean.Common.Abstractions
{
    public abstract class CASTableObject : System.IDisposable
    {
        protected Rig mCurrentRig;

        public List<Preset> AllPresets
        {
            get
            {
                var allPresets = new List<Preset>(Presets);
                if (DefaultPreset != null)
                {
                    allPresets.Insert(0, DefaultPreset);
                }
                return allPresets;
            }
        }

        public abstract Rig CurrentRig
        {
            get;
        }

        public Preset DefaultPreset
        {
            get;
            protected set;
        }

        public string DefaultPresetKey
        {
            get;
            protected set;
        }

        public readonly s3pi.Interfaces.IPackage ParentPackage;

        public readonly List<Preset> Presets = new List<Preset>();

        public readonly string ResourceKey;

        public delegate void UpdateUIDelegate(CASTableObject castableObject, int lodIndex, int groupIndex);

        public CASTableObject(s3pi.Interfaces.IPackage package, s3pi.Interfaces.IResourceIndexEntry resourceIndexEntry)
        {
            ResourceKey = resourceIndexEntry.ReverseEvaluateResourceKey();
            ParentPackage = package;
        }

        public void ClearCurrentRig()
        {
            mCurrentRig = null;
        }

        public virtual void Dispose()
        {
            AllPresets.ForEach(x =>
                {
                    var casPartPreset = x as CASPartPreset;
                    if (casPartPreset != null)
                    {
                        casPartPreset.XmlFile.Close();
                    }
                    x.Dispose();
                });
        }

        public void SaveDefaultPreset()
        {   
            if (DefaultPreset == null || DefaultPresetKey == null)
            {
                return;
            }
            var defaultPresetResourceIndexEntry = ParentPackage.EvaluateResourceKey(DefaultPresetKey).ResourceIndexEntry;
            ParentPackage.DeleteResource(defaultPresetResourceIndexEntry);
            ParentPackage.AddResource(defaultPresetResourceIndexEntry, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(((CASPartPreset)AllPresets[0]).XmlFile.ReadToEnd())), true);
        }

        public abstract void SavePresets();
    }
}
