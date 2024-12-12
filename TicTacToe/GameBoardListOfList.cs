using System;
using System.Collections.Generic;
using System.Threading;

namespace TicTacToe
{
    public class GameBoardListOfList : BaseGameBoard, IGameBoard
    {
        private readonly List<List<char>> board;

        /*How to Initialize: each inner list represents a row of the Tic-Tac-Toe board
            List<List<char>> boardList = new List<List<char>>();
            boardList.Add(new List<char> { ' ', ' ', ' ' }); // First row
            boardList.Add(new List<char> { ' ', ' ', ' ' }); // Second row
            boardList.Add(new List<char> { ' ', ' ', ' ' }); // Third row
        */

        /*private void test2()
        {
            board = new List<List<char>>();
            for (int i = 0; i < 3; i++)
            {
                board.Add(new List<char>());
                for (int j = 0; j < 3; j++)
                {
                    board.ElementAt(i).Add(' ');
                }
            }
        }*/

        // Constructor to initialize the board with a default size
        public GameBoardListOfList(int boardSize = 3)
        {
            Size = boardSize;
            board = new List<List<char>>();
            for (int i = 0; i < boardSize; i++)
            {
                List<char> row = new List<char>();
                for (int j = 0; j < boardSize; j++)
                    row.Add(' ');
                board.Add(row);
            }
        }

        public bool MakeMove(int row, int col, char playerSymbol)
        {
            if (row >= 0 && row < Size && col >= 0 && col < Size && board[row][col] == ' ')
            {
                board[row][col] = playerSymbol;
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
            foreach (var row in board)
                foreach (char cell in row)
                    if (cell == ' ') return false;
            return true;
        }

        private bool CheckRow(int row, char playerSymbol)
        {
            for (int col = 0; col < Size; col++)
                if (board[row][col] != playerSymbol) return false;
            return true;
        }

        private bool CheckColumn(int col, char playerSymbol)
        {
            for (int row = 0; row < Size; row++)
                if (board[row][col] != playerSymbol) return false;
            return true;
        }

        private bool CheckDiagonals(char player)
        {
            bool leftDiagonal = true, rightDiagonal = true;
            for (int i = 0; i < Size; i++)
            {
                if (board[i][i] != player) leftDiagonal = false;
                if (board[i][Size - 1 - i] != player) rightDiagonal = false;
            }

            return leftDiagonal || rightDiagonal;
        }

        public void ClearBoard()
        {
            for (int row = 0; row < Size; row++)
                for (int col = 0; col < Size; col++)
                    board[row][col] = ' '; // reset each cell to empty
        }

        public void Display()
        {
            Console.WriteLine("List of List Game Board:");
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
                    Console.Write($" {board[row][col]} ");
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
                    if (board[row][col] == ' ')
                    {
                        board[row][col] = computerSymbol;
                        if (CheckWin(computerSymbol))
                            return; // take winning move
                        board[row][col] = ' '; // undo the move
                    }
                }
            }

            for (int row = 0; row < Size; row++) // check for blocking move
            {
                for (int col = 0; col < Size; col++)
                {
                    if (board[row][col] == ' ')
                    {
                        board[row][col] = opponentSymbol;
                        if (CheckWin(opponentSymbol))
                        {
                            board[row][col] = computerSymbol; // block opponent
                            return;
                        }
                        board[row][col] = ' '; // undo the move
                    }
                }
            }

            // Random move as fallback
            ComputerMoveRandom(computerSymbol);
        }

        public void ComputerMoveRandom(char computerSymbol)
        {
            var random = new Random(Environment.TickCount);
            int row, col;

            do
            {
                row = random.Next(0, Size);
                col = random.Next(0, Size);
            } while (board[row][col] != ' '); // make sure the cell is empty

            MakeMove(row, col, computerSymbol);
        }
    }
}