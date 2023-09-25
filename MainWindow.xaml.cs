using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
using Newtonsoft.Json;
using Microsoft.Win32;
using GBASelector.Models;
using EmuBoot.Properties;
using GBASelector;
using System.Security.Policy;
using System.Configuration;

namespace EmuBoot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //global variables for the Add Platform page
        string _emulatorPath = Settings.Default.EmulatorsDirectory;
        string _romsPath = Settings.Default.RomsDirectory;
        string _coversPath = Settings.Default.CoversDirectory;
        List<Platform> listPlatforms = new List<Platform>();
        public MainWindow()
        {
            InitializeComponent();

            // Open first time startup dialog for user to input a couple settings
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Misc. loading actions
            lblEmu.Content = $"Path to emulator .exe file: '{Settings.Default.EmulatorsDirectory}'";
            lblRoms.Content = $"Path to roms folder: '{Settings.Default.RomsDirectory}'";
            lblCovers.Content = $"Path to covers folder: '{Settings.Default.CoversDirectory}'";
            if (Settings.Default.IsFullScreen)
            {
                CheckFullscreen.IsChecked = true;
            }
            DeSerializeObjects();
            ReLoadPlatforms();
        }

        private void OpenSettings()
        {
            EditSettings firstStartup = new EditSettings(Settings.Default.Properties);
            firstStartup.ShowDialog();
        }

        private void AddPlatformTab(Platform platform, int index)
        {
            platform.PlatformTab.Width = 100;
            platform.PlatformEdit += Platform_PlatformEdit;
            platform.PlatformDelete += Platform_PlatformDelete;
            tC.Items.Insert(index, platform.PlatformTab);
        }

        private void Platform_PlatformDelete(Platform obj)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show($"Delete platform {obj.Name}?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                tC.Items.RemoveAt(listPlatforms.IndexOf(obj));
                listPlatforms.Remove(obj);
                SerializeObjects();
            }
        }

        private void Platform_PlatformEdit(Platform obj)
        {
            tC.SelectedIndex = listPlatforms.IndexOf(obj);
            EditPlatform editPlatform = new EditPlatform(obj)
            {
                Owner = this
            };
            if (editPlatform.ShowDialog() == true)
            {
                int temp = tC.SelectedIndex;
                // Remove platforms old TabItem
                tC.Items.RemoveAt(temp);

                Platform editedPlatform = editPlatform.NewPlatform;
                Platform platform = new Platform(editedPlatform.Name, editedPlatform.Extension, editedPlatform.EmuPath, editedPlatform.GamePaths, editedPlatform.CoverPaths);


                // Insert new Platform where old Platform is in listPlatforms
                listPlatforms.Insert(listPlatforms.IndexOf(obj), platform);

                // Remove old Platform from listPlatforms
                listPlatforms.Remove(obj);
                AddPlatformTab(platform, temp);

                SerializeObjects();
                tC.SelectedIndex = tC.Items.Count - 2;
            }
        }

        private void ReLoadPlatforms()
        {
            int tempIndex = tC.SelectedIndex;
            DeSerializeObjects();
            int tabCount = tC.Items.Count;
            for(int i = 0; i < tabCount - 1; i++)
            {
                tC.Items.RemoveAt(0);
            }
            int index = 0;
            foreach(Platform platform in listPlatforms)
            {
                AddPlatformTab(platform, index);
                index++;
            }
            SerializeObjects();
            tC.SelectedIndex = tempIndex;
        }

        // Loading and unloading Platforms.json data.
        private void SerializeObjects()
        {
            string json = JsonConvert.SerializeObject(listPlatforms);

            File.WriteAllText("Platforms.json", json);
        }

        private void DeSerializeObjects()
        {
            try
            {
                string json = File.ReadAllText("Platforms.json");
                if(JsonConvert.DeserializeObject<List<Platform>>(json) != null)
                {
                    listPlatforms = JsonConvert.DeserializeObject<List<Platform>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Deserializing Platforms.json: {ex.Message}");
            }
        }

        // Event Handlers
        private void btnAddPlatform_Click(object sender, RoutedEventArgs e)
        {
            if (_emulatorPath != null && _emulatorPath.Length > 0 && _romsPath != null && _romsPath.Length > 0 && _coversPath != null && _coversPath.Length > 0)
            {
                Platform platform = new Platform(txtPlatform.Text, txtExtension.Text, _emulatorPath, _romsPath, _coversPath);
                listPlatforms.Add(platform);
                AddPlatformTab(platform, listPlatforms.Count - 1);

                txtPlatform.Text = "";
                txtExtension.Text = "";

                SerializeObjects();
            }
        }

        private void btnBrowseEmu_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select Emulator Executable File";
            fileDialog.Filter = "Executable Files (*.exe)|*.exe";
            if (fileDialog.ShowDialog() == true)
            {
                _emulatorPath = fileDialog.FileName;
                lblEmu.Content = $"Path to emulator .exe file: '{_emulatorPath}'";
            }
        }

        private void btnBrowseRoms_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = Settings.Default.RomsDirectory;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _romsPath = folderBrowserDialog.SelectedPath;
                lblRoms.Content = $"Path to roms folder: '{_romsPath}'";
            }
        }

        private void btnBrowseCovers_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = Settings.Default.CoversDirectory;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _coversPath = folderBrowserDialog.SelectedPath;
                lblCovers.Content = $"Path to covers folder: '{_coversPath}'";
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SerializeObjects();
            Close();
        }

        private void btnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            button.Background = Brushes.Red;
        }

        private void btnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            button.Background = Brushes.Transparent;
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            OpenSettings();
        }

        private void ButtonGridView_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.IsGridView = true;
            Settings.Default.Save();
            SerializeObjects();
            ReLoadPlatforms();
        }

        private void ButtonListView_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.IsGridView = false;
            Settings.Default.Save();
            SerializeObjects();
            ReLoadPlatforms();
        }

        private void CheckFullscreen_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.IsFullScreen = true;
            Settings.Default.Save();
        }

        private void CheckFullscreen_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.IsFullScreen = false;
            Settings.Default.Save();
        }
    }
}
