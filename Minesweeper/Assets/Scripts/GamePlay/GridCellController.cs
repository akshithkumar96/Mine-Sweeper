using UnityEngine;
using Com.Minesweeper.Data;

namespace Com.Minesweeper.Gameplay
{
    public class GridCellController
    {
        private GridCell[,] _gridCells;

        private int _width;
        private int _height;
        private int _mineCount;
        private BoardController _boardController;

        public GridCell[,] GetGridCell => _gridCells;

        public GridCellController(int width, int height, int mineCount, BoardController boardController)
        {
            this._width = width;
            this._height = height;
            this._mineCount = mineCount;
            _boardController = boardController;
            InputController.MouseButtonDown += OnMouseButtonDown;
        }

        #region Game Initialization

        public GridCell[,] InitializeGame()
        {
            _gridCells = new GridCell[_width, _height];
            GenerateCells();
            GenerateMines();
            GenerateNumbers();

            return _gridCells;
        }

        /// <summary>
        /// Generate the cell for give height and width
        /// </summary>
        private void GenerateCells()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    GridCell v_cell = new GridCell()
                    {
                        position = new Vector3Int(x, y, 0),
                        type = CellType.Empty
                    };
                    _gridCells[x, y] = v_cell;
                }
            }
        }

        /// <summary>
        /// randomly generates mine 
        /// </summary>
        private void GenerateMines()
        {
            for (int i = 0; i < _mineCount; i++)
            {
                int x = Random.Range(0, _width);
                int y = Random.Range(0, _height);

                while (_gridCells[x, y].type == CellType.Mine)
                {
                    x++;

                    if (x >= _width)
                    {
                        x = 0;
                        y++;

                        if (y >= _height)
                        {
                            y = 0;
                        }
                    }
                }

                _gridCells[x, y].type = CellType.Mine;
            }
        }

        /// <summary>
        /// generated appropriate numbers for the given cells
        /// </summary>
        private void GenerateNumbers()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    GridCell v_cell = _gridCells[x, y];

                    if (v_cell.type == CellType.Mine)
                    {
                        continue;
                    }

                    v_cell.number = CountMines(x, y);

                    if (v_cell.number > 0)
                    {
                        v_cell.type = CellType.Number;
                    }

                    _gridCells[x, y] = v_cell;
                }
            }
        }

        /// <summary>
        /// To count mines for a given cell
        /// </summary>
        /// <param name="cellX"></param>
        /// <param name="cellY"></param>
        /// <returns></returns>
        private int CountMines(int cellX, int cellY)
        {
            int v_count = 0;

            for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
            {
                for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
                {
                    if (adjacentX == 0 && adjacentY == 0)
                    {
                        continue;
                    }

                    int x = cellX + adjacentX;
                    int y = cellY + adjacentY;

                    if (GetCell(x, y).type == CellType.Mine)
                    {
                        v_count++;
                    }
                }
            }

            return v_count;
        }

        /// <summary>
        /// returns the appropriate cell for the given position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private GridCell GetCell(int x, int y)
        {
            if (IsValidCell(x, y))
            {
                return _gridCells[x, y];
            }
            else
            {
                return new GridCell();
            }
        }

        /// <summary>
        /// Checks for valid cell
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsValidCell(int x, int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }
        #endregion

        #region Game Logic
        private void OnMouseButtonDown(int index)
        {
            if (index == 1)
            {
                FlagCell();
            }
            else if (index == 0)
            {
                RevealCell();
            }
        }

        /// <summary>
        /// Flag the seleted tile
        /// </summary>
        private void FlagCell()
        {
            Vector3 v_worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int v_cellPosition = _boardController.tilemap.WorldToCell(v_worldPosition);
            GridCell v_cell = GetCell(v_cellPosition.x, v_cellPosition.y);

            // Cannot flag if already revealed
            if (v_cell.type == CellType.Invalid || v_cell.revealed)
            {
                return;
            }

            v_cell.flagged = !v_cell.flagged;
            _gridCells[v_cellPosition.x, v_cellPosition.y] = v_cell;
            _boardController.Draw(_gridCells);
        }

        /// <summary>
        /// On Player click on grid reveal the grid cell check for mine 
        /// </summary>
        private void RevealCell()
        {
            Vector3 v_worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int v_cellPosition = _boardController.tilemap.WorldToCell(v_worldPosition);
            GridCell v_cell = GetCell(v_cellPosition.x, v_cellPosition.y);

            // Cannot reveal if already revealed or while flagged
            if (v_cell.type == CellType.Invalid || v_cell.revealed || v_cell.flagged)
            {
                return;
            }

            switch (v_cell.type)
            {
                case CellType.Mine:
                    ExplodeCell(v_cell);
                    break;

                case CellType.Empty:
                    PopulateEmptyCell(v_cell);
                    CheckWinCondition();
                    break;

                default:
                    v_cell.revealed = true;
                    _gridCells[v_cellPosition.x, v_cellPosition.y] = v_cell;
                    CheckWinCondition();
                    break;
            }

            _boardController.Draw(_gridCells);
        }

        /// <summary>
        /// For Auto Solver Reveal Functionality
        /// </summary>
        /// <param name="cell"></param>
        public void RevealCell(GridCell cell)
        {
            if (cell.type == CellType.Invalid || cell.revealed || cell.flagged)
            {
                return;
            }

            switch (cell.type)
            {
                case CellType.Mine:
                    ExplodeCell(cell);
                    break;

                case CellType.Empty:
                    PopulateEmptyCell(cell);
                    CheckWinCondition();
                    break;

                default:
                    cell.revealed = true;
                    _gridCells[cell.position.x, cell.position.y] = cell;
                    CheckWinCondition();
                    break;
            }

            _boardController.Draw(_gridCells);
        }

        /// <summary>
        /// For populating the click and check for 
        /// </summary>
        /// <param name="cell"></param>
        private void PopulateEmptyCell(GridCell cell)
        {
            // Recursive exit conditions
            if (cell.revealed) return;
            if (cell.type == CellType.Mine || cell.type == CellType.Invalid) return;

            // Reveal the cell
            cell.revealed = true;
            _gridCells[cell.position.x, cell.position.y] = cell;

            // Keep flooding if the cell is empty, otherwise stop at numbers
            if (cell.type == CellType.Empty)
            {
                PopulateEmptyCell(GetCell(cell.position.x - 1, cell.position.y));
                PopulateEmptyCell(GetCell(cell.position.x + 1, cell.position.y));
                PopulateEmptyCell(GetCell(cell.position.x, cell.position.y - 1));
                PopulateEmptyCell(GetCell(cell.position.x, cell.position.y + 1));
            }
        }

        /// <summary>
        /// On clicking on Mine Explode and show game Over screen
        /// </summary>
        /// <param name="cell"></param>
        private void ExplodeCell(GridCell cell)
        {
            GameEvents.OnGameOver?.Invoke(new GameOverStat() { isWin = false });

            // Set the mine as exploded
            cell.exploded = true;
            cell.revealed = true;
            _gridCells[cell.position.x, cell.position.y] = cell;

            // Reveal all other mines
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    cell = _gridCells[x, y];

                    if (cell.type == CellType.Mine)
                    {
                        cell.revealed = true;
                        _gridCells[x, y] = cell;
                    }
                }
            }
        }

        /// <summary>
        /// Check for all empty or number cell are being revealed or not to check for win condition
        /// </summary>
        private void CheckWinCondition()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    GridCell cell = _gridCells[x, y];

                    // All non-mine cells must be revealed to have won
                    if (cell.type != CellType.Mine && !cell.revealed)
                    {
                        return; // no win
                    }
                }
            }
            GameEvents.OnGameOver?.Invoke(new GameOverStat() { isWin = true });

            // Flag all the mines
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    GridCell v_cell = _gridCells[x, y];

                    if (v_cell.type == CellType.Mine)
                    {
                        v_cell.flagged = true;
                        _gridCells[x, y] = v_cell;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// unsubribing the Events
        /// </summary>
        ~GridCellController()
        {
            InputController.MouseButtonDown -= OnMouseButtonDown;
        }
    }
}
