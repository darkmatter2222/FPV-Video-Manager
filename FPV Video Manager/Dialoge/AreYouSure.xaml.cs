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
    /// Interaction logic for AreYouSure.xaml
    /// </summary>
    public partial class AreYouSure : UserControl
    {
        public bool IsAccepted = false;
        public AreYouSure()
        {
            InitializeComponent();
        }

        private void YesButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsAccepted = true;
        }

        private void NoButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsAccepted = false;
        }
    }
}
