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
    
    public partial class InternshipScholarshipsDetails
    {
        public int InternshipScholarshipDetailID { get; set; }
        public int InternshipScholarshipID { get; set; }
        public int EmployeeCareerHistoryID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public bool IsPassengerOrderCompensation { get; set; }
    
        public virtual EmployeesCareersHistory EmployeesCareersHistory { get; set; }
        public virtual InternshipScholarships InternshipScholarships { get; set; }
    }
}