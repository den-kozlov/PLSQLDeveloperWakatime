using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WakaTime
{



    public class WakaTime
    {
        public static bool Debug = true;
        private const string PLUGIN_NAME = "Wakatime Plugin";
        private const int PLUGIN_MENU_INDEX = 10;


        private static WakaTime _instance;

        static readonly PythonCliParameters PythonCliParameters = new PythonCliParameters();

        private static IdeCreateWindow createWindowCallback;
        private static IdeSetText setTextCallback;
        private static SysVersion sysVersion;

        private int pluginId;

        private WakaTime(int id)
        {
            pluginId = id;
        }

        #region DLL exported API
        [DllExport("IdentifyPlugIn", CallingConvention = CallingConvention.Cdecl)]
        public static string IdentifyPlugIn(int id)
        {
            if (_instance == null)
            {
                _instance = new WakaTime(id);
            }
            return PLUGIN_NAME;
        }

        [DllExport("RegisterCallback", CallingConvention = CallingConvention.Cdecl)]
        public static void RegisterCallback(int index, IntPtr function)
        {
            switch (index)
            {
                case CallbackIndexes.SYS_VERSION:
                    sysVersion = Marshal.GetDelegateForFunctionPointer<SysVersion>(function);
                    break;
                case CallbackIndexes.CREATE_WINDOW_CALLBACK:
                    createWindowCallback = (IdeCreateWindow)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeCreateWindow));
                    break;
                case CallbackIndexes.SET_TEXT_CALLBACK:
                    setTextCallback = (IdeSetText)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeSetText));
                    break;

            }
        }

        [DllExport("OnActivate", CallingConvention = CallingConvention.Cdecl)]
        public static void OnActivate()
        {
  /*
            // Make sure python is installed
            if (!PythonManager.IsPythonInstalled())
            {
                var url = PythonManager.PythonDownloadUrl;
                Downloader.DownloadPython(url, WakaTimeConstants.UserConfigDir);
            }

            if (!DoesCliExist() || !IsCliLatestVersion())
            {
                try
                {
                    Directory.Delete(string.Format("{0}\\wakatime-master", WakaTimeConstants.UserConfigDir), true);
                }
                catch {  }

                Downloader.DownloadCli(WakaTimeConstants.CliUrl, WakaTimeConstants.UserConfigDir);
            }
*/
        }

        [DllExport("OnMenuClick", CallingConvention = CallingConvention.Cdecl)]
        public static void OnMenuClick(int index)
        {
            if (index == PLUGIN_MENU_INDEX)
            {
//                _instance.ShowDemoForm();
            }
        }

        [DllExport("CreateMenuItem", CallingConvention = CallingConvention.Cdecl)]
        public static string CreateMenuItem(int index)
        {
            if (sysVersion() < 1200)
            {
                switch (index)
                {
                    case 10:
                        return "Plug-Ins / WakaTime / Configure...";
                }
            }
            else
            {
                switch (index)
                {
                    case 1:
                        return "TAB=Plug-Ins";
                    case 2:
                        return "GROUP=WakaTime";
                    case 10:
                        return "ITEM=Configure...";
                }
            }
            return "";
        }

        [DllExport("About", CallingConvention = CallingConvention.Cdecl)]
        public static string About()
        {
            return "WakaTime client for PL/SQL Developer.\nVisit https://wakatime.com for more info.";
        }
        #endregion

        public string Name
        {
            get
            {
                return PLUGIN_NAME;
            }

        }
        static bool DoesCliExist()
        {
            return File.Exists(PythonCliParameters.Cli);
        }
        static bool IsCliLatestVersion()
        {
            var process = new RunProcess(PythonManager.GetPython(), PythonCliParameters.Cli, "--version");
            process.Run();

            var wakatimeVersion = WakaTimeConstants.CurrentWakaTimeCliVersion();

            return process.Success && process.Error.Equals(wakatimeVersion);
        }

    }
}
