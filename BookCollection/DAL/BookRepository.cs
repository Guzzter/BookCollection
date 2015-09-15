using BookCollection.Models;
using BookCollection.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookCollection.DAL
{

    public interface IBookRepository
    {
        void SetContext(BookContext bookContext);
        IEnumerable<Book> GetMostRecentBooks(int size);
        IEnumerable<CategoryGroup> GetBookCategories();
        IEnumerable<CategoryGroup> GetBookSeries();
    }

    public class BookRepository : IBookRepository
    {
        private BookContext db;
        public void SetContext(BookContext bookContext)
        {
            db = bookContext;
        }

        public IEnumerable<Book> GetMostRecentBooks(int size)
        {
            return db.Books.OrderByDescending(b => b.CreationDate).Take(size);
        }

        public IEnumerable<CategoryGroup> GetBookCategories()
        {
            var totalBooks = db.Books.Count();
            var taglist = from books in db.Books
                group books by books.Category.Title into catGroup
                select new CategoryGroup()
                {
                    CategoryName = catGroup.Key,
                    BookCount = catGroup.Count(),
                    TotalBookCount = totalBooks
                };

            return taglist.Where(c => c.CategoryName != "");
        }

        public IEnumerable<CategoryGroup> GetBookSeries()
        {
            var totalBooks = db.Books.Count();
            var taglist = from books in db.Books
                          group books by books.Serie into catGroup
                          select new CategoryGroup()
                          {
                              CategoryName = catGroup.Key,
                              BookCount = catGroup.Count(),
                              TotalBookCount = totalBooks
                          };

            return taglist.Where(c => c.CategoryName != "");
        }

        
    }
}