using UnityEngine;
using UnityEngine.Tilemaps;
using Com.Minesweeper.Data;

namespace Com.Minesweeper.Gameplay
{
    public class BoardController : MonoBehaviour, IBoard
    {
        public Tilemap tilemap;
        [SerializeField] public TileAsset tileAsset;

        /// <summary>
        /// Board and Grid generator
        /// </summary>
        /// <param name="gridCells"> generated cells</param>
        public void GenerateBoard(GridCell[,] gridCells)
        {
            int v_width = gridCells.GetLength(0);
            int v_height = gridCells.GetLength(1);

            for (int x = 0; x < v_width; x++)
            {
                for (int y = 0; y < v_height; y++)
                {
                    GridCell v_cell = gridCells[x, y];
                    tilemap.SetTile(v_cell.position, tileAsset.closed);
                }
            }
        }

        /// <summary>
        /// Update Tile state and its Tile
        /// </summary>
        /// <param name="gridCells"></param>
        public void Draw(GridCell[,] gridCells)
        {
            int v_width = gridCells.GetLength(0);
            int v_height = gridCells.GetLength(1);

            for (int x = 0; x < v_width; x++)
            {
                for (int y = 0; y < v_height; y++)
                {
                    GridCell v_cell = gridCells[x, y];
                    tilemap.SetTile(v_cell.position, GetTile(v_cell));
                }
            }
        }

        private Tile GetTile(GridCell gridCell)
        {
            if (gridCell.revealed)
            {
                return GetRevealedTile(gridCell);
            }
            else if (gridCell.flagged)
            {
                return tileAsset.flag;
            }
            else
            {
                return tileAsset.closed;
            }
        }

        private Tile GetRevealedTile(GridCell gridCell)
        {
            switch (gridCell.type)
            {
                case CellType.Empty: return tileAsset.empty;
                case CellType.Mine: return gridCell.exploded ? tileAsset.exploded : tileAsset.mine;
                case CellType.Number: return tileAsset.GetNumberTile(gridCell.number);
                default: return null;
            }
        }
    }
}