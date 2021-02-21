using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class KSARegionsDAL
    {
        public List<KSARegions> GetKSARegions()
        {
            try
            {
                var db = new HCMEntities();
                return db.KSARegions.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
