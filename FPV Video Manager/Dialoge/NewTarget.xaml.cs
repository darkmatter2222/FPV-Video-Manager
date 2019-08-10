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

namespace FPV_Video_Manager.Dialoge
{
    /// <summary>
    /// Interaction logic for NewTarget.xaml
    /// </summary>
    public partial class NewTarget : UserControl
    {
        public NewTarget(string _target)
        {
            InitializeComponent();
            TargetTextBox.Text = _target;
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
    }
}
