using BookCollection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookCollection.ViewModels
{
    /// <summary>
    /// Book has a view model to create/edit actions. Needed for handling many-to-many relationships (subjects and authors)
    /// </summary>
    public class BookViewModel
    {
        public Book Book { get; set; }

        public IEnumerable<SelectListItem> AllSubjects { get; set; }

        private List<int> _selectedSubjects;
        public List<int> SelectedSubjects
        {
            get
            {
                if (_selectedSubjects == null && Book.Subjects != null)
                {
                    _selectedSubjects = Book.Subjects.Select(m => m.SubjectID).ToList();
                }
                return _selectedSubjects;
            }
            set { _selectedSubjects = value; }
        }

        public IEnumerable<SelectListItem> AllAuthors { get; set; }

        private List<int> _selectedAuthors;
        public List<int> SelectedAuthors
        {
            get
            {
                if (_selectedAuthors == null && Book.Authors != null)
                {
                    _selectedAuthors = Book.Authors.Select(m => m.AuthorID).ToList();
                }
                return _selectedAuthors;
            }
            set { _selectedAuthors = value; }
        }

    }
}