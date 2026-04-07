using System;
using System.Collections.Generic;
using s3pi.Extensions;
using s3pi.Filetable;
using s3pi.Interfaces;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
    public sealed class ExtensionAttribute : Attribute
    {
    }
}

namespace Destrospean.S3PIExtensions
{
    public class AttributeNotFoundException : Exception
    {
        public AttributeNotFoundException()
        {
        }

        public AttributeNotFoundException(string message) : base(message)
        {
        }

        public AttributeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public struct EvaluatedResourceKey
    {
        public readonly IPackage Package;

        public readonly IResourceIndexEntry ResourceIndexEntry;

        public EvaluatedResourceKey(IPackage package, IResourceIndexEntry resourceIndexEntry)
        {
            Package = package;
            ResourceIndexEntry = resourceIndexEntry;
        }
    }

    public class ResourceIndexEntryNotFoundException : Exception
    {
        public ResourceIndexEntryNotFoundException()
        {
        }

        public ResourceIndexEntryNotFoundException(string message) : base(message)
        {
        }

        public ResourceIndexEntryNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class ResourceKey : IResourceKey
    {
        public ulong Instance
        {
            get;
            set;
        }

        public uint ResourceGroup
        {
            get;
            set;
        }

        public uint ResourceType
        {
            get;
            set;
        }

        public ResourceKey(uint type, uint group, ulong instance)
        {
            Instance = instance;
            ResourceGroup = group;
            ResourceType = type;
        }

        public int CompareTo(IResourceKey other)
        {
            var result = ResourceType.CompareTo(other.ResourceType);
            if (result != 0)
            {
                return result;
            }
            if ((result = ResourceGroup.CompareTo(other.ResourceGroup)) != 0)
            {
                return result;
            }
            return Instance.CompareTo(other.Instance);
        }

        public bool Equals(IResourceKey a, IResourceKey b)
        {
            return a.Equals(b);
        }

        public bool Equals(IResourceKey other)
        {
            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            return ResourceType.GetHashCode() ^ ResourceGroup.GetHashCode() ^ Instance.GetHashCode();
        }

        public int GetHashCode(IResourceKey resourceKey)
        {
            return resourceKey.GetHashCode();
        }
    }

    public static class ResourceUtils
    {
        static Dictionary<PackageTag, IPackage> sGameContentPackages, sGameImageResourcePackages, sGameThumbnailResourcePackages;

        static object sLock = new object();

        static List<string> sMissingResourceKeys;

        public static Dictionary<PackageTag, IPackage> GameContentPackages
        {
            get
            {
                if (sGameContentPackages == null)
                {
                    lock (sLock)
                    {
                        sGameContentPackages = new Dictionary<PackageTag, IPackage>();
                        foreach (var game in GameFolders.Games)
                        {
                            var enumerator = game.GameContent.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                sGameContentPackages.Add(enumerator.Current, s3pi.Package.Package.OpenPackage(0, enumerator.Current.Path));
                            }
                        }
                    }
                }
                return sGameContentPackages;
            }
        }

        public static Dictionary<PackageTag, IPackage> GameImageResourcePackages
        {
            get
            {
                if (sGameImageResourcePackages == null)
                {
                    lock (sLock)
                    {
                        sGameImageResourcePackages = new Dictionary<PackageTag, IPackage>();
                        foreach (var game in GameFolders.Games)
                        {
                            var enumerator = game.DDSImages.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                sGameImageResourcePackages.Add(enumerator.Current, s3pi.Package.Package.OpenPackage(0, enumerator.Current.Path));
                            }
                        }
                    }
                }
                return sGameImageResourcePackages;
            }
        }

        public static Dictionary<PackageTag, IPackage> GameThumbnailResourcePackages
        {
            get
            {
                if (sGameThumbnailResourcePackages == null)
                {
                    lock (sLock)
                    {
                        sGameThumbnailResourcePackages = new Dictionary<PackageTag, IPackage>();
                        foreach (var game in GameFolders.Games)
                        {
                            var enumerator = game.Thumbnails.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                sGameThumbnailResourcePackages.Add(enumerator.Current, s3pi.Package.Package.OpenPackage(0, enumerator.Current.Path));
                            }
                        }
                    }
                }
                return sGameThumbnailResourcePackages;
            }
        }

        public static List<string> MissingResourceKeys
        {
            get
            {
                if (sMissingResourceKeys == null)
                {
                    sMissingResourceKeys = new List<string>();
                }
                return sMissingResourceKeys;
            }
        }

