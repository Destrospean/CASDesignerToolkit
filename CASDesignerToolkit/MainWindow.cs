using System;
using System.Collections.Generic;
using System.IO;
using Destrospean.CmarNYCBorrowed;
using Destrospean.Common;
using Destrospean.Common.Abstractions;
using Destrospean.DestrospeanCASPEditor;
using Destrospean.DestrospeanCASPEditor.Widgets;
using Destrospean.Graphics.OpenGL;
using Destrospean.S3PIExtensions;
using Gtk;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3pi.WrapperDealer;

public partial class MainWindow : RendererMainWindow
{
    System.Drawing.Bitmap mAlphaCheckerboard;

    SizeAllocatedHandler mGLWidgetSizeAllocatedHandler;

    readonly string mOriginalWindowTitle;

    PresetNotebook mPresetNotebook;

    SwitchPageHandler mResourcePropertyNotebookSwitchPageHandler;

    string mSaveAsPath;

    public IPackage CurrentPackage;

    public override NextStateOptions NextState
    {
        set
        {
            if (value == NextStateOptions.UnsavedChangesAndUpdateModels)
            {
                Sim.PreloadedLODsMorphed.Clear();
            }
            if (value.HasFlag(NextStateOptions.UpdateModels))
            {
                GlobalState.Meshes.Clear();
                Sim.LoadGEOMs(mPresetNotebook.CurrentPage == -1 ? 0 : mPresetNotebook.CurrentPage, ResourcePropertyNotebook.CurrentPage, GlobalState.LoadTexture);
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

    public MainWindow() : base(WindowType.Toplevel)
    {
        Build();
        mOriginalWindowTitle = Title;
        RescaleAndReposition();
        BuildResourceTable();
        new System.Threading.Thread(ChoosePatternDialog.LoadCache).Start();
        if (!File.Exists(PatternUtils.CacheFilePath))
        {
            new CacheGenerationWindow("Please wait as caches are being generated...", this, new Gdk.Pixbuf(System.Reflection.Assembly.GetExecutingAssembly(), "Destrospean.DestrospeanCASPEditor.Icons.CASDesignerToolkit.png"));
        }
        UseAdvancedShadersAction.Active = ApplicationSettings.UseAdvancedOpenGLShaders;
        ResourcePropertyNotebook.RemovePage(0);
        var alphaCheckerboardImageWidget = new Gtk.Image(((System.Drawing.Bitmap)mAlphaCheckerboard.Clone(new System.Drawing.Rectangle(0, 0, Image.Allocation.Width, Image.Allocation.Height), mAlphaCheckerboard.PixelFormat)).ToPixbuf())
            {
                HeightRequest = Image.HeightRequest,
                WidthRequest = Image.WidthRequest,
                Xalign = 0,
                Yalign = 0
            };
        ImageTable.Attach(alphaCheckerboardImageWidget, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        PrepareGLWidget();
        GLWidget.SetSizeRequest(Image.WidthRequest, Image.HeightRequest);
        ImageTable.Attach(GLWidget, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        Image.SizeAllocated += (o, args) =>
            {
                alphaCheckerboardImageWidget.Pixbuf = ((System.Drawing.Bitmap)mAlphaCheckerboard.Clone(new System.Drawing.Rectangle(0, 0, Image.Allocation.Width, Image.Allocation.Height), mAlphaCheckerboard.PixelFormat)).ToPixbuf();
                List<Gdk.Pixbuf> pixbufs;
                TreeIter iter;
                TreeModel model;
                var scale = Image.Pixbuf == null ? 1 : (float)Math.Min(Image.Allocation.Width, Image.Allocation.Height) / Math.Min(Image.Pixbuf.Width, Image.Pixbuf.Height);
                if (scale != 1 && ResourceTreeView.Selection.GetSelected(out model, out iter) && ImageUtils.PreloadedImagePixbufs.TryGetValue(((IResourceIndexEntry)model.GetValue(iter, 4)).ReverseEvaluateResourceKey(), out pixbufs))
                {
                    new System.Threading.Thread(() => Application.Invoke((sender, e) => Image.Pixbuf = pixbufs[0].ScaleSimple((int)(Image.Pixbuf.Width * scale), (int)(Image.Pixbuf.Height * scale), Gdk.InterpType.Bilinear))).Start();
                }
            };
        MainHPaned.ShowAll();
        GLWidget.Hide();
    }

    void AddCASTableObjectWidgets(CASTableObject castableObject)
    {
        try
        {
            GlobalState.Meshes.Clear();
            var flagNotebook = new Notebook
                {
                    ShowTabs = false
                };
            HBox buttonHBox = new HBox(false, 0), 
            flagPageButtonHBox = new HBox(false, 0)
                {
                    WidthRequest = Image.Allocation.Width
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
            exportTextureButton = new Button("Export Texture"),
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
            buttonHBox.PackEnd(addPresetButtonAlignment, false, true, 0);
            Destrospean.CmarNYCBorrowed.Action additionalToggleAction = delegate
                {
                    castableObject.ClearCurrentRig();
                    NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                };
            var casPart = castableObject as CASPart;
            if (casPart != null)
            {
                Sim.CurrentCASPart = casPart;
                for (var i = 0; i < 2; i++)
                {
                    flagTables.Add(new Table(2, 3, true));
                    flagNotebook.AppendPage(flagTables[i], new Label());
                }
                flagTables[0].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Clothing Category", additionalToggleAction, casPart.CASPartResource, "ClothingCategory"), 0, 1, 0, 2);
                flagTables[0].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Clothing Type", additionalToggleAction, casPart.CASPartResource, "Clothing"), 1, 2, 0, 2);
                flagTables[0].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Data Type", additionalToggleAction, casPart.CASPartResource, "DataType"), 2, 3, 0, 2);
                flagTables[1].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Age", additionalToggleAction, casPart.CASPartResource.AgeGender, "Age"), 0, 1, 0, 2);
                flagTables[1].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Gender", additionalToggleAction, casPart.CASPartResource.AgeGender, "Gender"), 1, 2, 0, 1);
                flagTables[1].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Species", additionalToggleAction, casPart.CASPartResource.AgeGender, "Species"), 2, 3, 0, 2);
                flagTables[1].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame("Handedness", additionalToggleAction, casPart.CASPartResource.AgeGender, "Handedness"), 1, 2, 1, 2);
            }
            var gameObject = castableObject as GameObject;
            if (gameObject != null)
            {
                var frameCount = 3;
                foreach (var property in gameObject.CatalogResource.GetType().GetProperties())
                {
                    if (frameCount == 3)
                    {
                        flagTables.Add(new Table(2, 3, true));
                        flagNotebook.AppendPage(flagTables[flagTables.Count - 1], new Label());
                        frameCount = 0;
                    }
                    if (property.Name.EndsWith("Flags"))
                    {
                        flagTables[flagTables.Count - 1].Attach(WidgetUtils.GetEnumPropertyCheckButtonsInNewFrame(property.Name, additionalToggleAction, gameObject.CatalogResource, property.Name), (uint)frameCount, (uint)frameCount + 1, 0, 2);
                        frameCount++;
                    }
                }
                if (frameCount == 0)
                {
                    flagNotebook.RemovePage(flagNotebook.NPages - 1);
                    flagTables.RemoveAt(flagTables.Count - 1);
                }
            }
            ResourcePropertyTable.Attach(flagPageVBox, 0, 1, 0, 1);
            mPresetNotebook = PresetNotebook.CreateInstance(castableObject, Image);
            mPresetNotebook.Scrollable = true;
            mPresetNotebook.SwitchPage += (o, args) => NextState = NextStateOptions.UpdateModels;
            ResourcePropertyTable.Attach(mPresetNotebook, 1, 2, 0, 1);
            ResourcePropertyTable.ShowAll();
            BuildLODNotebook(casPart);
        }
        catch (Exception ex)
        {
            ProgramUtils.WriteError(ex);
            throw;
        }
    }

    void BuildLODNotebook(CASPart casPart, int startLODPageIndex = 0, int startGEOMPageIndex = 0)
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
            mResourcePropertyNotebookSwitchPageHandler = (o, args) =>
                {
                    Sim.PreloadedLODsMorphed.Clear();
                    NextState = NextStateOptions.UpdateModels;
                };
            ResourcePropertyNotebook.SwitchPage += mResourcePropertyNotebookSwitchPageHandler;
            foreach (var lodKvp in casPart.LODs)
            {
                var geomNotebook = new Notebook
                    {
                        ShowTabs = false
                    };
                var actionGroup = new ActionGroup("Default");
                Gtk.Action addMeshGroupAction = new Gtk.Action("AddMeshGroupAction", "Add Group", null, Stock.Add),
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
                actionGroup.Add(new Gtk.Action("ExportAction", "Export", null, Stock.SaveAs));
                actionGroup.Add(new Gtk.Action("ImportAction", "Import", null, Stock.Directory));
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
                                <menuitem name='AddMeshGroupAction' action='AddMeshGroupAction'/>
                                <menuitem name='DeleteMeshGroupAction' action='DeleteMeshGroupAction'/>
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
                            </menu>
                        </menubar>
                    </ui>");
                var menuBar = (MenuBar)uiManager.GetWidget("/GEOMPropertiesMenuBar");
                menuBar.PackDirection = PackDirection.Rtl;
                Button nextButton = new Button(new Arrow(ArrowType.Right, ShadowType.None)
                    {
                        Xalign = .5f
                    }),
                prevButton = new Button(new Arrow(ArrowType.Left, ShadowType.None)
                    {
                        Xalign = .5f
                    });
                var pageIndexLabel = new Label
                    {
                        Xalign = .5f
                    };
                nextButton.Clicked += (sender, e) => geomNotebook.NextPage();
                prevButton.Clicked += (sender, e) => geomNotebook.PrevPage();
                Alignment nextButtonAlignment = new Alignment(.5f, .5f, 0, 0),
                prevButtonAlignment = new Alignment(.5f, .5f, 0, 0);
                nextButtonAlignment.Add(nextButton);
                prevButtonAlignment.Add(prevButton);
                geomNotebook.SwitchPage += (o, args) =>
                    {
                        pageIndexLabel.Text = geomNotebook.CurrentPage.ToString();
                        nextButton.Sensitive = geomNotebook.CurrentPage < geomNotebook.NPages - 1;
                        prevButton.Sensitive = geomNotebook.CurrentPage > 0;
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
                                    Name = meshFileType == MeshFileType.GEOM ? "The Sims 3 GEOM Resource" : meshFileType == MeshFileType.OBJ ? "Wavefront OBJ" : meshFileType == MeshFileType.WSO ? "The Sims Resource Workshop Object" : null
                                };
                            fileFilter.AddPattern(meshFileType == MeshFileType.GEOM ? "*.simgeom" : meshFileType == MeshFileType.OBJ ? "*.obj" : meshFileType == MeshFileType.WSO ? "*.wso" : null);
                            fileChooserDialog.AddFilter(fileFilter);
                            if (fileChooserDialog.Run() == (int)ResponseType.Accept)
                            {
                                casPart.ExportMeshGroup(lodKvp.Key, geomNotebook.CurrentPage, meshFileType, fileChooserDialog.Filename, PreloadedData.GEOMs, PreloadedData.VPXYs);
                            }
                            fileChooserDialog.Destroy();
                        }
                        catch (Exception ex)
                        {
                            ProgramUtils.WriteError(ex);
                            throw;
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
                                    Name = meshFileType == MeshFileType.OBJ ? "Wavefront OBJ" : meshFileType == MeshFileType.WSO ? "The Sims Resource Workshop Object" : null
                                };
                            fileFilter.AddPattern(meshFileType == MeshFileType.OBJ ? "*.obj" : meshFileType == MeshFileType.WSO ? "*.wso" : null);
                            fileChooserDialog.AddFilter(fileFilter);
                            if (fileChooserDialog.Run() == (int)ResponseType.Accept)
                            {
                                casPart.ImportMeshGroup(lodKvp.Key, geomNotebook.CurrentPage, meshFileType, fileChooserDialog.Filename, RefreshLODNotebook, PreloadedData.GEOMs, PreloadedData.VPXYs);
                            }
                            fileChooserDialog.Destroy();
                        }
                        catch (Exception ex)
                        {
                            ProgramUtils.WriteError(ex);
                            throw;
                        }
                    };
                addMeshGroupAction.Activated += (sender, e) =>
                    {
                        int selectedGEOMIndex = geomNotebook.CurrentPage,
                        selectedLODIndex = ResourcePropertyNotebook.CurrentPage;
                        casPart.AddMeshGroup(lodKvp.Key, PreloadedData.GEOMs, PreloadedData.VPXYs);
                        casPart.LoadLODs(PreloadedData.GEOMs, PreloadedData.VPXYs);
                        foreach (var child in ResourcePropertyNotebook.Children)
                        {
                            ResourcePropertyNotebook.Remove(child);
                        }
                        BuildLODNotebook(casPart, selectedLODIndex, selectedGEOMIndex + 1);
                        NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                    };
                deleteMeshGroupAction.Activated += (sender, e) =>
                    {
                        int selectedGEOMIndex = geomNotebook.CurrentPage,
                        selectedLODIndex = ResourcePropertyNotebook.CurrentPage;
                        casPart.DeleteMeshGroup(lodKvp.Key, selectedGEOMIndex, PreloadedData.GEOMs, PreloadedData.VPXYs);
                        casPart.LoadLODs(PreloadedData.GEOMs, PreloadedData.VPXYs);
                        foreach (var child in ResourcePropertyNotebook.Children)
                        {
                            ResourcePropertyNotebook.Remove(child);
                        }
                        BuildLODNotebook(casPart, selectedLODIndex, selectedGEOMIndex == 0 ? 0 : selectedGEOMIndex - 1);
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
                                Name = "The Sims 3 GEOM Resource"
                            };
                        fileFilter.AddPattern("*.simgeom");
                        fileChooserDialog.AddFilter(fileFilter);
                        if (fileChooserDialog.Run() == (int)ResponseType.Accept)
                        {
                            try
                            {
                                casPart.ImportMesh(lodKvp.Key, geomNotebook.CurrentPage, fileChooserDialog.Filename, RefreshLODNotebook, PreloadedData.GEOMs, PreloadedData.VPXYs);
                            }
                            catch (Exception ex)
                            {
                                ProgramUtils.WriteError(ex);
                                throw;
                            }
                        }
                        fileChooserDialog.Destroy();
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
                Destrospean.CmarNYCBorrowed.Action changeOtherSlidersAndUpdateModels = delegate
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
                        ModelsNeedUpdated = true;
                    };
                fatnessHScale.ValueChanged += (sender, e) =>
                    {
                        Sim.Fat = fatnessHScale.Value > 0 ? (float)fatnessHScale.Value : 0;
                        Sim.Thin = fatnessHScale.Value < 0 ? (float)-fatnessHScale.Value : 0;
                        changeOtherSlidersAndUpdateModels();
                    };
                fitnessHScale.ValueChanged += (sender, e) =>
                    {
                        Sim.Fit = (float)fitnessHScale.Value;
                        changeOtherSlidersAndUpdateModels();
                    };
                specialHScale.ValueChanged += (sender, e) =>
                    {
                        Sim.Special = (float)specialHScale.Value;
                        changeOtherSlidersAndUpdateModels();
                    };
                var geomPageButtonHBox = new HBox(false, 0);
                geomPageButtonHBox.PackEnd(menuBar, true, true, 4);
                geomPageButtonHBox.PackStart(prevButtonAlignment, false, true, 4);
                geomPageButtonHBox.PackStart(pageIndexLabel, false, true, 4);
                geomPageButtonHBox.PackStart(nextButtonAlignment, false, true, 4);
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var iconSize = WidgetUtils.SmallImageSize << 1;
                geomPageButtonHBox.PackStart(new Image(new Gdk.Pixbuf(assembly, "Destrospean.DestrospeanCASPEditor.Icons.Fatness.png", iconSize, iconSize)), false, true, 4);
                geomPageButtonHBox.PackStart(fatnessHScale, true, true, 4);
                geomPageButtonHBox.PackStart(new Image(new Gdk.Pixbuf(assembly, "Destrospean.DestrospeanCASPEditor.Icons.Fitness.png", iconSize, iconSize)), false, true, 4);
                geomPageButtonHBox.PackStart(fitnessHScale, true, true, 4);
                geomPageButtonHBox.PackStart(new Image(new Gdk.Pixbuf(assembly, "Destrospean.DestrospeanCASPEditor.Icons.BabyBump.png", iconSize, iconSize)), false, true, 4);
                geomPageButtonHBox.PackStart(specialHScale, true, true, 4);
                geomPageButtonHBox.ShowAll();
                var lodPageVBox = new VBox(false, 0);
                lodPageVBox.PackStart(geomPageButtonHBox, false, true, 0);
                lodPageVBox.PackStart(geomNotebook, true, true, 0);
                lodPageVBox.ShowAll();
                ResourcePropertyNotebook.AppendPage(lodPageVBox, new Label("LOD " + lodKvp.Key.ToString()));
                lodKvp.Value.ForEach(x => geomNotebook.AddProperties(CurrentPackage, x, Image));
                if (lodKvp.Value == new List<List<GEOM>>(casPart.LODs.Values)[startLODPageIndex])
                {
                    ResourcePropertyNotebook.CurrentPage = startLODPageIndex;
                    geomNotebook.CurrentPage = startGEOMPageIndex;
                }
            }
        }
        catch (Exception ex)
        {
            ProgramUtils.WriteError(ex);
            throw;
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
                    GlobalState.Meshes.Clear();
                    GLWidget.Hide();
                    Image.Clear();
                    foreach (var child in ResourcePropertyTable.Children)
                    {
                        ResourcePropertyTable.Remove(child);
                    }
                    while (ResourcePropertyNotebook.NPages > 0)
                    {
                        ResourcePropertyNotebook.RemovePage(0);
                    }
                    TreeIter iter;
                    TreeModel model;
                    if (ResourceTreeView.Selection.GetSelected(out model, out iter))
                    {
                        var key = ((IResourceIndexEntry)model.GetValue(iter, 4)).ReverseEvaluateResourceKey();
                        switch ((string)model.GetValue(iter, 0))
                        {
                            case "_IMG":
                                List<Gdk.Pixbuf> pixbufs;
                                if (ImageUtils.PreloadedImagePixbufs.TryGetValue(key, out pixbufs))
                                {
                                    var scale = (float)Math.Min(ImageTable.Allocation.Width, ImageTable.Allocation.Height) / Math.Min(pixbufs[0].Width, pixbufs[0].Height);
                                    Image.Pixbuf = pixbufs[0].ScaleSimple((int)(pixbufs[0].Width * scale), (int)(pixbufs[0].Height * scale), Gdk.InterpType.Bilinear);
                                }
                                break;
                            case "CASP":
                                GLWidget.Show();
                                AddCASTableObjectWidgets(PreloadedData.CASParts[key]);
                                break;
                            case "OBJD":
                                GLWidget.Show();
                                AddCASTableObjectWidgets(PreloadedData.GameObjects[key]);
                                break;
                        }
                    }
                };
        }
        catch (Exception ex)
        {
            ProgramUtils.WriteError(ex);
            throw;
        }
    }

    void RefreshLODNotebook(CASPart casPart, int lodIndex, int geomIndex)
    {
        foreach (var child in ResourcePropertyNotebook.Children)
        {
            ResourcePropertyNotebook.Remove(child);
        }
        BuildLODNotebook(casPart, lodIndex, geomIndex);
        NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
    }

    public void ClearTemporaryData()
    {
        Sim.CurrentCASPart = null;
        mSaveAsPath = null;
        GlobalState.Meshes.Clear();
        PreloadedData.CASParts.Clear();
        PreloadedData.GameObjects.Clear();
        PreloadedData.GEOMs.Clear();
        GlobalState.Materials.Clear();
        PreloadedData.VPXYs.Clear();
        Sim.PreloadedLODsMorphed.Clear();
        GlobalState.DeleteTextures();
        ImageUtils.PreloadedGameImagePixbufs.Clear();
        ImageUtils.PreloadedGameImages.Clear();
        ImageUtils.PreloadedImagePixbufs.Clear();
        ImageUtils.PreloadedImages.Clear();
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
            }
            while (ResourcePropertyNotebook.NPages > 0)
            {
                ResourcePropertyNotebook.RemovePage(0);
            }
            foreach (var action in new Gtk.Action[]
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
                    case "OBJD":
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
                            PreloadedData.GEOMs[key] = new GEOM(new BinaryReader(((APackage)CurrentPackage).GetResource(resourceIndexEntry)));
                        }
                        break;
                    case "OBJD":
                        if (!PreloadedData.GameObjects.ContainsKey(key) || missingResourceKeyIndex > -1)
                        {
                            PreloadedData.GameObjects[key] = new GameObject(CurrentPackage, resourceIndexEntry, PreloadedData.MLODs, PreloadedData.MODLs, PreloadedData.VPXYs);
                        }
                        break;
                    case "VPXY":
                        if (!PreloadedData.VPXYs.ContainsKey(key) || missingResourceKeyIndex > -1)
                        {
                            PreloadedData.VPXYs[key] = (GenericRCOLResource)WrapperDealer.GetResource(0, CurrentPackage, resourceIndexEntry);
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
            foreach (var gameObject in PreloadedData.GameObjects.Values)
            {
                AddCASTableObjectWidgets(gameObject);
            }
            ResourceTreeView.Selection.SelectPath(new TreePath("0"));
        }
        catch (Exception ex)
        {
            ProgramUtils.WriteError(ex);
            throw;
        }
    }

    public override void RescaleAndReposition(bool skipRescale = false)
    {
        try
        {
            var monitorGeometry = Screen.GetMonitorGeometry(Screen.GetMonitorAtWindow(GdkWindow));
            var scaleEnvironmentVariable = Environment.GetEnvironmentVariable("CASDTK_SCALE");
            if (!skipRescale)
            {
                WidgetUtils.Scale = string.IsNullOrEmpty(scaleEnvironmentVariable) ? Platform.IsUnix ? (monitorGeometry.Height < 1080 ? 1080 : monitorGeometry.Height) / 1080f : 1 : float.Parse(scaleEnvironmentVariable, System.Globalization.CultureInfo.InvariantCulture);
                WidgetUtils.WineScaleDenominator = Platform.IsRunningUnderWine ? (float)Screen.Resolution / 96 : 1;
                SetDefaultSize((int)(DefaultWidth * WidgetUtils.Scale), (int)(DefaultHeight * WidgetUtils.Scale));
                foreach (var widget in new Widget[]
                    {
                        Image,
                        ImageTable,
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
            }
            mAlphaCheckerboard = ImageUtils.CreateCheckerboard(monitorGeometry.Width, monitorGeometry.Height, (int)(8 * WidgetUtils.Scale), System.Drawing.Color.FromArgb(191, 191, 191), System.Drawing.Color.FromArgb(127, 127, 127));
            Move(((int)(monitorGeometry.Width / WidgetUtils.WineScaleDenominator) - WidthRequest) >> 1, ((int)(monitorGeometry.Height / WidgetUtils.WineScaleDenominator) - HeightRequest) >> 1);
        }
        catch (Exception ex)
        {
            ProgramUtils.WriteError(ex);
            throw;
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
            foreach (var gameObjectKvp in PreloadedData.GameObjects)
            {
                if (ResourceUtils.MissingResourceKeys.Exists(x => x.ToLowerInvariant() == gameObjectKvp.Key.ToLowerInvariant()))
                {
                    continue;
                }
                gameObjectKvp.Value.SavePresets();
                CurrentPackage.ReplaceResource(CurrentPackage.EvaluateResourceKey(gameObjectKvp.Key).ResourceIndexEntry, gameObjectKvp.Value.CatalogResource);
            }
            foreach (var geometryResourceKvp in PreloadedData.GEOMs)
            {
                var stream = new MemoryStream();
                PreloadedData.GEOMs[geometryResourceKvp.Key].Write(new BinaryWriter(stream));
                var resourceIndexEntry = CurrentPackage.EvaluateResourceKey(geometryResourceKvp.Key).ResourceIndexEntry;
                CurrentPackage.AddResource(resourceIndexEntry, stream, false);
                CurrentPackage.DeleteResource(resourceIndexEntry);
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
            ProgramUtils.WriteError(ex);
            throw;
        }
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
        s3pi.Package.Package.ClosePackage(0, CurrentPackage);
        CurrentPackage = null;
        ResourceUtils.MissingResourceKeys.Clear();
        RefreshWidgets();
        NextState = NextStateOptions.NoUnsavedChanges;
        Title = OriginalWindowTitle;
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
        Application.Quit();
    }

    protected void OnDeleteResourceActionActivated(object sender, EventArgs e)
    {
        TreeIter iter;
        TreeModel model;
        ResourceTreeView.Selection.GetSelected(out model, out iter);
        var resourceIndexEntry = (IResourceIndexEntry)model.GetValue(iter, 4);
        CurrentPackage.DeleteResource(resourceIndexEntry);
        ResourceUtils.MissingResourceKeys.Add(resourceIndexEntry.ReverseEvaluateResourceKey());
        RefreshWidgets(false);
        NextState = NextStateOptions.UnsavedChanges;
    }

    protected void OnGameFoldersActionActivated(object sender, EventArgs e)
    {
        new GameFoldersDialog(this);
    }

    protected void OnImportResourceActionActivated(object sender, EventArgs e)
    {
        var fileChooserDialog = new FileChooserDialog("Import Resource", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
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
                ProgramUtils.WriteError(ex);
                throw;
            }
        }
        fileChooserDialog.Destroy();
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
                Name = "The Sims 3 DBPF Package"
            };
        fileFilter.AddPattern("*.package");
        fileChooserDialog.AddFilter(fileFilter);
        if (fileChooserDialog.Run() == (int)ResponseType.Accept)
        {
            try
            {
                var package = s3pi.Package.Package.OpenPackage(0, fileChooserDialog.Filename, true);
                s3pi.Package.Package.ClosePackage(0, CurrentPackage);
                CurrentPackage = package;
                ResourceUtils.MissingResourceKeys.Clear();
                RefreshWidgets();
                NextState = NextStateOptions.NoUnsavedChanges;
                AddFilePathToWindowTitle(fileChooserDialog.Filename);
            }
            catch (Exception ex)
            {
                ProgramUtils.WriteError(ex);
                throw;
            }
        }
        fileChooserDialog.Destroy();
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
        Application.Quit();
    }

    protected void OnReplaceResourceActionActivated(object sender, EventArgs e)
    {
        var fileChooserDialog = new FileChooserDialog("Replace Resource", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
        if (fileChooserDialog.Run() == (int)ResponseType.Accept)
        {
            try
            {
                TreeIter iter;
                TreeModel model;
                ResourceTreeView.Selection.GetSelected(out model, out iter);
                IResourceIndexEntry resourceIndexEntry = CurrentPackage.GetResourceIndexEntry((IResourceIndexEntry)model.GetValue(iter, 4)),
                tempResourceIndexEntry = CurrentPackage.AddResource(fileChooserDialog.Filename, resourceIndexEntry, false);
                CurrentPackage.ResolveResourceType(tempResourceIndexEntry);
                CurrentPackage.ReplaceResource(resourceIndexEntry, WrapperDealer.GetResource(0, CurrentPackage, tempResourceIndexEntry));
                CurrentPackage.DeleteResource(tempResourceIndexEntry);
                ResourceUtils.MissingResourceKeys.Add(resourceIndexEntry.ReverseEvaluateResourceKey());
                RefreshWidgets(false);
                foreach (var casPartKvp in PreloadedData.CASParts)
                {
                    casPartKvp.Value.AllPresets.ForEach(x => x.RegenerateTexture());
                }
                NextState = NextStateOptions.UnsavedChanges;
            }
            catch (Exception ex)
            {
                ProgramUtils.WriteError(ex);
                throw;
            }
        }
        fileChooserDialog.Destroy();
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
                    //var resourceIndexEntry = (IResourceIndexEntry)ResourceListStore.GetValue(iter, 4);
                    //Console.WriteLine(resourceIndexEntry.ReverseEvaluateResourceKey());
                    break;
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
                Name = "The Sims 3 DBPF Package"
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
    }

    protected void OnUseAdvancedShadersActionToggled(object sender, EventArgs e)
    {
        ApplicationSettings.UseAdvancedOpenGLShaders = UseAdvancedShadersAction.Active;
        GlobalState.ActiveShader = UseAdvancedShadersAction.Active ? "lit_advanced" : "textured";
    }
}
