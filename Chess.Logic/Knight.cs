namespace Chess.Logic;

public class Knight : Piece
{
    public override bool CanMove(int newRank, int newFile)
    {
        if (!IsMoveValidForAnyPieceType(newRank, newFile))
            return false;

        if (!IsMovingLikeAKnight(newRank, newFile))
            return false;

        return true;
    }

    private bool IsMovingLikeAKnight(int newRank, int newFile)
    {
        if (Math.Abs(newRank - CurrentRank) == 2 && Math.Abs(newFile - CurrentFile) == 1)
            return true;

        if (Math.Abs(newRank - CurrentRank) == 1 && Math.Abs(newFile - CurrentFile) == 2)
            return true;

        return false;
    }
}