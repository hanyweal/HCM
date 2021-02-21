using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class GovernmentFundsTypesViewModel
    {
        public int GovernmentFundTypeID { get; set; }

        [CustomDisplay("GovernmentFundsTypesText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string GovernmentFundTypeName { get; set; }
    }
}