using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;
using Gdk;
using Gtk;

namespace Destrospean.DestrospeanCASPEditor.Widgets
{
    public class ImageResourceComboBox : ComboBox
    {
        protected readonly System.Collections.Generic.List<ImageResourceComboBoxEntry> mEntries;

        protected static readonly System.Collections.Generic.Dictionary<string, Pixbuf> mPresetTextureSmallPixbufs = new System.Collections.Generic.Dictionary<string, Pixbuf>();

        public int EntryCount
        {
            get
            {
                return mEntries.Count;
            }
        }

        public ImageResourceComboBoxEntry this[int index]
        {
            get
            {
                return mEntries[index];
            }
            set
            {
                mEntries[index] = value;
            }
        }

        public class ImageResourceComboBoxCellRendererText : Gtk.CellRendererText
        {
            protected override void Render(Drawable window, Widget widget, Rectangle background_area, Rectangle cell_area, Rectangle expose_area, CellRendererState flags)
            {
                if (flags.HasFlag(CellRendererState.Prelit))
                {
                    //System.Console.WriteLine(Text);
                }
                base.Render(window, widget, background_area, cell_area, expose_area, flags);
            }
        }

        public struct ImageResourceComboBoxEntry
        {
            public readonly Pixbuf Image;

            public readonly string Label;

            public ImageResourceComboBoxEntry(Pixbuf image, string label)
            {
                Image = image;
                Label = label;
            }
        }

        protected ImageResourceComboBox(System.Collections.Generic.List<ImageResourceComboBoxEntry> entries) : base()
        {
            mEntries = entries;
        }

