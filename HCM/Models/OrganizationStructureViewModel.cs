using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class OrganizationStructureViewModel
    {
        [CustomDisplay("OrganizationParentText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int ParentOrganizationID { get; set; }

        public int? OrganizationID { get; set; }

        [CustomDisplay("OrganizationCodeNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string OrganizationCode { get; set; }

        [CustomDisplay("OrganizationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string OrganizationName { get; set; }

        [CustomDisplay("OrganizationNameText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string FullOrganizationName { get; set; }

        [CustomDisplay("BranchNameText")]
        public int BranchName { get; set; }

        [CustomDisplay("BranchNameText")]
        public int BranchID { get; set; }

        public int? ManagerCodeID { get; set; }

        public EmployeesViewModel OrganizationManager { get; set; }

    }
}