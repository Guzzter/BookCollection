using BookCollection.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BookCollection.DAL
{
    
    public class BookContext : DbContext
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
    }
}