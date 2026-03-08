namespace Destrospean.DestrospeanCASPEditor
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                System.Console.SetError(new System.IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + "error.log", true));
                if (Common.Platform.IsWindows)
                {
                    foreach (var filename in System.IO.Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory))
                    {
                        Common.Platform.Windows.Unblock(filename);
                    }
                }
                Gtk.Application.Init();
                new MainWindow(args.Length > 0 ? args[0] : null);
                Gtk.Application.Run();
            }
            catch (System.Exception ex)
            {
                Common.ProgramUtils.WriteError(ex);
                throw;
            }
        }
    }
}
