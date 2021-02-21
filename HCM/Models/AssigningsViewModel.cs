using HCM.Classes.CustomAttributes;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class AssigningsViewModel : BaseViewModel, IValidatableObject
    {
        #region Employee

        [CustomDisplay("EmployeeCareerHistoryIDText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCareerHistoryID { get; set; }

        public EmployeesViewModel Employee { get; set; }

        public int EmployeeCodeID { get; set; }
        #endregion

        public int AssigningID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("AssigningStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? AssginingStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("AssigningEndDateText")]
        [CustomCompareTwoDates("AssginingStartDate", true, ErrorMessage = "CompareBetweenDatesText")]
        public DateTime? AssginingEndDate { get; set; }

        [CustomDisplay("AssigningTypeText")]
        public AssigningsTypesBLL AssigningType { get; set; }

        [CustomDisplay("KSARegionText")]
        public KSARegionsBLL ExternalKSARegion { get; set; }

        [CustomDisplay("ExternalCityText")]
        //[CustomRequired(ErrorMessage = "RequiredExternalCityText")]
        public KSACitiesBLL ExternalKSACity { get; set; }

        public int? ManagerCodeID { get; set; }

        public ParentOrganizationManagerViewModel Manager { get; set; }

        [CustomDisplay("ExternalOrganizationText")]
        public string ExternalOrganization { get; set; }

        #region Job
        public int? JobID { get; set; }

        [CustomDisplay("JobNameText")]
        //[CustomRequired(ErrorMessage = "RequiredJobNameText")]
        public string JobName { get; set; }

        public JobsViewModel Job { get; set; }
        #endregion

        #region Organization
        public int? OrganizationID { get; set; }

        [CustomDisplay("OrganizationNameText")]
        public string OrganizationName { get; set; }

        public OrganizationStructureViewModel OrganizationStructure { get; set; }
        #endregion

        [CustomDisplay("AssigningIsFinishedText")]
        public bool IsFinished { get; set; }

        [CustomDisplay("AssigningReasonText")]
        public AssigningsReasonsBLL AssigningReason { get; set; }

        [CustomDisplay("AssigningReasonOtherText")]
        public string AssigningReasonOther { get; set; }

        [CustomDisplay("AssigningEndReasonText")]
        public int EndAssigningReasonID { get; set; }

        [CustomDisplay("NotesText")]
        public string Notes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.AssigningReason.AssigningReasonID == (int)AssigningsReasonsEnum.Other)
            {
                if (string.IsNullOrEmpty(AssigningReasonOther))
                    yield return new ValidationResult(Resources.Globalization.RequiredAssigningReasonOtherText);
            }

            if (this.AssigningType.AssigningTypeID == Convert.ToInt32(AssigningsTypesEnum.External))
            {
                if (ExternalKSACity == null)      //if (string.IsNullOrEmpty(ExternalCity))
                    yield return new ValidationResult(Resources.Globalization.RequiredExternalCityText);
                if (string.IsNullOrEmpty(ExternalOrganization))
                    yield return new ValidationResult(Resources.Globalization.RequiredExternalOrganizationText);
            }
            else
            {
                if (!ManagerCodeID.HasValue)
                    yield return new ValidationResult(Resources.Globalization.RequiredManagerText);
                if (string.IsNullOrEmpty(OrganizationName))
                    yield return new ValidationResult(Resources.Globalization.RequiredOrganizationNameText);
                if (string.IsNullOrEmpty(JobName))
                    yield return new ValidationResult(Resources.Globalization.RequiredJobNameText);
            }
        }
    }
}