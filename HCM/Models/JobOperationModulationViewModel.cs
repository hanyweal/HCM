using HCM.Classes.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class JobOperationModulationViewModel
    {
        public int OrganizationJobID { get; set; }
        [CustomDisplay("OrganizationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int OrganizationID { get; set; }
        
        [CustomDisplay("JobNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int JobID { get; set; }
        
        [CustomDisplay("JobOperationDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime JobOperationDate { get; set; }
    }
}