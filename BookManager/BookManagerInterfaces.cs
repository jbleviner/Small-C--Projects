using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager
{
    /// <summary>
    /// 
    /// This doesn't really work as an interface since there is only one class that uses all of these methods.
    /// </summary>
    public interface IBookManager
    {
        List<Book> GetBooksOnShelf();

        List<Book> GetBooksInTBR();

        void ViewShelfAndTBR();

        void AddBookToShelf(Book book);

        void AddBookToTBR(Book book);

        bool MoveBookToShelf(string title);

        //void SaveBooksToFile();

        bool RemoveBook(string title);

        bool AddTagsToBook(string title, List<string> tags);

        bool RemoveTagsFromBook(string title, List<string> tags);
    }

    /// <summary>
    /// Interface for B UI service
    /// </summary>
    [Obsolete]
    interface IBookManagerService
    {
        void AddBookToShelfOrTBR(IBookManager bookManager);
        void SaveBooksToFile(IBookManager bookManager, string filePath);
    }
}
