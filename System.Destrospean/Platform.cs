using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Destrospean
{
    public static class Platform
    {
        public static string CacheDirectoryPath
        {
            get
            {
                return IsMacOS ? Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) : IsUnix ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/.cache" : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
        }

        public static bool IsLinux
        {
            get
            {
                return (OS & OSFlags.Linux) != 0;
            }
        }

        public static bool IsMacOS
        {
            get
            {
                return (OS & OSFlags.Darwin) != 0;
            }
        }

        public static bool IsRunningUnderWine
        {
            get
            {
                return IsWindows && Windows.IsRunningUnderWine;
            }
        }

        public static bool IsUnix
        {
            get
            {
                return (OS & OSFlags.Unix) != 0;
            }
        }

        public static bool IsWindows
        {
            get
            {
                return (OS & OSFlags.Win32) != 0;
            }
        }

        public static OSFlags OS
        {
            get
            {
                switch ((int)Environment.OSVersion.Platform)
                {
                    case 4:
                    case 128:
                        var os = OSFlags.Unix;
                        var uname = GetCommandOutput("uname").TrimEnd('\n');
                        switch (uname)
                        {
                            case "Darwin":
                            case "Linux":
                                os |= (OSFlags)Enum.Parse(typeof(OSFlags), uname);
                                break;
                        }
                        return os;
                    default:
                        return OSFlags.Win32;
                }
            }
        }

        [Flags]
        public enum OSFlags : byte
        {
            Win32 = 1,
            Unix,
            Linux = 4,
            Darwin = 8
        }

        public static class FreeDesktop
        {
            public static void CreateShortcut(string shortcutPath, string targetPath, string workingDirectory = "", string iconPath = "", string name = "", string description = "", string[] categories = null, string[] mimeTypes = null)
            {
                System.IO.File.WriteAllText(shortcutPath, string.Format(@"[Desktop Entry]
Type=Application
Name={0}
Exec='{1}' %f
Path={2}
Icon={3}
Comment={4}
Categories={5}
MimeType={6}", name, targetPath, workingDirectory, iconPath, description, string.Join(";", categories ?? new string[0]), string.Join(";", mimeTypes ?? new string[0])));
            }

            public static void SetFileAssociation(string mimeType, string description, string pattern)
            {
                string mimeDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/mime",
                mimeTypePath = mimeDirectoryPath + "/packages/" + mimeType + ".xml";
                if (!System.IO.File.Exists(mimeTypePath))
                {
                    System.IO.File.WriteAllText(mimeTypePath, string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<mime-info xmlns=""http://www.freedesktop.org/standards/shared-mime-info"">
  <mime-type type=""application/{0}"">
    <comment>{1}</comment>
    <glob pattern=""{2}""/>
  </mime-type>
</mime-info>", mimeType, description, pattern));
                    GetCommandOutput("update-mime-database", mimeDirectoryPath);
                }
            }
        }

        public static class Windows
        {
            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool DeleteFile(string name);

            [DllImport("Shell32.dll")]
            static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

            [DllImport("ntdll.dll", EntryPoint = "wine_get_version")]
            static extern IntPtr WineGetVersion();

            internal static bool IsRunningUnderWine
            {
                get
                {
                    try
                    {
                        return WineGetVersion() != IntPtr.Zero;
                    }
                    catch (EntryPointNotFoundException)
                    {
                        return false;
                    }
                }
            }

            public static void CreateShortcut(string shortcutPath, string targetPath, string workingDirectory = null, string iconPath = null, string description = null)
            {
                var shortcut = (IWshRuntimeLibrary.IWshShortcut)new IWshRuntimeLibrary.WshShell().CreateShortcut(shortcutPath);
                shortcut.Description = description;
                shortcut.IconLocation = iconPath;
                shortcut.TargetPath = targetPath;
                shortcut.WorkingDirectory = workingDirectory;
                shortcut.Save();
            }

            public static void SetFileAssociation(string fileType, string description, string extension, System.Reflection.Assembly assembly)
            {
                var classesRegistryPath = "HKEY_CURRENT_USER\\Software\\Classes\\";
                Registry.SetValue(classesRegistryPath + fileType, "", description);
                Registry.SetValue(classesRegistryPath + fileType + "\\shell\\open\\command", "", assembly.Location + " \"%1\"");
                Registry.SetValue(classesRegistryPath + extension, "", fileType);
                SHChangeNotify(0x8000000, 0x2000, IntPtr.Zero, IntPtr.Zero);
            }

            public static bool Unblock(string filename)
            {
                return DeleteFile(filename + ":Zone.Identifier");
            }
        }

        public static string GetCommandOutput(string command, string arguments = "")
        {
            using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                        {
                            Arguments = arguments,
                            CreateNoWindow = true,
                            FileName = command,
                            RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false
                        }
                })
            {
                process.Start();
                string error = process.StandardError.ReadToEnd(),
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return string.IsNullOrEmpty(error) ? output : string.Format("Error: {0}\nOutput: {1}", error, output);
            }
        }
    }
}
