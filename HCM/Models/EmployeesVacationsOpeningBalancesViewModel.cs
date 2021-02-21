using HCM.Classes.CustomAttributes;
using HCMBLL;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class EmployeesVacationsOpeningBalancesViewModel
    {

        [CustomDisplay("VacationTypeText")]
        public VacationsTypesBLL VacationType { get; set; }

        [CustomDisplay("OpeningBalanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int? OpeningBalance { get; set; }

    }
}