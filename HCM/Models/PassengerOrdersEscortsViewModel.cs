using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class PassengerOrdersEscortsViewModel: BaseViewModel
    {
        
        [CustomDisplay("NameText")]
        public string EscortName { get; set; }
        [CustomDisplay("EmployeeIDNoText")]
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredStringLengthText")]
        public string EscortIDNo { get; set; }
        [CustomDisplay("AgeText")]
        [StringLength(2, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredStringLengthText")]
        public string EscortAge { get; set; }
        [CustomDisplay("RelativeRelationText")]
        public string EscortRelativeRelation { get; set; }
    }
}
