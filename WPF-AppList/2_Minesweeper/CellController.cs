namespace WPF_AppList._2_Minesweeper
{
    public class CellController
    {
        private CellBase[,] _cellGrid;
        private int _cellGridWidth;
        private int _cellGridHeight;
        private int _mineCount;

        public CellController()
        {
            _cellGridWidth = 15;
            _cellGridHeight = 15;
            _mineCount = 15;

            _cellGrid = new CellBase[_cellGridHeight, _cellGridWidth];
        }
    }
}
