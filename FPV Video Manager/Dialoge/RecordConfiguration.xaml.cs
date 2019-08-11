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
using System.Text.RegularExpressions;

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
            RenderDecisions();
        }

        public void RenderDecisions()
        {
            bool isValid = true;
            string ErrorMessage = "";
            // validate date time format

            Dictionary<string, dynamic> dateTimeFormatResult = GetFormatedDateString();

            if (!((bool)dateTimeFormatResult["valid"]))
            {
                // invalid
                DestinationDateTimeFormatValidCheckMark.Visibility = Visibility.Collapsed;
                isValid = false;
            }
            else
            {
                if (IsLegal((string)dateTimeFormatResult["formattedTimeString"]))
                {
                    // valid
                    DestinationDateTimeFormatValidCheckMark.Visibility = Visibility.Visible;
                }
                else
                {
                    // invalid
                    DestinationDateTimeFormatValidCheckMark.Visibility = Visibility.Collapsed;
                    isValid = false;
                }
            }

            // validate prefix
            if (IsLegal(TargetPrefix.Text))
            {
                // valid
                DestinationPrefixValidCheckMark.Visibility = Visibility.Visible;
            }
            else
            {
                // invalid
                DestinationPrefixValidCheckMark.Visibility = Visibility.Collapsed;
                isValid = false;
            }

            // validate suffix
            if (IsLegal(TargetSuffix.Text))
            {
                // valid
                DestinationSuffixValidCheckMark.Visibility = Visibility.Visible;
            }
            else
            {
                // invalid
                DestinationSuffixValidCheckMark.Visibility = Visibility.Collapsed;
                isValid = false;
            }

            // construct Result
            string FinalResult = "";

            if (TargetPrefix.Text.Trim().Length > 0)
                FinalResult += $@"{TargetPrefix.Text.ToString()}-";

            if (((string)dateTimeFormatResult["formattedTimeString"]).Trim().Length > 0)
                FinalResult += ((string)dateTimeFormatResult["formattedTimeString"]).Trim();

            if (TargetSuffix.Text.Trim().Length > 0)
                FinalResult += $@"-{TargetSuffix.Text.ToString()}";

            if (!IsLegal(FinalResult))
            {
                isValid = false;
                ErrorMessage = "<Total Length is greater 200 Chars>";
            }


            // render Final String and Unlock Accept Button
            if (isValid)
            {
                // construct result

                FinalFormatLabel.Content = $@"Final Result: {FinalResult}";
                AcceptButton.IsEnabled = true;
            }
            else
            {
                DestinationDateTimeFormatValidCheckMark.Visibility = Visibility.Collapsed;
                FinalFormatLabel.Content = ErrorMessage;
                AcceptButton.IsEnabled = false;
            }
        }

        public bool IsLegal(string target)
        {
            bool valid = true;

            Regex unspupportedRegex = new Regex("(^(PRN|AUX|NUL|CON|COM[1-9]|LPT[1-9]|(\\.+)$)(\\..*)?$)|(([\\x00-\\x1f\\\\?*:\";‌​|/<>])+)|([\\.]+)", RegexOptions.IgnoreCase);

            if (unspupportedRegex.Match(target).Success)
            {
                valid = false;
            }

            if (target.Length > 200)
            {
                valid = false;
            }

            return valid;
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

        public Dictionary<string, dynamic> GetFormatedDateString()
        {
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();

            bool valid = false;

            string formattedTimeString = "";

            try
            {
                formattedTimeString = GetDesieredUserTime().ToString(TargetDateTimeFormat.Text);
                valid = true;
            }
            catch
            {
            }

            result.Add("valid", valid);
            result.Add("formattedTimeString", formattedTimeString);

            return result;
        }

        private void TargetPrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            RenderDecisions();
        }

        private void TargetSuffix_TextChanged(object sender, TextChangedEventArgs e)
        {
            RenderDecisions();
        }
    }
}
