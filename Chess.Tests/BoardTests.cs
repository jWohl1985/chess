using Chess.Logic;
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
            pawn.Rank.Should().Be(1);
            pawn.File.Should().Be(i);
        }

        for (int i = 0; i <= 7; i++)
        {
            Pawn? pawn = board.State[6, i] as Pawn;

            if (pawn is null)
                Assert.Fail();
            pawn.Color.Should().Be(PieceColor.Black);
            pawn.Rank.Should().Be(6);
            pawn.File.Should().Be(i);
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
        if (whiteRook1 is null || whiteRook2 is null || blackRook1 is null || blackRook2 is null)
            Assert.Fail();

        whiteRook1.Color.Should().Be(PieceColor.White);
        whiteRook2.Color.Should().Be(PieceColor.White);
        blackRook1.Color.Should().Be(PieceColor.Black);
        blackRook2.Color.Should().Be(PieceColor.Black);

        whiteRook1.Rank.Should().Be(0);
        whiteRook2.Rank.Should().Be(0);
        blackRook1.Rank.Should().Be(7);
        blackRook2.Rank.Should().Be(7);

        whiteRook1.File.Should().Be(0);
        whiteRook2.File.Should().Be(7);
        blackRook1.File.Should().Be(0);
        blackRook2.File.Should().Be(7);
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
        if (whiteKnight1 is null || whiteKnight2 is null || blackKnight1 is null || blackKnight2 is null)
            Assert.Fail();

        whiteKnight1.Color.Should().Be(PieceColor.White);
        whiteKnight2.Color.Should().Be(PieceColor.White);
        blackKnight1.Color.Should().Be(PieceColor.Black);
        blackKnight2.Color.Should().Be(PieceColor.Black);

        whiteKnight1.Rank.Should().Be(0);
        whiteKnight2.Rank.Should().Be(0);
        blackKnight1.Rank.Should().Be(7);
        blackKnight2.Rank.Should().Be(7);

        whiteKnight1.File.Should().Be(1);
        whiteKnight2.File.Should().Be(6);
        blackKnight1.File.Should().Be(1);
        blackKnight2.File.Should().Be(6);
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
        if (whiteBishop1 is null || whiteBishop2 is null || blackBishop1 is null || blackBishop2 is null)
            Assert.Fail();

        whiteBishop1.Color.Should().Be(PieceColor.White);
        whiteBishop2.Color.Should().Be(PieceColor.White);
        blackBishop1.Color.Should().Be(PieceColor.Black);
        blackBishop2.Color.Should().Be(PieceColor.Black);

        whiteBishop1.Rank.Should().Be(0);
        whiteBishop2.Rank.Should().Be(0);
        blackBishop1.Rank.Should().Be(7);
        blackBishop2.Rank.Should().Be(7);

        whiteBishop1.File.Should().Be(2);
        whiteBishop2.File.Should().Be(5);
        blackBishop1.File.Should().Be(2);
        blackBishop2.File.Should().Be(5);
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
        if (whiteQueen is null || blackQueen is null)
            Assert.Fail();

        whiteQueen.Color.Should().Be(PieceColor.White);
        blackQueen.Color.Should().Be(PieceColor.Black);

        whiteQueen.Rank.Should().Be(0);
        blackQueen.Rank.Should().Be(7);

        whiteQueen.File.Should().Be(3);
        blackQueen.File.Should().Be(3);
    }

    [Fact]
    public void Board_Should_Set_Up_Kings_Correctly()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Act
        board.SetupGame();

        King? whiteKing = board.State[0, 4] as King;
        King? blackKing = board.State[7, 4] as King;

        // Assert
        if (whiteKing is null || blackKing is null)
            Assert.Fail();

        whiteKing.Color.Should().Be(PieceColor.White);
        blackKing.Color.Should().Be(PieceColor.Black);

        whiteKing.Rank.Should().Be(0);
        blackKing.Rank.Should().Be(7);

        whiteKing.File.Should().Be(4);
        blackKing.File.Should().Be(4);
    }

    [Fact]
    public void Board_Should_Have_White_Turn_First()
    {
        // Arrange
        GameBoard board = new GameBoard();

        // Assert
        board.TurnColor.Should().Be(PieceColor.White);
    }
}