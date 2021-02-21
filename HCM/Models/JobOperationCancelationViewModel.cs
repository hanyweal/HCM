using HCM.Classes.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class JobOperationCancelationViewModel
    {
        public int OrganizationJobID { get; set; }
              
        [CustomDisplay("JobOperationDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime JobOperationDate { get; set; }
    }
}