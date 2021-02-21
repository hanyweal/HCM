using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class GovernmentDeductionsTypesDAL
    {
        public List<GovernmentDeductionsTypes> GetGovernmentDeductionTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.GovernmentDeductionsTypes.ToList();
            }
            catch
            {
                throw;
            }
        }

        public GovernmentDeductionsTypes GetByGovernmentDeductionTypeID(int GovernmentDeductionTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.GovernmentDeductionsTypes.SingleOrDefault(x => x.GovernmentDeductionTypeID.Equals(GovernmentDeductionTypeID));
            }
            catch
            {
                throw;
            }
        }
    }
}
