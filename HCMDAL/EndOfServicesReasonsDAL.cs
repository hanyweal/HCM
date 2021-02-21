using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class EndOfServicesReasonsDAL
    {
        public List<EndOfServicesReasons> GetEndOfServicesReasons()
        {
            try
            {
                var db = new HCMEntities();
                return db.EndOfServicesReasons.Include("EndOfServicesCases").ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EndOfServicesReasons> GetByEndOfServiceCaseID(int EndOfServiceCaseID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EndOfServicesReasons.Include("EndOfServicesCases")
                                .Where(c => c.EndOfServiceCaseID == EndOfServiceCaseID).ToList();
            }
            catch
            {
                throw;
            }
        }

    }
}
