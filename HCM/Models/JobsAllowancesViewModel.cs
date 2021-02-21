using HCM.Classes.CustomAttributes;
using HCMBLL;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class JobsAllowancesViewModel
    {
        public int JobAllowanceID { get; set; }

        [CustomDisplay("AllowanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int AllowanceID { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public AllowancesBLL Allowance { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int JobID { get; set; }

        [CustomDisplay("JobNameText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public JobsBLL Job { get; set; }

        [CustomDisplay("IsActiveText")]
        public bool IsActive { get; set; }
    }
}