using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookManager
{
    /// <summary>
    /// This class holds the primary logic for the book manager program.
    /// </summary>
    public class BookManager : IBookManager
    {
        private List<Book> bookShelf = new List<Book>();
        private List<Book> tbrList = new List<Book>();

        private static string filePath = "C:\\Users\\Leviner.Jonathan\\Documents\\";
        public List<Book> GetBooksOnShelf() { return bookShelf; }
        public List<Book> GetBooksInTBR() { return tbrList; }

        /*private IBookManagerService bookManagerSerice;

        public BookManager()
        {
            bookManagerSerice = new BookInventoryService();
        }

        public void AddBookToShelfOrTBR()
        {
            bookManagerSerice.AddBookToShelfOrTBR(this);
        }

        public void SaveBooksToFile()
        {
            var fullPath = filePath + "books.txt";
            bookManagerSerice.SaveBooksToFile(this, fullPath);
        }*/

        public void ViewShelfAndTBR()
        {
            Console.Clear();
            Console.WriteLine("======== Books on Shelf ========");
            if (bookShelf.Count <= 0)
                Console.WriteLine("No books on shelf.");
            else
                foreach (var book in bookShelf)
                    Console.WriteLine(book.ToString());

            Console.WriteLine("\n======== \"To Be Read\" ========");
            if (tbrList.Count <= 0)
                Console.WriteLine("No books in TBR.");
            else
                foreach (var book in tbrList)
                    Console.WriteLine(book.ToString());
        }

        public void AddBookToShelf(Book book)
        {
            bookShelf.Add(book);
            Console.WriteLine($"\n\"{book.Title}\" has been added to your shelf.");
        }

        public void AddBookToTBR(Book book)
        {
            tbrList.Add(book);
            Console.WriteLine($"\n\"{book.Title}\" has been added to your TBR list.");
        }

        public bool MoveBookToShelf(string title)
        {
            // instead of using LINQ logic, you could do this:
            /*Book foundBook = null;
            foreach (var book in bookShelf)
                if (book.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    foundBook = book;
                    break;  // Exit the loop once we find the book
                }*/

            var book = tbrList.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (book != null)
            {
                tbrList.Remove(book);
                bookShelf.Add(book);
                ViewShelfAndTBR();
                Console.WriteLine($"\n\"{title}\" has been moved from your TBR to your shelf.");
                return true;
            }
            Console.WriteLine($"\"{title}\" not found in TBR. Please try again.");
            return false;
        }

        public bool RemoveBook(string title)
        {
            var removeFromShelf = bookShelf.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            var removeFromTBR = tbrList.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (removeFromShelf != null)
            {
                bookShelf.Remove(removeFromShelf);
                ViewShelfAndTBR();
                Console.WriteLine($"\n\"{title}\" has been removed from your shelf.");
                return true; // Book found and removed from shelf
            }
            else if (removeFromTBR != null)
            {
                tbrList.Remove(removeFromTBR);
                ViewShelfAndTBR();
                Console.WriteLine($"\n\"{title}\" has been removed from your TBR.");
                return true; // Book found and removed from TBR
            }
            Console.WriteLine($"\"{title}\" not found on shelf or in TBR.");
            return false;
        }

        public bool AddTagsToBook(string title, List<string> tags)
        {
            var bookOnShelf = bookShelf.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            var bookInTBR = tbrList.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (bookOnShelf != null)
            {
                bookOnShelf.Tags.AddRange(tags);
                ViewShelfAndTBR();
                Console.WriteLine($"\nTags have been added to \"{title}\".");
                return true;
            }
            else if (bookInTBR != null)
            {
                bookInTBR.Tags.AddRange(tags);
                ViewShelfAndTBR();
                Console.WriteLine($"\nTags have been added to \"{title}\".");
                return true;
            }
            Console.WriteLine($"{title} not found.");
            return false;
        }

        public bool RemoveTagsFromBook(string title, List<string> tagsToRemove)
        {
            var bookOnShelf = bookShelf.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            var bookInTBR = tbrList.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (bookOnShelf != null)
            {
                bookOnShelf.Tags = bookOnShelf.Tags.Except(tagsToRemove).ToList(); // remove the specified tags
                ViewShelfAndTBR();
                Console.WriteLine($"\nTags have been removed from \"{title}\".");
                return true;
            }
            else if (bookInTBR != null)
            {
                bookInTBR.Tags = bookInTBR.Tags.Except(tagsToRemove).ToList(); // remove the specified tags
                ViewShelfAndTBR();
                Console.WriteLine($"\nTags have been removed from \"{title}\".");
                return true;
            }
            Console.WriteLine($"{tagsToRemove} not found.");
            return false;
        }

        [Obsolete("Moved ViewTBR functionality to ViewShelfAndTBR")]
        public void ViewTBR()
        {
            Console.Clear();
            Console.WriteLine("======== TBR List ========");
            foreach (var book in tbrList)
                Console.WriteLine(book.ToString());
        }

        [Obsolete]
        public void ReorderShelf(string sortBy = "default")
        {
            List<Book> sortedShelf = bookShelf;
            List<Book> sortedTBR = tbrList;

            switch (sortBy.ToLower())
            {
                case "author":
                    sortedShelf = bookShelf.OrderBy(b => b.Author).ToList();
                    sortedTBR = tbrList.OrderBy(b => b.Author).ToList();
                    break;
                case "title":
                    sortedShelf = bookShelf.OrderBy(b => b.Title).ToList();
                    sortedTBR = tbrList.OrderBy(b => b.Title).ToList();
                    break;
                case "default":
                default:
                    break;
            }

            foreach (var book in sortedShelf)
                Console.WriteLine(book);
        }
    }
}