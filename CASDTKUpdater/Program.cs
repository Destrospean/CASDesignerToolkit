using System.Diagnostics;
using System.IO;

namespace Destrospean.DestrospeanCASPEditor.Updater
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }
            var mainProcess = Process.GetProcessById(int.Parse(args[0]));
            mainProcess.Kill();
            mainProcess.WaitForExit();
            var executablePath = System.AppDomain.CurrentDomain.BaseDirectory;
            foreach (var directoryName in Directory.GetDirectories(executablePath))
            {
                if (!directoryName.EndsWith("Update"))
                {
                    Directory.Delete(directoryName, true);
                }
            }
            foreach (var filename in Directory.GetFiles(executablePath))
            {
                if (!filename.EndsWith("CASDTKUpdater.exe"))
                {
                    File.Delete(filename);
                }
            }
            foreach (var directoryName in Directory.GetDirectories(executablePath + "Update" + Path.DirectorySeparatorChar + "CASDesignerToolkit"))
            {
                Directory.Move(directoryName, executablePath + directoryName.Substring(directoryName.LastIndexOf(Path.DirectorySeparatorChar)));
            }
            foreach (var filename in Directory.GetFiles(executablePath + "Update" + Path.DirectorySeparatorChar + "CASDesignerToolkit"))
            {
                File.Move(filename, executablePath + filename.Substring(filename.LastIndexOf(Path.DirectorySeparatorChar)));
            }
            Directory.Delete(executablePath + "Update", true);
            using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                        {
                            Arguments = args.Length == 1 ? "" : args[1],
                            CreateNoWindow = true,
                            FileName = "CASDesignerToolkit",
                            UseShellExecute = false
                        }
                })
            {
                process.Start();
            }
        }
    }
}
