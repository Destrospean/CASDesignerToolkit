using System.Collections.Generic;
using CASPartResource;
using Gtk;

namespace Destrospean.DestrospeanCASPEditor
{
    public partial class SimPreviewDialog : Dialog
    {
        public readonly Dictionary<ClothingType, Dictionary<string, string>> CASParts = new Dictionary<ClothingType, Dictionary<string, string>>();

        public readonly List<ClothingType> CASPartsDisabled = new List<ClothingType>();

        public SimPreviewDialog(Window parent) : base("Sim Preview", parent, DialogFlags.Modal)
        {
            Build();
            this.RescaleAndReposition(parent);
            var sim = ((RendererMainWindow)MainWindowBase.Singleton).Sim;
            //var skinColorButton = new ColorButton(new Gdk.Color((byte)(sim.SkinColor[0] * byte.MaxValue), (byte)(sim.SkinColor[1] * byte.MaxValue), (byte)(sim.SkinColor[2] * byte.MaxValue)));
            var skinDarknessHScale = new HScale(0, 1, .01);
            skinDarknessHScale.Value = sim.SkinDarkness;
            var skinDarknessCheckButton = new CheckButton("Skin Tone")
                {
                    Active = sim.OverrideSkinColor,
                    UseUnderline = false,
                    Xalign = 0
                };
            Alignment skinDarknessHScaleAlignment = new Alignment(0, .5f, 1, 0),
            skinDarknessCheckButtonAlignment = new Alignment(0, .5f, 1, 0)
                {
                    LeftPadding = (uint)WidgetUtils.SmallImageSize
                };
            skinDarknessHScaleAlignment.Add(skinDarknessHScale);
            skinDarknessCheckButtonAlignment.Add(skinDarknessCheckButton);
            SimPreviewTable.Attach(skinDarknessCheckButtonAlignment, 0, 1, SimPreviewTable.NRows - 1, SimPreviewTable.NRows, AttachOptions.Fill, 0, 0, 0);
            SimPreviewTable.Attach(skinDarknessHScaleAlignment, 1, 2, SimPreviewTable.NRows - 1, SimPreviewTable.NRows, AttachOptions.Expand | AttachOptions.Fill, 0, 0, 0);
            SimPreviewTable.NRows++;
            foreach (ClothingType clothingType in System.Enum.GetValues(typeof(ClothingType)))
            {
                if (clothingType == 0)
                {
                    continue;
                }
                string resourceKey = null;
                Dictionary<string, string> casPart = null;
                var label = new Label(sim.CASPartOverrides.TryGetValue(clothingType, out casPart) ? casPart["Unknown1"] : "")
                    {
                        UseUnderline = false,
                        Xalign = 0
                    };
                if (casPart != null)
                {
                    CASParts[clothingType] = casPart;
                    resourceKey = casPart["ResourceKey"];
                }
                if (clothingType == sim.CurrentCASPart.CASPartResource.Clothing)
                {
                    CASParts[clothingType] = new Dictionary<string, string>
                    {
                        {
                            "Age",
                            sim.CurrentCASPart.CASPartResource.AgeGender.Age.ToString()
                        },
                        {
                            "Clothing",
                            sim.CurrentCASPart.CASPartResource.Clothing.ToString()
                        },
                        {
                            "Gender",
                            sim.CurrentCASPart.CASPartResource.AgeGender.Gender.ToString()
                        },
                        {
                            "ResourceKey",
                            sim.CurrentCASPart.ResourceKey
                        },
                        {
                            "Species",
                            sim.CurrentCASPart.CASPartResource.AgeGender.Species.ToString()
                        },
                        {
                            "Unknown1",
                            sim.CurrentCASPart.CASPartResource.Unknown1
                        }
                    };
                }
                Button button = new Button(label),
                clearButton = new Button(new Gtk.Image(Stock.Clear, IconSize.Menu))
                    {
                        Relief = ReliefStyle.None
                    };
                button.Clicked += (sender, e) =>
                    {
                        var chooseObjectDialog = new ChooseObjectDialog(this, s3pi.Package.Package.NewPackage(0), clothingType, new List<Dictionary<string, string>>(CASParts.Values).FindAll(x => !CASPartsDisabled.Contains((ClothingType)System.Enum.Parse(typeof(ClothingType), x["Clothing"]))).ToArray());
                        if (chooseObjectDialog.Run() == (int)ResponseType.Ok)
                        {
                            label.Text = chooseObjectDialog.CASPartName;
                            resourceKey = chooseObjectDialog.ResourceKey;
                            var tempCASPart = sim.GetCASPart(clothingType, resourceKey);
                            CASParts[clothingType] = new Dictionary<string, string>
                            {
                                {
                                    "Age",
                                    tempCASPart.CASPartResource.AgeGender.Age.ToString()
                                },
                                {
                                    "Clothing",
                                    tempCASPart.CASPartResource.Clothing.ToString()
                                },
                                {
                                    "Gender",
                                    tempCASPart.CASPartResource.AgeGender.Gender.ToString()
                                },
                                {
                                    "ResourceKey",
                                    tempCASPart.ResourceKey
                                },
                                {
                                    "Species",
                                    tempCASPart.CASPartResource.AgeGender.Species.ToString()
                                },
                                {
                                    "Unknown1",
                                    tempCASPart.CASPartResource.Unknown1
                                }
                            };
                        }
                        chooseObjectDialog.Destroy();
                        chooseObjectDialog.Dispose();
                    };
                clearButton.Clicked += (sender, e) =>
                    {
                        label.Text = "";
                        resourceKey = null;
                        CASParts.Remove(clothingType);
                    };
                var checkButton = new CheckButton(clothingType.ToString())
                    {
                        Active = !sim.CASPartOverridesDisabled.Contains(clothingType),
                        UseUnderline = false,
                        Xalign = 0
                    };
                checkButton.Toggled += (sender, e) =>
                    {
                        if (checkButton.Active)
                        {
                            if (CASPartsDisabled.Contains(clothingType))
                            {
                                CASPartsDisabled.Remove(clothingType);
                            }
                        }
                        else if (!CASPartsDisabled.Contains(clothingType))
                        {
                            CASPartsDisabled.Add(clothingType);
                        }
                    };
                Alignment buttonAlignment = new Alignment(0, .5f, 1, 0),
                checkButtonAlignment = new Alignment(0, .5f, 1, 0)
                    {
                        LeftPadding = (uint)WidgetUtils.SmallImageSize
                    },
                clearButtonAlignment = new Alignment(1, .5f, 0, 0);
                buttonAlignment.Add(button);
                checkButtonAlignment.Add(checkButton);
                clearButtonAlignment.Add(clearButton);
                var hbox = new HBox(false, 4);
                hbox.PackStart(checkButtonAlignment);
                hbox.PackEnd(clearButtonAlignment);
                SimPreviewTable.Attach(hbox, 0, 1, SimPreviewTable.NRows - 1, SimPreviewTable.NRows, AttachOptions.Fill, 0, 0, 0);
                SimPreviewTable.Attach(buttonAlignment, 1, 2, SimPreviewTable.NRows - 1, SimPreviewTable.NRows, AttachOptions.Expand | AttachOptions.Fill, 0, 0, 0);
                SimPreviewTable.NRows++;
                Response += (o, args) =>
                    {
                        if (args.ResponseId != ResponseType.Ok)
                        {
                            return;
                        }
                        if (resourceKey != null)
                        {
                            sim.SetCASPartOverride(clothingType, resourceKey);
                        }
                        else if (sim.CASPartOverrides.ContainsKey(clothingType))
                        {
                            sim.CASPartOverrides.Remove(clothingType);
                        }
                        if (checkButton.Active)
                        {
                            if (sim.CASPartOverridesDisabled.Contains(clothingType))
                            {
                                sim.CASPartOverridesDisabled.Remove(clothingType);
                            }
                        }
                        else if (!sim.CASPartOverridesDisabled.Contains(clothingType))
                        {
                            sim.CASPartOverridesDisabled.Add(clothingType);
                        }
                    };
            }
            Response += (o, args) =>
                {
                    if (args.ResponseId == ResponseType.Ok)
                    {
                        if (sim.OverrideSkinColor = skinDarknessCheckButton.Active)
                        {
                            sim.SkinDarkness = (float)skinDarknessHScale.Value;
                            //sim.SkinColor[0] = (float)(skinColorButton.Color.Red >> 8) / byte.MaxValue;
                            //sim.SkinColor[1] = (float)(skinColorButton.Color.Green >> 8) / byte.MaxValue;
                            //sim.SkinColor[2] = (float)(skinColorButton.Color.Blue >> 8) / byte.MaxValue;
                        }
                    }
                };
            ShowAll();
        }
    }
}
