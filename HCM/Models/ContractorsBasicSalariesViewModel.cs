using HCM.Classes.CustomAttributes;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HCM.Models
{
    public class ContractorsBasicSalariesViewModel : BaseViewModel
    {
        public int ContractorBasicSalaryID { get; set; }
        
        [CustomDisplay("BasicSalaryText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public double BasicSalary { get; set; }

        [CustomDisplay("TransfareAllowanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public double TransfareAllowance { get; set; }

        public virtual EmployeesCodesBLL EmployeeCode { get; set; }

        [CustomDisplay("EmployeesText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCodeID { get; set; }

        public EmployeesViewModel EmployeeVM { get; set; }

    }
}