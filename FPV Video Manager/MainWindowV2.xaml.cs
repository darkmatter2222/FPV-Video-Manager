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

namespace FPV_Video_Manager
{
    /// <summary>
    /// Interaction logic for MainWindowV2.xaml
    /// </summary>
    public partial class MainWindowV2 : Window
    {
        public MainWindowV2()
        {
            InitializeComponent();
            MainListingListBox.Items.Add(new ListBoxItem() { Content = new InterfaceControls.MainListingHeader(), IsHitTestVisible = false });
            MainListingListBox.Items.Add(new ListBoxItem() { Content = new InterfaceControls.MainListingHeader() });
        }
    }
}
