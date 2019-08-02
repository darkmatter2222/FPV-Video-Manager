using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FPV_Video_Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
                {
                    MessageBoxResult MBR = MessageBox.Show("FPV File Manager is already running, please use the alreay running instance of FPV File manager. \r\n\r\nCheck your system tray incase it is running headless.", "SH*% Hitting Fan");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
            catch
            {
            }

            try
            {
                new Config.Configuration().LoadFromConfig();
            }
            catch
            {
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }
    }
}
