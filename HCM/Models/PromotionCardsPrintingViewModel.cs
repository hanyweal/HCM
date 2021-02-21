using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class PromotionCardsPrintingViewModel
    {
        public int PromotionCardPrintingID { get; set; }

        [CustomDisplay("EmployeeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCodeID { get; set; }
        public int PromotionPeriodID { get; set; }
        public EmployeesViewModel Employee { get; set; }

    }
}