using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;

namespace HCM.Models
{
    public class BaseViewModel
    {
        [CustomDisplay("CreatedDateText")]
        public DateTime CreatedDate { get; set; }

        [CustomDisplay("CreatedByText")]
        public string CreatedBy { get; set; }

        public string GetCreatedByDisplayed(EmployeesCodesBLL Employee)
        {
            return Employee != null ? Employee.EmployeeCodeNo + " - " + Employee.Employee.EmployeeNameAr : null;
        }
    }
}