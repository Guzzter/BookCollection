using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookCollection.Models
{
    

    public class Book
    {
        public Book()
        {
            CreationDate = DateTime.Now;
            Material = Material.SoftCover;
            Language = Language.NL;
            Read = true;
        }

        public int BookID { get; set; }
        [Required]
        public string Title { get; set; }

        [NotMapped]
        public string MainAuthor { get
            {
                Author main = Authors.FirstOrDefault();
                if (main == null)
                {
                    return string.Empty;
                }
                else
                {
                    return main.Fullname;
                }
            }
        }
        

        [Display(Name = "Alternative Title")]
        public string AlternativeTitle { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Create Date")]
        public DateTime? CreationDate { get; set; }

        [Display(Name = "Init Print Year")]
        [Range(1455, 2030)]
        public int? InitialPrintedYear { get; set; }
        [Display(Name = "Actual Print Year")]
        [Range(1455, 2030)]
        public int? ActualPrintYear { get; set; }

        [Required]
        
        public Language Language { get; set; }
        [Required]
        public Material Material { get; set; }

        public string Serie { get; set; }
        public string Code { get; set; }
        public bool Read { get; set; }
        public int Pages { get; set; }
        public string ISBN { get; set; }

        [DataType(DataType.Url)]
        public string Website { get; set; }

        public string CoverLink { get; set; }

        public int AuthorID { get; set; }
        public int CategoryID { get; set; }
        public int PublisherID { get; set; }
        public int MainSubjectID { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
        public virtual Publisher Publisher { get; set; }

        public virtual Category Category { get; set; }

        public virtual Subject MainSubject { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }





    }
}