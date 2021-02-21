using HCM.Classes.CustomAttributes;

namespace HCM.Models
{
    public class ParentOrganizationManagerViewModel
    {
        public int? ManagerCodeID { get; set; }

        [CustomDisplay("EmployeeCodeNoText")]
        public string ManagerCodeNo { get; set; }

        [CustomDisplay("EmployeeNameArText")]
        public string ManagerNameAr { get; set; }
    }
}