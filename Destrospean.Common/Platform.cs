using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Destrospean.Common
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
                return (OS & OSFlags.Windows) != 0;
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
                        return OSFlags.Windows;
                }
            }
        }

        [Flags]
        public enum OSFlags
        {
            Windows = 1,
            Unix,
            Linux = 4,
            Darwin = 8
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

            public static void SetFileAssociation(string friendlyTypeName, string extension)
            {
                var assembly = System.Reflection.Assembly.GetEntryAssembly();
                string assemblyName = assembly.GetName().Name,
                classesRegistryPath = "HKEY_CURRENT_USER\\Software\\Classes\\";
                Registry.SetValue(classesRegistryPath + assemblyName, "", "My File Type");
                Registry.SetValue(classesRegistryPath + assemblyName, "FriendlyTypeName", friendlyTypeName);
                Registry.SetValue(classesRegistryPath + assemblyName + "\\shell\\open\\command", "", assembly.Location + " \"%1\"");
                Registry.SetValue(classesRegistryPath + extension, "", assemblyName);
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
