using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class EmployeesVacationsManagmentOpeningBalancesViewModel
    {
        public int VacationID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        [CustomDisplay("EmployeeText")]
        public int EmployeeCodeID { get; set; }
        //--===========================                                          ============================================--
        public VacationsTypesBLL VacationType { get; set; }
        public int? OpeningBalance { get; set; }
        //--===========================                                          =============================================--
        public int? EmployeeCareerHistoryID { get; set; }
        public VacationsActionsBLL VacationAction { get; set; }
        public DateTime? VacationStartDate { get; set; }
        public DateTime? VacationEndDate { get; set; }
        public int VacationPeriod { get; set; }
        public DateTime? WorkDate { get; set; }
        public string Notes { get; set; }
        //--==================================================================================================================--
    }
}