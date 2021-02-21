using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class ScholarshipsViewModel //: IValidatableObject
    {
        public int ScholarshipID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("ScholarshipStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? ScholarshipStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("ScholarshipEndDateText")]
        //[CustomCompareTwoDates("ScholarshipStartDate", true, ErrorMessage = "CompareBetweenDatesText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? ScholarshipEndDate { get; set; }

        [Range(1, 2000, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        [CustomDisplay("ScholarshipPeriodText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int ScholarshipPeriod { get; set; }

        [CustomDisplay("ScholarshipTypeText")]
        public ScholarshipsTypesBLL ScholarshipType { get; set; }

        [CustomDisplay("ScholarshipLocationText")]
        public string Location { get; set; }

        [CustomDisplay("ScholarshipReasonText")]
        public string ScholarshipReason { get; set; }

        [CustomDisplay("ScholarshipQualificationDegreeText")]
        public List<QualificationsDegreesBLL> QualificationsDegrees { get; set; }

        public QualificationsDegreesBLL QualificationDegree { get; set; }

        [CustomDisplay("ScholarshipQualificationText")]
        public List<QualificationsBLL> Qualifications { get; set; }

        public QualificationsBLL Qualification { get; set; }

        #region Employee
        [CustomDisplay("EmployeeCodeIDText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCodeID { get; set; }

        public EmployeesViewModel Employee { get; set; }
        #endregion

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //switch (this.ScholarshipType.ScholarshipTypeID)
        //{
        //    case (Int32)ScholarshipsTypesEnum.Internal:                     
        //            if (string.IsNullOrEmpty(Location))
        //                yield return new ValidationResult(Resources.Globalization.RequiredScholarshipLocationText); 
        //            break;

        //    case (Int32)ScholarshipsTypesEnum.External:

        //            if (string.IsNullOrEmpty(Country))
        //                yield return new ValidationResult(Resources.Globalization.RequiredScholarshipCountryText); 

        //        break;
        //    default:
        //        break;
        //}  
        //}

        [CustomDisplay("KSACityText")]
        public List<KSACitiesBLL> KSACities { get; set; }

        [CustomDisplay("KSACityText")]
        public KSACitiesBLL KSACity { get; set; }

        [CustomDisplay("ScholarshipCountryText")]
        public List<CountriesBLL> Countries { get; set; }

        [CustomDisplay("ScholarshipCountryText")]
        public CountriesBLL Country { get; set; }

        [CustomDisplay("ScholarshipActionText")]
        public ScholarshipsActionsBLL ScholarshipAction { get; set; }
    }
}