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

namespace BookCollection.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(IBookRepository rep, IBookContext bc) : base(rep, bc) { /* constructor forward to BaseController */ }

        // GET: Categories
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, bool noPaging = false)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var cats = from s in db.Query<Category>()
                             select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                cats = cats.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    cats = cats.OrderByDescending(s => s.Title);
                    break;
                default:  // Name ascending 
                    cats = cats.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(cats.ToPagedList(pageNumber, pageSize));
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Category category = db.Find<Category>(id);
            if (category == null)
            {
                return HttpNotFound();
            }else
            {
                category.Books = db.Query<Book>().Where(b => b.CategoryID == category.CategoryID).ToList();
            }
            return View(category);
        }

        public ActionResult DetailsForName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction("Index");
            }
            Category category = db.Query<Category>().FirstOrDefault(b => b.Title == name);
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                category.Books = db.Query<Book>().Where(b => b.CategoryID == category.CategoryID).ToList();
            }
            return View("Details", category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,Title")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Find<Category>(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,Title")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.SetState(category, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Find<Category>(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Find<Category>(id);
            db.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
