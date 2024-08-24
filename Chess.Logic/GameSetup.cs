using static Chess.Logic.GameBoard;

namespace Chess.Logic;

public static class GameSetup
{
    public static void SetupPieces(GameBoard board)
    {
        SetupPawns(board);
        SetupRooks(board);
        SetupKnights(board);
        SetupBishops(board);
        SetupQueens(board);
        // kings are set up in the board constructor
    }

    private static void SetupPawns(GameBoard board)
    {
        for (int i = FILE_A; i <= FILE_H; i++)
        {
            board.State[RANK_2, i] = new Pawn() { Board = board, Color = PieceColor.White };
        }

        for (int i = FILE_A; i <= FILE_H; i++)
        {
            board.State[RANK_7, i] = new Pawn() { Board = board, Color = PieceColor.Black };
        }
    }

    private static void SetupRooks(GameBoard board)
    {
        board.State[RANK_1, FILE_A] = new Rook() { Board = board, Color = PieceColor.White };
        board.State[RANK_1, FILE_H] = new Rook() { Board = board, Color = PieceColor.White };
        board.State[RANK_8, FILE_A] = new Rook() { Board = board, Color = PieceColor.Black };
        board.State[RANK_8, FILE_H] = new Rook() { Board = board, Color = PieceColor.Black };
    }

    private static void SetupKnights(GameBoard board)
    {
        board.State[RANK_1, FILE_B] = new Knight() { Board = board, Color = PieceColor.White };
        board.State[RANK_1, FILE_G] = new Knight() { Board = board, Color = PieceColor.White };
        board.State[RANK_8, FILE_B] = new Knight() { Board = board, Color = PieceColor.Black };
        board.State[RANK_8, FILE_G] = new Knight() { Board = board, Color = PieceColor.Black };
    }

    private static void SetupBishops(GameBoard board)
    {
        board.State[RANK_1, FILE_C] = new Bishop() { Board = board, Color = PieceColor.White };
        board.State[RANK_1, FILE_F] = new Bishop() { Board = board, Color = PieceColor.White };
        board.State[RANK_8, FILE_C] = new Bishop() { Board = board, Color = PieceColor.Black };
        board.State[RANK_8, FILE_F] = new Bishop() { Board = board, Color = PieceColor.Black };
    }

    private static void SetupQueens(GameBoard board)
    {
        board.State[RANK_1, FILE_D] = new Queen() { Board = board, Color = PieceColor.White };
        board.State[RANK_8, FILE_D] = new Queen() { Board = board, Color = PieceColor.Black };
    }
}