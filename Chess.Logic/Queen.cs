using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic;

public class Queen : Piece
{
    public override bool CanMove(int rank, int file)
    {
        if (!IsValidMove(rank, file))
            return false;

        if (rank != Rank && File != file && (Math.Abs(rank - Rank) != Math.Abs(file - File))) // not moaving straight or diagonal
            return false;

        if (rank == Rank && file < File) // moving left
        {
            for (int i = File - 1; i > file; i--)
            {
                if (Board.State[Rank, i] is not null)
                    return false;
            }
        }
        else if (rank == Rank && file > File) // moving right
        {
            for (int i = File + 1; i < file; i++)
            {
                if (Board.State[Rank, i] is not null)
                    return false;
            }
        }
        else if (file == File && rank < Rank) // moving down
        {
            for (int i = Rank - 1; i > rank; i--)
            {
                if (Board.State[i, File] is not null)
                    return false;
            }
        }
        else if (file == File && rank > Rank) // moving up
        {
            for (int i = Rank + 1; i < rank; i++)
            {
                if (Board.State[i, File] is not null)
                    return false;
            }
        }
        else if (rank > Rank && file > File) // moving up right
        {
            for (int i = Rank + 1; i < rank; i++)
            {
                for (int j = File + 1; j < file; j++)
                {
                    if (Board.State[i, j] is not null)
                        return false;
                }
            }
        }
        else if (rank > Rank && file < File) // moving up left
        {
            for (int i = Rank + 1; i < rank; i++)
            {
                for (int j = File - 1; j > file; j--)
                {
                    if (Board.State[i, j] is not null)
                        return false;
                }
            }
        }
        else if (rank < Rank && file > File) // moving down right
        {
            for (int i = Rank - 1; i > rank; i--)
            {
                for (int j = File + 1; j < file; j++)
                {
                    if (Board.State[i, j] is not null)
                        return false;
                }
            }
        }
        else if (rank < Rank && file < File) // moving down left
        {
            for (int i = Rank - 1; i > rank; i--)
            {
                for (int j = File - 1; j > file; j--)
                {
                    if (Board.State[i, j] is not null)
                        return false;
                }
            }
        }

        return true;
    }
}
