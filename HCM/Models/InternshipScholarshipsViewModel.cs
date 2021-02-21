using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class InternshipScholarshipsViewModel : BaseViewModel
    {
        public int InternshipScholarshipID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("InternshipScholarshipStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime InternshipScholarshipStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("InternshipScholarshipEndDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime InternshipScholarshipEndDate { get; set; }

        [CustomDisplay("InternshipScholarshipPeriodText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int InternshipScholarshipPeriod { get; set; }

        [CustomDisplay("InternshipScholarshipLocationText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string InternshipScholarshipLocation { get; set; }

        [CustomDisplay("InternshipScholarshipTypeText")]
        public List<InternshipScholarshipsTypesBLL> InternshipScholarshipsTypes { get; set; }

        [CustomDisplay("InternshipScholarshipTypeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public InternshipScholarshipsTypesBLL InternshipScholarshipType { get; set; }

        [CustomDisplay("InternshipScholarshipReasonText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string InternshipScholarshipReason { get; set; }

        [CustomDisplay("CountryText")]
        public List<CountriesBLL> Countries { get; set; }

        [CustomDisplay("CountryText")]
        public CountriesBLL Country { get; set; }

        [CustomDisplay("KSARegionText")]
        public List<KSARegionsBLL> KSARegions { get; set; }

        [CustomDisplay("KSARegionText")]
        public KSARegionsBLL KSARegion { get; set; }

        [CustomDisplay("KSACityText")]
        public List<KSACitiesBLL> KSACities { get; set; }

        [CustomDisplay("KSACityText")]
        public KSACitiesBLL KSACity { get; set; }

        [CustomDisplay("EmployeeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public InternshipScholarshipsDetailsBLL InternshipScholarshipDetailRequest { get; set; }

        public List<InternshipScholarshipsDetailsBLL> InternshipScholarshipsDetails { get; set; }
    }
}