using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsRecordsEmployeesEvaluationsDetailsDAL : CommonEntityDAL
    {
        public List<PromotionsRecordsEmployeesEvaluationsDetails> GetPromotionsRecordsEmployeesEvaluationsDetails()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecordsEmployeesEvaluationsDetails.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsEmployeesEvaluationsDetails> GetByPromotionRecordEmployeeID(int PromotionRecordEmployeeID)
        {
            try
            {
                var db = new HCMEntities();

                return db.PromotionsRecordsEmployeesEvaluationsDetails.Where(x => x.PromotionRecordEmployeeID == PromotionRecordEmployeeID).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}