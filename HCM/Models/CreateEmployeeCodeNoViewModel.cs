using HCM.Classes.CustomAttributes;
using HCMBLL;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class CreateEmployeeCodeNoViewModel : BaseViewModel
    {
        [CustomDisplay("EmployeeIDNoText")]
        //[CustomRequired(ErrorMessage = "RequiredEmployeeIDNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeID { get; set; }
        [CustomDisplay("EmployeeCodeNoText")]
        [StringLength(8, MinimumLength = 8, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredStringLengthText")]
        public string EmployeeCodeNo { get; set; }
        public EmployeesTypesBLL EmployeeType { get; set; }
    }
}