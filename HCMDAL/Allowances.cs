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
    
    public partial class Allowances
    {
        public Allowances()
        {
            this.EmployeesAllowances = new HashSet<EmployeesAllowances>();
            this.JobsAllowances = new HashSet<JobsAllowances>();
        }
    
        public int AllowanceID { get; set; }
        public string AllowanceName { get; set; }
        public int AllowanceAmountTypeID { get; set; }
        public double AllowanceAmount { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public int AllowanceCalculationTypeID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
    
        public virtual AllowancesAmountTypes AllowancesAmountTypes { get; set; }
        public virtual ICollection<EmployeesAllowances> EmployeesAllowances { get; set; }
        public virtual ICollection<JobsAllowances> JobsAllowances { get; set; }
        public virtual AllowancesCalculationTypes AllowancesCalculationTypes { get; set; }
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual EmployeesCodes LastUpdatedByNav { get; set; }
    }
}
