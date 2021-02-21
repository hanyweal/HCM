using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class EmployeesExperiencesViewModel : BaseViewModel
    {
        public int EmployeeExperienceID { get; set; }

        [Range(0, 99, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        [CustomDisplay("TotalYearsText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int TotalYears { get; set; }

        [Range(0, 12, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        [CustomDisplay("TotalMonthsText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int TotalMonths { get; set; }

        [Range(0, 30, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        [CustomDisplay("TotalDaysText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int TotalDays { get; set; }

        public EmployeesViewModel EmployeeVM { get; set; }

        public bool HasCreatingAccess { get; set; }
        public bool HasUpdatingAccess { get; set; }
        public bool HasDeletingAccess { get; set; }
    }
}