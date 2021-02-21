using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class InsteadDeportationsViewModel : BaseViewModel
    {
        public int InsteadDeportationID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("DeportationDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime DeportationDate { get; set; }

        public EmployeesCareersHistoryBLL EmployeeCareersHistory { get; set; }

        [CustomDisplay("EmployeeCodeIDText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCodeID { get; set; }
        public int? EmployeeCareerHistoryID { get; set; }


        [CustomDisplay("NotesText")]
        public string Note { get; set; }

        [CustomDisplay("InsteadDeportationAmountText")]
        [Range(1, 100000, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        public double? Amount { get; set; }
    }
}