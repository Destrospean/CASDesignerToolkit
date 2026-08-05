using System;
using System.Collections.Generic;
using System.Destrospean;
using Destrospean.CmarNYCBorrowed;
using Destrospean.Common;
using Destrospean.Common.Abstractions;
using Destrospean.DestrospeanCASPEditor.Widgets;
using Destrospean.Graphics.OpenGL;
using Destrospean.S3PIExtensions;
using Destrospean.zoeoeBorrowed;
using Gdk;
using Gtk;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using Shader = Destrospean.CmarNYCBorrowed.Shader;

namespace Destrospean.DestrospeanCASPEditor
{
    public static class WidgetUtils
    {
        public delegate int Comparison<T>(T a, T b);

        public static uint DefaultTableColumnSpacing
        {
            get
            {
                return (uint)(DefaultTableColumnSpacingBase * Scale);
            }
        }

        public static int DefaultTableColumnSpacingBase = 6, SmallImageSizeBase = 16;

        public static float Scale, WineScaleDenominator;

        public static int SmallImageSize
        {
            get
            {
                return (int)(SmallImageSizeBase * Scale);
            }
        }

        public static void AddProperties(this Notebook notebook, IPackage package, GEOM geometryResource, Preset preset, Gtk.Image imageWidget, int pageIndexOffset = 0)
        {
            try
            {
                var scrolledWindow = new ScrolledWindow();
                var table = new Table(1, 2, false)
                    {
                        ColumnSpacing = DefaultTableColumnSpacing
                    };
                scrolledWindow.AddWithViewport(table);
                notebook.AppendPage(scrolledWindow, new Label("GEOM " + (notebook.NPages + pageIndexOffset).ToString()));
                table.AddProperties(package, geometryResource, preset, scrolledWindow, imageWidget);
                table.SizeAllocated += (o, args) =>
                    {
                        var maxHeight = 0;
                        foreach (var child in table.Children)
                        {
                            maxHeight = Math.Max(child.Allocation.Height, maxHeight);
                        }
                        foreach (var child in table.Children)
                        {
                            child.HeightRequest = maxHeight;
                        }
                    };
                notebook.ShowAll();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        public static void AddProperties(this Notebook notebook, string label, IPackage package, LODData lodData, MeshGroupData meshGroupData, uint materialState, Preset preset, Gtk.Image imageWidget, GameObject.UpdateUIDelegate updateUICallback)
        {
            try
            {
                var scrolledWindow = new ScrolledWindow();
                var table = new Table(1, 2, false)
                    {
                        ColumnSpacing = DefaultTableColumnSpacing
                    };
                scrolledWindow.AddWithViewport(table);
                notebook.AppendPage(scrolledWindow, new Label(label));
                table.AddProperties(package, lodData, meshGroupData, materialState, preset, scrolledWindow, imageWidget, updateUICallback);
                table.SizeAllocated += (o, args) =>
                    {
                        var maxHeight = 0;
                        foreach (var child in table.Children)
                        {
                            maxHeight = Math.Max(child.Allocation.Height, maxHeight);
                        }
                        foreach (var child in table.Children)
                        {
                            child.HeightRequest = maxHeight;
                        }
                    };
                notebook.ShowAll();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        public static void AddProperties(this Table table, IPackage package, GEOM geometryResource, Preset preset, ScrolledWindow scrolledWindow, Gtk.Image imageWidget)
        {
            try
            {
                var mainWindow = MainWindowBase.Singleton;
                var geometryResourceKey = "";
                foreach (var geometryResourceKvp in PreloadedData.GEOMs)
                {
                    if (geometryResourceKvp.Value == geometryResource)
                    {
                        geometryResourceKey = geometryResourceKvp.Key;
                        break;
                    }
                }
                var shaders = new List<string>();
                foreach (var shader in Enum.GetNames(typeof(Shader)))
                {
                    shaders.Add(string.Format("{0} ({1})", shader, (uint)Enum.Parse(typeof(Shader), shader)));
                }
                shaders.Sort();
                var shaderComboBoxAlignment = new Alignment(0, .5f, 1, 0);
                var shaderComboBox = new ComboBox(shaders.ToArray())
                    {
                        Active = shaders.IndexOf(string.Format("{0} ({1})", (Shader)geometryResource.ShaderHash, (uint)geometryResource.ShaderHash))
                    };
                shaderComboBoxAlignment.Add(shaderComboBox);
                shaderComboBox.Changed += (sender, e) =>
                    {
                        geometryResource.SetShader((uint)Enum.Parse(typeof(Shader), shaderComboBox.ActiveText.Split(' ')[0]));
                        mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                    };
                table.Attach(new Label("Shader")
                    {
                        Xalign = 0
                    }, 0, 1, table.NRows - 1, table.NRows, AttachOptions.Fill, 0, 0, 0);
                table.Attach(shaderComboBoxAlignment, 1, 2, table.NRows - 1, table.NRows, AttachOptions.Expand | AttachOptions.Fill, 0, 0, 0);
                table.NRows++;
                for (var i = 0; i < geometryResource.Shader.FieldCount; i++)
                {
                    Widget valueWidget = null;
                    var alignment = new Alignment(0, .5f, 0, 0);
                    uint fieldType, valueType;
                    var field = geometryResource.Shader.GetField(i, out fieldType, out valueType);
                    var fieldIndex = i;
                    switch ((MeshFormatDataType)valueType)
                    {
                        case MeshFormatDataType.Float:
                            switch (field.Length)
                            {
                                case 1:
                                    var spinButtonFloat = new SpinButton(new Adjustment((float)field[0], float.MinValue, float.MaxValue, 1, 10, 0), 0, 4);
                                    spinButtonFloat.ValueChanged += (sender, e) =>
                                        {
                                            field[0] = (float)spinButtonFloat.Value;
                                            mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                                        };
                                    valueWidget = spinButtonFloat;
                                    break;
                                case 2:
                                    var hBox = new HBox();
                                    var spinButtons = new List<SpinButton>
                                        {
                                            new SpinButton(new Adjustment((float)field[0], float.MinValue, float.MaxValue, 1, 10, 0), 0, 4),
                                            new SpinButton(new Adjustment((float)field[1], float.MinValue, float.MaxValue, 1, 10, 0), 0, 4)
                                        };
                                    spinButtons[0].ValueChanged += (sender, e) =>
                                        {
                                            field[0] = (float)spinButtons[0].Value;
                                            mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                                        };
                                    spinButtons[1].ValueChanged += (sender, e) =>
                                        {
                                            field[1] = (float)spinButtons[1].Value;
                                            mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                                        };
                                    spinButtons.ForEach(x => hBox.PackStart(x, false, false, 0));
                                    valueWidget = hBox;
                                    break;
                                case 3:
                                    var colorButton = new ColorButton
                                        {
                                            Color = new Color
                                                {
                                                    Blue = (ushort)((float)field[2] * ushort.MaxValue),
                                                    Green = (ushort)((float)field[1] * ushort.MaxValue),
                                                    Red = (ushort)((float)field[0] * ushort.MaxValue)
                                                }
                                        };
                                    colorButton.ColorSet += (sender, e) =>
                                        {
                                            field[0] = (float)colorButton.Color.Red / ushort.MaxValue;
                                            field[1] = (float)colorButton.Color.Green / ushort.MaxValue;
                                            field[2] = (float)colorButton.Color.Blue / ushort.MaxValue;
                                            var color = new OpenTK.Vector3((float)field[0], (float)field[1], (float)field[2]);
                                            Material material;
                                            if (!GlobalState.Materials.TryGetValue(geometryResourceKey, out material))
                                            {
                                                mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                                                return;
                                            }
                                            lock (GlobalState.Lock)
                                            {
                                                switch ((FieldType)fieldType)
                                                {
#pragma warning disable 0618
                                                    case FieldType.Ambient:
#pragma warning restore 0618
                                                        material.AmbientColor = color;
                                                        break;
                                                    case FieldType.Diffuse:
                                                        material.DiffuseColor = color;
                                                        break;
                                                    case FieldType.Specular:
                                                        material.SpecularColor = color;
                                                        break;
                                                }
                                            }
                                            mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                                        };
                                    valueWidget = colorButton;
                                    break;
                                case 4:
                                    var colorButtonWithAlpha = new ColorButton
                                        {
                                            Alpha = (ushort)((float)field[3] * ushort.MaxValue),
                                            Color = new Color
                                                {
                                                    Blue = (ushort)((float)field[2] * ushort.MaxValue),
                                                    Green = (ushort)((float)field[1] * ushort.MaxValue),
                                                    Red = (ushort)((float)field[0] * ushort.MaxValue)
                                                },
                                            UseAlpha = true
                                        };
                                    colorButtonWithAlpha.ColorSet += (sender, e) =>
                                        {
                                            field[0] = (float)colorButtonWithAlpha.Color.Red / ushort.MaxValue;
                                            field[1] = (float)colorButtonWithAlpha.Color.Green / ushort.MaxValue;
                                            field[2] = (float)colorButtonWithAlpha.Color.Blue / ushort.MaxValue;
                                            field[3] = (float)colorButtonWithAlpha.Alpha / ushort.MaxValue;
                                            var color = new OpenTK.Vector3((float)field[0], (float)field[1], (float)field[2]);
                                            Material material;
                                            if (!GlobalState.Materials.TryGetValue(geometryResourceKey, out material))
                                            {
                                                mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                                                return;
                                            }
                                            lock (GlobalState.Lock)
                                            {
                                                switch ((FieldType)fieldType)
                                                {
#pragma warning disable 0618
                                                    case FieldType.Ambient:
#pragma warning restore 0618
                                                        material.AmbientColor = color;
                                                        break;
                                                    case FieldType.Diffuse:
                                                        material.DiffuseColor = color;
                                                        break;
                                                    case FieldType.Specular:
                                                        material.SpecularColor = color;
                                                        break;
                                                }
                                            }
                                            mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                                        };
                                    valueWidget = colorButtonWithAlpha;
                                    break;
                            }
                            break;
                        case MeshFormatDataType.Byte4:
                            var spinButtonInt = new SpinButton(new Adjustment((int)field[0], int.MinValue, int.MaxValue, 1, 10, 0), 0, 0);
                            spinButtonInt.ValueChanged += (sender, e) =>
                                {
                                    field[0] = spinButtonInt.ValueAsInt;
                                    mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                                };
                            valueWidget = spinButtonInt;
                            break;
                        case MeshFormatDataType.Uint:
                            alignment.Xscale = 1;
                            var comboBox = ImageResourceComboBox.CreateInstance(package, new ResourceKey(geometryResource.TGIList[(uint)field[0]].Type, geometryResource.TGIList[(uint)field[0]].Group, geometryResource.TGIList[(uint)field[0]].Instance).ReverseEvaluateResourceKey(), preset, imageWidget);
                            var comboBoxLastActive = comboBox.Active;
                            comboBox.Changed += (sender, e) =>
                                {
                                    if (comboBox.Active == comboBox.EntryCount - 1 || comboBox.Active == comboBoxLastActive)
                                    {
                                        return;
                                    }
                                    comboBoxLastActive = comboBox.Active;
                                    var key = comboBox[comboBox.Active].Label;
                                    var index = Array.FindIndex(geometryResource.TGIList, x => new ResourceKey(x.Type, x.Group, x.Instance).ReverseEvaluateResourceKey() == key);
                                    if (index == -1)
                                    {
                                        var temp = new List<TGI>(geometryResource.TGIList);
                                        var resourceIndexEntry = package.EvaluateImageResourceKey(key).ResourceIndexEntry;
                                        temp.Add(new TGI(resourceIndexEntry.ResourceType, resourceIndexEntry.ResourceGroup, resourceIndexEntry.Instance));
                                        geometryResource.TGIList = temp.ToArray();
                                        index = geometryResource.TGIList.Length - 1;
                                    }
                                    field[0] = (uint)index;
                                    Material material;
                                    if (!GlobalState.Materials.TryGetValue(geometryResourceKey, out material))
                                    {
                                        mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                                        return;
                                    }
                                    lock (GlobalState.Lock)
                                    {
                                        switch ((FieldType)fieldType)
                                        {
                                            case FieldType.AmbientOcclusionMap:
                                                material.AmbientMap = key;
                                                break;
                                            case FieldType.DiffuseMap:
                                                material.DiffuseMap = key;
                                                break;
                                            case FieldType.NormalMap:
                                                material.NormalMap = key;
                                                break;
                                            case FieldType.SpecularMap:
                                                material.SpecularMap = key;
                                                break;
                                        }
                                    }
                                    mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                                };
                            valueWidget = comboBox;
                            break;
                    }
                    var deleteButton = new Button(DefaultTableColumnSpacingBase == 6 ? new Gtk.Image(Stock.Delete, IconSize.Menu) : new Gtk.Image(new Gtk.Image().RenderIcon(Stock.Delete, IconSize.Menu, "").ScaleSimple(SmallImageSize, SmallImageSize, InterpType.Bilinear)))
                        {
                            Relief = ReliefStyle.None
                        };
                    deleteButton.Clicked += (sender, e) =>
                        {
                            geometryResource.Shader.RemoveField(fieldIndex);
                            foreach (var child in table.Children)
                            {
                                table.Remove(child);
                            }
                            table.AddProperties(package, geometryResource, preset, scrolledWindow, imageWidget);
                            table.ShowAll();
                            mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                        };
                    var labelHBox = new HBox(false, 6);
                    labelHBox.PackStart(new Label(((FieldType)fieldType).ToString())
                        {
                            UseUnderline = false,
                            Xalign = 0
                        }, true, true, 0);
                    labelHBox.PackEnd(deleteButton, false, true, 0);
                    table.Attach(labelHBox, 0, 1, table.NRows - 1, table.NRows, AttachOptions.Fill, 0, 0, 0);
                    alignment.Add(valueWidget);
                    table.Attach(alignment, 1, 2, table.NRows - 1, table.NRows, AttachOptions.Expand | AttachOptions.Fill, 0, 0, 0);
                    table.NRows++;
                }
                var addPropertyButtonHBox = new HBox(false, 4);
                addPropertyButtonHBox.PackStart(new Gtk.Image(Stock.Add, IconSize.SmallToolbar)
                    {
                        Xalign = 1
                    }, true, true, 0);
                addPropertyButtonHBox.PackStart(new Label("Add Property")
                    {
                        Xalign = 0
                    }, true, true, 0);
                var addPropertyButton = new Button(addPropertyButtonHBox);
                addPropertyButton.Clicked += (sender, e) =>
                    {
                        var addMaterialPropertyDialog = new AddMaterialPropertyDialog(mainWindow);
                        if (addMaterialPropertyDialog.Run() == (int)ResponseType.Ok)
                        {
                            foreach (var child in table.Children)
                            {
                                table.Remove(child);
                            }
                            geometryResource.Shader.AddField(addMaterialPropertyDialog.Field, (uint)addMaterialPropertyDialog.ValueType, addMaterialPropertyDialog.ValueCount);
                            table.AddProperties(package, geometryResource, preset, scrolledWindow, imageWidget);
                            table.ShowAll();
                            scrolledWindow.Vadjustment.Value = scrolledWindow.Vadjustment.Upper;
                            mainWindow.NextState = NextStateOptions.UnsavedChangesAndUpdateModels;
                        }
                        addMaterialPropertyDialog.Destroy();
                        addMaterialPropertyDialog.Dispose();
                    };
                table.Attach(addPropertyButton, 0, 2, table.NRows - 1, table.NRows, AttachOptions.Fill, 0, 0, 0);
                table.NRows++;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        public static void AddProperties(this Table table, IPackage package, LODData lodData, MeshGroupData meshGroupData, uint materialState, Preset preset, ScrolledWindow scrolledWindow, Gtk.Image imageWidget, GameObject.UpdateUIDelegate updateUICallback)
        {
            try
            {
                var mainWindow = MainWindowBase.Singleton;
                System.Destrospean.Action updateUI = () =>
                    {
                        var gameObject = (GameObject)preset.CASTableObject;
                        updateUICallback(gameObject, new List<LODData>(gameObject.LODs.Values).IndexOf(lodData), lodData.MeshGroups.IndexOf(meshGroupData), materialState);
                    };
                var mlodResource = (GenericRCOLResource)lodData.Resource;
                var matd = mlodResource == null ? null : meshGroupData.MaterialSet == null ? meshGroupData.DirectMATD : mlodResource.ChunkEntries[meshGroupData.MaterialSet.Entries.Find(x => (uint)x.MaterialState == materialState).Index.TGIBlockIndex + mlodResource.PublicChunks].RCOLBlock as MATD;
                var shaders = new List<string>();
                foreach (var shader in Enum.GetNames(typeof(Shader)))
                {
                    shaders.Add(string.Format("{0} ({1})", shader, (uint)Enum.Parse(typeof(Shader), shader)));
                }
                shaders.Sort();
                var shaderComboBoxAlignment = new Alignment(0, .5f, 1, 0);
                var shaderComboBox = new ComboBox(shaders.ToArray())
                    {
                        Active = shaders.IndexOf(string.Format("{0} ({1})", (Shader)matd.Shader, (uint)matd.Shader))
                    };
                shaderComboBoxAlignment.Add(shaderComboBox);
                shaderComboBox.Changed += (sender, e) =>
                    {
                        matd.Shader = (ShaderType)Enum.Parse(typeof(Shader), shaderComboBox.ActiveText.Split(' ')[0]);
                        updateUI();
                    };
                table.Attach(new Label("Shader")
                    {
                        Xalign = 0
                    }, 0, 1, table.NRows - 1, table.NRows, AttachOptions.Fill, 0, 0, 0);
                table.Attach(shaderComboBoxAlignment, 1, 2, table.NRows - 1, table.NRows, AttachOptions.Expand | AttachOptions.Fill, 0, 0, 0);
                table.NRows++;
                foreach (var element in new List<ShaderData>(matd.Mtnf.SData))
                {
                    Widget valueWidget = null;
                    var alignment = new Alignment(0, .5f, 0, 0);
                    var elementFloat = element as ElementFloat;
                    if (elementFloat != null)
                    {
                        var spinButton = new SpinButton(new Adjustment(elementFloat.Data, float.MinValue, float.MaxValue, 1, 10, 0), 0, 4);
                        spinButton.ValueChanged += (sender, e) =>
                            {
                                elementFloat.Data = (float)spinButton.Value;
                                updateUI();
                            };
                        valueWidget = spinButton;
                        goto AttachLabelAndValueWidget;
                    }
                    var elementFloat2 = element as ElementFloat2;
                    if (elementFloat2 != null)
                    {
                        var hBox = new HBox();
                        var spinButtons = new[]
                            {
                                new SpinButton(new Adjustment(elementFloat2.Data0, float.MinValue, float.MaxValue, 1, 10, 0), 0, 4),
                                new SpinButton(new Adjustment(elementFloat2.Data1, float.MinValue, float.MaxValue, 1, 10, 0), 0, 4)
                            };
                        spinButtons[0].ValueChanged += (sender, e) =>
                            {
                                elementFloat2.Data0 = (float)spinButtons[0].Value;
                                updateUI();
                            };
                        spinButtons[1].ValueChanged += (sender, e) =>
                            {
                                elementFloat2.Data1 = (float)spinButtons[1].Value;
                                updateUI();
                            };
                        foreach (var spinButton in spinButtons)
                        {
                            hBox.PackStart(spinButton, false, false, 0);
                        }
                        valueWidget = hBox;
                        goto AttachLabelAndValueWidget;
                    }
                    var elementFloat3 = element as ElementFloat3;
                    if (elementFloat3 != null)
                    {
                        var colorButton = new ColorButton
                            {
                                Color = new Color
                                    {
                                        Blue = (ushort)(elementFloat3.Data2 * ushort.MaxValue),
                                        Green = (ushort)(elementFloat3.Data1 * ushort.MaxValue),
                                        Red = (ushort)(elementFloat3.Data0 * ushort.MaxValue)
                                    }
                            };
                        colorButton.ColorSet += (sender, e) =>
                            {
                                elementFloat3.Data0 = (float)colorButton.Color.Red / ushort.MaxValue;
                                elementFloat3.Data1 = (float)colorButton.Color.Green / ushort.MaxValue;
                                elementFloat3.Data2 = (float)colorButton.Color.Blue / ushort.MaxValue;
                                var color = new OpenTK.Vector3(elementFloat3.Data0, elementFloat3.Data1, elementFloat3.Data2);
                                Material material;
                                if (!GlobalState.Materials.TryGetValue(matd.MaterialNameHash.ToString(), out material))
                                {
                                    updateUI();
                                    return;
                                }
                                lock (GlobalState.Lock)
                                {
                                    switch (element.Field)
                                    {
#pragma warning disable 0618
                                        case FieldType.Ambient:
#pragma warning restore 0618
                                            material.AmbientColor = color;
                                            break;
                                        case FieldType.Diffuse:
                                            material.DiffuseColor = color;
                                            break;
                                        case FieldType.Specular:
                                            material.SpecularColor = color;
                                            break;
                                    }
                                }
                                updateUI();
                            };
                        valueWidget = colorButton;
                        goto AttachLabelAndValueWidget;
                    }
                    var elementFloat4 = element as ElementFloat4;
                    if (elementFloat4 != null)
                    {
                        var colorButton = new ColorButton
                            {
                                Alpha = (ushort)(elementFloat4.Data3 * ushort.MaxValue),
                                Color = new Color
                                    {
                                        Blue = (ushort)(elementFloat4.Data2 * ushort.MaxValue),
                                        Green = (ushort)(elementFloat4.Data1 * ushort.MaxValue),
                                        Red = (ushort)(elementFloat4.Data0 * ushort.MaxValue)
                                    },
                                UseAlpha = true
                            };
                        colorButton.ColorSet += (sender, e) =>
                            {
                                elementFloat4.Data0 = (float)colorButton.Color.Red / ushort.MaxValue;
                                elementFloat4.Data1 = (float)colorButton.Color.Green / ushort.MaxValue;
                                elementFloat4.Data2 = (float)colorButton.Color.Blue / ushort.MaxValue;
                                elementFloat4.Data3 = (float)colorButton.Alpha / ushort.MaxValue;
                                var color = new OpenTK.Vector3(elementFloat4.Data0, elementFloat4.Data1, elementFloat4.Data2);
                                Material material;
                                if (!GlobalState.Materials.TryGetValue(matd.MaterialNameHash.ToString(), out material))
                                {
                                    updateUI();
                                    return;
                                }
                                lock (GlobalState.Lock)
                                {
                                    switch (element.Field)
                                    {
#pragma warning disable 0618
                                        case FieldType.Ambient:
#pragma warning restore 0618
                                            material.AmbientColor = color;
                                            break;
                                        case FieldType.Diffuse:
                                            material.DiffuseColor = color;
                                            break;
                                        case FieldType.Specular:
                                            material.SpecularColor = color;
                                            break;
                                    }
                                }
                                updateUI();
                            };
                        valueWidget = colorButton;
                        goto AttachLabelAndValueWidget;
                    }
                    var elementInt = element as ElementInt;
                    if (elementInt != null)
                    {
                        var spinButton = new SpinButton(new Adjustment(elementInt.Data, int.MinValue, int.MaxValue, 1, 10, 0), 0, 0);
                        spinButton.ValueChanged += (sender, e) =>
                            {
                                elementInt.Data = spinButton.ValueAsInt;
                                updateUI();
                            };
                        valueWidget = spinButton;
                        goto AttachLabelAndValueWidget;
                    }
                    var elementTextureRef = element as ElementTextureRef;
                    if (elementTextureRef != null)
                    {
                        alignment.Xscale = 1;
                        var comboBox = ImageResourceComboBox.CreateInstance(package, mlodResource.Resources[elementTextureRef.Data.TGIBlockIndex].ReverseEvaluateResourceKey(), preset, imageWidget);
                        var comboBoxLastActive = comboBox.Active;
                        comboBox.Changed += (sender, e) =>
                            {
                                if (comboBox.Active == comboBox.EntryCount - 1 || comboBox.Active == comboBoxLastActive)
                                {
                                    return;
                                }
                                comboBoxLastActive = comboBox.Active;
                                var key = comboBox[comboBox.Active].Label;
                                var index = mlodResource.Resources.FindIndex(x => x.ReverseEvaluateResourceKey() == key);
                                if (index == -1)
                                {
                                    mlodResource.Resources.Add(new TGIBlock(0, null, package.EvaluateImageResourceKey(key).ResourceIndexEntry));
                                    index = mlodResource.Resources.Count - 1;
                                }
                                elementTextureRef.Data.TGIBlockIndex = index;
                                Material material;
                                if (!GlobalState.Materials.TryGetValue(matd.MaterialNameHash.ToString(), out material))
                                {
                                    updateUI();
                                    return;
                                }
                                lock (GlobalState.Lock)
                                {
                                    switch (element.Field)
                                    {
                                        case FieldType.AmbientOcclusionMap:
                                            material.AmbientMap = key;
                                            break;
                                        case FieldType.DiffuseMap:
                                            material.DiffuseMap = key;
                                            break;
                                        case FieldType.NormalMap:
                                            material.NormalMap = key;
                                            break;
                                        case FieldType.SpecularMap:
                                            material.SpecularMap = key;
                                            break;
                                    }
                                }
                                updateUI();
                            };
                        valueWidget = comboBox;
                    }
                    AttachLabelAndValueWidget:
                    var deleteButton = new Button(DefaultTableColumnSpacingBase == 6 ? new Gtk.Image(Stock.Delete, IconSize.Menu) : new Gtk.Image(new Gtk.Image().RenderIcon(Stock.Delete, IconSize.Menu, "").ScaleSimple(SmallImageSize, SmallImageSize, InterpType.Bilinear)))
                        {
                            Relief = ReliefStyle.None,
                        };
                    deleteButton.Clicked += (sender, e) =>
                        {
                            matd.Mtnf.SData.Remove(element);
                            foreach (var child in table.Children)
                            {
                                table.Remove(child);
                            }
                            table.AddProperties(package, lodData, meshGroupData, materialState, preset, scrolledWindow, imageWidget, updateUICallback);
                            table.ShowAll();
                            updateUI();
                        };
                    var labelHBox = new HBox(false, 6);
                    labelHBox.PackStart(new Label(element.Field.ToString())
                        {
                            UseUnderline = false,
                            Xalign = 0
                        }, true, true, 0);
                    labelHBox.PackEnd(deleteButton, false, true, 0);
                    table.Attach(labelHBox, 0, 1, table.NRows - 1, table.NRows, AttachOptions.Fill, 0, 0, 0);
                    alignment.Add(valueWidget);
                    table.Attach(alignment, 1, 2, table.NRows - 1, table.NRows, AttachOptions.Expand | AttachOptions.Fill, 0, 0, 0);
                    table.NRows++;
                }
                var addPropertyButtonHBox = new HBox(false, 4);
                addPropertyButtonHBox.PackStart(new Gtk.Image(Stock.Add, IconSize.SmallToolbar)
                    {
                        Xalign = 1
                    }, true, true, 0);
                addPropertyButtonHBox.PackStart(new Label("Add Property")
                    {
                        Xalign = 0
                    }, true, true, 0);
                var addPropertyButton = new Button(addPropertyButtonHBox);
                addPropertyButton.Clicked += (sender, e) =>
                    {
                        var addMaterialPropertyDialog = new AddMaterialPropertyDialog(mainWindow);
                        if (addMaterialPropertyDialog.Run() == (int)ResponseType.Ok)
                        {
                            foreach (var child in table.Children)
                            {
                                table.Remove(child);
                            }
                            var element = addMaterialPropertyDialog.DataType == typeof(ElementTextureRef) ? new ElementTextureRef(0, null) : (ShaderData)Activator.CreateInstance(addMaterialPropertyDialog.DataType, 0, null);
                            element.Field = (FieldType)addMaterialPropertyDialog.Field;
                            matd.Mtnf.SData.Add(element);
                            table.AddProperties(package, lodData, meshGroupData, materialState, preset, scrolledWindow, imageWidget, updateUICallback);
                            table.ShowAll();
                            scrolledWindow.Vadjustment.Value = scrolledWindow.Vadjustment.Upper;
                            updateUI();
                        }
                        addMaterialPropertyDialog.Destroy();
                        addMaterialPropertyDialog.Dispose();
                    };
                table.Attach(addPropertyButton, 0, 2, table.NRows - 1, table.NRows, AttachOptions.Fill, 0, 0, 0);
                table.NRows++;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        public static Frame GetEnumPropertyCheckButtonsInNewFrame(string label, System.Destrospean.Action additionalToggleAction, object propertyHolder, params string[] propertyPathComponents)
        {
            try
            {
                var property = propertyHolder;
                var propertyInfo = property.GetType().GetProperty(propertyPathComponents[0]);
                for (var i = 1; i < propertyPathComponents.Length; i++)
                {
                    property = propertyInfo.GetValue(property, null);
                    propertyInfo = property.GetType().GetProperty(propertyPathComponents[i]);
                }
                var enumInstance = propertyInfo.GetValue(property, null);
                bool disableToggled = false,
                isFlagType = enumInstance.GetType().IsDefined(typeof(FlagsAttribute), false);
                var frame = new Frame
                    {
                        Label = label
                    };
                RadioButton groupRadioButton = null;
                var scrolledWindow = new ScrolledWindow();
                var vBox = new VBox();
                frame.Add(scrolledWindow);
                scrolledWindow.AddWithViewport(vBox);
                foreach (var value in Enum.GetValues(enumInstance.GetType()))
                {
                    CheckButton checkButton;
                    if (isFlagType)
                    {
                        checkButton = new CheckButton(value.ToString())
                            {
                                Active = ((Enum)enumInstance).HasFlag((Enum)value),
                                UseUnderline = false
                            };
                    }
                    else
                    {
                        disableToggled = true;
                        checkButton = new RadioButton(value.ToString())
                            {
                                UseUnderline = false
                            };
                        if (groupRadioButton == null)
                        {
                            groupRadioButton = (RadioButton)checkButton;
                        }
                        else
                        {
                            ((RadioButton)checkButton).Group = groupRadioButton.Group;
                        }
                        checkButton.Active = enumInstance.ToString() == value.ToString();
                        disableToggled = false;
                    }
                    checkButton.Toggled += (sender, e) =>
                        {
                            if (disableToggled)
                            {
                                return;
                            }
                            if (isFlagType)
                            {
                                switch (enumInstance.GetType().GetEnumUnderlyingType().Name)
                                {
                                    case "Byte":
                                        propertyInfo.SetValue(property, (byte)((byte)enumInstance ^ (byte)value), null);
                                        break;
                                    case "Char":
                                        propertyInfo.SetValue(property, (char)((char)enumInstance ^ (char)value), null);
                                        break;
                                    case "Int16":
                                        propertyInfo.SetValue(property, (short)((short)enumInstance ^ (short)value), null);
                                        break;
                                    case "Int32":
                                        propertyInfo.SetValue(property, (int)enumInstance ^ (int)value, null);
                                        break;
                                    case "Int64":
                                        propertyInfo.SetValue(property, (long)enumInstance ^ (long)value, null);
                                        break;
                                    case "SByte":
                                        propertyInfo.SetValue(property, (sbyte)((sbyte)enumInstance ^ (sbyte)value), null);
                                        break;
                                    case "UInt16":
                                        propertyInfo.SetValue(property, (ushort)((ushort)enumInstance ^ (ushort)value), null);
                                        break;
                                    case "UInt32":
                                        propertyInfo.SetValue(property, (uint)enumInstance ^ (uint)value, null);
                                        break;
                                    case "UInt64":
                                        propertyInfo.SetValue(property, (ulong)enumInstance ^ (ulong)value, null);
                                        break;
                                }
                            }
                            else
                            {
                                propertyInfo.SetValue(property, value, null);
                            }
                            enumInstance = propertyInfo.GetValue(property, null);
                            additionalToggleAction();
                        };
                    vBox.PackStart(checkButton, false, false, 0);
                }
                return frame;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return null;
            }
        }

        public static Frame GetEnumPropertyCheckButtonsInNewFrame(string label, object propertyHolder, params string[] propertyPathComponents)
        {
            return GetEnumPropertyCheckButtonsInNewFrame(label, () =>
                {
                }, propertyHolder, propertyPathComponents);
        }

        public static void ReorderTabs(this Notebook notebook, Comparison<Widget> comparison)
        {
            bool swapped;
            for (var i = 0; i < notebook.NPages - 1; i++)
            {
                swapped = false;
                for (var j = 0; j < notebook.NPages - i - 1; j++)
                {
                    switch (comparison(notebook.GetNthPage(j), notebook.GetNthPage(j + 1)))
                    {
                        case -1:
                            notebook.ReorderChild(notebook.GetNthPage(j + 1), j);
                            swapped = true;
                            break;
                        case 1:
                            notebook.ReorderChild(notebook.GetNthPage(j), j + 1);
                            swapped = true;
                            break;
                    }
                }
                if (!swapped)
                {
                    break;
                }
            }
        }

        public static void RescaleAndReposition(this Gtk.Window self, Gtk.Window parent)
        {
            RescaleAndReposition(self, parent, Scale);
        }

        public static void RescaleAndReposition(this Gtk.Window self, Gtk.Window parent, float minScale)
        {
            try
            {
                var scale = WidgetUtils.Scale > minScale || Platform.IsWindows ? WidgetUtils.Scale : minScale;
                self.SetSizeRequest(self.WidthRequest == -1 ? -1 : (int)(self.WidthRequest * scale), self.HeightRequest == -1 ? -1 : (int)(self.HeightRequest * scale));
                int parentHeight, parentWidth, parentX, parentY;
                parent.GetPosition(out parentX, out parentY);
                parent.GetSize(out parentWidth, out parentHeight);
                self.Move(parentX + (parentWidth >> 1) - (self.WidthRequest >> 1), parentY + (parentHeight >> 1) - (self.HeightRequest >> 1));
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
    }
}
