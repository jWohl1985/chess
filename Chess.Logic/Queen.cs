namespace Chess.Logic;

public class Queen : Piece
{
    public override bool CanMove(int newRank, int newFile)
    {
        if (!IsMoveValidForAnyPieceType(newRank, newFile))
            return false;

        if (!IsMovingLikeAQueen(newRank, newFile))
            return false;

        if (IsAnotherPieceInTheWay(newRank, newFile))
            return false;

        return true;
    }

    private bool IsMovingLikeAQueen(int newRank, int newFile)
    {
        bool isMovingHorizontal = newRank == CurrentRank;
        bool isMovingVertical = newFile == CurrentFile;
        bool isMovingDiagonal = Math.Abs(newRank - CurrentRank) == Math.Abs(newFile - CurrentFile);

        return isMovingHorizontal || isMovingVertical || isMovingDiagonal;
    }

    private bool IsAnotherPieceInTheWay(int newRank, int newFile)
    {
        if (newRank == CurrentRank && newFile < CurrentFile) // moving left
        {
            for (int i = CurrentFile - 1; i > newFile; i--)
            {
                if (Board.State[CurrentRank, i] is not null)
                    return true;
            }
        }
        else if (newRank == CurrentRank && newFile > CurrentFile) // moving right
        {
            for (int i = CurrentFile + 1; i < newFile; i++)
            {
                if (Board.State[CurrentRank, i] is not null)
                    return true;
            }
        }
        else if (newFile == CurrentFile && newRank < CurrentRank) // moving down
        {
            for (int i = CurrentRank - 1; i > newRank; i--)
            {
                if (Board.State[i, CurrentFile] is not null)
                    return true;
            }
        }
        else if (newFile == CurrentFile && newRank > CurrentRank) // moving up
        {
            for (int i = CurrentRank + 1; i < newRank; i++)
            {
                if (Board.State[i, CurrentFile] is not null)
                    return true;
            }
        }
        else if (newRank > CurrentRank && newFile > CurrentFile) // moving up right
        {
            for (int i = CurrentRank + 1; i < newRank; i++)
            {
                for (int j = CurrentFile + 1; j < newFile; j++)
                {
                    if (Board.State[i, j] is not null)
                        return true;
                }
            }
        }
        else if (newRank > CurrentRank && newFile < CurrentFile) // moving up left
        {
            for (int i = CurrentRank + 1; i < newRank; i++)
            {
                for (int j = CurrentFile - 1; j > newFile; j--)
                {
                    if (Board.State[i, j] is not null)
                        return true;
                }
            }
        }
        else if (newRank < CurrentRank && newFile > CurrentFile) // moving down right
        {
            for (int i = CurrentRank - 1; i > newRank; i--)
            {
                for (int j = CurrentFile + 1; j < newFile; j++)
                {
                    if (Board.State[i, j] is not null)
                        return true;
                }
            }
        }
        else if (newRank < CurrentRank && newFile < CurrentFile) // moving down left
        {
            for (int i = CurrentRank - 1; i > newRank; i--)
            {
                for (int j = CurrentFile - 1; j > newFile; j--)
                {
                    if (Board.State[i, j] is not null)
                        return true;
                }
            }
        }

        return false;
    }
}