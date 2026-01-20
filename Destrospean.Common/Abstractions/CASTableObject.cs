using System.Collections.Generic;
using Destrospean.CmarNYCBorrowed;
using Destrospean.S3PIExtensions;

namespace Destrospean.Common.Abstractions
{
    public abstract class CASTableObject
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

        public readonly Preset DefaultPreset;

        public readonly string DefaultPresetKey;

        public readonly s3pi.Interfaces.IPackage ParentPackage;

        public readonly List<Preset> Presets = new List<Preset>();

        public delegate void UpdateUIDelegate(CASTableObject castableObject, int lodIndex, int groupIndex);

        public CASTableObject(s3pi.Interfaces.IPackage package, s3pi.Interfaces.IResourceIndexEntry resourceIndexEntry)
        {
            ParentPackage = package;
            var defaultPresetResourceIndexEntries = ParentPackage.FindAll(x => x.ResourceType == ResourceUtils.GetResourceType("_XML") && x.ResourceGroup == resourceIndexEntry.ResourceGroup && x.Instance == resourceIndexEntry.Instance);
            if (defaultPresetResourceIndexEntries.Count == 0)
            {
                DefaultPresetKey = null;
                DefaultPreset = null;
            }
            else
            {
                DefaultPresetKey = defaultPresetResourceIndexEntries[0].ReverseEvaluateResourceKey();
                DefaultPreset = new CASPartPreset(this, new System.IO.StreamReader(((s3pi.Interfaces.APackage)ParentPackage).GetResource(defaultPresetResourceIndexEntries[0])));
            }
        }

        public void ClearCurrentRig()
        {
            mCurrentRig = null;
        }

        public void SaveDefaultPreset()
        {   
            if (DefaultPreset == null || DefaultPresetKey == null)
            {
                return;
            }
            var defaultPresetResourceIndexEntry = ParentPackage.EvaluateResourceKey(DefaultPresetKey).ResourceIndexEntry;
            var tempResourceIndexEntry = ParentPackage.AddResource(defaultPresetResourceIndexEntry, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(((CASPartPreset)AllPresets[0]).XmlFile.ReadToEnd())), false);
            ParentPackage.ReplaceResource(defaultPresetResourceIndexEntry, s3pi.WrapperDealer.WrapperDealer.GetResource(0, ParentPackage, tempResourceIndexEntry));
            ParentPackage.DeleteResource(tempResourceIndexEntry);
        }

        public abstract void SavePresets();
    }
}
