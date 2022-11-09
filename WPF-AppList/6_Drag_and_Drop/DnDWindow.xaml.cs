using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Text.Json;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;

namespace WPF_AppList._6_Drag_and_Drop
{
    /// <summary>
    /// Interaction logic for DnDWindow.xaml
    /// </summary>
    public partial class DnDWindow : Window
    {
        #region Variables

        private PhantomBrickData _phantom;
        private Point _phantomDragPoint;
        private bool _isDragging;

        #endregion
        

        #region Constructor

        public DnDWindow()
        {
            InitializeComponent();
            _phantom.PhantomBrick = null;
            _phantom.SourceBrick = null;
        }

        #endregion


        #region Canvas interaction

        /// <summary>
        /// Left click + move - makes copy (phantom) of brick and moves it.
        /// Right click - remove
        /// </summary>
        private void Brick_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _phantom.SourceBrick = sender as Rectangle;

            // Start dragging brick with left click
            if (e.ChangedButton == MouseButton.Left)
            {
                _isDragging = (_phantom.SourceBrick != null);
                _phantomDragPoint = e.GetPosition(_phantom.SourceBrick);
            }
            // Delete brick with right click
            else if (e.ChangedButton == MouseButton.Right)
            {
                if (_phantom.SourceBrick != BaseBrick1 && _phantom.SourceBrick != BaseBrick2)
                {
                    Field.Children.Remove(_phantom.SourceBrick);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _isDragging = false;
            _phantom.PhantomBrick = null!;
            _phantom.SourceBrick = null!;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging)
                return;
            
            Point point = e.GetPosition(Field);
            Title = point.X + " - " + point.Y;

            // first movement on button hold (no phantom)
            if (_phantom.PhantomBrick == null)
            {
                // Deriving from BaseBrick1 and BaseBrick2
                if (_phantom.SourceBrick == BaseBrick1 || _phantom.SourceBrick == BaseBrick2)
                {
                    _phantom.PhantomBrick = new Rectangle()
                    {
                        Width = _phantom.SourceBrick!.Width,
                        Height = _phantom.SourceBrick.Height,
                        Fill = _phantom.SourceBrick.Fill,
                        Stroke = _phantom.SourceBrick.Stroke,
                        StrokeThickness = _phantom.SourceBrick.StrokeThickness,
                        Opacity = .5
                    };
                    Field.Children.Add(_phantom.PhantomBrick);
                }
                // Moving derived bricks
                else
                {
                    _phantom.PhantomBrick = _phantom.SourceBrick;
                    _phantom.PhantomBrick!.Opacity = .5;
                }
            }

            Canvas.SetLeft(_phantom.PhantomBrick, point.X - _phantomDragPoint.X);
            Canvas.SetTop(_phantom.PhantomBrick, point.Y - _phantomDragPoint.Y);
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_phantom.PhantomBrick != null)
            {
                _phantom.PhantomBrick!.Opacity = 1;
                _phantom.PhantomBrick.MouseDown += Brick_MouseDown;

                _isDragging = false;
                _phantom.PhantomBrick = null;
            }
        }

        #endregion


        #region Menu interaction

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Json files (*.json)|*.json";
            saveDialog.FilterIndex = 2;
            saveDialog.RestoreDirectory = true;
            saveDialog.InitialDirectory = @"C:\";

            if (saveDialog.ShowDialog() == true)
            {
                List<BrickData> bricks = new();
                foreach (var child in Field.Children)
                {
                    if (child is Rectangle brick && (brick != BaseBrick1 && brick != BaseBrick2))
                    {
                        bricks.Add(new BrickData()
                        {
                            Height = brick.Height,
                            Width = brick.Width,
                            Left = Canvas.GetLeft(brick),
                            Top = Canvas.GetTop(brick),
                            Type = (brick.Fill.Equals(Brushes.Salmon) ? 1 : 2)
                        });
                    }
                }

                string json = JsonSerializer.Serialize(bricks);
                File.WriteAllText(saveDialog.FileName, json);
            }
        }

        private void MenuLoad_Click(object sender, RoutedEventArgs e)
        {
            List<BrickData> bricks = new();
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "Json files (*.json)|*.json";

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    bricks = JsonSerializer.Deserialize<List<BrickData>>(File.ReadAllText(openDialog.FileName));
                }
                catch
                {
                    MessageBox.Show("Error!");
                    return;
                }
            }

            // Remove all non-base bricks
            List<UIElement> toRemove = new();
            foreach (var child in Field.Children)
            {
                if (child is Rectangle brick && (brick != BaseBrick1 && brick != BaseBrick2))
                {
                    toRemove.Add(brick);
                }
            }
            foreach (var item in toRemove)
            {
                Field.Children.Remove(item);
            }


            // Add all bricks from file
            foreach (var data in bricks)
            {
                var brick = new Rectangle
                {
                    Width = data.Width,
                    Height = data.Height,
                    Fill = (data.Type == 1) ? Brushes.Salmon : Brushes.Lime,
                    Stroke = (data.Type == 1) ? Brushes.Maroon : Brushes.Orange,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(brick, data.Left);
                Canvas.SetTop(brick, data.Top);
                brick.MouseDown += Brick_MouseDown;

                Field.Children.Add(brick);
            }
        }

        #endregion
    }
}
