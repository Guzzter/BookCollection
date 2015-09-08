using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookCollection.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        public string Title { get; set; }
    }
}