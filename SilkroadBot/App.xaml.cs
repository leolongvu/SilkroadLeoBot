using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LeoBot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        KeyboardListener KListener = new KeyboardListener();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            KListener.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            if (Proxy.connection == true)
            {
                if (args.Key == Key.F6)
                {
                    Globals.MainWindow.Checkstarthotkey();
                }
                if (args.Key == Key.F7)
                {
                    Globals.MainWindow.Checkstophotkey();
                }
                if (args.Key == Key.F8)
                {
                    Globals.MainWindow.Checkcoordinate();
                }
            }            
        }
        
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            KListener.Dispose();
        }
    }
}
