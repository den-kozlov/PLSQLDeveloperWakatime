using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WakaTime
{



    public class WakaTime
    {
        private const int PLUGIN_MENU_INDEX = 10;
        private const string PLUGIN_NAME = "Wakatime Plugin";

        private static string _version = string.Empty;
        private static string _editorVersion = string.Empty;

        private static WakaTime _instance;
        private static WakaTimeConfigFile _wakaTimeConfigFile;
        public static string ApiKey;
        public static bool Debug;
        static readonly PythonCliParameters PythonCliParameters = new PythonCliParameters();
        private static string _lastFile;
        DateTime _lastHeartbeat = DateTime.UtcNow.AddMinutes(-3);

        private static IdeCreateWindow createWindowCallback;
        private static IdeSetText setTextCallback;
        private static SysVersion sysVersion;
        private static IdeGetConnectionInfo getConnectionInfo;
        private static IdeGetWindowType getWindowType;
        private static IdeSetStatusMessage setStatusMessage;

        private int pluginId;
        private static bool PythonAvailable = false;
        private static bool WakaTimeCliAvailable = false;

        private WakaTime(int id)
        {
            pluginId = id;
            Version version = GetType().Assembly.GetName().Version;
            _version = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            _wakaTimeConfigFile = new WakaTimeConfigFile();
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
                case CallbackIndexes.SYS_VERSION_CALLBACK:
                    sysVersion = Marshal.GetDelegateForFunctionPointer<SysVersion>(function);
                    Logger.Debug("Callback initialized: Sys_Version");
                    break;
                case CallbackIndexes.IDE_CREATE_WINDOW_CALLBACK:
                    createWindowCallback = Marshal.GetDelegateForFunctionPointer<IdeCreateWindow>(function);
                    Logger.Debug("Callback initialized: IDE_CreateWindow");
                    break;
                case CallbackIndexes.IDE_SET_TEXT_CALLBACK:
                    setTextCallback = Marshal.GetDelegateForFunctionPointer<IdeSetText>(function);
                    Logger.Debug("Callback initialized: IDE_SetText");
                    break;
                case CallbackIndexes.IDE_GET_CONNECTION_INFO_CALLBACK:
                    getConnectionInfo = Marshal.GetDelegateForFunctionPointer<IdeGetConnectionInfo>(function);
                    Logger.Debug("Callback initialized: IDE_GetConnectionInfo");
                    break;
                case CallbackIndexes.IDE_GET_WINDOW_TYPE_CALLBACK:
                    getWindowType = Marshal.GetDelegateForFunctionPointer<IdeGetWindowType>(function);
                    Logger.Debug("Callback initialized: IDE_GetWindowType");
                    break;
                case CallbackIndexes.IDE_SET_STATUS_MESSAGE_CALLBACK:
                    setStatusMessage = Marshal.GetDelegateForFunctionPointer<IdeSetStatusMessage>(function);
                    Logger.Debug("Callback initialized: IDE_SetStatusMessage");
                    break;

            }
        }

        [DllExport("OnActivate", CallingConvention = CallingConvention.Cdecl)]
        public static void OnActivate()
        {
            Logger.Info("OnActivate");

            GetSettings();

            Task.Run(() =>
            {
                try
                {
                    if (!PythonManager.IsPythonInstalled())
                    {
                        var url = PythonManager.PythonDownloadUrl;
                        Downloader.DownloadPython(url, WakaTimeConstants.UserConfigDir);
                    }
                    PythonAvailable = true;
                }
                catch (Exception ex)
                {
                    Logger.Error("Error checking for Python", ex);
                }

            });
            Task.Run(() =>
            {
                try
                {
                    setStatusMessage?.Invoke("Checking WakaTime Cli version");
                    if (!DoesCliExist() || !IsCliLatestVersion())
                    {
                        string wakaDir = string.Format("{0}\\wakatime-master", WakaTimeConstants.UserConfigDir);
                        try
                        {
                            Directory.Delete(wakaDir, true);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(string.Format("Unable to delete folder {0}", wakaDir), ex);
                        }

                        setStatusMessage?.Invoke("Updating WakaTime Cli");
                        Downloader.DownloadCli(WakaTimeConstants.CliUrl, WakaTimeConstants.UserConfigDir);
                    }
                    WakaTimeCliAvailable = true;
                }
                catch (Exception ex)
                {
                    WakaTimeCliAvailable = false;
                    Logger.Error("Error checking for WakaTime Cli", ex);
                }
            });
        }

        [DllExport("OnWindowChange", CallingConvention = CallingConvention.Cdecl)]
        public static void OnWindowChange()
        {
            var type = (WindowType)getWindowType?.Invoke();
            if (type != WindowType.wtProcEdit)
                return;
        }

        [DllExport("OnWindowCreated", CallingConvention = CallingConvention.Cdecl)]
        public static void OnWindowCreated(int windowType)
        {

        }

        [DllExport("BeforeExecuteWindow", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static bool BeforeExecuteWindow(int windowType)
        {
            return true;
        }

        [DllExport("OnConnectionChange", CallingConvention = CallingConvention.Cdecl)]
        public static void OnConnectionChange()
        {

        }

        [DllExport("OnWindowConnectionChange", CallingConvention = CallingConvention.Cdecl)]
        public static void OnWindowConnectionChange()
        {

        }

        [DllExport("OnFileLoaded", CallingConvention = CallingConvention.Cdecl)]
        public static void OnFileLoaded(int windowType, int mode)
        {

        }
        
        [DllExport("OnFileSaved", CallingConvention = CallingConvention.Cdecl)]
        public static void OnFileSaved(int windowType, int mode)
        {

        }

        [DllExport("About", CallingConvention = CallingConvention.Cdecl)]
        public static string About()
        {
            return "WakaTime client for PL/SQL Developer.\nVisit https://wakatime.com for more info.";
        }
        #endregion

        [DllExport("Configure", CallingConvention = CallingConvention.Cdecl)]
        public static void Configure()
        {
            _instance.ShowConfigurationForm();
        }
        public string Name
        {
            get
            {
                return PLUGIN_NAME;
            }

        }
        private static void GetSettings()
        {
            ApiKey = _wakaTimeConfigFile.ApiKey;
            Debug = _wakaTimeConfigFile.Debug;
        }

        private void HandleActivity(string fileName = null, string objectName = null, string schemaName = null, bool isWrite = false)
        {
            if (!(PythonAvailable && WakaTimeCliAvailable))
                return;
        }
        private bool EnoughTimePassed()
        {
            return _lastHeartbeat < DateTime.UtcNow.AddMinutes(-1);
        }

        public static void SendHeartbeat(string fileName, bool isWrite)
        {
            PythonCliParameters.Key = ApiKey;
            PythonCliParameters.File = fileName;
            PythonCliParameters.Plugin = string.Format("{0}/{1} {2}/{3}", WakaTimeConstants.EditorName, _editorVersion, WakaTimeConstants.PluginName, _version);
            PythonCliParameters.IsWrite = isWrite;
            //PythonCliParameters.Project = GetProjectName();

            var pythonBinary = PythonManager.GetPython();
            if (pythonBinary != null)
            {
                var process = new RunProcess(pythonBinary, PythonCliParameters.ToArray());
                if (Debug)
                {
                    Logger.Debug(string.Format("[\"{0}\", \"{1}\"]", pythonBinary, string.Join("\", \"", PythonCliParameters.ToArray(true))));
                    process.Run();
                    Logger.Debug(string.Format("CLI STDOUT: {0}", process.Output));
                    Logger.Debug(string.Format("CLI STDERR: {0}", process.Error));
                }
                else
                    process.RunInBackground();
            }
            else
                Logger.Error("Could not send heartbeat because python is not installed");
        }

        static bool DoesCliExist()
        {
            bool result = File.Exists(PythonCliParameters.Cli);
            if (!result)
            {
                Logger.Debug(string.Format("WakaTime Cli wasn't found in expected location({0})", PythonCliParameters.Cli));
            }
            return result;
        }
        static bool IsCliLatestVersion()
        {
            var process = new RunProcess(PythonManager.GetPython(), PythonCliParameters.Cli, "--version");
            process.Run();

            var wakatimeVersion = WakaTimeConstants.CurrentWakaTimeCliVersion();

            Logger.Debug(string.Format("Current WakaTime Cli version is {0}, latest version is {1}", process.Error.ToString(), wakatimeVersion.ToString()));
            return process.Success && process.Error.Equals(wakatimeVersion);
        }

        private void ShowConfigurationForm()
        {
            var form = new WakaApiKeyForm();
            form.ShowDialog();
        }
    }
}
