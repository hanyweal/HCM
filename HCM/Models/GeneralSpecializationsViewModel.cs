using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class GeneralSpecializationsViewModel
    {
        public int GeneralSpecializationID { get; set; }

        [CustomDisplay("GeneralSpecializationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string GeneralSpecializationName { get; set; }

        [CustomDisplay("QualificationDegreeNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int QualificationDegreeID { get; set; }

        [CustomDisplay("QualificationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int QualificationID { get; set; }

        [CustomDisplay("DirectPointsText")]

        public decimal? DirectPoints { get; set; }

        [CustomDisplay("IndirectPointsText")]

        public decimal? IndirectPoints { get; set; }

        public string QualificationName { get; set; }
        public string QualificationDegreeName { get; set; }

    }
}