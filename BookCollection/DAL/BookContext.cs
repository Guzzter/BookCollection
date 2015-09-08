using BookCollection.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BookCollection.DAL
{
    public interface IBookContext : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        void SaveChanges();
        void AddRange<T>(IEnumerable<T> entities) where T : class;
    }

    public class BookContext : DbContext, IBookContext
    {
        public BookContext() : base("BookCollectionCS")
        {

        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Only need when no explicit model is created
            //Many to Many relationships: intermediate table BookAuthors
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors).WithMany(a => a.Books)
                .Map(t => t.MapLeftKey("BookID")
                    .MapRightKey("AuthorID")
                    .ToTable("BookAuthors"));

            //Many to Many relationships: intermediate table BookSubjects
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Subjects).WithMany(s => s.Books)
                .Map(t => t.MapLeftKey("BookID")
                    .MapRightKey("SubjectID")
                    .ToTable("BookSubjects"));

        }

        IQueryable<T> IBookContext.Query<T>()
        {
            return Set<T>();
        }

        void IBookContext.Add<T>(T entity)
        {
            Set<T>().Add(entity);
        }

        void IBookContext.AddRange<T>(IEnumerable<T> entities)
        {
            Set<T>().AddRange(entities);
        }

        void IBookContext.Update<T>(T entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        void IBookContext.Remove<T>(T entity)
        {
            Set<T>().Remove(entity);
        }

        void IBookContext.SaveChanges()
        {
            SaveChanges();
        }
    }
}