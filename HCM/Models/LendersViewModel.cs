using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class LendersViewModel : BaseViewModel 
    {
        #region Employee
        [CustomDisplay("EmployeeCodeIDText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCareerHistoryID { get; set; }

        public int? EmployeeCodeID { get; set; }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public EmployeesViewModel Employee { get; set; }
        #endregion

        public int LenderID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("LenderStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime LenderStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("LenderEndDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        [CustomCompareTwoDates("LenderStartDate", true, ErrorMessage = "CompareBetweenDatesText")]
        public DateTime LenderEndDate { get; set; }

        [CustomDisplay("LenderOrganizationText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string LenderOrganization { get; set; }

        [CustomDisplay("KSACityText")]
        public KSACitiesBLL KSACity { get; set; }

        [CustomDisplay("LenderIsFinishedText")]
        public bool IsFinished { get; set; }

        [CustomDisplay("LenderEndReasonText")]
        public string LenderEndReason { get; set; }
          
    }
}