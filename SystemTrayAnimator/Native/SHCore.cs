using System.Runtime.InteropServices;
using SystemTrayAnimator.Native.Enums;

namespace SystemTrayAnimator.Native
{
    static class SHCore
    {
        [DllImport("SHCore.dll", SetLastError = true)]
        public static extern bool SetProcessDpiAwareness(ProcessDpiAwareness processDpiAwareness);
    }
}
