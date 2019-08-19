using System;
using System.Runtime.InteropServices;

namespace WakaTime
{
    enum WindowType
    {
        wtOther = 0,
        wtSQL = 1,
        wtTest = 2,
        wtProcEdit = 3,
        wtCommand = 4,
        wtPlan = 5,
        wtReport = 6
    };

    delegate void IdeCreateWindow(int windowType, string text, [MarshalAs(UnmanagedType.Bool)] bool execute);
    delegate int SysVersion();
    delegate void IdeGetConnectionInfo(out string Username, out string Password, out string Database);
    delegate int IdeGetWindowType();
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool IdeSetStatusMessage(string Text);
    delegate IntPtr IdeFilename();
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool IdeSetText(string text);

    class CallbackIndexes
    {
        public const int SYS_VERSION_CALLBACK = 1;
        public const int IDE_GET_CONNECTION_INFO_CALLBACK = 12;
        public const int IDE_GET_WINDOW_TYPE_CALLBACK = 14;
        public const int IDE_CREATE_WINDOW_CALLBACK = 20;
        public const int IDE_FILENAME_CALLBACK = 23;
        public const int IDE_SET_TEXT_CALLBACK = 34;
        public const int IDE_SET_STATUS_MESSAGE_CALLBACK = 35;
    }

}
