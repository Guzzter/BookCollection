using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookCollection;
using BookCollection.Controllers;
using BookCollection.DAL;
using BookCollection.Logging;
using Moq;
using BookCollection.Models;
using BookCollection.ViewModels;

namespace BookCollection.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        Mock<IBookContext> _bookContext = new Mock<IBookContext>();
        Mock<IBookRepository> _bookRepo = new Mock<IBookRepository>();
        Mock<ILogger> _log = new Mock<ILogger>();

        [TestInitialize]
        public void Init()
        {
            _bookContext = new Mock<IBookContext>();
            _bookRepo = new Mock<IBookRepository>();
            _log = new Mock<ILogger>();

        }
        
        /// <summary>
        /// Generic helper to create a IQueryable list of certain type
        /// </summary>
        /// <typeparam name="T">Type needed</typeparam>
        /// <param name="count">Amount of items needed</param>
        /// <param name="prefix">Prefix title that can be used in a Name, Lastname or Title property to identify a unique item</param>
        /// <returns></returns>
        private IQueryable<T> GetDummies<T>(int count, string prefix = null)
        {
            var items = new List<T>();
            if (string.IsNullOrWhiteSpace(prefix))
            {
                prefix = typeof(T).Name;
            }
            for (int i = 1; i <= count; i++)
            {
                var obj = Activator.CreateInstance<T>();
                if (typeof(T).GetProperty("Title") != null)
                {
                    typeof(T).GetProperty("Title").SetValue(obj, prefix + " " + i);
                }
                if (typeof(T).GetProperty("Name") != null)
                {
                    typeof(T).GetProperty("Name").SetValue(obj, prefix + " " + i);
                }
                if (typeof(T).GetProperty("Lastname") != null)
                {
                    typeof(T).GetProperty("Lastname").SetValue(obj, prefix + " " + i);
                }

                items.Add(obj);
            }

            return items.ToArray().AsQueryable();
        }


        [TestMethod]
        public void Index()
        {
            var books = GetDummies<Book>(5, "Book Title");
            /*_bookContext.Setup(bc => bc.Query<Book>()).Returns(books);
            var authors = GetDummies<Author>(3);
            _bookContext.Setup(bc => bc.Query<Author>()).Returns(authors);
            var subjects = GetDummies<Subject>(5);
            _bookContext.Setup(bc => bc.Query<Subject>()).Returns(subjects);
            var pubs = GetDummies<Publisher>(2);
            _bookContext.Setup(bc => bc.Query<Publisher>()).Returns(pubs);
            */
            _bookRepo.Setup(br => br.GetMostRecentBooks(5)).Returns(books.Take(5));

            // Arrange
            HomeController controller = new HomeController(_bookRepo.Object, _bookContext.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            var tagCloud = result.ViewBag.CatTagCloud as IEnumerable<CategoryGroup>;
            Assert.IsNotNull(tagCloud);

            var serieTagCloud = result.ViewBag.SerieTagCloud as IEnumerable<CategoryGroup>;
            Assert.IsNotNull(serieTagCloud);

            var mostRecent = (result.ViewBag.MostRecent as IEnumerable<Book>).ToList();
            Assert.IsNotNull(mostRecent);
            Assert.AreEqual(5, mostRecent.Count());
            Assert.AreEqual("Book Title 1", mostRecent[0].Title);

        }

        /*[TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }
        */

    }
}
