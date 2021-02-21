using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class JobsOperationsTypesViewModel 
    {
        [CustomDisplay("JobOperationTypeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public virtual int JobOperationTypeID { get; set; }

        [CustomDisplay("JobOperationTypeNameText")]
        public virtual string JobOperationTypeName { get; set; }

    }
}