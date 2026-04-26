using System;
using System.Collections.Generic;
using System.Drawing;
using Destrospean.Common;
using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;
using Gtk;
using s3pi.Interfaces;
using s3pi.WrapperDealer;

namespace Destrospean.DestrospeanCASPEditor
{
    public partial class ChooseObjectDialog : Dialog
    {
        public string CASPartName
        {
            get;
            private set;
        }

        public static int ColumnSpacing = 6;

        public static readonly Dictionary<string, Gdk.Pixbuf> PreloadedCASPartImagePixbufs = new Dictionary<string, Gdk.Pixbuf>();

        public string ResourceKey
        {
            get;
            private set;
        }

        public ChooseObjectDialog(Window parent, IPackage package, CASPartResource.ClothingType clothingType, CASPart[] casParts) : this("Choose CAS Part", parent, package, clothingType, casParts)
        {
        }

        public ChooseObjectDialog(string title, Window parent, IPackage package, CASPartResource.ClothingType clothingType, CASPart[] casParts) : base(title, parent, DialogFlags.Modal)
        {
            Build();
            IconView.ColumnSpacing = ColumnSpacing;
            if (parent != null)
            {
                this.RescaleAndReposition(parent);
            }
            var casPartsNamesKeysThumbnailKeys = new List<string[]>();
            List<string> keys = new List<string>(),
            names = new List<string>();
            var listStore = new ListStore(typeof(string), typeof(Gdk.Pixbuf));
            var uncachedCASPartExists = false;
            foreach (var casPartLookupKvp in CASPart.CASPartLookupCache)
            {
                if ((CASPartResource.ClothingType)Enum.Parse(typeof(CASPartResource.ClothingType), casPartLookupKvp.Value["Clothing"]) != clothingType || Array.Exists(casParts, x => x != null && x.CASPartResource.Clothing != clothingType && ((x.CASPartResource.AgeGender.Age & (CASPartResource.AgeFlags)Enum.Parse(typeof(CASPartResource.AgeFlags), casPartLookupKvp.Value["Age"])) == 0 || (x.CASPartResource.AgeGender.Gender & (CASPartResource.GenderFlags)Enum.Parse(typeof(CASPartResource.GenderFlags), casPartLookupKvp.Value["Gender"])) == 0 || (CASPart.GetAdjustedSpecies(x.CASPartResource.AgeGender.Species) & CASPart.GetAdjustedSpecies((CASPartResource.SpeciesType)Enum.Parse(typeof(CASPartResource.SpeciesType), casPartLookupKvp.Value["Species"]))) == 0)))
                {
                    continue;
                }
                casPartsNamesKeysThumbnailKeys.Add(new[]
                    {
                        casPartLookupKvp.Value["Unknown1"],
                        casPartLookupKvp.Key,
                        "key:626F60CE" + casPartLookupKvp.Key.Substring(12)
                    });
            }
            casPartsNamesKeysThumbnailKeys.Sort((a, b) => a[0].CompareTo(b[0]));
            foreach (var casPartNameKeyThumbnailKey in casPartsNamesKeysThumbnailKeys)
            {
                Gdk.Pixbuf pixbuf = null;
                if (!PreloadedCASPartImagePixbufs.TryGetValue(casPartNameKeyThumbnailKey[1], out pixbuf))
                {
                    Bitmap casPartImage = null;
                    try
                    {
                        var evaluated = package.EvaluateThumbnailResourceKey(casPartNameKeyThumbnailKey[2]);
                        casPartImage = new Bitmap(((APackage)evaluated.Package).GetResource(evaluated.ResourceIndexEntry));
                    }
                    catch (ResourceIndexEntryNotFoundException)
                    {
                        casPartImage = new Bitmap(64, 64);
                        using (var graphics = System.Drawing.Graphics.FromImage(casPartImage))
                        {
                            graphics.Clear(Color.Transparent);
                        }
                    }
                    casPartImage = CASPartThumbnailCache.Singleton.PreloadedThumbnails[casPartNameKeyThumbnailKey[1]] = new System.Drawing.Bitmap(casPartImage, 64, 64);
                    pixbuf = PreloadedCASPartImagePixbufs[casPartNameKeyThumbnailKey[1]] = casPartImage.ToPixbuf();
                    uncachedCASPartExists = true;
                }
                listStore.AppendValues(casPartNameKeyThumbnailKey[0], pixbuf.ScaleSimple(WidgetUtils.SmallImageSize << 1, WidgetUtils.SmallImageSize << 1, Gdk.InterpType.Bilinear));
                keys.Add(casPartNameKeyThumbnailKey[1]);
                names.Add(casPartNameKeyThumbnailKey[0]);
            }
            if (uncachedCASPartExists)
            {
                CASPartThumbnailCache.Singleton.SaveCache();
            }
            IconView.Model = listStore;
            IconView.PixbufColumn = 1;
            IconView.TooltipColumn = 0;
            IconView.SelectionChanged += (sender, e) => OKButton.Sensitive = IconView.SelectedItems.Length > 0;
            var casPartIndex = Array.FindIndex(casParts, x => x.CASPartResource.Clothing == clothingType);
            if (casPartIndex > -1)
            {
                var index = names.FindIndex(x => x == casParts[casPartIndex].CASPartResource.Unknown1);
                IconView.SelectPath(new TreePath((index > -1 ? index : 0).ToString()));
            }
            else
            {
                IconView.SelectPath(new TreePath("0"));
            }
            Response += (o, args) =>
                {
                    if (args.ResponseId == ResponseType.Ok)
                    {
                        CASPartName = names[IconView.SelectedItems[0].Indices[0]];
                        ResourceKey = keys[IconView.SelectedItems[0].Indices[0]];
                    }
                };
        }

        public static void GenerateCache()
        {
            if (System.IO.File.Exists(CASPartThumbnailCache.Singleton.CacheFilePath))
            {
                return;
            }
            CASPartThumbnailCache.Singleton.GenerateCache(s3pi.Package.Package.NewPackage(0));
            foreach (var casPartImageKvp in CASPartThumbnailCache.Singleton.PreloadedThumbnails)
            {
                PreloadedCASPartImagePixbufs.Add(casPartImageKvp.Key, casPartImageKvp.Value.ToPixbuf());
            }
        }

        public static bool LoadCache()
        {
            if (!CASPartThumbnailCache.Singleton.LoadCache())
            {
                return false;
            }
            foreach (var casPartImageKvp in CASPartThumbnailCache.Singleton.PreloadedThumbnails)
            {
                PreloadedCASPartImagePixbufs.Add(casPartImageKvp.Key, casPartImageKvp.Value.ToPixbuf());
            }
            return true;
        }
    }
}
