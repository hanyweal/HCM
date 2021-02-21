using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class AllowancesAmountTypesDAL
    {
        public List<AllowancesAmountTypes> GetAllowancesAmountTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.AllowancesAmountTypes.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