        static bool TryEvaluateResourceKeyInternal(this IPackage package, string key, out EvaluatedResourceKey evaluated)
        {
            lock (sLock)
            {
                var tgi = new ulong[3];
                var i = 0;
                foreach (var hex in key.Substring(4).Split(':'))
                {
                    tgi[i++] = Convert.ToUInt64(hex, 16);
                }
                var results = package.FindAll(x => x.ResourceType == tgi[0] && x.ResourceGroup == tgi[1] && x.Instance == tgi[2]);
                if (results.Count == 0)
                {
                    evaluated = new EvaluatedResourceKey(null, null);
                    return false;
                }
                evaluated = new EvaluatedResourceKey(package, results[0]);
                return true;
            }
        }

        public static IResourceIndexEntry AddResource(this IPackage package, string filename, IResourceKey resourceKey = null, bool rejectDups = true)
        {
            return package.AddResource(resourceKey ?? new ResourceKey(0, 0, System.Security.Cryptography.FNV64.GetHash(Guid.NewGuid().ToString())), System.IO.File.OpenRead(filename), rejectDups);
        }

        public static void ClearGamePackages()
        {
            sGameContentPackages = null;
            sGameImageResourcePackages = null;
            sGameThumbnailResourcePackages = null;
        }

        public static EvaluatedResourceKey EvaluateImageResourceKey(this IPackage package, string key)
        {
            try
            {
                EvaluatedResourceKey evaluated;
                if (package.TryEvaluateResourceKeyInternal(key, out evaluated))
                {
                    return evaluated;
                }
                foreach (var gamePackage in GameImageResourcePackages.Values)
                {
                    if (gamePackage.TryEvaluateResourceKeyInternal(key, out evaluated))
                    {
                        return evaluated;
                    }
                }
                throw new ResourceIndexEntryNotFoundException("No image resource with the given key, \"" + key + ",\" could be found.");
            }
            catch (FormatException)
            {
                return EvaluateImageResourceKey(package, "key:" + GetResourceType("_IMG").ToString("X8") + ":00000000:" + System.Security.Cryptography.FNV64.GetHash(key.Substring(key.LastIndexOf("\\") + 1, key.LastIndexOf(".") - key.LastIndexOf("\\") - 1)).ToString("X16"));
            }
        }

        public static EvaluatedResourceKey EvaluateResourceKey(this IPackage package, string key)
        {   
            try
            {
                EvaluatedResourceKey evaluated;
                if (package.TryEvaluateResourceKeyInternal(key, out evaluated))
                {
                    return evaluated;
                }
                foreach (var gamePackage in GameContentPackages.Values)
                {
                    if (gamePackage.TryEvaluateResourceKeyInternal(key, out evaluated))
                    {
                        return evaluated;
                    }
                }
                throw new ResourceIndexEntryNotFoundException("No resource with the given key, \"" + key + ",\" could be found.");
            }
            catch (FormatException)
            {
                return EvaluateImageResourceKey(package, "key:" + GetResourceType("_IMG").ToString("X8") + ":00000000:" + System.Security.Cryptography.FNV64.GetHash(key.Substring(key.LastIndexOf("\\") + 1, key.LastIndexOf(".") - key.LastIndexOf("\\") - 1)).ToString("X16"));
            }
        }

        public static EvaluatedResourceKey EvaluateResourceKey(this IPackage package, System.Xml.XmlNode xmlNode)
        {   
            if (!((System.Xml.XmlElement)xmlNode).HasAttribute("reskey"))
            {
                throw new AttributeNotFoundException("The XML node given does not have the \"reskey\" attribute.");
            }
            var key = xmlNode.Attributes["reskey"].Value;
            EvaluatedResourceKey evaluated;
            if (package.TryEvaluateResourceKeyInternal(key, out evaluated))
            {
                return evaluated;
            }
            foreach (var gamePackage in GameContentPackages.Values)
            {
                if (gamePackage.TryEvaluateResourceKeyInternal(key, out evaluated))
                {
                    return evaluated;
                }
            }
            throw new ResourceIndexEntryNotFoundException("No resource with the key, \"" + key + ",\" referenced in the XML node could be found.");
        }

