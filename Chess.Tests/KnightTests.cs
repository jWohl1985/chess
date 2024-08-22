using Chess.Logic;
using FluentAssertions;

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
        _board.State[7, 7] = _whiteKnight;

        // Act
        bool canMoveTooFarUpRight = _whiteKnight.CanMove(_whiteKnight.Rank + 1, _whiteKnight.File - 2);
        bool canMoveTooFarDownRight = _whiteKnight.CanMove(_whiteKnight.Rank + 1, _whiteKnight.File + 2);
        bool canMoveTooFarDownLeft = _whiteKnight.CanMove(_whiteKnight.Rank - 1, _whiteKnight.File + 2);
        bool canMoveTooFarUpLeft = _whiteKnight.CanMove(_whiteKnight.Rank + 2, _whiteKnight.File - 1);

        // Assert
        canMoveTooFarUpRight.Should().BeFalse();
        canMoveTooFarDownRight.Should().BeFalse();
        canMoveTooFarDownLeft.Should().BeFalse();
        canMoveTooFarUpRight.Should().BeFalse();
    }

    [Fact]
    public void Should_Only_Move_In_L_Shape()
    {
        // Arrange
        _board.State[3, 3] = _whiteKnight;

        // Act
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                bool canMoveToSquare = _whiteKnight.CanMove(i, j);

                if (i == 4 && (j == 5 || j == 1))
                    canMoveToSquare.Should().BeTrue();

                else if (i == 5 && (j == 4 || j == 2))
                    canMoveToSquare.Should().BeTrue();

                else if (i == 2 && (j == 5 || j == 1))
                    canMoveToSquare.Should().BeTrue();

                else if (i == 1 && (j == 4 || j == 2))
                    canMoveToSquare.Should().BeTrue();

                else
                    canMoveToSquare.Should().BeFalse();
            }
        }
    }

    [Fact]
    public void Should_Not_Move_To_The_Same_Square()
    {
        // Arrange
        _board.State[3, 3] = _whiteKnight;

        // Act
        bool canMoveToSameSquare = _whiteKnight.CanMove(3, 3);

        // Assert
        canMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteKnight;
        _board.State[5, 4] = new Queen() { Board = _board, Color = PieceColor.White };

        _board.State[6, 6] = _blackKnight;
        _board.State[4, 5] = new Queen() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveOntoOwnPiece = _whiteKnight.CanMove(5, 4);
        bool blackCanMoveOntoOwnPiece = _blackKnight.CanMove(4, 5);

        // Assert
        whiteCanMoveOntoOwnPiece.Should().BeFalse();
        blackCanMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Jump_Over_Friendly_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteKnight;
        _board.State[4, 4] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[4, 3] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[3, 4] = new Pawn() { Board = _board, Color = PieceColor.White };

        _board.State[0, 0] = _blackKnight;
        _board.State[1, 0] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[0, 1] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[1, 1] = new Pawn() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanJumpPieces = _whiteKnight.CanMove(5, 4);
        bool blackCanJumpPieces = _blackKnight.CanMove(2, 1);

        // Assert
        whiteCanJumpPieces.Should().BeTrue();
        blackCanJumpPieces.Should().BeTrue();
    }

    [Fact]
    public void Should_Capture_Enemy_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteKnight;
        _board.State[1, 2] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[6, 6] = _blackKnight;
        _board.State[4, 5] = new Knight() { Board = _board, Color = PieceColor.White };

        // Assert
        bool whiteCanCaptureEnemyPiece = _whiteKnight.CanMove(1, 2);
        bool blackCanCaptureEnemyPiece = _blackKnight.CanMove(4, 5);

        // Assert
        whiteCanCaptureEnemyPiece.Should().BeTrue();
        blackCanCaptureEnemyPiece.Should().BeTrue();
    }

    [Fact]
    public void Should_Jump_Over_Enemy_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteKnight;
        _board.State[4, 4] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[4, 3] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[3, 4] = new Pawn() { Board = _board, Color = PieceColor.Black };

        _board.State[0, 0] = _blackKnight;
        _board.State[1, 0] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[0, 1] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[1, 1] = new Pawn() { Board = _board, Color = PieceColor.White };

        // Act
        bool whiteCanJumpOverEnemyPieces = _whiteKnight.CanMove(5, 4);
        bool blackCanJumpOverEnemyPieces = _blackKnight.CanMove(2, 1);

        // Assert
        whiteCanJumpOverEnemyPieces.Should().BeTrue();
        blackCanJumpOverEnemyPieces.Should().BeTrue();
    }
}