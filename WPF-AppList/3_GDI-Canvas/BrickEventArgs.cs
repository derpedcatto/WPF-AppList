using System;
using System.Windows.Shapes;

namespace WPF_AppList._3_GDI_Canvas
{
    public class BrickEventArgs : EventArgs
    {
        public Rectangle Brick { get; }

        public BrickEventArgs(Rectangle brick)
        {
            this.Brick = brick;
        }
    }
}
