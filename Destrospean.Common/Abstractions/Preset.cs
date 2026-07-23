namespace Destrospean.Common.Abstractions
{
    public abstract class Preset : Complate, System.IDisposable
    {
        protected CASTableObject mCASTableObject;

        protected PresetInternalBase mInternal;

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

        public override s3pi.Interfaces.IPackage ParentPackage
        {
            get
            {
                return CASTableObject.ParentPackage;
            }
        }

        public System.Collections.Generic.List<Pattern> Patterns
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

        public override System.Collections.Generic.IDictionary<string, PropertyMeta> PropertiesTyped
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

        public System.Drawing.Bitmap Texture
        {
            get
            {
                return mInternal.Texture;
            }
        }

        protected abstract class PresetInternalBase : Complate
        {
            protected System.Drawing.Bitmap mTexture;

            public string AmbientMap
            {
                get;
                protected set;
            }

            public override CASTableObject CASTableObject
            {
                get
                {
                    return Preset.CASTableObject;
                }
            }

            public override s3pi.Interfaces.IPackage ParentPackage
            {
                get
                {
                    return Preset.ParentPackage;
                }
            }

            public virtual System.Collections.Generic.List<Pattern> Patterns
            {
                get;
                protected set;
            }

            public string[] PatternSlotNames
            {
                get
                {
                    return Patterns.ConvertAll(x => x.SlotName).ToArray();
                }
            }

            public Preset Preset
            {
                get;
                protected set;
            }

            public string SpecularMap
            {
                get;
                protected set;
            }

            public abstract System.Drawing.Bitmap NewTexture
            {
                get;
            }

            public System.Drawing.Bitmap Texture
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
                    if (mTexture != null)
                    {
                        mTexture.Dispose();
                    }
                    mTexture = value;
                }
            }

            public abstract void ReplacePresetComplate();

            public void ReplacePresetComplate(S3PIExtensions.PackageResourceIndexEntryTuple evaluated)
            {
                mXmlDocument.LoadXml(new System.IO.StreamReader(((s3pi.Interfaces.APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry)).ReadToEnd());
                PropertiesTyped.Clear();
                foreach (System.Xml.XmlNode childNode in mXmlDocument.SelectSingleNode("complate").ChildNodes)
                {
                    if (childNode.Name == "variables")
                    {
                        foreach (System.Xml.XmlNode grandchildNode in childNode.ChildNodes)
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

        public abstract void AddPattern(string patternSlotName, string newComplateName);

        public void Dispose()
        {
            lock (Lock)
            {
                mInternal.Texture = null;
                foreach (var pattern in Patterns)
                {
                    try
                    {
                        var patternImageDisposable = pattern.PatternImage as System.IDisposable;
                        if (patternImageDisposable != null)
                        {
                            patternImageDisposable.Dispose();
                        }
                    }
                    catch (System.ArgumentNullException)
                    {
                    }
                }
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
                    lock (Lock)
                    {
                        mInternal.Texture = mInternal.NewTexture;
                    }
                    MarkModelsNeedUpdatedCallback();
                }).Start();
        }

        public abstract void ReplacePattern(string patternSlotName, string patternKey);

        public override void SetValue(string propertyName, string newValue, System.Destrospean.Action beforeMarkUnsaved = null)
        {
            mInternal.SetValue(propertyName, newValue, beforeMarkUnsaved ?? RegenerateTexture);
        }
    }
}
