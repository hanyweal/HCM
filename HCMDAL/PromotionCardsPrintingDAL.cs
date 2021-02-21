using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionCardsPrintingDAL
    {
        public int Insert(PromotionCardsPrinting PromotionCardsPrinting)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PromotionCardsPrinting.Add(PromotionCardsPrinting);
                    db.SaveChanges();
                    return PromotionCardsPrinting.PromotionCardPrintingID;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionCardsPrinting> GetPromotionCardsPrintingByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionCardsPrinting.Where(x => x.EmployeeCodeID.Equals(EmployeeCodeID)).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionCardsPrinting> GetPromotionCardsPrintingByPromotionPeriodID(int PromotionPeriodID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionCardsPrinting.Where(x => x.PromotionPeriodID.Equals(PromotionPeriodID)).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
