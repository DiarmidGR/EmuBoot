using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using EmuBoot;
using EmuBoot.Properties;
using Newtonsoft.Json;

namespace GBASelector.Models
{
    public class Platform
    {
        readonly List<string> _gamePaths;

        public string Name { get; set; }
        public string Extension { get; set; }
        public string EmuPath { get; set; }
        public string GamePaths { get; set; }
        public string CoverPaths { get; set; }
        [JsonIgnore]
        public TabItem PlatformTab { get; }
        public event Action<Platform> PlatformDelete;
        public event Action<Platform> PlatformEdit;

        public Platform(string name, string extension, string emuPath, string gamePaths, string coverPaths)
        {
            // Set name, extension, emulator path and game paths variables
            Name = name;
            Extension = extension;
            EmuPath = emuPath;
            GamePaths = gamePaths;
            CoverPaths = coverPaths;
            _gamePaths = ScanDirectory(GamePaths, Extension);
            PlatformTab = GenPlatformTab();
        }

        private TabItem GenPlatformTab()
        {
            TabItem platformTab = new TabItem
            {
                Header = Name
            };
            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            switch (Settings.Default.IsGridView)
            {
                case true:
                    scrollViewer.Content = GenViewGrid();
                    break;
                case false:
                    scrollViewer.Content = GenViewList();
                    break;
            }
            platformTab.Content = scrollViewer;

            // Add ContextMenu to our tab
            ContextMenu contextMenu = new ContextMenu();

            // Edit item within ContextMenu
            MenuItem EditTab = new MenuItem
            {
                Header = "Edit"
            };
            EditTab.Click += EditTab_Click;
            contextMenu.Items.Add(EditTab);

            // Delete item within  ContextMenu
            MenuItem DeleteTab = new MenuItem
            {
                Header = "Delete"
            };
            DeleteTab.Click += DeleteTab_Click;
            contextMenu.Items.Add(DeleteTab);

            platformTab.ContextMenu = contextMenu;

            return platformTab;
        }


        private void EditTab_Click(object sender, RoutedEventArgs e)
        {
            PlatformEdit?.Invoke(this);
        }

        private void DeleteTab_Click(object sender, RoutedEventArgs e)
        {
            PlatformDelete?.Invoke(this);
        }

        public List<string> ScanDirectory(string checkFolder, string extension)
        {
            List<string> matchingFiles = new List<string>();

            try
            {
                // Get a list of all files in the directory
                string[] files = Directory.GetFiles(checkFolder);

                // Filter the list to include only files with the specified extension
                matchingFiles = files
                    .Where(file => file.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the process
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return matchingFiles;
        }

        private Grid GenViewGrid()
        {
            Grid grid = new Grid();
            for(int i = 0; i < _gamePaths.Count / 4 + 1; i++)
            {
                RowDefinition rowDefinition = new RowDefinition
                {
                    Height = GridLength.Auto
                };
                grid.RowDefinitions.Add(rowDefinition);
            }
            for(int i = 0; i < 4; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition
                {
                    Width = new GridLength(260)
                };
                grid.ColumnDefinitions.Add(columnDefinition);
            }

            int temp = 0;
            for(int row = 0; row < grid.RowDefinitions.Count; row++)
            {
                for(int col = 0; col < grid.ColumnDefinitions.Count; col++)
                {
                    // Exit condition so we don't go out of range with our list index
                    if (temp >= _gamePaths.Count())
                        break;

                    // Create new Game
                    string coverPath = CoverPaths + "\\" + Path.GetFileNameWithoutExtension(_gamePaths[temp]) + ".png";
                    Game game = new Game(_gamePaths[temp], coverPath, EmuPath);

                    grid.Children.Add(game.GameCard);
                    Grid.SetRow(game.GameCard, row);
                    Grid.SetColumn(game.GameCard, col);
                    
                    temp++;
                }
            }
            return grid;
        }

        private Grid GenViewList()
        {
            Grid grid = new Grid();
            for (int i = 0; i < _gamePaths.Count; i++)
            {
                RowDefinition rowDefinition = new RowDefinition
                {
                    Height = new GridLength(60),
                };
                grid.RowDefinitions.Add(rowDefinition);
                // Create new Game
                string coverPath = CoverPaths + "\\" + Path.GetFileNameWithoutExtension(_gamePaths[i]) + ".png";
                Game game = new Game(_gamePaths[i], coverPath, EmuPath);

                grid.Children.Add(game.GameListItem);
                Grid.SetRow(game.GameListItem, i);
            }
            return grid;
        }
    }
}
