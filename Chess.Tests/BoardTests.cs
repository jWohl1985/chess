using Chess.Logic;
using FluentAssertions;
using static Chess.Logic.GameBoard;

namespace Chess.Tests;

public class BoardTests
{
    [Fact]
    public void Board_Constants_Should_Be_Correct_Values()
    {
        // Arrange

        // Act

        // Assert
        BOARD_WIDTH.Should().Be(8);
        BOARD_HEIGHT.Should().Be(8);

        RANK_1.Should().Be(0);
        RANK_2.Should().Be(1);
        RANK_3.Should().Be(2);
        RANK_4.Should().Be(3);
        RANK_5.Should().Be(4);
        RANK_6.Should().Be(5);
        RANK_7.Should().Be(6);
        RANK_8.Should().Be(7);

        FILE_A.Should().Be(0);
        FILE_B.Should().Be(1);
        FILE_C.Should().Be(2);
        FILE_D.Should().Be(3);
        FILE_E.Should().Be(4);
        FILE_F.Should().Be(5);
        FILE_G.Should().Be(6);
        FILE_H.Should().Be(7);
    }

    [Fact]
    public void Board_Should_Be_Correct_Size()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        // Assert
        board.State.GetLength(0).Should().Be(8);
        board.State.GetLength(1).Should().Be(8);
    }

    [Fact]
    public void Board_Should_Set_Up_Pawns_Correctly()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        // Assert

        for (int i = FILE_A; i <= FILE_H; i++)
        {
            Pawn? pawn = board.State[RANK_2, i] as Pawn;

            pawn.Should().NotBeNull();
            pawn!.Color.Should().Be(PieceColor.White);
            pawn.CurrentRank.Should().Be(RANK_2);
            pawn.CurrentFile.Should().Be(i);
        }

        for (int i = FILE_A; i <= FILE_H; i++)
        {
            Pawn? pawn = board.State[RANK_7, i] as Pawn;

            pawn.Should().NotBeNull();
            pawn!.Color.Should().Be(PieceColor.Black);
            pawn.CurrentRank.Should().Be(RANK_7);
            pawn.CurrentFile.Should().Be(i);
        }
    }

    [Fact]
    public void Board_Should_Set_Up_Rooks_Correctly()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        Rook? whiteRook1 = board.State[RANK_1, FILE_A] as Rook;
        Rook? whiteRook2 = board.State[RANK_1, FILE_H] as Rook;

        Rook? blackRook1 = board.State[RANK_8, FILE_A] as Rook;
        Rook? blackRook2 = board.State[RANK_8, FILE_H] as Rook;

        // Assert
        whiteRook1.Should().NotBeNull();
        whiteRook2.Should().NotBeNull();
        blackRook1.Should().NotBeNull();
        blackRook2.Should().NotBeNull();

        whiteRook1!.Color.Should().Be(PieceColor.White);
        whiteRook2!.Color.Should().Be(PieceColor.White);
        blackRook1!.Color.Should().Be(PieceColor.Black);
        blackRook2!.Color.Should().Be(PieceColor.Black);

        whiteRook1.CurrentRank.Should().Be(RANK_1);
        whiteRook2.CurrentRank.Should().Be(RANK_1);
        blackRook1.CurrentRank.Should().Be(RANK_8);
        blackRook2.CurrentRank.Should().Be(RANK_8);

        whiteRook1.CurrentFile.Should().Be(FILE_A);
        whiteRook2.CurrentFile.Should().Be(FILE_H);
        blackRook1.CurrentFile.Should().Be(FILE_A);
        blackRook2.CurrentFile.Should().Be(FILE_H);
    }

    [Fact]
    public void Board_Should_Set_Up_Knights_Correctly()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        Knight? whiteKnight1 = board.State[RANK_1, FILE_B] as Knight;
        Knight? whiteKnight2 = board.State[RANK_1, FILE_G] as Knight;

        Knight? blackKnight1 = board.State[RANK_8, FILE_B] as Knight;
        Knight? blackKnight2 = board.State[RANK_8, FILE_G] as Knight;

        // Assert
        whiteKnight1.Should().NotBeNull();
        whiteKnight2.Should().NotBeNull();
        blackKnight1.Should().NotBeNull();
        blackKnight2.Should().NotBeNull();

        whiteKnight1!.Color.Should().Be(PieceColor.White);
        whiteKnight2!.Color.Should().Be(PieceColor.White);
        blackKnight1!.Color.Should().Be(PieceColor.Black);
        blackKnight2!.Color.Should().Be(PieceColor.Black);

        whiteKnight1.CurrentRank.Should().Be(RANK_1);
        whiteKnight2.CurrentRank.Should().Be(RANK_1);
        blackKnight1.CurrentRank.Should().Be(RANK_8);
        blackKnight2.CurrentRank.Should().Be(RANK_8);

        whiteKnight1.CurrentFile.Should().Be(FILE_B);
        whiteKnight2.CurrentFile.Should().Be(FILE_G);
        blackKnight1.CurrentFile.Should().Be(FILE_B);
        blackKnight2.CurrentFile.Should().Be(FILE_G);
    }

    [Fact]
    public void Board_Should_Set_Up_Bishops_Correctly()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        Bishop? whiteBishop1 = board.State[RANK_1, FILE_C] as Bishop;
        Bishop? whiteBishop2 = board.State[RANK_1, FILE_F] as Bishop;

        Bishop? blackBishop1 = board.State[RANK_8, FILE_C] as Bishop;
        Bishop? blackBishop2 = board.State[RANK_8, FILE_F] as Bishop;

        // Assert
        whiteBishop1.Should().NotBeNull();
        whiteBishop2.Should().NotBeNull();
        blackBishop1.Should().NotBeNull();
        blackBishop2.Should().NotBeNull();

        whiteBishop1!.Color.Should().Be(PieceColor.White);
        whiteBishop2!.Color.Should().Be(PieceColor.White);
        blackBishop1!.Color.Should().Be(PieceColor.Black);
        blackBishop2!.Color.Should().Be(PieceColor.Black);

        whiteBishop1.CurrentRank.Should().Be(RANK_1);
        whiteBishop2.CurrentRank.Should().Be(RANK_1);
        blackBishop1.CurrentRank.Should().Be(RANK_8);
        blackBishop2.CurrentRank.Should().Be(RANK_8);

        whiteBishop1.CurrentFile.Should().Be(FILE_C);
        whiteBishop2.CurrentFile.Should().Be(FILE_F);
        blackBishop1.CurrentFile.Should().Be(FILE_C);
        blackBishop2.CurrentFile.Should().Be(FILE_F);
    }

    [Fact]
    public void Board_Should_Set_Up_Queens_Correctly()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        Queen? whiteQueen = board.State[RANK_1, FILE_D] as Queen;
        Queen? blackQueen = board.State[RANK_8, FILE_D] as Queen;

        // Assert
        whiteQueen.Should().NotBeNull();
        blackQueen.Should().NotBeNull();

        whiteQueen!.Color.Should().Be(PieceColor.White);
        blackQueen!.Color.Should().Be(PieceColor.Black);

        whiteQueen.CurrentRank.Should().Be(RANK_1);
        blackQueen.CurrentRank.Should().Be(RANK_8);

        whiteQueen.CurrentFile.Should().Be(FILE_D);
        blackQueen.CurrentFile.Should().Be(FILE_D);
    }

    [Fact]
    public void Board_Should_Set_Up_Kings_Correctly()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        King? whiteKing = board.State[RANK_1, FILE_E] as King;
        King? blackKing = board.State[RANK_8, FILE_E] as King;

        // Assert
        whiteKing.Should().NotBeNull();
        blackKing.Should().NotBeNull();

        whiteKing!.Color.Should().Be(PieceColor.White);
        blackKing!.Color.Should().Be(PieceColor.Black);

        whiteKing.CurrentRank.Should().Be(RANK_1);
        blackKing.CurrentRank.Should().Be(RANK_8);

        whiteKing.CurrentFile.Should().Be(FILE_E);
        blackKing.CurrentFile.Should().Be(FILE_E);
    }

    [Fact]
    public void Board_Should_Have_White_Turn_First()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        // Assert
        board.TurnColor.Should().Be(PieceColor.White);
    }

    [Fact]
    public void Board_Should_Have_Middle_Four_Ranks_Empty()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        // Assert
        for(int i = RANK_3; i <= RANK_6; i++)
        {
            for (int j = FILE_A; j <= FILE_H; j++)
            {
                board.State[i, j].Should().BeNull();
            }
        }
    }

    [Fact]
    public void Board_Should_Be_Clear_After_Clear_Board_Method()
    {
        // Arrange
        GameBoard board = new GameBoard();
        board.SetupGame();

        // Act
        board.ClearBoard();

        // Assert
        for(int i = RANK_1; i <= RANK_8; i++)
        {
            for(int j = FILE_A; j <= FILE_H; j++)
            {
                board.State[i, j].Should().BeNull();
            }
        }
    }
}