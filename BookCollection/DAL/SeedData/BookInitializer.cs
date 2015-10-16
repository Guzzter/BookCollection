using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookCollection.Models;
using System.Reflection;
using CsvHelper;
using System.IO;
using System.Text;
using GBUtils.Extension;
using System.Data.Entity.Validation;
using BookCollection.Helpers;
using BookCollection.Logging;

namespace BookCollection.DAL.SeedData
{

    public class BookInitializer : System.Data.Entity.CreateDatabaseIfNotExists<BookContext>
    {
        protected override void Seed(BookContext context)
        {
            var bs = new BookSeeder(context, new TraceLogger(), new CsvSeedDataProvider());
            bs.Run();
        }
    }

    public class BookSeeder
    {
        private IEnumerable<seedDataModel> _dataRows;
        private IBookContext _c;
        private ILogger _log;

        public BookSeeder(IBookContext context, ILogger logger, ISeedDataProvider data)
        {
            _c = context;
            _log = logger;
            _dataRows = data.GetData();
        }

        public void Run()
        { 
            var testLimit = 1000000;
            
            var pubs = GetPublishers().Take(testLimit);
            _c.AddRange(pubs);
            _c.SaveChanges();

            var authors = GetAuthors().Take(testLimit);
            _c.AddRange(authors);
            _c.SaveChanges();


            IEnumerable<Subject> subjects = GetSubjects().Take(testLimit);
            _c.AddRange(subjects);
            _c.SaveChanges();

            SeedCategories();

            pubs = _c.Query<Publisher>().ToList();
            authors = _c.Query<Author>().ToList();
            subjects = _c.Query<Subject>().ToList();
            var cats = _c.Query<Category>().ToList();

            IEnumerable<Book> books = GetBooks(pubs, authors, cats, subjects).Take(testLimit);
            foreach (var b in books)
            {
                try
                {
                    
                    _c.Add(b);
                    _c.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    _log.Error("Could not save book {0}, reason: {1}", b.Title, e.Message);
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        _log.Error("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            _log.Error("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    _c.LocalClear<Book>();
                }
                catch (Exception ex)
                {
                    _log.Error("Could not save book {0}, reason: {1}", b.Title, ex.Message);
                }
            }
            
        }

        public IEnumerable<Book> GetBooks(IEnumerable<Publisher> pubs, IEnumerable<Author> authors, IEnumerable<Category> cats, IEnumerable<Subject> subs)
        {
            char[] splitters = new char[] { ',', '\t'};
            var list = new List<Book>();

            foreach (var item in _dataRows)
            {
                string field = item.Title;
                if (!string.IsNullOrWhiteSpace(field))
                {
                    //if (!string.IsNullOrEmpty(item.Subjects1))
                    //{
                    //    var main = item.Subjects1.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
                    //}

                    int? initialPrint = null;
                    int? actualPrint = null;
                    if (!string.IsNullOrWhiteSpace(item.PrintedYears))
                    {
                        var jaren = item.PrintedYears.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
                        if (jaren.Length > 0)
                        {
                            try
                            {
                                initialPrint = int.Parse(jaren[0].Trim());
                                if (initialPrint == 0)
                                    initialPrint = null;
                            }
                            catch { }
                        }
                        if (jaren.Length > 1)
                        {
                            try
                            {
                                actualPrint = int.Parse(jaren[1].Trim());
                                if (actualPrint == 0)
                                    actualPrint = null;
                            }
                            catch { }

                        }
                    }

                    var catName = CleanUpCategory(item.Type);
                    var cat = cats.FirstOrDefault(c => c.Title == catName.FirstCharacterUppercaseRestLowercase());
                    if (cat == null)
                        cat = cats.FirstOrDefault(c => c.Title == "?");
                    
                    
                    DateTime date;
                    DateTime? nullDate = null;

                    if (!string.IsNullOrWhiteSpace(item.CreateDate) && DateTime.TryParse(item.CreateDate, out date)) {
                        nullDate = date;
                    }

                    var auth = authors.FirstOrDefault(a => a.OrigKey == item.Author);
                    var pub = pubs.FirstOrDefault(a => a.Name.Equals(item.Publisher, StringComparison.OrdinalIgnoreCase));
                    if (pub == null)
                    {
                        pub = pubs.FirstOrDefault(a => a.Name.Equals("?", StringComparison.OrdinalIgnoreCase));
                    }
                    //Subject mainSub = null;
                    List<Subject> subjects = new List<Subject>();
                    
                    if (!string.IsNullOrWhiteSpace(item.Subjects1))
                    {
                        var splt = item.Subjects1.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
                        //mainSub = subs.FirstOrDefault(s => s.Name.Equals(splt[0], StringComparison.OrdinalIgnoreCase));
                        
                        if (splt.Length > 0)
                        {
                            for (int i=0; i < splt.Length; i++)
                            {
                                string subTitle = splt[i].Trim();
                                if (!IsBlacklistedSubject(subTitle)) {
                                    var dbSup = subs.FirstOrDefault(s => s.Name.Equals(subTitle, StringComparison.OrdinalIgnoreCase));
                                    if (dbSup != null && !subjects.Contains(dbSup)) {
                                        subjects.Add(dbSup);
                                    }
                                }
                            }
                        }
                    }

                    string name = field.FirstCharacterUppercase();
                    if (list.Count(p => p.Title.Equals(name, StringComparison.OrdinalIgnoreCase)) == 0)
                    {
                        var b = new Book() {
                            Title = name,
                            AlternativeTitle = item.AlternativeTitle,
                            Language = GetLangFromCode(item.Code),
                            Material = GetMaterialFromCode(item.Code),
                            ActualPrintYear = actualPrint,
                            InitialPrintedYear = initialPrint,
                            Category = cat,
                            Serie = Converters.RemoveSerieNr(item.Serie),
                            CreationDate = nullDate,
                            Publisher = pub,
                            Subjects = subjects,
                            Code = item.Contents,
                            Condition = Condition.Used,
                            CodeWithinSerie = Converters.ExtractSerieNr(item.Serie),
                            Rating = 0
                        };

                        if (auth != null)
                        {
                            b.Authors = new List<Author>();
                            b.Authors.Add(auth);
                        }

                        list.Add(b);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// c = samenvatting
        /// p = pocketboeken
        /// l = luxe kaft, series
        /// z = boeken met een zachte kaft
        /// k = boeken met een los, kartonnen omhulsel
        /// b = willekeurig ander boek met een harde kaft
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Material GetMaterialFromCode(string code)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                code = code.ToLowerInvariant();
                if (code.StartsWith("c"))
                    return Material.Summary;
                if (code.StartsWith("p"))
                    return Material.Pocket;
                if (code.StartsWith("l"))
                    return Material.Deluxe;
                if (code.StartsWith("z"))
                    return Material.SoftCover;
                if (code.StartsWith("k"))
                    return Material.SeparateBox;
                if (code.StartsWith("b"))
                    return Material.HardCover;

            }
            return Material.SoftCover;
        }

        public Language GetLangFromCode(string code)
        {
            if (!string.IsNullOrWhiteSpace(code)) {
                code = code.ToLowerInvariant();
                if (code.EndsWith("e"))
                    return Language.EN;
                if (code.EndsWith("d"))
                    return Language.DE;
                if (code.EndsWith("f"))
                    return Language.FR;
                if (code.EndsWith("s"))
                    return Language.ES;
                if (code.EndsWith("i"))
                    return Language.IT;
                if (code.EndsWith("n"))
                    return Language.NL;
            }
            return Language.Other;
        }

        public IEnumerable<Publisher> GetPublishers()
        {
            var list = new List<Publisher>();

            foreach (var item in _dataRows)
            {
                string field = item.Publisher;
                if (!string.IsNullOrWhiteSpace(field))
                {
                    string name = field.FirstCharacterUppercase();
                    if (list.Count(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) == 0)
                    {
                        list.Add(new Publisher() { Name = name });
                    }
                }
            }

            list.Add(new Publisher() { Name = "?" });
            return list;
        }

        public bool IsBlacklistedSubject(string subTitle)
        {
            if (subTitle.Equals("e.a.") || subTitle.Equals("ea.") || subTitle.Equals("ea"))
                return true;
            return false;
        }

        public IEnumerable<Subject> GetSubjects()
        {
            char[] splitters = new char[] { ',' };
            var list = new List<Subject>();

            foreach (var item in _dataRows)
            {
                var splits = item.Subjects1.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
                foreach(var i in splits)
                {
                    var name = i.Trim().FirstCharacterUppercase();
                    if (!IsBlacklistedSubject(name) && list.Count(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) == 0)
                    {
                        list.Add(new Subject() { Name = name.Replace("\"", "") });
                    }
                }

                splits = item.Subjects2.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
                foreach (var i in splits)
                {
                    var name = i.Trim().FirstCharacterUppercase();
                    if (!IsBlacklistedSubject(name) && list.Count(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) == 0)
                    {
                        list.Add(new Subject() { Name = name.Replace("\"", "") });
                    }
                }

            }

            return list;
        }

        public IEnumerable<Author> GetAuthors()
        {
            var list = new List<Author>();

            foreach (var item in _dataRows)
            {
                string field = item.Author;
                if (!string.IsNullOrWhiteSpace(field))
                {
                    var fields = field.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    
                    string name = fields[0].FirstCharacterUppercase();
                    string firstName = (fields.Length > 1) ? fields[1] : ""; 
                    if (list.Count(p => p.OrigKey.Equals(field, StringComparison.OrdinalIgnoreCase)) == 0)
                    {
                        if (!name.Equals("e.a."))
                        {
                            list.Add(new Author() { OrigKey = field, Lastname = name.Replace("e.a.", string.Empty), Firstname = firstName.Replace("e.a.", string.Empty) });
                        }
                    }
                }
            }

            return list;
        }


        public List<Category> SeedCategories()
        {
            var list = new List<Category>();

            foreach (var item in _dataRows)
            {
                string field = item.Type;
                
                if (!string.IsNullOrWhiteSpace(field))
                {
                    field = CleanUpCategory(field);
                    if (list.Count(p => p.Title.Equals(field, StringComparison.OrdinalIgnoreCase)) == 0)
                    {
                        list.Add(new Category() { Title = field });
                    }
                }
            }

            //list.Add(new Category() { Title = "?" });

            list.ForEach(c => c.Title = c.Title.FirstCharacterUppercaseRestLowercase());
            _c.AddRange(list);
            _c.SaveChanges();

            return list;
        }

        public string CleanUpCategory(string orig)
        {
            if (string.IsNullOrWhiteSpace(orig))
                return "?";

            string temp = orig.ToLowerInvariant().Replace("NULL", "?")
                .Replace("romannetje", "roman")
                .Replace("anecdotes", "anekdotes")
                .Replace("biogr. studies", "biografieën")
                .Replace("korte biografie", "biografie")
                .Replace("biogr. studie", "biografie")  
                .Replace("lesmateriaal/hw", "lesmateriaal")
                .Replace("naslagatlas", "atlas")
                .Replace("novellen", "novelle")
                .Replace("reisgidsje", "reisgids")
                .Replace("schoolatlas", "atlas")
                .Replace("sutdie", "studie")
                .Replace("studies", "studie")
                .Replace("toneelstukken", "toneelstuk");

            return temp;
        }
    }
}