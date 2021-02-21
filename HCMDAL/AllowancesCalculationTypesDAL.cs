using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class AllowancesCalculationTypesDAL
    {
        public List<AllowancesCalculationTypes> GetAllowancesCalculationTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.AllowancesCalculationTypes.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
