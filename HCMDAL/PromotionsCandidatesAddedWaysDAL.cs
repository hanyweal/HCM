using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsCandidatesAddedWaysDAL
    {
        public List<PromotionsCandidatesAddedWays> GetPromotionsCandidatesAddedWays()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsCandidatesAddedWays.ToList();
            }
            catch
            {
                throw;
            }
        }

    }
}