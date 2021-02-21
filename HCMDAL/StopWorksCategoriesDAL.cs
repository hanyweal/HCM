using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class StopWorksCategoriesDAL
    {
        public List<StopWorksCategories> GetStopWorksCategories()
        {
            try
            {
                var db = new HCMEntities();
                return db.StopWorksCategories.ToList();
            }
            catch
            {
                throw;
            }
        }

        public StopWorksCategories GetByStopWorkCategoryID(int StopWorkCategoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.StopWorksCategories.SingleOrDefault(x => x.StopWorkCategoryID.Equals(StopWorkCategoryID));
            }
            catch
            {
                throw;
            }
        }
    }
}
