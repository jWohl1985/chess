using Chess.Logic;
using FluentAssertions;
using static Chess.Logic.GameBoard;

namespace Chess.Tests;

public class QueenTests
{
    private readonly GameBoard _board;
    private readonly Queen _whiteQueen;

    public QueenTests()
    {
        _board = new GameBoard();
        _whiteQueen = new Queen() { Board = _board, Color = PieceColor.White };
    }

    [Fact]
    public void Should_Not_Be_Able_To_Move_Off_Board()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteQueen;

        // Act
        bool canMoveTooFarLeft = _whiteQueen.CanMove(RANK_4, -1);
        bool canMoveTooFarRight = _whiteQueen.CanMove(RANK_4, 8);
        bool canMoveTooFarUp = _whiteQueen.CanMove(8, FILE_D);
        bool canMoveTooFarDown = _whiteQueen.CanMove(-1, FILE_D);

        bool canMoveTooFarUpRight = _whiteQueen.CanMove(8, 8);
        bool canMoveTooFarDownRight = _whiteQueen.CanMove(-1, FILE_H);
        bool canMoveTooFarDownLeft = _whiteQueen.CanMove(-1, -1);
        bool canMoveTooFarUpLeft = _whiteQueen.CanMove(RANK_8, -1);

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
        _board.State[RANK_4, FILE_D] = _whiteQueen;

        // Act
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                if (_board.State[i, j] is not null)
                    continue;

                bool canMoveToSquare = _whiteQueen.CanMove(i, j);
                bool movingStraight = i == _whiteQueen.CurrentRank || j == _whiteQueen.CurrentFile;
                bool movingDiagonal = Math.Abs(i - _whiteQueen.CurrentRank) == Math.Abs(j - _whiteQueen.CurrentFile);

                // Assert
                canMoveToSquare.Should().Be(movingStraight || movingDiagonal);
            }
        }
    }

    [Fact]
    public void Should_Not_Move_To_The_Same_Square()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteQueen;

        // Act
        bool canMoveToSameSquare = _whiteQueen.CanMove(RANK_4, FILE_D);

        // Assert
        canMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteQueen;
        _board.State[RANK_6, FILE_F] = new Pawn() { Board = _board, Color = PieceColor.White };

        // Act
        bool canMoveOntoOwnPiece = _whiteQueen.CanMove(RANK_6, FILE_F);

        // Assert
        canMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Through_Friendly_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteQueen;
        _board.State[RANK_5, FILE_C] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_5, FILE_D] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_5, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_D] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_C] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_C] = new Pawn() { Board = _board, Color = PieceColor.White };

        // Act
        bool canMoveThroughOwnPiece1 = _whiteQueen.CanMove(RANK_6, FILE_B);
        bool canMoveThroughOwnPiece2 = _whiteQueen.CanMove(RANK_6, FILE_D);
        bool canMoveThroughOwnPiece3 = _whiteQueen.CanMove(RANK_6, FILE_F);
        bool canMoveThroughOwnPiece4 = _whiteQueen.CanMove(RANK_4, FILE_F);
        bool canMoveThroughOwnPiece5 = _whiteQueen.CanMove(RANK_2, FILE_F);
        bool canMoveThroughOwnPiece6 = _whiteQueen.CanMove(RANK_2, FILE_D);
        bool canMoveThroughOwnPiece7 = _whiteQueen.CanMove(RANK_2, FILE_B);
        bool canMoveThroughOwnPiece8 = _whiteQueen.CanMove(RANK_4, FILE_B);

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
        _board.State[RANK_4, FILE_D] = _whiteQueen;
        _board.State[RANK_1, FILE_A] = new Knight() { Board = _board, Color = PieceColor.Black };

        // Assert
        bool canCaptureEnemyPiece = _whiteQueen.CanMove(RANK_1, FILE_A);

        // Assert
        canCaptureEnemyPiece.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Move_Through_Enemy_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteQueen;
        _board.State[RANK_5, FILE_C] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_5, FILE_D] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_5, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_4, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_3, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_3, FILE_D] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_3, FILE_C] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_4, FILE_C] = new Pawn() { Board = _board, Color = PieceColor.Black };

        // Act
        bool canMoveThroughEnemyPiece1 = _whiteQueen.CanMove(RANK_6, FILE_B);
        bool canMoveThroughEnemyPiece2 = _whiteQueen.CanMove(RANK_6, FILE_D);
        bool canMoveThroughEnemyPiece3 = _whiteQueen.CanMove(RANK_6, FILE_F);
        bool canMoveThroughEnemyPiece4 = _whiteQueen.CanMove(RANK_4, FILE_F);
        bool canMoveThroughEnemyPiece5 = _whiteQueen.CanMove(RANK_2, FILE_F);
        bool canMoveThroughEnemyPiece6 = _whiteQueen.CanMove(RANK_2, FILE_D);
        bool canMoveThroughEnemyPiece7 = _whiteQueen.CanMove(RANK_2, FILE_B);
        bool canMoveThroughEnemyPiece8 = _whiteQueen.CanMove(RANK_4, FILE_B);

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