using Chess.Logic;
using FluentAssertions;

namespace Chess.Tests;

public class BishopTests
{
    private readonly GameBoard _board;
    private readonly Bishop _whiteBishop;
    private readonly Bishop _blackBishop;

    public BishopTests()
    {
        _board = new GameBoard();

        _whiteBishop = new Bishop() { Board = _board, Color = PieceColor.White };
        _blackBishop = new Bishop() { Board = _board, Color = PieceColor.Black };
    }

    [Fact]
    public void Should_Not_Be_Able_To_Move_Off_Board()
    {
        // Arrange
        _board.State[3, 3] = _whiteBishop;

        // Act
        bool canMoveTooFarUpRight = _whiteBishop.CanMove(_whiteBishop.Rank + 5, _whiteBishop.File + 5);
        bool canMoveTooFarDownRight = _whiteBishop.CanMove(_whiteBishop.Rank - 4, _whiteBishop.File + 4);
        bool canMoveTooFarDownLeft = _whiteBishop.CanMove(_whiteBishop.Rank - 4, _whiteBishop.File - 4);
        bool canMoveTooFarUpLeft = _whiteBishop.CanMove(_whiteBishop.Rank + 5, _whiteBishop.File - 4);

        // Assert
        canMoveTooFarUpRight.Should().BeFalse();
        canMoveTooFarDownRight.Should().BeFalse();
        canMoveTooFarDownLeft.Should().BeFalse();
        canMoveTooFarUpLeft.Should().BeFalse();
    }

    [Fact]
    public void Should_Only_Move_Diagonally()
    {
        // Arrange
        _board.State[3, 3] = _whiteBishop;

        // Act
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                bool canMoveToSquare = _whiteBishop.CanMove(i, j);

                if (Math.Abs(i - _whiteBishop.Rank) != Math.Abs(j - _whiteBishop.Rank))
                {
                        // Assert
                        canMoveToSquare.Should().BeFalse();
                }
                else
                {
                    // Assert
                    if (i != _whiteBishop.Rank && j != _whiteBishop.File)
                    {
                        canMoveToSquare.Should().BeTrue();
                    }
                }
            }
        }
    }

    [Fact]
    public void Should_Not_Move_To_The_Same_Square()
    {
        // Arrange
        _board.State[3, 3] = _whiteBishop;

        // Act
        bool canMoveToSameSquare = _whiteBishop.CanMove(3, 3);

        // Assert
        canMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteBishop;
        _board.State[4, 4] = new Queen() { Board = _board, Color = PieceColor.White };

        _board.State[6, 6] = _blackBishop;
        _board.State[7, 7] = new Queen() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveOntoOwnPiece = _whiteBishop.CanMove(4, 4);
        bool blackCanMoveOntoOwnPiece = _blackBishop.CanMove(7, 7);

        // Assert
        whiteCanMoveOntoOwnPiece.Should().BeFalse();
        blackCanMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Through_Friendly_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteBishop;
        _board.State[5, 5] = new Bishop() { Board = _board, Color = PieceColor.White };

        _board.State[0, 0] = _blackBishop;
        _board.State[1, 1] = new Bishop() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveThroughOwnPiece = _whiteBishop.CanMove(7, 7);
        bool blackCanMoveThroughOwnPiece = _blackBishop.CanMove(2, 2);

        // Assert
        whiteCanMoveThroughOwnPiece.Should().BeFalse();
        blackCanMoveThroughOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Capture_Enemy_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteBishop;
        _board.State[1, 1] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[4, 4] = _blackBishop;
        _board.State[6, 6] = new Knight() { Board = _board, Color = PieceColor.White };

        // Assert
        bool whiteCanCaptureEnemyPiece = _whiteBishop.CanMove(1, 1);
        bool blackCanCaptureEnemyPiece = _blackBishop.CanMove(6, 6);

        // Assert
        whiteCanCaptureEnemyPiece.Should().BeTrue();
        blackCanCaptureEnemyPiece.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Move_Through_Enemy_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteBishop;
        _board.State[4, 4] = new Bishop() { Board = _board, Color = PieceColor.Black };

        _board.State[0, 0] = _blackBishop;
        _board.State[1, 1] = new Bishop() { Board = _board, Color = PieceColor.White };

        // Act
        bool whiteCanMoveThroughEnemyPiece = _whiteBishop.CanMove(5, 5);
        bool blackCanMoveThroughEnemyPiece = _blackBishop.CanMove(2, 2);

        // Assert
        whiteCanMoveThroughEnemyPiece.Should().BeFalse();
        blackCanMoveThroughEnemyPiece.Should().BeFalse();
    }
}
