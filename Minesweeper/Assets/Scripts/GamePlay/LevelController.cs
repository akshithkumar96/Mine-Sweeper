using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Minesweeper.Data;

namespace Com.Minesweeper.Gameplay
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Leveltype currentLevelType;
        [SerializeField] private List<LevelData> leveldata;

        public GameLevel GetLevelData()
        {
            return leveldata.Find((LevelData levelData) => levelData.leveltype == currentLevelType).gameLevel;
        }
    }

    [System.Serializable]
    public struct LevelData
    {
        public Leveltype leveltype;
        public GameLevel gameLevel;
    }

    public enum Leveltype
    {
        Easy,
        Medium,
        Hard
    }
}