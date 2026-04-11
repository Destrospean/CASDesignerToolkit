using System.Collections.Generic;
using System.Destrospean;
using System.Globalization;
using Destrospean.Common.Abstractions;
using Gdk;
using Gtk;

namespace Destrospean.DestrospeanCASPEditor.Widgets
{
    public class PresetNotebook : Notebook
    {
        protected bool mDisableSwitchPage = false;

        protected readonly bool mIsSubNotebook;

        public CASTableObject CASTableObject
        {
            get;
            private set;
        }

        public Gtk.Image Image
        {
            get;
            private set;
        }

        public delegate void InsertComplatePageDelegate(string label, Table table, int index);

        public int LastSelectedPage
        {
            get;
            private set;
        }

        class PageLabelHBox : HBox
        {
            public readonly string PresetID;

            public PageLabelHBox(bool homogeneous, int spacing, string presetID) : base(homogeneous, spacing)
            {
                PresetID = presetID;
            }
        }

        protected PresetNotebook(CASTableObject castableObject, Gtk.Image imageWidget, bool isSubNotebook = false) : base()
        {
            try
            {
                CASTableObject = castableObject;
                Image = imageWidget;
                mIsSubNotebook = isSubNotebook;
                LastSelectedPage = 0;
                SwitchPage += (o, args) =>
                    {
                        if (mDisableSwitchPage || mIsSubNotebook)
                        {
                            return;
                        }
                        /*
                        if (NPages > 1 && CurrentPage == NPages - 1)
                        {
                            CASTableObject.Presets.Add(new Preset(CASTableObject, CASTableObject.AllPresets[LastSelectedPage].XmlFile));
                            ((PresetNotebook)CurrentPageWidget).AddPreset(CASTableObject.AllPresets[CASTableObject.AllPresets.Count - 1]);
                            SetTabLabel(GetNthPage(0), GetPageLabelHBox(-CASTableObject.AllPresets.Count, CASTableObject.DefaultPreset != null));
                            SetTabLabel(CurrentPageWidget, GetPageLabelHBox(CASTableObject.Presets.Count - CASTableObject.AllPresets.Count - 1));
                            AppendPage(new PresetNotebook(CASTableObject, Image, true), new Gtk.Image(Stock.Add, IconSize.SmallToolbar)
                                {
                                    Xalign = Platform.IsWindows ? 1 : .5f
                                });
                            ShowAll();
                            MainWindowBase.Singleton.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                        }
                        */
                        LastSelectedPage = CurrentPage;
                    };
                if (!mIsSubNotebook)
                {
                    PageReordered += (o, args) =>
                        {
                            var newIndex = (int)args.P1 - CASTableObject.AllPresets.Count + CASTableObject.Presets.Count;
                            if (newIndex == -1)
                            {
                                ReorderChild(args.P0, (int)args.P1 + 1);
                                return;
                            }
                            var preset = CASTableObject.Presets.Find(x => x.ID == ((PageLabelHBox)GetTabLabel(args.P0)).PresetID);
                            CASTableObject.Presets.Remove(preset);
                            CASTableObject.Presets.Insert(newIndex, preset);
                            for (var i = CASTableObject.AllPresets.Count - CASTableObject.Presets.Count; i < CASTableObject.AllPresets.Count; i++)
                            {
                                SetTabLabel(GetNthPage(i), GetPageLabelHBox(i - NPages - CASTableObject.AllPresets.Count + CASTableObject.Presets.Count));
                            }
                            Complate.MarkUnsavedChangesCallback();
                        };
                }
            }
            catch (System.Exception ex)
            {
                Logger.WriteError(ex);
                throw;
            }
        }

