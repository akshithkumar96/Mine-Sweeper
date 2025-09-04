using Com.Minesweeper.Data;

namespace Com.Minesweeper.Gameplay
{
    public interface IBoard
    {
        void GenerateBoard(GridCell[,] gridCells);
        void Draw(GridCell[,] gridcells);
    }
}
