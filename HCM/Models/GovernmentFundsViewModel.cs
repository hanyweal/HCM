using HCM.Classes.CustomAttributes;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class GovernmentFundsViewModel : BaseViewModel, IValidatableObject
    {
        public int GovernmentFundID { get; set; }

        [CustomDisplay("LoanNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string LoanNo { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("LoanDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime LoanDate { get; set; }

        //[DataType(DataType.Date)]
        //[CustomDisplay("ContractDateText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        //public DateTime ContractDate { get; set; }

        [CustomDisplay("KSACityText")]
        public List<KSACitiesBLL> KSACities { get; set; }

        [CustomDisplay("KSACityText")]
        public KSACitiesBLL KSACity { get; set; }

        [CustomDisplay("ContractNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string ContractNo { get; set; }

        [CustomDisplay("SponserToNameText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string SponserToName { get; set; }

        [CustomDisplay("SponserToIDNoText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string SponserToIDNo { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("DeductionStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public System.DateTime DeductionStartDate { get; set; }

        [CustomDisplay("MonthlyDeductionAmountText")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public double MonthlyDeductionAmount { get; set; }

        [CustomDisplay("TotalDeductionAmountText")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public double TotalDeductionAmount { get; set; }

        [CustomDisplay("RemainingDeductionAmountText")]
        [DisplayFormat(DataFormatString ="{0:N}", ApplyFormatInEditMode =false)]
        public double RemainingDeductionAmount { get; set; }

        public virtual EmployeesCodesBLL EmployeeCode { get; set; }

        [CustomDisplay("GovernmentFundsTypesText")]
        public virtual GovernmentFundsTypesBLL GovernmentFundType { get; set; }

        [CustomDisplay("GovernmentDeductionsTypesText")]
        public virtual GovernmentDeductionsTypesBLL GovernmentDeductionType { get; set; }

        [CustomDisplay("EmployeesText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCodeID { get; set; }

        public EmployeesViewModel EmployeeVM { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (GovernmentDeductionType.GovernmentDeductionTypeID == Convert.ToInt32(GovernmentDeductionsTypesEnum.Sponser))
            {
                if (string.IsNullOrEmpty(SponserToName))
                    yield return new ValidationResult(Resources.Globalization.RequiredSponserToNameText);

                if (string.IsNullOrEmpty(SponserToIDNo))
                    yield return new ValidationResult(Resources.Globalization.RequiredSponserToIDNoText);
            }
        }

        public bool? IsActive { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("DeactiveDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? DeactiveDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("LetterDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? LetterDate { get; set; }


        [CustomDisplay("LetterNoText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string LetterNo { get; set; }

        [CustomDisplay("NotesText")]
        public string Notes { get; set; }

        [CustomDisplay("GovernmentFundDeactiveReasonText")]
        public virtual GovernmentFundsDeactiveReasonsBLL GovernmentFundDeactiveReason { get; set; }

        [CustomDisplay("BankIPANText")]
        public virtual string BankIPAN { get; set; }
    }
}