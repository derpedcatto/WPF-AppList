using System.Windows;
using System.Windows.Shapes;

namespace WPF_AppList._6_Drag_and_Drop
{
    struct PhantomBrickData
    {
        /// <summary>
        /// Copied object.
        /// </summary>
        public Rectangle? SourceBrick { get; set; }

        /// <summary>
        /// Temporary copy (phantom) of object.
        /// </summary>
        public Rectangle? PhantomBrick { get; set; }
    }
}
