using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class ScholarshipsJoiningViewModel
    {
        public int ScholarshipID { get; set; }
        public int ScholarshipTypeID { get; set; }

        [CustomDisplay("NotesText")]
        public string Notes { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("ScholarshipStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? ScholarshipStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("ScholarshipEndDateText")]
        //[CustomCompareTwoDates("ScholarshipStartDate", true, ErrorMessage = "CompareBetweenDatesText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? ScholarshipEndDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("ScholarshipJoinDateText")]
        //[CustomCompareTwoDates("ScholarshipStartDate", true, ErrorMessage = "CompareBetweenDatesText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? ScholarshipJoinDate { get; set; }

        [CustomDisplay("ScholarshipPeriodText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int ScholarshipPeriod { get; set; }

        public int EmployeeCodeID { get; set; }

        public CountriesBLL Country { get; set; }

        public KSACitiesBLL KSACity { get; set; }
        public QualificationsBLL Qualification { get; set; }

        [CustomDisplay("IsPassedText")]
        public bool IsPassed { get; set; }
    }
}