using Destrospean.Common.Abstractions;
using Destrospean.S3PIExtensions;
using Gdk;
using Gtk;

namespace Destrospean.DestrospeanCASPEditor.Widgets
{
    public class ImageResourceComboBox : ComboBox
    {
        protected readonly System.Collections.Generic.List<ImageResourceComboBoxEntry> mEntries;

        protected static readonly System.Collections.Generic.Dictionary<string, Pixbuf> mThumbnails = new System.Collections.Generic.Dictionary<string, Pixbuf>();

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

        protected class CellRendererText : Gtk.CellRendererText
        {
            protected ComboBox mComboBox;

            protected string mCurrentImageResourceKey;

            protected Gtk.Image mImage;

            public CellRendererText(ComboBox comboBox, Gtk.Image imageWidget) : base()
            {
                mComboBox = comboBox;
                mImage = imageWidget;
            }

            protected override void Render(Drawable window, Widget widget, Rectangle backgroundArea, Rectangle cellArea, Rectangle exposeArea, CellRendererState state)
            {
                System.Collections.Generic.List<Pixbuf> pixbufs;
                Widget glWidget = null;
                if (System.Array.Exists(System.AppDomain.CurrentDomain.GetAssemblies(), x => x.GetName().Name == "GLWidget"))
                {
                    var propertyInfo = MainWindowBase.Singleton.GetType().GetProperty("GLWidget");
                    if (propertyInfo != null)
                    {
                        glWidget = propertyInfo.GetValue(MainWindowBase.Singleton, null) as Widget;
                    }
                }
                if (mComboBox.PopupShown && state.HasFlag(CellRendererState.Prelit) && (ImageUtils.PreloadedGameImagePixbufs.TryGetValue(Text, out pixbufs) || ImageUtils.PreloadedImagePixbufs.TryGetValue(Text, out pixbufs)))
                {
                    mImage.Pixbuf = pixbufs[0];
                    mCurrentImageResourceKey = Text;
                    if (glWidget != null)
                    {
                        glWidget.Hide();
                    }
                    MainWindowBase.Singleton.DrawImage();
                }
                else if (mCurrentImageResourceKey == Text)
                {
                    mImage.Clear();
                    mCurrentImageResourceKey = null;
                    if (glWidget != null)
                    {
                        glWidget.Show();
                    }
                }
                base.Render(window, widget, backgroundArea, cellArea, exposeArea, state);
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
                                if (!mThumbnails.TryGetValue(key, out pixbuf))
                                {
                                    pixbuf = preset.Texture.ToPixbuf().ScaleSimple(WidgetUtils.SmallImageSize, WidgetUtils.SmallImageSize, InterpType.Bilinear);
                                    mThumbnails.Add(key, pixbuf);
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
                entries.Add(new ImageResourceComboBoxEntry(null, ""));
                listStore.AppendValues(entries[entries.Count - 1].Image, entries[entries.Count - 1].Label);
                entries.Add(new ImageResourceComboBoxEntry(null, "Specify key..."));
                listStore.AppendValues(entries[entries.Count - 1].Image, entries[entries.Count - 1].Label);
                var comboBox = new ImageResourceComboBox(entries)
                    {
                        Active = entries.FindIndex(x => x.Label.ToLowerInvariant() == currentValueKey.ToLowerInvariant()),
                        Model = listStore
                    };
                var comboBoxLastActive = comboBox.Active;
                comboBox.RowSeparatorFunc = (model, iter) => (string)model.GetValue(iter, 1) == "";
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
                                        entries.Insert(entries.Count - 2, new ImageResourceComboBoxEntry(pixbufs[1], textEntryDialog.TextEntryValue.ToUpperInvariant().Replace("KEY", "key")));
                                        listStore.InsertWithValues(entries.Count - 3, entries[entries.Count - 3].Image, entries[entries.Count - 3].Label);
                                        comboBox.Active = entries.Count - 3;
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
                            textEntryDialog.Dispose();
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
                var textRenderer = new CellRendererText(comboBox, imageWidget)
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
                System.Destrospean.Logger.WriteError(ex);
                throw;
            }
        }

        public static void DeleteThumbnails()
        {
            foreach (var key in new System.Collections.Generic.List<string>(mThumbnails.Keys))
            {
                mThumbnails[key].Dispose();
                mThumbnails.Remove(key);
            }
        }
    }
}
