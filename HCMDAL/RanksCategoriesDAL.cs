using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class RanksCategoriesDAL
    {
        public List<RanksCategories> GetRanksCategories()
        {
            try
            {
                var db = new HCMEntities();
                return db.RanksCategories.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
