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
    
    public partial class DelegationsKinds
    {
        public DelegationsKinds()
        {
            this.Delegations = new HashSet<Delegations>();
        }
    
        public int DelegationKindID { get; set; }
        public string DelegationKindName { get; set; }
        public Nullable<bool> IsIncludeInventory { get; set; }
    
        public virtual ICollection<Delegations> Delegations { get; set; }
    }
}