using System.Collections.Generic;
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
                            MainWindow.Singleton.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                        }
                        */
                        LastSelectedPage = CurrentPage;
                    };
            }
            catch (System.Exception ex)
            {
                Common.ProgramUtils.WriteError(ex);
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
                            colorButton.ColorSet += (sender, e) => complate[propertyName] = string.Join(",", System.Array.ConvertAll(new ushort[]
                                {
                                    colorButton.Color.Red,
                                    colorButton.Color.Green,
                                    colorButton.Color.Blue,
                                    colorButton.Alpha
                                }, x => (float)x / ushort.MaxValue));
                            valueWidget = colorButton;
                            break;
                        case "float":
                            alignment.Xscale = 0;
                            var spinButton = new SpinButton(new Adjustment(float.Parse(value, CultureInfo.InvariantCulture), float.MinValue, float.MaxValue, .0001, 10, 0), 0, 4);
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
                                        for (var i = 0; i < (mIsSubNotebook ? this : (PresetNotebook)CurrentPageWidget).NPages; i++)
                                        {
                                            var patternTable = (Table)((Viewport)((ScrolledWindow)(mIsSubNotebook ? this : (PresetNotebook)CurrentPageWidget).GetNthPage(i)).Child).Child;
                                            foreach (var child in patternTable.Children)
                                            {
                                                patternTable.Remove(child);
                                            }
                                            patternTable.NRows = 1;
                                            AddPropertiesToTable(patternTable, i == 0 ? complate : ((Preset)complate).Patterns[i - 1]);
                                        }
                                    }
                                    choosePatternDialog.Destroy();
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
                                    new SpinButton(new Adjustment(coordinates[0], float.MinValue, float.MaxValue, .0001, 10, 0), 0, 4),
                                    new SpinButton(new Adjustment(coordinates[1], float.MinValue, float.MaxValue, .0001, 10, 0), 0, 4)
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
                Common.ProgramUtils.WriteError(ex);
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
                var hBox = new HBox(false, 0);
                hBox.PackStart(new Label(isDefault ? "Default" : "Preset " + pageIndex.ToString()), true, true, 0);
                if (CASTableObject.DefaultPreset == null ? CASTableObject.Presets.Count > 1 : !isDefault)
                {
                    hBox.PackEnd(deleteButton, false, true, 0);
                }
                hBox.ShowAll();
                return hBox;
            }
            catch (System.Exception ex)
            {
                Common.ProgramUtils.WriteError(ex);
                throw;
            }
        }

        public void AddGameObjectPreset()
        {
            CASTableObject.Presets.Add(new GameObjectPreset(CASTableObject, ((GameObjectPreset)CASTableObject.AllPresets[CurrentPage]).MaterialBlock));
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
                    if (complateAsPreset != null && !complateAsPreset.Patterns.Exists(x => x.SlotName == addPatternSlotName))
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
                                    ShowAll();
                                }
                                choosePatternDialog.Destroy();
                            };
                        complateTable.Attach(addPatternButton, 0, 2, 0, 1);
                        complateTable.NRows++;
                    }
                    AddPropertiesToTable(complateTable, complate);
                }
                ShowAll();
            }
            catch (System.Exception ex)
            {
                Common.ProgramUtils.WriteError(ex);
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
                Common.ProgramUtils.WriteError(ex);
                throw;
            }
        }
    }
}
