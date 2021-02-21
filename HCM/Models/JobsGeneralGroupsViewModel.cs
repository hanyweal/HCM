using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class JobsGeneralGroupsViewModel : BaseViewModel
    {
        public int? JobGeneralGroupID { get; set; }

        [CustomDisplay("JobGeneralGroupNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string JobGeneralGroupName { get; set; }
    }
}