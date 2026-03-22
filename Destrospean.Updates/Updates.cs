namespace System.Destrospean
{
    public class Updates
    {
        public static bool CheckForUpdates(string username, string repository, string localVersion, out string latestReleaseName, out string latestReleaseDescription, out string latestReleaseDownloadUrl, out string latestReleaseFilename)
        {
            latestReleaseDescription = null;
            latestReleaseDownloadUrl = null;
            latestReleaseFilename = null;
            latestReleaseName = null;
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(repository); 
                var latestRelease = ((Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(client.GetStringAsync("https://api.github.com/repos/" + username + "/" + repository + "/releases").Result))[0];
                if (localVersion.CompareTo(latestRelease["tag_name"].ToString().TrimStart('v')) < 0)
                {
                    latestReleaseDescription = latestRelease["body"].ToString();
                    latestReleaseName = latestRelease["name"].ToString();
                    foreach (var asset in latestRelease["assets"])
                    {
                        foreach (var flag in System.Enum.GetNames(typeof(Platform.OSFlags)))
                        {
                            var filename = asset["name"].ToString();
                            if (Platform.OS.HasFlag((Platform.OSFlags)Enum.Parse(typeof(Platform.OSFlags), flag)) && filename.Contains(flag.ToString().ToLowerInvariant()) && filename.Contains("Self-Extractor"))
                            {
                                latestReleaseFilename = filename;
                                latestReleaseDownloadUrl = ((Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(client.GetStringAsync(asset["url"].ToString()).Result))["browser_download_url"].ToString();
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
