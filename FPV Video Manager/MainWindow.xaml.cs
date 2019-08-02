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
using Engine;
using RestSharp;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace FPV_Video_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Config.Configuration appConfig = new Config.Configuration();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DriveEngine driveEngine = new DriveEngine();
        private System.Windows.Forms.NotifyIcon notifyIcon = null;

        public MainWindow()
        {
            new Reporting.AppStartup().Execute();
            InitializeComponent();

            SpeakStatusCheckBox.IsChecked = appConfig.Config.speakStages;
            SpeakStatusCheckBox.Checked += SpeakStatusCheckBox_Checked;
            SpeakStatusCheckBox.Unchecked += SpeakStatusCheckBox_Unchecked;

            driveEngine.DriveChange += DriveEngine_DriveChange;
            driveEngine.InitializeEngine();

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Click += new EventHandler(notifyIcon_Click);

            notifyIcon.Icon = (System.Drawing.Icon)FPV_Video_Manager.Properties.Resources.icon1;

            Versionlabel.Content = "v" +  System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

            Thread t = new Thread(CheckForUpdate);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        public void CheckForUpdate()
        {
            try
            {
                var client = new RestClient("https://api.github.com/repos/darkmatter2222/FPV-Video-Manager/releases");
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                JObject jo = JObject.Parse("{\"Root\":" + response.Content + "}");
                if (!jo["Root"][0]["tag_name"].ToString().Equals(System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()))
                {
                    UpdateWindow UW = new UpdateWindow(jo["Root"][0]["body"].ToString());
                    UW.ShowDialog();
                }
            }
            catch
            {
            }

        }

        void notifyIcon_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Visibility = Visibility.Visible;
            notifyIcon.Visible = false;
        }

        private void DriveEngine_DriveChange(List<DriveInformation> _LDI, EventArgs e)
        {
            Dispatcher.Invoke(new Action(() => UpdateDrivesList(_LDI)));
        }

        public void UpdateDrivesList(List<DriveInformation> _LDI)
        {
            DrivesList.Items.Clear();
            DrivesList.SelectedIndex = -1;
            foreach (var drive in _LDI)
                DrivesList.Items.Add(new InterfaceControls.Drive(_DI:drive));
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
                MainConfig.Content = new InterfaceControls.ConfigItem(((InterfaceControls.Drive)e.AddedItems[0]).DI);
            }
            else
            {
                MainConfig.Content = null;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            driveEngine.TerminateEngine();
            new GlobalEngineSwitch().AllEnginesRunning = false;
        }

        private void HeadlessLabel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Going headless means the app will continute to run and you wont see it. \r\n\r\n"+
                "To reopen this app, click the icon that will be added to your systen tray. \r\n\r\nProceed with going headless?", "Are you sure", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                this.ShowInTaskbar = false;
                this.Visibility = Visibility.Collapsed;
                notifyIcon.Visible = true;
            }
        }

        private void AboutLabel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/darkmatter2222/FPV-Video-Manager");
            }
            catch
            {
                MessageBox.Show("https://github.com/darkmatter2222/FPV-Video-Manager");
            }
        }



        private void SpeakStatusCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            appConfig.Config.speakStages = true;
            appConfig.save();
        }

        private void SpeakStatusCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            appConfig.Config.speakStages = false;
            appConfig.save();
        }
    }
}
