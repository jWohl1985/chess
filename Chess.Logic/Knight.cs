using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic;

public class Knight : Piece
{
    public override bool CanMove(int rank, int file)
    {
        if (!IsValidMove(rank, file))
            return false;

        if (Math.Abs(rank - Rank) == 2 && Math.Abs(file - File) == 1)
            return true;

        if (Math.Abs(rank - Rank) == 1 && Math.Abs(file - File) == 2)
            return true;

        return false;
    }
}
