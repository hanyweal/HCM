using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class BusinessRulesDAL
    {
        public List<BusinessRules> GetByBusinessSubCategory(int BusinessSubCategory)
        {
            try
            {
                var db = new HCMEntities();
                return db.BusinessRules.Where(x => x.BusinessSubCategoryID.Equals(BusinessSubCategory)).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
