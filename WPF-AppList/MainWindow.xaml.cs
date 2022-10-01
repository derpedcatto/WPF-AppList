using System.Windows;
using WPF_AppList._2_Minesweeper;

namespace WPF_AppList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void ButtonContainers_Click(object sender, RoutedEventArgs e)
        {
            new WindowContainers().ShowDialog();
        }

        private void ButtonContainersHomework_Click(object sender, RoutedEventArgs e)
        {
            new WindowContainersHomework().ShowDialog();
        }

        private void ButtonMinesweeper_Click(object sender, RoutedEventArgs e)
        {
            new MinesweeperWindow().ShowDialog();
        }

        private void ButtonGDI_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not done yet!");
        }
    }
}
