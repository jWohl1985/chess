using Chess.Logic;
using FluentAssertions;
using System.Collections.Generic;
using Xunit.Sdk;

namespace Chess.Tests;

public class KingTests
{
    private readonly GameBoard _board;
    private readonly King _whiteKing;
    private readonly King _blackKing;

    public KingTests()
    {
        _board = new GameBoard();
        _whiteKing = _board.WhiteKing;
        _blackKing = _board.BlackKing;
    }

    [Fact]
    public void Should_Not_Be_Able_To_Move_Off_Board()
    {
        // Arrange
        _board.State[0, 0] = _whiteKing;

        // Act
        bool canMoveTooFarLeft = _whiteKing.CanMove(0, -1);
        bool canMoveTooFarDown = _whiteKing.CanMove(-1, 0);

        // Arrange
        _board.State[0, 0] = null;
        _board.State[7, 7] = _whiteKing;

        // Act
        bool canMoveTooFarRight = _whiteKing.CanMove(0, 8);
        bool canMoveTooFarUp = _whiteKing.CanMove(8, 7);

        // Assert
        canMoveTooFarLeft.Should().BeFalse();
        canMoveTooFarRight.Should().BeFalse();
        canMoveTooFarUp.Should().BeFalse();
        canMoveTooFarDown.Should().BeFalse();
    }

