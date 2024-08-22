using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic;

public class GameBoard
{
    public const int WIDTH = 8;
    public const int HEIGHT = 8;

    public Piece?[,] State { get; private set; } = new Piece[WIDTH, HEIGHT];

    public PieceColor TurnColor { get; private set; }

    public King WhiteKing { get; private set; }

    public King BlackKing { get; private set; }

    public GameBoard()
    {
        TurnColor = PieceColor.White;
        WhiteKing = new King() { Board = this, Color = PieceColor.White };
        BlackKing = new King() { Board = this, Color = PieceColor.Black };
    }

    public void SetupGame()
    {
        for (int i = 0; i <= WIDTH; i++)
        {
            State[1, i] = new Pawn()
            {
                Board = this,
                Color = PieceColor.White,
            };
        }

        for (int i = 0; i <= WIDTH; i++)
        {
            State[6, i] = new Pawn()
            {
                Board = this,
                Color = PieceColor.Black,
            };
        }

        State[0, 0] = new Rook() { Board = this, Color = PieceColor.White };
        State[0, 7] = new Rook() { Board = this, Color = PieceColor.White };
        State[0, 1] = new Knight() { Board = this, Color = PieceColor.White };
        State[0, 6] = new Knight() { Board = this, Color = PieceColor.White };
        State[0, 2] = new Bishop() { Board = this, Color = PieceColor.White };
        State[0, 5] = new Bishop() { Board = this, Color = PieceColor.White };
        State[0, 3] = new Queen() { Board = this, Color = PieceColor.White };
        State[0, 4] = WhiteKing;

        State[7, 0] = new Rook() { Board = this, Color = PieceColor.Black };
        State[7, 7] = new Rook() { Board = this, Color = PieceColor.Black };
        State[7, 1] = new Knight() { Board = this, Color = PieceColor.Black };
        State[7, 6] = new Knight() { Board = this, Color = PieceColor.Black };
        State[7, 2] = new Bishop() { Board = this, Color = PieceColor.Black };
        State[7, 5] = new Bishop() { Board = this, Color = PieceColor.Black };
        State[7, 3] = new Queen() { Board = this, Color = PieceColor.Black };
        State[7, 4] = BlackKing;

        TurnColor = PieceColor.White;
    }

    public void ClearBoard() => State = new Piece?[WIDTH, HEIGHT];

    public void ChangePlayerTurns()
    {
        TurnColor = TurnColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
    }

    public bool AttemptMove(Piece movingPiece, int newRank, int newFile)
    {
        if (MoveIsLegal(movingPiece, newRank, newFile))
        {
            State[newRank, newFile] = movingPiece;
            ChangePlayerTurns();
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool MoveIsLegal(Piece movingPiece, int newRank, int newFile)
    {
        if (movingPiece.Board != this) // the piece is not on this board
        {
            throw new Exception("Piece is checking a different board than it's on!");
        }

        if (movingPiece.Color != TurnColor // not that player's turn
            || !movingPiece.CanMove(newRank, newFile) // the piece doesn't move that way
            || MovePutsPlayersKingInCheck(movingPiece, newRank, newFile)) // move would put own king in check
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool MovePutsPlayersKingInCheck(Piece movingPiece, int newRank, int newFile)
    {
        int oldRank = movingPiece.Rank;
        int oldFile = movingPiece.File;

        // store the contents of the destination square if a piece is already there, and temporarily set the moving piece there
        Piece? pieceOnDestinationSquare = State[newRank, newFile];
        State[oldRank, oldFile] = null;
        State[newRank, newFile] = movingPiece;

        // see if the king is in check
        King kingOfCurrentPlayer = TurnColor == PieceColor.White ? WhiteKing : BlackKing;
        bool movePutsKingInCheck = kingOfCurrentPlayer.IsInCheck ? false : true;

        // put things back how they were
        State[oldRank, oldFile] = movingPiece;
        State[newRank, newFile] = pieceOnDestinationSquare;

        return movePutsKingInCheck;
    }
}