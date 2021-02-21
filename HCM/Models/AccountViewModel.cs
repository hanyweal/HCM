using HCM.Classes.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class AccountViewModel
    {
        public int UserID { get; set; }

        public string EmployeeName { get; set; }

        public string OrganizationName { get; set; }

        public string RankName { get; set; }

        public string JobName { get; set; }

        public bool IsAdmin { get; set; }

        [CustomDisplay("UserNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        [StringLength(8, MinimumLength = 8, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationStringLengthText")]
        public string UserName { get; set; }

        [CustomDisplay("OldPasswordText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string OldPassword { get; set; }

        [CustomDisplay("PasswordText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string Password { get; set; }

        [CustomDisplay("ConfirmPasswordText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ConfirmNotMatchedText")]
        //[CustomCompare("Password", ErrorMessage = "ConfirmNotMatchedText")]
        public string ConfirmPassword { get; set; }

        public bool RememberMe { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public string EMail { get; set; }

        public string UserPic { get; set; }
    }
}