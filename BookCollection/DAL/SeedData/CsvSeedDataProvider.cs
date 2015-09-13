using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace BookCollection.DAL
{
    public class CsvSeedDataProvider : ISeedDataProvider
    {
        public IEnumerable<seedDataModel> GetData()
        {
            var dataRows = new List<seedDataModel>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "BookCollection.DAL.SeedData.madbooks_seeddata.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.IsHeaderCaseSensitive = false;
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.Configuration.SkipEmptyRecords = true;
                    csvReader.Configuration.TrimFields = true;
                    csvReader.Configuration.TrimHeaders = true;
                    csvReader.Configuration.Delimiter = ";";

                    dataRows = csvReader.GetRecords<seedDataModel>().ToList();
                }
            }

            return dataRows;
        }
    }

}