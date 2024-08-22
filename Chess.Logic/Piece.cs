using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic;

public abstract class Piece
{
    public required PieceColor Color { get; init; }

    public required GameBoard Board { get; init; }

    public int Rank => GetRank();

    public int File => GetFile();


    public abstract bool CanMove(int rank, int file);

    protected bool IsValidMove(int newRank, int newFile)
    {
        if (newRank < 0 || newRank > 7 || newFile < 0 || newFile > 7) // moving off the board
            return false;

        if (newRank == Rank && newFile == File) // trying to 'move' to the same square
            return false;

        if (Board.State[newRank, newFile] is Piece piece && piece.Color == Color) // trying to move onto friendly piece
            return false;

        return true;
    }

    private int GetRank()
    {
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                if (Board.State[i, j] == this)
                    return i;
            }
        }

        // if we got this far something is wrong
        throw new Exception("Piece is not on the board!");
    }

    private int GetFile()
    {
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                if (Board.State[i, j] == this)
                    return j;
            }
        }

        // if we got this far something is wrong
        throw new Exception("Piece is not on the board!");
    }
}
