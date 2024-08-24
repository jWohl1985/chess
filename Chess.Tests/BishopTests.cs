using Chess.Logic;
using FluentAssertions;
using static Chess.Logic.GameBoard;

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
        _board.State[RANK_4, FILE_D] = _whiteBishop;

        // Act
        bool canMoveTooFarUpRight = _whiteBishop.CanMove(_whiteBishop.CurrentRank + 5, _whiteBishop.CurrentFile + 5);
        bool canMoveTooFarDownRight = _whiteBishop.CanMove(_whiteBishop.CurrentRank - 4, _whiteBishop.CurrentFile + 4);
        bool canMoveTooFarDownLeft = _whiteBishop.CanMove(_whiteBishop.CurrentRank - 4, _whiteBishop.CurrentFile - 4);
        bool canMoveTooFarUpLeft = _whiteBishop.CanMove(_whiteBishop.CurrentRank + 5, _whiteBishop.CurrentFile - 4);

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
        _board.State[RANK_4, FILE_D] = _whiteBishop;
        List<(int, int)> validMoves = new()
        {
            (RANK_1, FILE_A), (RANK_2, FILE_B), (RANK_3, FILE_C), // down-left
            (RANK_5, FILE_C), (RANK_6, FILE_B), (RANK_7, FILE_A), // up-left
            (RANK_5, FILE_E), (RANK_6, FILE_F), (RANK_7, FILE_G), (RANK_8, FILE_H), // up-right
            (RANK_3, FILE_E), (RANK_2, FILE_F), (RANK_1, FILE_G) // down-right
        };

        // Act
        for (int i = RANK_1; i <= RANK_8; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                bool canMoveToSquare = _whiteBishop.CanMove(i, j);

                // Assert
                if (validMoves.Contains((i, j)))
                {
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
        _board.State[RANK_4, FILE_D] = _whiteBishop;

        // Act
        bool canMoveToSameSquare = _whiteBishop.CanMove(RANK_4, FILE_D);

        // Assert
        canMoveToSameSquare.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Onto_Friendly_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteBishop;
        _board.State[RANK_5, FILE_E] = new Queen() { Board = _board, Color = PieceColor.White };

        _board.State[RANK_7, FILE_F] = _blackBishop;
        _board.State[RANK_8, FILE_H] = new Queen() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveOntoOwnPiece = _whiteBishop.CanMove(RANK_5, FILE_E);
        bool blackCanMoveOntoOwnPiece = _blackBishop.CanMove(RANK_8, FILE_H);

        // Assert
        whiteCanMoveOntoOwnPiece.Should().BeFalse();
        blackCanMoveOntoOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_Through_Friendly_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteBishop;
        _board.State[RANK_6, FILE_F] = new Bishop() { Board = _board, Color = PieceColor.White };

        _board.State[RANK_1, FILE_A] = _blackBishop;
        _board.State[RANK_2, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.Black };

        // Act
        bool whiteCanMoveThroughOwnPiece = _whiteBishop.CanMove(RANK_8, FILE_H);
        bool blackCanMoveThroughOwnPiece = _blackBishop.CanMove(RANK_3, FILE_C);

        // Assert
        whiteCanMoveThroughOwnPiece.Should().BeFalse();
        blackCanMoveThroughOwnPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Capture_Enemy_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteBishop;
        _board.State[RANK_2, FILE_B] = new Knight() { Board = _board, Color = PieceColor.Black };

        _board.State[RANK_5, FILE_E] = _blackBishop;
        _board.State[RANK_7, FILE_G] = new Knight() { Board = _board, Color = PieceColor.White };

        // Assert
        bool whiteCanCaptureEnemyPiece = _whiteBishop.CanMove(RANK_2, FILE_B);
        bool blackCanCaptureEnemyPiece = _blackBishop.CanMove(RANK_7, FILE_G);

        // Assert
        whiteCanCaptureEnemyPiece.Should().BeTrue();
        blackCanCaptureEnemyPiece.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Move_Through_Enemy_Pieces()
    {
        // Arrange
        _board.State[RANK_4, FILE_D] = _whiteBishop;
        _board.State[RANK_5, FILE_E] = new Bishop() { Board = _board, Color = PieceColor.Black };

        _board.State[RANK_1, FILE_A] = _blackBishop;
        _board.State[RANK_2, FILE_B] = new Bishop() { Board = _board, Color = PieceColor.White };

        // Act
        bool whiteCanMoveThroughEnemyPiece = _whiteBishop.CanMove(RANK_6, FILE_F);
        bool blackCanMoveThroughEnemyPiece = _blackBishop.CanMove(RANK_3, FILE_C);

        // Assert
        whiteCanMoveThroughEnemyPiece.Should().BeFalse();
        blackCanMoveThroughEnemyPiece.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Move_If_King_Would_Be_In_Check()
    {
        // Arrange
        // the kings are already on e1 and e8 when the board is newed
        _board.State[RANK_4, FILE_E] = _whiteBishop;
        _board.State[RANK_5, FILE_E] = new Rook() { Board = _board, Color = PieceColor.Black };
        _board.State[RANK_6, FILE_E] = new Rook() { Board = _board, Color = PieceColor.White };
        _board.State[RANK_7, FILE_E] = _blackBishop;

        // Act
        bool whiteCanPutOwnKingInCheck = _whiteBishop.CanMove(RANK_5, FILE_F);
        bool blackCanPutOwnKingInCheck = _blackBishop.CanMove(RANK_6, FILE_F);

        // Assert
        whiteCanPutOwnKingInCheck.Should().BeFalse();
        blackCanPutOwnKingInCheck.Should().BeFalse();
    }
}