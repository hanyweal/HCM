using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class StopWorksViewModel : BaseViewModel, IValidatableObject
    {
        public int StopWorkID { get; set; }

        [CustomDisplay("EmployeeCodeIDText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCareerHistoryID { get; set; }

        public int? EmployeeCodeID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("StopWorkStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime StopWorkStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("StopWorkEndDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? StopWorkEndDate { get; set; }

        [CustomDisplay("StartStopWorkDecisionNumberText")]
        public string StartStopWorkDecisionNumber { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("StartStopWorkDecisionDateText")]
        public DateTime? StartStopWorkDecisionDate { get; set; }

        [CustomDisplay("EndStopWorkDecisionNumberText")]
        public string EndStopWorkDecisionNumber { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("EndStopWorkDecisionDateText")]
        public DateTime? EndStopWorkDecisionDate { get; set; }

        [CustomDisplay("IsConvictedText")]
        public bool? IsConvicted { get; set; }

        [CustomDisplay("StopPointText")]
        public string StopPoint { get; set; }

        [CustomDisplay("NotesText")]
        public string Note { get; set; }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public EmployeesViewModel Employee { get; set; }

        [CustomDisplay("StopWorksCategoriesText")]
        public List<StopWorksCategoriesBLL> StopWorksCategories { get; set; }

        [CustomDisplay("StopWorkCategoryText")]
        public StopWorksCategoriesBLL StopWorkCategory { get; set; }

        [CustomDisplay("StopWorksTypesText")]
        public List<StopWorksTypesBLL> StopWorksTypes { get; set; }

        [CustomDisplay("StopWorkTypeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public StopWorksTypesBLL StopWorkType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsConvicted.HasValue)
            {
                if (!StopWorkEndDate.HasValue)
                    yield return new ValidationResult(Resources.Globalization.StopWorkEndDateText, new[] { "IsConvicted" });
            }
        }
    }
}