using UnityEngine;
using UnityEngine.Tilemaps;

namespace Com.Minesweeper.Data
{
    [CreateAssetMenu(fileName = "TileAssets", menuName = "Assets/ScriptableObject/TileData")]
    public class TileAsset : ScriptableObject
    {
        public Tile closed;
        public Tile empty;
        public Tile mine;
        public Tile exploded;
        public Tile flag;
        public Tile num1;
        public Tile num2;
        public Tile num3;
        public Tile num4;
        public Tile num5;
        public Tile num6;
        public Tile num7;
        public Tile num8;

        public Tile GetNumberTile(int number)
        {
            switch (number)
            {
                case 1: return num1;
                case 2: return num2;
                case 3: return num3;
                case 4: return num4;
                case 5: return num5;
                case 6: return num6;
                case 7: return num7;
                case 8: return num8;
                default: return null;
            }
        }
    }
}