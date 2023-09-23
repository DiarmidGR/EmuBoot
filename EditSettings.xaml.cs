using EmuBoot.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

namespace EmuBoot
{
    /// <summary>
    /// Interaction logic for FirstStartup.xaml
    /// </summary>
    public partial class EditSettings : Window
    {
        // Global variables
        Dictionary<string, string> _settings = new Dictionary<string, string>();
        string EmuPath;
        string RomsPath;

        public EditSettings(SettingsPropertyCollection properties)
        {
            InitializeComponent();
            Loaded += EditSettings_Loaded;
        }

        private void EditSettings_Loaded(object sender, RoutedEventArgs e)
        {
            EmuPath = Settings.Default.EmulatorsDirectory;
            LabelEmulators.Content = Settings.Default["EmulatorsDirectory"];

            RomsPath = Settings.Default.RomsDirectory;
            LabelRoms.Content = Settings.Default["RomsDirectory"];
        }

        private void BrowseFiles(Label label, string setting)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = EmuPath;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label.Content = $" {folderBrowserDialog.SelectedPath}";
                EmuPath = folderBrowserDialog.SelectedPath;
                Properties.Settings.Default[setting] = folderBrowserDialog.SelectedPath;
            }
        }

        private void BrowseEmulators_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "BrowseEmulators":
                        BrowseFiles(LabelEmulators, "EmulatorsDirectory");
                        break;

                    case "BrowseRoms":
                        BrowseFiles(LabelRoms, "RomsDirectory");
                        break;
                }
            }
        }

        private void BrowseRoms_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = RomsPath;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LabelRoms.Content = $" {folderBrowserDialog.SelectedPath}";
                RomsPath = folderBrowserDialog.SelectedPath;
                Properties.Settings.Default["RomsDirectory"] = folderBrowserDialog.SelectedPath;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(EmuPath))
            {
                MessageBox.Show($"Invalid emulator directory: {EmuPath}");
                return;
            }
            if (!Directory.Exists(RomsPath))
            {
                MessageBox.Show($"Invalid roms directory: {RomsPath}");
                return;
            }
            Properties.Settings.Default["RomsDirectory"] = RomsPath;
            Properties.Settings.Default["EmulatorsDirectory"] = EmuPath;
            Properties.Settings.Default.Save();
            this.DialogResult = true;
        }
    }
}
