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
        public RecordConfiguration(bool _audiableNotification, bool _autoCompression)
        {
            InitializeComponent();
        }
    }
}
