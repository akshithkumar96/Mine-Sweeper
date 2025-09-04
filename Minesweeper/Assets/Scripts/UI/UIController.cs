using UnityEngine;
using UnityEngine.UI;
using Com.Minesweeper.Gameplay;

namespace Com.Minesweeper.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button restartBtn;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gamePlayPanel;
        [SerializeField] private GameObject gameOverpanel;
        [SerializeField] private Text gameOverText;

        [SerializeField] private GameController _gameController;

        private void OnEnable()
        {
            startBtn.onClick.AddListener(OnStartGameClick);
            restartBtn.onClick.AddListener(OnRestrartBtnClick);
            GameEvents.OnGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            startBtn.onClick.RemoveListener(OnStartGameClick);
            restartBtn.onClick.RemoveListener(OnRestrartBtnClick);
            GameEvents.OnGameOver -= OnGameOver;

        }

        /// <summary>
        /// Start Button click functionality
        /// </summary>
        private void OnStartGameClick()
        {
            gamePlayPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
            _gameController.StartGame();
        }

        /// <summary>
        /// Restart Button click functionality
        /// </summary>
        private void OnRestrartBtnClick()
        {
            gamePlayPanel.SetActive(true);
            gameOverpanel.SetActive(false);
            _gameController.StartGame();
        }

        /// <summary>
        /// On Game Over UI functionality
        /// </summary>
        /// <param name="gamedata"></param>
        private void OnGameOver(GameOverStat gamedata)
        {
            gamePlayPanel.SetActive(false);
            gameOverText.text = gamedata.isWin ? "Game Win" : "Game Lost";
            gameOverpanel.SetActive(true);
        }
    }
}

public struct GameOverStat
{
    public bool isWin;
    public int score;
}
