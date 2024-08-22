using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic;

public class King : Piece
{
    public bool IsInCheck => IsBeingAttackedByEnemyPiece();

    private PieceColor oppositeColor => Color == PieceColor.White ? PieceColor.Black : PieceColor.White;

    public override bool CanMove(int rank, int file)
    {
        if (!IsValidMove(rank, file))
        {
            return false;
        }

        if (Math.Abs(rank - Rank) <=1 && Math.Abs(file - File) <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
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
        if (Rank < 7 && Color == PieceColor.White)
        {
            if (File > 0 && Board.State[Rank + 1, File - 1] is Pawn pawn1 && pawn1.Color == oppositeColor)
                return true;

            if (File < 7 && Board.State[Rank + 1, File + 1] is Pawn pawn2 && pawn2.Color == oppositeColor)
                return true;
        }

        if (Rank > 0 && Color == PieceColor.Black)
        {
            if (File > 0 && Board.State[Rank - 1, File - 1] is Pawn pawn1 && pawn1.Color == oppositeColor)
                return true;

            if (File < 7 && Board.State[Rank - 1, File + 1] is Pawn pawn2 && pawn2.Color == oppositeColor)
                return true;
        }

        return false;
    }

    private bool IsAttackedByKnight()
    {
        if (Rank < 6)
        {
            if (File > 0 && Board.State[Rank + 2, File - 1] is Knight knight1 && knight1.Color == oppositeColor)
                return true;

            else if (File < 7 && Board.State[Rank + 2, File + 1] is Knight knight2 && knight2.Color == oppositeColor)
                return true;
        }
        if (Rank > 1)
        {
            if (File > 0 && Board.State[Rank - 2, File - 1] is Knight knight1 && knight1.Color == oppositeColor)
                return true;
            else if (File < 7 && Board.State[Rank - 2, File + 1] is Knight knight2 && knight2.Color == oppositeColor)
                return true;
        }
        if (Rank < 7)
        {
            if (File > 1 && Board.State[Rank + 1, File - 2] is Knight knight1 && knight1.Color == oppositeColor)
                return true;

            if (File < 6 && Board.State[Rank + 1, File + 2] is Knight knight2 && knight2.Color == oppositeColor)
                return true;
        }
        if (Rank > 0)
        {
            if (File > 1 && Board.State[Rank - 1, File - 2] is Knight knight1 && knight1.Color == oppositeColor)
                return true;
            if (File < 6 && Board.State[Rank - 1, File + 2] is Knight knight2 && knight2.Color == oppositeColor)
                return true;
        }

        return false;
    }

    private bool IsAttackedDiagonallyByBishopOrQueen()
    {
        // Check up and to the right
        for (int i = 1; Rank + i <= 7 && File + i <= 7; i++)
        {

            if (Board.State[Rank + i, File + i] is null)
                continue;

            else if (Board.State[Rank + i, File + i] is Queen queen && queen.Color == oppositeColor)
                return true;

            else if (Board.State[Rank + i, File + i] is Bishop bishop && bishop.Color == oppositeColor)
                return true;

            else
                break;
        }

        // Check up and to the left
        for (int i = 1; Rank + i <= 7 && File - i >= 0; i++)
        {
            if (Board.State[Rank + i, File - i] is null)
                continue;

            else if (Board.State[Rank + i, File - i] is Queen queen && queen.Color == oppositeColor)
                return true;

            else if (Board.State[Rank + i, File - i] is Bishop bishop && bishop.Color == oppositeColor)
                return true;

            else
                break;
        }

        // Check down and to the right
        for (int i = 1; Rank - i >= 0 && File + i <= 7; i++)
        {
            if (Board.State[Rank - i, File + i] is null)
                continue;

            else if (Board.State[Rank - i, File + i] is Queen queen && queen.Color == oppositeColor)
                return true;

            else if (Board.State[Rank - i, File + i] is Bishop bishop && bishop.Color == oppositeColor)
                return true;

            else
                break;
        }

        // Check down and to the left
        for (int i = 1; Rank - i >= 0 && File - i >= 0; i++)
        {
            if (Board.State[Rank - i, File - i] is null)
                continue;

            else if (Board.State[Rank - i, File - i] is Queen queen && queen.Color == oppositeColor)
                return true;

            else if (Board.State[Rank - i, File - i] is Bishop bishop && bishop.Color == oppositeColor)
                return true;

            else
                break;
        }

        return false;
    }

    private bool IsAttackedHorizontallyByRookOrQueen()
    {
        // Check to the right
        for (int i = File + 1; i <= 7; i++)
        {
            if (Board.State[Rank, i] is null)
                continue;

            else if (Board.State[Rank, i] is Queen queen && queen.Color == oppositeColor)
                return true;

            else if (Board.State[Rank, i] is Rook rook && rook.Color == oppositeColor)
                return true;

            else
                break;
        }

        // Check to the left
        for (int i = File - 1; i >= 0; i--)
        {
            if (Board.State[Rank, i] is null)
                continue;

            else if (Board.State[Rank, i] is Queen queen && queen.Color == oppositeColor)
                return true;

            else if (Board.State[Rank, i] is Rook rook && rook.Color == oppositeColor)
                return true;

            else
                break;
        }

        return false;
    }

    private bool IsAttackedVerticallyByRookOrQueen()
    {
        // Check up
        for (int i = File + 1; i <= 7; i++)
        {
            if (Board.State[i, File] is null)
                continue;

            else if (Board.State[i, File] is Queen queen && queen.Color == oppositeColor)
                return true;

            else if (Board.State[i, File] is Rook rook && rook.Color == oppositeColor)
                return true;

            else
                break;
        }

        // Check down
        for (int i = File - 1; i >= 0; i--)
        {
            if (Board.State[i, File] is null)
                continue;

            else if (Board.State[i, File] is Queen queen && queen.Color == oppositeColor)
                return true;

            else if (Board.State[i, File] is Rook rook && rook.Color == oppositeColor)
                return true;

            else
                break;
        }

        return false;
    }

    private bool IsAdjacentToEnemyKing()
    {
        King otherKing = Color == PieceColor.White ? Board.BlackKing : Board.WhiteKing;

        try
        {
            int otherKingRank = otherKing.Rank;
            int otherKingFile = otherKing.File;

            if (Math.Abs(Rank - otherKingRank) <= 1 && Math.Abs(File - otherKingFile) <= 1)
                return true;
            else
                return false;
        }
        catch
        {
            return false;
        }
    }
}
