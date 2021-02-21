using HCM.Classes.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class JobOperationPushingUpViewModel
    {
        public int OrganizationJobID { get; set; }
        [CustomDisplay("RankNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int RankID { get; set; }
        
        [CustomDisplay("JobNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string JobNo { get; set; }
        
        [CustomDisplay("JobOperationDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime JobOperationDate { get; set; }
    }
}