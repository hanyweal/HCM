using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class SportsSeasonsViewModel
    {
        public int SportsSeasonID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("SportsSeasonStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? SportsSeasonStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("SportsSeasonEndDateText")]
        //[CustomCompareTwoDates("ScholarshipStartDate", true, ErrorMessage = "CompareBetweenDatesText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? SportsSeasonEndDate { get; set; }

        [CustomDisplay("MaturityYearBalanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public MaturityYearsBalancesBLL MaturityYearBalance { get; set; }

        [Range(1, 999, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        [CustomDisplay("SportsSeasonPeriodText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int SportsSeasonPeriod { get; set; }

        [CustomDisplay("SportsSeasonDescriptionText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string SportsSeasonDescription { get; set; }
    }
}