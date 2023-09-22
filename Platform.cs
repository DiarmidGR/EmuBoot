using System.Collections.Generic;
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
    public class Platform
    {
        readonly BitmapImage NoCover = new BitmapImage(new Uri("/images/no-cover.png", UriKind.RelativeOrAbsolute));
        public string _PlatformName;
        public string _FileExtension;
        public string _EmuPath;
        public string _RomsPath;
        public List<string> _FilePaths = new List<string>();
        private Grid _Grid;
        private Grid _PopulatedGrid;
        private Border _ImageBorder;

        // Values to handle default game card dimensions.
        readonly int iColumnWidth = 256;

        public event Action<Platform> PlatformDelete;
        public event Action<Platform> PlatformEdit;

        public Platform(string platformName, string fileExtension, string emuPath, string romsPath)
        {
            _PlatformName = platformName;
            _FileExtension = fileExtension;
            _EmuPath = emuPath;
            _RomsPath = romsPath;
            ScanDirectory();
        }

        /// <summary>
        /// Method to scan the directory string stored in _RomsPath in order to make a list of
        /// this platforms rom file paths stored in _FilePaths
        /// </summary>
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
        /// <summary>
        /// Method to create the grid that we will populate with all of our clickable rom shortcuts
        /// </summary>
        /// <param name="tabControl"></param>
        public void CreateGrid(TabControl tabControl)
        {
            // Create our TabItem for our TabControl
            TabItem tabItem = new TabItem
            {
                Header = _PlatformName
            };

            // Create our Grid
            _Grid = new Grid();
            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = _Grid
            };
            tabItem.Content = scrollViewer;
            for (int i = 0; i < 3; i++)
            {
                RowDefinition rowDefinition = new RowDefinition
                {
                    Height = GridLength.Auto
                };
                _Grid.RowDefinitions.Add(rowDefinition);
            }
            for (int i = 0; i < 4; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition
                {
                    Width = new GridLength(iColumnWidth + 4)
                };
                _Grid.ColumnDefinitions.Add(columnDefinition);
            }

            // Add event handlers to our tabItem
            tabItem.PreviewMouseLeftButtonDown += TabItem_PreviewMouseLeftButtonDown;

            ContextMenu contextMenu = new ContextMenu();

            // Edit item within ContextMenu
            MenuItem miEdit = new MenuItem
            {
                Header = "Edit"
            };
            miEdit.Click += MiEdit_Click;
            contextMenu.Items.Add(miEdit);

            // Delete item within  ContextMenu
            MenuItem miDelete = new MenuItem
            {
                Header = "Delete"
            };
            miDelete.Click += MiDelete_Click;
            contextMenu.Items.Add(miDelete);

            tabItem.ContextMenu = contextMenu;

            // Append our final item to the page
            tabControl.Items.Insert(tabControl.Items.Count-1,tabItem);
            PopulateGrid();
        }




        /// <summary>
        /// Method to populate the grid created in CreateGrid method with our clickable rom shortcuts
        /// </summary>
        public void PopulateGrid()
        {
            int temp = 0;
            if (_PopulatedGrid is null)
            {
                for (int row = 0; row < _Grid.RowDefinitions.Count(); row++)
                {
                    for (int col = 0; col < _Grid.ColumnDefinitions.Count(); col++)
                    {
                        // Exit condition so we don't go out of range with our list index (temp)
                        if (temp >= _FilePaths.Count())
                            break;

                        Image cover = new Image
                        {
                            Width = iColumnWidth,
                            Stretch = Stretch.Uniform
                        };
                        string coverPath = _RomsPath + "\\Covers\\" + Path.ChangeExtension(Path.GetFileName(_FilePaths[temp]), ".png").ToString();
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
                            Tag = Path.Combine(_EmuPath, _FilePaths[temp]),
                            BorderBrush = Brushes.Transparent, // Set the border color
                            BorderThickness = new Thickness(2), // Set the border thickness
                            Margin = new Thickness(2), // Adjust margin as needed
                        };
                        imageBorder.MouseEnter += ImageBorder_MouseEnter;
                        imageBorder.MouseLeave += ImageBorder_MouseLeave;
                        imageBorder.MouseDown += ImageBorder_MouseDown;
                        imageBorder.Child = cover;
                        _ImageBorder = imageBorder;
                        _Grid.Children.Add(_ImageBorder);
                        Grid.SetRow(imageBorder, row);
                        Grid.SetColumn(imageBorder, col);
                        temp++;
                    }
                }
                _PopulatedGrid = _Grid;
            }
            else
            {
                _Grid = _PopulatedGrid;
            }
        }

        // Event Handlers
        private void MiDelete_Click(object sender, RoutedEventArgs e)
        {
            PlatformDelete?.Invoke(this);
        }
        private void MiEdit_Click(object sender, RoutedEventArgs e)
        {
            PlatformEdit?.Invoke(this);
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
