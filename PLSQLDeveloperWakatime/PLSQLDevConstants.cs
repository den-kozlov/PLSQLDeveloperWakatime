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
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool IdeGetWindowObject(out string ObjectType, out string ObjectOwner, out string ObjectName, out string SubObject);
    delegate int IdeGetCursorX();
    delegate int IdeGetCursorY();

    class CallbackIndexes
    {
        public const int SYS_VERSION = 1;
        public const int IDE_GET_CONNECTION_INFO = 12;
        public const int IDE_GET_WINDOW_TYPE = 14;
        public const int IDE_CREATE_WINDOW = 20;
        public const int IDE_FILENAME = 23;
        public const int IDE_SET_TEXT = 34;
        public const int IDE_SET_STATUS_MESSAGE = 35;
        public const int IDE_GET_WINDOW_OBJECT = 110;
        public const int IDE_GET_CURSOR_X = 141;
        public const int IDE_GET_CURSOR_Y = 142;
    }

}
