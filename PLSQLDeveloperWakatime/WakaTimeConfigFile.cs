using System;
using System.Text;

namespace WakaTime
{
    public class ConfigFile
    {
        public string ApiKey { get; set; }
        public string Proxy { get; set; }
        public bool Debug {
            get { return logLevel == LogLevel.Debug; }
        }
        public LogLevel logLevel { get; set; }
        public string PythonBinaryLocation { get; set; }

        private readonly string _configFilepath;

        public ConfigFile()
        {
            _configFilepath = GetConfigFilePath();
            Read();
        }

        public void Read()
        {
            var ret = new StringBuilder(2083);

            ApiKey = NativeMethods.GetPrivateProfileString("settings", "api_key", "", ret, 2083, _configFilepath) > 0
                ? ret.ToString()
                : string.Empty;

            Proxy = NativeMethods.GetPrivateProfileString("settings", "proxy", "", ret, 2083, _configFilepath) > 0
                ? ret.ToString()
                : string.Empty;

            PythonBinaryLocation = NativeMethods.GetPrivateProfileString("settings", "python_location", "", ret, 2083, _configFilepath) > 0
                ? ret.ToString()
                : string.Empty;
            // ReSharper disable once InvertIf
            if (NativeMethods.GetPrivateProfileString("settings", "log_level", "none", ret, 2083, _configFilepath) > 0)
            {
                LogLevel level;
                if (Enum.TryParse(ret.ToString(), ignoreCase: true, out level))
                    logLevel = level;
                else
                    logLevel = LogLevel.None;
            }
        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(ApiKey))
                NativeMethods.WritePrivateProfileString("settings", "api_key", ApiKey.Trim(), _configFilepath);

            NativeMethods.WritePrivateProfileString("settings", "python_location", PythonBinaryLocation.Trim(), _configFilepath);
            NativeMethods.WritePrivateProfileString("settings", "proxy", Proxy.Trim(), _configFilepath);
            NativeMethods.WritePrivateProfileString("settings", "log_level", logLevel.ToString().ToLower(), _configFilepath);
        }

        static string GetConfigFilePath()
        {
            var userHomeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return userHomeDir + "\\.wakatime.cfg";
        }
    }
}