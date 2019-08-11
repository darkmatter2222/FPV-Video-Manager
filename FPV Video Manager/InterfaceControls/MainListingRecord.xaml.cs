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
using MaterialDesignThemes.Wpf;
using InterlacingLayer;

namespace FPV_Video_Manager.InterfaceControls
{
    /// <summary>
    /// Interaction logic for MainListingRecord.xaml
    /// </summary>
    public partial class MainListingRecord : UserControl
    {
        GlobalVariables Globals = new GlobalVariables();
        RecordConfig recordConfig;

        public bool recordComitted = false;
        public bool elementChanged = false;
        public ListBoxItem ParrentLBI = null;

        public MainListingRecord(RecordConfig _rc)
        {
            recordConfig = _rc;
            InitializeComponent();
            StatusUpdate();
        }

        private async void SourceElip_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var view = new Dialoge.NewTarget(SourceTextBox.Text);

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            if (!view.cxled)
            {
                if (!SourceTextBox.Text.Equals(view.TargetTextBox.Text))
                {
                    elementChanged = true;
                    SourceTextBox.Text = view.TargetTextBox.Text;
                }
            }

            StatusUpdate();
        }

        private async void DestElip_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var view = new Dialoge.NewTarget(DestTextBox.Text);

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            if (!view.cxled)
            {
                if (!DestTextBox.Text.Equals(view.TargetTextBox.Text))
                {
                    elementChanged = true;
                    DestTextBox.Text = view.TargetTextBox.Text;
                }
            }

            StatusUpdate();
        }

        private async void RecordSettingsButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var view = new Dialoge.RecordConfiguration(recordConfig);

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            if (!view.isCxled)
            {
                if (recordConfig.audiableNotification != (view.AudiableCheckBox.IsChecked ?? false))
                {
                    elementChanged = true;
                    recordConfig.audiableNotification = view.AudiableCheckBox.IsChecked ?? false;
                }

                if (recordConfig.autoCompression != (view.AutoCompressionCheckBox.IsChecked ?? false))
                {
                    elementChanged = true;
                    recordConfig.autoCompression = view.AutoCompressionCheckBox.IsChecked ?? false;
                }

                if (!recordConfig.fileNameing.Equals(((ComboBoxItem)view.FileNamingComboBox.SelectedItem).Content.ToString()))
                {
                    elementChanged = true;
                    recordConfig.fileNameing = ((ComboBoxItem)view.FileNamingComboBox.SelectedItem).Content.ToString();
                }

                if (!recordConfig.targetTimeZone.Equals(((ComboBoxItem)view.TargetTimeZoneComboBox.SelectedItem).Content.ToString()))
                {
                    elementChanged = true;
                    recordConfig.targetTimeZone = ((ComboBoxItem)view.TargetTimeZoneComboBox.SelectedItem).Content.ToString();
                }

                if (!recordConfig.destinationTargetFormat.Equals(view.TargetDateTimeFormat.Text))
                {
                    elementChanged = true;
                    recordConfig.destinationTargetFormat = view.TargetDateTimeFormat.Text;
                }

                if (!recordConfig.targetPrefix.Equals(view.TargetPrefix.Text))
                {
                    elementChanged = true;
                    recordConfig.targetPrefix = view.TargetPrefix.Text;
                }

                if (!recordConfig.targetSuffix.Equals(view.TargetSuffix.Text))
                {
                    elementChanged = true;
                    recordConfig.targetSuffix = view.TargetSuffix.Text;
                }
            }



            StatusUpdate();
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        public void StatusUpdate()
        {
            bool RecordComplete = true;

            if (string.IsNullOrWhiteSpace(SourceTextBox.Text) || SourceTextBox.Text.Length < 1)
                RecordComplete = false;

            if (string.IsNullOrWhiteSpace(DestTextBox.Text) || DestTextBox.Text.Length < 1)
                RecordComplete = false;

            if (!RecordComplete)
            {
                StatusTextBox.Text = "Pending Record Completion...";
                RecordSaveButton.Visibility = Visibility.Collapsed;
                return;
            }

            if (elementChanged)
                RecordSaveButton.Visibility = Visibility.Visible;
            else
                RecordSaveButton.Visibility = Visibility.Collapsed;

            if (elementChanged)
            {
                StatusTextBox.Text = "Pending Record Save...";
                recordComitted = false;
                return;
            }

            StatusTextBox.Text = "Pending Engine Cycle";
            return;

            // perform record Saved Check
        }

        private void RecordSaveButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (recordConfig.sourceID.Equals("Pending Save..."))
                recordConfig.sourceID = Guid.NewGuid().ToString();

            elementChanged = false;
            recordComitted = true;

            StatusUpdate();
        }

        private async void RecordTrashButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var view = new Dialoge.AreYouSure();

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            if(view.IsAccepted)
                Globals.MWV2.MainListingListBox.Items.Remove(ParrentLBI);
        }
    }
}
