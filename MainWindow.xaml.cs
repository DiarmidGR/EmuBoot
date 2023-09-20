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
using Wpf.Ui.Controls;

namespace GBASelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Platform nds = new Platform("NDS", ".nds", Properties.Settings.Default.NDSPath);
        Platform gba = new Platform("GBA", ".gba", Properties.Settings.Default.GBAPath);
        Platform snes = new Platform("SNES", ".smc", Properties.Settings.Default.SNESPath);
        List<Platform> listPlatforms = new List<Platform>();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listPlatforms.Add(snes);
            listPlatforms.Add(gba);
            listPlatforms.Add(nds);
            //PopulateGrid(Properties.Settings.Default.GBAPath, GBAFiles, gridButtons);
            //tabDS.PreviewMouseDown += TabDS_PreviewMouseDown;
            //tabGBA.PreviewMouseDown += TabGBA_PreviewMouseDown;
            foreach (Platform platform in listPlatforms)
            {
                platform.CreateGrid(tC);
            }
            tC.SelectedIndex = 0;
        }

        // Event Handlers
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
