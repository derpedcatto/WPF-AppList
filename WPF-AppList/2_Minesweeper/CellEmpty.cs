using System.Windows.Media;

namespace WPF_AppList._2_Minesweeper
{
    public class CellEmpty : CellBase
    {
        public bool NoMinesAround { get; }


        public CellEmpty(char surroundingMinesCount) : base(surroundingMinesCount, Colors.FloralWhite) 
        {
            NoMinesAround = (surroundingMinesCount == ' ');
        }
    }
}