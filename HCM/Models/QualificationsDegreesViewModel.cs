using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;


namespace HCM.Models
{
    public class QualificationsDegreesViewModel
    {
        public int QualificationDegreeID { get; set; }

        [CustomDisplay("QualificationDegreeNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string QualificationDegreeName { get; set; }

    }
}