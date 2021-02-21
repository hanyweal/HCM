using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class VacationsViewModel : CommonPrivilagesEntity
    {
        public int VacationID { get; set; }

        [CustomDisplay("EmployeeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int? EmployeeCareerHistoryID { get; set; }

        public int? EmployeeCodeID { get; set; }

        public string EmployeeCodeNo { get; set; }

        [CustomDisplay("VacationTypeText")]
        public VacationsTypesBLL VacationType { get; set; }

        [CustomDisplay("VacationActionText")]
        public VacationsActionsBLL VacationAction { get; set; }

        [CustomDisplay("SickVacationTypeText")]
        public SickVacationsTypesBLL SickVacationType { get; set; }

        [CustomDisplay("VacationStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? VacationStartDate { get; set; }

        [CustomDisplay("VacationEndDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? VacationEndDate { get; set; }

        [CustomDisplay("StudiesVacationStartDateText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? StudiesVacationStartDate { get; set; }

        [Range(1, 9999, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        [CustomDisplay("VacationPeriodText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int? VacationPeriod { get; set; }

        [CustomDisplay("WorkDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? WorkDate { get; set; }

        [CustomDisplay("NotesText")]
        public string Notes { get; set; }

        #region HolidaySetting
        [CustomDisplay("MaturityYearBalanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int MaturityYearID { get; set; }

        [CustomDisplay("HolidayTypeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int HolidayTypeID { get; set; }

        public int HolidayAttendanceDetailID { get; set; }
        #endregion

        public int? SportsSeasonID { get; set; }

        [CustomDisplay("MaturityYearBalanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int SportsSeasonMaturityYearID { get; set; }

        [CustomDisplay("NormalVacationTypeText")]
        public int? NormalVacationTypeID { get; set; }

        public bool IsSickLeaveAmountPaid { get; set; }

        public EmployeesViewModel EVacationAuthorizedPerson { get; set; }
    }
}