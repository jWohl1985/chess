using Chess.Logic;
using FluentAssertions;
using static Chess.Logic.GameBoard;

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
        _board.State[RANK_4, FILE_D] = _whiteRook;

        // Act
        bool canMoveTooFarLeft = _whiteRook.CanMove(_whiteRook.CurrentRank, _whiteRook.CurrentFile - 4);
        bool canMoveTooFarRight = _whiteRook.CanMove(_whiteRook.CurrentRank, _whiteRook.CurrentFile + 5);
        bool canMoveTooFarDown = _whiteRook.CanMove(_whiteRook.CurrentRank - 4, _whiteRook.CurrentFile);
        bool canMoveTooFarUp = _whiteRook.CanMove(_whiteRook.CurrentRank + 5, _whiteRook.CurrentFile);

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
        _board.State[RANK_4, FILE_D] = _whiteRook;

        // Act
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_B; j++)
            {
                if (_board.State[i, j] is not null)
                    continue;

                bool canMoveToSquare = _whiteRook.CanMove(i, j);
                bool movingStraight = i == _whiteRook.CurrentRank || j == _whiteRook.CurrentFile;

                // Assert
                canMoveToSquare.Should().Be(movingStraight);
            }
        }
    }

    [Fact]
    public void Should_Not_Move_To_The_Same_Square()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteRook;

        // Act
        bool canMoveToSameSquare = _whiteRook.CanMove(RANK_4, FILE_D);

        // Assert
        canMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteRook;
        _board.State[RANK_4, FILE_E] = new Queen() { Board = _board, Color = PieceColor.White };

        _board.State[RANK_6, FILE_F] = _blackRook;
        _board.State[RANK_6, FILE_B] = new Queen() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveOntoOwnPiece = _whiteRook.CanMove(RANK_4, FILE_E);
        bool blackCanMoveOntoOwnPiece = _blackRook.CanMove(RANK_6, FILE_B);

        // Assert
        whiteCanMoveOntoOwnPiece.Should().BeFalse();
        blackCanMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Through_Friendly_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteRook;
        _board.State[RANK_6, FILE_D] = new Bishop() { Board = _board, Color = PieceColor.White };

        _board.State[RANK_6, FILE_G] = _blackRook;
        _board.State[RANK_3, FILE_G] = new Bishop() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveThroughOwnPiece = _whiteRook.CanMove(RANK_7, FILE_D);
        bool blackCanMoveThroughOwnPiece = _blackRook.CanMove(RANK_1, FILE_G);

        // Assert
        whiteCanMoveThroughOwnPiece.Should().BeFalse();
        blackCanMoveThroughOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Capture_Enemy_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteRook;
        _board.State[RANK_2, FILE_D] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[RANK_6, FILE_F] = _blackRook;
        _board.State[RANK_6, FILE_H] = new Knight() { Board = _board, Color = PieceColor.White };

        // Assert
        bool whiteCanCaptureEnemyPiece = _whiteRook.CanMove(RANK_2, FILE_D);
        bool blackCanCaptureEnemyPiece = _blackRook.CanMove(RANK_6, FILE_H);

        // Assert
        whiteCanCaptureEnemyPiece.Should().BeTrue();
        blackCanCaptureEnemyPiece.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Move_Through_Enemy_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteRook;
        _board.State[RANK_4, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.Black };

        _board.State[RANK_6, FILE_F] = _blackRook;
        _board.State[RANK_4, FILE_F] = new Bishop() { Board = _board, Color = PieceColor.White };

        // Act
        bool whiteCanMoveThroughEnemyPiece = _whiteRook.CanMove(RANK_4, FILE_A);
        bool blackCanMoveThroughEnemyPiece = _blackRook.CanMove(RANK_1, FILE_F);

        // Assert
        whiteCanMoveThroughEnemyPiece.Should().BeFalse();
        blackCanMoveThroughEnemyPiece.Should().BeFalse();
    }
}