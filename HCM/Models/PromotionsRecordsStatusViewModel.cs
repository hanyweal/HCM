using HCM.Classes.CustomAttributes;

namespace HCM.Models
{
    public class PromotionsRecordsStatusViewModel
    {
        public int PromotionRecordStatusID { get; set; }

        [CustomDisplay("PromotionRecordStatusNameText")]
        public string PromotionRecordStatusName { get; set; }

    }
}