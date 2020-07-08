using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SharkFin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex = null;
        private static bool isMutexOwner;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // TODO generate an actual GUID rather than a value pasted from stackoverflow
            mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}", out isMutexOwner);
            if (!isMutexOwner)
            {
                this.Shutdown(1);
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (isMutexOwner)
            {
                mutex.ReleaseMutex();
            }
            mutex.Dispose();
            mutex = null;
        }
    }
}
