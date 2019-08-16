using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.IO;

namespace WakaTime
{
    class Downloader
    {
        static public void DownloadCli(string url, string dir)
        {
            Logger.Debug("Downloading wakatime cli...");
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new WebClient();
            var localZipFile = dir + "\\wakatime-cli.zip";

            // Download wakatime cli
            client.DownloadFile(url, localZipFile);

            Logger.Debug("Finished downloading wakatime cli.");

            // Extract wakatime cli zip file
            ZipFile.ExtractToDirectory(localZipFile, dir);
            File.Delete(localZipFile);
        }

        static public void DownloadPython(string url, string dir)
        {
            Logger.Debug("Downloading python...");

            var localFile = dir + "\\python.msi";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new WebClient();
            client.DownloadFile(url, localFile);

            Logger.Debug("Finished downloading python.");

            var arguments = "/i \"" + localFile + "\"";
            arguments = arguments + " /norestart /qb!";

            var procInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                FileName = "msiexec",
                CreateNoWindow = true,
                Arguments = arguments
            };

            Process.Start(procInfo);
        }
    }
}