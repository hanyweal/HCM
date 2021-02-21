using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class PromotionsRecordsViewModel
    {
        public int? PromotionRecordID { get; set; }

        [CustomDisplay("YearText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int YearID { get; set; }

        [CustomDisplay("PromotionRecordPeriodNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int PromotionPeriodID { get; set; }

        public PromotionsPeriodsBLL PromotionPeriod { get; set; }

        [CustomDisplay("RankNameText")]
        // [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string RankName { get; set; }

        [CustomDisplay("RankNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int RankID { get; set; }

        [CustomDisplay("JobCategoryNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int JobCategoryID { get; set; }

        [CustomDisplay("PromotionRecordDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime PromotionRecordDate { get; set; }

        [CustomDisplay("PromotionRecordNoText")]
        public int? PromotionRecordNo { get; set; }

        [CustomDisplay("PromotionRecordStatusText")]
        public PromotionsRecordsStatusViewModel PromotionRecordStatus { get; set; }

        public int PromotionRecordToolbarID { get; set; }
    }
}