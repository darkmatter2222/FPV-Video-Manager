using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Engine;
using System.Threading;
using System.IO;

namespace FPV_Video_Manager.InterfaceControls
{
    /// <summary>
    /// Interaction logic for Drive.xaml
    /// </summary>
    public partial class Drive : UserControl
    {
        public DriveInformation DI;
        Thread T;
        bool ThreadRunning = false;

        public Drive(DriveInformation _DI)
        {
            InitializeComponent();

            DI = _DI;

            DriveNameTextBlock.Text = _DI.Name;

            if (_DI.isMonitoring)
            {
                MonitoringIc.Visibility = Visibility.Visible;
                BeginFileManager();
            }
            else
                MonitoringIc.Visibility = Visibility.Collapsed;
        }

        public void BeginFileManager()
        {
            ThreadRunning = true;
            T = new Thread(WorkingThread);
            T.Start();
        }

        public void WorkingThread()
        {
            while (ThreadRunning && new GlobalEngineSwitch().AllEnginesRunning)
            {
                Thread.Sleep(1000);
                try
                {
                    Dispatcher.Invoke(new Action(() => StatusLabel.Content = "Looking for Content..."));

                    string[] SourceFiles = Directory.GetFiles(DI.source);

                    if (SourceFiles.Length > 0)
                    {
                        Dispatcher.Invoke(new Action(() => StatusLabel.Content = $@"{SourceFiles.Length} Files Found..."));
                        Thread.Sleep(500);
                        //if(!Directory.Exists($@"{DI.destination}\"))
                        //Dispatcher.Invoke(new Action(() => StatusLabel.Content = $@"{SourceFiles.Length} Files Found..."));


                    }

                }
                catch
                {
                    
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ThreadRunning = false;
        }
    }
}
