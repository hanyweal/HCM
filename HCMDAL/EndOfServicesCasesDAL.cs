using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class EndOfServicesCasesDAL
    {
        public List<EndOfServicesCases> GetEndOfServicesCases()
        {
            try
            {
                var db = new HCMEntities();
                return db.EndOfServicesCases.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
