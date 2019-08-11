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

        public void AddConfigToInterface(string[] sourceID)
        {
            
        }

        public void RemoveConfigFromInterface()
        {
            foreach (ListBoxItem LBI in MainListingListBox.Items)
            {
                
            }
        }
    }
}
