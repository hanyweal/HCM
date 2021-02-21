using HCM.Classes.CustomAttributes;
using HCMBLL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class KSACitiesViewModel //: IValidatableObject
    {
        public int KSACityID { get; set; }

        [CustomDisplay("KSACityNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string KSACityName { get; set; }

        [CustomDisplay("KSARegionText")]
        public List<KSARegionsBLL> KSARegions { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public KSARegionsBLL KSARegion { get; set; }
    }
}