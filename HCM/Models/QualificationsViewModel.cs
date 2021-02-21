using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class QualificationsViewModel
    {
        public int QualificationID { get; set; }

        [CustomDisplay("QualificationNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string QualificationName { get; set; }

        [CustomDisplay("QualificationDegreeNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int QualificationDegreeID { get; set; }

        public string QualificationDegreeName { get; set; }

        [CustomDisplay("DirectPointsText")]

        public decimal? DirectPoints { get; set; }

        [CustomDisplay("IndirectPointsText")]

        public decimal? IndirectPoints { get; set; }
    }
}