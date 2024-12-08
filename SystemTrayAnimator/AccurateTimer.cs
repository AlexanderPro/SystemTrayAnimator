using System;
using System.Threading;
using System.Runtime.InteropServices;

namespace SystemTrayAnimator
{
    class AccurateTimer
    {
        private const int EVENT_TYPE = 1;

        private delegate void TimerEventDelegate(int id, int msg, IntPtr user, int dw1, int dw2);

        [DllImport("winmm.dll")]
        private static extern int timeBeginPeriod(int msec);

        [DllImport("winmm.dll")]
        private static extern int timeEndPeriod(int msec);

        [DllImport("winmm.dll")]
        private static extern int timeSetEvent(int delay, int resolution, TimerEventDelegate handler, IntPtr user, int eventType);

        [DllImport("winmm.dll")]
        private static extern int timeKillEvent(int id);


        private int _timerId;
        private Action _action;
        private TimerEventDelegate _handler;

        public void Start(Action action, int delay)
        {
            _action = action;
            timeBeginPeriod(1);
            _handler = new TimerEventDelegate(TimerCallback);
            _timerId = timeSetEvent(delay, 0, _handler, IntPtr.Zero, EVENT_TYPE);
        }

        public void Stop()
        {
            timeKillEvent(_timerId);
            timeEndPeriod(1);
            Thread.Sleep(100);
        }

        private void TimerCallback(int id, int msg, IntPtr user, int dw1, int dw2)
        {
            if (_timerId != 0)
            {
                _action();
            }
        }
    }
}
