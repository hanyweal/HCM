using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class StopWorksTypesDAL
    {
        public List<StopWorksTypes> GetStopWorksTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.StopWorksTypes.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<StopWorksTypes> GetByStopWorkCategoryID(int StopWorkCategoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.StopWorksTypes.Include("StopWorksCategories").Where(x => x.StopWorkCategoryID == StopWorkCategoryID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public StopWorksTypes GetByStopWorkTypeID(int StopWorkTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.StopWorksTypes.Include("StopWorksCategories").SingleOrDefault(x => x.StopWorkTypeID.Equals(StopWorkTypeID));
            }
            catch
            {
                throw;
            }
        }
    }
}
