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
    
    public partial class ExactSpecializations
    {
        public ExactSpecializations()
        {
            this.EmployeesQualifications = new HashSet<EmployeesQualifications>();
        }
    
        public int ExactSpecializationID { get; set; }
        public Nullable<int> GeneralSpecializationID { get; set; }
        public string ExactSpecializationName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
    
        public virtual EmployeesCodes EmployeesCodes { get; set; }
        public virtual ICollection<EmployeesQualifications> EmployeesQualifications { get; set; }
        public virtual GeneralSpecializations GeneralSpecializations { get; set; }
    }
}