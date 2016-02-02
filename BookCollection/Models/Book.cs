using BookCollection.Helpers;
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

        #region Unmapped helper field
        /*
        [NotMapped]
        public string MainAuthor
        {
            get
            {
                Author main = Authors == null ? null : Authors.FirstOrDefault();
                return main == null ? string.Empty : main.Fullname;
            }
        }

        [NotMapped]
        public int MainAuthorID
        {
            get
            {
                Author main = Authors == null ? null : Authors.FirstOrDefault();
                return main == null ? 0 : main.AuthorID;
            }
        }
        */
        #endregion

        [Display(Name = "Alternative Title")]
        public string AlternativeTitle { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Create Date")]
        [UIHint("DatePicker")]
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

        [Required]
        public Condition Condition { get; set; }

        public string Location { get; set; }

        [Range(0,5)]
        public int? Rating { get; set; }

        public string Serie { get; set; }
        public string CodeWithinSerie { get; set; }
        public string Code { get; set; }
        public string SortField { get; set; }
        public bool Read { get; set; }
        public int? Pages { get; set; }

        [RegularExpression(Validators.ISBNREGEX, ErrorMessage = "ISBN is not valid, example: '1-901259-09-9'")]
        public string ISBN { get; set; }

        [DataType(DataType.Url)]
        public string Website { get; set; }

        public string CoverLink { get; set; }

        public string ReviewNote { get; set; }
        public int CategoryID { get; set; }
        public int PublisherID { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
        public virtual Publisher Publisher { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}