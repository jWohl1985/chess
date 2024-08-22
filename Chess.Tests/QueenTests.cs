using Chess.Logic;
using FluentAssertions;
using Xunit.Sdk;

namespace Chess.Tests;

public class QueenTests
{
    private readonly GameBoard _board;
    private readonly Queen _queen;

    public QueenTests()
    {
        _board = new GameBoard();
        _queen = new Queen()
        {
            Board = _board,
            Color = PieceColor.White,
        };
    }

    [Fact]
    public void Should_Not_Be_Able_To_Move_Off_Board()
    {
        // Arrange
        _board.State[3, 3] = _queen;

        // Act
        bool canMoveTooFarLeft = _queen.CanMove(3, -1);
        bool canMoveTooFarRight = _queen.CanMove(3, 8);
        bool canMoveTooFarUp = _queen.CanMove(8, 3);
        bool canMoveTooFarDown = _queen.CanMove(-1, 3);

        bool canMoveTooFarUpRight = _queen.CanMove(8, 8);
        bool canMoveTooFarDownRight = _queen.CanMove(-1, 7);
        bool canMoveTooFarDownLeft = _queen.CanMove(-1, -1);
        bool canMoveTooFarUpLeft = _queen.CanMove(7, -1);

        // Assert
        canMoveTooFarLeft.Should().BeFalse();
        canMoveTooFarRight.Should().BeFalse();
        canMoveTooFarUp.Should().BeFalse();
        canMoveTooFarDown.Should().BeFalse();

        canMoveTooFarUpRight.Should().BeFalse();
        canMoveTooFarDownRight.Should().BeFalse();
        canMoveTooFarDownLeft.Should().BeFalse();
        canMoveTooFarUpLeft.Should().BeFalse();
    }

    [Fact]
    public void Should_Only_Move_Diagonal_And_Straight()
    {
        // Arrange
        _board.State[3, 3] = _queen;

        // Act
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                bool canMoveToSquare = _queen.CanMove(i, j);

                if (i == _queen.Rank && j == _queen.File)
                {
                    // Assert
                    canMoveToSquare.Should().BeFalse();
                }
                else if (i == _queen.Rank || j == _queen.File)
                {
                    // Assert
                    canMoveToSquare.Should().BeTrue();
                }
                else if (Math.Abs(i - _queen.Rank) == Math.Abs(j - _queen.File))
                {
                    // Assert
                    canMoveToSquare.Should().BeTrue();
                }
                else
                {
                    canMoveToSquare.Should().BeFalse();
                }

            }
        }
    }

    [Fact]
    public void Should_Not_Move_To_The_Same_Square()
    {
        // Arrange
        _board.State[3, 3] = _queen;

        // Act
        bool canMoveToSameSquare = _queen.CanMove(3, 3);

        // Assert
        canMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _queen;
        _board.State[5, 5] = new Pawn() { Board = _board, Color = PieceColor.White };

        // Act
        bool canMoveOntoOwnPiece = _queen.CanMove(5, 5);

        // Assert
        canMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Through_Friendly_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _queen;
        _board.State[4, 2] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[4, 3] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[4, 4] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[3, 4] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[2, 4] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[2, 3] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[2, 2] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[3, 2] = new Pawn() { Board = _board, Color = PieceColor.White };

        // Act
        bool canMoveThroughOwnPiece1 = _queen.CanMove(5, 1);
        bool canMoveThroughOwnPiece2 = _queen.CanMove(5, 3);
        bool canMoveThroughOwnPiece3 = _queen.CanMove(5, 5);
        bool canMoveThroughOwnPiece4 = _queen.CanMove(3, 5);
        bool canMoveThroughOwnPiece5 = _queen.CanMove(1, 5);
        bool canMoveThroughOwnPiece6 = _queen.CanMove(1, 3);
        bool canMoveThroughOwnPiece7 = _queen.CanMove(1, 1);
        bool canMoveThroughOwnPiece8 = _queen.CanMove(3, 1);

        // Assert
        canMoveThroughOwnPiece1.Should().BeFalse();
        canMoveThroughOwnPiece2.Should().BeFalse();
        canMoveThroughOwnPiece3.Should().BeFalse();
        canMoveThroughOwnPiece4.Should().BeFalse();
        canMoveThroughOwnPiece5.Should().BeFalse();
        canMoveThroughOwnPiece6.Should().BeFalse();
        canMoveThroughOwnPiece7.Should().BeFalse();
        canMoveThroughOwnPiece8.Should().BeFalse();
    }

    [Fact]
    public void Should_Capture_Enemy_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _queen;
        _board.State[0, 0] = new Knight() { Board = _board, Color = PieceColor.Black };

        // Assert
        bool canCaptureEnemyPiece = _queen.CanMove(0, 0);

        // Assert
        canCaptureEnemyPiece.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Move_Through_Enemy_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _queen;
        _board.State[4, 2] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[4, 3] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[4, 4] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[3, 4] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[2, 4] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[2, 3] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[2, 2] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[3, 2] = new Pawn() { Board = _board, Color = PieceColor.Black };

        // Act
        bool canMoveThroughEnemyPiece1 = _queen.CanMove(5, 1);
        bool canMoveThroughEnemyPiece2 = _queen.CanMove(5, 3);
        bool canMoveThroughEnemyPiece3 = _queen.CanMove(5, 5);
        bool canMoveThroughEnemyPiece4 = _queen.CanMove(3, 5);
        bool canMoveThroughEnemyPiece5 = _queen.CanMove(1, 5);
        bool canMoveThroughEnemyPiece6 = _queen.CanMove(1, 3);
        bool canMoveThroughEnemyPiece7 = _queen.CanMove(1, 1);
        bool canMoveThroughEnemyPiece8 = _queen.CanMove(3, 1);

        // Assert
        canMoveThroughEnemyPiece1.Should().BeFalse();
        canMoveThroughEnemyPiece2.Should().BeFalse();
        canMoveThroughEnemyPiece3.Should().BeFalse();
        canMoveThroughEnemyPiece4.Should().BeFalse();
        canMoveThroughEnemyPiece5.Should().BeFalse();
        canMoveThroughEnemyPiece6.Should().BeFalse();
        canMoveThroughEnemyPiece7.Should().BeFalse();
        canMoveThroughEnemyPiece8.Should().BeFalse();
    }
}
