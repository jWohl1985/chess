namespace Chess.Logic;

public class Bishop : Piece
{
    public override bool CanMove(int newRank, int newFile)
    {
        if (!IsMoveValidForAnyPieceType(newRank, newFile))
            return false;

        if (!IsMovingDiagonally(newRank, newFile))
            return false;

        if (IsAnotherPieceInTheWay(newRank, newFile))
            return false;

        return true;
    }

    private bool IsMovingDiagonally(int newRank, int newFile)
    {
        return Math.Abs(newRank - CurrentRank) == Math.Abs(newFile - CurrentFile);
    }

    private bool IsAnotherPieceInTheWay(int newRank, int newFile)
    {
        if (newRank > CurrentRank && newFile > CurrentFile) // moving up-right
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
        else if (newRank > CurrentRank && newFile < CurrentFile) // moving up-left
        {
            for (int i = CurrentRank + 1; i < newRank; i++)
            {
                for (int j = CurrentFile - 1; j > CurrentFile; j--)
                {
                    if (Board.State[i, j] is not null)
                        return true;
                }
            }
        }
        else if (newRank < CurrentRank && newFile > CurrentFile) // moving down-right
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
        else if (newRank < CurrentRank && newFile < CurrentFile) // moving down-left
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
