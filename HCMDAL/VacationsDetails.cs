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
    
    public partial class VacationsDetails
    {
        public int VacationDetailID { get; set; }
        public int VacationActionID { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public bool IsApproved { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int VacationID { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string Notes { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public string MainframeNo { get; set; }
    
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual Vacations Vacations { get; set; }
        public virtual VacationsActions VacationsActions { get; set; }
        public virtual EmployeesCodes ApprovedByNav { get; set; }
        public virtual EmployeesCodes LastUpdatedByNav { get; set; }
    }
}
