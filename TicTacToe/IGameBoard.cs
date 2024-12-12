using System.Collections.Generic;

namespace TicTacToe
{
    /// <summary>
    /// Interface: Only defines a contract (method signatures and properties) that implementing classes must fulfill. It doesn't provide any implementation.
    /// Each GameBoard implementation (GameBoard2D, GameBoardJagged, etc.) must fully define methods like Display() and CheckWin(), even if the logic is identical across implementations.
    /// </summary>
    public interface IGameBoard
    {
        char CurrentPlayer { get; set; }
        int Size { get; set; }

        bool MakeMove(int row, int col, char playerSymbol);
        void ComputerMove(char computerSymbol, char opponentSymbol);
        void ComputerMoveRandom(char computerSymbol);
        bool CheckWin(char playerSymbol);
        bool CheckDraw();
        void ClearBoard();
        void Display();
    }
}