        protected void AddPropertiesToTable(Table table, Complate complate)
        {
            try
            {
                foreach (var propertyName in complate.PropertyNames)
                {
                    Complate.PropertyMeta propertyMeta;
                    if (!complate.PropertiesTyped.TryGetValue(propertyName, out propertyMeta))
                    {
                        continue;
                    }
                    Widget valueWidget = null;
                    var alignment = new Alignment(0, .5f, 1, 0);
                    var value = complate[propertyName];
                    switch (propertyMeta.Type)
                    {
                        case "bool":
                            var checkButton = new CheckButton
                                {
                                    Active = bool.Parse(value),
                                    UseUnderline = false
                                };
                            checkButton.Toggled += (sender, e) => complate[propertyName] = checkButton.Active.ToString();
                            valueWidget = checkButton;
                            break;
                        case "color":
                            alignment.Xscale = 0;
                            var rgba = System.Array.ConvertAll(value.Split(','), x => (ushort)(float.Parse(x, CultureInfo.InvariantCulture) * ushort.MaxValue));
                            var colorButton = new ColorButton
                                {
                                    Alpha = rgba[3],
                                    Color = new Color
                                        {
                                            Blue = rgba[2],
                                            Green = rgba[1],
                                            Red = rgba[0]
                                        },
                                    UseAlpha = true
                                };
                            colorButton.ColorSet += (sender, e) => complate[propertyName] = string.Join(",", System.Array.ConvertAll(new[]
                                {
                                    colorButton.Color.Red,
                                    colorButton.Color.Green,
                                    colorButton.Color.Blue,
                                    colorButton.Alpha
                                }, x => ((float)x / ushort.MaxValue).ToString("F4", CultureInfo.InvariantCulture)));
                            valueWidget = colorButton;
                            break;
                        case "float":
                            alignment.Xscale = 0;
                            var spinButton = new SpinButton(new Adjustment(float.Parse(value, CultureInfo.InvariantCulture), float.MinValue, float.MaxValue, 1, 10, 0), 0, 4);
                            spinButton.ValueChanged += (sender, e) => complate[propertyName] = spinButton.Value.ToString("F4", CultureInfo.InvariantCulture);
                            valueWidget = spinButton;
                            break;
                        case "pattern":
                            var button = new Button(new Label(value)
                                {
                                    UseUnderline = false,
                                    Xalign = 0
                                });
                            button.Clicked += (sender, e) =>
                                {
                                    var choosePatternDialog = new ChoosePatternDialog(MainWindowBase.Singleton, complate.ParentPackage);
                                    if (choosePatternDialog.Run() == (int)ResponseType.Ok)
                                    {
                                        ((Preset)complate).ReplacePattern(propertyName, choosePatternDialog.ResourceKey);
                                        complate[propertyName] = choosePatternDialog.PatternPath;
                                        var patterns = new List<Pattern>(((Preset)complate).Patterns);
                                        patterns.Sort((a, b) => a.SlotName == "Logo" && b.SlotName != "Logo" ? 1 : a.SlotName != "Logo" && b.SlotName == "Logo" ? -1 : a.SlotName.CompareTo(b.SlotName));
                                        for (var i = 0; i < (mIsSubNotebook ? this : (PresetNotebook)CurrentPageWidget).NPages; i++)
                                        {
                                            var patternTable = (Table)((Viewport)((ScrolledWindow)(mIsSubNotebook ? this : (PresetNotebook)CurrentPageWidget).GetNthPage(i)).Child).Child;
                                            var times = patternTable.Children.Length == 0 ? 2 : 1;
                                            for (var j = 0; j < times; j++)
                                            {
                                                foreach (var child in patternTable.Children)
                                                {
                                                    patternTable.Remove(child);
                                                    child.Destroy();
                                                    child.Dispose();
                                                }
                                                patternTable.NRows = 1;
                                                AddPropertiesToTable(patternTable, i == 0 ? complate : patterns[i - 1]);
                                            }
                                        }
                                    }
                                    choosePatternDialog.Destroy();
                                    choosePatternDialog.Dispose();
                                };
                            valueWidget = button;
                            break;
                        case "string":
                            var entry = new Entry
                                {
                                    Sensitive = false,
                                    Text = value
                                };
                            entry.Changed += (sender, e) => complate[propertyName] = entry.Text;
                            valueWidget = entry;
                            break;
                        case "texture":
                            var comboBox = ImageResourceComboBox.CreateInstance(complate.ParentPackage, value, complate as Preset ?? ((Pattern)complate).Preset, Image, true);
                            var comboBoxLastActive = comboBox.Active;
                            comboBox.Changed += (sender, e) =>
                                {
                                    if (comboBox.Active < comboBox.EntryCount - 1 && comboBoxLastActive != comboBox.Active)
                                    {
                                        comboBoxLastActive = comboBox.Active;
                                        complate[propertyName] = comboBox[comboBox.Active].Label;
                                    }
                                };
                            valueWidget = comboBox;
                            break;
                        case "vec2":
                            var hBox = new HBox();
                            var coordinates = System.Array.ConvertAll(value.Split(','), x => float.Parse(x, CultureInfo.InvariantCulture));
                            var spinButtons = new List<SpinButton>
                                {
                                    new SpinButton(new Adjustment(coordinates[0], float.MinValue, float.MaxValue, 1, 10, 0), 0, 4),
                                    new SpinButton(new Adjustment(coordinates[1], float.MinValue, float.MaxValue, 1, 10, 0), 0, 4)
                                };
                            spinButtons.ForEach(x =>
                                {
                                    x.ValueChanged += (sender, e) => complate[propertyName] = spinButtons[0].Value.ToString("F4", CultureInfo.InvariantCulture) + "," + spinButtons[1].Value.ToString("F4", CultureInfo.InvariantCulture);
                                    hBox.PackStart(x, false, false, 0);
                                });
                            valueWidget = hBox;
                            break;
                    }
                    table.Attach(new Label(propertyName)
                        {
                            UseUnderline = false,
                            Xalign = 0
                        }, 0, 1, table.NRows - 1, table.NRows, AttachOptions.Fill, 0, 0, 0);
                    alignment.Add(valueWidget);
                    table.Attach(alignment, 1, 2, table.NRows - 1, table.NRows, AttachOptions.Expand | AttachOptions.Fill, 0, 0, 0);
                    table.NRows++;
                }
                table.SizeAllocated += (o, args) =>
                    {
                        var maxHeight = 0;
                        foreach (var child in table.Children)
                        {
                            maxHeight = System.Math.Max(child.Allocation.Height, maxHeight);
                        }
                        foreach (var child in table.Children)
                        {
                            child.HeightRequest = maxHeight;
                        }
                    };
                table.ShowAll();
            }
            catch (System.Exception ex)
            {
                Logger.WriteError(ex);
                throw;
            }
        }

