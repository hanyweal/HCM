//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HCMDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class PromotionsDecisions
    {
        public PromotionsDecisions()
        {
            this.PromotionsRecordsEmployees = new HashSet<PromotionsRecordsEmployees>();
        }
    
        public int PromotionDecisionID { get; set; }
        public string PromotionDecisionName { get; set; }
    
        public virtual ICollection<PromotionsRecordsEmployees> PromotionsRecordsEmployees { get; set; }
    }
}