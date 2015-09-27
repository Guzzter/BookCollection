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
        void SetContext(IBookContext bookContext);
        IEnumerable<Book> GetMostRecentBooks(int size);
        IEnumerable<CategoryGroup> GetBookCategories();
        IEnumerable<CategoryGroup> GetBookSeries();
    }

    public class BookRepository : IBookRepository
    {
        private IBookContext db;
        public void SetContext(IBookContext bookContext)
        {
            db = bookContext;
        }

        public IEnumerable<Book> GetMostRecentBooks(int size)
        {
            return db.Query<Book>().OrderByDescending(b => b.CreationDate).Take(size);
        }

        public IEnumerable<CategoryGroup> GetBookCategories()
        {
            var totalBooks = db.Query<Book>().Count();
            var taglist = from books in db.Query<Book>()
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
            var totalBooks = db.Query<Book>().Count();
            var taglist = from books in db.Query<Book>()
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