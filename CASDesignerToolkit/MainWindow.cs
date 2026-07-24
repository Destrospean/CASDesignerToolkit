using System;
using System.Collections.Generic;
using System.Destrospean;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Destrospean.CmarNYCBorrowed;
using Destrospean.Common;
using Destrospean.Common.Abstractions;
using Destrospean.DestrospeanCASPEditor;
using Destrospean.DestrospeanCASPEditor.Widgets;
using Destrospean.Graphics.OpenGL;
using Destrospean.Graphics.OpenGL.Sims3;
using Destrospean.S3PIExtensions;
using Gtk;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3pi.Package;

public partial class MainWindow : RendererMainWindow
{
    readonly List<Tuple<uint, Gtk.Action>> mActionsToRemove = new List<Tuple<uint, Gtk.Action>>();

    Thread mAddMusicThread, mLoadMeshesThread, mPlayMusicThread, mRandomizeCASPartsThread;

    Gdk.Pixbuf mAlphaCheckerboardPixbuf, mBabyBumpPixbuf, mFatnessPixbuf, mFitnessPixbuf;

    AudioPlayer mAudioPlayer;

    bool mDisableUpdateModels = false;

    SizeAllocatedHandler mGLWidgetSizeAllocatedHandler;

    object mLock = new object();

    readonly string mOriginalWindowTitle;

    PresetNotebook mPresetNotebook;

    string mSaveAsPath;

    SwitchPageHandler mResourcePropertyNotebookSwitchPageHandler;

    public IPackage CurrentPackage;

    public Image Image = new Image();

    public override NextStateOptions NextState
    {
        set
        {
            if (value.HasFlag(NextStateOptions.UpdateModels) && !mDisableUpdateModels && GlobalState.GLInitialized)
            {
                GlobalState.CurrentLODIndex = ResourcePropertyNotebook.CurrentPage;
                if (mLoadMeshesThread != null)
                {
                    mLoadMeshesThread.Abort();
                }
                (mLoadMeshesThread = new Thread(() =>
                    {
                        lock (mLock)
                        {
                            foreach (var materialKvp in GlobalState.Materials)
                            {
                                GlobalState.LockedMaterials[materialKvp.Key] = materialKvp.Value;
                            }
                            foreach (var meshKvp in GlobalState.Meshes)
                            {
                                GlobalState.LockedMeshes[meshKvp.Key] = meshKvp.Value;
                            }
                            GlobalState.Locked = true;
                            lock (GlobalState.Lock)
                            {
                                GlobalState.Meshes.Clear();
                                GlobalState.Materials.Clear();
                            }
                            TreeIter iter;
                            TreeModel model;
                            if (ResourceTreeView.Selection.GetSelected(out model, out iter))
                            {
                                switch ((string)model.GetValue(iter, 0))
                                {
                                    case "CASP":
                                        Sim.LoadMeshes(mPresetNotebook.CurrentPage == -1 ? 0 : mPresetNotebook.CurrentPage, ResourcePropertyNotebook.CurrentPage, GlobalState.LoadTexture, (casPartVolume, currentPreset, presetTexture, ambientAndSpecularMapTextures, material, loadTextureCallback) => Application.Invoke((sender, e) => Sim.LoadMeshOnMainThread(casPartVolume, currentPreset, presetTexture, ambientAndSpecularMapTextures, material, loadTextureCallback)));
                                        break;
                                }
                            }
                            GlobalState.Locked = false;
                            foreach (var imageKey in new List<string>(ImageUtils.PreloadedGameImages.Keys))
                            {
                                if (!ImageUtils.PreloadedGameImagePixbufs.ContainsKey(imageKey))
                                {
                                    ImageUtils.PreloadedGameImages[imageKey].Dispose();
                                    ImageUtils.PreloadedGameImages.Remove(imageKey);
                                    Application.Invoke((sender, e) => GlobalState.DeleteTexture(imageKey));
                                }
                            }
                            GlobalState.LockedMaterials.Clear();
                            GlobalState.LockedMeshes.Clear();
                        }
                    })).Start();
            }
            if (value.HasFlag(NextStateOptions.UnsavedChanges))
            {
                Title += HasUnsavedChanges ? "" : " *";
                HasUnsavedChanges = true;
            }
            else if (value == NextStateOptions.NoUnsavedChanges)
            {
                Title = Title.Substring(0, HasUnsavedChanges && Title.EndsWith(" *") ? Title.Length - 2 : Title.Length);
                HasUnsavedChanges = false;
            }
        }
    }

    public override string OriginalWindowTitle
    {
        get
        {
            return mOriginalWindowTitle;
        }
    }

