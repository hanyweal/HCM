using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class EndOfServicesViewModel : BaseViewModel
    {
        public int EndOfServiceID { get; set; }

        [CustomDisplay("EndOfServiceDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        //[CustomCompareTwoDates("EndOfServiceDecisionDate", true, ErrorMessage = "CompareBetweenDatesText")]
        public DateTime EndOfServiceDate { get; set; }

        [CustomDisplay("EmployeesText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCareerHistoryID { get; set; }

        [CustomDisplay("EndOfServiceDecisionNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string EndOfServiceDecisionNo { get; set; }

        [CustomDisplay("EndOfServiceDecisionDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime EndOfServiceDecisionDate { get; set; }

        [CustomDisplay("EndOfServiceCaseText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EndOfServiceCaseID { get; set; }

        [CustomDisplay("EndOfServiceReasonText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EndOfServiceReasonID { get; set; }

        public EndOfServicesReasonsBLL EndOfServiceReason { get; set; }

        public EmployeesViewModel EmployeeVM { get; set; }

        #region EndOfServicesVacations

        [CustomDisplay("VacationStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? EndOfServiceVacationStartDate { get; set; }

        [CustomDisplay("VacationEndDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? EndOfServiceVacationEndDate { get; set; }

        public VacationsTypesBLL EndOfServiceVacationType { get; set; }

        // public EndOfServicesVacationsBLL EndOfServiceVacation { get; set; }

        #endregion
    }
}