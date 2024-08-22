using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic;

public class Pawn : Piece
{
    public bool HasMoved => !(Color == PieceColor.White && Rank == 1) && !(Color == PieceColor.Black && Rank == 6);

    public override bool CanMove(int rank, int file)
    {
        if (!IsValidMove(rank, file))
            return false;

        if (Math.Abs(file - File) > 1) // can't move more than one file
            return false;

        int rankMovementDirection = Color == PieceColor.White ? 1 : -1;

        if (HasMoved && (rank - Rank) != rankMovementDirection) // can't move more than one rank in the direction of its color, unless its the first move
            return false;

        if (!HasMoved && Math.Abs(rank - Rank) > 2) // can't move more than two ranks if it hasn't moved
            return false;

        if (Math.Abs(file - File) == 1) // capturing
        {
            if (Board.State[Rank + rankMovementDirection, file] is not Piece piece || piece.Color == Color)
                return false;
        }
        else if (file == File) // moving forward
        {
            if (Math.Abs(rank - Rank) == 2)
            {
                if (Board.State[rank - rankMovementDirection, file] is Piece)
                    return false;

                if (Board.State[rank, file] is Piece)
                    return false;
            }
            else if (Math.Abs(rank - Rank) == 1)
            {
                if (Board.State[rank, file] is Piece)
                    return false;
            }
        }

        return true;
    }
}
