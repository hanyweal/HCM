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
    
    public partial class SickVacationsTypes
    {
        public SickVacationsTypes()
        {
            this.Vacations = new HashSet<Vacations>();
        }
    
        public int SickVacationTypeID { get; set; }
        public string SickVacationTypeName { get; set; }
    
        public virtual ICollection<Vacations> Vacations { get; set; }
    }
}
