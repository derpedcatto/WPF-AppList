using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF_AppList._2_Minesweeper
{
    public delegate void RevealEventHandler(object sender, EventArgs e);
    public delegate void FlagEventHandler(object sender, EventArgs e);

    public abstract class CellBase
    {
        public event RevealEventHandler RevealEvent;
        public event FlagEventHandler FlagEvent;

        public Label Label { get; private set; }
        public bool IsRevealed { get; private set; }
        public bool IsChecked { get; private set; }

        private const char _symbolFlag = '\x2691';
        private const char _symbolUnrevealed = '⠀';
        private char _symbolRevealed;
        private Color _colorUnrevealed = Colors.NavajoWhite;
        private Color _colorRevealed;


        public CellBase(char symbolRevealed, Color colorRevealed)
        {
            _symbolRevealed = symbolRevealed;
            _colorRevealed = colorRevealed;

            Label = new()
            {
                Content = _symbolUnrevealed,
                Margin = new Thickness(0.5),
                FontSize = 14,
                Background = new SolidColorBrush(_colorUnrevealed),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Label.MouseLeftButtonDown += MouseLeftClickEvent;
            Label.MouseRightButtonDown += MouseRightClickEvent;

            IsRevealed = false;
            IsChecked = false;
        }


        public virtual void Reveal()
        {
            IsRevealed = true;
            Label.Content = _symbolRevealed;
            Label.Background = new SolidColorBrush(_colorRevealed);
            Label.MouseLeftButtonDown -= MouseLeftClickEvent;
            Label.MouseRightButtonDown -= MouseRightClickEvent;
        }

        private void MouseLeftClickEvent(object sender, MouseEventArgs e)
        {
            if (!IsRevealed && !IsChecked)
            {
                Reveal();
                RevealEvent?.Invoke(this, null);
            }
        }

        private void MouseRightClickEvent(object sender, MouseEventArgs e)
        {
            if (IsRevealed)
                return;

            IsChecked = !IsChecked;
            Label.Content = IsChecked ? _symbolFlag : _symbolUnrevealed;

            FlagEvent?.Invoke(this, null);
        }
    }
}
