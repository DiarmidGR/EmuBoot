﻿using System;
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
using Wpf.Ui.Controls;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace GBASelector
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
            
            if (listPlatforms != null && listPlatforms.Count > 0)
            {
                foreach (Platform platform in listPlatforms)
                {
                    platform.CreateGrid(tC);
                }
            }
            tC.SelectedIndex = 0;
        }

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

        private void btnAddPlatform_Click(object sender, RoutedEventArgs e)
        {
            if (_emuPath != null && _emuPath.Length > 0 && _romsPath != null && _emuPath.Length > 0)
            {
                Platform platform = new Platform(txtPlatform.Text, txtExtension.Text, _emuPath, _romsPath);
                platform.CreateGrid(tC);
                if (listPlatforms==null)
                {
                    listPlatforms = new List<Platform>();
                }
                listPlatforms.Insert(listPlatforms.Count, platform);
            }
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
