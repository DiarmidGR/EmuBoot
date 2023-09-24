using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GBASelector.Models;
using Microsoft.Win32;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace EmuBoot
{
    /// <summary>
    /// Interaction logic for EditPlatform.xaml
    /// </summary>
    public partial class EditPlatform : Window
    {
        Platform Platform;
        public Platform NewPlatform { get; }
        public EditPlatform(Platform platform)
        {
            InitializeComponent();

            // Initialize our global variables
            Platform = platform;
            NewPlatform = platform;

            // Populate our UI elements
            TextName.Text = Platform.Name;
            LabelEmu.Content = $"'{Platform.EmuPath}'";
            LabelRoms.Content = $"'{Platform.GamePaths}'";
            LabelCovers.Content = $"'{Platform.CoverPaths}'";
            TextExtension.Text = Platform.Extension;
            this.Title = $"Edit {Platform.Name}";
        }

        private void BrowseEmu_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (File.Exists(Platform.EmuPath))
            {
                fileDialog.InitialDirectory = Platform.EmuPath;
            }
            fileDialog.Title = "Select Emulator Executable File";
            fileDialog.Filter = "Executable Files (*.exe)|*.exe";
            if (fileDialog.ShowDialog() == true)
            {
                NewPlatform.EmuPath = fileDialog.FileName;
                LabelEmu.Content = $"'{NewPlatform.EmuPath}'";
            }
        }

        private void BrowseRoms_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (File.Exists(Platform.GamePaths))
            {
                folderBrowserDialog.SelectedPath = Platform.GamePaths;
            }
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NewPlatform.GamePaths = folderBrowserDialog.SelectedPath;
                LabelRoms.Content = $"'{NewPlatform.GamePaths}'";
            }
        }

        private void BrowseCovers_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (File.Exists(Platform.CoverPaths))
            {
                folderBrowserDialog.SelectedPath = Platform.CoverPaths;
            }
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NewPlatform.CoverPaths = folderBrowserDialog.SelectedPath;
                LabelCovers.Content = $"'{NewPlatform.CoverPaths}'";
            }
        }

        private void SaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (TextExtension.Text.Length > 0 && TextName.Text.Length > 0)
            {
                NewPlatform.Extension = TextExtension.Text;
                NewPlatform.Name = TextName.Text;
            }
            this.DialogResult = true;
        }
    }
}
