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
    
    public partial class NormalVacationsTypes
    {
        public NormalVacationsTypes()
        {
            this.Vacations = new HashSet<Vacations>();
        }
    
        public int NormalVacationTypeID { get; set; }
        public string NormalVacationTypeName { get; set; }
    
        public virtual ICollection<Vacations> Vacations { get; set; }
    }
}