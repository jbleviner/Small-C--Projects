namespace TicTacToe
{
    /// <summary>
    /// Base Class: Can define both a contract (via abstract methods) and shared implementation (via concrete methods or properties).
    /// Shared functionality (like CheckDraw() or handling CurrentPlayer and Size) can be implemented in the base class and reused by all derived classes. 
    /// Only unique methods (like MakeMove() for different board structures) need to be overridden.
    /// </summary>
    public class BaseGameBoard     // abstract means the class cannot be instantiated directly
    {
        //use protected
        //public static IGameBoard lastSavedGame = null; // keeps track of the last game
        //public static int boardSize = 3;
        //public static int boardType = 1; // default to 2D array
        //public static char player1Symbol = 'X'; // default P1 symbol
        //public static char player2Symbol = 'O'; // default P2 symbol
        //public static bool isCPUOpponent = false; // default to no CPU
        //public static bool isCPUvsCPU = false;
        //public static string cpuDifficulty = "Random"; // default to easiest CPU difficulty

        public char CurrentPlayer { get; set; } // Tracks the current player
        public int Size { get; set; } // Board size

        //public virtual bool MakeMove(int row, int col, char playerSymbol) { return true; }

        //public GameBoardBase(int size)
        //{
        //    Size = size;
        //    CurrentPlayer = 'X'; // Default starting player
        //}
    }

    /*public class GameBoard : BaseGameBoard { 
    
        public GameBoard() { }
        public override bool MakeMove(int row, int col, char playerSymbol)
        {
            if (row >= 0 && row < Size && col >= 0 && col < Size && board[row, col] == ' ')
            {
                board[row, col] = playerSymbol;
                return true;
            }
            return false;
        }
    }

    public class GameBoard3 : BaseGameBoard
    {
        public GameBoard3() { }
        public override bool MakeMove(int row, int col, char playerSymbol)
        {
            if (row >= 0 && row < Size && col >= 0 && col < Size && board[row][col] == ' ')
            {
                board[row][col] = playerSymbol;
                return true;
            }
            return false;
        }
    }*/
}