        protected HBox GetPageLabelHBox(int pageIndexOffset = 0, bool isDefault = false)
        {
            try
            {
                var pageIndex = NPages + pageIndexOffset;
                var deleteButton = new Button
                    {
                        Relief = ReliefStyle.None,
                    };
                deleteButton.Add(new Gtk.Image(Stock.Delete, IconSize.Menu));
                deleteButton.Clicked += (sender, e) =>
                    {
                        mDisableSwitchPage = true;
                        CASTableObject.Presets[pageIndex].Dispose();
                        CASTableObject.Presets.RemoveAt(pageIndex);
                        while (NPages > 0)
                        {
                            RemovePage(0);
                        }
                        if (CASTableObject.DefaultPreset != null)
                        {
                            AddPreset(CASTableObject.DefaultPreset, true);
                        }
                        CASTableObject.Presets.ForEach(x => AddPreset(x));
                        CurrentPage = LastSelectedPage -= LastSelectedPage > pageIndex ? 1 : 0;
                        /*
                        CurrentPage = LastSelectedPage < NPages ? LastSelectedPage : NPages - 1;
                        AppendPage(new PresetNotebook(CASTableObject, Image, true), new Gtk.Image(Stock.Add, IconSize.SmallToolbar)
                            {
                                Xalign = Platform.IsWindows ? 1 : .5f
                            });
                        */
                        ShowAll();
                        //LastSelectedPage = CurrentPage;
                        mDisableSwitchPage = false;
                        MainWindowBase.Singleton.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                    };
                var hBox = new PageLabelHBox(false, 0, CASTableObject.AllPresets[pageIndex + CASTableObject.AllPresets.Count - CASTableObject.Presets.Count].ID);
                hBox.PackStart(new Label(isDefault ? "Default" : "Preset " + pageIndex), true, true, 0);
                if (CASTableObject.DefaultPreset == null ? CASTableObject.Presets.Count > 1 : !isDefault)
                {
                    hBox.PackEnd(deleteButton, false, true, 0);
                }
                hBox.ShowAll();
                return hBox;
            }
            catch (System.Exception ex)
            {
                Logger.WriteError(ex);
                throw;
            }
        }

