using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class ExactSpecializationsViewModel
    {
        public int ExactSpecializationID { get; set; }

        [CustomDisplay("ExactSpecializationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string ExactSpecializationName { get; set; }

        [CustomDisplay("QualificationDegreeNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int QualificationDegreeID { get; set; }

        public string QualificationDegreeName { get; set; }

        [CustomDisplay("QualificationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int QualificationID { get; set; }

        public string QualificationName { get; set; }

        [CustomDisplay("GeneralSpecializationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int GeneralSpecializationID { get; set; }

        public string GeneralSpecializationName { get; set; }
    }
}