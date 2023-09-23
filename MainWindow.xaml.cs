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

namespace EmuBoot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Platform> listPlatforms = new List<Platform>();
        string _emuPath;
        string _romsPath;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DeSerializeObjects();
            
            GeneratePlatforms();
            tC.SelectedIndex = 0;
        }

        private void GeneratePlatforms()
        {
            if (listPlatforms != null && listPlatforms.Count > 0)
            {
                int index = 0;
                foreach (Platform platform in listPlatforms)
                {
                    platform.ScanDirectory();
                    platform.CreateGrid(tC, index);
                    platform.PlatformDelete += DeletePlatform;
                    platform.PlatformEdit += EditPlatform;
                    index++;
                }
            }
        }

        private void GeneratePlatform(Platform platform, int index)
        {
            platform.ScanDirectory();
            platform.CreateGrid(tC, index);
            platform.PlatformDelete += DeletePlatform;
            platform.PlatformEdit += EditPlatform;
        }

        private void EditPlatform(Platform platform)
        {
            tC.SelectedIndex = listPlatforms.IndexOf(platform);
            EditPlatform editPlatform = new EditPlatform(platform)
            {
                Owner = this
            };
            if (editPlatform.ShowDialog() == true)
            {
                int index = listPlatforms.IndexOf(platform);
                listPlatforms.Remove(platform);
                tC.Items.RemoveAt(index);
                Platform editedPlatform = new Platform(editPlatform.Platform._PlatformName,
                    editPlatform.Platform._FileExtension, editPlatform.Platform._EmuPath,
                    editPlatform.Platform._RomsPath);
                listPlatforms.Insert(index, editedPlatform);
                GeneratePlatform(editedPlatform, index);
                tC.SelectedIndex = listPlatforms.IndexOf(editedPlatform);
            }
            SerializeObjects();
        }

        private void DeletePlatform(Platform platform)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show($"Delete platform {platform._PlatformName}?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                //remove the item from the TabControl, index of TabItem would be equal to index of platform
                int index = listPlatforms.IndexOf(platform);
                listPlatforms.RemoveAt(index);
                tC.Items.RemoveAt(index);
            }
            SerializeObjects();
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
                listPlatforms = JsonConvert.DeserializeObject<List<Platform>>(json);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error Deserializing Platforms.json: {ex.Message}");
            }
        }

        // Event Handlers
        private void btnAddPlatform_Click(object sender, RoutedEventArgs e)
        {
            if (_emuPath != null && _emuPath.Length > 0 && _romsPath != null && _emuPath.Length > 0)
            {
                if (listPlatforms == null)
                    listPlatforms = new List<Platform>();
                Platform platform = new Platform(txtPlatform.Text, txtExtension.Text, _emuPath, _romsPath);
                listPlatforms.Insert(listPlatforms.Count, platform);
                GeneratePlatform(platform, listPlatforms.Count-1);
                tC.SelectedIndex = listPlatforms.IndexOf(platform);
                SerializeObjects();
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

        private void btnBrowseEmu_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select an Executable File";
            fileDialog.Filter = "Executable Files (*.exe)|*.exe";
            if (fileDialog.ShowDialog() == true)
            {
                _emuPath = fileDialog.FileName;
                lblEmu.Content += $" {_emuPath}";
            }
        }

        private void btnBrowseRoms_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = "C:\\";
            if(folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _romsPath = folderBrowserDialog.SelectedPath;
                lblRoms.Content += $" {_romsPath}";
            }
        }
    }
}
