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

namespace FPV_Video_Manager.InterfaceControls
{
    /// <summary>
    /// Interaction logic for Drive.xaml
    /// </summary>
    public partial class Drive : UserControl
    {
        public DriveInformation DI;

        public Drive(DriveInformation _DI)
        {
            InitializeComponent();

            DI = _DI;

            DriveNameTextBlock.Text = _DI.Name;

            if (_DI.isMonitoring)
                MonitoringIc.Visibility = Visibility.Visible;
            else
                MonitoringIc.Visibility = Visibility.Collapsed;
        }
    }
}
