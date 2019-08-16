using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Reflection;

namespace WakaTime
{
    internal static class WakaTimeConstants
    {
        internal const string PluginName = "plsqldev-wakatime";
        internal static string PluginVersion =
            $"{typeof(WakaTime).Assembly.GetName().Version.Major}.{typeof(WakaTime).Assembly.GetName().Version.Minor}.{typeof(WakaTime).Assembly.GetName().Version.Build}";

        internal const string EditorName = "PLSQL Developer";
        internal static string EditorVersion;
        internal const string CliUrl = "https://github.com/wakatime/wakatime/archive/master.zip";
        internal const string CliFolder = @"wakatime-master\wakatime\cli.py";

        internal static Func<string> LatestWakaTimeCliVersion = () =>
        {
            var regex = new Regex(@"(__version_info__ = )(\(( ?\'[0-9]+\'\,?){3}\))");

            if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12))
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            }
            var client = WakaTime.GetWebClient();

            try
            {
                var about = client.DownloadString("https://raw.githubusercontent.com/wakatime/wakatime/master/wakatime/__about__.py");
                var match = regex.Match(about);

                if (match.Success)
                {
                    var grp1 = match.Groups[2];
                    var regexVersion = new Regex("([0-9]+)");
                    var match2 = regexVersion.Matches(grp1.Value);
                    return $"{match2[0].Value}.{match2[1].Value}.{match2[2].Value}";
                }
                else
                {
                    Logger.Warning("Couldn't auto resolve wakatime cli version");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception when checking current wakatime cli version: ", ex);
            }

            return string.Empty;
        };
    }
}