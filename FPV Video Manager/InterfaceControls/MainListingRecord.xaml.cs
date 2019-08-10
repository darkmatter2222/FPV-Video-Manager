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

namespace FPV_Video_Manager.InterfaceControls
{
    /// <summary>
    /// Interaction logic for MainListingRecord.xaml
    /// </summary>
    public partial class MainListingRecord : UserControl
    {
        public bool audiableNotification = false;
        public bool autoCompression = false;
        public MainListingRecord()
        {
            InitializeComponent();
        }

        private async void SourceElip_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var view = new Dialoge.NewTarget(SourceTextBox.Text);

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            SourceTextBox.Text = view.TargetTextBox.Text;
        }

        private async void DestElip_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var view = new Dialoge.NewTarget(DestTextBox.Text);

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            DestTextBox.Text = view.TargetTextBox.Text;
        }

        private async void SettingsButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var view = new Dialoge.RecordConfiguration(audiableNotification, autoCompression);

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }
    }
}
