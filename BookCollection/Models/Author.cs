using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookCollection.Models
{
    public class Author
    {
        public int AuthorID { get; set; }

        public string OrigKey { get; set; }
        [Required]
        public string Lastname { get; set; }
        public string Firstname { get; set; }

        [NotMapped]
        public string Fullname {
            get {
                return (Lastname + "," + Firstname).Trim(',');
            }
        }

        public virtual ICollection<Book> Books { get; set; }

    }
}