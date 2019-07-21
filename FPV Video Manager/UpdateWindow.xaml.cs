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

namespace FPV_Video_Manager
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }

        private void ExitLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/darkmatter2222/FPV-Video-Manager/releases");
            }
            catch
            {
                MessageBox.Show("Ur browser didnt want to open, here ya go! https://github.com/darkmatter2222/FPV-Video-Manager/releases");
            }
            this.Close();
        }
    }
}
