using static Chess.Logic.GameBoard;

namespace Chess.Logic;

public class King : Piece
{
    public bool IsInCheck => IsBeingAttackedByEnemyPiece();

    public override bool CanMove(int newRank, int newFile)
    {
        if (!IsMoveValidForAnyPieceType(newRank, newFile))
        {
            return false;
        }

        return Math.Abs(newRank - CurrentRank) <= 1 && Math.Abs(newFile - CurrentFile) <= 1;
    }

    private bool IsBeingAttackedByEnemyPiece()
    {
        if (IsAttackedByPawn()
            || IsAttackedByKnight()
            || IsAttackedDiagonallyByBishopOrQueen()
            || IsAttackedHorizontallyByRookOrQueen()
            || IsAttackedVerticallyByRookOrQueen()
            || IsAdjacentToEnemyKing())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsAttackedByPawn()
    {
        if (CurrentRank < RANK_7 && Color == PieceColor.White)
        {
            if (CurrentFile > FILE_A && Board.State[CurrentRank + 1, CurrentFile - 1] is Pawn pawn1 && pawn1.Color == PieceColor.Black)
                return true;

            if (CurrentFile < FILE_H && Board.State[CurrentRank + 1, CurrentFile + 1] is Pawn pawn2 && pawn2.Color == PieceColor.Black)
                return true;
        }

        if (CurrentRank > RANK_2 && Color == PieceColor.Black)
        {
            if (CurrentFile > FILE_A && Board.State[CurrentRank - 1, CurrentFile - 1] is Pawn pawn1 && pawn1.Color == PieceColor.White)
                return true;

            if (CurrentFile < FILE_H && Board.State[CurrentRank - 1, CurrentFile + 1] is Pawn pawn2 && pawn2.Color == PieceColor.White)
                return true;
        }

        return false;
    }

    private bool IsAttackedByKnight()
    {
        if (CurrentRank < RANK_7) // check two ranks up for knights
        {
            if (CurrentFile > FILE_A && Board.State[CurrentRank + 2, CurrentFile - 1] is Knight knight1 && knight1.Color != Color)
                return true;

            else if (CurrentFile < FILE_H && Board.State[CurrentRank + 2, CurrentFile + 1] is Knight knight2 && knight2.Color != Color)
                return true;
        }
        if (CurrentRank > RANK_2) // check two ranks down for knights
        {
            if (CurrentFile > FILE_A && Board.State[CurrentRank - 2, CurrentFile - 1] is Knight knight1 && knight1.Color != Color)
                return true;
            else if (CurrentFile < FILE_H && Board.State[CurrentRank - 2, CurrentFile + 1] is Knight knight2 && knight2.Color != Color)
                return true;
        }
        if (CurrentRank < RANK_8) // check one rank up for knights
        {
            if (CurrentFile > FILE_B && Board.State[CurrentRank + 1, CurrentFile - 2] is Knight knight1 && knight1.Color != Color)
                return true;

            if (CurrentFile < FILE_G && Board.State[CurrentRank + 1, CurrentFile + 2] is Knight knight2 && knight2.Color != Color)
                return true;
        }
        if (CurrentRank > RANK_1) // check one rank down for knights
        {
            if (CurrentFile > FILE_B && Board.State[CurrentRank - 1, CurrentFile - 2] is Knight knight1 && knight1.Color != Color)
                return true;
            if (CurrentFile < FILE_G && Board.State[CurrentRank - 1, CurrentFile + 2] is Knight knight2 && knight2.Color != Color)
                return true;
        }

        return false;
    }

    private bool IsAttackedDiagonallyByBishopOrQueen()
    {
        // Check up and to the right
        for (int i = 1; CurrentRank + i <= RANK_8 && CurrentFile + i <= FILE_H; i++)
        {
            if (Board.State[CurrentRank + i, CurrentFile + i] is null)
                continue;

            else if (Board.State[CurrentRank + i, CurrentFile + i] is Queen queen && queen.Color != Color)
                return true;

            else if (Board.State[CurrentRank + i, CurrentFile + i] is Bishop bishop && bishop.Color != Color)
                return true;

            else // we found a different piece that wouldn't be attacking us
                break;
        }

        // Check up and to the left
        for (int i = 1; CurrentRank + i <= RANK_8 && CurrentFile - i >= FILE_A; i++)
        {
            if (Board.State[CurrentRank + i, CurrentFile - i] is null)
                continue;

            else if (Board.State[CurrentRank + i, CurrentFile - i] is Queen queen && queen.Color != Color)
                return true;

            else if (Board.State[CurrentRank + i, CurrentFile - i] is Bishop bishop && bishop.Color != Color)
                return true;

            else // we found a different piece that wouldn't be attacking us
                break;
        }

        // Check down and to the right
        for (int i = 1; CurrentRank - i >= RANK_1 && CurrentFile + i <= FILE_H; i++)
        {
            if (Board.State[CurrentRank - i, CurrentFile + i] is null)
                continue;

            else if (Board.State[CurrentRank - i, CurrentFile + i] is Queen queen && queen.Color != Color)
                return true;

            else if (Board.State[CurrentRank - i, CurrentFile + i] is Bishop bishop && bishop.Color != Color)
                return true;

            else // we found a different piece that wouldn't be attacking us
                break;
        }

        // Check down and to the left
        for (int i = 1; CurrentRank - i >= RANK_1 && CurrentFile - i >= FILE_A; i++)
        {
            if (Board.State[CurrentRank - i, CurrentFile - i] is null)
                continue;

            else if (Board.State[CurrentRank - i, CurrentFile - i] is Queen queen && queen.Color != Color)
                return true;

            else if (Board.State[CurrentRank - i, CurrentFile - i] is Bishop bishop && bishop.Color != Color)
                return true;

            else // we found a different piece that wouldn't be attacking us
                break;
        }

        return false;
    }

    private bool IsAttackedHorizontallyByRookOrQueen()
    {
        // Check to the right
        for (int i = CurrentFile + 1; i <= FILE_H; i++)
        {
            if (Board.State[CurrentRank, i] is null)
                continue;

            else if (Board.State[CurrentRank, i] is Queen queen && queen.Color != Color)
                return true;

            else if (Board.State[CurrentRank, i] is Rook rook && rook.Color != Color)
                return true;

            else // we found a different piece that wouldn't be attacking us
                break;
        }

        // Check to the left
        for (int i = CurrentFile - 1; i >= FILE_A; i--)
        {
            if (Board.State[CurrentRank, i] is null)
                continue;

            else if (Board.State[CurrentRank, i] is Queen queen && queen.Color != Color)
                return true;

            else if (Board.State[CurrentRank, i] is Rook rook && rook.Color != Color)
                return true;

            else // we found a different piece that wouldn't be attacking us
                break;
        }

        return false;
    }

    private bool IsAttackedVerticallyByRookOrQueen()
    {
        // Check up
        for (int i = CurrentRank + 1; i <= RANK_8; i++)
        {
            if (Board.State[i, CurrentFile] is null)
                continue;

            else if (Board.State[i, CurrentFile] is Queen queen && queen.Color != Color)
                return true;

            else if (Board.State[i, CurrentFile] is Rook rook && rook.Color != Color)
                return true;

            else // we found a different piece that wouldn't be attacking us
                break;
        }

        // Check down
        for (int i = CurrentRank - 1; i >= RANK_1; i--)
        {
            if (Board.State[i, CurrentFile] is null)
                continue;

            else if (Board.State[i, CurrentFile] is Queen queen && queen.Color != Color)
                return true;

            else if (Board.State[i, CurrentFile] is Rook rook && rook.Color != Color)
                return true;

            else // we found a different piece that wouldn't be attacking us
                break;
        }

        return false;
    }

    private bool IsAdjacentToEnemyKing()
    {
        King otherKing = Color == PieceColor.White ? Board.BlackKing : Board.WhiteKing;

        int otherKingRank = otherKing.CurrentRank;
        int otherKingFile = otherKing.CurrentFile;

        return Math.Abs(CurrentRank - otherKingRank) <= 1 && Math.Abs(CurrentFile - otherKingFile) <= 1;
    }
}