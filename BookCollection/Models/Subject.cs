using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookCollection.Models
{
    public class Subject
    {
        public int SubjectID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}