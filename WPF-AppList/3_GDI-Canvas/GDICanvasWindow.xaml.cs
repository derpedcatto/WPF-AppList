using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPF_AppList._3_GDI_Canvas
{
    /// <summary>
    /// Interaction logic for GDICanvasWindow.xaml
    /// </summary>
    public partial class GDICanvasWindow : Window
    {
        #region Variables

        private System.Windows.Threading.DispatcherTimer _timer;
        private System.Windows.Threading.DispatcherTimer _clock;
        private TimeSpan _clockValue;

        private bool _leftKeyHold;
        private bool _rightKeyHold;

        private Rectangle? _removedBrick;

        public int BonusBricksCollected { get; private set; }

        public List<Rectangle> BricksList { get; }
        public List<Rectangle> BonusBricksList { get; }

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GDICanvasWindow()
        {
            InitializeComponent();

            _timer = new() { Interval = new TimeSpan(0, 0, 0, 0, 20) };
            _timer.Tick += this.TimerTick;

            _clock = new() { Interval = new TimeSpan(0, 0, 1) };
            _clock.Tick += this.ClockTick;

            Ship.Tag = new ItemData { VelocityX = 5, VelocityY = 0 };
            Ball.Tag = new ItemData { VelocityX = 5, VelocityY = 5 };

            BonusBricksList = new();
            BricksList = new();

            // Adds all bricks to BricksList
            foreach (var item in Field.Children)
            {
                if (item is Rectangle brick && brick != Ship)
                    BricksList.Add(brick);
            }

            Movement.BallCollisionEvent += new BrickEventHandler(BrickHitEvent);
            Movement.BallOufOfBoundsEvent += new EventHandler(BallOutOfBoundsEvent);
            Movement.BonusBrickCollisionEvent += new BrickEventHandler(BonusBrickHitEvent);
            Movement.BonusBrickOutOfBoundsEvent += new BrickEventHandler(BonusBrickOutOfBoundsEvent);
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Removes brick reference from everywhere.
        /// </summary>
        private void RemoveBrick(Rectangle brick)
        {
            BricksList.Remove(brick);
            BonusBricksList.Remove(brick);
            Field.Children.Remove(brick);
        }

        /// <summary>
        /// Displays game over messages and closes the program.
        /// </summary>
        private void GameOver(bool isWin)
        {
            string msg = isWin ? "You won!" : "You lost!";
            MessageBox.Show(msg + $"\nTime: {LabelClock.Content}\nBonuses collected: {BonusBricksCollected}");
            this.Close();
        }

        #endregion


        #region Event Handlers

        /// <summary>
        /// Adds new BonusBrick in place of old brick.
        /// </summary>
        private void BrickHitEvent(BrickEventArgs e)
        {
            Rectangle bonusBrick = new()
            {
                Fill = e.Brick.Fill,
                Width = e.Brick.Width,
                Height = e.Brick.Height,
                Tag = new ItemData { VelocityX = 0, VelocityY = 2 }
            };
            Canvas.SetLeft(bonusBrick, Canvas.GetLeft(e.Brick));
            Canvas.SetTop(bonusBrick, Canvas.GetTop(e.Brick));
            Field.Children.Add(bonusBrick);
            BonusBricksList.Add(bonusBrick);

            _removedBrick = e.Brick;
        }

        /// <summary>
        /// Deletes bonus brick and adds to bonus counter.
        /// </summary>
        private void BonusBrickHitEvent(BrickEventArgs e)
        {
            BonusBricksCollected++;
            LabelCollectedBonusCount.Content = BonusBricksCollected;

            _removedBrick = e.Brick;
        }

        /// <summary>
        /// Deletes out of bounds bonus brick.
        /// </summary>
        private void BonusBrickOutOfBoundsEvent(BrickEventArgs e)
        {
            _removedBrick = e.Brick;
        }

        /// <summary>
        /// Ends game if ball is out of bounds.
        /// </summary>
        private void BallOutOfBoundsEvent(object sender, EventArgs e)
        {
            GameOver(false);
        }

        #endregion


        #region Events

        /// <summary>
        /// Game tick logic.
        /// </summary>
        private void TimerTick(object? sender, EventArgs e)
        {
            Movement.ShipMove(Field, Ship, _leftKeyHold, _rightKeyHold);
            Movement.BallMove(Field, Ball, Ship, BricksList);

            foreach(var brick in BonusBricksList)
                Movement.BonusBrickMove(Field, brick, Ship);

            if (_removedBrick != null)
                RemoveBrick(_removedBrick);

            if (BricksList.Count == 0 && BonusBricksList.Count == 0)
                GameOver(true);
        }

        /// <summary>
        /// Game timer logic. Updates elapsed time from game start.
        /// </summary>
        private void ClockTick(object? sender, EventArgs e)
        {
            _clockValue = _clockValue.Add(new TimeSpan(0, 0, 1));
            LabelClock.Content = _clockValue.ToString();
        }


        /// <summary>
        /// Window loading logic. Sets UI elements and timers.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LabelCollectedBonusCount.Content = "0";
            LabelClock.Content = _clockValue.ToString();

            _timer.Start();
            _clock.Start();
        }

        /// <summary>
        /// Logic on window close. Stops all timers.
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _timer.Stop();
            _clock.Stop();
        }


        /// <summary>
        /// Logic on keyboard key press/hold.
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    _leftKeyHold = true;
                    break;
                case Key.Right:
                    _rightKeyHold = true;
                    break;
            }
        }

        /// <summary>
        /// Logic on keyboard key release.
        /// </summary>
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    _leftKeyHold = false;
                    break;
                case Key.Right:
                    _rightKeyHold = false;
                    break;
            }
        }

        #endregion
    }
}
