using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BookCollection.Helpers
{
    public class Converters
    {
        public static string RemoveSerieNr(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            if (input.Contains("Byzantium"))
            {
                return "Byzantium";
            }
            string output = input.Trim();
            //Replace trailing / LHR
            output = Regex.Replace(output, @"\s?/\s?(\()?(LHR|lhr)(\))?$", "");
            //Replace trailing IIIA / IIIB
            output = Regex.Replace(output, @"(\()?[IVX]+[A-Z](\))?$", "");
            //Replace druk: , - deel 1
            output = Regex.Replace(output, @"\s?-\s?(\()?\s(deel)\s[0-9]+(\))?$", "");
            //Replace yearrange: 1500-1600
            output = Regex.Replace(output, @"\s[0-9]+-[0-9]+(\))?$", "");
            //Replace druk: , 2e druk
            output = Regex.Replace(output, @"\s?,\s?(\()?[0-9]*e\sdruk(\))?$", "");
            //Replace Roman numerals on end
            output = Regex.Replace(output, @"(\()?[IXV]+(\))?$", "");
            //Replace normal numerals on end
            output = Regex.Replace(output, @"(\()?[0-9]+(\))?$", "");

            return output.Trim();
        }

        public static string ExtractSerieNrEndCleanup(string input)
        {
            return input.Replace("(", "").Replace(")", "").Replace(",", " ").Replace("-", "").Replace("  ", " ").Trim();
        }
        public static string ExtractSerieNr(string input)
        {
            // trailing / LHR
            Match m = Regex.Match(input, @"\s?/\s?(\()?(LHR|lhr)(\))?$");
            if (m.Success)
            {
                //Replace trailing / LHR
                input = Regex.Replace(input, @"\s?/\s?(\()?(LHR|lhr)(\))?$", "");
            }


            // trailing IIIA / IIIB
            m = Regex.Match(input, @"(\()?[IVX]+[A-Z](\))?$");
            if (m.Success)
                return ExtractSerieNrEndCleanup(m.Value);

            //Replace druk: , - deel 1
            m = Regex.Match(input, @"\s?-\s?(\()?\s(deel)\s[0-9]+(\))?$");
            if (m.Success)
                return ExtractSerieNrEndCleanup(m.Value);

            // druk: II, 2e druk
            m = Regex.Match(input, @"\s?(\()?(I)+(\)),??\s?(\()?[0-9]*e\sdruk(\))?$");
            if (m.Success)
                return ExtractSerieNrEndCleanup(m.Value);


            // druk: , 2e druk
            m = Regex.Match(input, @"\s?,\s?(\()?[0-9]*e\sdruk(\))?$");
            if (m.Success)
                return ExtractSerieNrEndCleanup(m.Value);

            // yearrange: 1500-1600
            m = Regex.Match(input, @"\s[0-9]+-[0-9]+(\))?$");
            if (m.Success)
            {
                string temp = m.Value.Replace("-", " t/m ");
                return ExtractSerieNrEndCleanup(temp);
            }

            // Roman numerals on end
            m = Regex.Match(input, @"(\()?[IXV]+(\))?$");
            if (m.Success)
                return ExtractSerieNrEndCleanup(m.Value);

            // normal numerals on end
            m = Regex.Match(input, @"(\()?[0-9]+(\))?$");
            if (m.Success)
                return ExtractSerieNrEndCleanup(m.Value);

            return "";

        }
    }
}