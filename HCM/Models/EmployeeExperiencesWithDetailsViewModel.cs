using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HCM.Models;
using HCMBLL.Enums;

namespace HCM.Models
{
    public class EmployeeExperiencesWithDetailsViewModel : BaseViewModel, IValidatableObject
    {
        public virtual int EmployeeExperienceWithDetailID
        {
            get;
            set;
        }

        [CustomDisplay("FromDateHijriText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public virtual DateTime FromDate
        {
            get;
            set;
        }

        [CustomDisplay("ToDateHijriText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public virtual DateTime ToDate
        {
            get;
            set;
        }

        [CustomDisplay("FromGregDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public virtual DateTime FromGregDate
        {
            get;
            set;
        }

        [CustomDisplay("ToGregDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public virtual DateTime ToGregDate
        {
            get;
            set;
        }

        [CustomDisplay("SectorTypeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public virtual int SectorTypeID
        {
            get;
            set;
        }

        [CustomDisplay("SectorNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public virtual string SectorName
        {
            get;
            set;
        }

        [CustomDisplay("EmployeeJobNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public virtual string JobName
        {
            get;
            set;
        }

        public EmployeesViewModel EmployeeVM { get; set; }

        public SectorsTypesViewModel SectorsTypesVM { get; set; }

        public bool HasCreatingAccess { get; set; }
        public bool HasUpdatingAccess { get; set; }
        public bool HasDeletingAccess { get; set; }

        [CustomDisplay("EmployeeExperienceTotalYearsText")]
        public int TotalYears { get; set; }
        [CustomDisplay("EmployeeExperienceTotalMonthsText")]
        public int TotalMonths { get; set; }
        [CustomDisplay("EmployeeExperienceTotalDaysText")]
        public int TotalDays { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FromDate > ToDate)
            {
                yield return new ValidationResult(Resources.Globalization.ToDateEndDateValidationText);
            }
        }

    }
}