using HCM.Classes.CustomAttributes;
using HCMBLL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class AllowancesViewModel
    {
        public int? AllowanceID { get; set; }

        [CustomDisplay("AllowanceNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string AllowanceName { get; set; }

        [CustomDisplay("AllowanceAmountTypeText")]
        public List<AllowancesAmountTypesBLL> AllowancesAmountTypes { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public AllowancesAmountTypesBLL AllowanceAmountType { get; set; }


        [CustomDisplay("AllowanceCalculationTypeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public AllowancesCalculationTypesBLL AllowanceCalculationType { get; set; }

        [CustomDisplay("AllowanceAmountText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public double AllowanceAmount { get; set; }

        [CustomDisplay("IsActiveText")]
        public bool IsActive { get; set; }
    }
}