        public static EvaluatedResourceKey EvaluateThumbnailResourceKey(this IPackage package, string key)
        {
            try
            {
                EvaluatedResourceKey evaluated;
                if (package.TryEvaluateResourceKeyInternal(key, out evaluated))
                {
                    return evaluated;
                }
                foreach (var gamePackage in GameThumbnailResourcePackages.Values)
                {
                    if (gamePackage.TryEvaluateResourceKeyInternal(key, out evaluated))
                    {
                        return evaluated;
                    }
                }
                throw new ResourceIndexEntryNotFoundException("No thumbnail resource with the given key, \"" + key + ",\" could be found.");
            }
            catch (FormatException)
            {
                return EvaluateThumbnailResourceKey(package, "key:" + GetResourceType("THUM").ToString("X8") + ":00000000:" + System.Security.Cryptography.FNV64.GetHash(key.Substring(key.LastIndexOf("\\") + 1, key.LastIndexOf(".") - key.LastIndexOf("\\") - 1)).ToString("X16"));
            }
        }

        public static NameMapResource.NameMapResource[] GetNameMapResources(this IPackage package)
        {
            return package.FindAll(x => x.GetResourceTypeTag() == "_KEY").ConvertAll(x => (NameMapResource.NameMapResource)s3pi.WrapperDealer.WrapperDealer.GetResource(0, package, x)).ToArray();
        }

        public static IResourceIndexEntry GetResourceIndexEntry(this IPackage package, IResourceKey resourceKey)
        {
            return package.Find(x => x.ResourceType == resourceKey.ResourceType && x.ResourceGroup == resourceKey.ResourceGroup && x.Instance == resourceKey.Instance);
        }

        public static uint GetResourceType(string tag)
        {
            foreach (var type in ExtList.Ext.Keys)
            {
                if (ExtList.Ext[type].Contains(tag))
                {
                    return Convert.ToUInt32(type, 16);
                }
            }
            return 0;
        }

        public static string GetResourceTypeTag(this IResourceKey resourceKey)
        {
            return ExtList.Ext[resourceKey.ResourceType][0];
        }

        public static void ResolveResourceType(this IPackage package, IResourceIndexEntry resourceIndexEntry)
        {
            var stream = ((APackage)package).GetResource(resourceIndexEntry);
            string tag = null;
            try
            {
                var buffer = new byte[5];
                stream.Read(buffer, 0, buffer.Length);
                if ("DDS |" == new string(Array.ConvertAll(buffer, x => (char)x)))
                {
                    tag = "_IMG";
                    goto FinalSteps;
                }
            }
            catch
            {
            }
            try
            {
                new System.Xml.XmlDocument().Load(stream);
                tag = "_XML";
                goto FinalSteps;
            }
            catch
            {
            }
            try
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.GetName().Name == "s3pi.CASPartResource")
                    {
                        Activator.CreateInstance(assembly.GetType("CASPartResource.BlendGeometryResource"), 0, stream);
                        tag = "BGEO";
                        goto FinalSteps;
                    }
                }
            }
            catch
            {
            }
            try
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.GetName().Name == "s3pi.CASPartResource")
                    {
                        Activator.CreateInstance(assembly.GetType("CASPartResource.CASPartResource"), 0, stream);
                        tag = "CASP";
                        goto FinalSteps;
                    }
                }
            }
            catch
            {
            }
            try
            {
                var buffer = new byte[4];
                stream.Position = 45;
                stream.Read(buffer, 0, buffer.Length);
                if ("GEOM" == new string(Array.ConvertAll(buffer, x => (char)x)))
                {
                    tag = "GEOM";
                    goto FinalSteps;
                }
            }
            catch
            {
            }
            try
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.GetName().Name == "s3pi.TxtcResource")
                    {
                        Activator.CreateInstance(assembly.GetType("TxtcResource.TxtcResource"), 0, stream);
                        tag = "TXTC";
                        goto FinalSteps;
                    }
                }
            }
            catch
            {
            }
            try
            {
                tag = new s3pi.GenericRCOLResource.GenericRCOLResource(0, stream).ChunkEntries[0].RCOLBlock.Tag;
                goto FinalSteps;
            }
            catch
            {
            }
            FinalSteps:
            if (!string.IsNullOrEmpty(tag))
            {
                resourceIndexEntry.ResourceType = GetResourceType(tag);
            }
        }

        public static string ReverseEvaluateResourceKey(this IResourceKey resourceKey)
        {   
            return "key:" + resourceKey.ResourceType.ToString("X8") + ":" + resourceKey.ResourceGroup.ToString("X8") + ":" + resourceKey.Instance.ToString("X16");
        }

        public static Dictionary<ulong, string> ToDictionary(this NameMapResource.NameMapResource nameMapResource)
        {
            var nameMapDictionary = new Dictionary<ulong, string>();
            var enumerator = nameMapResource.GetEnumerator();
            while (enumerator.MoveNext())
            {
                nameMapDictionary.Add(enumerator.Current.Key, enumerator.Current.Value);
            }
            return nameMapDictionary;
        }
    }
}
