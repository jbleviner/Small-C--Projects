using System;
using System.Collections.Generic;
using System.Threading;

namespace TicTacToe
{
    public class GameBoardDictionary : BaseGameBoard, IGameBoard
    {
        private readonly Dictionary<int, char> board;

        /*How to Initialize: the dictionary uses an integer key to represent the position on the board, where 1 could represent the top-left corner, 2 represents the next cell, and so on
          in this case, each key maps to a character ('X', 'O', or ' ')
            Dictionary<int, char> boardDict = new Dictionary<int, char>();
            boardDict[1] = ' '; // Cell 1
            boardDict[2] = ' '; // Cell 2
            boardDict[3] = ' '; // Cell 3, etc.
        */

        /*private void test3()
        {
            board = new Dictionary<int, char>();
            for (int i = 1; i <= 9; i++)
            {
                board.Add(i, ' ');
            }
        }*/

        // Constructor to initialize the board with a default size
        public GameBoardDictionary(int boardSize = 3)
        {
            Size = boardSize;
            board = new Dictionary<int, char>();
            for (int i = 1; i <= boardSize * boardSize; i++) // initialize the dictionary with keys from 1 to boardSize^2
            {
                board[i] = ' ';
            }
        }

        

        public bool MakeMove(int row, int col, char playerSymbol)
        {
            int index = (row * Size) + col + 1; // Convert (row, col) to a 1D key
            if (board.ContainsKey(index) && board[index] == ' ')
            {
                board[index] = playerSymbol;
                return true;
            }
            return false;
        }

        public bool CheckWin(char playerSymbol)
        {
            for (int i = 0; i < Size; i++)
                if (CheckRow(i, playerSymbol) || CheckColumn(i, playerSymbol)) return true;

            if (CheckDiagonals(playerSymbol)) return true;

            return false;
        }

        public bool CheckDraw()
        {
            foreach (char cell in board.Values)
                if (cell == ' ') return false;
            return true;
        }

        private bool CheckRow(int row, char playerSymbol)
        {
            for (int col = 0; col < Size; col++)
            {
                int index = (row * Size) + col + 1; // Convert (row, col) to a 1D key
                if (board[index] != playerSymbol) return false;
            }
            return true;
        }

        private bool CheckColumn(int col, char playerSymbol)
        {
            for (int row = 0; row < Size; row++)
            {
                int index = (row * Size) + col + 1; // Convert (row, col) to a 1D key
                if (board[index] != playerSymbol) return false;
            }
            return true;
        }

        private bool CheckDiagonals(char playerSymbol)
        {
            bool leftDiagonal = true, rightDiagonal = true;

            for (int i = 0; i < Size; i++)
            {
                int leftIndex = (i * Size) + i + 1; // Top-left to bottom-right
                int rightIndex = (i * Size) + (Size - 1 - i) + 1; // Top-right to bottom-left

                if (board[leftIndex] != playerSymbol) leftDiagonal = false;
                if (board[rightIndex] != playerSymbol) rightDiagonal = false;
            }

            return leftDiagonal || rightDiagonal;
        }

        public void ClearBoard()
        {
            for (int i = 1; i <= Size * Size; i++)
            {
                board[i] = ' '; // reset each cell to empty
            }
        }

        public void Display()
        {
            Console.WriteLine("Dictionary Game Board:");
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
                    int index = (row * Size) + col + 1;
                    Console.Write($" {board[index]} ");
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
            for (int row = 0; row < Size; row++) // check for winning move
            {
                for (int col = 0; col < Size; col++)
                {
                    int index = (row * Size) + col + 1;
                    if (board[index] == ' ')
                    {
                        board[index] = computerSymbol;
                        if (CheckWin(computerSymbol))
                            return; // take winning move
                        board[index] = ' '; // undo the move
                    }
                }
            }

            for (int row = 0; row < Size; row++) // check for blocking move
            {
                for (int col = 0; col < Size; col++)
                {
                    int index = (row * Size) + col + 1;
                    if (board[index] == ' ')
                    {
                        board[index] = opponentSymbol;
                        if (CheckWin(opponentSymbol))
                        {
                            board[index] = computerSymbol; // block opponent
                            return;
                        }
                        board[index] = ' '; // undo the move
                    }
                }
            }

            // Random move as fallback
            ComputerMoveRandom(computerSymbol);
        }

        public void ComputerMoveRandom(char computerSymbol)
        {
            var random = new Random(Environment.TickCount);
            int index;

            do
            {
                index = random.Next(1, board.Count + 1); // Generate a random key within the dictionary range
            } while (board[index] != ' '); // Ensure the cell is empty

            // Calculate row and column based on the index
            int row = (index - 1) / Size;
            int col = (index - 1) % Size;

            MakeMove(row, col, computerSymbol);
        }
    }
}