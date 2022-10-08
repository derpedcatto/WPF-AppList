using System.Windows;
using WPF_AppList._2_Minesweeper;
using WPF_AppList._3_GDI_Canvas;
using WPF_AppList._4_Resources_Styles;
using WPF_AppList._5_4_Homework;
using WPF_AppList._5_Resources_Triggers;
using WPF_AppList._6_Drag_and_Drop;

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
            new GDICanvasWindow().ShowDialog();
        }

        private void ButtonStyles_Click(object sender, RoutedEventArgs e)
        {
            new StylesWindow().ShowDialog();
        }

        private void ButtonTriggers_Click(object sender, RoutedEventArgs e)
        {
            new TriggersWindow().ShowDialog();
        }

        private void ButtonStylesTriggersHomework_Click(object sender, RoutedEventArgs e)
        {
            new FormRegistrationWindow().ShowDialog();
        }

        private void ButtonDnD_Click(object sender, RoutedEventArgs e)
        {
            new DnDWindow().ShowDialog();
        }
    }
}
