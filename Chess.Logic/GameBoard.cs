namespace Chess.Logic;

public class GameBoard
{
    public const int BOARD_WIDTH = 8;
    public const int BOARD_HEIGHT = 8;

    public const int RANK_1 = 0;
    public const int RANK_2 = 1;
    public const int RANK_3 = 2;
    public const int RANK_4 = 3;
    public const int RANK_5 = 4;
    public const int RANK_6 = 5;
    public const int RANK_7 = 6;
    public const int RANK_8 = 7;

    public const int FILE_A = 0;
    public const int FILE_B = 1;
    public const int FILE_C = 2;
    public const int FILE_D = 3;
    public const int FILE_E = 4;
    public const int FILE_F = 5;
    public const int FILE_G = 6;
    public const int FILE_H = 7;

    public Piece?[,] State { get; private set; } = new Piece[BOARD_WIDTH, BOARD_HEIGHT];

    public PieceColor TurnColor { get; private set; }

    public King WhiteKing { get; private set; }

    public King BlackKing { get; private set; }

    public GameBoard()
    {
        WhiteKing = new King() { Board = this, Color = PieceColor.White };
        BlackKing = new King() { Board = this, Color = PieceColor.Black };
        State[RANK_1, FILE_E] = WhiteKing;
        State[RANK_8, FILE_E] = BlackKing;
    }

    public void SetupGame()
    {
        GameSetup.SetupPieces(this);
        TurnColor = PieceColor.White;
    }

    public void ClearBoard() => State = new Piece?[BOARD_WIDTH, BOARD_HEIGHT];

    public void ChangePlayerTurns()
    {
        TurnColor = TurnColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
    }

    public bool AttemptMove(Piece movingPiece, int newRank, int newFile)
    {
        if (movingPiece.Color == TurnColor && movingPiece.CanMove(newRank, newFile))
        {
            State[movingPiece.CurrentRank, movingPiece.CurrentFile] = null;
            State[newRank, newFile] = movingPiece;
            ChangePlayerTurns();
            return true;
        }
        else
        {
            return false;
        }
    }
}