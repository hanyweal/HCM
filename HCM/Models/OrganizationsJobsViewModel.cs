using HCM.Classes.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class OrganizationsJobsViewModel
    {
        public int EmployeeCareerHistoryID { get; set; }

        public int OrganizationJobID { get; set; }

        [CustomDisplay("JobNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string JobNo { get; set; }

        [CustomDisplay("IsVacantText")]        
        public bool IsVacant { get; set; }

        public JobsOperationsTypesViewModel JobOperationType { get; set; }

        [CustomDisplay("JobOperationDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? JobOperationDate {
            get;
            set; 
        }

        #region Job
        [CustomDisplay("JobNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int JobID { get; set; }

        [CustomDisplay("JobNameText")]
        //[CustomRequired(ErrorMessage = "RequiredJobNameText")]
        public string JobName { get; set; }

        public JobsViewModel Job { get; set; }
        #endregion

        #region Organization
        [CustomDisplay("OrganizationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int OrganizationID { get; set; }

        [CustomDisplay("OrganizationNameText")]
        //[CustomRequired(ErrorMessage = "RequiredOrganizationNameText")]
        public string OrganizationName { get; set; }

        public OrganizationStructureViewModel OrganizationStructure { get; set; }
        #endregion

        #region Rank
        [CustomDisplay("RankNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int RankID { get; set; }

        [CustomDisplay("RankNameText")]
        //[CustomRequired(ErrorMessage = "RequiredJobNameText")]
        public string RankName { get; set; }

        public RanksViewModel Rank { get; set; }
        #endregion

        #region Rank
        [CustomDisplay("OrganizationJobStatusText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int OrganizationJobStatusID { get; set; }
        #endregion
    }
}