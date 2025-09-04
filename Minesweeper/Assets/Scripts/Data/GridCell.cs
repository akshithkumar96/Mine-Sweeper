using UnityEngine;

namespace Com.Minesweeper.Data
{
    public struct GridCell
    {
        public Vector3Int position;
        public CellType type;
        public int number;
        public bool revealed;
        public bool flagged;
        public bool exploded;
    }

    public enum CellType
    {
        Invalid,
        Empty,
        Mine,
        Number,
    }

    public enum CellState
    {
        Closed,
        Revealed,
        Flagged,
    }
}