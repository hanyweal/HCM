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
    
    public partial class AllowancesCalculationTypes
    {
        public AllowancesCalculationTypes()
        {
            this.Allowances = new HashSet<Allowances>();
        }
    
        public int AllowanceCalculationTypeID { get; set; }
        public string AllowanceCalculationTypeName { get; set; }
    
        public virtual ICollection<Allowances> Allowances { get; set; }
    }
}
