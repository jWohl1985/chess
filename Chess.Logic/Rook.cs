using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic;

public class Rook : Piece
{
    public override bool CanMove(int rank, int file)
    {
        if (!IsValidMove(rank, file))
            return false;

        if (rank != Rank && file != File) // trying to move not in a straight line
            return false;

        if (rank < Rank) // down
        {
            for (int i = Rank - 1; i > rank; i--)
            {
                if (Board.State[i, File] is not null)
                    return false;
            }
        }
        else if (rank > Rank) // up
        {
            for (int i = Rank + 1; i < rank; i++)
            {
                if (Board.State[i, File] is not null)
                    return false;
            }
        }
        else if (file < File) // moving left
        {
            for (int i = File - 1; i > file; i--)
            {
                if (Board.State[Rank, i] is not null)
                    return false;
            }
        }
        else if (file > File) // moving right
        {
            for (int i = File + 1; i < file; i++)
            {
                if (Board.State[Rank, i] is not null)
                    return false;
            }
        }

        return true;
    }
            
}
