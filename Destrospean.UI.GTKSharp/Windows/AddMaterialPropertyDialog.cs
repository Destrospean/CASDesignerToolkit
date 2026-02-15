using System.Collections.Generic;
using Gtk;
using s3pi.GenericRCOLResource;

namespace Destrospean.DestrospeanCASPEditor
{
    public partial class AddMaterialPropertyDialog : Dialog
    {
        public uint Field
        {
            get;
            private set;
        }

        public System.Type DataType
        {
            get;
            private set;
        }

        public uint ValueCount
        {
            get;
            private set;
        }

        public CmarNYCBorrowed.MeshFormatDataType ValueType
        {
            get;
            private set;
        }

        public AddMaterialPropertyDialog(Window parent) : this("Add Property", parent)
        {
        }

        public AddMaterialPropertyDialog(string title, Window parent) : base(title, parent, DialogFlags.Modal)
        {
            Build();
            this.RescaleAndReposition(parent, 8f / 7);
            Alignment.LeftPadding = (uint)WidgetUtils.SmallImageSize;
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(ShaderData));
            CellRendererText dataTypeCell = new CellRendererText(),
            fieldCell = new CellRendererText();
            ListStore dataTypeListStore = new ListStore(typeof(string)),
            fieldListStore = new ListStore(typeof(string));
            List<string> dataTypes = new List<string>(),
            fields = new List<string>(System.Enum.GetNames(typeof(FieldType)));
            foreach (var dataType in assembly.GetTypes())
            {
                if (dataType.IsClass && dataType.Namespace == "s3pi.GenericRCOLResource" && dataType.Name.StartsWith("Element") && !dataType.Name.EndsWith("Key"))
                {
                    dataTypes.Add(dataType.Name);
                }
            }
            dataTypes.Sort();
            dataTypes.ForEach(x => dataTypeListStore.AppendValues(x));
            DataTypeComboBox.Model = dataTypeListStore;
            DataTypeComboBox.Active = 0;
            DataTypeComboBox.PackStart(dataTypeCell, true);
            fields.Remove("None");
            fields.Sort();
            fields.Insert(0, "None");
            fields.ForEach(x => fieldListStore.AppendValues(x));
            FieldComboBox.Model = fieldListStore;
            FieldComboBox.Active = 0;
            FieldComboBox.PackStart(fieldCell, true);
            Response += (o, args) =>
                {
                    if (args.ResponseId == ResponseType.Ok)
                    {
                        switch (dataTypes[DataTypeComboBox.Active])
                        {
                            case "ElementFloat":
                                ValueType = CmarNYCBorrowed.MeshFormatDataType.Float;
                                ValueCount = 1;
                                break;
                            case "ElementFloat2":
                                ValueType = CmarNYCBorrowed.MeshFormatDataType.Float;
                                ValueCount = 2;
                                break;
                            case "ElementFloat3":
                                ValueType = CmarNYCBorrowed.MeshFormatDataType.Float;
                                ValueCount = 3;
                                break;
                            case "ElementFloat4":
                                ValueType = CmarNYCBorrowed.MeshFormatDataType.Float;
                                ValueCount = 4;
                                break;
                            case "ElementInt":
                                ValueType = CmarNYCBorrowed.MeshFormatDataType.Byte4;
                                ValueCount = 1;
                                break;
                            case "ElementTextureRef":
                                ValueType = CmarNYCBorrowed.MeshFormatDataType.Uint;
                                ValueCount = 1;
                                break;
                        }
                        DataType = assembly.GetType("s3pi.GenericRCOLResource." + dataTypes[DataTypeComboBox.Active]);
                        Field = (uint)System.Enum.Parse(typeof(FieldType), fields[FieldComboBox.Active]);
                    }
                };
        }
    }
}
