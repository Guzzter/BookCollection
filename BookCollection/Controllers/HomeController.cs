using BookCollection.DAL;
using BookCollection.Helpers;
using BookCollection.Models;
using BookCollection.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BookCollection.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IBookRepository rep, IBookContext bc) : base(rep, bc) { /* constructor forward to BaseController */ }

        public ActionResult Index()
        {
            // http://www.mikesdotnetting.com/article/107/creating-a-tag-cloud-using-asp-net-mvc-and-the-entity-framework

            ViewBag.CatTagCloud = repo.GetBookCategories();
            ViewBag.SerieTagCloud = repo.GetBookSeries();
            ViewBag.MostRecent = repo.GetMostRecentBooks(5);
            // ViewData = ViewData is a dictionary object that you put data into, which then becomes available to the view. ViewData is a derivative of the ViewDataDictionary class, so you can access by the familiar "key/value" syntax.
            // ViewBag = The ViewBag object is a wrapper around the ViewData object that allows you to create dynamic properties for the ViewBag.

            return View();
        }

        public ActionResult DownloadExcel()
        {
            var data = db.Query<Book>().ToList();
            GridView gv = new GridView();
            gv.DataSource = data;
            gv.DataBind();

            return new DownloadExcelFileActionResult(gv, "Books.xls");
        }

        public ActionResult About()
        {
            Statistics stats = new Statistics();
            stats.Books = db.Query<Book>().Count();
            stats.Authors = db.Query<Author>().Count();
            stats.Subjects = db.Query<Subject>().Count();
            stats.Publishers = db.Query<Publisher>().Count();
            
            // Commenting out LINQ to show how to do the same thing in SQL.
            IQueryable<CategoryGroup> grp = from books in db.Query<Book>()
                                                 group books by books.Category.Title into catGroup
                                                 select new CategoryGroup()
                                                 {
                                                     CategoryName = catGroup.Key,
                                                     BookCount = catGroup.Count()
                                                 };

            stats.CategoryGroupStats = grp.OrderByDescending(cg => cg.BookCount).ThenBy(cg => cg.CategoryName).ToList();
            stats.Categories = stats.CategoryGroupStats.Count();

            IQueryable<CategoryGroup> grpLang = from books in db.Query<Book>()
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