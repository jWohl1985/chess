using Chess.Logic;
using FluentAssertions;

namespace Chess.Tests;

public class PawnTests
{
    private readonly GameBoard _board;
    private readonly Pawn _whitePawn;
    private readonly Pawn _blackPawn;

    public PawnTests()
    {
        _board = new GameBoard();
        _whitePawn = new Pawn() { Board = _board, Color = PieceColor.White };
        _blackPawn = new Pawn() { Board = _board, Color = PieceColor.Black };
    }

    [Fact]
    public void Has_Moved_Should_Only_Be_True_When_Pawn_Is_Not_On_Starting_Rank()
    {
        // Arrange
        _board.State[1, 2] = _whitePawn;
        _board.State[6, 3] = _blackPawn;
        Pawn movedWhitePawn = new Pawn() { Board = _board, Color = PieceColor.White };
        Pawn movedBlackPawn = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[3, 5] = movedWhitePawn;
        _board.State[5, 7] = movedBlackPawn;

        // Act

        // Assert
        _whitePawn.HasMoved.Should().BeFalse();
        _blackPawn.HasMoved.Should().BeFalse();
        movedWhitePawn.HasMoved.Should().BeTrue();
        movedBlackPawn.HasMoved.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Be_Able_To_Move_Off_Board()
    {
        // Arrange
        _board.State[7, 3] = _whitePawn;
        _board.State[0, 1] = _blackPawn;

        // Act
        bool canMoveTooFarUp = _whitePawn.CanMove(8, 3);
        bool canMoveTooFarDown = _whitePawn.CanMove(-1, 1);

        // Assert
        canMoveTooFarDown.Should().BeFalse();
        canMoveTooFarUp.Should().BeFalse();
    }

    [Fact]
    public void Should_Only_Move_One_Square_Forward_After_Moving()
    {
        // Arrange
        _board.State[3, 3] = _whitePawn;
        _board.State[5, 1] = _blackPawn;

        // Act
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                bool whitePawnCanMove = _whitePawn.CanMove(i, j);
                bool blackPawnCanMove = _blackPawn.CanMove(i, j);

                if (i == _whitePawn.Rank && j == _whitePawn.File)
                    whitePawnCanMove.Should().BeFalse();
                else if (i == _whitePawn.Rank + 1 && j == _whitePawn.File)
                    whitePawnCanMove.Should().BeTrue();
                else
                    whitePawnCanMove.Should().BeFalse();

                if (i == _blackPawn.Rank && j == _blackPawn.File)
                    blackPawnCanMove.Should().BeFalse();
                else if (i == _blackPawn.Rank - 1 && j == _blackPawn.File)
                    blackPawnCanMove.Should().BeTrue();
                else
                    blackPawnCanMove.Should().BeFalse();
            }
        }
    }

    [Fact]
    public void Should_Not_Move_To_The_Same_Square()
    {
        // Arrange
        _board.State[3, 3] = _whitePawn;
        _board.State[4, 4] = _blackPawn;

        // Act
        bool whitePawnCanMoveToSameSquare = _whitePawn.CanMove(3, 3);
        bool blackPawnCanMoveToSameSquare = _blackPawn.CanMove(4, 4);

        // Assert
        whitePawnCanMoveToSameSquare.Should().BeFalse();
        blackPawnCanMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whitePawn;
        _board.State[4, 3] = new Queen() { Board = _board, Color = PieceColor.White };

        _board.State[5, 5] = _blackPawn;
        _board.State[4, 5] = new Queen() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whitePawnCanMoveOntoOwnPiece = _whitePawn.CanMove(4, 3);
        bool blackPawnCanMoveOntoOwnPiece = _blackPawn.CanMove(4, 5);

        // Assert
        whitePawnCanMoveOntoOwnPiece.Should().BeFalse();
        blackPawnCanMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Be_Able_To_Move_Two_Squares_On_First_Move()
    {
        // Arrange
        _board.State[1, 3] = _whitePawn;
        _board.State[6, 5] = _blackPawn;

        // Act
        bool whiteCanMoveTwoSquares = _whitePawn.CanMove(3, 3);
        bool blackCanMoveTwoSquares = _blackPawn.CanMove(4, 5);

        // Assert
        whiteCanMoveTwoSquares.Should().BeTrue();
        blackCanMoveTwoSquares.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Move_Through_Friendly_Pieces()
    {
        // Arrange
        _board.State[1, 3] = _whitePawn;
        _board.State[2, 3] = new Knight() { Board = _board, Color = PieceColor.White };

        _board.State[6, 5] = _blackPawn;
        _board.State[5, 5] = new Bishop() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveThroughOwnPiece = _whitePawn.CanMove(3, 3);
        bool blackCanMoveThroughOwnPiece = _blackPawn.CanMove(4, 5);

        // Assert
        whiteCanMoveThroughOwnPiece.Should().BeFalse();
        blackCanMoveThroughOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Through_Enemy_Pieces()
    {
        // Arrange
        _board.State[1, 3] = _whitePawn;
        _board.State[2, 3] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[6, 5] = _blackPawn;
        _board.State[5, 5] = new Bishop() { Board = _board, Color = PieceColor.White };

        // Act
        bool whiteCanMoveThroughOwnPiece = _whitePawn.CanMove(3, 3);
        bool blackCanMoveThroughOwnPiece = _blackPawn.CanMove(4, 5);

        // Assert
        whiteCanMoveThroughOwnPiece.Should().BeFalse();
        blackCanMoveThroughOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Capture_Enemy_Pieces()
    {
        // Arrange
        _board.State[3, 1] = _whitePawn;
        _board.State[4, 0] = new Knight() { Board = _board, Color = PieceColor.Black };
        _board.State[4, 2] = new Bishop() { Board = _board, Color = PieceColor.Black };

        _board.State[5, 5] = _blackPawn;
        _board.State[4, 4] = new Knight() { Board = _board, Color = PieceColor.White };
        _board.State[4, 6] = new Bishop() { Board = _board, Color = PieceColor.White };


        // Assert
        bool whiteCanCaptureLeft = _whitePawn.CanMove(4, 0);
        bool whiteCanCaptureRight = _whitePawn.CanMove(4, 2);
        bool blackCanCaptureLeft = _blackPawn.CanMove(4, 4);
        bool blackCanCaptureRight = _blackPawn.CanMove(4, 6);

        // Assert
        whiteCanCaptureLeft.Should().BeTrue();
        whiteCanCaptureRight.Should().BeTrue();
        blackCanCaptureLeft.Should().BeTrue();
        blackCanCaptureRight.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Capture_Own_Pieces()
    {
        // Arrange
        _board.State[3, 1] = _whitePawn;
        _board.State[4, 0] = new Knight() { Board = _board, Color = PieceColor.White };
        _board.State[4, 2] = new Bishop() { Board = _board, Color = PieceColor.White };

        _board.State[5, 5] = _blackPawn;
        _board.State[4, 4] = new Knight() { Board = _board, Color = PieceColor.Black };
        _board.State[4, 6] = new Bishop() { Board = _board, Color = PieceColor.Black };


        // Assert
        bool whiteCanCaptureLeft = _whitePawn.CanMove(4, 0);
        bool whiteCanCaptureRight = _whitePawn.CanMove(4, 2);
        bool blackCanCaptureLeft = _blackPawn.CanMove(4, 4);
        bool blackCanCaptureRight = _blackPawn.CanMove(4, 6);

        // Assert
        whiteCanCaptureLeft.Should().BeFalse();
        whiteCanCaptureRight.Should().BeFalse();
        blackCanCaptureLeft.Should().BeFalse();
        blackCanCaptureRight.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Capture_Forward_One_Square()
    {
        // Arrange
        _board.State[1, 1] = _whitePawn;
        _board.State[2, 1] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[6, 5] = _blackPawn;
        _board.State[5, 5] = new Knight() { Board = _board, Color = PieceColor.White };


        // Assert
        bool whiteCanCaptureForward = _whitePawn.CanMove(2, 1);
        bool blackCanCaptureForward = _blackPawn.CanMove(5, 5);

        // Assert
        whiteCanCaptureForward.Should().BeFalse();
        blackCanCaptureForward.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Capture_Forward_Two_Squares()
    {
        // Arrange
        _board.State[1, 1] = _whitePawn;
        _board.State[3, 1] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[6, 5] = _blackPawn;
        _board.State[4, 5] = new Knight() { Board = _board, Color = PieceColor.White };


        // Assert
        bool whiteCanCaptureForward = _whitePawn.CanMove(3, 1);
        bool blackCanCaptureForward = _blackPawn.CanMove(4, 5);

        // Assert
        whiteCanCaptureForward.Should().BeFalse();
        blackCanCaptureForward.Should().BeFalse();
    }
}
