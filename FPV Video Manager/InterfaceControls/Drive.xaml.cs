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

            StatusLabel.Content = "Doing Nothing";
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
                Thread.Sleep(100);
                try
                {
                    Dispatcher.Invoke(new Action(() => StatusLabel.Content = "Looking for Content..."));

                    string[] SourceFiles = Directory.GetFiles($@"{DI.Name}{DI.source}");

                    if (SourceFiles.Length > 0)
                    {
                        Dispatcher.Invoke(new Action(() => StatusLabel.Content = $@"{SourceFiles.Length} Files Found..."));

                        Thread.Sleep(100);

                        if (!Directory.Exists($@"{DI.destination}\{DI.offloadFolderName}"))
                            Directory.CreateDirectory($@"{DI.destination}\{DI.offloadFolderName}");

                        Dispatcher.Invoke(new Action(() => StatusLabel.Content = $@"Moving File..."));

                        File.Move(SourceFiles[0], $@"{DI.destination}\{DI.offloadFolderName}\{SourceFiles[0].ToUpper().Replace($@"{DI.Name}{DI.source}".ToUpper(),"")}");

                        Thread.Sleep(100);

                        Dispatcher.Invoke(new Action(() => StatusLabel.Content = $@"File Moved..."));
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