    public readonly ListStore ResourceListStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(IResourceIndexEntry));

    public const string ShortcutDescription = "Create wearable items for Sims in The Sims 3";

    public string ShortcutPath
    {
        get
        {
            return Platform.IsMacOS ? null : Platform.IsWindows ? Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\" + OriginalWindowTitle + ".lnk" : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/applications/" + OriginalWindowTitle + ".desktop";
        }
    }

    public class ApplicationSettings : GlobalState.ApplicationSettings
    {
        const string kCheckForUpdatesAutomaticallyKey = "Check for Updates Automatically",
        kPlayMusicKey = "Play Music";

        public static bool CheckForUpdatesAutomatically
        {
            get
            {
                return Settings == null || !Settings.ContainsKey(kCheckForUpdatesAutomaticallyKey) || (bool)Settings[kCheckForUpdatesAutomaticallyKey];
            }
            set
            {
                if (Settings == null)
                {
                    Settings = new Dictionary<string, object>();
                }
                Settings[kCheckForUpdatesAutomaticallyKey] = value;
                SaveSettings();
            }
        }

        public static bool PlayMusic
        {
            get
            {
                return Settings == null || !Settings.ContainsKey(kPlayMusicKey) || (bool)Settings[kPlayMusicKey];
            }
            set
            {
                if (Settings == null)
                {
                    Settings = new Dictionary<string, object>();
                }
                Settings[kPlayMusicKey] = value;
                SaveSettings();
            }
        }
    }

    public MainWindow(string packagePath = null) : base(WindowType.Toplevel)
    {
        Build();
        mOriginalWindowTitle = Title;
        RescaleAndReposition();
        if (ShortcutPath != null && File.Exists(ShortcutPath))
        {
            CreateShortcutAction.Label = "Delete Shortcut";
            CreateShortcutAction.StockId = Stock.Delete;
        }
        BuildResourceTable();
        new Thread(() =>
            {
                if (!ChoosePatternDialog.LoadCache() && File.Exists(PatternThumbnailCache.Singleton.CacheFilePath))
                {
                    File.Delete(PatternThumbnailCache.Singleton.CacheFilePath);
                }
            }).Start();
        new Thread(() =>
            {
                if (!CASPart.LoadLookupCache() && File.Exists(CASPart.LookupCacheFilePath))
                {
                    File.Delete(CASPart.LookupCacheFilePath);
                }
                if (!ChooseObjectDialog.LoadCache() && File.Exists(CASPartThumbnailCache.Singleton.CacheFilePath))
                {
                    File.Delete(CASPartThumbnailCache.Singleton.CacheFilePath);
                };
            }).Start();
        mAudioPlayer = new AudioPlayer();
        (mAddMusicThread = new Thread(mAudioPlayer.AddMusic)).Start();
        var waitBeforeUpdateCheck = false;
        CacheGenerationWindow.GenerateCachesAction = () =>
            {
                Sensitive = false;
                try
                {
                    if (mAddMusicThread != null && mAddMusicThread.IsAlive)
                    {
                        mAddMusicThread.Join();
                    }
                    ChoosePatternDialog.GenerateCache();
                    CASPart.GenerateLookupCache();
                    ChooseObjectDialog.GenerateCache();
                    mAudioPlayer.Clear();
                    mAudioPlayer.AddMusic();
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
                Sensitive = true;
                waitBeforeUpdateCheck = false;
            };
        new Thread(() =>
            {
                Thread.Sleep(1000);
                if (!File.Exists(PatternThumbnailCache.Singleton.CacheFilePath) || !File.Exists(CASPart.LookupCacheFilePath) || !File.Exists(CASPartThumbnailCache.Singleton.CacheFilePath))
                {
                    waitBeforeUpdateCheck = true;
                    Application.Invoke((sender, e) => new CacheGenerationWindow(this, Icon));
                }
                new Thread(() =>
                    {
                        if (!ApplicationSettings.CheckForUpdatesAutomatically || File.Exists(AppDomain.CurrentDomain.BaseDirectory + "noupdate"))
                        {
                            return;
                        }
                        while (waitBeforeUpdateCheck)
                        {   
                        }
                        try
                        {
                            CheckForUpdates();
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteError(ex);
                        }
                    }).Start();
            }).Start();
        var assembly = Assembly.GetEntryAssembly();
        var iconSize = (int)(32 * WidgetUtils.Scale);
        var treeViewSelectionColor = ResourceTreeView.Style.Base(StateType.Selected);
        mBabyBumpPixbuf = new Gdk.Pixbuf(assembly, "Destrospean.DestrospeanCASPEditor.Icons.BabyBump.png", iconSize, iconSize).Colorize(treeViewSelectionColor);
        mFatnessPixbuf = new Gdk.Pixbuf(assembly, "Destrospean.DestrospeanCASPEditor.Icons.Fatness.png", iconSize, iconSize).Colorize(treeViewSelectionColor);
        mFitnessPixbuf = new Gdk.Pixbuf(assembly, "Destrospean.DestrospeanCASPEditor.Icons.Fitness.png", iconSize, iconSize).Colorize(treeViewSelectionColor);
        CheckForUpdatesAutomaticallyAction.Active = ApplicationSettings.CheckForUpdatesAutomatically;
        PlayMusicAction.Active = ApplicationSettings.PlayMusic;
        UseAdvancedShadersAction.Active = ApplicationSettings.UseAdvancedOpenGLShaders;
        PlayMusicAction.Toggled += (sender, e) =>
            {
                if ((ApplicationSettings.PlayMusic = PlayMusicAction.Active) && !string.IsNullOrEmpty(mAudioPlayer.CurrentMusicModes))
                {
                    (mPlayMusicThread = new Thread(() => mAudioPlayer.PlayMusic(mAudioPlayer.CurrentMusicModes.Split(',')))).Start();
                }
                else
                {
                    mAudioPlayer.Stop();
                    if (mPlayMusicThread != null)
                    {
                        mPlayMusicThread.Abort();
                    }
                }
            };
        ResourcePropertyNotebook.RemovePage(0);
        PrepareGLWidget();
        GLWidget.SetSizeRequest(DrawingArea.WidthRequest, DrawingArea.HeightRequest);
        DrawingAreaTable.Attach(GLWidget, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        Image.SetSizeRequest(1024, 1024);
        DrawingArea.ExposeEvent += (o, args) => DrawImage();
        ScrolledWindow.SizeAllocated += (o, args) =>
            {
                if (ScrolledWindow.Hadjustment.Upper > ScrolledWindow.Hadjustment.PageSize)
                {
                    ResourceTreeView.Style.FontDescription.Size = (int)((ResourceTreeView.Style.FontDescription.Size / Pango.Scale.PangoScale - 1) * Pango.Scale.PangoScale);
                    ChoosePatternDialog.ColumnSpacing -= 1;
                    WidgetUtils.SmallImageSizeBase -= 1;
                    WidgetUtils.DefaultTableColumnSpacingBase -= 1;
                    AdjustFontSizes(this, ResourceTreeView.Style.FontDescription);
                }
            };
        MainHPaned.ShowAll();
        GLWidget.Hide();
        if (packagePath != null)
        {
            mAddMusicThread.Join();
            CurrentPackage = Package.OpenPackage(0, packagePath, true);
            RefreshWidgets();
            AddFilePathToWindowTitle(packagePath);
        }
        if (Platform.IsWindows)
        {
            string currentBuild, displayVersion, productName;
            Platform.Windows.GetWindowsVersion(out productName, out currentBuild, out displayVersion);
            int currentBuildNumber;
            if (int.TryParse(currentBuild, out currentBuildNumber) && currentBuildNumber < 10240)
            {
                Icon = new Gdk.Pixbuf(assembly, "Destrospean.DestrospeanCASPEditor.Icons.CASDesignerToolkit.png", 32, 32);
            }
        }
    }

    void AddCASTableObjectWidgets(CASTableObject castableObject)
    {
        try
        {
            var flagNotebook = new Notebook
                {
                    ShowTabs = false
                };
            HBox buttonHBox = new HBox(false, 0), 
            flagPageButtonHBox = new HBox(false, 0)
                {
                    WidthRequest = DrawingArea.Allocation.Width
                };
            if (mGLWidgetSizeAllocatedHandler != null)
            {
                GLWidget.SizeAllocated -= mGLWidgetSizeAllocatedHandler;
            }
            mGLWidgetSizeAllocatedHandler = (o, args) =>
                {
                    if (flagPageButtonHBox != null)
                    {
                        flagPageButtonHBox.WidthRequest = GLWidget.Allocation.Width;
                    }
                };
            GLWidget.SizeAllocated += mGLWidgetSizeAllocatedHandler;
            var flagPageVBox = new VBox(false, 0);
            var flagTables = new List<Table>();
            flagPageVBox.PackStart(buttonHBox, false, false, 0);
            flagPageVBox.PackStart(flagNotebook, true, true, 0);
            Button addPresetButton = new Button(new Gtk.Image(Stock.Add, IconSize.SmallToolbar)),
            exportTextureButton = new Button("Export Texture")
                {
                    Sensitive = castableObject.AllPresets.Count > 0
                },
            nextButton = new Button(new Arrow(ArrowType.Right, ShadowType.None)
                {
                    Xalign = .5f
                }),
            prevButton = new Button(new Arrow(ArrowType.Left, ShadowType.None)
                {
                    Xalign = .5f
                }),
            resetViewButton = new Button("Reset View");
            addPresetButton.Clicked += (sender, e) => mPresetNotebook.AddPreset();
            exportTextureButton.Clicked += (sender, e) =>
                {
                    var fileChooserDialog = new FileChooserDialog("Export Texture", this, FileChooserAction.Save, "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept);
                    var fileFilter = new FileFilter
                        {
                            Name = "Portable Network Graphics"
                        };
                    fileFilter.AddPattern("*.png");
                    fileChooserDialog.AddFilter(fileFilter);
                    if (fileChooserDialog.Run() == (int)ResponseType.Accept)
                    {
                        castableObject.AllPresets[mPresetNotebook.CurrentPage].Texture.Save(fileChooserDialog.Filename + (fileChooserDialog.Filename.ToLowerInvariant().EndsWith(".png") ? "" : ".png"), System.Drawing.Imaging.ImageFormat.Png);
                    }
                    fileChooserDialog.Destroy();
                    fileChooserDialog.Dispose();
                };
            nextButton.Clicked += (sender, e) => flagNotebook.NextPage();
            prevButton.Clicked += (sender, e) => flagNotebook.PrevPage();
            resetViewButton.Clicked += (sender, e) =>
                {
                    GlobalState.Camera.Orientation = new OpenTK.Vector3((float)Math.PI, 0, 0);
                    GlobalState.Camera.Position = new OpenTK.Vector3(0, 1, 4);
                    GlobalState.CurrentRotation = OpenTK.Vector3.Zero;
                    mFOV = OpenTK.MathHelper.DegreesToRadians(30);
                };
            flagNotebook.SwitchPage += (o, args) =>
                {
                    nextButton.Sensitive = flagNotebook.CurrentPage < flagNotebook.NPages - 1;
                    prevButton.Sensitive = flagNotebook.CurrentPage > 0;
                };
            Alignment addPresetButtonAlignment = new Alignment(.5f, .5f, 0, 0),
            nextButtonAlignment = new Alignment(.5f, .5f, 0, 0),
            prevButtonAlignment = new Alignment(.5f, .5f, 0, 0);
            addPresetButtonAlignment.Add(addPresetButton);
            nextButtonAlignment.Add(nextButton);
            prevButtonAlignment.Add(prevButton);
            flagPageButtonHBox.PackStart(prevButtonAlignment, false, true, 4);
            flagPageButtonHBox.PackStart(nextButtonAlignment, false, true, 4);
            flagPageButtonHBox.PackEnd(resetViewButton, false, true, 4);
            flagPageButtonHBox.PackEnd(exportTextureButton, false, true, 4);
            buttonHBox.PackStart(flagPageButtonHBox, false, true, 0);
            if (castableObject.AllPresets.Count > 0)
            {
                buttonHBox.PackEnd(addPresetButtonAlignment, false, true, 0);
            }
            System.Destrospean.Action additionalToggleAction = delegate
                {
                    NextState = NextStateOptions.UnsavedChanges;
                    castableObject.ClearCurrentRig();
                    RandomizeCASParts();
                };
            var casPart = castableObject as CASPart;
            if (casPart != null)
            {
                var showMaternityPartsOnlyCheckButton = new CheckButton("Maternity Mode")
                    {
                        Active = Sim.ShowMaternityPartsOnly
                    };
                showMaternityPartsOnlyCheckButton.Toggled += (sender, e) =>
                    {
                        Sim.ShowMaternityPartsOnly = showMaternityPartsOnlyCheckButton.Active;
                        RandomizeCASParts();
                    };
                var showMaternityPartsOnlyCheckButtonAlignment = new Alignment(0, 0, 0, 0)
                    {
                        LeftPadding = (uint)(6 * WidgetUtils.Scale)
                    };
                showMaternityPartsOnlyCheckButtonAlignment.Add(showMaternityPartsOnlyCheckButton);
                buttonHBox.PackStart(showMaternityPartsOnlyCheckButtonAlignment, false, true, 0);
                Sim.CurrentCASPart = casPart;
                for (var i = 0; i < 2; i++)
                {
                    flagTables.Add(new Table(2, 3, true));
                    flagNotebook.AppendPage(flagTables[i], new Label());
                }
                flagTables[0].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Clothing Category", additionalToggleAction, casPart.CASPartResource, "ClothingCategory"), 0, 1, 0, 2);
                flagTables[0].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Clothing Type", additionalToggleAction, casPart.CASPartResource, "Clothing"), 1, 2, 0, 2);
                flagTables[0].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Data Type", () =>
                    {
                        foreach (var preset in casPart.AllPresets)
                        {
                            foreach (System.Xml.XmlElement element in ((System.Xml.XmlDocument)preset.GetType().GetField("mXmlDocument", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(preset)).SelectSingleNode("preset").SelectSingleNode("complate").ChildNodes)
                            {
                                if (element.Name.ToLowerInvariant() == "value" && "bodytypeparttype".Contains((element.GetAttribute("key") ?? "").ToLowerInvariant()))
                                {
                                    switch (casPart.CASPartResource.DataType)
                                    {
                                        case CASPartResource.DataTypeFlags.Accessory:
                                        case CASPartResource.DataTypeFlags.Body:
                                        case CASPartResource.DataTypeFlags.Hair:
                                        case CASPartResource.DataTypeFlags.Scalp:
                                            element.SetAttribute("value", casPart.CASPartResource.DataType.ToString());
                                            break;
                                        case CASPartResource.DataTypeFlags.FaceOverlay:
                                            element.SetAttribute("value", "Face");
                                            break;
                                    }
                                }
                            }
                        }
                        NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                    }, casPart.CASPartResource, "DataType"), 2, 3, 0, 2);
                flagTables[1].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Age", additionalToggleAction, casPart.CASPartResource.AgeGender, "Age"), 0, 1, 0, 2);
                flagTables[1].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Gender", additionalToggleAction, casPart.CASPartResource.AgeGender, "Gender"), 1, 2, 0, 1);
                flagTables[1].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Species", additionalToggleAction, casPart.CASPartResource.AgeGender, "Species"), 2, 3, 0, 2);
                flagTables[1].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Handedness", additionalToggleAction, casPart.CASPartResource.AgeGender, "Handedness"), 1, 2, 1, 2);
            }
            ResourcePropertyTable.Attach(flagPageVBox, 0, castableObject.AllPresets.Count == 0 ? 2u : 1u, 0, 1);
            mPresetNotebook = PresetNotebook.CreateInstance(castableObject, Image);
            mPresetNotebook.Scrollable = true;
            mPresetNotebook.SwitchPage += (o, args) => NextState = NextStateOptions.UpdateModels;
            ResourcePropertyTable.Attach(mPresetNotebook, 1, 2, 0, 1);
            ResourcePropertyTable.ShowAll();
            BuildLODNotebook(casPart);
            if (casPart != null)
            {
                var simPreviewAction = new Gtk.Action("SimPreviewAction", "Sim Preview...", null, Stock.Preferences);
                uint simPreviewActionMergeID = UIManager.NewMergeId(),
                simPreviewSeparatorMergeID = UIManager.NewMergeId();
                UIManager.AddUi(simPreviewSeparatorMergeID, "/MainMenuBar/SettingsAction", "SimPreviewSeparator", "SimPreviewSeparator", UIManagerItemType.Separator, false);
                UIManager.AddUi(simPreviewActionMergeID, "/MainMenuBar/SettingsAction", "SimPreviewAction", "SimPreviewAction", UIManagerItemType.Menuitem, false);
                SettingsAction.ActionGroup.Add(simPreviewAction);
                simPreviewAction.Activated += (sender, e) =>
                    {
                        var simPreviewDialog = new SimPreviewDialog(this);
                        if (simPreviewDialog.Run() == (int)ResponseType.Ok)
                        {
                            RandomizeCASParts();
                        }
                        simPreviewDialog.Destroy();
                        simPreviewDialog.Dispose();
                    };
                mActionsToRemove.Add(new Tuple<uint, Gtk.Action>(simPreviewActionMergeID, simPreviewAction));
                mActionsToRemove.Add(new Tuple<uint, Gtk.Action>(simPreviewSeparatorMergeID, null));
            }
        }
        catch (Exception ex)
        {
            Logger.WriteError(ex);
        }
    }

    void BuildLODNotebook(CASPart casPart, int startLODPageIndex = 0, int startMeshGroupPageIndex = 0)
    {
        try
        {
            if (casPart == null)
            {
                return;
            }
            if (mResourcePropertyNotebookSwitchPageHandler != null)
            {
                ResourcePropertyNotebook.SwitchPage -= mResourcePropertyNotebookSwitchPageHandler;
            }
            mResourcePropertyNotebookSwitchPageHandler = (o, args) => NextState = NextStateOptions.UpdateModels;
            ResourcePropertyNotebook.SwitchPage += mResourcePropertyNotebookSwitchPageHandler;
            foreach (var lodKvp in casPart.LODs)
            {
                var meshGroupNotebook = new Notebook
                    {
                        ShowTabs = false
                    };
                var actionGroup = new ActionGroup("Default");
                Gtk.Action addMeshGroupAction = new Gtk.Action("AddMeshGroupAction", "Add Group", null, Stock.Add)
                    {
                        Sensitive = lodKvp.Value.Count > 0
                    },
                deleteMeshGroupAction = new Gtk.Action("DeleteMeshGroupAction", "Delete Group", null, Stock.Delete)
                    {
                        Sensitive = lodKvp.Value.Count > 1
                    },
                exportGEOMAction = new Gtk.Action("ExportGEOMAction", "Export GEOM", null, Stock.SaveAs),
                exportOBJAction = new Gtk.Action("ExportOBJAction", "Export OBJ", null, Stock.SaveAs),
                exportWSOAction = new Gtk.Action("ExportWSOAction", "Export WSO", null, Stock.SaveAs),
                importGEOMAction = new Gtk.Action("ImportGEOMAction", "Import GEOM", null, Stock.Directory),
                importOBJAction = new Gtk.Action("ImportOBJAction", "Import OBJ", null, Stock.Directory),
                importWSOAction = new Gtk.Action("ImportWSOAction", "Import WSO", null, Stock.Directory);
                actionGroup.Add(new Gtk.Action("ExportAction", "Export", null, Stock.SaveAs)
                    {
                        Sensitive = lodKvp.Value.Count > 0
                    });
                actionGroup.Add(new Gtk.Action("ImportAction", "Import", null, Stock.Directory)
                    {
                        Sensitive = lodKvp.Value.Count > 0
                    });
                actionGroup.Add(new Gtk.Action("OptionsAction", "Options"));
                actionGroup.Add(addMeshGroupAction);
                actionGroup.Add(deleteMeshGroupAction);
                actionGroup.Add(exportGEOMAction);
                actionGroup.Add(exportOBJAction);
                actionGroup.Add(exportWSOAction);
                actionGroup.Add(importGEOMAction);
                actionGroup.Add(importOBJAction);
                actionGroup.Add(importWSOAction);
                var uiManager = new UIManager();
                uiManager.InsertActionGroup(actionGroup, 0);
                uiManager.AddUiFromString(@"
                    <ui>
                        <menubar name='GEOMPropertiesMenuBar'>
                            <menu name='OptionsAction' action='OptionsAction'>
                                <menu name='ImportAction' action='ImportAction'>
                                    <menuitem name='ImportGEOMAction' action='ImportGEOMAction'/>
                                    <menuitem name='ImportOBJAction' action='ImportOBJAction'/>
                                    <menuitem name='ImportWSOAction' action='ImportWSOAction'/>
                                </menu>                            
                                <menu name='ExportAction' action='ExportAction'>
                                    <menuitem name='ExportGEOMAction' action='ExportGEOMAction'/>
                                    <menuitem name='ExportOBJAction' action='ExportOBJAction'/>
                                    <menuitem name='ExportWSOAction' action='ExportWSOAction'/>
                                </menu>
                                <separator />
                                <menuitem name='AddMeshGroupAction' action='AddMeshGroupAction'/>
                                <menuitem name='DeleteMeshGroupAction' action='DeleteMeshGroupAction'/>
                            </menu>
                        </menubar>
                    </ui>");
                var menuBar = (MenuBar)uiManager.GetWidget("/GEOMPropertiesMenuBar");
                menuBar.PackDirection = PackDirection.Rtl;
                Button nextButton = new Button(new Arrow(ArrowType.Right, ShadowType.None)
                    {
                        Xalign = .5f
                    })
                    {
                        Sensitive = false
                    },
                prevButton = new Button(new Arrow(ArrowType.Left, ShadowType.None)
                    {
                        Xalign = .5f
                    })
                    {
                        Sensitive = false
                    };
                var pageIndexLabel = new Label
                    {
                        Xalign = .5f
                    };
                nextButton.Clicked += (sender, e) => meshGroupNotebook.NextPage();
                prevButton.Clicked += (sender, e) => meshGroupNotebook.PrevPage();
                Alignment nextButtonAlignment = new Alignment(.5f, .5f, 0, 0),
                prevButtonAlignment = new Alignment(.5f, .5f, 0, 0);
                nextButtonAlignment.Add(nextButton);
                prevButtonAlignment.Add(prevButton);
                meshGroupNotebook.SwitchPage += (o, args) =>
                    {
                        pageIndexLabel.Text = meshGroupNotebook.CurrentPage.ToString();
                        nextButton.Sensitive = meshGroupNotebook.CurrentPage < meshGroupNotebook.NPages - 1;
                        prevButton.Sensitive = meshGroupNotebook.CurrentPage > 0;
                    };
                Action<MeshFileType> exportMeshGroup = (meshFileType) =>
                    {
                        try
                        {
                            switch (meshFileType)
                            {
                                case MeshFileType.GEOM:
                                case MeshFileType.OBJ:
                                case MeshFileType.WSO:
                                    break;
                                default:
                                    return;
                            }
                            var fileChooserDialog = new FileChooserDialog("Export " + meshFileType.ToString(), this, FileChooserAction.Save, "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept);
                            var fileFilter = new FileFilter
                                {
                                    Name = FileTypes.GetName(meshFileType)
                                };
                            fileFilter.AddPattern(meshFileType == MeshFileType.GEOM ? "*.simgeom" : meshFileType == MeshFileType.OBJ ? "*.obj" : meshFileType == MeshFileType.WSO ? "*.wso" : null);
                            fileChooserDialog.AddFilter(fileFilter);
                            if (fileChooserDialog.Run() == (int)ResponseType.Accept)
                            {
                                casPart.ExportMeshGroup(lodKvp.Key, meshGroupNotebook.CurrentPage, meshFileType, fileChooserDialog.Filename, PreloadedData.GEOMs, PreloadedData.VPXYs);
                            }
                            fileChooserDialog.Destroy();
                            fileChooserDialog.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteError(ex);
                        }
                    },
                importMeshGroup = (meshFileType) =>
                    {
                        try
                        {
                            switch (meshFileType)
                            {
                                case MeshFileType.OBJ:
                                case MeshFileType.WSO:
                                    break;
                                default:
                                    return;
                            }
                            var fileChooserDialog = new FileChooserDialog("Import " + meshFileType.ToString(), this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
                            var fileFilter = new FileFilter
                                {
                                    Name = FileTypes.GetName(meshFileType)
                                };
                            fileFilter.AddPattern(meshFileType == MeshFileType.OBJ ? "*.obj" : meshFileType == MeshFileType.WSO ? "*.wso" : null);
                            fileChooserDialog.AddFilter(fileFilter);
                            if (fileChooserDialog.Run() == (int)ResponseType.Accept)
                            {
                                lock (SimBase.Lock)
                                {
                                    casPart.ImportMeshGroup(lodKvp.Key, meshGroupNotebook.CurrentPage, meshFileType, fileChooserDialog.Filename, RefreshLODNotebook, PreloadedData.GEOMs, PreloadedData.VPXYs);
                                }
                            }
                            fileChooserDialog.Destroy();
                            fileChooserDialog.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteError(ex);
                        }
                    };
                addMeshGroupAction.Activated += (sender, e) =>
                    {
                        int selectedLODIndex = ResourcePropertyNotebook.CurrentPage,
                        selectedMeshGroupIndex = meshGroupNotebook.CurrentPage;
                        casPart.AddMeshGroup(lodKvp.Key, PreloadedData.GEOMs, PreloadedData.VPXYs);
                        casPart.LoadLODs(PreloadedData.GEOMs, PreloadedData.VPXYs);
                        foreach (var child in ResourcePropertyNotebook.Children)
                        {
                            ResourcePropertyNotebook.Remove(child);
                            child.Destroy();
                            child.Dispose();
                        }
                        BuildLODNotebook(casPart, selectedLODIndex, selectedMeshGroupIndex + 1);
                        NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                    };
                deleteMeshGroupAction.Activated += (sender, e) =>
                    {
                        int selectedLODIndex = ResourcePropertyNotebook.CurrentPage,
                        selectedMeshGroupIndex = meshGroupNotebook.CurrentPage;
                        casPart.DeleteMeshGroup(lodKvp.Key, selectedMeshGroupIndex, PreloadedData.GEOMs, PreloadedData.VPXYs);
                        casPart.LoadLODs(PreloadedData.GEOMs, PreloadedData.VPXYs);
                        foreach (var child in ResourcePropertyNotebook.Children)
                        {
                            ResourcePropertyNotebook.Remove(child);
                            child.Destroy();
                            child.Dispose();
                        }
                        BuildLODNotebook(casPart, selectedLODIndex, selectedMeshGroupIndex == 0 ? 0 : selectedMeshGroupIndex - 1);
                        NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                    };
                exportGEOMAction.Activated += (sender, e) => exportMeshGroup(MeshFileType.GEOM);
                exportOBJAction.Activated += (sender, e) => exportMeshGroup(MeshFileType.OBJ);
                exportWSOAction.Activated += (sender, e) => exportMeshGroup(MeshFileType.WSO);
                importGEOMAction.Activated += (sender, e) =>
                    {
                        var fileChooserDialog = new FileChooserDialog("Import GEOM", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
                        var fileFilter = new FileFilter
                            {
                                Name = FileTypes.GEOM
                            };
                        fileFilter.AddPattern("*.simgeom");
                        fileChooserDialog.AddFilter(fileFilter);
                        if (fileChooserDialog.Run() == (int)ResponseType.Accept)
                        {
                            try
                            {
                                casPart.ImportMeshGroup(lodKvp.Key, meshGroupNotebook.CurrentPage, fileChooserDialog.Filename, RefreshLODNotebook, PreloadedData.GEOMs, PreloadedData.VPXYs);
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteError(ex);
                            }
                        }
                        fileChooserDialog.Destroy();
                        fileChooserDialog.Dispose();
                    };
                importOBJAction.Activated += (sender, e) => importMeshGroup(MeshFileType.OBJ);
                importWSOAction.Activated += (sender, e) => importMeshGroup(MeshFileType.WSO);
                HScale fatnessHScale = new HScale(-1, 1, .01)
                    {
                        Value = Sim.Fat - Sim.Thin
                    },
                fitnessHScale = new HScale(0, 1, .01)
                    {
                        Value = Sim.Fit
                    },
                specialHScale = new HScale(0, 1, .01)
                    {
                        Value = Sim.Special
                    };
                System.Destrospean.Action changeOtherSliders = delegate
                    {
                        for (var i = 0; i < casPart.LODs.Count; i++)
                        {
                            if (new List<int>(casPart.LODs.Keys)[i] == lodKvp.Key)
                            {
                                continue;
                            }
                            foreach (var child in ((VBox)ResourcePropertyNotebook.GetNthPage(i)).Children)
                            {
                                var hBox = child as HBox;
                                if (hBox != null)
                                {
                                    var hScaleIndex = 0;
                                    foreach (var hBoxChild in hBox.Children)
                                    {
                                        var hScale = hBoxChild as HScale;
                                        if (hScale != null)
                                        {
                                            hScale.Value = hScaleIndex == 0 ? fatnessHScale.Value : hScaleIndex == 1 ? fitnessHScale.Value : specialHScale.Value;
                                            hScaleIndex++;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    };
                fatnessHScale.ValueChanged += (sender, e) =>
                    {
                        Sim.Fat = fatnessHScale.Value > 0 ? (float)fatnessHScale.Value : 0;
                        Sim.Thin = fatnessHScale.Value < 0 ? (float)-fatnessHScale.Value : 0;
                        changeOtherSliders();
                    };
                fitnessHScale.ValueChanged += (sender, e) =>
                    {
                        Sim.Fit = (float)fitnessHScale.Value;
                        changeOtherSliders();
                    };
                specialHScale.ValueChanged += (sender, e) =>
                    {
                        Sim.Special = (float)specialHScale.Value;
                        changeOtherSliders();
                    };
                var meshGroupPageButtonHBox = new HBox(false, 0);
                meshGroupPageButtonHBox.PackEnd(menuBar, true, true, 4);
                meshGroupPageButtonHBox.PackStart(prevButtonAlignment, false, true, 4);
                meshGroupPageButtonHBox.PackStart(pageIndexLabel, false, true, 4);
                meshGroupPageButtonHBox.PackStart(nextButtonAlignment, false, true, 4);
                meshGroupPageButtonHBox.PackStart(new Image(mFatnessPixbuf), false, true, 4);
                meshGroupPageButtonHBox.PackStart(fatnessHScale, true, true, 4);
                meshGroupPageButtonHBox.PackStart(new Image(mFitnessPixbuf), false, true, 4);
                meshGroupPageButtonHBox.PackStart(fitnessHScale, true, true, 4);
                meshGroupPageButtonHBox.PackStart(new Image(mBabyBumpPixbuf), false, true, 4);
                meshGroupPageButtonHBox.PackStart(specialHScale, true, true, 4);
                meshGroupPageButtonHBox.ShowAll();
                var lodPageVBox = new VBox(false, 0);
                lodPageVBox.PackStart(meshGroupPageButtonHBox, false, true, 0);
                lodPageVBox.PackStart(meshGroupNotebook, true, true, 0);
                lodPageVBox.ShowAll();
                ResourcePropertyNotebook.AppendPage(lodPageVBox, new Label("LOD " + lodKvp.Key.ToString()));
                lodKvp.Value.ForEach(x => meshGroupNotebook.AddProperties(CurrentPackage, x.GEOM, casPart.AllPresets[mPresetNotebook.CurrentPage == -1 ? 0 : mPresetNotebook.CurrentPage], Image));
                if (lodKvp.Value == new List<List<CASPart.GEOMAndKey>>(casPart.LODs.Values)[startLODPageIndex])
                {
                    ResourcePropertyNotebook.CurrentPage = startLODPageIndex;
                    meshGroupNotebook.CurrentPage = startMeshGroupPageIndex;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.WriteError(ex);
        }
    }

    void BuildResourceTable()
    {
        try
        {
            CellRendererText groupCell = new CellRendererText(),
            instanceCell = new CellRendererText(),
            tagCell = new CellRendererText(),
            typeCell = new CellRendererText();
            TreeViewColumn groupColumn = new TreeViewColumn
                {
                    Title = "Group"
                },
            instanceColumn = new TreeViewColumn
                {
                    Title = "Instance"
                },
            tagColumn = new TreeViewColumn
                {
                    Title = "Tag"
                },
            typeColumn = new TreeViewColumn
                {
                    Title = "Type"
                };
            tagColumn.PackStart(tagCell, true);
            tagColumn.AddAttribute(tagCell, "text", 0);
            typeColumn.PackStart(typeCell, true);
            typeColumn.AddAttribute(typeCell, "text", 1);
            groupColumn.PackStart(groupCell, true);
            groupColumn.AddAttribute(groupCell, "text", 2);
            instanceColumn.PackStart(instanceCell, true);
            instanceColumn.AddAttribute(instanceCell, "text", 3);
            ResourceTreeView.AppendColumn(tagColumn);
            ResourceTreeView.AppendColumn(typeColumn);
            ResourceTreeView.AppendColumn(groupColumn);
            ResourceTreeView.AppendColumn(instanceColumn);
            ResourceTreeView.Model = ResourceListStore;
            ResourceTreeView.ButtonPressEvent += OnResourceTreeViewButtonPress;
            ResourceTreeView.Selection.Changed += (sender, e) => 
                {
                    while (mActionsToRemove.Count > 0)
                    {
                        UIManager.RemoveUi(mActionsToRemove[0].Item1);
                        if (mActionsToRemove[0].Item2 != null)
                        {
                            SettingsAction.ActionGroup.Remove(mActionsToRemove[0].Item2);
                        }
                        mActionsToRemove.RemoveAt(0);
                    }
                    mDisableUpdateModels = true;
                    GLWidget.Hide();
                    Image.Clear();
                    DrawImage();
                    foreach (var child in ResourcePropertyTable.Children)
                    {
                        ResourcePropertyTable.Remove(child);
                        child.Destroy();
                        child.Dispose();
                    }
                    foreach (var child in ResourcePropertyNotebook.Children)
                    {
                        ResourcePropertyNotebook.Remove(child);
                        child.Destroy();
                        child.Dispose();
                    }
                    TreeIter iter;
                    TreeModel model;
                    if (ResourceTreeView.Selection.GetSelected(out model, out iter))
                    {
                        const string forAllMusicModeCasesCase = "for all music mode cases";
                        string key = ((IResourceIndexEntry)model.GetValue(iter, 4)).ReverseEvaluateResourceKey(),
                        musicModes = null;
                        switch ((string)model.GetValue(iter, 0))
                        {
                            case "_IMG":
                                List<Gdk.Pixbuf> pixbufs;
                                if (ImageUtils.PreloadedImagePixbufs.TryGetValue(key, out pixbufs))
                                {
                                    Image.Pixbuf = pixbufs[0];
                                    DrawImage();
                                }
                                break;
                            case "CASP":
                                musicModes = "music_mode_cas";
                                GLWidget.Show();
                                AddCASTableObjectWidgets(PreloadedData.CASParts[key]);
                                mDisableUpdateModels = false;
                                RandomizeCASParts();
                                goto case forAllMusicModeCasesCase;
                            case "OBJD":
                                musicModes = "music_mode_build,music_mode_buy";
                                GLWidget.Show();
                                mDisableUpdateModels = false;
                                goto case forAllMusicModeCasesCase;
                            case forAllMusicModeCasesCase:
                                if (ApplicationSettings.PlayMusic && musicModes != mAudioPlayer.CurrentMusicModes)
                                {
                                    if (mPlayMusicThread != null)
                                    {
                                        mPlayMusicThread.Abort();
                                    }
                                    (mPlayMusicThread = new Thread(() => mAudioPlayer.PlayMusic(musicModes.Split(',')))).Start();
                                }
                                mAudioPlayer.CurrentMusicModes = musicModes;
                                break;
                        }
                    }
                    AdjustFontSizes(this, Style.FontDescription);
                };
        }
        catch (Exception ex)
        {
            Logger.WriteError(ex);
        }
    }

    void RandomizeCASParts()
    {
        if (mRandomizeCASPartsThread != null)
        {
            mRandomizeCASPartsThread.Abort();
        }
        (mRandomizeCASPartsThread = new Thread(() =>
            {
                lock (mLock)
                {
                    Sim.RandomizeCASParts();
                    Application.Invoke((sender, e) => NextState = NextStateOptions.UpdateModels);
                }
            })).Start();
    }

    void RefreshLODNotebook(CASTableObject castableObject, int lodIndex, int groupIndex)
    {
        foreach (var child in ResourcePropertyNotebook.Children)
        {
            ResourcePropertyNotebook.Remove(child);
            child.Destroy();
            child.Dispose();
        }
        var casPart = castableObject as CASPart;
        if (casPart == null)
        {
        }
        else
        {
            BuildLODNotebook(casPart, lodIndex, groupIndex);
        }
        NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
    }

    public static void AdjustFontSizes(Container container, Pango.FontDescription fontDescription)
    {
        container.ModifyFont(fontDescription);
        foreach (var child in container.Children)
        {
            var childContainer = child as Container;
            if (childContainer == null)
            {
                child.ModifyFont(fontDescription);
            }
            else
            {
                AdjustFontSizes(childContainer, fontDescription);
            }
        }
    }

    public static void CheckForUpdates()
    {
        var assemblyName = Assembly.GetEntryAssembly().GetName();
        string latestReleaseDescription,
        latestReleaseDownloadUrl,
        latestReleaseFilename,
        latestReleaseName,
        localVersion = assemblyName.Version.ToString().Remove(assemblyName.Version.ToString().LastIndexOf('.'));
        if (Updates.CheckForUpdates("Destrospean", assemblyName.Name, localVersion, out latestReleaseName, out latestReleaseDescription, out latestReleaseDownloadUrl, out latestReleaseFilename))
        {
            Application.Invoke((sender, e) =>
                {
                    Singleton.Sensitive = false;
                    var newUpdateDialog = new NewUpdateDialog("An update is available for " + Singleton.OriginalWindowTitle + "!", "v" + localVersion, latestReleaseName, latestReleaseDescription);
                    if (newUpdateDialog.Run() != (int)ResponseType.Apply)
                    {
                        Singleton.Sensitive = true;
                        newUpdateDialog.Destroy();
                        newUpdateDialog.Dispose();
                        return;
                    }
                    newUpdateDialog.Destroy();
                    newUpdateDialog.Dispose();
                    string executablePath = AppDomain.CurrentDomain.BaseDirectory,
                    tempPath = executablePath + "Update" + System.IO.Path.DirectorySeparatorChar;
                    Directory.CreateDirectory(tempPath);
                    File.WriteAllBytes(tempPath + latestReleaseFilename, Updates.GetByteArray(latestReleaseDownloadUrl, assemblyName.Name));
                    if (Platform.IsUnix)
                    {
                        Platform.GetCommandOutput("chmod", "755 \"" + tempPath + latestReleaseFilename + "\"");
                    }
                    using (var process = new Process
                        {
                            StartInfo = new ProcessStartInfo
                                {
                                    CreateNoWindow = true,
                                    FileName = tempPath + latestReleaseFilename,
                                    UseShellExecute = false,
                                    WorkingDirectory = tempPath
                                }
                        })
                    {
                        process.Start();
                        process.WaitForExit();
                    }
                    string updaterFilename = "CASDTKUpdater.exe",
                    downloadedUpdaterPath = tempPath + assemblyName.Name + System.IO.Path.DirectorySeparatorChar + updaterFilename;
                    if (File.Exists(downloadedUpdaterPath))
                    {
                        if (File.Exists(executablePath + updaterFilename))
                        {
                            File.Delete(executablePath + updaterFilename);
                        }
                        File.Move(downloadedUpdaterPath, executablePath + updaterFilename);
                    }
                    if (Platform.IsWindows)
                    {
                        if (File.Exists(executablePath + updaterFilename))
                        {
                            using (var process = new Process
                                {
                                    StartInfo = new ProcessStartInfo
                                        {
                                            Arguments = Process.GetCurrentProcess().Id.ToString(),
                                            CreateNoWindow = true,
                                            FileName = updaterFilename,
                                            UseShellExecute = false,
                                            WorkingDirectory = executablePath
                                        }
                                })
                            {
                                process.Start();
                            }
                        }
                        return;
                    }
                    foreach (var directoryName in Directory.GetDirectories(executablePath))
                    {
                        if (!directoryName.EndsWith("Update"))
                        {
                            Directory.Delete(directoryName, true);
                        }
                    }
                    foreach (var filename in Directory.GetFiles(executablePath))
                    {
                        File.Delete(filename);
                    }
                    foreach (var directoryName in Directory.GetDirectories(executablePath + "Update" + System.IO.Path.DirectorySeparatorChar + assemblyName.Name))
                    {
                        Directory.Move(directoryName, executablePath + directoryName.Substring(directoryName.LastIndexOf(System.IO.Path.DirectorySeparatorChar)));
                    }
                    foreach (var filename in Directory.GetFiles(executablePath + "Update" + System.IO.Path.DirectorySeparatorChar + assemblyName.Name))
                    {
                        File.Move(filename, executablePath + filename.Substring(filename.LastIndexOf(System.IO.Path.DirectorySeparatorChar)));
                    }
                    Directory.Delete(executablePath + "Update", true);
                    using (var process = new Process
                        {
                            StartInfo = new ProcessStartInfo
                                {
                                    CreateNoWindow = true,
                                    FileName = executablePath + (Environment.GetEnvironmentVariable("CASDTK_IMMUTABLE") == "1" ? "start.sh" : assemblyName.Name),
                                    UseShellExecute = false
                                }
                        })
                    {
                        process.Start();
                    }
                    Environment.Exit(0);
                });
        }
    }

    public void ClearTemporaryData()
    {
        lock (GlobalState.Lock)
        {
            mAudioPlayer.CurrentMusicModes = null;
            Sim.CurrentCASPart = null;
            Sim.CASPartOverrides.Clear();
            Sim.CASPartOverridesDisabled.Clear();
            mSaveAsPath = null;
            GlobalState.Meshes.Clear();
            foreach (var key in new List<string>(PreloadedData.CASParts.Keys))
            {
                PreloadedData.CASParts[key].Dispose();
                PreloadedData.CASParts.Remove(key);
            }
            PreloadedData.GEOMs.Clear();
            PreloadedData.VPXYs.Clear();
            GlobalState.Materials.Clear();
            GlobalState.DeleteTextures();
            ImageUtils.DeletePreloadedImages();
            ImageResourceComboBox.DeleteThumbnails();
        }
    }

    public override void DrawImage()
    {
        using (var context = CairoHelper.Create(DrawingArea.GdkWindow))
        {
            using (var surface = SurfaceCreateFromPixbuf(mAlphaCheckerboardPixbuf))
            {
                context.SetSourceSurface(surface, 0, 0);
                context.Paint();
            }
            if (Image.Pixbuf != null)
            {
                var scale = (float)Math.Min(DrawingArea.Allocation.Width, DrawingArea.Allocation.Height) / Math.Min(Image.Pixbuf.Width, Image.Pixbuf.Height);
                context.Scale(scale, scale);
                using (var surface = SurfaceCreateFromPixbuf(Image.Pixbuf))
                {
                    context.SetSourceSurface(surface, 0, 0);
                    context.Paint();
                }
            }
        }
    }

    public void ExportResource(IResourceIndexEntry resourceIndexEntry)
    {
        var fileChooserDialog = new FileChooserDialog("Export Resource", this, FileChooserAction.Save, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
        if (fileChooserDialog.Run() == (int)ResponseType.Accept)
        {
            try
            {
                using (var fileStream = File.Create(fileChooserDialog.Filename))
                {
                    ((APackage)CurrentPackage).GetResource(resourceIndexEntry).CopyTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
        fileChooserDialog.Destroy();
        fileChooserDialog.Dispose();
    }

    public void RefreshWidgets(bool clearTemporaryData = true)
    {
        try
        {
            if (clearTemporaryData)
            {
                ClearTemporaryData();
            }
            Image.Clear();
            ResourceListStore.Clear();
            foreach (var child in ResourcePropertyTable.Children)
            {
                ResourcePropertyTable.Remove(child);
                child.Destroy();
                child.Dispose();
            }
            foreach (var child in ResourcePropertyNotebook.Children)
            {
                ResourcePropertyNotebook.Remove(child);
                child.Destroy();
                child.Dispose();
            }
            foreach (var action in new[]
                {
                    CloseAction,
                    ResourceAction,
                    SaveAction,
                    SaveAsAction
                })
            {
                action.Sensitive = CurrentPackage != null;
            }
            if (!CloseAction.Sensitive)
            {
                return;
            }
            var resourceList = CurrentPackage.GetResourceList;
            resourceList.Sort((a, b) => ResourceUtils.GetResourceTypeTag(a).CompareTo(ResourceUtils.GetResourceTypeTag(b)));
            foreach (var resourceIndexEntry in resourceList.FindAll(x => !x.IsDeleted))
            {
                var tag = ResourceUtils.GetResourceTypeTag(resourceIndexEntry);
                switch (tag)
                {
                    case "_IMG":
                    case "CASP":
                        ResourceListStore.AppendValues(tag, "0x" + resourceIndexEntry.ResourceType.ToString("X8"), "0x" + resourceIndexEntry.ResourceGroup.ToString("X8"), "0x" + resourceIndexEntry.Instance.ToString("X16"), resourceIndexEntry);
                        break;
                }
                var key = resourceIndexEntry.ReverseEvaluateResourceKey();
                var missingResourceKeyIndex = ResourceUtils.MissingResourceKeys.FindIndex(x => x.ToLowerInvariant() == key.ToLowerInvariant());
                switch (tag)
                {
                    case "_IMG":
                        if ((!ImageUtils.PreloadedImagePixbufs.ContainsKey(key) || missingResourceKeyIndex > -1) && CurrentPackage.PreloadImage(resourceIndexEntry, Image))
                        {
                            ImageUtils.PreloadedImagePixbufs[key].Add(ImageUtils.PreloadedImagePixbufs[key][0].ScaleSimple(WidgetUtils.SmallImageSize, WidgetUtils.SmallImageSize, Gdk.InterpType.Bilinear));
                        }
                        break;
                    case "CASP":
                        if (!PreloadedData.CASParts.ContainsKey(key) || missingResourceKeyIndex > -1)
                        {
                            PreloadedData.CASParts[key] = new CASPart(CurrentPackage, resourceIndexEntry, PreloadedData.GEOMs, PreloadedData.VPXYs);
                        }
                        break;
                    case "GEOM":
                        if (!PreloadedData.GEOMs.ContainsKey(key) || missingResourceKeyIndex > -1)
                        {
                            using (var reader = new BinaryReader(((APackage)CurrentPackage).GetResource(resourceIndexEntry)))
                            {
                                PreloadedData.GEOMs[key] = new GEOM(reader);
                            }
                        }
                        break;
                    case "VPXY":
                        if (!PreloadedData.VPXYs.ContainsKey(key) || missingResourceKeyIndex > -1)
                        {
                            PreloadedData.VPXYs[key] = new GenericRCOLResource(0, ((APackage)CurrentPackage).GetResource(resourceIndexEntry));
                        }
                        break;
                }
                if (missingResourceKeyIndex > -1)
                {
                    ResourceUtils.MissingResourceKeys.RemoveAt(missingResourceKeyIndex);
                }
            }
            foreach (var casPart in PreloadedData.CASParts.Values)
            {
                AddCASTableObjectWidgets(casPart);
            }
            ResourceTreeView.Selection.SelectPath(new TreePath("0"));
        }
        catch (Exception ex)
        {
            Logger.WriteError(ex);
        }
    }

    public void ReplaceResource(IResourceIndexEntry resourceIndexEntry)
    {
        var fileChooserDialog = new FileChooserDialog("Replace Resource", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
        if (fileChooserDialog.Run() == (int)ResponseType.Accept)
        {
            try
            {
                CurrentPackage.DeleteResource(resourceIndexEntry);
                var tempResourceIndexEntry = CurrentPackage.AddResource(fileChooserDialog.Filename, resourceIndexEntry, true);
                CurrentPackage.ResolveResourceType(tempResourceIndexEntry);
                ResourceUtils.MissingResourceKeys.Add(tempResourceIndexEntry.ReverseEvaluateResourceKey());
                RefreshWidgets(false);
                foreach (var casPartKvp in PreloadedData.CASParts)
                {
                    casPartKvp.Value.AllPresets.ForEach(x => x.RegenerateTexture());
                }
                NextState = NextStateOptions.UnsavedChanges;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
        fileChooserDialog.Destroy();
        fileChooserDialog.Dispose();
    }

    public override void RescaleAndReposition()
    {
        try
        {
            var monitorGeometry = Screen.GetMonitorGeometry(Screen.GetMonitorAtWindow(GdkWindow));
            var scaleEnvironmentVariable = Environment.GetEnvironmentVariable("CASDTK_SCALE");
            WidgetUtils.Scale = string.IsNullOrEmpty(scaleEnvironmentVariable) ? Platform.IsUnix ? (monitorGeometry.Height < 1080 ? 1080 : monitorGeometry.Height) / 1080f : 1 : float.Parse(scaleEnvironmentVariable, System.Globalization.CultureInfo.InvariantCulture);
            WidgetUtils.WineScaleDenominator = Platform.IsRunningUnderWine ? (float)Screen.Resolution / 96 : 1;
            SetDefaultSize((int)(DefaultWidth * WidgetUtils.Scale), (int)(DefaultHeight * WidgetUtils.Scale));
            foreach (var widget in new Widget[]
                {
                    DrawingArea,
                    DrawingAreaTable,
                    MainHPaned,
                    ResourcePropertyNotebook,
                    ResourcePropertyTable,
                    ResourceTreeView,
                    this
                })
            {
                widget.SetSizeRequest(widget.WidthRequest == -1 ? -1 : (int)(widget.WidthRequest * WidgetUtils.Scale), widget.HeightRequest == -1 ? -1 : (int)(widget.HeightRequest * WidgetUtils.Scale));
            }
            Resize(DefaultWidth, DefaultHeight);
            mAlphaCheckerboardPixbuf = ImageUtils.CreateCheckerboard(monitorGeometry.Width, monitorGeometry.Height, (int)(8 * WidgetUtils.Scale), System.Drawing.Color.FromArgb(191, 191, 191), System.Drawing.Color.FromArgb(127, 127, 127)).ToPixbuf();
            Move(((int)(monitorGeometry.Width / WidgetUtils.WineScaleDenominator) - WidthRequest) >> 1, ((int)(monitorGeometry.Height / WidgetUtils.WineScaleDenominator) - HeightRequest) >> 1);
        }
        catch (Exception ex)
        {
            Logger.WriteError(ex);
        }
    }

    public void SavePackage(string path = null)
    {
        try
        {
            if (string.IsNullOrEmpty(mSaveAsPath))
            {
                mSaveAsPath = path;
            }
            foreach (var casPartKvp in PreloadedData.CASParts)
            {
                if (ResourceUtils.MissingResourceKeys.Exists(x => x.ToLowerInvariant() == casPartKvp.Key.ToLowerInvariant()))
                {
                    continue;
                }
                casPartKvp.Value.SavePresets();
                CurrentPackage.ReplaceResource(CurrentPackage.EvaluateResourceKey(casPartKvp.Key).ResourceIndexEntry, casPartKvp.Value.CASPartResource);
            }
            foreach (var geometryResourceKvp in PreloadedData.GEOMs)
            {
                var stream = new MemoryStream();
                PreloadedData.GEOMs[geometryResourceKvp.Key].Write(new BinaryWriter(stream));
                var evaluated = CurrentPackage.EvaluateResourceKey(geometryResourceKvp.Key);
                if (evaluated.Package == CurrentPackage)
                {
                    CurrentPackage.AddResource(evaluated.ResourceIndexEntry, stream, false);
                    CurrentPackage.DeleteResource(evaluated.ResourceIndexEntry);
                }
            }
            foreach (var vpxyResourceKvp in PreloadedData.VPXYs)
            {
                CurrentPackage.ReplaceResource(CurrentPackage.EvaluateResourceKey(vpxyResourceKvp.Key).ResourceIndexEntry, vpxyResourceKvp.Value);
            }
            CurrentPackage.FindAll(x => !x.IsDeleted && x.Compressed == 0).ForEach(x => x.Compressed = 0xFFFF);
            if (string.IsNullOrEmpty(mSaveAsPath))
            {
                CurrentPackage.SavePackage();
            }
            else
            {
                CurrentPackage.SaveAs(mSaveAsPath);
            }
            NextState = NextStateOptions.NoUnsavedChanges;
        }
        catch (Exception ex)
        {
            Logger.WriteError(ex);
        }
    }

    public static Cairo.ImageSurface SurfaceCreateFromPixbuf(Gdk.Pixbuf pixbuf)
    {
        var surface = new Cairo.ImageSurface(Cairo.Format.ARGB32, pixbuf.Width, pixbuf.Height);
        using (var context = new Cairo.Context(surface))
        {
            CairoHelper.SetSourcePixbuf(context, pixbuf, 0, 0);
            context.Paint();
        }
        return surface;
    }

    protected void OnCheckForUpdatesActionActivated(object sender, EventArgs e)
    {
        CheckForUpdates();
    }

    protected void OnCheckForUpdatesAutomaticallyActionToggled(object sender, EventArgs e)
    {
        ApplicationSettings.CheckForUpdatesAutomatically = CheckForUpdatesAutomaticallyAction.Active;
    }

    protected void OnCloseActionActivated(object sender, EventArgs e)
    {
        if (HasUnsavedChanges)
        {
            switch (GetUnsavedChangesDialogResponseType())
            {
                case ResponseType.No:
                    break;
                case ResponseType.Yes:
                    SavePackage();
                    break;
                default:
                    return;
            }
        }
        mAudioPlayer.Stop();
        if (mPlayMusicThread != null)
        {
            mPlayMusicThread.Abort();
        }
        if (mRandomizeCASPartsThread != null)
        {
            mRandomizeCASPartsThread.Abort();
        }
        Package.ClosePackage(0, CurrentPackage);
        CurrentPackage = null;
        ResourceUtils.MissingResourceKeys.Clear();
        RefreshWidgets();
        NextState = NextStateOptions.NoUnsavedChanges;
        Title = OriginalWindowTitle;
    }

    protected void OnCreateShortcutActionActivated(object sender, EventArgs e)
    {
        try
        {
            if (File.Exists(ShortcutPath))
            {
                File.Delete(ShortcutPath);
                CreateShortcutAction.Label = "Create Shortcut";
                CreateShortcutAction.StockId = Stock.Execute;
                return;
            }
            var assembly = Assembly.GetEntryAssembly();
            if (Platform.IsMacOS)
            {
                return;
            }
            if (Platform.IsWindows)
            {
                Platform.Windows.CreateShortcut(ShortcutPath, assembly.Location, AppDomain.CurrentDomain.BaseDirectory, null, ShortcutDescription);
                Platform.Windows.SetFileAssociation("DBPFPackage", FileTypes.DBPFPackage, ".package", assembly.Location);
            }
            else
            {
                var mimeType = "x-wine-extension-package";
                var assemblyName = assembly.GetName().Name;
                Platform.FreeDesktop.CreateShortcut(ShortcutPath, AppDomain.CurrentDomain.BaseDirectory + (Environment.GetEnvironmentVariable("CASDTK_IMMUTABLE") == "1" ? "start.sh" : assemblyName), AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.BaseDirectory + assemblyName + ".svg", OriginalWindowTitle, ShortcutDescription, new[]
                    {
                        "Game"
                    },
                    new[]
                    {
                        mimeType
                    });
                Platform.FreeDesktop.SetFileAssociation(mimeType, FileTypes.DBPFPackage, "*.package");
            }
            CreateShortcutAction.Label = "Delete Shortcut";
            CreateShortcutAction.StockId = Stock.Delete;
        }
        catch (Exception ex)
        {
            Logger.WriteError(ex);
        }
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        if (HasUnsavedChanges)
        {
            switch (GetUnsavedChangesDialogResponseType())
            {
                case ResponseType.No:
                    break;
                case ResponseType.Yes:
                    SavePackage();
                    break;
                default:
                    a.RetVal = true;
                    return;
            }
        }
        mAudioPlayer.Stop();
        if (mPlayMusicThread != null)
        {
            mPlayMusicThread.Abort();
        }
        if (mRandomizeCASPartsThread != null)
        {
            mRandomizeCASPartsThread.Abort();
        }
        Application.Quit();
    }

    protected void OnDeleteResourceActionActivated(object sender, EventArgs e)
    {
        TreeIter iter;
        TreeModel model;
        ResourceTreeView.Selection.GetSelected(out model, out iter);
        var resourceIndexEntry = CurrentPackage.GetResourceIndexEntry((IResourceIndexEntry)model.GetValue(iter, 4));
        CurrentPackage.DeleteResource(resourceIndexEntry);
        ResourceUtils.MissingResourceKeys.Add(resourceIndexEntry.ReverseEvaluateResourceKey());
        RefreshWidgets(false);
        NextState = NextStateOptions.UnsavedChanges;
    }

    protected void OnExportResourceActionActivated(object sender, EventArgs e)
    {
        TreeIter iter;
        TreeModel model;
        if (ResourceTreeView.Selection.GetSelected(out model, out iter))
        {
            ExportResource(CurrentPackage.GetResourceIndexEntry((IResourceIndexEntry)model.GetValue(iter, 4)));
        }
    }

    protected void OnGameFoldersActionActivated(object sender, EventArgs e)
    {
        new GameFoldersDialog(this);
    }

    protected void OnImportResourceActionActivated(object sender, EventArgs e)
    {
        var fileChooserDialog = new FileChooserDialog("Import Image Resource", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
        var fileFilter = new FileFilter
            {
                Name = "DirectDraw Surface"
            };
        fileFilter.AddPattern("*.dds");
        fileChooserDialog.AddFilter(fileFilter);
        if (fileChooserDialog.Run() == (int)ResponseType.Accept)
        {
            try
            {
                CurrentPackage.ResolveResourceType(CurrentPackage.AddResource(fileChooserDialog.Filename));
                RefreshWidgets(false);
                NextState = NextStateOptions.UnsavedChanges;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
        fileChooserDialog.Destroy();
        fileChooserDialog.Dispose();
    }

    protected void OnOpenActionActivated(object sender, EventArgs e)
    {
        if (HasUnsavedChanges)
        {
            switch (GetUnsavedChangesDialogResponseType())
            {
                case ResponseType.No:
                    break;
                case ResponseType.Yes:
                    SavePackage();
                    break;
                default:
                    return;
            }
        }
        var fileChooserDialog = new FileChooserDialog("Open Package", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
        var fileFilter = new FileFilter
            {
                Name = FileTypes.DBPFPackage
            };
        fileFilter.AddPattern("*.package");
        fileChooserDialog.AddFilter(fileFilter);
        if (fileChooserDialog.Run() == (int)ResponseType.Accept)
        {
            try
            {
                if (mAddMusicThread != null && mAddMusicThread.IsAlive)
                {
                    mAddMusicThread.Join();
                }
                mAudioPlayer.Stop();
                if (mPlayMusicThread != null)
                {
                    mPlayMusicThread.Abort();
                }
                if (mRandomizeCASPartsThread != null)
                {
                    mRandomizeCASPartsThread.Abort();
                }
                var package = Package.OpenPackage(0, fileChooserDialog.Filename, true);
                Package.ClosePackage(0, CurrentPackage);
                CurrentPackage = package;
                ResourceUtils.MissingResourceKeys.Clear();
                RefreshWidgets();
                NextState = NextStateOptions.NoUnsavedChanges;
                AddFilePathToWindowTitle(fileChooserDialog.Filename);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
        fileChooserDialog.Destroy();
        fileChooserDialog.Dispose();
    }

    protected void OnQuitActionActivated(object sender, EventArgs e)
    {
        if (HasUnsavedChanges)
        {
            switch (GetUnsavedChangesDialogResponseType())
            {
                case ResponseType.No:
                    break;
                case ResponseType.Yes:
                    SavePackage();
                    break;
                default:
                    return;
            }
        }
        Destroy();
        mAudioPlayer.Stop();
        if (mPlayMusicThread != null)
        {
            mPlayMusicThread.Abort();
        }
        if (mRandomizeCASPartsThread != null)
        {
            mRandomizeCASPartsThread.Abort();
        }
        Application.Quit();
    }

    protected void OnReplaceResourceActionActivated(object sender, EventArgs e)
    {
        TreeIter iter;
        TreeModel model;
        if (ResourceTreeView.Selection.GetSelected(out model, out iter))
        {
            ReplaceResource(CurrentPackage.GetResourceIndexEntry((IResourceIndexEntry)model.GetValue(iter, 4)));
        }
    }

    [GLib.ConnectBefore]
    protected void OnResourceTreeViewButtonPress(object o, ButtonPressEventArgs args)
    {
        TreeViewColumn column;
        TreeIter iter;
        TreePath path;
        int x, y;
        if (ResourceTreeView.GetPathAtPos((int)args.Event.X, (int)args.Event.Y, out path, out column, out x, out y))
        {
            ResourceListStore.GetIter(out iter, path);
            switch (args.Event.Button)
            {
                case 1:
                    ResourceTreeView.Selection.SelectIter(iter);
                    break;
                case 3:
                    var uiManager = new UIManager();
                    var actionGroup = new ActionGroup("Default");
                    Gtk.Action deleteResourceAction = new Gtk.Action("DeleteResourceAction", "Delete", null, Stock.Delete),
                    exportResourceAction = new Gtk.Action("ExportResourceAction", "Export", null, Stock.SaveAs),
                    replaceResourceAction = new Gtk.Action("ReplaceResourceAction", "Replace", null, Stock.Convert);
                    actionGroup.Add(deleteResourceAction);
                    actionGroup.Add(exportResourceAction);
                    actionGroup.Add(replaceResourceAction);
                    uiManager.InsertActionGroup(actionGroup, 0);
                    uiManager.AddUiFromString(@"
                        <ui>
                            <popup name='ResourcePopup'>
                                <menuitem name='ExportResourceAction' action='ExportResourceAction'/>
                                <menuitem name='ReplaceResourceAction' action='ReplaceResourceAction'/>
                                <menuitem name='DeleteResourceAction' action='DeleteResourceAction'/>
                            </popup>
                        </ui>");
                    var menu = (Menu)uiManager.GetWidget("/ResourcePopup");
                    menu.ShowAll();
                    var resourceIndexEntry = CurrentPackage.GetResourceIndexEntry((IResourceIndexEntry)ResourceListStore.GetValue(iter, 4));
                    deleteResourceAction.Activated += (sender, e) =>
                        {
                            CurrentPackage.DeleteResource(resourceIndexEntry);
                            ResourceUtils.MissingResourceKeys.Add(resourceIndexEntry.ReverseEvaluateResourceKey());
                            RefreshWidgets(false);
                            NextState = NextStateOptions.UnsavedChanges;
                        };
                    exportResourceAction.Activated += (sender, e) => ExportResource(resourceIndexEntry);
                    replaceResourceAction.Activated += (sender, e) => ReplaceResource(resourceIndexEntry);
                    menu.Popup();
                    goto case 1;
            }
        }
        args.RetVal = true;
    }

    protected void OnSaveActionActivated(object sender, EventArgs e)
    {
        SavePackage();
    }

    protected void OnSaveAsActionActivated(object sender, EventArgs e)
    {
        var fileChooserDialog = new FileChooserDialog("Save Package As", this, FileChooserAction.Save, "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept);
        var fileFilter = new FileFilter
            {
                Name = FileTypes.DBPFPackage
            };
        fileFilter.AddPattern("*.package");
        fileChooserDialog.AddFilter(fileFilter);
        if (fileChooserDialog.Run() == (int)ResponseType.Accept)
        {
            var path = fileChooserDialog.Filename + (fileChooserDialog.Filename.ToLowerInvariant().EndsWith(".package") ? "" : ".package");
            SavePackage(path);
            AddFilePathToWindowTitle(path);
        }
        fileChooserDialog.Destroy();
        fileChooserDialog.Dispose();
    }

    protected void OnUseAdvancedShadersActionToggled(object sender, EventArgs e)
    {
        ApplicationSettings.UseAdvancedOpenGLShaders = UseAdvancedShadersAction.Active;
        GlobalState.ActiveShader = UseAdvancedShadersAction.Active ? "lit" : "textured";
    }
}
