using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    /// <summary>
    /// This class handles the game UI
    /// </summary>
    public class GameMenuManager : GameLogic // need to inherit the "global variables" from GameLogic
    {
        private class MenuOption
        {
            public Action action;
            public string displayText;
        }

        // Change MenuManager -> GameManger
        // Move game logic into own class OR keep in new GameManager class
        // Remove everything in Program that isn't related to shell instance for this program
        // Use game manager class to handle transitioning between running a game and handling the menu options

        private static readonly Dictionary<int, MenuOption> mainMenuOptions = new Dictionary<int, MenuOption>
        {
            { 1, new MenuOption() { action = () => { lastSavedGame = null; StartGameSubmenu(); } , displayText = "Start New Game" } },
            { 2, new MenuOption() { action = () => { DemoMode(); } , displayText = "Demo Mode" } },
            { 3, new MenuOption() { action = () => { Program.Exit(); }, displayText = "Exit" } }
        };

        private static readonly Dictionary<int, MenuOption> startgameSubMenuOptions = new Dictionary<int, MenuOption>
        {
            { 1, new MenuOption() { action = () => { lastSavedGame = null; StartGame(CreateBoardInstance(boardType, boardSize), player1Symbol); } , displayText = "Play" } },
            { 2, new MenuOption() { action = () => { SelectBoardType(); }, displayText = "Select Board Type"} },
            { 3, new MenuOption() { action = () => { ChangeBoardSize(); }, displayText = "Change Board Size"} },
            { 4, new MenuOption() { action = () => { SelectPlayerSymbols(); } , displayText = "Select Player Symbols" } },
            { 5, new MenuOption() { action = () => { ChangeOppopnent(); }, displayText = "Change Opponent"} },
            { 6, new MenuOption() { action = () => { GameMenu(); }, displayText = "Return to Main Menu"} }
        };

        public static void StartGameLoop()
        {
            bool running = true;
            while (running)
            {
                GameMenu();
            }
        }

        public static void GameMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(".-----. _         .-----.             .-----.            ");
            Console.WriteLine("`-. .-':_;        `-. .-'             `-. .-'            ");
            Console.WriteLine("  : :  .-. .--.     : : .--.   .--.     : : .--.  .--.   ");
            Console.WriteLine("  : :  : :'  ..'    : :' .; ; '  ..'    : :' .; :' '_.'  ");
            Console.WriteLine("  :_;  :_;`.__.'    :_;`.__,_;`.__.'    :_;`.__.'`.__.'  \n");
            Console.ResetColor();
            //Console.WriteLine("\nWelcome to Tic-Tac-Toe!\n");

            // remove the "Continue Last Saved Game" option if lastSavedGame is null
            var continueOptionKey = mainMenuOptions.Where(pair => pair.Value.displayText == "Continue Last Saved Game").Select(pair => pair.Key).FirstOrDefault();
            if (lastSavedGame == null && continueOptionKey != 0) mainMenuOptions.Remove(continueOptionKey); // Remove the dynamic option
            if (lastSavedGame != null && continueOptionKey == 0) // Add the "Continue Last Saved Game" option if it doesn't already exist
            {
                mainMenuOptions.Add(mainMenuOptions.Count + 1, new MenuOption { action = () => StartGame(lastSavedGame, lastSavedGame.CurrentPlayer), displayText = "Continue Last Saved Game" });
            }

            /*if (lastSavedGame != null && !mainMenuOptions.Values.Any(option => option.displayText == "Continue Last Saved Game"))
            {
                mainMenuOptions.Add(mainMenuOptions.Count + 1, new MenuOption() { action = () => StartGame(lastSavedGame, lastSavedGame.CurrentPlayer), displayText = "Continue Last Saved Game" });
            }*/

            foreach (var option in mainMenuOptions)
                Console.WriteLine($"{option.Key}. {option.Value.displayText}");

            Console.Write("\nChoose an option: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && mainMenuOptions.ContainsKey(choice))
                mainMenuOptions[choice].action.Invoke();
            else
            {
                Console.WriteLine("Invalid choice. Press any key to try again.");
                Console.ReadKey();
            }
        }

        public static void StartGameSubmenu()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Game Menu");
                Console.ResetColor();

                foreach (var option in startgameSubMenuOptions)
                    Console.WriteLine($"{option.Key}. {option.Value.displayText}");

                Console.Write("\nChoose an option: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && startgameSubMenuOptions.ContainsKey(choice))
                {
                    startgameSubMenuOptions[choice].action.Invoke();
                    break; // Exit the submenu loop after a valid action
                }
                else
                {
                    Console.WriteLine("Invalid choice. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }

        public static void DemoMode()
        {
            while (true)
            {
                Console.Write("\u001bc\x1b[3J");    // https://stackoverflow.com/questions/75471607/console-clear-doesnt-clean-up-the-whole-console
                Console.WriteLine("Demo Mode: Select a board type to demo.");
                Console.WriteLine("1. 2D Array Board");
                Console.WriteLine("2. Jagged Array Board");
                Console.WriteLine("3. List of Lists Board");
                Console.WriteLine("4. Dictionary Board");
                Console.WriteLine("5. Exit to Main Menu");
                Console.Write("\nChoose an option: ");

                switch (Console.ReadLine().Trim())
                {
                    case "1":
                        RunDemoAnimated(new GameBoard2D(3), "2D Array Board");
                        break;
                    case "2":
                        RunDemoAnimated(new GameBoardJagged(3), "Jagged Array Board");
                        break;
                    case "3":
                        RunDemoAnimated(new GameBoardListOfList(3), "List of Lists Board");
                        break;
                    case "4":
                        RunDemoAnimated(new GameBoardDictionary(3), "Dictionary Board");
                        break;
                    case "5":
                        return; // exit Demo Mode and return to main menu
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void SelectBoardType()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Select Board Type:");
            Console.ResetColor();
            Console.WriteLine("1. 2D Array");
            Console.WriteLine("2. Jagged Array");
            Console.WriteLine("3. List of Lists");
            Console.WriteLine("4. Dictionary");
            Console.Write("\nChoose an option: ");

            var boardTypeNames = new Dictionary<int, string> { { 1, "2D Array" }, { 2, "Jagged Array" }, { 3, "List of Lists" }, { 4, "Dictionary" } };

            if (int.TryParse(Console.ReadLine(), out int choice) && boardTypeNames.ContainsKey(choice))
            {
                boardType = choice;
                lastSavedGame = null; // reset saved game
                Console.WriteLine($"Board type set to {boardTypeNames[choice]}.");
            }
            else
            {
                lastSavedGame = null; // reset saved game
                Console.WriteLine("Invalid choice. Defaulting to 2D Array.");
                boardType = 1;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            StartGameSubmenu();
        }

        private static void ChangeBoardSize()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Change Board Size");
            Console.ResetColor();

            while (true)
            {
                Console.Write("Enter the new board size (min 3, max 9): ");
                if (int.TryParse(Console.ReadLine(), out int newSize) && newSize >= 3 && newSize <= 9)
                {
                    boardSize = newSize;
                    lastSavedGame = null; // reset saved game
                    Console.WriteLine($"Board size set to {newSize}.\n");
                    CreateBoardInstance(boardType, newSize).Display();
                    break;
                }
                else
                    Console.WriteLine("Invalid size. Please try again.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            StartGameSubmenu();
        }

        private static void SelectPlayerSymbols()
        {
            char GetValidSymbol(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true); // read key without displaying it
                    char inputChar = keyInfo.KeyChar;

                    if (!char.IsControl(inputChar)) // check if the character is a letter or digit
                    {
                        Console.WriteLine(inputChar); // echo the valid character
                        return inputChar;
                    }

                    Console.WriteLine("\nInvalid input. Please enter a letter, number, or symbol.");
                }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Select Player Symbols");
            Console.ResetColor();
            Console.WriteLine("Please enter a letter, number, or symbol.");

            while (true)
            {
                char input1 = GetValidSymbol("Enter Player 1's Symbol: ");
                char input2 = GetValidSymbol("Enter Player 2's Symbol: ");

                if (input1 == input2)
                {
                    Console.WriteLine("Symbols must be unique. Please try again.");
                }
                else
                {
                    player1Symbol = input1;
                    player2Symbol = input2;
                    Console.WriteLine($"Player 1's Symbol: {player1Symbol}, Player 2's Symbol: {player2Symbol}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                }
            }

            StartGameSubmenu();
        }

        private static void ChangeOppopnent()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Change Opponent:");
            Console.ResetColor();
            Console.WriteLine("1. User vs. User");
            Console.WriteLine("2. User vs Easy CPU");
            Console.WriteLine("3. User vs Hard CPU");
            Console.WriteLine("4. CPU vs. CPU");
            Console.WriteLine("5. Return to Game Menu");
            Console.Write("\nChoose an option: ");

            while (true)
            {
                switch (Console.ReadLine().Trim())
                {
                    case "1":
                        lastSavedGame = null; // reset saved game
                        isCPUOpponent = false;
                        isCPUvsCPU = false;
                        Console.WriteLine("Opponent changed to User vs. User.");
                        break;
                    case "2":
                        lastSavedGame = null; // reset saved game
                        isCPUOpponent = true;
                        isCPUvsCPU = false;
                        cpuDifficulty = "Random";
                        Console.WriteLine("Opponent changed to User vs. Easy CPU.");
                        break;
                    case "3":
                        lastSavedGame = null; // reset saved game
                        isCPUOpponent = true;
                        isCPUvsCPU = false;
                        cpuDifficulty = "Smart";
                        Console.WriteLine("Opponent changed to User vs. Hard CPU.");
                        break;
                    case "4":
                        lastSavedGame = null; // reset saved game
                        isCPUOpponent = true;
                        isCPUvsCPU = true;
                        Console.WriteLine("Opponent changed to CPU vs. CPU.");
                        break;
                    case "5":
                        Console.WriteLine("\nReturning to menu...");
                        StartGameSubmenu();
                        return; // exit this method and return to submenu
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        continue; // loop again for a valid input
                }

                break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            StartGameSubmenu();
        }
    }
}