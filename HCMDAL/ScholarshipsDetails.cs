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
    
    public partial class ScholarshipsDetails
    {
        public int ScholarshipDetailID { get; set; }
        public int ScholarshipActionID { get; set; }
        public int ScholarshipID { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public string Notes { get; set; }
    
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual EmployeesCodes LastUpdatedByNav { get; set; }
        public virtual Scholarships Scholarships { get; set; }
        public virtual ScholarshipsActions ScholarshipsActions { get; set; }
    }
}
