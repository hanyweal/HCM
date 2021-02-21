using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class JobsGroupsViewModel : BaseViewModel
    {

        public int? JobGroupID { get; set; }

        [CustomDisplay("JobGroupNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string JobGroupName { get; set; }

        [CustomDisplay("JobGeneralGroupNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int JobGeneralGroupID { get; set; }

        public string JobGeneralGroupName { get; set; }
    }
}