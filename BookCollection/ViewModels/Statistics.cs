using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookCollection.ViewModels
{
    public class Statistics
    {
        public int Books { get; set; }
        public int Authors { get; set; }
        public int Categories { get; set; }
        public int Subjects { get; set; }
        public int Publishers { get; set; }

        public IEnumerable<CategoryGroup> CatGroupStats { get; set; }
    }
}