using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using Destrospean.CmarNYCBorrowed;
using Destrospean.Common;
using Destrospean.S3PIExtensions;
using Gtk;
using s3pi.Interfaces;
using s3pi.WrapperDealer;

namespace Destrospean.DestrospeanCASPEditor
{
    public partial class ChoosePatternDialog : Dialog
    {
        public static int ColumnSpacing = 6;

        public string PatternPath
        {
            get;
            private set;
        }

        public static readonly Dictionary<string, Gdk.Pixbuf> PreloadedPatternImagePixbufs = new Dictionary<string, Gdk.Pixbuf>();

        public string ResourceKey
        {
            get;
            private set;
        }

        public ChoosePatternDialog(Window parent, IPackage package) : this("Choose Pattern", parent, package)
        {
        }

        public ChoosePatternDialog(string title, Window parent, IPackage package) : base(title, parent, DialogFlags.Modal)
        {
            Build();
            PatternIconView.ColumnSpacing = ColumnSpacing;
            if (parent != null)
            {
                this.RescaleAndReposition(parent);
            }
            List<string> categories = new List<string>(),
            patternKeys = new List<string>(),
            patternPaths = new List<string>();
            var categoryCell = new CellRendererText();
            ListStore categoryListStore = new ListStore(typeof(string)),
            patternListStore = new ListStore(typeof(string), typeof(Gdk.Pixbuf));
            var gamePatternListEvaluated = package.EvaluateResourceKey("key:D4D9FBE5:00000000:1BDE14D18B416FEC");
            var patternsByCategory = new Dictionary<string, List<string>>();
            System.Action<IResource> addPatternsByCategory = (patternListResource) =>
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(new System.IO.StreamReader(patternListResource.Stream).ReadToEnd());
                    foreach (XmlNode childNode in xmlDocument.SelectSingleNode("patternlist").ChildNodes)
                    {
                        if (childNode.Name == "category")
                        {
                            var category = childNode.Attributes["name"].Value;
                            patternsByCategory.Add(category, new List<string>());
                            foreach (XmlElement grandchildNode in childNode.ChildNodes)
                            {
                                if (grandchildNode.Name == "pattern")
                                {
                                    var key = "";
                                    if (grandchildNode.HasAttribute("reskey"))
                                    {
                                        key = grandchildNode.Attributes["reskey"].Value;
                                    }
                                    else if (grandchildNode.HasAttribute("name"))
                                    {
                                        key = "key:0333406C:00000000:" + System.Security.Cryptography.FNV64.GetHash(grandchildNode.Attributes["name"].Value).ToString("X16");
                                    }
                                    else
                                    {
                                        throw new AttributeNotFoundException("The pattern XML node given does not have a \"reskey\" or \"name\" attribute.");
                                    }
                                    patternsByCategory[category].Add(key);
                                    patternsByCategory[category].Add(grandchildNode.HasAttribute("name") ? "Materials\\" + category + "\\" + grandchildNode.Attributes["name"].Value : key);
                                }
                            }
                        }
                    }
                };
            System.Action<int> setPatternKeysAndPaths = (categoryIndex) =>
                {
                    patternKeys.Clear();
                    patternPaths.Clear();
                    patternListStore.Clear();
                    var patternKeysPaths = patternsByCategory[categories[categoryIndex]];
                    var patternNamesKeysPaths = new List<string[]>();
                    var uncachedPatternExists = false;
                    for (var i = 0; i < patternKeysPaths.Count; i += 2)
                    {
                        patternNamesKeysPaths.Add(new string[]
                            {
                                patternKeysPaths[i + 1].Substring(patternKeysPaths[i + 1].LastIndexOf("\\") + 1),
                                patternKeysPaths[i],
                                patternKeysPaths[i + 1]
                            });
                    }
                    patternNamesKeysPaths.Sort((a, b) => a[0].CompareTo(b[0]));
                    foreach (var patternNameKeyPath in patternNamesKeysPaths)
                    {
                        Gdk.Pixbuf pixbuf = null;
                        if (!PreloadedPatternImagePixbufs.TryGetValue(patternNameKeyPath[1], out pixbuf))
                        {
                            Bitmap patternImage = null;
                            PatternInfo patternInfo;
                            if (!PatternUtils.PreloadedPatterns.TryGetValue(patternNameKeyPath[1], out patternInfo))
                            {
                                patternInfo = PatternUtils.PreloadedPatterns[patternNameKeyPath[1]] = PatternUtils.GetPatternInfo(package, patternNameKeyPath[1]);
                            }
                            switch (patternInfo.Type)
                            {
                                case PatternType.Colored:
                                    patternImage = package.GetRGBPatternImage(patternInfo, ImageUtils.GetTexture);
                                    break;
                                case PatternType.HSV:
                                    patternImage = package.GetHSVPatternImage(patternInfo, ImageUtils.GetTexture);
                                    break;
                            }
                            if (patternInfo.Type == PatternType.Solid)
                            {
                                patternImage = new Bitmap(64, 64);
                                using (var graphics = System.Drawing.Graphics.FromImage(patternImage))
                                {
                                    graphics.Clear(Color.FromArgb((byte)(patternInfo.SolidColor[0] * byte.MaxValue), (byte)(patternInfo.SolidColor[1] * byte.MaxValue), (byte)(patternInfo.SolidColor[2] * byte.MaxValue)));
                                }
                            }
                            if (patternImage != null)
                            {
                                patternImage = PatternUtils.PreloadedPatternImages[patternNameKeyPath[1]] = new System.Drawing.Bitmap(patternImage, 64, 64);
                                pixbuf = PreloadedPatternImagePixbufs[patternNameKeyPath[1]] = patternImage.ToPixbuf();
                            }
                            uncachedPatternExists = true;
                        }
                        patternListStore.AppendValues(patternNameKeyPath[0], pixbuf.ScaleSimple(WidgetUtils.SmallImageSize << 1, WidgetUtils.SmallImageSize << 1, Gdk.InterpType.Bilinear));
                        patternKeys.Add(patternNameKeyPath[1]);
                        patternPaths.Add(patternNameKeyPath[2]);
                    }
                    if (uncachedPatternExists)
                    {
                        PatternUtils.SaveCache();
                    }
                    PatternIconView.SelectPath(new TreePath("0"));
                };
            addPatternsByCategory(WrapperDealer.GetResource(0, gamePatternListEvaluated.Package, gamePatternListEvaluated.ResourceIndexEntry));
            foreach (var patternListResourceIndexEntry in package.FindAll(x => x.ResourceType == ResourceUtils.GetResourceType("PTRN")))
            {
                addPatternsByCategory(WrapperDealer.GetResource(0, package, patternListResourceIndexEntry));
            }
            categories.AddRange(patternsByCategory.Keys);
            categories.Sort();
            if (categories.Contains("Old"))
            {
                categories.Remove("Old");
            }
            categories.ForEach(x => categoryListStore.AppendValues(x));
            CategoryComboBox.Model = categoryListStore;
            CategoryComboBox.PackStart(categoryCell, true);
            CategoryComboBox.Active = 0;
            CategoryComboBox.Changed += (sender, e) => setPatternKeysAndPaths(CategoryComboBox.Active);
            PatternIconView.Model = patternListStore;
            PatternIconView.PixbufColumn = 1;
            PatternIconView.TooltipColumn = 0;
            PatternIconView.SelectionChanged += (sender, e) => OKButton.Sensitive = PatternIconView.SelectedItems.Length > 0;
            setPatternKeysAndPaths(0);
            Response += (o, args) =>
                {
                    if (args.ResponseId == ResponseType.Ok)
                    {
                        PatternPath = patternPaths[PatternIconView.SelectedItems[0].Indices[0]];
                        ResourceKey = patternKeys[PatternIconView.SelectedItems[0].Indices[0]];
                    }
                };
        }

        public static void GenerateCache(IPackage package)
        {
            if (System.IO.File.Exists(PatternUtils.CacheFilePath))
            {
                return;
            }
            PatternUtils.GenerateCache(package);
            foreach (var patternImageKvp in PatternUtils.PreloadedPatternImages)
            {
                PreloadedPatternImagePixbufs.Add(patternImageKvp.Key, patternImageKvp.Value.ToPixbuf());
            }
        }

        public static void LoadCache()
        {
            PatternUtils.LoadCache();
            foreach (var patternImageKvp in PatternUtils.PreloadedPatternImages)
            {
                PreloadedPatternImagePixbufs.Add(patternImageKvp.Key, patternImageKvp.Value.ToPixbuf());
            }
        }
    }
}
