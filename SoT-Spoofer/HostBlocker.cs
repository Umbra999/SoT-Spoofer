using SoT_Spoofer.Wrappers;
using System.Net;

namespace SoT_Spoofer
{
    internal class HostBlocker
    {
        public static void ApplyBlock()
        {
            string HostsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts");

            List<string> AllHostLines = File.ReadAllLines(HostsFile).ToList();

            foreach (string url in Blocklist)
            {
                bool IsExisting = false;
                foreach (string line in AllHostLines)
                {
                    if (line.Contains(url))
                    {
                        IsExisting = true;
                        break;
                    }
                }
                if (!IsExisting) AllHostLines.Add($"0.0.0.0 {url}");
            }

            File.WriteAllLines(HostsFile, AllHostLines);

            foreach (string url in Blocklist)
            {
                try
                {
                    Uri uri = new("http://" + url + "/");
                    var ip = Dns.GetHostAddresses(uri.Host)[0];
                    Logger.LogError($"failed to block {url}");
                }
                catch
                {
                    Logger.LogDebug($"{url} is succesfully blocked");
                }
            }
        }

        private static readonly string[] Blocklist = new string[]
        {
            // Xbox
            "cdn.optimizely.com",
            "analytics.xboxlive.com",
            "cdf-anon.xboxlive.com",
            "settings-ssl.xboxlive.com",
            // Sea of Thieves
            "athenaprod.maelstrom.gameservices.xboxlive.com",
            //"e5ed.playfabapi.com", breaks voice chat, only use if voice isnt needed
            //"playfabapi.com", breaks voice chat, only use if voice isnt needed
        };


    }
}
