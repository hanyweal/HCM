using HCM.Classes.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class PromotionsPeriodsViewModel
    {
        [CustomDisplay("YearText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int YearID { get; set; }

        [CustomDisplay("PromotionRecordPeriodNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int PromotionPeriodID { get; set; }

        //[CustomDisplay("YearText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        //public MaturityYearsBalancesBLL Year { get; set; }

        //[CustomDisplay("PromotionRecordPeriodNameText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        //public PeriodsBLL Period { get; set; }

        [CustomDisplay("PromotionPeriodNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int PeriodID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("PromotionStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime PromotionStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("PromotionEndDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime PromotionEndDate { get; set; }

        [CustomDisplay("IsActiveText")]
        public bool IsActive { get; set; }

        public int MaturityYear { get; set; }
        public string PeriodName { get; set; }
    }
}