        public void AddGameObjectPreset()
        {
            var casPartPreset = CASTableObject.AllPresets[CurrentPage] as CASPartPreset;
            if (casPartPreset == null)
            {
                CASTableObject.Presets.Add(new GameObjectPreset(CASTableObject, ((GameObjectPreset)CASTableObject.AllPresets[CurrentPage]).MaterialBlock));
                return;
            }
            ((GameObject)CASTableObject).AddCASPartPreset(casPartPreset);
        }

        public void AddPreset()
        {
            if (CASTableObject is CASPart)
            {
                CASTableObject.Presets.Add(new CASPartPreset(CASTableObject, ((CASPartPreset)CASTableObject.AllPresets[CurrentPage]).XmlFile));
            }
            else
            {
                AddGameObjectPreset();
            }
            AddPreset(CASTableObject.AllPresets[CASTableObject.AllPresets.Count - 1]);
            CurrentPage = CASTableObject.AllPresets.Count - 1;
            SetTabLabel(GetNthPage(0), GetPageLabelHBox(-CASTableObject.AllPresets.Count, CASTableObject.DefaultPreset != null));
            SetTabLabel(CurrentPageWidget, GetPageLabelHBox(CASTableObject.Presets.Count - CASTableObject.AllPresets.Count - 1));
            ShowAll();
            MainWindowBase.Singleton.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
        }

