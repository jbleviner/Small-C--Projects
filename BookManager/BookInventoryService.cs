using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookManager
{
    [Obsolete]
    class BookInventoryService : IBookManagerService
    {

        public BookInventoryService() { }

        public void SaveBooksToFile(IBookManager bookManager, string filePath)
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

        public void AddBookToShelfOrTBR(IBookManager bookManager)
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

            //AskToRepeatAction(exitToMenu, AddBookToShelfOrTBR, "add");
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
    }
}
