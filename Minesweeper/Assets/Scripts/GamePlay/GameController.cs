using System;
using UnityEngine;
using Com.Minesweeper.Data;

namespace Com.Minesweeper.Gameplay
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelController _levelController;
        [SerializeField] private BoardController _boardController;

        private int _width;
        private int _height;
        private int _mineCount;
        private GameState _gameState;
        private GridCellController _gridcellController;

        public GameState GetGameState => _gameState;
        public GridCellController GetGridCellController => _gridcellController;

        private void OnEnable()
        {
            GameEvents.OnGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameEvents.OnGameOver -= OnGameOver;
        }

        /// <summary>
        /// game Initialization 
        /// </summary>
        private void Initialization()
        {
            _width = _levelController.GetLevelData().width;
            _height = _levelController.GetLevelData().height;
            _mineCount = _levelController.GetLevelData().mineCount;
        }

        /// <summary>
        /// Start Game functinality 
        /// added creating grid logic
        /// </summary>
        public void StartGame()
        {
            Initialization();
            _gridcellController = new GridCellController(_width, _height, _mineCount, _boardController);
            GridCell[,] v_gridcell = _gridcellController.InitializeGame();
            _boardController.GenerateBoard(v_gridcell);
            Camera.main.transform.position = new Vector3(_width / 2f, _height / 2f, -10f);
            _gameState = GameState.Playing;
        }

        /// <summary>
        /// Game ver state change functionality
        /// </summary>
        /// <param name="gameOverStat"></param>
        private void OnGameOver(GameOverStat gameOverStat)
        {
            _gameState = GameState.GameOver;
        }
    }

    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver
    }

    public class GameEvents
    {
        public static Action<GameOverStat> OnGameOver;
    }
}