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
    
    public partial class Periods
    {
        public Periods()
        {
            this.PromotionsPeriods = new HashSet<PromotionsPeriods>();
        }
    
        public int PeriodID { get; set; }
        public string PeriodName { get; set; }
    
        public virtual ICollection<PromotionsPeriods> PromotionsPeriods { get; set; }
    }
}
