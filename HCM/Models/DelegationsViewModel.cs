using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class DelegationsViewModel : BaseViewModel //, IValidatableObject
    {
        public int DelegationID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("DelegationStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DelegationStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("DelegationEndDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DelegationEndDate { get; set; }

        [CustomDisplay("DelegationPeriodText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int DelegationPeriod { get; set; }

        [CustomDisplay("DelegationDistancePeriodText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int? DelegationDistancePeriod { get; set; }

        [CustomDisplay("DelegationTypeText")]
        public List<DelegationsTypesBLL> DelegationsTypes { get; set; }

        [CustomDisplay("DelegationTypeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DelegationsTypesBLL DelegationType { get; set; }

        [CustomDisplay("DelegationKindText")]
        public List<DelegationsKindsBLL> DelegationsKinds { get; set; }

        [CustomDisplay("DelegationKindText")]
        public DelegationsKindsBLL DelegationKind { get; set; }

        [CustomDisplay("DelegationReasonText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string DelegationReason { get; set; }

        [CustomDisplay("NotesText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string Notes { get; set; }

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

        [CustomDisplay("DelegationDestinationText")]
        public string DelegationDestination { get; set; }

        [CustomDisplay("EmployeeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DelegationsDetailsBLL DelegationDetailRequest { get; set; }
        [CustomDisplay("ApproveText")]
        public bool IsApprove { get; set; }

        public List<DelegationsDetailsBLL> DelegationsDetails { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (DelegationType.DelegationTypeID == Convert.ToInt32(DelegationsTypesEnum.External))
        //    {
        //        if (!CountryID.HasValue)
        //            yield return new ValidationResult(Resources.Globalization.RequiredCountryText); 
        //    }
        //    else
        //    {
        //        if (!KSACityID.HasValue)
        //            yield return new ValidationResult(Resources.Globalization.RequiredKSACityText);
        //    }
        //}

    }
}