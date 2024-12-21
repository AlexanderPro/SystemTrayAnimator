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
        private bool _started;

        public AccurateTimer(Action action)
        {
            _action = action;
            _handler = new TimerEventDelegate(TimerCallback);
            _started = false;
        }

        public void Start(int delay)
        {
            if (!_started)
            {
                timeBeginPeriod(1);
                _timerId = timeSetEvent(delay, 0, _handler, IntPtr.Zero, EVENT_TYPE);
                _started = true;
            }
        }

        public void Stop()
        {
            if (_started)
            {
                timeKillEvent(_timerId);
                timeEndPeriod(1);
                Thread.Sleep(100);
                _started = false;
            }
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
