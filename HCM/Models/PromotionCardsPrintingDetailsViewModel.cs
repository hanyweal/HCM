using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class PromotionCardsPrintingDetailsViewModel
    {
        public int PromotionCardPrintingID { get; set; }
        public int EmployeeCodeID { get; set; }
        public int PromotionPeriodID {get;set;}

        [CustomDisplay("IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllowncesDetailsText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public bool? IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllowncesDetails { get; set; }

        [CustomDisplay("IsApprovedDetailsText")]
        public bool IsApprovedDetails { get; set; }
    }
}