using HCM.Classes.CustomAttributes;
using HCMBLL;

namespace HCM.Models
{
    public class InternshipScholarshipsDetailsViewModel
    {
        [CustomDisplay("InternshipScholarshipBasicInfoText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int? InternshipScholarshipDetailID { get; set; }

        public BaseInternshipScholarshipsBLL InternshipScholarship
        {
            get;
            set;
        }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

    }
}