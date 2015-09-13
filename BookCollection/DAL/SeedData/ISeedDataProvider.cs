using System.Collections.Generic;

namespace BookCollection.DAL
{
    public interface ISeedDataProvider
    {
        IEnumerable<seedDataModel> GetData();
    }
}