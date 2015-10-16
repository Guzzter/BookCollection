using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BookCollection.Helpers
{
    public class Validators
    {
        public const string ISBNREGEX = @"^(ISBN\s)?(?=[-0-9xX ]{13}$)(?:[0-9]+[- ]){3}[0-9]*[xX0-9]$";
        public static bool IsValidIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return false;
            }
            return Regex.IsMatch(isbn, ISBNREGEX);
        }
    }
}