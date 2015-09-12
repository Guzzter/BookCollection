using BookCollection.DAL;
using BookCollection.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookCollection.Controllers
{
    public class HomeController : Controller
    {
        private BookContext db = new BookContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            Statistics stats = new Statistics();
            stats.Books = db.Books.Count();
            stats.Authors = db.Authors.Count();
            stats.Subjects = db.Subjects.Count();
            stats.Publishers = db.Publishers.Count();
            
            // Commenting out LINQ to show how to do the same thing in SQL.
            IQueryable<CategoryGroup> grp = from books in db.Books
                                                 group books by books.Category.Title into catGroup
                                                 select new CategoryGroup()
                                                 {
                                                     CategoryName = catGroup.Key,
                                                     BookCount = catGroup.Count()
                                                 };

            stats.CategoryGroupStats = grp.OrderByDescending(cg => cg.BookCount).ThenBy(cg => cg.CategoryName).ToList();
            stats.Categories = stats.CategoryGroupStats.Count();

            IQueryable<CategoryGroup> grpLang = from books in db.Books
                                            group books by books.Language into catGroup
                                            select new CategoryGroup()
                                            {
                                                CategoryName = catGroup.Key.ToString(),
                                                BookCount = catGroup.Count()
                                            };

            stats.LanguageGroupStats = grpLang.OrderByDescending(cg => cg.BookCount).ThenBy(cg => cg.CategoryName).ToList();
            stats.Languages = stats.LanguageGroupStats.Count();

            // SQL version of the above LINQ code.
            //string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
            //    + "FROM Person "
            //    + "WHERE Discriminator = 'Student' "
            //    + "GROUP BY EnrollmentDate";
            //IEnumerable<EnrollmentDateGroup> data = db.Database.SqlQuery<EnrollmentDateGroup>(query);

            return View(stats);
        }
        
    }
}