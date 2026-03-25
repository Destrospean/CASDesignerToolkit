using System;
using System.Destrospean;
using System.IO;

namespace Destrospean.DestrospeanCASPEditor
{
    class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.SetError(new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "error.log", true));
                    break;
                }
                catch
                {
                }
            }
            try
            {
                if (Platform.IsWindows)
                {
                    foreach (var filename in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
                    {
                        Platform.Windows.Unblock(filename);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            try
            {
                Common.ApplicationSettings.Singleton = new MainWindow.ApplicationSettings();
                Gtk.Application.Init();
                new MainWindow(args.Length > 0 ? args[0] : null);
                Gtk.Application.Run();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                throw;
            }
        }
    }
}
