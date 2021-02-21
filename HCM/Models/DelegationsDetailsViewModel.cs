using HCM.Classes.CustomAttributes;
using HCMBLL;

namespace HCM.Models
{
    public class DelegationsDetailsViewModel
    {
        [CustomDisplay("DelegationBasicInfoText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int? DelegationDetailID { get; set; }

        public BaseDelegationsBLL Delegation
        {
            get;
            set;
        }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }
    }
}