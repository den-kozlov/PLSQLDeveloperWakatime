using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Script.Serialization;

namespace WakaTime
{



    public class WakaTime
    {
        private const int PLUGIN_MENU_INDEX = 10;
        private const string PLUGIN_NAME = "Wakatime Plugin";

        private static WakaTime _instance;
        private static ConfigFile config;
        static readonly PythonCliParameters PythonCliParameters = new PythonCliParameters();
        private static string _lastFile;
        DateTime _lastHeartbeat = DateTime.UtcNow.AddMinutes(-3);

        private static readonly ConcurrentQueue<Heartbeat> HeartbeatQueue = new ConcurrentQueue<Heartbeat>();
        private static readonly Timer Timer = new Timer();
        private const int HeartbeatFrequency = 2; // minutes        

        private static IdeCreateWindow createWindowCallback;
        private static IdeSetText setTextCallback;
        private static SysVersion sysVersion;
        private static IdeGetConnectionInfo getConnectionInfo;
        private static IdeGetWindowType getWindowType;
        private static IdeSetStatusMessage setStatusMessage;
        private static IdeFilename func_IdeFilename;
        private static string ideFilename()
        {
            if (func_IdeFilename != null)
            {
                IntPtr fileName = func_IdeFilename();
                return Marshal.PtrToStringAnsi(fileName);
            }
            else
                return "";
        }

        private int pluginId;
        private static bool WakaTimeCliAvailable = false;

