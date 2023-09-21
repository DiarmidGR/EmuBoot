﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;
using GBASelector.Properties;

namespace GBASelector
{
    internal class Platform
    {
        readonly BitmapImage NoCover = new BitmapImage(new Uri("/images/no-cover.png", UriKind.RelativeOrAbsolute));
        public string _PlatformName;
        public string _FileExtension;
        public string _EmuPath;
        public string _RomsPath;
        public List<string> _FilePaths = new List<string>();
        private Grid _Grid;

        public event Action<Platform> PlatformDelete;

        public Platform(string platformName, string fileExtension, string emuPath, string romsPath)
        {
            _PlatformName = platformName;
            _FileExtension = fileExtension;
            _EmuPath = emuPath;
            _RomsPath = romsPath;
            ScanDirectory();
        }

        public void ScanDirectory()
        {
            try
            {
                if (!string.IsNullOrEmpty(_RomsPath) && Directory.Exists(_RomsPath))
                {
                    // Get a list of file names in the specified directory.
                    _FilePaths = Directory.GetFiles(_RomsPath, "*" + _FileExtension).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public void CreateGrid(TabControl tabControl)
        {
            // Create our TabItem for our TabControl
            TabItem tabItem = new TabItem();
            tabItem.Header = _PlatformName;

            // Create our Grid
            _Grid = new Grid();
            tabItem.Content = _Grid;
            for (int i = 0; i < 3; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new System.Windows.GridLength(260);
                _Grid.RowDefinitions.Add(rowDefinition);
            }
            for (int i = 0; i < 4; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new System.Windows.GridLength(260);
                _Grid.ColumnDefinitions.Add(columnDefinition);
            }

            // Add event handlers to our tabItem
            tabItem.PreviewMouseLeftButtonDown += TabItem_PreviewMouseLeftButtonDown;

            // Context menu
            ContextMenu contextMenu = new ContextMenu();
            MenuItem miEdit = new MenuItem
            {
                Header = "Edit"
            };
            contextMenu.Items.Add(miEdit);
            MenuItem miDelete = new MenuItem
            {
                Header = "Delete"
            };
            miDelete.Click += MiDelete_Click;
            contextMenu.Items.Add(miDelete);

            tabItem.ContextMenu = contextMenu;

            // Append our final item to the page
            tabControl.Items.Insert(0,tabItem);
            PopulateGrid();
        }

        private void MiDelete_Click(object sender, RoutedEventArgs e)
        {
            PlatformDelete?.Invoke(this);
        }

        private void PopulateGrid()
        {
            int temp = 0;

            for (int row = 0; row < _Grid.RowDefinitions.Count(); row++)
            {
                for (int col = 0; col < _Grid.ColumnDefinitions.Count(); col++)
                {
                    // Exit condition so we don't go out of range with our list index (temp)
                    if (temp >= _FilePaths.Count())
                        break;

                    Image cover = new Image
                    {
                        Width = 256,
                        Height = 256,
                        Stretch = Stretch.Fill
                    };
                    string coverPath = _RomsPath + "\\Covers\\" + Path.ChangeExtension(Path.GetFileName(_FilePaths[temp]), ".png").ToString();
                    Console.WriteLine(coverPath);
                    if (File.Exists(coverPath))
                    {
                        BitmapImage bitmap = new BitmapImage(new Uri(coverPath, UriKind.RelativeOrAbsolute));
                        cover.Source = bitmap;
                    }
                    else
                    {
                        cover.Source = NoCover;
                    }
                    // Add border to image
                    Border imageBorder = new Border
                    {
                        Tag = Path.Combine(_EmuPath,  _FilePaths[temp]),
                        BorderBrush = Brushes.Transparent, // Set the border color
                        BorderThickness = new Thickness(2), // Set the border thickness
                        Margin = new Thickness(2) // Adjust margin as needed
                    };
                    imageBorder.MouseEnter += ImageBorder_MouseEnter;
                    imageBorder.MouseLeave += ImageBorder_MouseLeave;
                    imageBorder.MouseDown += ImageBorder_MouseDown;
                    imageBorder.Child = cover;

                    _Grid.Children.Add(imageBorder);
                    Grid.SetRow(imageBorder, row);
                    Grid.SetColumn(imageBorder, col);
                    temp++;
                }
            }
        }

        private void ImageBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is string gamePath)
            {
                Process.Start(gamePath);
            }
        }

        private void ImageBorder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = (Border)sender;
            border.BorderBrush = Brushes.Transparent;
        }

        private void ImageBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = (Border)sender;
            border.BorderBrush = Brushes.Aquamarine;
        }

        private void TabItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PopulateGrid();
        }
    }
}
