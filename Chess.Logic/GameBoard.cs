using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic;

public class GameBoard
{
    // keeps track of where all the pieces are in a 2D array. Slot is null if there is nothing there.
    public Piece?[,] State { get; private set; }

    // keeps track of whose turn it is, white or black
    public PieceColor TurnColor { get; private set; }

    public void SetupGame()
    {
        // new up the State
        // new up all the pieces and set them in their correct squares per the rules
    }
}