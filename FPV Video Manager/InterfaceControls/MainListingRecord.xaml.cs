﻿using System;
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
using System.IO;
using Engine;

namespace FPV_Video_Manager.InterfaceControls
{
    /// <summary>
    /// Interaction logic for MainListingRecord.xaml
    /// </summary>
    public partial class MainListingRecord : UserControl
    {
        GlobalVariables Globals = new GlobalVariables();
        public RecordConfig recordConfig;
        InterlacingLayer.InterlacingConfiguration interlacingConfiguration = new InterlacingConfiguration();

        public bool recordComitted = false;
        public bool elementChanged = false;
        public ListBoxItem ParrentLBI = null;
        public DriveInformation driveInformation;

        public MainListingRecord(RecordConfig _rc, DriveInformation _driveInformation)
        {
            recordConfig = _rc;
            driveInformation = _driveInformation;
            
            InitializeComponent();
            if (_driveInformation != null)
            {
                SourceTextBox.Text = recordConfig.source.Replace(recordConfig.sourceID, driveInformation.Name);
                
            }
            else
            {
                SourceTextBox.Text = recordConfig.source;
            }

            DestTextBox.Text = recordConfig.destination;

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
                    recordConfig.source = SourceTextBox.Text;
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
                    recordConfig.destination = DestTextBox.Text;
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
            // get ID from Source, missing? create
            string sourceRoot = Directory.GetDirectoryRoot(SourceTextBox.Text);
            string destRoot = Directory.GetDirectoryRoot(DestTextBox.Text);

            if (sourceRoot.Equals(destRoot))
            {
                MessageBox.Show("This app does not support same source and destination root at this time.");
                // TODO
                return;
            }

            if (driveInformation == null)
            {
                foreach (DriveInformation Drive in new DriveEngine().knownConnectedDrives)
                {
                    if (Drive.Name.Equals(sourceRoot))
                    {
                        recordConfig.sourceID = Drive.GetIDElseCreateandGet();
                        break;
                    }
                }
            }
            else
            {
                recordConfig.sourceID = driveInformation.GetIDElseCreateandGet();
            }

            recordConfig.source = recordConfig.source.Replace(sourceRoot, recordConfig.sourceID);

            interlacingConfiguration.SaveRecord(recordConfig);

            elementChanged = false;
            recordComitted = true;

            StatusUpdate();
        }

        private async void RecordTrashButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var view = new Dialoge.AreYouSure();

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            if (view.IsAccepted)
            {
                interlacingConfiguration.DeleteRecord(recordConfig);

                if (view.IsAccepted)
                    Globals.MWV2.MainListingListBox.Items.Remove(ParrentLBI);
            }
        }
    }
}
