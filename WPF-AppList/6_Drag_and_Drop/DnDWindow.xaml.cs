using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Text.Json;
using System.Collections.Generic;
using System.Windows.Media;

namespace WPF_AppList._6_Drag_and_Drop
{
    class BrickData
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public int Type { get; set; }
    }


    /// <summary>
    /// Interaction logic for DnDWindow.xaml
    /// </summary>
    public partial class DnDWindow : Window
    {
        #region Variables

        private bool _isDragging;
        private Rectangle? _phantom;
        private Rectangle? _phantomSource;
        private Point _phantomDragPoint;
        private string _json;

        #endregion
        

        #region Constructor

        public DnDWindow()
        {
            InitializeComponent();
            _phantom = null;
            _phantomSource = null;
        }

        #endregion


        #region Canvas interaction

        /// <summary>
        /// Left click mouse move - makes copy of (phantom) of brick and moves it.
        /// Right click - remove
        /// </summary>
        private void Brick_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _phantomSource = sender as Rectangle;
            if (e.ChangedButton == MouseButton.Left)
            {
                _isDragging = (_phantomSource != null);
                _phantomDragPoint = e.GetPosition(_phantomSource);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                if (_phantomSource != Brick && _phantomSource != Brick2)
                {
                    Field.Children.Remove(_phantomSource);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _isDragging = false;
            _phantom = null!;
            _phantomSource = null!;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point point = e.GetPosition(Field);
                Title = point.X + " - " + point.Y;

                // first movement on button hold (no phantom)
                if (_phantom == null)
                {
                    // Deriving from Brick and Brick2
                    if (_phantomSource == Brick || _phantomSource == Brick2)
                    {
                        _phantom = new Rectangle()
                        {
                            Width = _phantomSource!.Width,
                            Height = _phantomSource.Height,
                            Fill = _phantomSource.Fill,
                            Stroke = _phantomSource.Stroke,
                            StrokeThickness = _phantomSource.StrokeThickness,
                            Opacity = .5
                        };
                        Field.Children.Add(_phantom);
                    }
                    // Moving derived bricks
                    else
                    {
                        _phantom = _phantomSource;
                        _phantom!.Opacity = .5;
                    }
                }
                Canvas.SetLeft(_phantom, point.X - _phantomDragPoint.X);
                Canvas.SetTop(_phantom, point.Y - _phantomDragPoint.Y);
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_phantom != null)
            {
                _phantom!.Opacity = 1;
                _phantom.MouseDown += Brick_MouseDown;

                _isDragging = false;
                _phantom = null;
            }
        }

        #endregion


        #region Menu interaction

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            List<BrickData> bricks = new();
            foreach(var child in Field.Children)
            {
                if (child is Rectangle brick && (brick != Brick && brick != Brick2))
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
            _json = JsonSerializer.Serialize(bricks);
        }

        private void MenuLoad_Click(object sender, RoutedEventArgs e)
        {
            BrickData[] bricks;
            try
            {
                bricks = JsonSerializer.Deserialize<BrickData[]>(_json);
            }
            catch
            {
                MessageBox.Show("Error!");
                return;
            }

            List<UIElement> toRemove = new();
            foreach (var child in Field.Children)
            {
                if (child is Rectangle brick && (brick != Brick && brick != Brick2))
                {
                    toRemove.Add(brick);
                }
            }
            foreach (var item in toRemove)
                Field.Children.Remove(item);

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
