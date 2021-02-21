using HCMBLL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class RanksViewModel
    {
        public int RankID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string RankName { get; set; }

        public List<RanksCategoriesBLL> RanksCategories { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public RanksCategoriesBLL RankCategory { get; set; }

    }
}