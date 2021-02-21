using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class PromotionsRecordsEmployeesViewModel
    {
        public int PromotionRecordEmployeeID { get; set; }

        public bool IsDeserveExtraBonus { get; set; }

        public bool IsRemovedAfterIncluding { get; set; }

        [CustomDisplay("RemovingReasonText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string RemovingReason { get; set; }

        public int RemovingBy { get; set; }

    }
}