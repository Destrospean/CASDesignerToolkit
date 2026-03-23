using System.Net.Http;
using bsn.HttpClientSync;

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
            using (var client = new HttpClient(new HttpClientSyncHandler()))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/" + username + "/" + repository + "/releases");
                request.Headers.Add("Accept", "*/*");
                request.Headers.Add("User-Agent", repository + "/" + localVersion);
                var response = client.Send(request);
                var latestRelease = ((Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content.ReadAsString()))[0];
                request.Dispose();
                response.Dispose();
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
                                request = new HttpRequestMessage(HttpMethod.Get, asset["url"].ToString());
                                response = client.Send(request);
                                latestReleaseFilename = filename;
                                latestReleaseDownloadUrl = ((Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content.ReadAsString()))["browser_download_url"].ToString();
                                request.Dispose();
                                response.Dispose();
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
