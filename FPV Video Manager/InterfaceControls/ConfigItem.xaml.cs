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
using System.IO;

namespace FPV_Video_Manager.InterfaceControls
{
    /// <summary>
    /// Interaction logic for ConfigItem.xaml
    /// </summary>
    public partial class ConfigItem : UserControl
    {
        DriveInformation DI = new DriveInformation();
        DriveEngine driveEngine = new DriveEngine();

        public ConfigItem(DriveInformation _DI)
        {
            DI = _DI;
            InitializeComponent();
            SourceLabel.Visibility = Visibility.Collapsed;
            SourcePathTextBox.Visibility = Visibility.Collapsed;
            DestinationLabel.Visibility = Visibility.Collapsed;
            DestinationPathTextBox.Visibility = Visibility.Collapsed;
            SaveButton.Visibility = Visibility.Collapsed;
            SourceValidationIco.Visibility = Visibility.Collapsed;
            DestinationValidationIco.Visibility = Visibility.Collapsed;

            if (DI.isMonitoring)
            {
                MonitoringCheckBox.IsChecked = true;

                SourcePathTextBox.Text = DI.source;
                DestinationPathTextBox.Text = DI.destination;

                SourceLabel.Visibility = Visibility.Visible;
                SourcePathTextBox.Visibility = Visibility.Visible;
                DestinationLabel.Visibility = Visibility.Visible;
                DestinationPathTextBox.Visibility = Visibility.Visible;
            }
        }




        public bool ValidateDirectoryPath(string path)
        {
            bool valid = false;

            if (Directory.Exists(path))
                valid = true;

            return valid;
        }

        private void SourcePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SourcePathTextBox.Text.Length < DI.Name.Length || !SourcePathTextBox.Text.Substring(0, DI.Name.Length).Equals(DI.Name))
            {
                SourcePathTextBox.Text = DI.Name + SourcePathTextBox.Text;
                SourcePathTextBox.ScrollToEnd();
            }

            bool validility = ValidateDirectoryPath(SourcePathTextBox.Text);
            if (validility)
                SourceValidationIco.Visibility = Visibility.Visible;
            else
                SourceValidationIco.Visibility = Visibility.Collapsed;

            AdjustSaveVisibility();
        }

        private void DestinationPathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool validility = ValidateDirectoryPath(DestinationPathTextBox.Text);
            if (validility)
                DestinationValidationIco.Visibility = Visibility.Visible;
            else
                DestinationValidationIco.Visibility = Visibility.Collapsed;

            AdjustSaveVisibility();
        }

        private void MonitoringCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SourceLabel.Visibility = Visibility.Visible;
            SourcePathTextBox.Visibility = Visibility.Visible;
            DestinationLabel.Visibility = Visibility.Visible;
            DestinationPathTextBox.Visibility = Visibility.Visible;
            SourcePathTextBox_TextChanged(null,null);
            DestinationPathTextBox_TextChanged(null,null);
        }

        private void MonitoringCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            bool performSave = DisableMonitoringCheck();

            SourceLabel.Visibility = Visibility.Collapsed;
            SourcePathTextBox.Visibility = Visibility.Collapsed;
            DestinationLabel.Visibility = Visibility.Collapsed;
            DestinationPathTextBox.Visibility = Visibility.Collapsed;
            SaveButton.Visibility = Visibility.Collapsed;
            SourceValidationIco.Visibility = Visibility.Collapsed;
            DestinationValidationIco.Visibility = Visibility.Collapsed;

            if (performSave)
            {
                DI.isMonitoring = false;
                DI.save();
                driveEngine.ReloadAll();
            }
        }

        public bool DisableMonitoringCheck()
        {
            bool disable = false;

            if (DI.isMonitoring)
            {
                if (!MonitoringCheckBox.IsChecked ?? false)
                {
                    MessageBoxResult result = MessageBox.Show("Remove Monitoring From This Drive?", "Are you sure", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                        disable = true;
                }
            }
            return disable;
        }

        public void AdjustSaveVisibility()
        {
            if (DestinationValidationIco.Visibility == Visibility.Visible && SourceValidationIco.Visibility == Visibility.Visible)
            {
                SaveButton.Visibility = Visibility.Visible;
            }
            else
            {
                SaveButton.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DI.isMonitoring = true;
            DI.source = SourcePathTextBox.Text;
            DI.destination = DestinationPathTextBox.Text;

            DI.save();
            driveEngine.ReloadAll();
        }
    }
}
