namespace Destrospean.Common.Abstractions
{
    public interface IPreset
    {
        string AmbientMap
        {
            get;
        }

        System.Collections.Generic.List<Pattern> Patterns
        {
            get;
        }

        CASTableObject CASTableObject
        {
            get;
        }

        s3pi.Interfaces.IPackage ParentPackage
        {
            get;
        }

        string[] PatternSlotNames
        {
            get;
        }

        System.Collections.Generic.IDictionary<string, Complate.PropertyMeta> PropertiesTyped
        {
            get;
        }

        string[] PropertyNames
        {
            get;
        }

        string SpecularMap
        {
            get;
        }

        System.Drawing.Bitmap Texture
        {
            get;
        }

        void AddPattern(string patternSlotName, string newComplateName);

        string GetValue(string propertyName);

        void RegenerateTexture();

        void ReplacePattern(string patternSlotName, string patternKey);

        void SetValue(string propertyName, string newValue, CmarNYCBorrowed.Action beforeMarkUnsaved = null);
    }
}
