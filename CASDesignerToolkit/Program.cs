namespace Destrospean.DestrospeanCASPEditor
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                MainWindow.CheckForUpdates();
                if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "CASDTKUpdater.exe"))
                {
                    System.IO.File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "CASDTKUpdater.exe");
                }
                System.Console.SetError(new System.IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + "error.log", true));
                if (System.Destrospean.Platform.IsWindows)
                {
                    foreach (var filename in System.IO.Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory))
                    {
                        System.Destrospean.Platform.Windows.Unblock(filename);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Destrospean.Logger.WriteError(ex);
            }
            try
            {
                Common.ApplicationSettings.Singleton = new MainWindow.ApplicationSettings();
                Gtk.Application.Init();
                new MainWindow(args.Length > 0 ? args[0] : null);
                Gtk.Application.Run();
            }
            catch (System.Exception ex)
            {
                System.Destrospean.Logger.WriteError(ex);
                throw;
            }
        }
    }
}
