using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class EmployeesAllowancesViewModel : BaseViewModel
    {
        public int EmployeeAllowanceID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("AllowanceStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime AllowanceStartDate { get; set; }

        public AllowancesBLL Allowance { get; set; }

        [CustomDisplay("AllowanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int AllowanceID { get; set; }

        public EmployeesCareersHistoryBLL EmployeeCareersHistory { get; set; }

        [CustomDisplay("EmployeeCodeIDText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCodeID { get; set; }
        public int EmployeeCareerHistoryID { get; set; }


        [CustomDisplay("IsActiveText")]
        public bool IsActive { get; set; }
    }
}