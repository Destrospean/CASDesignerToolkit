namespace Destrospean.DestrospeanCASPEditor
{
    public partial class NewUpdateDialog : Gtk.Dialog
    {
        public NewUpdateDialog(string message, string currentVersion, string newVersion, string description)
        {
            Build();
            Icon = MainWindowBase.Singleton.Icon;
            this.RescaleAndReposition(MainWindowBase.Singleton);
            ChangelogTextView.Buffer.Text = description;
            ChangelogTextView.Indent = WidgetUtils.SmallImageSize;
            UpdateAvailableLabel.Text = message + "\nCurrent Version: " + currentVersion + "\nNew Version: " + newVersion;
            InfoIconAlignment.LeftPadding = InfoIconAlignment.RightPadding = (uint)WidgetUtils.SmallImageSize >> 1;
        }
    }
}
