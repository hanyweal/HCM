using HCM.Classes.CustomAttributes;
using HCMBLL;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class JobsViewModel
    {
        public int? JobID { get; set; }

        [CustomDisplay("JobNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string JobName { get; set; }

        [CustomDisplay("JobCategoryNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int? JobCategoryID { get; set; }

        public string JobCategoryName { get; set; }


        [CustomDisplay("JobCodeText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string JobCode { get; set; }

        [CustomDisplay("JobGroupNameText")]
        public JobsGroupsBLL JobGroup { get; set; }

        [CustomDisplay("JobGeneralGroupNameText")]
        public JobsGeneralGroupsBLL JobGeneralGroup { get; set; }

        public string PartialName { get; set; }

    }
}