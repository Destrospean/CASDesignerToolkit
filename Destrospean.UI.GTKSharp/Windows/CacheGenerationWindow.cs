namespace Destrospean.DestrospeanCASPEditor
{
    public partial class CacheGenerationWindow : Gtk.Window
    {
        public static CmarNYCBorrowed.Action GenerateCachesAction;

        public CacheGenerationWindow(Gtk.Window parent, Gdk.Pixbuf icon) : base(Gtk.WindowType.Toplevel)
        {
            Build();
            Icon = icon;
            this.RescaleAndReposition(parent);
            Reposition();
            new System.Threading.Thread(() =>
                {
                    GenerateCachesAction();
                    Destroy();
                    Dispose();
                }).Start();
        }

        void Reposition()
        {
            var monitorGeometry = Screen.GetMonitorGeometry(Screen.GetMonitorAtWindow(GdkWindow));
            Move(((int)(monitorGeometry.Width / WidgetUtils.WineScaleDenominator) - WidthRequest) >> 1, ((int)(monitorGeometry.Height / WidgetUtils.WineScaleDenominator) - HeightRequest) >> 1);
        }
    }
}
