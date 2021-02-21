using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class OverTimesViewModel : BaseViewModel
    {
        public int OverTimeID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("OverTimeStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime OverTimeStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("OverTimeEndDateText")]
        public DateTime? OverTimeEndDate { get; set; }

        [CustomDisplay("OverTimePeriodText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int OverTimePeriod { get; set; }

        [CustomDisplay("OverTimeWeekWorkHoursAvgText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        [Range(0, 24, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        public double WeekWorkHoursAvg { get; set; }

        [CustomDisplay("FridayHoursAvgText")]
        [Range(0, 24, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        public double? FridayHoursAvg { get; set; }

        [CustomDisplay("SaturdayHoursAvgText")]
        [Range(0, 24, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        public double? SaturdayHoursAvg { get; set; }

        [CustomDisplay("OverTimeTaskText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string Tasks { get; set; }

        [CustomDisplay("RequesterText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string Requester { get; set; }

        public List<OverTimesDetailsBLL> OverTimesDetails { get; set; }

        [CustomDisplay("EmployeeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public OverTimesDetailsBLL OverTimeDetailRequest { get; set; }

        [CustomDisplay("ApproveText")]
        public bool IsApprove { get; set; }

        [CustomDisplay("AddOverTimesDaysText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string OverTimesDays { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    //if (WeekWorkHoursAvg <= .1 || WeekWorkHoursAvg>24)
        //    //        yield return new ValidationResult(Resources.Globalization.RequiredExternalCityText);
        //    //    //if (string.IsNullOrEmpty(ExternalOrganization))
        //    //    //    yield return new ValidationResult(Resources.Globalization.RequiredExternalOrganizationText);
        //}
    }
}