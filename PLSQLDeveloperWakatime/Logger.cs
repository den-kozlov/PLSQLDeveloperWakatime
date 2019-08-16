using System;
using System.Diagnostics;
using System.IO;

namespace WakaTime
{
    public enum LogLevel
    {
        Debug = 1,
        Info,
        Warning,
        Error,
        None
    };

    static class Logger
    {
        static private StreamWriter outStream = null;
        static public LogLevel logLevel = LogLevel.None;

        static private void Init()
        {
            outStream = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\plsqlwakatime.log", true);
        }

        internal static void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        internal static void Error(string message, Exception ex = null)
        {
            var exceptionMessage = string.Format("{0}: {1}", message, ex);

            Log(LogLevel.Error, exceptionMessage);
        }

        internal static void Warning(string message)
        {
            Log(LogLevel.Warning, message);
        }

        internal static void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        private static void Log(LogLevel level, string message)
        {
            if (level < logLevel)
                return;

            if (outStream == null)
                Init();

            switch (level)
            {
                case LogLevel.Debug:
                    outStream.Write("[DEBUG] ");
                    break;
                case LogLevel.Info:
                    outStream.Write("[INFO ] ");
                    break;
                case LogLevel.Warning:
                    outStream.Write("[WARN ] ");
                    break;
                case LogLevel.Error:
                    outStream.Write("[ERROR] ");
                    break;
            }
            outStream.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToString(), message));
            outStream.Flush();
        }
    }
}