        public void AddPreset(Preset preset, bool isDefault = false)
        {
            try
            {
                var subNotebook = mIsSubNotebook ? this : new PresetNotebook(CASTableObject, Image, true);
                if (!mIsSubNotebook)
                {
                    AppendPage(subNotebook, GetPageLabelHBox(CASTableObject.Presets.Count - CASTableObject.AllPresets.Count, isDefault));
                    SetTabReorderable(GetNthPage(NPages - 1), !isDefault);
                }
                var complates = new List<Complate>
                    {
                        preset
                    };
                complates.AddRange(preset.Patterns);
                foreach (var complate in complates)
                {
                    var addPatternSlotName = "Pattern D";
                    InsertComplatePageDelegate insertComplatePage = (label, table, index) =>
                        {
                            var scrolledWindow = new ScrolledWindow();
                            scrolledWindow.AddWithViewport(table);
                            subNotebook.InsertPage(scrolledWindow, new Label(label), index);
                        };
                    var complateAsPreset = complate as Preset;
                    var complateTable = new Table(1, 2, false)
                        {
                            ColumnSpacing = WidgetUtils.DefaultTableColumnSpacing
                        };
                    insertComplatePage(complateAsPreset == null ? ((Pattern)complate).SlotName : "Configuration", complateTable, subNotebook.NPages);
                    if (complateAsPreset != null && complateAsPreset.Patterns.Exists(x => x.SlotName.StartsWith("Pattern ")) && !complateAsPreset.Patterns.Exists(x => x.SlotName == addPatternSlotName))
                    {
                        var addPatternButtonHBox = new HBox(false, 4);
                        addPatternButtonHBox.PackStart(new Gtk.Image(Stock.Add, IconSize.SmallToolbar)
                            {
                                Xalign = 1
                            }, true, true, 0);
                        addPatternButtonHBox.PackStart(new Label("Add " + addPatternSlotName)
                            {
                                Xalign = 0
                            }, true, true, 0);
                        var addPatternButton = new Button(addPatternButtonHBox);
                        addPatternButton.Clicked += (sender, e) =>
                            {
                                var choosePatternDialog = new ChoosePatternDialog(MainWindowBase.Singleton, complate.ParentPackage);
                                if (choosePatternDialog.Run() == (int)ResponseType.Ok)
                                {
                                    complateAsPreset.AddPattern(addPatternSlotName, complate.CASTableObject is CASPart ? "CasRgbaMask" : "ObjectRgbaMask");
                                    complateAsPreset.ReplacePattern(addPatternSlotName, choosePatternDialog.ResourceKey);
                                    complate[addPatternSlotName] = choosePatternDialog.PatternPath;
                                    insertComplatePage(addPatternSlotName, new Table(1, 2, false), complateAsPreset.Patterns.Count);
                                    for (var i = 0; i < subNotebook.NPages; i++)
                                    {
                                        var patternTable = (Table)((Viewport)((ScrolledWindow)subNotebook.GetNthPage(i)).Child).Child;
                                        foreach (var child in patternTable.Children)
                                        {
                                            patternTable.Remove(child);
                                        }
                                        patternTable.NRows = 1;
                                        AddPropertiesToTable(patternTable, i == 0 ? complate : complateAsPreset.Patterns[i - 1]);
                                    }
                                    subNotebook.ReorderChild(subNotebook.GetNthPage(subNotebook.NPages - 1), complateAsPreset.Patterns.FindLastIndex(x => x.SlotName != "Logo"));
                                    ShowAll();
                                }
                                choosePatternDialog.Destroy();
                                choosePatternDialog.Dispose();
                            };
                        complateTable.Attach(addPatternButton, 0, 2, 0, 1);
                        complateTable.NRows++;
                    }
                    bool swapped;
                    for (var i = 0; i < subNotebook.NPages - 1; i++)
                    {
                        swapped = false;
                        for (var j = 0; j < subNotebook.NPages - i - 1; j++)
                        {
                            string a = subNotebook.GetTabLabelText(subNotebook.GetNthPage(j)),
                            b = subNotebook.GetTabLabelText(subNotebook.GetNthPage(j + 1));
                            if (string.Compare(a, b) == 1 && b != "Logo" || a == "Logo")
                            {
                                subNotebook.ReorderChild(subNotebook.GetNthPage(j), j + 1);
                                swapped = true;
                            }
                        }
                        if (!swapped)
                        {
                            break;
                        }
                    }
                    AddPropertiesToTable(complateTable, complate);
                }
                ShowAll();
            }
            catch (System.Exception ex)
            {
                Logger.WriteError(ex);
                throw;
            }
        }

        public static PresetNotebook CreateInstance(CASTableObject castableObject, Gtk.Image imageWidget)
        {
            try
            {
                var notebook = new PresetNotebook(castableObject, imageWidget);
                if (castableObject.DefaultPreset != null)
                {
                    notebook.AddPreset(castableObject.DefaultPreset, true);
                }
                castableObject.Presets.ForEach(x => notebook.AddPreset(x));
                /*
                notebook.AppendPage(new PresetNotebook(castableObject, imageWidget, true), new Gtk.Image(Stock.Add, IconSize.SmallToolbar)
                    {
                        Xalign = Platform.IsWindows ? 1 : .5f
                    });
                */
                notebook.ShowAll();
                return notebook;
            }
            catch (System.Exception ex)
            {
                Logger.WriteError(ex);
                throw;
            }
        }
    }
}
