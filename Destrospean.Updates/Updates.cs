using Org.BouncyCastle.Crypto.Tls;

namespace System.Destrospean
{
    public class Updates
    {
        class DummyTlsAuthentication : TlsAuthentication
        {
            public TlsCredentials GetClientCredentials(CertificateRequest certificateRequest)
            {
                return null;
            }

            public void NotifyServerCertificate(Certificate certificate)
            {
            }
        }

        class DummyTlsClient : DefaultTlsClient
        {
            public override TlsAuthentication GetAuthentication() 
            {
                return new DummyTlsAuthentication();
            }
        }

        public static bool CheckForUpdates(string username, string repository, string localVersion, out string latestReleaseName, out string latestReleaseDescription, out string latestReleaseDownloadUrl, out string latestReleaseFilename)
        {
            latestReleaseDescription = null;
            latestReleaseDownloadUrl = null;
            latestReleaseFilename = null;
            latestReleaseName = null;
            var latestRelease = ((Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(GetString("https://api.github.com/repos/" + username + "/" + repository + "/releases", repository)))[0];
            int[] latestVersionArray = Array.ConvertAll(latestRelease["tag_name"].ToString().TrimStart('v').Split('.'), int.Parse),
            localVersionArray = Array.ConvertAll(localVersion.Split('.'), int.Parse);
            if (latestVersionArray[0] > localVersionArray[0] || latestVersionArray[0] == localVersionArray[0] && (latestVersionArray[1] > localVersionArray[1] || latestVersionArray[0] == localVersionArray[0] && latestVersionArray[2] > localVersionArray[2]))
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
                            latestReleaseDownloadUrl = ((Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(GetString(asset["url"].ToString(), repository)))["browser_download_url"].ToString();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static byte[] GetByteArray(string url, string userAgent)
        {
            url = url.Substring(url.IndexOf("//") + 2);
            string hostname = url.Remove(url.IndexOf('/')),
            path = url.Substring(url.IndexOf('/'));
            var responseSplit = new string[0];
            using (var client = new System.Net.Sockets.TcpClient(hostname, 443))
            {
                var secureRandom = new Org.BouncyCastle.Security.SecureRandom();
                var protocol = new TlsClientProtocol(client.GetStream(), secureRandom);
                protocol.Connect(new DummyTlsClient());
                using (var stream = protocol.Stream)
                {
                    var writer = new System.IO.StreamWriter(stream);
                    writer.Write(string.Format("GET {0} HTTP/1.1\r\nHost: {1}\r\nUser-Agent: {2}]\r\nConnection: close\r\n\r\n", path, hostname, userAgent));
                    writer.Flush();
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        var response = reader.ReadToEnd();
                        responseSplit = response.Remove(response.IndexOf("\r\n\r\n")).Split(new[]
                            {
                                "\r\n"
                            }, StringSplitOptions.None);
                        
                    }
                }
            }
            if (responseSplit.Length > 1)
            {
                foreach (var item in responseSplit)
                {
                    if (item.StartsWith("Location: "))
                    {
                        url = item.Substring(item.IndexOf("//") + 2);
                        hostname = url.Remove(url.IndexOf('/'));
                        path = url.Substring(url.IndexOf('/'));
                        using (var client = new System.Net.Sockets.TcpClient(hostname, 443))
                        {
                            var secureRandom = new Org.BouncyCastle.Security.SecureRandom();
                            var protocol = new TlsClientProtocol(client.GetStream(), secureRandom);
                            protocol.Connect(new DummyTlsClient());
                            using (var stream = protocol.Stream)
                            {
                                var request = string.Format("GET {0} HTTP/1.1\r\nHost: {1}\r\nUser-Agent: {2}]\r\nConnection: close\r\n\r\n", path, hostname, userAgent);
                                var writer = new System.IO.StreamWriter(stream);
                                writer.Write(request);
                                writer.Flush();
                                var encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
                                using (var reader = new System.IO.StreamReader(stream, encoding))
                                {
                                    var response = reader.ReadToEnd();
                                    return encoding.GetBytes(response.Substring(response.IndexOf("\r\n\r\n") + 4));
                                }
                            }
                        }
                    }
                }
            }
            return new byte[0];
        }

        public static string GetString(string url, string userAgent)
        {
            url = url.Substring(url.IndexOf("//") + 2);
            string hostname = url.Remove(url.IndexOf('/')),
            path = url.Substring(url.IndexOf('/'));
            using (var client = new System.Net.Sockets.TcpClient(hostname, 443))
            {
                var secureRandom = new Org.BouncyCastle.Security.SecureRandom();
                var protocol = new TlsClientProtocol(client.GetStream(), secureRandom);
                protocol.Connect(new DummyTlsClient());
                using (var stream = protocol.Stream)
                {
                    var writer = new System.IO.StreamWriter(stream);
                    writer.Write(string.Format("GET {0} HTTP/1.1\r\nHost: {1}\r\nUser-Agent: {2}]\r\nConnection: close\r\n\r\n", path, hostname, userAgent));
                    writer.Flush();
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        var response = reader.ReadToEnd();
                        response = response.Substring(response.IndexOf("\r\n\r\n") + 4);
                        var responseSplit = response.Split(new[]
                            {
                                "\r\n"
                            }, StringSplitOptions.None);
                        if (responseSplit.Length > 1)
                        {
                            response = "";
                            for (var i = 1; i < responseSplit.Length; i += 2)
                            {
                                response += responseSplit[i];
                            }
                        }
                        return response;
                    }
                }
            }
        }
    }
}
