using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class EmployeesUpdateViewModel
    {
        #region Employee
        [CustomDisplay("EmployeeCodeIDText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCodeID { get; set; }

        [CustomDisplay("EmployeeIDNoText")]
        public string EmployeeIDNo { get; set; }
        #endregion

        [CustomDisplay("EmployeeMobileNoText")]
        public string MobileNo { get; set; }

        [CustomDisplay("EmployeeNewMobileNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        [CustomCompareNotMatched("MobileNo", ErrorMessage = "CompareNotMatchedText")]
        public string NewMobileNo { get; set; }

        [CustomDisplay("MobileOTPText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string MobileOTP { get; set; }

        [CustomDisplay("NewFirstNameArText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string NewFirstNameAr { get; set; }

        [CustomDisplay("NewMiddleNameArText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string NewMiddleNameAr { get; set; }

        [CustomDisplay("NewGrandFatherNameArText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string NewGrandFatherNameAr { get; set; }

        [CustomDisplay("NewFifthNameArText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string NewFifthNameAr { get; set; }

        [CustomDisplay("NewLastNameArText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string NewLastNameAr { get; set; }

        [CustomDisplay("NewFirstNameEnText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string NewFirstNameEn { get; set; }

        [CustomDisplay("NewMiddleNameEnText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string NewMiddleNameEn { get; set; }

        [CustomDisplay("NewGrandFatherNameEnText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string NewGrandFatherNameEn { get; set; }

        [CustomDisplay("NewFifthNameEnText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string NewFifthNameEn { get; set; }

        [CustomDisplay("NewLastNameEnText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string NewLastNameEn { get; set; }
    }
}