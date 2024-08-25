using Chess.Logic;
using FluentAssertions;
using static Chess.Logic.GameBoard;

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
        _board.State[RANK_2, FILE_C] = _whitePawn;
        _board.State[RANK_7, FILE_D] = _blackPawn;
        Pawn movedWhitePawn = new() { Board = _board, Color = PieceColor.White };
        Pawn movedBlackPawn = new() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_4, FILE_F] = movedWhitePawn;
        _board.State[RANK_6, FILE_G] = movedBlackPawn;

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
        _board.State[RANK_8, FILE_D] = _whitePawn;
        _board.State[RANK_1, FILE_B] = _blackPawn;

        // Act
        bool canMoveTooFarUp = _whitePawn.CanMove(8, FILE_D);
        bool canMoveTooFarDown = _whitePawn.CanMove(-1, FILE_B);

        // Assert
        canMoveTooFarDown.Should().BeFalse();
        canMoveTooFarUp.Should().BeFalse();
    }

    [Fact]
    public void Should_Only_Move_One_Square_Forward_After_Moving()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whitePawn;
        _board.State[RANK_6, FILE_B] = _blackPawn;

        // Act
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                // Assert
                _whitePawn.CanMove(i, j).Should().Be(i == _whitePawn.CurrentRank + 1 && j == _whitePawn.CurrentFile);
                _blackPawn.CanMove(i, j).Should().Be(i == _blackPawn.CurrentRank - 1 && j == _blackPawn.CurrentFile);
            }
        }
    }

    [Fact]
    public void Should_Not_Move_To_The_Same_Square()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whitePawn;
        _board.State[RANK_5, FILE_E] = _blackPawn;

        // Act
        bool whitePawnCanMoveToSameSquare = _whitePawn.CanMove(RANK_4, FILE_D);
        bool blackPawnCanMoveToSameSquare = _blackPawn.CanMove(RANK_5, FILE_E);

        // Assert
        whitePawnCanMoveToSameSquare.Should().BeFalse();
        blackPawnCanMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whitePawn;
        _board.State[RANK_5, FILE_D] = new Queen() { Board = _board, Color = PieceColor.White };

        _board.State[RANK_6, FILE_F] = _blackPawn;
        _board.State[RANK_5, FILE_F] = new Queen() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whitePawnCanMoveOntoOwnPiece = _whitePawn.CanMove(RANK_5, FILE_D);
        bool blackPawnCanMoveOntoOwnPiece = _blackPawn.CanMove(RANK_5, FILE_F);

        // Assert
        whitePawnCanMoveOntoOwnPiece.Should().BeFalse();
        blackPawnCanMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Be_Able_To_Move_Two_Squares_On_First_Move()
    {
        // Arrange
        _board.State[RANK_2, FILE_D] = _whitePawn;
        _board.State[RANK_7, FILE_F] = _blackPawn;

        // Act
        bool whiteCanMoveTwoSquares = _whitePawn.CanMove(RANK_4, FILE_D);
        bool blackCanMoveTwoSquares = _blackPawn.CanMove(RANK_5, FILE_F);

        // Assert
        whiteCanMoveTwoSquares.Should().BeTrue();
        blackCanMoveTwoSquares.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Move_Through_Friendly_Pieces()
    {
        // Arrange
        _board.State[RANK_2, FILE_D] = _whitePawn;
        _board.State[RANK_3, FILE_D] = new Knight() { Board = _board, Color = PieceColor.White };

        _board.State[RANK_7, FILE_F] = _blackPawn;
        _board.State[RANK_6, FILE_F] = new Bishop() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveThroughOwnPiece = _whitePawn.CanMove(RANK_4, FILE_D);
        bool blackCanMoveThroughOwnPiece = _blackPawn.CanMove(RANK_5, FILE_F);

        // Assert
        whiteCanMoveThroughOwnPiece.Should().BeFalse();
        blackCanMoveThroughOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Through_Enemy_Pieces()
    {
        // Arrange
        _board.State[RANK_2, FILE_D] = _whitePawn;
        _board.State[RANK_3, FILE_D] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[RANK_7, FILE_F] = _blackPawn;
        _board.State[RANK_6, FILE_F] = new Bishop() { Board = _board, Color = PieceColor.White };

        // Act
        bool whiteCanMoveThroughOwnPiece = _whitePawn.CanMove(RANK_4, FILE_D);
        bool blackCanMoveThroughOwnPiece = _blackPawn.CanMove(RANK_5, FILE_F);

        // Assert
        whiteCanMoveThroughOwnPiece.Should().BeFalse();
        blackCanMoveThroughOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Capture_Enemy_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_B] = _whitePawn;
        _board.State[RANK_5, FILE_A] = new Knight() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_5, FILE_C] = new Bishop() { Board = _board, Color = PieceColor.Black };

        _board.State[RANK_6, FILE_F] = _blackPawn;
        _board.State[RANK_5, FILE_E] = new Knight() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_5, FILE_G] = new Bishop() { Board = _board, Color = PieceColor.White };

        // Assert
        bool whiteCanCaptureLeft = _whitePawn.CanMove(RANK_5, FILE_A);
        bool whiteCanCaptureRight = _whitePawn.CanMove(RANK_5, FILE_C);
        bool blackCanCaptureLeft = _blackPawn.CanMove(RANK_5, FILE_E);
        bool blackCanCaptureRight = _blackPawn.CanMove(RANK_5, FILE_G);

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
        _board.State[RANK_4, FILE_B] = _whitePawn;
        _board.State[RANK_5, FILE_A] = new Knight() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_5, FILE_C] = new Bishop() { Board = _board, Color = PieceColor.White };

        _board.State[RANK_6, FILE_F] = _blackPawn;
        _board.State[RANK_5, FILE_E] = new Knight() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_5, FILE_G] = new Bishop() { Board = _board, Color = PieceColor.Black };


        // Assert
        bool whiteCanCaptureLeft = _whitePawn.CanMove(RANK_5, FILE_A);
        bool whiteCanCaptureRight = _whitePawn.CanMove(RANK_5, FILE_C);
        bool blackCanCaptureLeft = _blackPawn.CanMove(RANK_5, FILE_E);
        bool blackCanCaptureRight = _blackPawn.CanMove(RANK_5, FILE_G);

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
        _board.State[RANK_2, FILE_B] = _whitePawn;
        _board.State[RANK_3, FILE_B] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[RANK_7, FILE_F] = _blackPawn;
        _board.State[RANK_6, FILE_F] = new Knight() { Board = _board, Color = PieceColor.White };


        // Assert
        bool whiteCanCaptureForward = _whitePawn.CanMove(RANK_3, FILE_B);
        bool blackCanCaptureForward = _blackPawn.CanMove(RANK_6, FILE_F);

        // Assert
        whiteCanCaptureForward.Should().BeFalse();
        blackCanCaptureForward.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Capture_Forward_Two_Squares()
    {
        // Arrange
        _board.State[RANK_2, FILE_B] = _whitePawn;
        _board.State[RANK_4, FILE_B] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[RANK_7, FILE_F] = _blackPawn;
        _board.State[RANK_5, FILE_F] = new Knight() { Board = _board, Color = PieceColor.White };


        // Assert
        bool whiteCanCaptureForward = _whitePawn.CanMove(RANK_4, FILE_B);
        bool blackCanCaptureForward = _blackPawn.CanMove(RANK_5, FILE_F);

        // Assert
        whiteCanCaptureForward.Should().BeFalse();
        blackCanCaptureForward.Should().BeFalse();
    }
}