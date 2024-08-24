using static Chess.Logic.GameBoard;

namespace Chess.Logic;

public abstract class Piece
{
    public required PieceColor Color { get; init; }

    public required GameBoard Board { get; init; }

    public int CurrentRank => GetRank();

    public int CurrentFile => GetFile();

    public abstract bool CanMove(int rank, int file);

    protected bool IsMoveValidForAnyPieceType(int newRank, int newFile)
    {
        if (newRank < RANK_1 || newRank > RANK_8 || newFile < FILE_A || newFile > FILE_H) // moving off the board
            return false;

        if (newRank == CurrentRank && newFile == CurrentFile) // trying to 'move' to the square we're already on
            return false;

        if (Board.State[newRank, newFile] is Piece piece && piece.Color == Color) // trying to move onto a friendly piece
            return false;

        if (DoesMovePutOwnKingInCheck(newRank, newFile)) // the move puts our own king in check
            return false;

        return true;
    }

    private bool DoesMovePutOwnKingInCheck(int newRank, int newFile)
    {
        int oldRank = CurrentRank;
        int oldFile = CurrentFile;

        // temporarily put this piece on its new square, saving whatever piece was already there if we're capturing
        Piece? pieceBeingCaptured = Board.State[newRank, newFile];
        Board.State[oldRank, oldFile] = null;
        Board.State[newRank, newFile] = this;

        // see if the king is in check
        King kingOfCurrentPlayer = Color == PieceColor.White ? Board.WhiteKing : Board.BlackKing;
        bool movePutsKingInCheck = kingOfCurrentPlayer.IsInCheck ? true : false;

        // put things back how they were
        Board.State[oldRank, oldFile] = this;
        Board.State[newRank, newFile] = pieceBeingCaptured;

        return movePutsKingInCheck;
    }

    private int GetRank()
    {
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                if (Board.State[i, j] == this)
                    return i;
            }
        }

        return -1;
    }

    private int GetFile()
    {
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                if (Board.State[i, j] == this)
                    return j;
            }
        }

        return -1;
    }
}