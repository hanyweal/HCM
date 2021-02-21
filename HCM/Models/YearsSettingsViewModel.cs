using HCM.Classes.CustomAttributes;
using HCMBLL;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class YearsSettingsViewModel : BaseViewModel
    {
        [CustomDisplay("MaturityYearBalanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public MaturityYearsBalancesBLL MaturityYearBalance { get; set; }
    }
}