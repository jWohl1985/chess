using Chess.Logic;
using FluentAssertions;

namespace Chess.Tests;

public class RookTests
{
    private readonly GameBoard _board;
    private readonly Rook _whiteRook;
    private readonly Rook _blackRook;

    public RookTests()
    {
        _board = new GameBoard();
        _whiteRook = new Rook() { Board = _board, Color = PieceColor.White };
        _blackRook = new Rook() { Board = _board, Color = PieceColor.Black };
    }

    [Fact]
    public void Should_Not_Be_Able_To_Move_Off_Board()
    {
        // Arrange
        _board.State[3, 3] = _whiteRook;

        // Act
        bool canMoveTooFarLeft = _whiteRook.CanMove(_whiteRook.Rank, _whiteRook.File - 4);
        bool canMoveTooFarRight = _whiteRook.CanMove(_whiteRook.Rank, _whiteRook.File + 5);
        bool canMoveTooFarDown = _whiteRook.CanMove(_whiteRook.Rank - 4, _whiteRook.File);
        bool canMoveTooFarUp = _whiteRook.CanMove(_whiteRook.Rank + 5, _whiteRook.File);

        // Assert
        canMoveTooFarLeft.Should().BeFalse();
        canMoveTooFarRight.Should().BeFalse();
        canMoveTooFarDown.Should().BeFalse();
        canMoveTooFarUp.Should().BeFalse();
    }

    [Fact]
    public void Should_Only_Move_In_Straight_Lines()
    {
        // Arrange
        _board.State[3, 3] = _whiteRook;

        // Act
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                bool canMoveToSquare = _whiteRook.CanMove(i, j);

                if (i != _whiteRook.Rank && j != _whiteRook.File)
                {

                    if (i != _whiteRook.Rank && j != _whiteRook.File)
                    {
                        // Assert
                        canMoveToSquare.Should().BeFalse();
                    }
                }
                else
                {
                    // Assert
                    if (i != _whiteRook.Rank && j != _whiteRook.File)
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
        _board.State[3, 3] = _whiteRook;

        // Act
        bool canMoveToSameSquare = _whiteRook.CanMove(3, 3);

        // Assert
        canMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteRook;
        _board.State[3, 4] = new Queen() { Board = _board, Color = PieceColor.White };

        _board.State[5, 5] = _blackRook;
        _board.State[5, 1] = new Queen() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveOntoOwnPiece = _whiteRook.CanMove(3, 4);
        bool blackCanMoveOntoOwnPiece = _blackRook.CanMove(5, 1);

        // Assert
        whiteCanMoveOntoOwnPiece.Should().BeFalse();
        blackCanMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Through_Friendly_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteRook;
        _board.State[5, 3] = new Bishop() { Board = _board, Color = PieceColor.White };

        _board.State[3, 6] = _blackRook;
        _board.State[2, 6] = new Bishop() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveThroughOwnPiece = _whiteRook.CanMove(6, 3);
        bool blackCanMoveThroughOwnPiece = _blackRook.CanMove(0, 6);

        // Assert
        whiteCanMoveThroughOwnPiece.Should().BeFalse();
        blackCanMoveThroughOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Capture_Enemy_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteRook;
        _board.State[1, 3] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[5, 5] = _blackRook;
        _board.State[5, 7] = new Knight() { Board = _board, Color = PieceColor.White };

        // Assert
        bool whiteCanCaptureEnemyPiece = _whiteRook.CanMove(1, 3);
        bool blackCanCaptureEnemyPiece = _blackRook.CanMove(5, 7);

        // Assert
        whiteCanCaptureEnemyPiece.Should().BeTrue();
        blackCanCaptureEnemyPiece.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Move_Through_Enemy_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteRook;
        _board.State[3, 1] = new Bishop() { Board = _board, Color = PieceColor.Black };

        _board.State[5, 5] = _blackRook;
        _board.State[3, 5] = new Bishop() { Board = _board, Color = PieceColor.White };

        // Act
        bool whiteCanMoveThroughEnemyPiece = _whiteRook.CanMove(3, 0);
        bool blackCanMoveThroughEnemyPiece = _blackRook.CanMove(0, 5);

        // Assert
        whiteCanMoveThroughEnemyPiece.Should().BeFalse();
        blackCanMoveThroughEnemyPiece.Should().BeFalse();
    }
}
