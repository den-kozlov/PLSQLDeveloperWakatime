using System.Runtime.InteropServices;

namespace WakaTime
{
    enum WindowType
    {
        wtSQL = 1,
        wtTest,
        wtProcEdit,
        wtCommand,
        wtPlan
    };

    delegate void IdeCreateWindow(int windowType, string text, [MarshalAs(UnmanagedType.Bool)] bool execute);
    delegate int SysVersion();

    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool IdeSetText(string text);

    class CallbackIndexes
    {
        public const int SYS_VERSION = 1;
        public const int CREATE_WINDOW_CALLBACK = 20;
        public const int SET_TEXT_CALLBACK = 34;
    }

}
