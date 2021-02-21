using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class ScholarshipsExtentionViewModel
    {
        public int ScholarshipID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("FromDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("ToDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? ToDate { get; set; }
        public int ScholarshipTypeID { get; set; }

        [CustomDisplay("NotesText")]
        public string Notes { get; set; }

        [Range(1, 999, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        [CustomDisplay("ExtentionPeriodText")]
        public int ExtentionPeriod { get; set; }

        public int EmployeeCodeID { get; set; }

        public CountriesBLL Country { get; set; }

        public KSACitiesBLL KSACity { get; set; }

        public QualificationsBLL Qualification { get; set; }

        public EmployeesViewModel Employee { get; set; }
    }
}