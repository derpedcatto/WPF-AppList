using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_AppList._2_Minesweeper
{
    /// <summary>
    /// Interaction logic for MinesweeperWindow.xaml
    /// </summary>
    public partial class MinesweeperWindow : Window
    {
        private CellController _cellController;

        public MinesweeperWindow()
        {
            InitializeComponent();
            _cellController = new();
        }
    }
}
