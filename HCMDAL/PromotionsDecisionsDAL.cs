using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsDecisionsDAL
    {
        public List<PromotionsDecisions> GetPromotionsDecisions()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsDecisions.ToList();
            }
            catch
            {
                throw;
            }
        }

    }
}