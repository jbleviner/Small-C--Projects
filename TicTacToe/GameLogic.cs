using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace TicTacToe
{
    /// <summary>
    /// This class handles the game logic
    /// </summary>
    public class GameLogic : BaseGameBoard
    {
        public static IGameBoard lastSavedGame = null; // keeps track of the last game
        public static int boardSize = 3;
        public static int boardType = 1; // default to 2D array
        public static char player1Symbol = 'X'; // default P1 symbol
        public static char player2Symbol = 'O'; // default P2 symbol
        public static bool isCPUOpponent = false; // default to no CPU
        public static bool isCPUvsCPU = false;
        public static string cpuDifficulty = "Random"; // default to easiest CPU difficulty

        public static IGameBoard CreateBoardInstance(int boardType, int boardSize)
        {
            switch (boardType)
            {
                case 1:
                    return new GameBoard2D(boardSize);
                case 2:
                    return new GameBoardJagged(boardSize);
                case 3:
                    return new GameBoardListOfList(boardSize);
                case 4:
                    return new GameBoardDictionary(boardSize);
                default:
                    return new GameBoard2D(boardSize);
            }
        }

        public static void StartGame(IGameBoard gameBoard, char startingPlayer)
        {
            gameBoard.CurrentPlayer = startingPlayer;
            char computer1Symbol = player1Symbol;
            char computer2Symbol = player2Symbol;
            bool isGameRunning = true;

            while (isGameRunning)
            {
                Console.Clear();
                gameBoard.Display();

                if (isCPUvsCPU)
                {
                    Console.WriteLine($"\nCPU {gameBoard.CurrentPlayer} is making its move...");
                    if (gameBoard.CurrentPlayer == computer1Symbol)
                        gameBoard.ComputerMove(computer1Symbol, computer2Symbol); // CPU 1 move against CPU 2
                    else
                        gameBoard.ComputerMove(computer2Symbol, computer1Symbol); // CPU 2 move against CPU 1
                    Thread.Sleep(1000); // pause for animation

                    if (ContinueGame(gameBoard)) continue;
                    else break;
                }

                if (isCPUOpponent && gameBoard.CurrentPlayer == computer2Symbol) // Computer's move against player
                {
                    Console.WriteLine($"\nComputer ({cpuDifficulty}) is making its move...");
                    if (cpuDifficulty == "Smart")
                        gameBoard.ComputerMove(computer2Symbol, player1Symbol); // assume "Smart" logic is implemented
                    else
                        gameBoard.ComputerMoveRandom(computer2Symbol); // default to "Random"
                    Thread.Sleep(1000); // pause for animation

                    if (ContinueGame(gameBoard)) continue;
                    else break;
                }

                if (!isCPUvsCPU)
                {
                    Console.Write($"\nPlayer {gameBoard.CurrentPlayer}, enter your move as 'row column' (e.g., 1 1), or type 'exit' to quit: ");
                    string input = Console.ReadLine()?.Trim();

                    if (input?.ToLower() == "exit" || input?.ToLower() == "x")
                    {
                        Console.WriteLine("Game saved.\nPress any key to continue...");
                        Console.ReadKey();
                        lastSavedGame = gameBoard; // save game state
                        //GameMenuManager.StartGameSubmenu();
                        break;
                    }

                    string[] move = Regex.Replace(input, @"\s+", " ").Split(' ');
                    if (move.Length == 2 && int.TryParse(move[0], out int row) && int.TryParse(move[1], out int col))
                    {
                        row--; col--; // convert to zero-based index
                        if (!gameBoard.MakeMove(row, col, gameBoard.CurrentPlayer))
                        {
                            Console.WriteLine("Invalid move. Try again.");
                            Console.ReadKey();
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                        Console.ReadKey();
                        continue;
                    }

                    if (ContinueGame(gameBoard)) continue;
                    else break;
                }
            }

            FinalizeGame();
        }

        private static bool ContinueGame(IGameBoard gameBoard)
        {
            // check for win or draw
            if (gameBoard.CheckWin(gameBoard.CurrentPlayer))
            {
                Console.Clear();
                gameBoard.Display();
                Console.WriteLine($"\nPlayer {gameBoard.CurrentPlayer} wins!");
                return false;
            }
            else if (gameBoard.CheckDraw())
            {
                Console.Clear();
                gameBoard.Display();
                Console.WriteLine("\nIt's a draw!");
                return false;
            }
            else // switch players
                gameBoard.CurrentPlayer = (gameBoard.CurrentPlayer == player1Symbol) ? player2Symbol : player1Symbol;

            return true;
        }

        private static void FinalizeGame()
        {
            // prompt for replay only if the game wasn't saved
            if (lastSavedGame == null)
            {
                Console.Write("\nPlay again? (y/N): ");

                string playAgain = Console.ReadLine()?.ToLower();
                if (playAgain == "y" || playAgain == "yes")
                    StartGame(CreateBoardInstance(boardType, boardSize), player1Symbol); // replay the game
                else GameMenuManager.StartGameSubmenu();
            }
        }

        public static void RunDemoAnimated(IGameBoard board, string boardTypeName)
        {
            Console.Clear();
            Console.WriteLine($"Demo for {boardTypeName}:\n"); // Static header at the top

            var tests = new List<Action<IGameBoard, char>> { (TestRowWin), (TestColumnWin), (TestDiagonalWin) };

            foreach (var test in tests)
            {
                foreach (var player in new[] { player1Symbol, player2Symbol })
                {
                    // reserve a fixed position below the header for dynamic content
                    int startLine = 2; // header takes up 3 lines
                    Console.SetCursorPosition(0, startLine);
                    ClearBelow(startLine);

                    test(board, player);

                    Console.Write("Press any key to proceed to the next test...");
                    Console.ReadKey();
                }
            }

            TestDrawCondition(board, player1Symbol);
        }

        private static void AnimateMove(IGameBoard board, int row, int col, char player)
        {
            int startLine = 3; // start below header
            board.MakeMove(row, col, player);
            Thread.Sleep(500);

            Console.SetCursorPosition(0, startLine);
            ClearBelow(startLine); // clear previous board display
            board.Display();
        }

        private static void ClearBelow(int startLine)
        {
            for (int i = startLine; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, startLine);
        }

        private static void TestRowWin(IGameBoard board, char winningPlayer)
        {
            board.CurrentPlayer = (board.CurrentPlayer == player1Symbol) ? player2Symbol : player1Symbol;
            Console.WriteLine($"=== Row Win Condition for Player {board.CurrentPlayer} ===");
            board.ClearBoard();
            board.Display();

            if (winningPlayer == player1Symbol) // Player 1 wins on a row
            {
                AnimateMove(board, 0, 0, player1Symbol);
                AnimateMove(board, 1, 1, player2Symbol);
                AnimateMove(board, 0, 1, player1Symbol);
                AnimateMove(board, 2, 0, player2Symbol);
                AnimateMove(board, 0, 2, player1Symbol);
            }
            if (winningPlayer == player2Symbol) // Player 2 wins on a row
            {
                AnimateMove(board, 0, 0, player1Symbol);
                AnimateMove(board, 2, 0, player2Symbol);
                AnimateMove(board, 0, 1, player1Symbol);
                AnimateMove(board, 2, 1, player2Symbol);
                AnimateMove(board, 1, 1, player1Symbol);
                AnimateMove(board, 2, 2, player2Symbol);
            }
            if (board.CheckWin(board.CurrentPlayer))
                Console.WriteLine($"\nPlayer {board.CurrentPlayer} wins!\n");
            else Console.WriteLine("\nNo win condition found\n");
        }

        private static void TestColumnWin(IGameBoard board, char winningPlayer)
        {
            board.CurrentPlayer = (board.CurrentPlayer == player1Symbol) ? player2Symbol : player1Symbol;
            Console.WriteLine($"=== Column Win Condition for Player {board.CurrentPlayer} ===");
            board.ClearBoard();
            board.Display();

            if (winningPlayer == player1Symbol) // Player 1 wins on a column
            {
                AnimateMove(board, 0, 0, player1Symbol);
                AnimateMove(board, 1, 2, player2Symbol);
                AnimateMove(board, 1, 0, player1Symbol);
                AnimateMove(board, 0, 1, player2Symbol);
                AnimateMove(board, 2, 0, player1Symbol);
            }
            if (winningPlayer == player2Symbol) // Player 2 wins on a column
            {
                AnimateMove(board, 2, 0, player1Symbol);
                AnimateMove(board, 0, 2, player2Symbol);
                AnimateMove(board, 0, 1, player1Symbol);
                AnimateMove(board, 1, 2, player2Symbol);
                AnimateMove(board, 1, 1, player1Symbol);
                AnimateMove(board, 2, 2, player2Symbol);
            }
            if (board.CheckWin(board.CurrentPlayer))
                Console.WriteLine($"\nPlayer {board.CurrentPlayer} wins!\n");
            else Console.WriteLine("\nNo win condition found\n");
        }

        private static void TestDiagonalWin(IGameBoard board, char winningPlayer)
        {
            board.CurrentPlayer = (board.CurrentPlayer == player1Symbol) ? player2Symbol : player1Symbol;
            Console.WriteLine($"=== Diagonal Win Condition for Player {board.CurrentPlayer} ===");
            board.ClearBoard();
            board.Display();

            if (winningPlayer == player1Symbol) // Player 1 wins on a diagonal
            {
                AnimateMove(board, 0, 0, player1Symbol);
                AnimateMove(board, 2, 0, player2Symbol);
                AnimateMove(board, 1, 1, player1Symbol);
                AnimateMove(board, 0, 2, player2Symbol);
                AnimateMove(board, 2, 2, player1Symbol);
            }
            if (winningPlayer == player2Symbol) // Player 2 wins on a diagonal
            {
                AnimateMove(board, 0, 0, player1Symbol);
                AnimateMove(board, 2, 0, player2Symbol);
                AnimateMove(board, 1, 0, player1Symbol);
                AnimateMove(board, 1, 1, player2Symbol);
                AnimateMove(board, 2, 2, player1Symbol);
                AnimateMove(board, 0, 2, player2Symbol);
            }
            if (board.CheckWin(board.CurrentPlayer))
                Console.WriteLine($"\nPlayer {board.CurrentPlayer} wins!\n");
            else Console.WriteLine("\nNo win condition found\n");
        }

        private static void TestDrawCondition(IGameBoard board, char startingPlayer)
        {
            board.CurrentPlayer = startingPlayer;
            Console.WriteLine($"=== Draw Condition ===");
            board.ClearBoard();
            board.Display();

            if (startingPlayer == player1Symbol)
            {
                AnimateMove(board, 1, 1, player1Symbol);
                AnimateMove(board, 0, 0, player2Symbol);
                AnimateMove(board, 0, 2, player1Symbol);
                AnimateMove(board, 2, 0, player2Symbol);
                AnimateMove(board, 1, 0, player1Symbol);
                AnimateMove(board, 1, 2, player2Symbol);
                AnimateMove(board, 0, 1, player1Symbol);
                AnimateMove(board, 2, 1, player2Symbol);
                AnimateMove(board, 2, 2, player1Symbol);
            }
            if (board.CheckDraw())
                Console.WriteLine("\nIt's a draw!");
            else Console.WriteLine("\nNo draw condition found\n");
        }

        [Obsolete]
        public static void RunDemo(IGameBoard board, string boardTypeName)
        {
            Console.Clear();
            Console.WriteLine($"Demo for {boardTypeName}:\n");

            var tests = new List<(string Name, Action<IGameBoard, char> TestMethod)>
            {
                ("Row Win Condition", TestRowWin),
                ("Column Win Condition", TestColumnWin),
                ("Diagonal Win Condition", TestDiagonalWin)
            };

            foreach (var (Name, TestMethod) in tests) // I had no idea you could deconstruct variables like that. Might not be the best thing to do if the List were instantiated somewhere else.
            {
                foreach (var player in new[] { player1Symbol, player2Symbol })
                {
                    Console.WriteLine($"=== {Name} for Player {player} ===");
                    TestMethod(board, player);
                    Console.Write("\nPress any key to proceed to the next test...");
                    Console.ReadKey();
                    Console.SetCursorPosition(0, Console.CursorTop); // move cursor to the start of the current line
                    Console.Write(new string(' ', Console.WindowWidth)); // clear the line
                    Console.SetCursorPosition(0, Console.CursorTop - 1); // reset the cursor
                }
            }

            Console.WriteLine("=== Draw Condition ===");
            TestDrawCondition(board, player1Symbol);
        }
    }
}
