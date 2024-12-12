## Tic-Tac-Toe Mini Project
##### Rules:
1. Traditionally the first player plays with "X". So you can decide who wants to go "X" and who wants to go with "O". 
2. Only one player can play at a time.
3. If any of the players have filled a square then the other player and the same player cannot override that square.
4. There are only two win conditions: draw or win.
5. The player that succeeds in placing three respective mark (X or O) in a horizontal, vertical or diagonal row wins the game.

##### Design Constraints:
- [x] Use console to make a tic-tac-toe game
- [x] First try to create game board using 2D array
- [x] Eventually make it so the user can resize the game board
- [x] Make it clear whose turn it is by displaying "Player X's Turn" or "Player O's Turn" at the start of each round.
- [x] add selectable player symbols (X, O, and ' ' should be able to be something else)
- [x] Add demo mode menu point for all 4 boards (show player 1 win conditions, player 2 win conditions and a draw condition)
	- [x] make sure to use `CheckWin()` and `CheckDraw()` to validate conditions
	- [x] keep it simple (hard code conditions if needed)
	- [x] Animate the output window so it looks like an actual game is being played
- [x] Make a computer opponent (1 with completely random numbers, 1 that's "smart")
	- [x] add `ComputerMove()` method for each board
	- [ ] either copy/paste `StartGame()` into a new method for computer vs. user or update `StartGame()` to include new `ComputerMove()` method
- [x] add base class (properties might include `CurrentPlayer` and/or `Size`)
- [x] Maybe make 2 computers play each other
- [ ] When a player wins, you can highlight the winning row, column, or diagonal by printing those cells in a different color. Use `Console.ForegroundColor` to change the text color temporarily.
- [ ] Add a leaderboard that gets read from and saved to a file
