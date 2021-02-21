using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HCM.Models
{
    public class HiringNewEmployeesViewModel : BaseViewModel
    {
        #region EmployeesViewModel
        public int EmployeeID { get; set; }

        [CustomDisplay("EmployeeIDNoText")]
        //[CustomRequired(ErrorMessage = "RequiredEmployeeIDNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredStringLengthText")]
        public string EmployeeIDNo { get; set; }

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
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
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
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
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
        // [DataType(DataType.EmailAddress,ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationEmailText")]
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
        public GendersBLL Gender { get; set; }

        [CustomDisplay("NationalityText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int CountryID { get; set; }

        //[CustomDisplay("NationalityText")]
        //public int CountryID { get; set; }
        //[CustomDisplay("HiringDateText")]
        //public DateTime? HiringDate { get; set; }

        //[CustomDisplay("EmployeeCurrentOrganizationText")]
        //public string EmployeeCurrentOrganization { get; set; }

        //private bool _IsShowAllFields = true;

        //public bool IsShowAllFields
        //{
        //    get
        //    {
        //        return _IsShowAllFields;
        //    }
        //    set
        //    {
        //        _IsShowAllFields = value;
        //    }
        //}
        #endregion

        #region CreateEmployeeCodeNo
        [CustomDisplay("EmployeeCodeNoText")]
        [StringLength(8, MinimumLength = 8, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredStringLengthText")]
        public string EmployeeCodeNo { get; set; }
        //public EmployeesTypesBLL EmployeeType { get; set; }
        #endregion

        #region EmployeesQualificationsViewModel
        public int EmployeeQualificationID { get; set; }

        [CustomDisplay("QualificationDegreeNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int QualificationDegreeID { get; set; }

        [CustomDisplay("QualificationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int QualificationID { get; set; }

        [CustomDisplay("GeneralSpecializationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int GeneralSpecializationID { get; set; }

        [CustomDisplay("ExactSpecializationNameText")]
        public int? ExactSpecializationID { get; set; }

        //public EmployeesViewModel EmployeeVM { get; set; }

        [CustomDisplay("UniSchNameText")]
        public string UniSchName { get; set; }

        [CustomDisplay("DepartmentText")]
        public string Department { get; set; }

        [CustomDisplay("FullGPAText")]
        [Range(1, 100, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        public decimal? FullGPA { get; set; }

        [CustomDisplay("GPAText")]
        [Range(1, 100, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        public decimal? GPA { get; set; }

        [CustomDisplay("StudyPlaceText")]
        public string StudyPlace { get; set; }

        [CustomDisplay("GraduationDateText")]
        public DateTime? GraduationDate { get; set; }

        [CustomDisplay("GraduationYearText")]
        public string GraduationYear { get; set; }

        [CustomDisplay("PercentageText")]
        public decimal Percentage { get; set; }

        [CustomDisplay("QualificationTypeText")]
        public int QualificationTypeID { get; set; }

        public bool HasCreatingAccess { get; set; }
        public bool HasUpdatingAccess { get; set; }
        public bool HasDeletingAccess { get; set; }


        #endregion

        #region ContractorsBasicSalariesViewModel
        [CustomDisplay("BasicSalaryText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public double BasicSalary { get; set; }

        [CustomDisplay("TransfareAllowanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public double TransfareAllowance { get; set; }

        public List<AllowancesBLL> Allowances { get; set; }
        #endregion

        #region EmployeeCareerHistory
        public int EmployeeCareerHistoryID { get; set; }

        public int OrganizationJobID { get; set; }

        public int CareerDegreeID { get; set; }

        public int CareerHistoryTypeID { get; set; }

        [CustomDisplay("HiringDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime JoinDate { get; set; }
        #endregion
    }
}