using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsRecordsEmployeesEducationsDetailsDAL : CommonEntityDAL
    {
        public List<PromotionsRecordsEmployeesEducationsDetails> GetPromotionsRecordsEmployeesEducationsDetails()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecordsEmployeesEducationsDetails.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsEmployeesEducationsDetails> GetByPromotionRecordEmployeeID(int PromotionRecordEmployeeID)
        {
            try
            {
                var db = new HCMEntities();

                return db.PromotionsRecordsEmployeesEducationsDetails.Where(x => x.PromotionRecordEmployeeID == PromotionRecordEmployeeID).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}