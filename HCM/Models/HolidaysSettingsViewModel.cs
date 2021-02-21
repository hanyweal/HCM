using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class HolidaysSettingsViewModel : BaseViewModel
    {
        public int? HolidaySettingID { get; set; }

        [CustomDisplay("HolidayTypeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public HolidaysTypesBLL HolidayType { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("HolidaySettingStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? HolidaySettingStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("HolidaySettingEndDateText")]
        public DateTime? HolidaySettingEndDate { get; set; }

        [CustomDisplay("MaturityYearBalanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public MaturityYearsBalancesBLL MaturityYearBalance { get; set; }

        [CustomDisplay("HolidaySettingPeriodText")]
        public int HolidaySettingPeriod { get; set; }
    }
}