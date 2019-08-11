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

namespace FPV_Video_Manager.Dialoge
{
    /// <summary>
    /// Interaction logic for RecordConfiguration.xaml
    /// </summary>
    public partial class RecordConfiguration : UserControl
    {
        public RecordConfiguration(bool _audiableNotification, bool _autoCompression, string _sourceID, string _destinationTargetFormat, string _targetTimeZone)
        {
            InitializeComponent();
            AudiableCheckBox.IsChecked = _audiableNotification;
            AutoCompressionCheckBox.IsChecked = _autoCompression;
            SourceIDTextBox.Text = _sourceID;

            bool recordFound = false;
            foreach (ComboBoxItem CBI in TargetTimeZoneComboBox.Items)
            {
                if (CBI.Content.Equals(_targetTimeZone))
                {
                    TargetTimeZoneComboBox.SelectedItem = CBI;
                    recordFound = true;
                    break;
                }
            }

            if (!recordFound)
            {
                MessageBox.Show($@"Heads up, Unable to set timezone {_targetTimeZone} as it does not exist in the combo box any longer. Please reselect and save.");
            }

            TargetDateTimeFormat.Text = _destinationTargetFormat;
        }

        private void TimeFormatQuestionButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings");
            }
            catch
            {
                MessageBox.Show("https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings");
            }
        }

        private void TargetDateTimeFormat_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string result = GetDesieredUserTime().ToString(TargetDateTimeFormat.Text);
                DestinationDateTimeFormatValidCheckMark.Visibility = Visibility.Visible;
                SampleFormatLabel.Content = result;
                AcceptButton.IsEnabled = true;
            }
            catch
            {
                DestinationDateTimeFormatValidCheckMark.Visibility = Visibility.Collapsed;
                SampleFormatLabel.Content = "";
                AcceptButton.IsEnabled = false;
            }
        }

        public DateTime GetDesieredUserTime()
        {
            switch (((ComboBoxItem)TargetTimeZoneComboBox.SelectedItem).Content)
            {
                case "System Time":
                    return DateTime.Now;
                case "UTC Time":
                    return DateTime.UtcNow;
                default :
                    return DateTime.UtcNow;
            }
        }
    }
}
