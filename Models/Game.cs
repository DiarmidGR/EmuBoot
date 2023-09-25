using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EmuBoot.Properties;
using Newtonsoft.Json;
using Image = System.Windows.Controls.Image;
using Path = System.IO.Path;

namespace GBASelector.Models
{
    public class Game
    {
        public string Name { get; }
        public string GamePath { get; }
        public string CoverPath { get; }
        public string EmuPath { get; }
        [JsonIgnore]
        public Border GameCard { get; }
        public Border GameListItem { get; }

        public Game(string gamePath, string coverPath, string emuPath)
        {
            GamePath = gamePath;
            Name = Path.GetFileNameWithoutExtension(gamePath);
            CoverPath = coverPath;
            EmuPath = emuPath;

            GameCard = GenGameCardGrid();
            GameListItem = GenGameCardList();
        }

        public Grid GenGameCardItem()
        {
            Grid grid = new Grid();
            Image image = new Image();
            image.Source = new BitmapImage(new Uri("Images\\NoCover.png", UriKind.RelativeOrAbsolute));
            image.Stretch = Stretch.Fill;
            image.ToolTip = new ToolTip
            {
                Content = Name
            };
            image.Height = 56;
            grid.Children.Add(image);
            Label label = new Label
            {
                Content = Name,
                Margin = new Thickness(10),
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            grid.Children.Add(label);
            return grid;
        }

        public Border GenGameCardList()
        {
            Border border = new Border
            {
                Tag = GamePath,
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(2),
                Margin = new Thickness(2),
                Child = GenGameCardItem(),
            };

            // Game Card event handlers
            border.MouseEnter += Border_MouseEnter;
            border.MouseLeave += Border_MouseLeave;
            border.MouseLeftButtonDown += Border_MouseLeftButtonDown;

            return border;
        }

        public Border GenGameCardGrid()
        {
            Border border = new Border
            {
                Tag = GamePath,
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(2),
                Margin = new Thickness(2),
                Child = GenGameImage()
            };

            // Game Card event handlers
            border.MouseEnter += Border_MouseEnter;
            border.MouseLeave += Border_MouseLeave;
            border.MouseLeftButtonDown += Border_MouseLeftButtonDown;

            return border;
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine($"EmuPath: {EmuPath}  GamePath {GamePath}");
            // Combine the emulator path and the command-line arguments
            string arguments = "";
            if (Settings.Default.IsFullScreen)
            {
                arguments = $"-f \"{GamePath}\"";
            }
            else
            {
                arguments = $"\"{GamePath}\"";
            }

            // Start the emulator with the specified arguments
            Process.Start(EmuPath, arguments);
        }

        private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = (Border)sender;
            border.BorderBrush = Brushes.Transparent;
        }

        private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = (Border)sender;
            border.BorderBrush = Brushes.Aquamarine;
        }

        public Image GenGameImage()
        {
            Image image = new Image();
            if (File.Exists(CoverPath))
            {
                image.Source = new BitmapImage(new Uri(CoverPath, UriKind.RelativeOrAbsolute));
                image.Stretch = Stretch.Uniform;
            }
            else
            {
                image.Source = new BitmapImage(new Uri("Images\\NoCover.png", UriKind.RelativeOrAbsolute));

                // Add tooltip to GameCard
                image.ToolTip = new ToolTip
                {
                    Content = Name
                };
            }
            image.Width = 256;
            image.MinHeight = 40;
            return image;
        }
    }
}
