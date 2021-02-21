using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class EmployeesDegreesUpgradesViewModel : BaseViewModel
    {
        public int EmployeeDegreeUpgradeID { get; set; }

        [CustomDisplay("EmployeeDegreeUpgradeYearText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeDegreeUpgradeYear { get; set; }
    }
}