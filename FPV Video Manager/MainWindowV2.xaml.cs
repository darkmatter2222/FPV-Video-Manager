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

namespace FPV_Video_Manager
{
    /// <summary>
    /// Interaction logic for MainWindowV2.xaml
    /// </summary>
    public partial class MainWindowV2 : Window
    {
        GlobalVariables Globals = new GlobalVariables();
        public MainWindowV2()
        {
            InitializeComponent();
            Globals.MWV2 = this;
            MainListingListBox.Items.Add(new ListBoxItem() { Content = new InterfaceControls.MainListingHeader(), IsHitTestVisible = false });
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

        public void UpdateInterface(string[] DesieredIDs)
        {
            List<string> IDsToAdd = new List<string>();
            List<string> IDsToRemove = new List<string>();


            foreach (string DesieredID in DesieredIDs)
            {
                bool IDOnInterface = false;
                foreach (ListBoxItem LBI in MainListingListBox.Items)
                {
                    if (((RecordConfig)LBI.Content).sourceID.Equals(DesieredID))
                    {
                        IDOnInterface = true;
                    }
                }

                if (!IDOnInterface)
                {
                    bool recordAdded = false;
                    foreach (RecordConfig recordConfig in new InterlacingConfiguration().GetRecords())
                    {
                        if (recordConfig.sourceID.Equals(DesieredID))
                        {
                            var TargetContent = new InterfaceControls.MainListingRecord(recordConfig);
                            ListBoxItem LBI = new ListBoxItem() { Content = TargetContent };
                            TargetContent.ParrentLBI = LBI;
                            MainListingListBox.Items.Add(LBI);
                            recordAdded = true;
                            break;
                        }
                    }

                    if (!recordAdded)
                    {
                        MessageBox.Show("ShitHitFan?");
                    }
                }
            }
        }
    }
}
