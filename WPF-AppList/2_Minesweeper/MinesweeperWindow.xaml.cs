using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private System.Windows.Threading.DispatcherTimer _timer;
        private TimeSpan _timerValue;
        private CellController _cellController;

        public MinesweeperWindow()
        {
            InitializeComponent();
            _cellController = new(this);

            _timerValue = new TimeSpan(0, 0, 0);
            LabelTimer.Content = _timerValue;

            _timer = new();
            _timer.Tick += UpdateLabelTimer;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private void UpdateLabelTimer(object sender, EventArgs e)
        {
            _timerValue = _timerValue.Add(new TimeSpan(00, 00, 01));
            LabelTimer.Content = _timerValue.ToString();
        }

        public void GameOver(bool isWin)
        {
            _timer.Stop();

            string endText = isWin ? "You won!" : "You lost!";
            var result = MessageBox.Show(endText + " Try again?", "", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
                this.Close();

            // restarting game
            _cellController.Dispose();
            _cellController = new(this);
            _timerValue = new TimeSpan(0, 0, 0);
            _timer.Start();
            LabelTimer.Content = _timerValue.ToString();
        }
    }
}
