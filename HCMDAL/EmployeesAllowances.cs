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
    
    public partial class EmployeesAllowances
    {
        public int EmployeeAllowanceID { get; set; }
        public int EmployeeCareerHistoryID { get; set; }
        public int AllowanceID { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public System.DateTime AllowanceStartDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
    
        public virtual EmployeesCareersHistory EmployeesCareersHistory { get; set; }
        public virtual Allowances Allowances { get; set; }
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual EmployeesCodes LastUpdatedByNav { get; set; }
    }
}
