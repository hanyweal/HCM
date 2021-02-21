using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class TransfersViewModel
    {
        public int TransferID { get; set; }

        public int EmployeeCareerHistoryID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("TransferDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime TransferDate { get; set; }

        [CustomDisplay("DestinationText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string Destination { get; set; }

        [CustomDisplay("KSACityText")]
        public KSACitiesBLL KSACity { get; set; }

        [CustomDisplay("TransferTypeText")]
        public TransfersTypesBLL TransfersTypes { get; set; }

        public EmployeesCareersHistoryBLL EmployeesCareersHistory { get; set; }

        [CustomDisplay("JobNameText")]
        public string JobName { get; set; }

        [CustomDisplay("RankNameText")]
        public string RankName { get; set; }

        [CustomDisplay("JobNoText")]
        public string JobCode { get; set; }

        [CustomDisplay("OrganizationNameText")]
        public string OrganizationName { get; set; }

        [CustomDisplay("CareerDegreeNameText")]
        public string CareerDegreeName { get; set; }

        #region Employee
        [CustomDisplay("EmployeeCodeIDText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCodeID { get; set; }

        public EmployeesViewModel Employee { get; set; }

        #endregion
    }
}