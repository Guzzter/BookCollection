using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookCollection.DAL
{
    public class seedDataModel
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string AlternativeTitle { get; set; }
        public string Serie { get; set; }
        public string Publisher { get; set; }

        public string PrintedYears { get; set; }
        public string Type { get; set; }

        public string Code { get; set; }
        public string Subjects1 { get; set; }
        public string Subjects2 { get; set; }

        public string Contents { get; set; }
        public string CreateDate { get; set; }
    }
}