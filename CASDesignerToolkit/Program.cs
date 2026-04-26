using System;
using System.Destrospean;

namespace Destrospean.DestrospeanCASPEditor
{
    class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => Logger.WriteError((Exception)e.ExceptionObject, true);
            try
            {
                if (Platform.IsWindows)
                {
                    foreach (var filename in System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
                    {
                        Platform.Windows.Unblock(filename);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            Common.ApplicationSettings.Singleton = new MainWindow.ApplicationSettings();
            Gtk.Application.Init();
            new MainWindow(args.Length > 0 ? args[0] : null);
            Gtk.Application.Run();
        }
    }
}
