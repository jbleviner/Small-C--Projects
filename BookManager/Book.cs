using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public string Edition { get; set; }
        public List<string> Tags { get; set; }

        public Book(string title, string author,  decimal price, string edition, List<string> tags)
        {
            Title = title;
            Author = author;
            Price = price;
            Edition = edition;
            Tags = tags;
        }

        public override string ToString()
        {
            return $"\"{Title}\" by {Author}" +
                $"\n - Price: ${Price}" +
                $"\n - Edition: {Edition}" +
                $"\n - Tags: {string.Join(", ", Tags)}";
        }
    }
}
