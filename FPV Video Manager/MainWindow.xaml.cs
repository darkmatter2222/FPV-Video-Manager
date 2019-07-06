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
using System.IO;
using System.Windows.Threading;

namespace FPV_Video_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        List<DriveData> KnownDrives = new List<DriveData>();
        bool DriveChangeDetected = false;

        public MainWindow()
        {
            InitializeComponent();
            BAL.Initialize init = new BAL.Initialize();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DrivesList.Items.Clear();

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            DriveChangeDetected = false;

            foreach (var drive in allDrives)
            {
                if (!ContainsDrive(drive.Name))
                {

                }
            }

               // DrivesList.Items.Add(new InterfaceControls.Drive(drive.Name));
        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ExitLabel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void DrivesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DrivesList.SelectedIndex > -1)
            {
                ShowWorkingRegion();
            }
            else
            {
                HideWorkingRegion();
            }
        }

        private void HideWorkingRegion()
        {
            SourceLabel.Visibility = Visibility.Collapsed;
            SourcePathTextBox.Visibility = Visibility.Collapsed;
            DestinationLabel.Visibility = Visibility.Collapsed;
            DestinationPathTextBox.Visibility = Visibility.Collapsed;
        }

        private void ShowWorkingRegion()
        {
            SourceLabel.Visibility = Visibility.Visible;
            SourcePathTextBox.Visibility = Visibility.Visible;
            DestinationLabel.Visibility = Visibility.Visible;
            DestinationPathTextBox.Visibility = Visibility.Visible;
        }

        public bool ContainsDrive(string driveInQuestion)
        {
            bool containsDrive = false;
            foreach (var drive in KnownDrives)
            {
                if (drive.DriveName.Equals(driveInQuestion))
                {
                    containsDrive = true;
                    break;
                }
            }
            return containsDrive;
        }

        class DriveData
        {
            public string DriveName = "";
        }
    }
}
