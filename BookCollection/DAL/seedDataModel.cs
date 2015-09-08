using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookCollection.DAL
{
    public class seedDataModel
    {
        public string Auteur { get; set; }
        public string Titel { get; set; }
        public string AlterTitel { get; set; }
        public string Serie { get; set; }
        public string Uitgever { get; set; }

        public string Jaren2 { get; set; }
        public string Type { get; set; }

        public string Code { get; set; }
        public string Onderwerp1 { get; set; }
        public string Onderwerp2 { get; set; }

        public string Inhoud { get; set; }
        public string InvoerDatum { get; set; }
    }
}