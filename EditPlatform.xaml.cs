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
using Microsoft.Win32;

namespace GBASelector
{
    /// <summary>
    /// Interaction logic for EditPlatform.xaml
    /// </summary>
    public partial class EditPlatform : Window
    {
        public Platform Platform;
        public EditPlatform(Platform platform)
        {
            InitializeComponent();
            Platform = platform;
            txtPlatformName.Text = platform._PlatformName;
            lblEmu.Content = $"({Platform._EmuPath})";
            lblRoms.Content = $"({Platform._RomsPath})";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtPlatformName.Text.Length > 0)
            {
                Platform._PlatformName = txtPlatformName.Text;
            }
            this.DialogResult = true;
        }

        private void btnBrowseEmu_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select an Executable File";
            fileDialog.Filter = "Executable Files (*.exe)|*.exe";
            if (fileDialog.ShowDialog() == true)
            {
                Platform._EmuPath = fileDialog.FileName;
                lblEmu.Content = $"({Platform._EmuPath})";
            }
        }

        private void btnBrowseRoms_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = "C:\\";
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Platform._RomsPath = folderBrowserDialog.SelectedPath;
                lblRoms.Content = $" {Platform._RomsPath}";
            }
        }
    }
}
