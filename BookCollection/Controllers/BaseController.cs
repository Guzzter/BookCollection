using BookCollection.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookCollection.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IBookRepository repo;
        protected readonly BookContext db;

        protected BaseController() : this(new BookRepository(), new BookContext())
        {

        }

        protected BaseController(IBookRepository rep, BookContext bc)
        {
            repo = rep;
            repo.SetContext(bc);
            db = bc;

        }
    }
}