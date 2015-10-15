using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookCollection.DAL;
using BookCollection.Models;
using PagedList;
using BookCollection.Helpers;

namespace BookCollection.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookContext _db;

        //TODO use base controller
        public BooksController(IBookContext dbContext)
        {
            _db = dbContext;
        }

        // GET: Books
        public ActionResult Index(string sortOrder, string currentFilter, string bookCategory, string serie, string searchString, int? page, bool noPaging = false)
        {
            ViewBag.CurrentSort = sortOrder;
            /*
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "name";
            ViewBag.AuthorSortParm = String.IsNullOrEmpty(sortOrder) ? "author_desc" : "author";
            ViewBag.PublisherSortParm = String.IsNullOrEmpty(sortOrder) ? "publisher_desc" : "publisher";
            ViewBag.YearSortParm = String.IsNullOrEmpty(sortOrder) ? "year_desc" : "year";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "category";
            ViewBag.SerieSortParm = String.IsNullOrEmpty(sortOrder) ? "serie_desc" : "serie";
            */
            if (!String.IsNullOrEmpty(sortOrder) && sortOrder.Contains("_"))
            {
                string[] sort = sortOrder.ToLowerInvariant().Split(new [] { '_' });

                if (sort[0].Equals("name"))
                {
                    ViewBag.NameSortParm = sort[1].Equals("desc") ? "name_desc" : "name_asc";
                }
                else if (sort[0].Equals("author"))
                {
                    ViewBag.AuthorSortParm = sort[1].Equals("desc") ? "author_desc" : "author_asc";
                }
                /*
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "name_asc";
                ViewBag.AuthorSortParm = String.IsNullOrEmpty(sortOrder) ? "author_desc" : "author";
                ViewBag.PublisherSortParm = String.IsNullOrEmpty(sortOrder) ? "publisher_desc" : "publisher";
                ViewBag.YearSortParm = String.IsNullOrEmpty(sortOrder) ? "year_desc" : "year";
                ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "category";
                ViewBag.SerieSortParm = String.IsNullOrEmpty(sortOrder) ? "serie_desc" : "serie";
                */

            }
            else
            {
                // default sort on name
                sortOrder = "name_asc";
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var books = _db.Query<Book>();

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString)
                                       || s.AlternativeTitle.Contains(searchString)
                                       || s.Authors.Count(a => a.Lastname.Contains(searchString))>0);
            }
            if (!string.IsNullOrEmpty(bookCategory))
            {
                books = books.Where(x => x.Category.Title == bookCategory);
            }
            if (!string.IsNullOrEmpty(serie))
            {
                books = books.Where(x => x.Serie == serie);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    books = books.OrderByDescending(s => s.Title);
                    break;
                case "publisher_asc":
                    books = books.OrderBy(s => s.Publisher.Name);
                    break;
                case "publisher_desc":
                    books = books.OrderByDescending(s => s.Publisher.Name);
                    break;
                case "author_asc":
                    books = books.OrderBy(s => s.MainAuthor);
                    break;
                case "author_desc":
                    books = books.OrderByDescending(s => s.MainAuthor);
                    break;
                case "category_asc":
                    books = books.OrderBy(s => s.Category.Title);
                    break;
                case "category_desc":
                    books = books.OrderByDescending(s => s.Category.Title);
                    break;
                case "year_asc":
                    books = books.OrderBy(s => s.ActualPrintYear);
                    break;
                case "year_desc":
                    books = books.OrderByDescending(s => s.ActualPrintYear);
                    break;
                case "serie_asc":
                    books = books.OrderBy(s => s.Serie);
                    break;
                case "serie_desc":
                    books = books.OrderByDescending(s => s.Serie);
                    break;
                default:  // Default Name ascending 
                    books = books.OrderBy(s => s.Title);
                    break;
            }

            // Eager load authors, categories and publishers
            books.Include(a => a.Authors);
            books.Include(a => a.Category);
            books.Include(a => a.Publisher);

            AddCategoriesForDropDownFilter();
            AddSerieForDropDownFilter();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(books.ToPagedList(pageNumber, pageSize));
        }

        private void AddCategoriesForDropDownFilter()
        {
            var CatLst = new List<string>();

            var GenreQry = _db.Query<Category>().OrderBy(c => c.Title).Select(c => c.Title);

            CatLst.AddRange(GenreQry.Distinct());
            ViewBag.BookCategory = new SelectList(CatLst);
        }

        private void AddSerieForDropDownFilter()
        {
            var serie = new List<string>();

            var SerieQry = _db.Query<Book>().Select(t => t.Serie);

            serie.AddRange(SerieQry.Distinct());
            ViewBag.Serie = new SelectList(serie);
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            // TODO: simplify
            Book book = _db.Query<Book>().FirstOrDefault(b => b.BookID == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            PopulateAuthorDropDownList();
            PopulateCategoryDropDownList();
            PopulatePublishersDropDownList();
            PopulateMainSubjectDropDownList();
            PopulateSubjectDropDownList();
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,Title,AlternativeTitle,AuthorID,CategoryID,InitialPrintedYear,ActualPrintYear,Language,Material,Read,Pages,ISBN,Website,CoverLink,Rating,CodeWithinSerie,Condition,ReviewNote,Location")] Book book)
        {
            if (ModelState.IsValid)
            {
                
                _db.Add(book);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateAuthorDropDownList(book.AuthorID);
            PopulateCategoryDropDownList(book.CategoryID);
            PopulatePublishersDropDownList(book.PublisherID);
            PopulateMainSubjectDropDownList(book.MainSubjectID);

            if (book.Subjects.Count > 0)
            {
                PopulateSubjectDropDownList(book.Subjects.First().SubjectID);
            }else
            {
                PopulateSubjectDropDownList();
            }
            
            return View(book);
        }

        #region dropdowns
        private void PopulateAuthorDropDownList(object selected = null)
        {
            var query = _db.Query<Author>().OrderBy(a => a.Lastname);
            ViewBag.AuthorID = new SelectList(query, "AuthorID", "Fullname", selected);
        }
        private void PopulateCategoryDropDownList(object selected = null)
        {
            var query = _db.Query<Category>().OrderBy(a => a.Title);
            ViewBag.CategoryID = new SelectList(query, "CategoryID", "Title", selected);
        }

        private void PopulatePublishersDropDownList(object selected = null)
        {
            var query = _db.Query<Publisher>().OrderBy(a => a.Name);
            ViewBag.PublisherID = new SelectList(query, "PublisherID", "Name", selected);
        }

        private void PopulateMainSubjectDropDownList(object selected = null)
        {
            var query = _db.Query<Subject>().OrderBy(a => a.Name);
            ViewBag.MainSubjectID = new SelectList(query, "MainSubjectID", "Name", selected);
        }

        private void PopulateSubjectDropDownList(object selected = null)
        {
            var query = _db.Query<Subject>().OrderBy(a => a.Name);
            ViewBag.SubjectID = new SelectList(query, "SubjectID", "Name", selected);
        }
        #endregion


        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = _db.Query<Book>().FirstOrDefault(b => b.BookID == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            PopulateAuthorDropDownList(book.AuthorID);
            PopulateCategoryDropDownList(book.CategoryID);
            PopulatePublishersDropDownList(book.PublisherID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,Title,AlternativeTitle,CreationDate,InitialPrintedYear,ActualPrintYear,Language,Material,Read,Pages,ISBN,Website,CoverLink,Rating,CodeWithinSerie,Condition,ReviewNote,Location")] Book book)
        {
            if (ModelState.IsValid)
            {
                _db.Update(book);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateAuthorDropDownList(book.AuthorID);
            PopulateCategoryDropDownList(book.CategoryID);
            PopulatePublishersDropDownList(book.PublisherID);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = _db.Query<Book>().FirstOrDefault(b => b.BookID == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = _db.Query<Book>().FirstOrDefault(b => b.BookID == id);
            _db.Remove(book);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult UploadCover(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = _db.Query<Book>().FirstOrDefault(b => b.BookID == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        public ActionResult UploadCompleted()
        {
            Book book = TempData["book"] as Book;
            return View(book);
        }

        [HttpPost]
        public ActionResult UploadCover(FormCollection formCollection, HttpPostedFileBase imagefile)
        {
            int id = Convert.ToInt32(formCollection["BookID"]);
            if (id == -1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = _db.Query<Book>().FirstOrDefault(b => b.BookID == id);
            if (book == null)
            {
                return HttpNotFound();
            }

            if (imagefile.ContentLength > 0)
            {
                // width + height will force size, care for distortion
                //Exmaple: ImageUpload imageUpload = new ImageUpload { Width = 800, Height = 700 };

                // height will increase the width proportionally
                //Example: ImageUpload imageUpload = new ImageUpload { Height= 600 };

                // width will increase the height proportionally
                ImageUpload imageUpload = new ImageUpload { Width = 300 };

                // rename, resize, and upload
                //return object that contains {bool Success,string ErrorMessage,string ImageName}
                ImageResult imageResult = imageUpload.RenameUploadFile(imagefile);
                if (imageResult.Success)
                {
                    book.ImageUrl = imageResult.ImageName;
                    _db.Update(book);
                    _db.SaveChanges();

                    TempData["book"] = book;

                    return RedirectToAction("UploadCompleted");
                }
                else
                {
                    // use imageResult.ErrorMessage to show the error
                    ViewBag.Error = imageResult.ErrorMessage;
                }
            }


            return View();
        }
    }
}
