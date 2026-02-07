namespace Destrospean.DestrospeanCASPEditor
{
    public partial class CacheGenerationWindow : Gtk.Window
    {
        public CacheGenerationWindow(Gtk.Window parent, Gdk.Pixbuf icon) : base(Gtk.WindowType.Toplevel)
        {
            Build();
            Icon = icon;
            this.RescaleAndReposition(parent);
            Reposition();
            new System.Threading.Thread(() =>
                {
                    MainWindowBase.Singleton.RescaleAndReposition(true);
                    MainWindowBase.Singleton.Sensitive = false;
                    try
                    {
                        ChoosePatternDialog.GenerateCache(s3pi.Package.Package.NewPackage(0));
                        Common.Abstractions.CASPart.GenerateLookupCache();
                    }
                    catch (System.Exception ex)
                    {
                        Common.ProgramUtils.WriteError(ex);
                        throw;
                    }
                    MainWindowBase.Singleton.Sensitive = true;
                    Destroy();
                }).Start();
        }

        void Reposition()
        {
            var monitorGeometry = Screen.GetMonitorGeometry(Screen.GetMonitorAtWindow(GdkWindow));
            Move(((int)(monitorGeometry.Width / WidgetUtils.WineScaleDenominator) - WidthRequest) >> 1, ((int)(monitorGeometry.Height / WidgetUtils.WineScaleDenominator) - HeightRequest) >> 1);
        }
    }
}
