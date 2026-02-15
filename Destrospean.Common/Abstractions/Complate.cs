using System.Collections.Generic;
using System.Xml;
using System.Drawing;

namespace Destrospean.Common.Abstractions
{
    public abstract class Complate
    {
        protected readonly IDictionary<string, PropertyMeta> mPropertiesTyped;

        protected readonly IDictionary<string, XmlNode> mPropertiesXmlNodes;

        protected readonly XmlDocument mXmlDocument = new XmlDocument();

        public abstract CASTableObject CASTableObject
        {
            get;
        }

        public static CmarNYCBorrowed.TextureUtils.GetTextureDelegate GetTextureCallback;

        public readonly string ID;

        public static object Lock = new object();

        public static CmarNYCBorrowed.Action MarkModelsNeedUpdatedCallback, MarkUnsavedChangesCallback;

        public abstract s3pi.Interfaces.IPackage ParentPackage
        {
            get;
        }

        public virtual IDictionary<string, PropertyMeta> PropertiesTyped
        {
            get
            {
                return mPropertiesTyped;
            }
        }

        public virtual string[] PropertyNames
        {
            get
            {
                return new List<string>(mPropertiesXmlNodes.Keys).ToArray();
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return GetValue(propertyName);
            }
            set
            {
                SetValue(propertyName, value);
            }
        }

        public struct PropertyMeta
        {
            public string DefaultValue, Type;

            public PropertyMeta(string type, string defaultValue)
            {
                DefaultValue = defaultValue;
                Type = type;
            }
        }

        protected class PropertyNameComparer : IComparer<string>
        {
            public int Compare(string a, string b)
            {
                string aCopy = a,
                bCopy = b;
                while (aCopy.Length < bCopy.Length)
                {
                    aCopy += " ";
                }
                while (aCopy.Length > bCopy.Length)
                {
                    bCopy += " ";
                }
                for (var i = 0; i < aCopy.Length; i++)
                {
                    if (aCopy[i] != bCopy[i] && aCopy.Substring(0, i) == bCopy.Substring(0, i))
                    {
                        bool aCharIsNum = '0' <= aCopy[i] && aCopy[i] <= '9',
                        bCharIsNum = '0' <= bCopy[i] && bCopy[i] <= '9';
                        if (aCharIsNum && !bCharIsNum)
                        {
                            return 1;
                        }
                        if (!aCharIsNum && bCharIsNum)
                        {
                            return -1;
                        }
                    }
                }
                return string.Compare(a, b);
            }
        }

        public Complate()
        {
            mPropertiesXmlNodes = new SortedDictionary<string, XmlNode>(new PropertyNameComparer());
            mPropertiesTyped = new SortedDictionary<string, PropertyMeta>(new PropertyNameComparer());
            ID = System.Guid.NewGuid().ToString();
        }

        public virtual string GetValue(string propertyName)
        {
            return mPropertiesXmlNodes.ContainsKey(propertyName) ? mPropertiesXmlNodes[propertyName].Attributes["value"].Value : "";
        }

        public static float[] ParseCommaSeparatedValues(string text)
        {
            return System.Array.ConvertAll(text.Split(','), x => float.Parse(x, System.Globalization.CultureInfo.InvariantCulture));
        }

        public static Bitmap QuadrupleCanvasSize(Bitmap image)
        {
            var imageCopy = new Bitmap(image.Width << 1, image.Height << 1); 
            using (var graphics = Graphics.FromImage(imageCopy))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, image.Width >> 1, image.Height >> 1);
            }
            return imageCopy;
        }

        public static Bitmap RotateImage(Bitmap image, float angle)
        {
            var imageCopy = new Bitmap(image.Width, image.Height); 
            using (var graphics = Graphics.FromImage(imageCopy))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.TranslateTransform(image.Width >> 1, image.Height >> 1);
                graphics.RotateTransform(angle);
                graphics.TranslateTransform(-image.Width >> 1, -image.Height >> 1);
                graphics.DrawImage(image, 0, 0);
            }
            return imageCopy;
        }

        public virtual void SetValue(string propertyName, string newValue, CmarNYCBorrowed.Action beforeMarkUnsaved = null)
        {
            mPropertiesXmlNodes[propertyName].Attributes["value"].Value = newValue;
            if (beforeMarkUnsaved != null)
            {
                beforeMarkUnsaved();
            }
            MarkUnsavedChangesCallback();
        }
    }
}
