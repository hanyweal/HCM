using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class CountriesViewModel //: IValidatableObject
    {
        public int CountryID { get; set; }

        [CustomDisplay("CountryNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string CountryName { get; set; }

    }
}