using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Minesweeper.Data
{
    [CreateAssetMenu(fileName = "GameLevel", menuName = "Assets/ScriptableObject/LevelData")]
    public class GameLevel : ScriptableObject
    {
        public int height;
        public int width;
        public int mineCount;

        private void OnValidate()
        {
            //Can change based on requirement
            mineCount = Mathf.Clamp(mineCount, 0, height * width / 2);
        }
    }
}