        internal static IWebProxy GetProxy()
        {
            WebProxy proxy = null;

            try
            {
                if (string.IsNullOrEmpty(config.Proxy))
                {
                    Logger.Debug("No proxy will be used. It's either not set or badly formatted.");
                    return null;
                }

                var proxyStr = config.Proxy;

                // Regex that matches proxy address with authentication
                var regProxyWithAuth = new Regex(@"\s*(https?:\/\/)?([^\s:]+):([^\s:]+)@([^\s:]+):(\d+)\s*");
                var match = regProxyWithAuth.Match(proxyStr);

                if (match.Success)
                {
                    var username = match.Groups[2].Value;
                    var password = match.Groups[3].Value;
                    var address = match.Groups[4].Value;
                    var port = match.Groups[5].Value;

                    var credentials = new NetworkCredential(username, password);
                    proxy = new WebProxy(string.Join(":", address, port), true, null, credentials);

                    Logger.Debug("A proxy with authentication will be used.");
                    return proxy;
                }

                // Regex that matches proxy address and port(no authentication)
                var regProxy = new Regex(@"\s*(https?:\/\/)?([^\s@]+):(\d+)\s*");
                match = regProxy.Match(proxyStr);

                if (match.Success)
                {
                    var address = match.Groups[2].Value;
                    var port = int.Parse(match.Groups[3].Value);

                    proxy = new WebProxy(address, port);

                    Logger.Debug("A proxy will be used.");
                    return proxy;
                }

                Logger.Debug("No proxy will be used. It's either not set or badly formatted.");
            }
            catch (Exception ex)
            {
                Logger.Error("Exception while parsing the proxy string from WakaTime config file. No proxy will be used.", ex);
            }

            return proxy;
        }
        internal static WebClient GetWebClient()
        {
            if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12))
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            }
            ServicePointManager.Expect100Continue = true;
            return new WebClient { Proxy = GetProxy() };
        }

        private WakaTime(int id)
        {
            pluginId = id;
            config = new ConfigFile();
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
                    break;
                case CallbackIndexes.IDE_CREATE_WINDOW_CALLBACK:
                    createWindowCallback = Marshal.GetDelegateForFunctionPointer<IdeCreateWindow>(function);
                    break;
                case CallbackIndexes.IDE_SET_TEXT_CALLBACK:
                    setTextCallback = Marshal.GetDelegateForFunctionPointer<IdeSetText>(function);
                    break;
                case CallbackIndexes.IDE_GET_CONNECTION_INFO_CALLBACK:
                    getConnectionInfo = Marshal.GetDelegateForFunctionPointer<IdeGetConnectionInfo>(function);
                    break;
                case CallbackIndexes.IDE_GET_WINDOW_TYPE_CALLBACK:
                    getWindowType = Marshal.GetDelegateForFunctionPointer<IdeGetWindowType>(function);
                    break;
                case CallbackIndexes.IDE_SET_STATUS_MESSAGE_CALLBACK:
                    setStatusMessage = Marshal.GetDelegateForFunctionPointer<IdeSetStatusMessage>(function);
                    break;
                case CallbackIndexes.IDE_FILENAME_CALLBACK:
                    func_IdeFilename = Marshal.GetDelegateForFunctionPointer<IdeFilename>(function);
                    break;
            }
        }

        [DllExport("OnActivate", CallingConvention = CallingConvention.Cdecl)]
        public static void OnActivate()
        {
            Logger.Info("OnActivate");

            Logger.Debug(string.Format("PL/SQL Developer version is {0}", formatPLSQLDeveloperVersion()));

            Task.Run(() =>
            {
                InitAsync();
            });
        }

        [DllExport("OnDeactivate", CallingConvention = CallingConvention.Cdecl)]
        public static void OnDeactivate()
        {
            CleanUp();
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
            string fileName = ideFilename();
            Logger.Debug("BeforeExecuteWindow, windowType={0}, fileName={1}", windowType, fileName);
            if (!String.IsNullOrEmpty(fileName))
            {
                _instance.HandleActivity(fileName: fileName, isWrite: true);
            }
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

        [DllExport("Configure", CallingConvention = CallingConvention.Cdecl)]
        public static void Configure()
        {
            _instance.ShowConfigurationForm();
        }
        #endregion
        
        private static void CleanUp()
        {
            if (Timer == null) return;

            Timer.Stop();
            Timer.Elapsed -= ProcessHeartbeats;
            Timer.Dispose();

            // make sure the queue is empty	
            ProcessHeartbeats();
        }
        private static void InitAsync()
        {
            try
            {
                Logger.logLevel = config.logLevel;
                WakaTimeConstants.EditorVersion = formatPLSQLDeveloperVersion();

                // Make sure python is installed
                if (!Dependencies.IsPythonInstalled())
                {
                    Dependencies.DownloadAndInstallPython();
                }

                if (!Dependencies.DoesCliExist() || !Dependencies.IsCliUpToDate())
                {
                    Dependencies.DownloadAndInstallCli();
                }
                WakaTimeCliAvailable = true;

                Timer.Interval = 1000 * 8;
                Timer.Elapsed += ProcessHeartbeats;
                Timer.Start();
            }
            catch (WebException ex)
            {
                Logger.Error("Are you behind a proxy? Try setting a proxy in WakaTime Settings with format https://user:pass@host:port. Exception Traceback:", ex);
            }
            catch (Exception ex)
            {
                Logger.Error("Error detecting dependencies. Exception Traceback:", ex);
            }
        }
        private static string formatPLSQLDeveloperVersion()
        {
            if (sysVersion == null)
                return "";

            int fullVer = sysVersion();
            int buildVer = fullVer % 10;
            int minorVer = fullVer % 100 - buildVer;
            int majorVer = fullVer / 100;
            return string.Format("{0}.{1}.{2}", majorVer, minorVer, buildVer);
        }

        public string Name
        {
            get
            {
                return PLUGIN_NAME;
            }

        }
        private static void ProcessHeartbeats(object sender, ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                ProcessHeartbeats();
            });
        }

        private void HandleActivity(string fileName = null, string objectName = null, string schemaName = null, int? lineNo = null, bool isWrite = false)
        {
            if (!WakaTimeCliAvailable)
                return;

            var now = DateTime.UtcNow;
            string entity = String.IsNullOrEmpty(fileName) ? objectName : fileName;

            if (!isWrite && _lastFile != null && !EnoughTimePassed(now) && entity.Equals(_lastFile))
                return;

            _lastFile = entity;
            _lastHeartbeat = now;

            AppendHeartbeat(now, entity, schemaName, lineNo, isWrite);
        }
        private static void AppendHeartbeat(DateTime time, string entity = null, string schemaName = null, int? lineNo = null, bool isWrite = false)
        {
            var h = new Heartbeat
            {
                entity = entity,
                lineNo = lineNo,
                timestamp = ToUnixEpoch(time),
                is_write = isWrite,
                project = schemaName
            };
            HeartbeatQueue.Enqueue(h);
        }
        private static string ToUnixEpoch(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = date - epoch;
            var seconds = Convert.ToInt64(Math.Floor(timestamp.TotalSeconds));
            var milliseconds = timestamp.ToString("ffffff");
            return $"{seconds}.{milliseconds}";
        }
        private bool EnoughTimePassed()
        {
            return _lastHeartbeat < DateTime.UtcNow.AddMinutes(-1);
        }

        private static void ProcessHeartbeats()
        {
            var pythonBinary = Dependencies.GetPython();
            if (pythonBinary != null)
            {
                // get first heartbeat from queue
                var gotOne = HeartbeatQueue.TryDequeue(out var heartbeat);
                if (!gotOne)
                    return;

                // remove all extra heartbeats from queue
                var extraHeartbeats = new ArrayList();
                while (HeartbeatQueue.TryDequeue(out var h))
                    extraHeartbeats.Add(new Heartbeat(h));
                var hasExtraHeartbeats = extraHeartbeats.Count > 0;

                PythonCliParameters.Key = config.ApiKey;
                PythonCliParameters.Plugin =
                    $"{WakaTimeConstants.EditorName}/{WakaTimeConstants.EditorVersion} {WakaTimeConstants.PluginName}/{WakaTimeConstants.PluginVersion}";
                PythonCliParameters.Entity = heartbeat.entity;
                PythonCliParameters.lineNo = heartbeat.lineNo?.ToString();
                PythonCliParameters.Time = heartbeat.timestamp;
                PythonCliParameters.IsWrite = heartbeat.is_write;
                PythonCliParameters.Project = heartbeat.project;
                PythonCliParameters.HasExtraHeartbeats = hasExtraHeartbeats;

                string extraHeartbeatsJson = null;
                if (hasExtraHeartbeats)
                    extraHeartbeatsJson = new JavaScriptSerializer().Serialize(extraHeartbeats);

                var process = new RunProcess(pythonBinary, PythonCliParameters.ToArray());
                if (config.Debug)
                {
                    Logger.Debug(
                        $"[\"{pythonBinary}\", \"{string.Join("\", \"", PythonCliParameters.ToArray(true))}\"]");
                    process.Run(extraHeartbeatsJson);
                    if (!string.IsNullOrEmpty(process.Output))
                        Logger.Debug(process.Output);
                    if (!string.IsNullOrEmpty(process.Error))
                        Logger.Debug(process.Error);
                }
                else
                    process.RunInBackground(extraHeartbeatsJson);

                if (!process.Success)
                {
                    Logger.Error("Could not send heartbeat.");
                    if (!string.IsNullOrEmpty(process.Output))
                        Logger.Error(process.Output);
                    if (!string.IsNullOrEmpty(process.Error))
                        Logger.Error(process.Error);
                }
            }
            else
                Logger.Error("Could not send heartbeat because python is not installed");
        }
        private bool EnoughTimePassed(DateTime now)
        {
            return _lastHeartbeat < now.AddMinutes(-1 * HeartbeatFrequency);
        }

        private static void SettingsFormOnConfigSaved(object sender, EventArgs eventArgs)
        {
            config.Read();
        }

        private void ShowConfigurationForm()
        {
            var form = new WakaApiKeyForm();
            form.ConfigSaved += SettingsFormOnConfigSaved;
            form.ShowDialog();
        }
    }
}
