using Com.Minesweeper.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.UI.CanvasScaler;

namespace Com.Minesweeper.Gameplay
{
    public class AutoSolver : MonoBehaviour
    {
        [SerializeField] private Tile solveTile;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private GameController _gameController;

        private GridCell[,] _gridCells;
        public GridCellController _cellController;
        private int _width;
        private int _height;
        private int _lastXIndex;
        private int _lastYIndex;
        private Coroutine _solver;
        private bool _isGameOver;

        public void Initialization()
        {
            _cellController = _gameController.GetGridCellController;
            _gridCells = _cellController.GetGridCell;
            _width = _gridCells.GetLength(0);
            _height = _gridCells.GetLength(1);
            _isGameOver = false;
            _lastXIndex = 0;
            _lastYIndex = 0;
        }

        private void OnEnable()
        {
            InputController.OnKeyBoardkeyDown += OnKeyBoardInputClick;
        }

        private void OnDisable()
        {
            InputController.OnKeyBoardkeyDown -= OnKeyBoardInputClick;
        }

        private void OnKeyBoardInputClick(KeyCode keyCode)
        {
            if (keyCode == KeyCode.S && _gameController.GetGameState == GameState.Playing)
            {
                Initialization();
                SolveGrid();
            }
        }

        /// <summary>
        /// For solving the grid 
        /// </summary>
        public void SolveGrid()
        {
            _solver = StartCoroutine(SolveNextGrid());
        }

        /// <summary>
        /// get the next grid and Execute click event
        /// </summary>
        /// <returns></returns>
        private IEnumerator SolveNextGrid()
        {
            GridCell v_gridcell = GetNextunRevealedCell();
            tilemap.SetTile(v_gridcell.position, solveTile);

            if (!_isGameOver)
            {
                yield return new WaitForSeconds(1.0f);
                _cellController.RevealCell(v_gridcell);
                _solver = StartCoroutine(SolveNextGrid());
            }
        }

        /// <summary>
        /// Get the next unrevealed cell
        /// </summary>
        /// <returns></returns>
        private GridCell GetNextunRevealedCell()
        {
            for (int i = _lastXIndex; i < _width; i++)
            {
                for (int j = _lastYIndex; j < _height; j++)
                {
                    var cell = _gridCells[i, j];
                    if (!cell.revealed && cell.type != CellType.Mine)
                    {
                        _lastXIndex = i;
                        if(_lastYIndex>=_height)
                        {
                            _lastYIndex = 0;
                        }
                        else
                        {
                            _lastYIndex = j;
                        }
                        return cell;
                    }
                }
                _lastYIndex = 0;
            }
            _isGameOver = true;
            GameEvents.OnGameOver?.Invoke(new GameOverStat() { isWin = true });
            return new GridCell();
        }
    }
}