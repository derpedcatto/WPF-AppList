using System;
using System.Drawing;
using System.Runtime;
using System.Windows.Controls.Primitives;

namespace WPF_AppList._2_Minesweeper
{
    public class CellController
    {
        #region Variables

        private MinesweeperWindow _window;
        private CellBase[,] _cellGrid;
        private Point _gridSize;
        private int _emptyCount;
        private int _mineCount;

        #endregion


        #region Constructor

        public CellController(MinesweeperWindow window)
        {
            _window = window;
            _gridSize = new Point(_window.CellWindowGrid.Rows, _window.CellWindowGrid.Columns);
            _mineCount = _gridSize.Y * _gridSize.X / 8;
            _emptyCount = 0;

            _window.LabelFlagCount.Content = _mineCount;

            _cellGrid = new CellBase[_gridSize.Y, _gridSize.X];
            SetCells();
        }

        #endregion


        #region Events

        private void RevealEvent(object sender, EventArgs e)
        {
            if (sender is CellEmpty cell)
            {
                _emptyCount--;

                if (cell.NoMinesAround)
                    CascadeRevealEmpty(GetCell(cell));

                if (_emptyCount == 0)
                    _window.GameOver(true);
            }

            if (sender is CellMine)
                _window.GameOver(false);
        }

        private void FlagEvent(object sender, EventArgs e)
        {
            var cell = sender as CellBase;
            _mineCount += cell.IsChecked ? -1 : 1;
            _window.LabelFlagCount.Content = _mineCount;
        }

        #endregion


        #region Methods - Public

        public void Dispose()
        {
            foreach (var cell in _cellGrid)
                _window.CellWindowGrid.Children.Remove(cell.Label);
        }

        #endregion


        #region Methods - Private

        /// <summary>
        /// Returns coordinates of given cell from cell grid.
        /// </summary>
        private Point GetCell(CellBase cell)
        {
            for (int i = 0; i < _gridSize.Y; i++)
            {
                for (int j = 0; j < _gridSize.X; j++)
                {
                    if (_cellGrid[i, j] == cell)
                        return new Point(j, i);
                }
            }
            return new Point(0, 0);
        }

        /// <summary>
        /// Returns a symbol of neighbouring mines count for empty cell.
        /// </summary>
        private char GetNeighbouringMineCount(int x, int y)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    try
                    {
                        if (_cellGrid[y + i, x + j] is CellMine)
                            count++;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            if (count == 0) return ' ';
            return (char)(count + 48);
        }

        /// <summary>
        /// Sets all cells in array and on grid.
        /// </summary>
        private void SetCells()
        {
            var rand = new Random();
            int minesToPlace = _mineCount;

            // Place mine cells
            while (minesToPlace != 0)
            {
                int x;
                int y;
                do
                {
                    x = rand.Next(_gridSize.X);
                    y = rand.Next(_gridSize.Y);

                } while (_cellGrid[y, x] != null);

                _cellGrid[y, x] = new CellMine();
                minesToPlace--;
            }

            // Place empty cells
            for (int i = 0; i < _gridSize.Y; i++)
            {
                for (int j = 0; j < _gridSize.X; j++)
                {
                    if (_cellGrid[i, j] is CellMine)
                        continue;

                    _cellGrid[i, j] = new CellEmpty(GetNeighbouringMineCount(j, i));
                    _emptyCount++;
                }
            }

            // Adding elements to _cellWindowGrid
            // and setting events to all cells
            foreach (var cell in _cellGrid)
            {
                _window.CellWindowGrid.Children.Add(cell.Label);
                cell.RevealEvent += new RevealEventHandler(RevealEvent);
                cell.FlagEvent += new FlagEventHandler(FlagEvent);
            }
        }

        /// <summary>
        /// Reveals all neighbouring empty cells until reaching empty cell with neighbouring mines.
        /// </summary>
        private void CascadeRevealEmpty(Point index)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    try
                    {
                        if (_cellGrid[index.Y + i, index.X + j] is CellEmpty _cell)
                            if (!_cell.IsChecked && !_cell.IsRevealed)
                            {
                                _cell.Reveal();
                                _emptyCount--;

                                if (_cell.NoMinesAround)
                                    CascadeRevealEmpty(new Point(index.X + j, index.Y + i));
                            }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        #endregion
    }
}
