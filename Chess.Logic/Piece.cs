using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic;

public abstract class Piece
{
    // White or black
    public required PieceColor Color { get; init; }

    // The board the piece is on
    public required GameBoard Board { get; init; }

    // the row on the board the piece is in, 0-8
    public int Rank => GetMyRank();

    // the column on the board the piece is in, 0-8
    public int File => GetMyFile();

    // returns whether or not the piece can move to the square at [rank,file] on Board
    public virtual bool CanMove(int rank, int file)
    {
        // return false if rank or file is < 0 or > 7
        // return false if the square is already occupied by a piece the same color

        // override this and implement the rest of the logic for the specific piece in that piece's class after calling base.CanMove();

        // just put this here so it compiles, delete
        return false;
    }

    private int GetMyRank()
    {
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                if (Board.State[i, j] == this)
                    return i;
            }
        }

        throw new Exception();
    }

    private int GetMyFile()
    {
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                if (Board.State[i, j] == this)
                    return j;
            }
        }

        throw new Exception();
    }
}
