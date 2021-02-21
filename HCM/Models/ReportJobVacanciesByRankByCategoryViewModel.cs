using HCM.Classes.CustomAttributes;

namespace HCM.Models
{
    public class ReportJobVacanciesByRankByCategoryViewModel
    {
        [CustomDisplay("RankNameText")]
        public int? RankID { get; set; }
        public int? JobCategoryID { get; set; }
    }
}