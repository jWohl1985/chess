using static Chess.Logic.GameBoard;

namespace Chess.Logic;

public class Pawn : Piece
{
    public bool HasMoved => (Color == PieceColor.White && CurrentRank != RANK_2) || (Color == PieceColor.Black && CurrentRank != RANK_7);

    public override bool CanMove(int newRank, int newFile)
    {
        if (!IsMoveValidForAnyPieceType(newRank, newFile))
            return false;

        if (!IsMovingTowardsPromotion(newRank, newFile))
            return false;

        if (Math.Abs(newFile - CurrentFile) > 1)
            return false;

        // can't move more than 1 rank if it's already moved
        if (HasMoved && Math.Abs(newRank - CurrentRank) != 1)
            return false;

        // can't move more than 2 ranks if it hasn't yet moved
        if (!HasMoved && Math.Abs(newRank - CurrentRank) > 2)
            return false;

        int rankMovementDirection = Color == PieceColor.White ? 1 : -1;

        if (Math.Abs(newFile - CurrentFile) == 1) // capturing
        {
            if (Board.State[CurrentRank + rankMovementDirection, newFile] is not Piece piece || piece.Color == Color)
                return false;
        }
        else if (newFile == CurrentFile) // moving forward
        {
            if (Board.State[newRank, newFile] is Piece) // can't land on other pieces when moving forward
                return false;

            if (Math.Abs(newRank - CurrentRank) == 2)
            {
                if (Board.State[newRank - rankMovementDirection, newFile] is Piece) // can't jump over a piece when moving 2
                    return false;
            }
        }

        return true;
    }

    private bool IsMovingTowardsPromotion(int newRank, int newFile)
    {
        if (Color == PieceColor.White && (newRank - CurrentRank) <= 0)
            return false;

        if (Color == PieceColor.Black && (newRank - CurrentRank) >= 0)
            return false;

        return true;
    }
}