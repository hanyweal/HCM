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
    
    public partial class EmployeesTypes
    {
        public EmployeesTypes()
        {
            this.EmployeesCodes = new HashSet<EmployeesCodes>();
        }
    
        public int EmployeeTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
    
        public virtual ICollection<EmployeesCodes> EmployeesCodes { get; set; }
    }
}
