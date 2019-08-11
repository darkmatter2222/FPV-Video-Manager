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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
using System.IO;

namespace FPV_Video_Manager.Dialoge
{
    /// <summary>
    /// Interaction logic for NewTarget.xaml
    /// </summary>
    public partial class NewTarget : UserControl
    {
        public bool cxled = false;
        public NewTarget(string _target)
        {
            InitializeComponent();
            TargetTextBox.Text = _target;
            TargetTextBox_TextChanged(null,null);
        }

        private void FolderAddButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Select Root Directory";
            dlg.IsFolderPicker = true;
            //dlg.InitialDirectory = currentDirectory;

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            //dlg.DefaultDirectory = currentDirectory;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folder = dlg.FileName;
                TargetTextBox.Text = folder;
                // Do something with selected folder string
            }
        }

        private void CxlButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            cxled = true;
        }

        private void TargetTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // perform directory validation
            bool validDirectory = Directory.Exists(TargetTextBox.Text);

            if (validDirectory)
            {
                ValidDirectoryTextBlock.Visibility = Visibility.Collapsed;
                AcceptButton.IsEnabled = true;
            }
            else
            {
                ValidDirectoryTextBlock.Visibility = Visibility.Visible;
                AcceptButton.IsEnabled = false;
            }
        }

        private void AcceptButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!TargetTextBox.Text.Substring(TargetTextBox.Text.Length - 1, 1).Equals(@"\"))
                TargetTextBox.Text += @"\";
        }
    }
}
