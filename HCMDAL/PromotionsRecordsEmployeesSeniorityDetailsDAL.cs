using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsRecordsEmployeesSeniorityDetailsDAL : CommonEntityDAL
    {
        public List<PromotionsRecordsEmployeesSeniorityDetails> GetPromotionsRecordsEmployeesSeniorityDetails()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecordsEmployeesSeniorityDetails.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsEmployeesSeniorityDetails> GetByPromotionRecordEmployeeID(int PromotionRecordEmployeeID)
        {
            try
            {
                var db = new HCMEntities();

                return db.PromotionsRecordsEmployeesSeniorityDetails.Where(x => x.PromotionRecordEmployeeID == PromotionRecordEmployeeID).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}