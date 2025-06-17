// Core game board logic and win condition checking
public class Board
{
    private int[,] board;
    public int Rows { get; private set; }
    public int Columns { get; private set; }

    public Board(int rows, int cols)
    {
        Rows = rows;
        Columns = cols;
        board = new int[rows, cols];
    }

    public bool CanPlaceInColumn(int col) => board[0, col] == 0;

    public bool PlacePiece(int col, int playerId, out int row)
    {
        for (int r = Rows - 1; r >= 0; r--)
        {
            if (board[r, col] == 0)
            {
                board[r, col] = playerId;
                row = r;
                return true;
            }
        }
        row = -1;
        return false;
    }

    public bool CheckWin(int row, int col, int playerId)
    {
        return CountDirection(row, col, 0, 1, playerId) + CountDirection(row, col, 0, -1, playerId) > 2 ||
               CountDirection(row, col, 1, 0, playerId) + CountDirection(row, col, -1, 0, playerId) > 2 ||
               CountDirection(row, col, 1, 1, playerId) + CountDirection(row, col, -1, -1, playerId) > 2 ||
               CountDirection(row, col, 1, -1, playerId) + CountDirection(row, col, -1, 1, playerId) > 2;
    }

    private int CountDirection(int row, int col, int dRow, int dCol, int playerId)
    {
        int count = 0;
        int r = row + dRow;
        int c = col + dCol;

        while (r >= 0 && r < Rows && c >= 0 && c < Columns && board[r, c] == playerId)
        {
            count++;
            r += dRow;
            c += dCol;
        }

        return count;
    }

    public bool IsFull()
    {
        for (int c = 0; c < Columns; c++)
            if (board[0, c] == 0)
                return false;

        return true;
    }

    public int[,] GetBoardCopy() => (int[,])board.Clone();
}
