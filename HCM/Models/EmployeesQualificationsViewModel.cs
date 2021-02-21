using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class EmployeesQualificationsViewModel : IValidatableObject
    {
        public int EmployeeQualificationID { get; set; }

        [CustomDisplay("QualificationDegreeNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int QualificationDegreeID { get; set; }

        [CustomDisplay("QualificationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int QualificationID { get; set; }

        [CustomDisplay("GeneralSpecializationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int GeneralSpecializationID { get; set; }

        [CustomDisplay("ExactSpecializationNameText")]
        public int ExactSpecializationID { get; set; }

        public EmployeesViewModel EmployeeVM { get; set; }

        [CustomDisplay("UniSchNameText")]
        public string UniSchName { get; set; }

        [CustomDisplay("DepartmentText")]
        public string Department { get; set; }

        [CustomDisplay("FullGPAText")]
        [Range(1, 100, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        public decimal? FullGPA { get; set; }

        [CustomDisplay("GPAText")]
        [Range(1, 100, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        public decimal? GPA { get; set; }

        [CustomDisplay("StudyPlaceText")]
        public string StudyPlace { get; set; }

        [CustomDisplay("GraduationDateText")]
        public DateTime? GraduationDate { get; set; }

        [CustomDisplay("GraduationYearText")]
        public string GraduationYear { get; set; }

        [CustomDisplay("PercentageText")]
        public decimal Percentage { get; set; }

        [CustomDisplay("QualificationTypeText")]
        public QualificationsTypesBLL QualificationType { get; set; }

        public bool HasCreatingAccess { get; set; }
        public bool HasUpdatingAccess { get; set; }
        public bool HasDeletingAccess { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!GraduationDate.HasValue && string.IsNullOrEmpty(GraduationYear))
            {
                yield return new ValidationResult(Resources.Globalization.RequiredGraduationYearOrGraduationDateText);
            }
        }
    }
}