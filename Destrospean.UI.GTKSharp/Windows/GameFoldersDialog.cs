using System.Collections.Generic;
using Destrospean.Common;
using Gtk;
using s3pi.Filetable;

namespace Destrospean.DestrospeanCASPEditor
{
    public partial class GameFoldersDialog : Dialog
    {
        public static Dictionary<Game, string> InstallDirectories
        {
            get
            {
                var installDirectories = new Dictionary<Game, string>();
                foreach (var installDir in GameFolders.InstallDirs.Split(';'))
                {
                    var installDirectoryKvp = installDir.Split('=');
                    if (installDirectoryKvp.Length == 2)
                    {
                        installDirectories.Add(GameFolders.byName(installDirectoryKvp[0]), installDirectoryKvp[1]);
                    }
                }
                return installDirectories;
            }
        }

        public GameFoldersDialog(Window parent) : base("Game Folders", parent, DialogFlags.Modal)
        {
            Build();
            this.RescaleAndReposition(parent);
            foreach (var game in GameFolders.Games)
            {
                var fileChooserButton = new FileChooserButton("Choose Folder for " + game.Longname, FileChooserAction.SelectFolder);
                object installDirectories;
                if (ApplicationSettings.Settings.TryGetValue(ApplicationSettings.GameFoldersKey, out installDirectories))
                {
                    var installDirectoriesDictionary = installDirectories as IDictionary<string, string>;
                    if (installDirectoriesDictionary == null)
                    {
                        Newtonsoft.Json.Linq.JToken path;
                        fileChooserButton.SetCurrentFolder(((Newtonsoft.Json.Linq.JObject)installDirectories).TryGetValue(game.Name, out path) ? path.ToString().Replace('/', System.IO.Path.DirectorySeparatorChar) : "");
                    }
                    else
                    {
                        string path;
                        fileChooserButton.SetCurrentFolder(installDirectoriesDictionary.TryGetValue(game.Name, out path) ? path.Replace('/', System.IO.Path.DirectorySeparatorChar) : "");
                    }
                }
                else
                {
                    fileChooserButton.SetCurrentFolder("");
                }
                Alignment fileChooserButtonAlignment = new Alignment(0, .5f, 1, 0)
                    {
                        LeftPadding = (uint)WidgetUtils.SmallImageSize,
                    },
                labelAlignment = new Alignment(0, .5f, 1, 0)
                    {
                        LeftPadding = (uint)WidgetUtils.SmallImageSize
                    };
                fileChooserButtonAlignment.Add(fileChooserButton);
                labelAlignment.Add(new Label(game.Name == "base" ? "Base Game" : game.Longname.Replace("The Sims 3 ", ""))
                    {
                        UseUnderline = false,
                        Xalign = 0
                    });
                GameFolderTable.Attach(labelAlignment, 0, 1, GameFolderTable.NRows - 1, GameFolderTable.NRows, AttachOptions.Fill | AttachOptions.Shrink, 0, 0, 0);
                GameFolderTable.Attach(fileChooserButtonAlignment, 1, 2, GameFolderTable.NRows - 1, GameFolderTable.NRows, AttachOptions.Expand | AttachOptions.Fill, 0, 0, 0);
                GameFolderTable.NRows++;
                Response += (o, args) =>
                    {
                        if (args.ResponseId == ResponseType.Ok)
                        {
                            SetInstallDirectory(game, fileChooserButton.Filename ?? "");
                        }
                    };
            }
            ShowAll();
        }

        public static void SetInstallDirectory(Game game, string path)
        {
            var installDirectories = InstallDirectories;
            installDirectories[game] = path;
            var output = "";
            var outputDictionary = new SortedDictionary<string, string>(new ApplicationSettings.GameFolderComparer());
            foreach (var installDirectoryKvp in installDirectories)
            {
                output += ";" + installDirectoryKvp.Key.Name + "=" + installDirectoryKvp.Value.Replace('\\', '/');
                outputDictionary.Add(installDirectoryKvp.Key.Name, installDirectoryKvp.Value.Replace('\\', '/'));
            }
            GameFolders.InstallDirs = output.Substring(1);
            ApplicationSettings.Settings[ApplicationSettings.GameFoldersKey] = outputDictionary;
            ApplicationSettings.SaveSettings();
        }

        protected void OnCancelButtonClicked(object sender, System.EventArgs e)
        {
            Destroy();
            Dispose();
        }

        protected void OnOKButtonClicked(object sender, System.EventArgs e)
        {
            Destroy();
            S3PIExtensions.ResourceUtils.ClearGamePackages();
            new CacheGenerationWindow(MainWindowBase.Singleton, Icon);
            Dispose();
        }
    }
}
