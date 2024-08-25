using Chess.Logic;
using FluentAssertions;
using static Chess.Logic.GameBoard;

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
        _board.ClearBoard();
    }

    [Fact]
    public void Should_Not_Be_Able_To_Move_Off_Board()
    {
        // Arrange
        _board.State[RANK_1, FILE_A] = _whiteKing;
        _board.State[RANK_8, FILE_H] = _blackKing;

        // Act
        bool canMoveTooFarLeft = _whiteKing.CanMove(RANK_1, -1);
        bool canMoveTooFarDown = _whiteKing.CanMove(-1, FILE_A);
        bool canMoveTooFarRight = _blackKing.CanMove(RANK_8, 8);
        bool canMoveTooFarUp = _blackKing.CanMove(8, FILE_H);

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
        _board.State[RANK_4, FILE_D] = _whiteKing;
        _board.State[RANK_7, FILE_G] = _blackKing;

        List<(int, int)> validWhiteMoves = new()
        {
            (RANK_5, FILE_C), (RANK_5, FILE_D), (RANK_5, FILE_E),
            (RANK_4, FILE_C), (RANK_4, FILE_E),
            (RANK_3, FILE_C), (RANK_3, FILE_D), (RANK_3, FILE_E),
        };

        List<(int, int)> validBlackMoves = new()
        {
            (RANK_8, FILE_F), (RANK_8, FILE_G), (RANK_8, FILE_H),
            (RANK_7, FILE_F), (RANK_7, FILE_H),
            (RANK_6, FILE_F), (RANK_6, FILE_G), (RANK_6, FILE_H),
        };

        // Act
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                bool whiteCanMoveToSquare = _whiteKing.CanMove(i, j);
                bool blackCanMoveToSquare = _blackKing.CanMove(i, j);

                // Assert
                if (validWhiteMoves.Contains((i, j)))
                {
                    whiteCanMoveToSquare.Should().BeTrue();
                }
                else
                {
                    whiteCanMoveToSquare.Should().BeFalse();
                }

                if (validBlackMoves.Contains((i, j)))
                {
                    blackCanMoveToSquare.Should().BeTrue();
                }
                else
                {
                    blackCanMoveToSquare.Should().BeFalse();
                }
            }
        }
    }

    [Fact]
    public void Should_Not_Move_To_The_Same_Square()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKing;
        _board.State[RANK_6, FILE_F] = _blackKing;

        // Act
        bool whiteCanMoveToSameSquare = _whiteKing.CanMove(RANK_4, FILE_D);
        bool blackCanMoveToSameSquare = _blackKing.CanMove(RANK_6, FILE_F);

        // Assert
        whiteCanMoveToSameSquare.Should().BeFalse();
        blackCanMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKing;
        _board.State[RANK_5, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.White };

        _board.State[RANK_7, FILE_G] = _blackKing;
        _board.State[RANK_8, FILE_H] = new Rook() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveOntoOwnPiece = _whiteKing.CanMove(RANK_5, FILE_E);
        bool blackCanMoveOntoOwnPiece = _blackKing.CanMove(RANK_8, FILE_H);

        // Assert
        whiteCanMoveOntoOwnPiece.Should().BeFalse();
        blackCanMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Capture_Enemy_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKing;
        _board.State[RANK_3, FILE_C] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[RANK_7, FILE_G] = _blackKing;
        _board.State[RANK_8, FILE_H] = new Knight() { Board = _board, Color = PieceColor.White };

        // Assert
        bool whiteCanCaptureEnemyPiece = _whiteKing.CanMove(RANK_3, FILE_C);
        bool blackCanCaptureEnemyPiece = _blackKing.CanMove(RANK_8, FILE_H);

        // Assert
        whiteCanCaptureEnemyPiece.Should().BeTrue();
        blackCanCaptureEnemyPiece.Should().BeTrue();
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_Pawn()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKing;
        Pawn blackPawn = new() { Board = _board, Color = PieceColor.Black };
        List<(int, int)> blackPawnCheckPositions = [(RANK_5, FILE_C), (RANK_5, FILE_E)];

        _board.State[RANK_7, FILE_G] = _blackKing;
        Pawn whitePawn = new() { Board = _board, Color = PieceColor.White };
        List<(int, int)> whitePawnCheckPositions = [(RANK_6, FILE_F), (RANK_6, FILE_H)];

        // Act
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                if (_board.State[i, j] is not null)
                    continue;

                // Assert
                _board.State[i, j] = blackPawn;
                _whiteKing.IsInCheck.Should().Be(blackPawnCheckPositions.Contains((i, j)));
                _board.State[i, j] = whitePawn;
                _blackKing.IsInCheck.Should().Be(whitePawnCheckPositions.Contains((i, j)));

                _board.State[i, j] = null;
            }
        }
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_Knight()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKing;
        Knight blackKnight = new() { Board = _board, Color = PieceColor.Black };
        List<(int, int)> blackKnightCheckPositions = 
            [(RANK_6, FILE_C), (RANK_6, FILE_E),
            (RANK_5, FILE_B), (RANK_5, FILE_F),
            (RANK_3, FILE_B), (RANK_3, FILE_F),
            (RANK_2, FILE_C), (RANK_2, FILE_E)];

        _board.State[RANK_6, FILE_F] = _blackKing;
        Knight whiteKnight = new() { Board = _board, Color = PieceColor.White };
        List<(int, int)> whiteKnightCheckPositions =
            [(RANK_8, FILE_E), (RANK_8, FILE_G),
             (RANK_7, FILE_D), (RANK_7, FILE_H),
             (RANK_5, FILE_D), (RANK_5, FILE_H),
             (RANK_4, FILE_E), (RANK_4, FILE_G)];
        
        // Act
        for(int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                if (_board.State[i, j] is not null)
                    continue;

                // Assert
                _board.State[i, j] = blackKnight;
                _whiteKing.IsInCheck.Should().Be(blackKnightCheckPositions.Contains((i, j)));
                _board.State[i, j] = whiteKnight;
                _blackKing.IsInCheck.Should().Be(whiteKnightCheckPositions.Contains((i, j)));

                _board.State[i, j] = null;
            }
        }
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_Bishop()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKing;
        Bishop blackBishop = new() { Board = _board, Color = PieceColor.Black };
        List<(int, int)> blackBishopCheckPositions =
            [(RANK_5, FILE_E), (RANK_6, FILE_F), (RANK_7, FILE_G), (RANK_8, FILE_H),
            (RANK_5, FILE_C), (RANK_6, FILE_B), (RANK_7, FILE_A),
            (RANK_3, FILE_E), (RANK_2, FILE_F), (RANK_1, FILE_G),
            (RANK_3, FILE_C), (RANK_2, FILE_B), (RANK_1, FILE_A)];

        _board.State[RANK_4, FILE_F] = _blackKing;
        Bishop whiteBishop = new() { Board = _board, Color = PieceColor.White };
        List<(int, int)> whiteBishopCheckPositions =
            [(RANK_5, FILE_G), (RANK_6, FILE_H),
             (RANK_5, FILE_E), (RANK_6, FILE_D), (RANK_7, FILE_C), (RANK_8, FILE_B),
             (RANK_3, FILE_G), (RANK_2, FILE_H),
             (RANK_3, FILE_E), (RANK_2, FILE_D), (RANK_1, FILE_C)];

        // Act
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                if (_board.State[i, j] is not null)
                    continue;

                // Assert
                _board.State[i, j] = blackBishop;
                _whiteKing.IsInCheck.Should().Be(blackBishopCheckPositions.Contains((i, j)));
                _board.State[i, j] = whiteBishop;
                _blackKing.IsInCheck.Should().Be(whiteBishopCheckPositions.Contains((i, j)));

                _board.State[i, j] = null;
            }
        }
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_Rook()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKing;
        Rook blackRook = new() { Board = _board, Color = PieceColor.Black };
        List<(int, int)> blackRookCheckPositions =
            [(RANK_4, FILE_A), (RANK_4, FILE_B), (RANK_4, FILE_C), (RANK_4, FILE_E), (RANK_4, FILE_F), (RANK_4, FILE_G), (RANK_4, FILE_H),
             (RANK_1, FILE_D), (RANK_2, FILE_D), (RANK_3, FILE_D), (RANK_5, FILE_D), (RANK_6, FILE_D), (RANK_7, FILE_D), (RANK_8, FILE_D)];

        _board.State[RANK_6, FILE_F] = _blackKing;
        Rook whiteRook = new() { Board = _board, Color = PieceColor.White };
        List<(int, int)> whiteRookCheckPositions =
            [(RANK_6, FILE_A), (RANK_6, FILE_B), (RANK_6, FILE_C), (RANK_6, FILE_D), (RANK_6, FILE_E), (RANK_6, FILE_G), (RANK_6, FILE_H),
             (RANK_1, FILE_F), (RANK_2, FILE_F), (RANK_3, FILE_F), (RANK_4, FILE_F), (RANK_5, FILE_F), (RANK_7, FILE_F), (RANK_8, FILE_F)];

        // Act
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_B; j++)
            {
                if (_board.State[i, j] is not null)
                    continue;

                // Assert
                _board.State[i, j] = blackRook;
                _whiteKing.IsInCheck.Should().Be(blackRookCheckPositions.Contains((i, j)));
                _board.State[i, j] = whiteRook;
                _blackKing.IsInCheck.Should().Be(whiteRookCheckPositions.Contains((i, j)));

                _board.State[i, j] = null;
            }
        }
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_Queen()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKing;
        Queen blackQueen = new() { Board = _board, Color = PieceColor.Black };
        List<(int, int)> blackQueenCheckPositions =
            [(RANK_1, FILE_A), (RANK_2, FILE_B), (RANK_3, FILE_C), (RANK_5, FILE_E), (RANK_6, FILE_F), (RANK_7, FILE_G), (RANK_8, FILE_H),
             (RANK_7, FILE_A), (RANK_6, FILE_B), (RANK_5, FILE_C), (RANK_3, FILE_E), (RANK_2, FILE_F), (RANK_1, FILE_G),
             (RANK_4, FILE_A), (RANK_4, FILE_B), (RANK_4, FILE_C), (RANK_4, FILE_E), (RANK_4, FILE_F), (RANK_4, FILE_H),
             (RANK_1, FILE_D), (RANK_2, FILE_D), (RANK_3, FILE_D), (RANK_5, FILE_D), (RANK_6, FILE_D), (RANK_7, FILE_D), (RANK_8, FILE_D)];

        _board.State[RANK_6, FILE_C] = _blackKing;
        Queen whiteQueen = new() { Board = _board, Color = PieceColor.White };
        List<(int, int)> whiteQueenCheckPositions =
            [(RANK_4, FILE_A), (RANK_5, FILE_B), (RANK_7, FILE_D), (RANK_8, FILE_E),
             (RANK_8, FILE_A), (RANK_7, FILE_B), (RANK_5, FILE_D), (RANK_4, FILE_E), (RANK_3, FILE_F), (RANK_2, FILE_G), (RANK_1, FILE_H),
             (RANK_6, FILE_A), (RANK_6, FILE_B), (RANK_6, FILE_D), (RANK_6, FILE_E), (RANK_6, FILE_F), (RANK_6, FILE_G), (RANK_6, FILE_H),
             (RANK_1, FILE_C), (RANK_2, FILE_C), (RANK_3, FILE_C), (RANK_4, FILE_C), (RANK_5, FILE_C), (RANK_7, FILE_C), (RANK_8, FILE_C)];

        // Act
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_B; j++)
            {
                if (_board.State[i, j] is not null)
                    continue;

                // Assert
                _board.State[i, j] = blackQueen;
                _whiteKing.IsInCheck.Should().Be(blackQueenCheckPositions.Contains((i, j)));
                _board.State[i, j] = whiteQueen;
                _blackKing.IsInCheck.Should().Be(whiteQueenCheckPositions.Contains((i, j)));

                _board.State[i, j] = null;
            }
        }
    }

    [Fact]
    public void Should_Be_In_Check_When_Attacked_By_Enemy_King()
    {
        /* This can't happen in the game, but the board will check
         * for this condition when determining if a move is legal */

        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKing;

        List<(int, int)> blackKingCheckPositions =
            [(RANK_5, FILE_C), (RANK_5, FILE_D), (RANK_5, FILE_E),
             (RANK_4, FILE_C), (RANK_4, FILE_E),
             (RANK_3, FILE_C), (RANK_3, FILE_D), (RANK_3, FILE_E)];

        // Act
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                if (_board.State[i, j] is not null)
                    continue;

                // Assert
                _board.State[i, j] = _blackKing;
                _whiteKing.IsInCheck.Should().Be(blackKingCheckPositions.Contains((i, j)));
                _blackKing.IsInCheck.Should().Be(blackKingCheckPositions.Contains((i, j)));

                _board.State[i, j] = null;
            }
        }
    }

    [Fact]
    public void White_King_Should_Not_Be_Checked_Through_White_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKing;

        // Pieces around the king to block checks
        _board.State[RANK_5, FILE_C] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_5, FILE_D] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_5, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_C] = new Knight() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_E] = new Knight() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_C] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_D] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_E] = new Bishop() { Board = _board, Color = PieceColor.White };

        // Enemy pieces that would check the king if the blockers weren't in place
        _board.State[RANK_6, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_7, FILE_D] = new Queen() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_6, FILE_F] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_4, FILE_A] = new Rook() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_4, FILE_H] = new Rook() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_1, FILE_D] = new Queen() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_2, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_2, FILE_F] = new Queen() { Board = _board, Color = PieceColor.Black };

        // Act
        bool inCheck = _whiteKing.IsInCheck;

        // Assert
        inCheck.Should().BeFalse();
    }

    [Fact]
    public void White_King_Should_Not_Be_Checked_Through_Black_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteKing;

        // Pieces around the king to block checks
        _board.State[RANK_5, FILE_C] = new Knight() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_5, FILE_D] = new Knight() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_5, FILE_E] = new Knight() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_C] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_C] = new Rook() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_D] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_E] = new Rook() { Board = _board, Color = PieceColor.White };

        // Enemy pieces that would check the king if the blockers weren't in place
        _board.State[RANK_6, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_7, FILE_D] = new Queen() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_6, FILE_F] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_4, FILE_A] = new Rook() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_4, FILE_H] = new Rook() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_1, FILE_D] = new Queen() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_2, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_2, FILE_F] = new Queen() { Board = _board, Color = PieceColor.Black };

        // Act
        bool inCheck = _whiteKing.IsInCheck;

        // Assert
        inCheck.Should().BeFalse();
    }

    [Fact]
    public void Black_King_Should_Not_Be_Checked_Through_Black_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _blackKing;

        // Pieces around the king to block checks
        _board.State[RANK_5, FILE_C] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_5, FILE_D] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_5, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_4, FILE_C] = new Knight() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_4, FILE_E] = new Knight() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_3, FILE_C] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_3, FILE_D] = new Bishop() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_3, FILE_E] = new Bishop() { Board = _board, Color = PieceColor.Black };

        // Enemy pieces that would check the king if the blockers weren't in place
        _board.State[RANK_6, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_7, FILE_D] = new Queen() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_6, FILE_F] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_A] = new Rook() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_H] = new Rook() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_1, FILE_D] = new Queen() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_2, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_2, FILE_F] = new Queen() { Board = _board, Color = PieceColor.White };

        // Act
        bool inCheck = _blackKing.IsInCheck;

        // Assert
        inCheck.Should().BeFalse();
    }

    [Fact]
    public void Black_King_Should_Not_Be_Checked_Through_White_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _blackKing;

        // Pieces around the king to block checks
        _board.State[RANK_5, FILE_C] = new Rook() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_5, FILE_D] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_5, FILE_E] = new Rook() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_C] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_E] = new Pawn() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_C] = new Knight() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_D] = new Knight() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_3, FILE_E] = new Knight() { Board = _board, Color = PieceColor.White };

        // Enemy pieces that would check the king if the blockers weren't in place
        _board.State[RANK_6, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_7, FILE_D] = new Queen() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_6, FILE_F] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_A] = new Rook() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_4, FILE_H] = new Rook() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_1, FILE_D] = new Queen() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_2, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_2, FILE_F] = new Queen() { Board = _board, Color = PieceColor.White };

        // Act
        bool inCheck = _blackKing.IsInCheck;

        // Assert
        inCheck.Should().BeFalse();
    }
}