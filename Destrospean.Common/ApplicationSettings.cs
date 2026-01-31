using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Destrospean.Common
{
    public static class ApplicationSettings
    {
        public static Dictionary<string, object> Settings
        {
            get;
            private set;
        }

        public static readonly string SettingsFilePath = string.Format("{0}{1}Destrospean{1}UserSettings.json", System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Path.DirectorySeparatorChar);

        public static bool UseAdvancedOpenGLShaders
        {
            get
            {
                return Settings == null || !Settings.ContainsKey(JSONNodeNames.UseAdvancedOpenGLShaders) || (bool)Settings[JSONNodeNames.UseAdvancedOpenGLShaders];
            }
            set
            {
                if (Settings == null)
                {
                    Settings = new Dictionary<string, object>();
                }
                Settings[JSONNodeNames.UseAdvancedOpenGLShaders] = value;
                SaveSettings();
            }
        }

        public class GameFolderComparer : IComparer<string>
        {
            public int Compare(string a, string b)
            {
                return s3pi.Filetable.GameFolders.Games.IndexOf(s3pi.Filetable.GameFolders.byName(a)).CompareTo(s3pi.Filetable.GameFolders.Games.IndexOf(s3pi.Filetable.GameFolders.byName(b)));
            }
        }

        public static class JSONNodeNames
        {
            public const string GameFolders = "The Sims 3 Installation Directories",
            UseAdvancedOpenGLShaders = "Use Advanced OpenGL Shaders";
        }

        public static void LoadSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                var installDirs = "";
                using (var stream = File.OpenText(SettingsFilePath))
                {
                    Settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(stream.ReadToEnd());
                    object installDirectories;
                    if (Settings.TryGetValue(JSONNodeNames.GameFolders, out installDirectories))
                    {
                        foreach (var installDirectoryKvp in (Newtonsoft.Json.Linq.JObject)installDirectories)
                        {
                            installDirs += ";" + installDirectoryKvp.Key + "=" + installDirectoryKvp.Value;
                        }
                        s3pi.Filetable.GameFolders.InstallDirs = installDirs.Substring(1);
                    }
                }
                return;
            }
            Settings = new Dictionary<string, object>();
        }

        public static void SaveSettings()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath));
            File.WriteAllText(SettingsFilePath, JsonConvert.SerializeObject(Settings, Formatting.Indented));
        }
    }
}
