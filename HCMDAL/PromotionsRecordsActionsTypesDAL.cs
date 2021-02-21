using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsRecordsActionsTypesDAL
    {
        public List<PromotionsRecordsActionsTypes> GetPromotionsRecordsActionsTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecordsActionsTypes.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
