using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF_AppList._2_Minesweeper
{
    public delegate void RevealEventHandler(object sender, EventArgs e);

    public abstract class CellBase
    {
        public event RevealEventHandler RevealEvent;

        public Label Label { get; private set; }
        public bool IsRevealed { get; private set; }
        public bool IsChecked { get; private set; }

        private const char _symbolFlag = '\x2691';
        private const char _symbolCell = ' ';
        private char _symbolRevealed;
        private Color _colorCell = Colors.NavajoWhite;
        private Color _colorRevealed;


        public CellBase(char symbolRevealed, Color colorRevealed)
        {
            _symbolRevealed = symbolRevealed;
            _colorRevealed = colorRevealed;

            Label = new()
            {
                Content = _symbolCell,
                Margin = new Thickness(1),
                Background = new SolidColorBrush(_colorCell)
            };
            Label.MouseLeftButtonDown += MouseLeftClickEvent;
            Label.MouseRightButtonDown += MouseRightClickEvent;

            IsRevealed = false;
            IsChecked = false;
        }


        protected virtual void Reveal()
        {
            Label.Content = _symbolRevealed;
            Label.Background = new SolidColorBrush(_colorRevealed);
            Label.MouseLeftButtonDown -= MouseLeftClickEvent;
            Label.MouseRightButtonDown -= MouseRightClickEvent;
        }

        public void MouseLeftClickEvent(object sender, MouseEventArgs e)
        {
            if (!IsRevealed && !IsChecked)
            {
                IsRevealed = true;
                Reveal();
                RevealEvent?.Invoke(this, null);
            }
        }

        public void MouseRightClickEvent(object sender, MouseEventArgs e)
        {
            if (IsRevealed)
                return;

            IsChecked = !IsChecked;

            if (IsChecked)
                Label.Content = _symbolFlag;
            else
                Label.Content = _symbolCell;
        }
    }
}