        public static ImageResourceComboBox CreateInstance(s3pi.Interfaces.IPackage package, string currentValue, Preset preset, Gtk.Image imageWidget, bool excludeTXTCs = false)
        {
            try
            {
                var entries = package.FindAll(x => "_IMGTXTC".Substring(0, preset is CASPartPreset || excludeTXTCs ? 4 : 8).Contains(x.GetResourceTypeTag())).ConvertAll(x =>
                    {
                        var key = x.ReverseEvaluateResourceKey();
                        switch (x.GetResourceTypeTag())
                        {
                            case "_IMG":
                                return new ImageResourceComboBoxEntry(ImageUtils.PreloadedImagePixbufs[key][1], key);
                            default:
                                Pixbuf pixbuf;
                                if (!mPresetTextureSmallPixbufs.TryGetValue(key, out pixbuf))
                                {
                                    pixbuf = preset.Texture.ToPixbuf().ScaleSimple(WidgetUtils.SmallImageSize, WidgetUtils.SmallImageSize, InterpType.Bilinear);
                                    mPresetTextureSmallPixbufs.Add(key, pixbuf);
                                }
                                return new ImageResourceComboBoxEntry(pixbuf, key);
                        }
                    });
                var listStore = new ListStore(typeof(Pixbuf), typeof(string));
                entries.ForEach(x => listStore.AppendValues(x.Image, x.Label));
                var currentValueKey = currentValue;
                try
                {
                    currentValueKey = "key:" + ResourceUtils.GetResourceType("_IMG").ToString("X8") + ":00000000:" + System.Security.Cryptography.FNV64.GetHash(currentValue.Substring(currentValue.LastIndexOf("\\") + 1, currentValue.LastIndexOf(".") - currentValue.LastIndexOf("\\") - 1)).ToString("X16");
                }
                catch
                {
                }
                var missing = ResourceUtils.MissingResourceKeys.Exists(x => x.ToLowerInvariant() == currentValueKey.ToLowerInvariant());
                if (!entries.Exists(x => x.Label.ToLowerInvariant() == currentValueKey.ToLowerInvariant()))
                {
                    System.Collections.Generic.List<Pixbuf> pixbufs = null;
                    if (!ImageUtils.PreloadedGameImagePixbufs.TryGetValue(currentValueKey, out pixbufs) && !missing)
                    {
                        try
                        {
                            var evaluated = package.EvaluateImageResourceKey(currentValue);
                            evaluated.Package.PreloadGameImage(evaluated.ResourceIndexEntry, imageWidget);
                            pixbufs = ImageUtils.PreloadedGameImagePixbufs[currentValueKey];
                            pixbufs.Add(pixbufs[0].ScaleSimple(WidgetUtils.SmallImageSize, WidgetUtils.SmallImageSize, InterpType.Bilinear));
                        }
                        catch
                        {
                            ResourceUtils.MissingResourceKeys.Add(currentValueKey);
                            missing = true;
                        }
                    }
                    entries.Add(new ImageResourceComboBoxEntry(missing ? null : pixbufs[1], currentValueKey.ToUpperInvariant().Replace("KEY", "key")));
                    listStore.AppendValues(entries[entries.Count - 1].Image, entries[entries.Count - 1].Label);
                }
                entries.Add(new ImageResourceComboBoxEntry(null, "<Specify key>"));
                listStore.AppendValues(entries[entries.Count - 1].Image, entries[entries.Count - 1].Label);
                var comboBox = new ImageResourceComboBox(entries)
                    {
                        Active = entries.FindIndex(x => x.Label.ToLowerInvariant() == currentValueKey.ToLowerInvariant()),
                        Model = listStore
                    };
                var comboBoxLastActive = comboBox.Active;
                comboBox.Changed += (sender, e) =>
                    {
                        if (comboBox.Active == entries.Count - 1)
                        {
                            var textEntryDialog = new TextEntryDialog("Specify Key", "Specify the image resource's key (in the format of \"key:########:########:################\"):", MainWindowBase.Singleton);
                            if (textEntryDialog.Run() == (int)ResponseType.Ok)
                            {
                                var existingEntryIndex = entries.FindIndex(x => x.Label.ToLowerInvariant() == textEntryDialog.TextEntryValue.ToLowerInvariant());
                                if (existingEntryIndex == -1)
                                {
                                    var exists = true;
                                    System.Collections.Generic.List<Pixbuf> pixbufs = null;
                                    if (!ImageUtils.PreloadedGameImagePixbufs.TryGetValue(textEntryDialog.TextEntryValue, out pixbufs))
                                    {
                                        try
                                        {
                                            var evaluated = package.EvaluateImageResourceKey(textEntryDialog.TextEntryValue);
                                            evaluated.Package.PreloadGameImage(evaluated.ResourceIndexEntry, imageWidget);
                                            pixbufs = ImageUtils.PreloadedGameImagePixbufs[textEntryDialog.TextEntryValue];
                                            pixbufs.Add(pixbufs[0].ScaleSimple(WidgetUtils.SmallImageSize, WidgetUtils.SmallImageSize, InterpType.Bilinear));
                                        }
                                        catch
                                        {
                                            comboBox.Active = comboBoxLastActive;
                                            exists = false;
                                        }
                                    }
                                    if (exists)
                                    {
                                        entries.Insert(entries.Count - 1, new ImageResourceComboBoxEntry(pixbufs[1], textEntryDialog.TextEntryValue.ToUpperInvariant().Replace("KEY", "key")));
                                        listStore.InsertWithValues(entries.Count - 2, entries[entries.Count - 2].Image, entries[entries.Count - 2].Label);
                                        comboBox.Active = entries.Count - 2;
                                    }
                                }
                                else
                                {
                                    comboBox.Active = existingEntryIndex;
                                }
                            }
                            else
                            {
                                comboBox.Active = comboBoxLastActive;
                            }
                            textEntryDialog.Destroy();
                        }
                        else
                        {
                            comboBoxLastActive = comboBox.Active;
                        }
                    };
                var pixbufRenderer = new CellRendererPixbuf
                    {
                        Xpad = 4
                    };
                var textRenderer = new ImageResourceComboBoxCellRendererText
                    {
                        Xpad = 4
                    };
                comboBox.PackStart(pixbufRenderer, false);
                comboBox.AddAttribute(pixbufRenderer, "pixbuf", 0);
                comboBox.PackStart(textRenderer, false);
                comboBox.AddAttribute(textRenderer, "text", 1);
                return comboBox;
            }
            catch (System.Exception ex)
            {
                Common.ProgramUtils.WriteError(ex);
                throw;
            }
        }

        public static void DeleteSmallTexturePixbufs()
        {
            mPresetTextureSmallPixbufs.Clear();
        }
    }
}
