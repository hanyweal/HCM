using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsRecordsStatusDAL
    {
        public List<PromotionsRecordsStatus> GetPromotionsRecordsStatus()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecordsStatus.ToList();
            }
            catch
            {
                throw;
            }
        }

    }
}