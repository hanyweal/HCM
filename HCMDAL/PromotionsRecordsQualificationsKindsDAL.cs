using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsRecordsQualificationsKindsDAL
    {
        public List<PromotionsRecordsQualificationsKinds> GetPromotionsRecordsQualificationsKinds()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecordsQualificationsKinds.ToList();
            }
            catch
            {
                throw;
            }
        }

    }
}