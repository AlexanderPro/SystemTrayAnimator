using System;
using System.Windows.Forms;
using System.Threading;
using SystemTrayAnimator.Utils;

namespace SystemTrayAnimator
{
    static class Program
    {
        private static Mutex _mutex;

        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
            Application.ThreadException += OnThreadException;

            _mutex = new Mutex(true, AssemblyUtils.AssemblyTitle, out var createNew);
            if (!createNew)
            {
                return;
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainApplicationContext());

            _mutex.ReleaseMutex();
        }

        static void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            ex ??= new Exception("OnCurrentDomainUnhandledException");
            OnThreadException(sender, new ThreadExceptionEventArgs(ex));
        }

        static void OnThreadException(object sender, ThreadExceptionEventArgs e) =>
            MessageBox.Show(e.Exception.ToString(), AssemblyUtils.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
