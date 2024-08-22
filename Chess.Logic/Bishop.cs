namespace Chess.Logic;

public class Bishop : Piece
{
    public override bool CanMove(int rank, int file)
    {
        if (!IsValidMove(rank, file))
            return false;

        if (Math.Abs(rank - Rank) != Math.Abs(file - File)) // trying to move not diagonally
            return false;

        if (rank > Rank && file > File) // moving up-right
        {
            for (int i = Rank + 1; i < rank; i++)
            {
                for (int j = File + 1; j < file; j++)
                {
                    if (Board.State[i, j] is not null)
                        return false;
                }
            }
        }
        else if (rank > Rank && file < File) // moving up-left
        {
            for (int i = Rank + 1; i < rank; i++)
            {
                for (int j = File - 1; j > File; j--)
                {
                    if (Board.State[i, j] is not null)
                        return false;
                }
            }
        }
        else if (rank < Rank && file > File) // moving down-right
        {
            for (int i = Rank -1; i > rank; i--)
            {
                for (int j = File + 1; j < file; j++)
                {
                    if (Board.State[i, j] is not null)
                        return false;
                }
            }
        }
        else if (rank < Rank && file < File) // moving down-left
        {
            for (int i = Rank -1; i > rank; i--)
            {
                for (int j = File - 1; j > file; j--)
                {
                    if (Board.State[i, j] is not null)
                        return false;
                }
            }
        }

        return true;
    }
}
