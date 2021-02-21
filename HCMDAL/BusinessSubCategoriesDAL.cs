using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class BusinessSubCategoriesDAL
    {
        public List<BusinessSubCategories> GetBusinessSubCategories()
        {
            try
            {
                var db = new HCMEntities();
                return db.BusinessSubCategories.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
