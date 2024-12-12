using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace BookManager
{
    /// <summary>
    /// This class builds the UI for the book manager program.
    /// </summary>
    public class Program
    {
        private static string filePath = "C:\\Users\\Leviner.Jonathan\\Documents\\";
        static IBookManager bookManager = new BookManager();

        private static void Main()
        {
            LoadBooksFromFile(filePath + "books.txt"); // Load books from file at the start
            //LoadBooksFromCsv(filePath + "books.csv");

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("======================== Book Manager ========================");
                Console.WriteLine("Welcome to the Book Manager Program! Please make a selection.");
                Console.WriteLine("   1. View Shelf and \"TBR\"");
                Console.WriteLine("   2. Add Book to Shelf or \"TBR\"");
                Console.WriteLine("   3. Organize Books");
                Console.WriteLine("   4. Add or Remove Book Tags");
                Console.WriteLine("   5. Exit");
                Console.WriteLine("==============================================================");
                Console.Write("Enter your choice (1-5): ");
                string choice = Console.ReadLine().Trim();

                switch (choice)
                {
                    case "1":
                        bookManager.ViewShelfAndTBR(); // try and add a way to sort by title, author, and default order
                        break;
                    case "2":
                        AddBookToShelfOrTBR();
                        break;
                    case "3":
                        MoveOrRemoveBook();
                        break;
                    case "4":
                        AddOrRemoveTags();
                        break;
                    case "5":
                        Console.Write("Saving books and exiting program... Goodbye!");
                        Thread.Sleep(1500);
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                SaveBooksToFile(filePath + "books.txt");    // Save after each operation is completed
                //BookInventoryService.SaveBooksToFile(bookManager, filePath + "books.txt");
                //SaveBooksToCsv(filePath + "books.csv");
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void AddBookToShelfOrTBR()
        {
            Console.Clear();
            Console.WriteLine("======== Add Book ========");
            Console.Write("Enter the title: ");
            string title = Console.ReadLine().Trim();

            Console.Write("Enter the author: ");
            string author = Console.ReadLine().Trim();
            while (!Regex.IsMatch(author, @"^[a-zA-Z\s\-']+$")) // use Regular expression to validate string characters
            {
                Console.WriteLine("Invalid input. Please enter a name containing only letters, spaces, hyphens, and apostrophes.");
                Console.Write("Enter the author: ");
                author = Console.ReadLine().Trim();
            }

            Console.Write("Enter the price: ");
            decimal price;
            string input = Console.ReadLine().Replace("$", "").Trim();
            while (!decimal.TryParse(input, out price) || price < 0) // TryParse returns a bool, can't just write price.Parse() //(&& !Regex.IsMatch(priceString, @"^[\d\,.]+$"))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for the price.");
                Console.Write("Enter the price: ");
                input = Console.ReadLine().Replace("$", "").Trim();
            }

            Console.Write("Enter the edition (optional): ");
            string edition = Console.ReadLine().Trim();
            while (!string.IsNullOrEmpty(edition) && !Regex.IsMatch(edition, @"^[\s\w\#,-]+$")) // use Regular expression to validate string characters
            {
                Console.WriteLine("Invalid input. Please enter a number or a word. (i.e. \"1\", \"#1\", \"1st\"), \"First\"");
                Console.Write("Enter the edition (optional): ");
                edition = Console.ReadLine().Trim();
            }

            Console.Write("Enter tags separated by commas (optional): ");
            string tagsInput = Console.ReadLine();
            List<string> tags = new List<string>(tagsInput.Split(',').Select(tag => tag.Trim()));

            Book book = new Book(title, author, price, edition, tags);

            bool loop = true; bool exitToMenu = false;
            Console.Write("Where would you like to add the book?\n   Type 'S' for Shelf or 'T' for TBR: ");
            while (loop)
            {
                string choice = Console.ReadLine().ToUpper().Trim();
                if (choice == "S" || choice == "SHELF")
                {
                    bookManager.AddBookToShelf(book);
                    break;
                }
                else if (choice == "T" || choice == "TBR")
                {
                    bookManager.AddBookToTBR(book);
                    break;
                }
                else
                    Console.Write("Invalid choice. Type 'S' or 'T': ");
            }

            AskToRepeatAction(exitToMenu, AddBookToShelfOrTBR, "add");
        }

        private static void MoveOrRemoveBook()
        {
            Console.Clear();
            bookManager.ViewShelfAndTBR();
            if (CheckIfBothListsAreEmpty()) return;
            Console.WriteLine("\n======== Organize Books ========");
            Console.Write("What would you like to do?" +
                "\n (R) - Remove a book from your shelf or TBR." +
                "\n (M) - Move a book from your TBR to the shelf." +
                "\n (X) - Exit to main menu." +
                "\nMake a selection: ");

            string title; bool loop = true; bool exitToMenu = false;
            while (loop) // sentinel condition
            {
                string choice = Console.ReadLine().ToUpper();
                if (choice == "R" || choice == "REMOVE" || choice == "RMV")
                {
                    if (CheckIfBothListsAreEmpty()) return;
                    Console.Write("\nEnter the title of the book you want to remove from the shelf or your TBR: ");
                    title = Console.ReadLine().Trim().ToLower();
                    loop = !bookManager.RemoveBook(title); // if result of RemoveBook() returns false, loop is set to false and jumps out of while loop
                }
                else if (choice == "M" || choice == "MOVE" || choice == "MV")
                {
                    if (CheckIfTBRIsEmpty()) return;
                    Console.Write("\nEnter the title of the book you want to move to the shelf: ");
                    title = Console.ReadLine().Trim().ToLower();
                    loop = !bookManager.MoveBookToShelf(title);
                }
                else if (choice == "X" || choice == "EXIT")
                {
                    Console.Write("Returning to menu...");
                    exitToMenu = true; // Set the flag to skip the next prompt
                    break;
                }
                else
                    Console.Write("Invalid choice. Type 'R', 'M', or 'X': ");
            }

            AskToRepeatAction(exitToMenu, MoveOrRemoveBook, "move or remove");
        }

        private static void AskToRepeatAction(bool exitToMenu, Action repeatAction, string actionName)
        {
            if (exitToMenu) return; // if the user chose to exit, skip asking for another action

            Console.Write($"\nWould you like to {actionName} another book? (Y/N) ");
            while (!exitToMenu)
            {
                string choice = Console.ReadLine().ToUpper().Trim();
                if (choice == "Y" || choice == "YES")
                {
                    repeatAction();  // call the provided action (e.g., AddBookToShelfOrTBR, MoveOrRemoveBook)
                    break;
                }
                else if (choice == "N" || choice == "NO")
                {
                    Console.Write("Returning to menu...");
                    break;
                }
                else
                    Console.Write("Invalid choice. Please type 'Y' for Yes or 'N' for No: ");
            }
        }

        private static void AddOrRemoveTags()
        {
            Console.Clear();
            bookManager.ViewShelfAndTBR();
            if (CheckIfBothListsAreEmpty()) return;

            Console.WriteLine("\n======== Add or Remove Book Tags ========");
            string title = ""; bool loop = true; bool doesBookExist = false;

            while (!doesBookExist) // prompt for title until a valid one is provided ("while true")
            {
                Console.Write("\nEnter the title of the book you want to modify tags for: ");
                title = Console.ReadLine().Trim().ToLower();
                // Check if the book exists on the shelf or in the TBR. Breaks once doesBookExist returns true.
                doesBookExist = bookManager.GetBooksOnShelf().Any(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase)) ||
                    bookManager.GetBooksInTBR().Any(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

                if (!doesBookExist)
                    Console.WriteLine($"Invalid input. \"{title}\" not found.");
            }

            Console.Write("Would you like to (A)dd or (R)emove tags? (A/R): ");
            while (loop)
            {
                string choice = Console.ReadLine().Trim().ToUpper();
                if (choice == "A" || choice == "ADD")
                {
                    Console.Write("Enter tags separated by commas: ");
                    string tagsInput = Console.ReadLine().Trim();
                    while (string.IsNullOrEmpty(tagsInput))
                    {
                        Console.WriteLine("Invalid input. Cannot add an empty tag.");
                        Console.Write("Enter tags separated by commas: ");
                        tagsInput = Console.ReadLine().Trim();
                    }
                    List<string> tags = tagsInput.Split(',').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToList();
                    loop = !bookManager.AddTagsToBook(title, tags); // set loop to "false" to break
                }
                else if (choice == "R" || choice == "REMOVE")
                {
                    Console.Write("Enter tags to remove, separated by commas: ");
                    string tagsInput = Console.ReadLine().Trim();
                    while (string.IsNullOrEmpty(tagsInput))
                    {
                        Console.WriteLine("Invalid input. Cannot remove empty tags.");
                        Console.Write("Enter tags to remove, separated by commas: ");
                        tagsInput = Console.ReadLine().Trim();
                    }
                    List<string> tagsToRemove = tagsInput.Split(',').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToList();
                    loop = !bookManager.RemoveTagsFromBook(title, tagsToRemove);
                }
                else
                    Console.Write("Invalid choice. Type 'A' or 'R': ");
            }
        }

        private static void SaveBooksToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("======== Books on Shelf ========");
                foreach (var book in bookManager.GetBooksOnShelf())
                    writer.WriteLine($"{book.Title}, {book.Author}, ${book.Price}, {book.Edition}, {string.Join("; ", book.Tags.Select(tag => tag.Trim()))}");

                writer.WriteLine("\n======== Books in TBR ========");
                foreach (var book in bookManager.GetBooksInTBR())
                    writer.WriteLine($"{book.Title}, {book.Author}, ${book.Price}, {book.Edition}, {string.Join("; ", book.Tags.Select(tag => tag.Trim()))}");
            }
        }

        private static void LoadBooksFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No saved data found. Starting fresh...");
                return;
            }

            using (StreamReader readFile = new StreamReader(filePath))
            {
                string line; bool isShelf = true;  // track whether we are reading shelf or TBR list
                while ((line = readFile.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;  // skip empty lines
                    if (line.StartsWith("======== Books on Shelf ========"))
                        continue;
                    if (line.StartsWith("======== Books in TBR ========"))
                    {
                        isShelf = false;
                        continue;
                    }
                    ParseDataFromFile(line, isShelf);
                }
            }
        }

        private static void ParseDataFromFile(string line, bool isShelf)
        {
            string[] bookData = line.Trim().Split(','); // split the line into book data (Title, Author, Price, Edition, Tags)
            if (bookData.Length < 5) return;  // skip invalid data

            string title = bookData[0].Trim();
            string author = bookData[1].Trim();
            decimal price = decimal.Parse(bookData[2].Replace("$", "").Trim());
            string edition = bookData[3].Trim();
            List<string> tags = bookData[4].Trim().Split(';').Select(tag => tag.Trim()).ToList();
            Book book = new Book(title, author, price, edition, tags);

            if (isShelf) // add to shelf or TBR list based on the flag
                bookManager.AddBookToShelf(book);
            else
                bookManager.AddBookToTBR(book);
        }

        private static bool CheckIfBothListsAreEmpty()
        {
            // check if there are any books on the shelf or in the TBR list
            if (!bookManager.GetBooksOnShelf().Any() && !bookManager.GetBooksInTBR().Any())
            {
                Console.WriteLine("\nNo books available.");
                return true; // return true if shelf is empty
            }
            return false;
        }

        [Obsolete("Functionality of RemoveBook() was changed so this function is now unused.")]
        private static bool CheckIfShelfIsEmpty()
        {
            // Check if there are any books on the shelf or in the TBR list
            if (!bookManager.GetBooksOnShelf().Any())
            {
                Console.WriteLine("\nNo books available on shelf.");
                return true;    // return true if shelf is empty
            }
            return false;
        }

        private static bool CheckIfTBRIsEmpty()
        {
            // Check if there are any books on the shelf or in the TBR list
            if (!bookManager.GetBooksInTBR().Any())
            {
                Console.WriteLine("\nNo books available in TBR.");
                return true;    // return true if TBR is empty
            }
            return false;
        }

        [Obsolete]
        private static void SaveBooksToCsv(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Title,Author,Price,Tags,Shelf"); // write header

                foreach (var book in bookManager.GetBooksOnShelf()) // write books from the shelf
                    writer.WriteLine($"{book.Title},{book.Author},{book.Price},{string.Join("; ", book.Tags)},Shelf");

                foreach (var book in bookManager.GetBooksInTBR()) // write books from the TBR list
                    writer.WriteLine($"{book.Title},{book.Author},{book.Price},{string.Join("; ", book.Tags)},TBR");
            }

            Console.WriteLine($"\nBooks have been saved to {filePath}");
        }

        [Obsolete]
        private static void LoadBooksFromCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No saved data found.");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                reader.ReadLine(); // skip header

                while ((line = reader.ReadLine()) != null)
                {
                    string[] bookData = line.Trim().Split(','); // split the line by commas into the relevant fields

                    if (bookData.Length < 6) return; // skip invalid data

                    string title = bookData[0];
                    string author = bookData[1];
                    decimal price = decimal.Parse(bookData[2]);
                    string edition = bookData[3].Trim();
                    List<string> tags = bookData[4].Trim().Split(';').ToList();
                    string location = bookData[5];

                    Book book = new Book(title, author, price, edition, tags);

                    if (location == "Shelf")
                        bookManager.AddBookToShelf(book);
                    else if (location == "TBR")
                        bookManager.AddBookToTBR(book);
                }
            }
        }
    }
}