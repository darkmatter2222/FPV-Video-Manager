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
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InterlacingLayer;
using Engine;

namespace FPV_Video_Manager
{
    /// <summary>
    /// Interaction logic for MainWindowV2.xaml
    /// </summary>
    public partial class MainWindowV2 : Window
    {
        GlobalVariables Globals = new GlobalVariables();
        DriveEngine driveEngine = new DriveEngine();

        public MainWindowV2()
        {
            new InterlacingConfiguration().LoadRecords();

            driveEngine.InitializeEngine();
            driveEngine.DriveChange += DriveEngine__DriveChange;

            InitializeComponent();
            Globals.MWV2 = this;
            MainListingListBox.Items.Add(new ListBoxItem() { Content = new InterfaceControls.MainListingHeader(), IsHitTestVisible = false });
        }


        private void DriveEngine__DriveChange(List<DriveInformation> _LDI, EventArgs e)
        {
            Dispatcher.Invoke(new Action(() => UpdateInterface(_LDI)));
        }

        private void MainListingListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainListingListBox.SelectedIndex != -1)
                MainListingListBox.SelectedIndex = -1;
        }

        private void MenuItemEditNew_Click(object sender, RoutedEventArgs e)
        {
            var TargetContent = new InterfaceControls.MainListingRecord(new RecordConfig());
            ListBoxItem LBI = new ListBoxItem() { Content = TargetContent };
            TargetContent.ParrentLBI = LBI;
            MainListingListBox.Items.Add(LBI);
        }

        public void UpdateInterface(List<DriveInformation> _LDI)
        {
            List<ListBoxItem> LBIsToRemove = new List<ListBoxItem>();

            foreach (DriveInformation driveInformation in _LDI)
            {
                if (driveInformation.sourceID != null)
                {
                    bool IDOnInterface = false;
                    foreach (ListBoxItem LBI in MainListingListBox.Items)
                    {
                        if (LBI.Content.GetType() == typeof(InterfaceControls.MainListingRecord))
                        {
                            if (((InterfaceControls.MainListingRecord)LBI.Content).recordConfig.sourceID.Equals(driveInformation.sourceID))
                            {
                                IDOnInterface = true;
                            }
                        }
                    }

                    if (!IDOnInterface)
                    {
                        bool recordAdded = false;
                        foreach (RecordConfig recordConfig in new InterlacingConfiguration().GetRecords())
                        {
                            if (recordConfig.sourceID.Equals(driveInformation.sourceID))
                            {
                                var TargetContent = new InterfaceControls.MainListingRecord(recordConfig);
                                ListBoxItem LBI = new ListBoxItem() { Content = TargetContent };
                                TargetContent.ParrentLBI = LBI;
                                MainListingListBox.Items.Add(LBI);
                                recordAdded = true;
                            }
                        }
                    }
                }
            }

            foreach (ListBoxItem LBI in MainListingListBox.Items)
            {
                bool idFound = false;
                if (LBI.Content.GetType() == typeof(InterfaceControls.MainListingRecord))
                {
                    foreach (DriveInformation driveInformation in _LDI)
                    {
                        if (driveInformation.sourceID != null)
                        {
                            if (driveInformation.sourceID.Equals(((InterfaceControls.MainListingRecord)LBI.Content).recordConfig.sourceID))
                            {
                                idFound = true;
                            }
                        }
                    }
                }
                if (!idFound)
                {
                    LBIsToRemove.Add(LBI);
                }
            }

            foreach (ListBoxItem LBI in LBIsToRemove)
            {
                MainListingListBox.Items.Remove(LBI);
            }

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            driveEngine.TerminateEngine();
            new GlobalEngineSwitch().AllEnginesRunning = false;
        }
    }
}
