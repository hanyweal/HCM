using HCM.Classes.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class CareerHistoryViewModel : BaseViewModel
    {
        public int EmployeeCodeID { get; set; }

        public int EmployeeCareerHistoryID { get; set; }

        [CustomDisplay("JobNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int OrganizationJobID { get; set; }

        [CustomDisplay("CareerDegreeNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int CareerDegreeID { get; set; }

        [CustomDisplay("CareerHistoryTypeNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int CareerHistoryTypeID { get; set; }

        [CustomDisplay("JoinDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? JoinDate { get; set; }
    }
}