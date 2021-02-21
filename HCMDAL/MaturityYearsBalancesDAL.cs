using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class MaturityYearsBalancesDAL
    {
        public List<MaturityYearsBalances> GetMaturityYearsBalances()
        {
            using (var db = new HCMEntities())
            {
                return db.MaturityYearsBalances.OrderBy(x => x.MaturityYear).ToList();
            }
        }
    }
}