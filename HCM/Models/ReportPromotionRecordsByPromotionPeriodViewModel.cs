using HCM.Classes.CustomAttributes;

namespace HCM.Models
{
    public class ReportPromotionRecordsByPromotionPeriodViewModel
    {
        [CustomDisplay("PromotionPeriodNameText")]
        public int PeriodID { get; set; }
    }
}