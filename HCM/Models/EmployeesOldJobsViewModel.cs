using HCM.Classes.CustomAttributes;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class EmployeesOldJobsViewModel : BaseViewModel
    {
         public virtual int EmployeeOldJobID { get; set; }

        [CustomDisplay("EmployeeCodeIDText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public virtual int EmployeeCodeID { get; set; }

        public EmployeesCodesBLL EmployeeCode { get; set; }

        [CustomDisplay("JobNameText")]
        public virtual string JobName { get; set; }

        [CustomDisplay("OrganizationNameText")]
        public virtual string OrganizationName { get; set; }

        [CustomDisplay("RankNameText")]
        public virtual string RankName { get; set; }

        [CustomDisplay("CareerDegreeNameText")]
        public virtual string CareerDegreeName { get; set; }

        [CustomDisplay("StartDateText")]
        public virtual DateTime EmployeeOldJobStartDate { get; set; }

        [CustomDisplay("EndDateText")]
        public virtual DateTime EmployeeOldJobEndDate { get; set; }

        [CustomDisplay("CreatedByText")]
        public string CreatedBy { get; set; }

        [CustomDisplay("CreatedDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public virtual DateTime CreatedDate { get; set; }

        [CustomDisplay("LastUpdatedByText")]
        public string LastUpdatedBy { get; set; }

        [CustomDisplay("LastUpdatedDateText")]
        public virtual DateTime LastUpdatedDate { get; set; }
        public string GetCreatedByDisplayed(EmployeesCodesBLL Employee)
        {
            return Employee != null ? Employee.EmployeeCodeNo + " - " + Employee.Employee.EmployeeNameAr : null;
        }
    }
}
