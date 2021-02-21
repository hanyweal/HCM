using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class EmployeesViewModel : BaseViewModel
    {
        public int EmployeeID { get; set; }

        public int? EmployeeCareerHistoryID { get; set; }

        public int? EmployeeCodeID { get; set; }

        [CustomDisplay("EmployeeCodeNoText")]
        [StringLength(8, MinimumLength = 8, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredStringLengthText")]
        public string EmployeeCodeNo { get; set; }

        [CustomDisplay("EmployeeNameArText")]
        public string EmployeeNameAr { get; set; }

        [CustomDisplay("EmployeeNameEnText")]
        public string EmployeeNameEn { get; set; }

        [CustomDisplay("EmployeeIDNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredStringLengthText")]
        public string EmployeeIDNo { get; set; }

        [CustomDisplay("JobNameText")]
        public string EmployeeJobName { get; set; }

        [CustomDisplay("RankCategoryNameText")]
        public string EmployeeRankCategoryName { get; set; }

        [CustomDisplay("RankNameText")]
        public string EmployeeRankName { get; set; }

        [CustomDisplay("JobNoText")]
        public string EmployeeJobNo { get; set; }

        [CustomDisplay("OrganizationNameText")]
        public string EmployeeOrganizationName { get; set; }

        [CustomDisplay("FirstNameArText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string FirstNameAr { get; set; }

        [CustomDisplay("MiddleNameArText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string MiddleNameAr { get; set; }

        [CustomDisplay("GrandFatherNameArText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string GrandFatherNameAr { get; set; }

        [CustomDisplay("FifthNameArText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string FifthNameAr { get; set; }

        [CustomDisplay("LastNameArText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string LastNameAr { get; set; }

        [CustomDisplay("FirstNameEnText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string FirstNameEn { get; set; }

        [CustomDisplay("MiddleNameEnText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string MiddleNameEn { get; set; }

        [CustomDisplay("GrandFatherNameEnText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string GrandFatherNameEn { get; set; }

        [CustomDisplay("FifthNameEnText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string FifthNameEn { get; set; }

        [CustomDisplay("LastNameEnText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string LastNameEn { get; set; }

        //[CustomRequired(ErrorMessage = "RequiredEmployeeBirthDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        [CustomDisplay("EmployeeBirthDateText")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? EmployeeBirthDate { get; set; }

        [CustomDisplay("EmployeeBirthPlaceText")]
        public string EmployeeBirthPlace { get; set; }

        [CustomDisplay("EmployeeMobileNoText")]
        //[CustomRequired(ErrorMessage = "RequiredEmployeeMobileNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredStringLengthText")]
        public string EmployeeMobileNo { get; set; }

        [CustomDisplay("EmployeePassportNoText")]
        public string EmployeePassportNo { get; set; }

        [CustomDisplay("EMailText")]
        //[DataType(DataType.EmailAddress,ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationEmailText")]
        //[EmailAddress()]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationEmailText")]
        public string EmployeeEMail { get; set; }

        [CustomDisplay("EmployeeIDIssueDateText")]
        public DateTime? EmployeeIDIssueDate { get; set; }

        [CustomDisplay("EmployeePassportSourceText")]
        public string EmployeePassportSource { get; set; }

        [CustomDisplay("EmployeePassportIssueDateText")]
        public DateTime? EmployeePassportIssueDate { get; set; }

        [CustomCompareTwoDates("EmployeePassportIssueDate", true, ErrorMessage = "CompareBetweenDatesText")]
        [CustomDisplay("EmployeePassportEndDateText")]
        public DateTime? EmployeePassportEndDate { get; set; }

        [CustomDisplay("EmployeeCurrentQualificationText")]
        public string EmployeeCurrentQualification { get; set; }

        [CustomDisplay("EmployeeIDExpiryDateText")]
        public DateTime? EmployeeIDExpiryDate { get; set; }

        [CustomDisplay("EmployeeIDCopyNoText")]
        public int? EmployeeIDCopyNo { get; set; }

        [CustomDisplay("EmployeeIDIssuePlaceText")]
        public string EmployeeIDIssuePlace { get; set; }

        [CustomDisplay("DependentCountText")]
        public int? DependentCount { get; set; }

        [CustomDisplay("MaritalStatusText")]
        public MaritalStatusBLL MaritalStatus { get; set; }

        [CustomDisplay("GenderText")]
        public int GenderID { get; set; }

        [CustomDisplay("CurrentJobJoinDateText")]
        public virtual DateTime? CurrentJobJoinDate { get; set; }

        [CustomDisplay("NationalityText")]
        //public CountriesBLL Nationality { get; set; }
        public int CountryID { get; set; }

        [CustomDisplay("HiringDateText")]
        public DateTime? HiringDate { get; set; }

        [CustomDisplay("EmployeeCurrentOrganizationText")]
        public string EmployeeCurrentOrganization { get; set; }

        private bool _IsShowAllFields = true;

        public bool IsShowAllFields
        {
            get
            {
                return _IsShowAllFields;
            }
            set
            {
                _IsShowAllFields = value;
            }
        }

        public string PartialName { get; set; }

    }
}