namespace Chess.Logic;

public class Rook : Piece
{
    public override bool CanMove(int newRank, int newFile)
    {
        if (!IsMoveValidForAnyPieceType(newRank, newFile))
            return false;

        if (newRank != CurrentRank && newFile != CurrentFile)
            return false;

        if (IsAnotherPieceInTheWay(newRank, newFile))
            return false;

        return true;
    }

    private bool IsAnotherPieceInTheWay(int newRank, int newFile)
    {
        if (newRank < CurrentRank) // down
        {
            for (int i = CurrentRank - 1; i > newRank; i--)
            {
                if (Board.State[i, CurrentFile] is not null)
                    return true;
            }
        }
        else if (newRank > CurrentRank) // up
        {
            for (int i = CurrentRank + 1; i < newRank; i++)
            {
                if (Board.State[i, CurrentFile] is not null)
                    return true;
            }
        }
        else if (newFile < CurrentFile) // moving left
        {
            for (int i = CurrentFile - 1; i > newFile; i--)
            {
                if (Board.State[CurrentRank, i] is not null)
                    return true;
            }
        }
        else if (newFile > CurrentFile) // moving right
        {
            for (int i = CurrentFile + 1; i < newFile; i++)
            {
                if (Board.State[CurrentRank, i] is not null)
                    return true;
            }
        }

        return false;
    }
            
}