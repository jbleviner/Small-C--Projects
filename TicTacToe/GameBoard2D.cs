using System;
using System.Collections.Generic;
using System.Threading;

namespace TicTacToe
{
    public class GameBoard2D : BaseGameBoard, IGameBoard
    {
        private readonly char[,] board;

        /*How to Initialize: this creates a board that holds 3 rows and 3 columns where each element can be accessed using two indices (board2D[row, col])
            char[,] board2D = new char[3, 3]; // A 3x3 board
        */

        // Constructor to initialize the board with a default size
        public GameBoard2D(int boardSize = 3)
        {
            Size = boardSize;
            board = new char[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    board[i, j] = ' ';
        }

        public bool MakeMove(int row, int col, char playerSymbol)
        {
            //base.MakeMove()

            if (row >= 0 && row < Size && col >= 0 && col < Size && board[row, col] == ' ')
            {
                board[row, col] = playerSymbol;
                return true;
            }
            return false;
        }

        public bool CheckWin(char playerSymbol)
        {
            // Check rows, columns, and diagonals
            for (int i = 0; i < Size; i++)
                if (CheckRow(i, playerSymbol) || CheckColumn(i, playerSymbol)) return true;

            if (CheckDiagonals(playerSymbol)) return true;

            return false;
        }

        public bool CheckDraw()
        {
            foreach (char cell in board)
                if (cell == ' ') return false;
            return true;
        }

        private bool CheckRow(int row, char playerSymbol)
        {
            for (int col = 0; col < Size; col++)
                if (board[row, col] != playerSymbol) return false;
            return true;
        }

        private bool CheckColumn(int col, char playerSymbol)
        {
            for (int row = 0; row < Size; row++)
                if (board[row, col] != playerSymbol) return false;
            return true;
        }

        private bool CheckDiagonals(char player)
        {
            bool leftDiagonal = true, rightDiagonal = true;
            for (int i = 0; i < Size; i++)
            {
                if (board[i, i] != player) leftDiagonal = false;
                if (board[i, Size - 1 - i] != player) rightDiagonal = false;
            }

            return leftDiagonal || rightDiagonal;
        }

        public void ClearBoard()
        {
            for (int row = 0; row < Size; row++)
                for (int col = 0; col < Size; col++)
                    board[row, col] = ' '; // reset each cell to empty
        }

        public void Display()
        {
            Console.WriteLine("2D Array Game Board:");
            Console.Write("   "); // Top-left corner padding
            for (int col = 0; col < Size; col++)
            {
                Console.Write($"{col + 1}   "); // Column numbers
            }
            Console.WriteLine();

            for (int row = 0; row < Size; row++)
            {
                Console.Write($"{row + 1} "); // Row numbers
                for (int col = 0; col < Size; col++)
                {
                    Console.Write($" {board[row, col]} ");
                    if (col < Size - 1)
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine();

                if (row < Size - 1)
                {
                    Console.Write("  "); // Row number padding
                    for (int col = 0; col < Size; col++)
                    {
                        Console.Write("---");
                        if (col < Size - 1)
                        {
                            Console.Write("+");
                        }
                    }
                    Console.WriteLine();
                }
            }
        }

        public void ComputerMove(char computerSymbol, char opponentSymbol)
        {
            // could try using minimax algorithm

            // try to take winning move
            if (TryMakeStrategicMove(computerSymbol, computerSymbol))
                return;

            // try to block opponent's winning move
            if (TryMakeStrategicMove(opponentSymbol, computerSymbol))
                return;

            // prefer center
            if (board[Size / 2, Size / 2] == ' ')
            {
                board[Size / 2, Size / 2] = computerSymbol;
                return;
            }

            // prefer corners
            int[][] corners = { new[] { 0, 0 }, new[] { 0, Size - 1 }, new[] { Size - 1, 0 }, new[] { Size - 1, Size - 1 } };
            foreach (var corner in corners)
            {
                if (board[corner[0], corner[1]] == ' ')
                {
                    board[corner[0], corner[1]] = computerSymbol;
                    return;
                }
            }

            // Random move as fallback
            ComputerMoveRandom(computerSymbol);
        }

        private bool TryMakeStrategicMove(char checkSymbol, char moveSymbol)
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (board[row, col] == ' ')
                    {
                        board[row, col] = checkSymbol;
                        if (CheckWin(checkSymbol))
                        {
                            board[row, col] = moveSymbol; // Commit move for the computer
                            return true;
                        }
                        board[row, col] = ' '; // Undo move
                    }
                }
            }
            return false; // No strategic move found
        }

        public void ComputerMoveRandom(char computerSymbol)
        {
            var random = new Random(Environment.TickCount);
            int row, col;

            // check for a deadloop, of deadloop then terminate program

            do
            {
                row = random.Next(0, Size);
                col = random.Next(0, Size);
            } while (board[row, col] != ' '); // make sure the cell is empty

            MakeMove(row, col, computerSymbol);
        }

        #region

        // Snippets that either didn't work or got replaced

        /*public void AnimatedDisplay(List<(int row, int col, char symbol)> moves)
        {
            foreach (var move in moves)
            {
                MakeMove(move.row, move.col, move.symbol); // Update the board
                Console.Clear(); // Clear the console
                Display(); // Show the updated board
                Thread.Sleep(750); // Wait for animation
            }
        }*/

        #endregion
    }
}