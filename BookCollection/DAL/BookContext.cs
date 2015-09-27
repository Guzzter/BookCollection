using BookCollection.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
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
        int SaveChanges();
        void AddRange<T>(IEnumerable<T> entities) where T : class;
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
        int ExecuteSqlCommand(string sqlStatement, params object[] parameters);
        int ExecuteSqlCommand(string sqlStatement);

        DbRawSqlQuery<T> ExecuteSqlQuery<T>(string sqlStatement, params object[] parameters) where T : class;
        void LocalClear<T>() where T : class;
        T Find<T>(int? id) where T : class;
        void SetState<T>(T entity, EntityState modified) where T : class;
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

        void IBookContext.LocalClear<T>()
        {
            Set<T>().Local.Clear();
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

        int IBookContext.SaveChanges()
        {
            return SaveChanges();
        }

        IEnumerable<DbEntityValidationResult> IBookContext.GetValidationErrors()
        {
            return GetValidationErrors();
        }

        int IBookContext.ExecuteSqlCommand(string sqlStatement, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sqlStatement, parameters);
        }

        int IBookContext.ExecuteSqlCommand(string sqlStatement)
        {
            return Database.ExecuteSqlCommand(sqlStatement);
        }

        DbRawSqlQuery<T> IBookContext.ExecuteSqlQuery<T>(string sqlStatement, params object[] parameters)
        {
            return Database.SqlQuery<T>(sqlStatement, parameters);
        }

        public T Find<T>(int? id) where T : class
        {
            return Set<T>().Find(id);
        }

        public void SetState<T>(T entity, EntityState modified) where T : class
        {
            Entry(entity).State = modified;
        }
    }
}