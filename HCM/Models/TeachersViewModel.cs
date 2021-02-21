using HCM.Classes.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class TeachersViewModel
    {
        public int TeacherID { get; set; }

        [CustomDisplay("EmployeeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int? EmployeeCareerHistoryID { get; set; }
        public int? EmployeeCodeID { get; set; }

        [CustomDisplay("StartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime StartDate { get; set; }

        [CustomDisplay("EndDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime EndDate { get; set; }

        public EmployeesViewModel Employee { get; set; }

    }
}