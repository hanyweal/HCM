using HCM.Classes.CustomAttributes;

namespace HCM.Models
{
    public class BusinessRulesViewModel
    {
        public int BusinessRuleID { get; set; }

        [CustomDisplay("BusinessRuleARText")]
        public string BusinessRuleAR { get; set; }
    }
}