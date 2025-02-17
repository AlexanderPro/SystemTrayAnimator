using System;
using System.Runtime.InteropServices;
using SystemTrayAnimator.Native.Enums;

namespace SystemTrayAnimator.Native
{
    static class User32
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetProcessDpiAwarenessContext(DpiAwarenessContext context);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetProcessDPIAware();

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr handle);
    }
}
