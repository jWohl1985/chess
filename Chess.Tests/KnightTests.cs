using Chess.Logic;
using FluentAssertions;
using static Chess.Logic.GameBoard;

namespace Chess.Tests;

public class KnightTests
{
    private readonly GameBoard _board;
    private readonly Knight _whiteKnight;
    private readonly Knight _blackKnight;

    public KnightTests()
    {
        _board = new GameBoard();
        _whiteKnight = new Knight() { Board = _board, Color = PieceColor.White };
        _blackKnight = new Knight() { Board = _board, Color = PieceColor.Black };
    }

    [Fact]
    public void Should_Not_Be_Able_To_Move_Off_Board()
    {
        // Arrange
        _board.State[RANK_8, FILE_H] = _whiteKnight;

        // Act
        bool canMoveTooFarUpRight = _whiteKnight.CanMove(_whiteKnight.CurrentRank + 1, _whiteKnight.CurrentFile - 2);
        bool canMoveTooFarDownRight = _whiteKnight.CanMove(_whiteKnight.CurrentRank + 1, _whiteKnight.CurrentFile + 2);
        bool canMoveTooFarDownLeft = _whiteKnight.CanMove(_whiteKnight.CurrentRank - 1, _whiteKnight.CurrentFile + 2);
        bool canMoveTooFarUpLeft = _whiteKnight.CanMove(_whiteKnight.CurrentRank + 2, _whiteKnight.CurrentFile - 1);

        // Assert
        canMoveTooFarUpRight.Should().BeFalse();
        canMoveTooFarDownRight.Should().BeFalse();
        canMoveTooFarDownLeft.Should().BeFalse();
        canMoveTooFarUpLeft.Should().BeFalse();
    }

    [Fact]
    public void Should_Only_Move_In_L_Shape()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKnight;
        List<(int, int)> validMoves =
            [(RANK_6, FILE_C), (RANK_6, FILE_E),
             (RANK_5, FILE_B), (RANK_5, FILE_F),
             (RANK_3, FILE_B), (RANK_3, FILE_F),
             (RANK_2, FILE_C), (RANK_2, FILE_E)];

        // Act
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                bool canMoveToSquare = _whiteKnight.CanMove(i, j);

                // Assert
                canMoveToSquare.Should().Be(validMoves.Contains((i, j)));
            }
        }
    }

    [Fact]
    public void Should_Not_Move_To_The_Same_Square()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKnight;

        // Act
        bool canMoveToSameSquare = _whiteKnight.CanMove(RANK_4, FILE_D);

        // Assert
        canMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKnight;
        _board.State[RANK_6, FILE_E] = new Queen() { Board = _board, Color = PieceColor.White };

        _board.State[RANK_7, FILE_G] = _blackKnight;
        _board.State[RANK_5, FILE_F] = new Queen() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveOntoOwnPiece = _whiteKnight.CanMove(RANK_6, FILE_E);
        bool blackCanMoveOntoOwnPiece = _blackKnight.CanMove(RANK_5, FILE_F);

        // Assert
        whiteCanMoveOntoOwnPiece.Should().BeFalse();
        blackCanMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Jump_Over_Friendly_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKnight;
        _board.State[RANK_5, FILE_E] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_5, FILE_D] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.White };

        _board.State[RANK_1, FILE_A] = _blackKnight;
        _board.State[RANK_2, FILE_A] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_1, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_2, FILE_B] = new Pawn() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanJumpPieces = _whiteKnight.CanMove(RANK_6, FILE_E);
        bool blackCanJumpPieces = _blackKnight.CanMove(RANK_3, FILE_B);

        // Assert
        whiteCanJumpPieces.Should().BeTrue();
        blackCanJumpPieces.Should().BeTrue();
    }

    [Fact]
    public void Should_Capture_Enemy_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKnight;
        _board.State[RANK_2, FILE_C] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[RANK_7, FILE_G] = _blackKnight;
        _board.State[RANK_5, FILE_F] = new Knight() { Board = _board, Color = PieceColor.White };

        // Assert
        bool whiteCanCaptureEnemyPiece = _whiteKnight.CanMove(RANK_2, FILE_C);
        bool blackCanCaptureEnemyPiece = _blackKnight.CanMove(RANK_5, FILE_F);

        // Assert
        whiteCanCaptureEnemyPiece.Should().BeTrue();
        blackCanCaptureEnemyPiece.Should().BeTrue();
    }

    [Fact]
    public void Should_Jump_Over_Enemy_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKnight;
        _board.State[RANK_5, FILE_E] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_5, FILE_D] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_4, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.Black };

        _board.State[RANK_1, FILE_A] = _blackKnight;
        _board.State[RANK_2, FILE_A] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_1, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_2, FILE_B] = new Pawn() { Board = _board, Color = PieceColor.White };

        // Act
        bool whiteCanJumpOverEnemyPieces = _whiteKnight.CanMove(RANK_6, FILE_E);
        bool blackCanJumpOverEnemyPieces = _blackKnight.CanMove(RANK_3, FILE_B);

        // Assert
        whiteCanJumpOverEnemyPieces.Should().BeTrue();
        blackCanJumpOverEnemyPieces.Should().BeTrue();
    }
}