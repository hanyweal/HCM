using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsRecordsLogsDAL : CommonEntityDAL
    {
        public int Insert(PromotionsRecordsLogs PromotionRecordLog)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PromotionsRecordsLogs.Add(PromotionRecordLog);
                    db.SaveChanges();
                    return PromotionRecordLog.PromotionRecordLogID;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsLogs> GetByPromotionRecordID(int PromotionRecordID, out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();

                var odData = db.PromotionsRecordsLogs
                                .Include("ActionByNav")
                                .Where(x => x.PromotionRecordID == PromotionRecordID);

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<PromotionsRecordsLogs> iq = odData.Where(p => p.PromotionRecordLogID != null);

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                        (p.ActionDescription.ToLower().Contains(search.ToLower()) ||
                            p.PromotionRecordNo.ToString().ToLower().Contains(search.ToLower())
                        ));
                }

                // Sorting.
                iq = iq.OrderBy(p => p.ActionDate);

                // Filter record count.
                recFilterOut = iq.Count();

                // Apply pagination.
                iq = iq.Skip(startRec).Take(pageSize);
                //Get list of data
                var data = iq.ToList();

                return data;

            }
            catch
            {
                throw;
            }
        }
    }
}