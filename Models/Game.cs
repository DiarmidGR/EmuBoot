using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        [JsonIgnore]
        public Border GameCard { get; }

        public Game(string gamePath, string coverPath)
        {
            GamePath = gamePath;
            Name = Path.GetFileNameWithoutExtension(gamePath);
            CoverPath = coverPath;
            GameCard = GenGameCard();
        }

        public Border GenGameCard()
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
            if (sender is Border border && border.Tag is string gamePath)
            {
                Process.Start(gamePath);
            }
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

        private Image GenGameImage()
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
