using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Destrospean.Common
{
    public static class Platform
    {
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
                return IsWindows && WineDetector.IsRunningUnderWine;
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

        static class FileUnblocker
        {
            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool DeleteFile(string name);

            public static bool Unblock(string filename)
            {
                return DeleteFile(filename + ":Zone.Identifier");
            }
        }

        static class WineDetector
        {
            [DllImport("ntdll.dll", EntryPoint = "wine_get_version")]
            static extern IntPtr WineGetVersion();

            public static bool IsRunningUnderWine
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
        }

        public static string GetCommandOutput(string command, string arguments = "")
        {
            var startInfo = new ProcessStartInfo
                {
                    Arguments = arguments,
                    CreateNoWindow = true,
                    FileName = command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };
            using (var process = new Process
                {
                    StartInfo = startInfo
                })
            {
                process.Start();
                string error = process.StandardError.ReadToEnd(),
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return string.IsNullOrEmpty(error) ? output : string.Format("Error: {0}\nOutput: {1}", error, output);
            }
        }

        public static bool UnblockFile(string filename)
        {
            return FileUnblocker.Unblock(filename);
        }
    }
}
