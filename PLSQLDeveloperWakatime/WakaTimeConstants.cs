using System;
using System.Net;
using System.Text.RegularExpressions;

namespace WakaTime
{
    internal static class WakaTimeConstants
    {
        internal const string CliUrl = "https://github.com/wakatime/wakatime/archive/master.zip";
        internal const string PluginName = "plsql-wakatime";
        internal const string EditorName = "PL/SQL Developer";
        internal const string CliFolder = @"wakatime-master\wakatime\cli.py";
        internal static string UserConfigDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        internal static Func<string> CurrentWakaTimeCliVersion = () =>
        {
            var regex = new Regex(@"(__version_info__ = )(\(( ?\'[0-9]+\'\,?){3}\))");
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new WebClient();
            try
            {
                var about = client.DownloadString("https://raw.githubusercontent.com/wakatime/wakatime/master/wakatime/__about__.py");
                var match = regex.Match(about);

                if (!match.Success)
                {
                    return string.Empty;
                }

                var grp1 = match.Groups[2];
                var regexVersion = new Regex("([0-9]+)");
                var match2 = regexVersion.Matches(grp1.Value);

                return string.Format("{0}.{1}.{2}", match2[0].Value, match2[1].Value, match2[2].Value);
            }
            catch(Exception ex)
            {
                Logger.Error("Unable to determine the latest version of WakaTime Cli", ex);
                return string.Empty;
            }
        };
    }
}