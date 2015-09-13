using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookCollection.DAL;
using System.Collections.Generic;
using BookCollection.Logging;
using Moq;
using BookCollection.Models;

namespace BookCollection.Tests.DAL
{
    [TestClass]
    public class BookSeederTest
    {

        Mock<ILogger> logger;
        Mock<ISeedDataProvider> dataProvider;
        Mock<IBookContext> bc;
        seedDataModel singleBook;

        [TestInitialize]
        public void Setup()
        {
            logger = new Mock<ILogger>();
            dataProvider = new Mock<ISeedDataProvider>();
            bc = new Mock<IBookContext>();
            singleBook = new seedDataModel()
            {
                Author = "Beltman, Guus",
                Title = "(50) Shades of MVC",
                AlternativeTitle = "50 tastes of MVC",
                Serie = "Great books of the world III",
                Publisher = "Atlas publishing",
                PrintedYears = "1900 2015",
                Type = "Romannetje",
                Code = "l051n",
                Subjects1 = "Lorem ipsum 1",
                Subjects2 = "Lorem ipsum 2",
                Contents = "Lodewijk XIV de zonnekoning van Frankrijk, Johanna de waanzinnige van Kastilië",
                CreateDate = ""
            };
        }

        [TestMethod]
        public void CleanUpCategoryTest()
        {
            // Arrange
            var bookInit = new BookSeeder(bc.Object, logger.Object, dataProvider.Object);

            // Act & Assert
            Assert.AreEqual("biografieën", bookInit.CleanUpCategory("biogr. studies"));

        }

        [TestMethod]
        public void SeedCategoriesTest()
        {
            // Arrange
            dataProvider.Setup(c => c.GetData()).Returns(new List<seedDataModel>() { singleBook });
            var bookInit = new BookSeeder(bc.Object, logger.Object, dataProvider.Object);

            // Act
            var list = bookInit.SeedCategories();

            // Assert
            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual("Roman", list[0].Title);
        }

        [TestMethod]
        public void SeedBookTest()
        {
            // Arrange
            dataProvider.Setup(c => c.GetData()).Returns(new List<seedDataModel>() { singleBook });
            var bookInit = new BookSeeder(bc.Object, logger.Object, dataProvider.Object);

            // Act
            List<Book> list = (List<Book>)bookInit.GetBooks(
                new List<Publisher>() { new Publisher() { Name = "" } },
                new List<Author> { new Author() { Lastname = "Beltman", Firstname = "Guus" } },
                new List<Category> { new Category { Title = "Roman" } },
                new List<Subject> { new Subject { Name = "Stuff" } });

            // Assert
            Assert.IsTrue(list.Count == 1);
            Assert.AreEqual("Great books of the world", list[0].Serie);
            Assert.AreEqual("III", list[0].CodeWithinSerie);

        }
    }
}