    [Fact]
    public void Should_Only_Move_One_Square_In_Any_Direction()
    {
        // Arrange
        _board.State[3, 3] = _whiteKing;

        // Act
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                bool canMoveToSquare = _whiteKing.CanMove(i, j);

                if (Math.Abs(i - _whiteKing.Rank) == 0 && Math.Abs(j - _whiteKing.Rank) == 0)
                {
                    canMoveToSquare.Should().BeFalse(); // 'moving' to the same square
                }
                else if (Math.Abs(i - _whiteKing.Rank) <= 1 && Math.Abs(j - _whiteKing.File) <= 1)
                {
                    canMoveToSquare.Should().BeTrue(); // moving 1 square in any direction
                }
                else
                {
                    canMoveToSquare.Should().BeFalse(); // moving more than 1 square
                }
            }
        }
    }

    [Fact]
    public void Should_Not_Move_To_The_Same_Square()
    {
        // Arrange
        _board.State[3, 3] = _whiteKing;

        // Act
        bool canMoveToSameSquare = _whiteKing.CanMove(3, 3);

        // Assert
        canMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteKing;
        _board.State[4, 4] = new Pawn() { Board = _board, Color = PieceColor.White };

        // Act
        bool canMoveOntoOwnPiece = _whiteKing.CanMove(4, 4);

        // Assert
        canMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Capture_Enemy_Pieces()
    {
        // Arrange
        _board.State[3, 3] = _whiteKing;
        _board.State[2, 2] = new Knight() { Board = _board, Color = PieceColor.Black };

        // Assert
        bool canCaptureEnemyPiece = _whiteKing.CanMove(2, 2);

        // Assert
        canCaptureEnemyPiece.Should().BeTrue();
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_Pawn()
    {
        // Arrange
        _board.State[3, 3] = _whiteKing;
        Pawn enemyPawn = new Pawn() { Board = _board, Color = PieceColor.Black };

        List<(int, int)> checkPositions = new List<(int, int)>()
        {
            (4, 2),
            (4, 4),
        };

        // Act
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                _board.ClearBoard();
                _board.State[i, j] = enemyPawn;
                _board.State[3, 3] = _whiteKing;

                // Assert
                if (checkPositions.Contains((i, j)))
                    _whiteKing.IsInCheck.Should().BeTrue();
                else
                    _whiteKing.IsInCheck.Should().BeFalse();
            }
        }
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_Knight()
    {
        // Arrange
        _board.State[3, 3] = _whiteKing;
        Knight enemyKnight = new Knight() { Board = _board, Color = PieceColor.Black };

        List<(int, int)> checkPositions = new List<(int, int)>()
        {
            (5, 2),
            (5, 4),
            (4, 1),
            (4, 5),
            (2, 1),
            (2, 5),
            (1, 2),
            (1, 4),
        };

        // Act
        for(int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <=7; j++)
            {
                _board.ClearBoard();
                _board.State[i, j] = enemyKnight;
                _board.State[3, 3] = _whiteKing;

                // Assert
                if (checkPositions.Contains((i, j)))
                    _whiteKing.IsInCheck.Should().BeTrue();
                else
                    _whiteKing.IsInCheck.Should().BeFalse();
            }
        }
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_Bishop()
    {
        // Arrange
        _board.State[3, 3] = _whiteKing;
        Bishop enemyBishop = new Bishop() { Board = _board, Color = PieceColor.Black };

        List<(int, int)> checkPositions = new List<(int, int)>()
        {
            (4, 4), (5, 5), (6, 6), (7, 7), // up and to the right
            (4, 2), (5, 1), (6, 0), // up and to the left
            (2, 4), (1, 5), (0, 6), // down and to the right
            (2, 2), (1, 1), (0, 0), // down and to the left
        };

        // Act
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                _board.ClearBoard();
                _board.State[i, j] = enemyBishop;
                _board.State[3, 3] = _whiteKing;

                // Assert
                if (checkPositions.Contains((i, j)))
                    _whiteKing.IsInCheck.Should().BeTrue();
                else
                    _whiteKing.IsInCheck.Should().BeFalse();
            }
        }
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_Rook()
    {
        // Arrange
        _board.State[3, 3] = _whiteKing;
        Rook enemyRook = new Rook() { Board = _board, Color = PieceColor.Black };

        List<(int, int)> checkPositions = new List<(int, int)>()
        {
            (3, 4), (3,5), (3,6), (3,7), // to the right
            (3, 2), (3, 1), (3, 0), // to the left
            (4, 3), (5, 3), (6, 3), (7, 3), // up
            (2, 3), (1, 3), (0, 3) // down
        };

        // Act
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                _board.ClearBoard();
                _board.State[i, j] = enemyRook;
                _board.State[3, 3] = _whiteKing;

                // Assert
                if (checkPositions.Contains((i, j)))
                    _whiteKing.IsInCheck.Should().BeTrue();
                else
                    _whiteKing.IsInCheck.Should().BeFalse();
            }
        }
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_Queen()
    {
        // Arrange
        _board.State[3, 3] = _whiteKing;
        Queen enemyQueen = new Queen() { Board = _board, Color = PieceColor.Black };

        List<(int, int)> checkPositions = new List<(int, int)>()
        {
            (3, 4), (3,5), (3,6), (3,7), // to the right
            (3, 2), (3, 1), (3, 0), // to the left
            (4, 3), (5, 3), (6, 3), (7, 3), // up
            (2, 3), (1, 3), (0, 3), // down
            (4, 4), (5, 5), (6, 6), (7, 7), // up and to the right
            (4, 2), (5, 1), (6, 0), // up and to the left
            (2, 4), (1, 5), (0, 6), // down and to the right
            (2, 2), (1, 1), (0, 0), // down and to the left
        };

        // Act
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                _board.ClearBoard();
                _board.State[i, j] = enemyQueen;
                _board.State[3, 3] = _whiteKing;

                // Assert
                if (checkPositions.Contains((i, j)))
                    _whiteKing.IsInCheck.Should().BeTrue();
                else
                    _whiteKing.IsInCheck.Should().BeFalse();
            }
        }
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_King()
    {
        // Although this can not actually happen in the game,
        // the board will check for this condition
        // when deciding if a move is legal

        // Arrange
        _board.State[3, 3] = _whiteKing;

        List<(int, int)> checkPositions = new List<(int, int)>()
        {
            (4, 3), (4, 4), (3, 4), (2, 4), (2, 3), (2, 2), (3, 2), (4, 2)
        };

        // Act
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                _board.ClearBoard();
                _board.State[i, j] = _blackKing;
                _board.State[3, 3] = _whiteKing;

                // Assert
                if (checkPositions.Contains((i, j)))
                    _whiteKing.IsInCheck.Should().BeTrue();
                else
                    _whiteKing.IsInCheck.Should().BeFalse();
            }
        }
    }
}
