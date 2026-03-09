using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Destrospean.Common
{
    public class ApplicationSettings
    {
        protected Dictionary<string, object> mSettings;

        public static readonly string GameFoldersKey = "The Sims 3 Installation Directories",
        SettingsFilePath = string.Format("{0}{1}Destrospean{1}UserSettings.json", System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Path.DirectorySeparatorChar);

        public static Dictionary<string, object> Settings
        {
            get
            {
                return Singleton.mSettings;
            }
            set
            {
                Singleton.mSettings = value;
            }
        }

        public static ApplicationSettings Singleton;

        public class GameFolderComparer : IComparer<string>
        {
            public int Compare(string a, string b)
            {
                return s3pi.Filetable.GameFolders.Games.IndexOf(s3pi.Filetable.GameFolders.byName(a)).CompareTo(s3pi.Filetable.GameFolders.Games.IndexOf(s3pi.Filetable.GameFolders.byName(b)));
            }
        }

        public static void LoadSettings()
        {
            if (Singleton == null)
            {
                Singleton = new ApplicationSettings();
            }
            if (File.Exists(SettingsFilePath))
            {
                var installDirs = "";
                using (var stream = File.OpenText(SettingsFilePath))
                {
                    Singleton.mSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(stream.ReadToEnd());
                    object installDirectories;
                    if (Settings.TryGetValue(GameFoldersKey, out installDirectories))
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
            Singleton.mSettings = new Dictionary<string, object>();
        }

        public static void SaveSettings()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath));
            File.WriteAllText(SettingsFilePath, JsonConvert.SerializeObject(Settings, Formatting.Indented));
        }
    }
}
