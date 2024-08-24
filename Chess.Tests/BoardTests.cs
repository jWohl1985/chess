using Chess.Logic;
using static Chess.Logic.GameBoard;
using FluentAssertions;

namespace Chess.Tests;

public class BoardTests
{
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

        for (int i = 0; i <= 7; i++)
        {
            Pawn? pawn = board.State[1, i] as Pawn;

            if (pawn is null) 
                Assert.Fail();
            pawn.Color.Should().Be(PieceColor.White);
            pawn.CurrentRank.Should().Be(1);
            pawn.CurrentFile.Should().Be(i);
        }

        for (int i = 0; i <= 7; i++)
        {
            Pawn? pawn = board.State[6, i] as Pawn;

            if (pawn is null)
                Assert.Fail();
            pawn.Color.Should().Be(PieceColor.Black);
            pawn.CurrentRank.Should().Be(6);
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

        Rook? whiteRook1 = board.State[0, 0] as Rook;
        Rook? whiteRook2 = board.State[0, 7] as Rook;

        Rook? blackRook1 = board.State[7, 0] as Rook;
        Rook? blackRook2 = board.State[7, 7] as Rook;

        // Assert
        whiteRook1.Should().NotBeNull();
        whiteRook2.Should().NotBeNull();
        blackRook1.Should().NotBeNull();
        blackRook2.Should().NotBeNull();

        whiteRook1!.Color.Should().Be(PieceColor.White);
        whiteRook2!.Color.Should().Be(PieceColor.White);
        blackRook1!.Color.Should().Be(PieceColor.Black);
        blackRook2!.Color.Should().Be(PieceColor.Black);

        whiteRook1.CurrentRank.Should().Be(0);
        whiteRook2.CurrentRank.Should().Be(0);
        blackRook1.CurrentRank.Should().Be(7);
        blackRook2.CurrentRank.Should().Be(7);

        whiteRook1.CurrentFile.Should().Be(0);
        whiteRook2.CurrentFile.Should().Be(7);
        blackRook1.CurrentFile.Should().Be(0);
        blackRook2.CurrentFile.Should().Be(7);
    }

    [Fact]
    public void Board_Should_Set_Up_Knights_Correctly()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        Knight? whiteKnight1 = board.State[0, 1] as Knight;
        Knight? whiteKnight2 = board.State[0, 6] as Knight;

        Knight? blackKnight1 = board.State[7, 1] as Knight;
        Knight? blackKnight2 = board.State[7, 6] as Knight;

        // Assert
        whiteKnight1.Should().NotBeNull();
        whiteKnight2.Should().NotBeNull();
        blackKnight1.Should().NotBeNull();
        blackKnight2.Should().NotBeNull();

        whiteKnight1!.Color.Should().Be(PieceColor.White);
        whiteKnight2!.Color.Should().Be(PieceColor.White);
        blackKnight1!.Color.Should().Be(PieceColor.Black);
        blackKnight2!.Color.Should().Be(PieceColor.Black);

        whiteKnight1.CurrentRank.Should().Be(0);
        whiteKnight2.CurrentRank.Should().Be(0);
        blackKnight1.CurrentRank.Should().Be(7);
        blackKnight2.CurrentRank.Should().Be(7);

        whiteKnight1.CurrentFile.Should().Be(1);
        whiteKnight2.CurrentFile.Should().Be(6);
        blackKnight1.CurrentFile.Should().Be(1);
        blackKnight2.CurrentFile.Should().Be(6);
    }

    [Fact]
    public void Board_Should_Set_Up_Bishops_Correctly()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        Bishop? whiteBishop1 = board.State[0, 2] as Bishop;
        Bishop? whiteBishop2 = board.State[0, 5] as Bishop;

        Bishop? blackBishop1 = board.State[7, 2] as Bishop;
        Bishop? blackBishop2 = board.State[7, 5] as Bishop;

        // Assert
        whiteBishop1.Should().NotBeNull();
        whiteBishop2.Should().NotBeNull();
        blackBishop1.Should().NotBeNull();
        blackBishop2.Should().NotBeNull();

        whiteBishop1!.Color.Should().Be(PieceColor.White);
        whiteBishop2!.Color.Should().Be(PieceColor.White);
        blackBishop1!.Color.Should().Be(PieceColor.Black);
        blackBishop2!.Color.Should().Be(PieceColor.Black);

        whiteBishop1.CurrentRank.Should().Be(0);
        whiteBishop2.CurrentRank.Should().Be(0);
        blackBishop1.CurrentRank.Should().Be(7);
        blackBishop2.CurrentRank.Should().Be(7);

        whiteBishop1.CurrentFile.Should().Be(2);
        whiteBishop2.CurrentFile.Should().Be(5);
        blackBishop1.CurrentFile.Should().Be(2);
        blackBishop2.CurrentFile.Should().Be(5);
    }

    [Fact]
    public void Board_Should_Set_Up_Queens_Correctly()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        Queen? whiteQueen = board.State[0, 3] as Queen;
        Queen? blackQueen = board.State[7, 3] as Queen;

        // Assert
        whiteQueen.Should().NotBeNull();
        blackQueen.Should().NotBeNull();

        whiteQueen!.Color.Should().Be(PieceColor.White);
        blackQueen!.Color.Should().Be(PieceColor.Black);

        whiteQueen.CurrentRank.Should().Be(0);
        blackQueen.CurrentRank.Should().Be(7);

        whiteQueen.CurrentFile.Should().Be(3);
        blackQueen.CurrentFile.Should().Be(3);
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

        whiteKing.CurrentRank.Should().Be(0);
        blackKing.CurrentRank.Should().Be(7);

        whiteKing.CurrentFile.Should().Be(4);
        blackKing.CurrentFile.Should().Be(4);
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
        for(int i = 2; i <=5; i++)
        {
            for (int j = 0; j <= 7; j++)
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
        foreach (Piece? piece in board.State)
        {
            piece.Should().BeNull();
        }
    }
}