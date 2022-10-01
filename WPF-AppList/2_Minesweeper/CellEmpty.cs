using System.Windows.Media;

namespace WPF_AppList._2_Minesweeper
{
    public class CellEmpty : CellBase
    {
        public CellEmpty(char surroundingMinesCount) : base(surroundingMinesCount, Colors.FloralWhite) { }
    }
}