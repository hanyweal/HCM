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
    
    public partial class Scholarships
    {
        public Scholarships()
        {
            this.ScholarshipsDetails = new HashSet<ScholarshipsDetails>();
        }
    
        public int ScholarshipID { get; set; }
        public int EmployeeCodeID { get; set; }
        public int ScholarshipTypeID { get; set; }
        public string Location { get; set; }
        public System.DateTime ScholarshipStartDate { get; set; }
        public System.DateTime ScholarshipEndDate { get; set; }
        public Nullable<int> QualificationID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public string ScholarshipReason { get; set; }
        public Nullable<int> KSACityID { get; set; }
        public Nullable<int> CountryID { get; set; }
        public bool IsCanceled { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ScholarshipJoinDate { get; set; }
        public Nullable<bool> IsPassed { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
    
        public virtual EmployeesCodes EmployeesCodes { get; set; }
        public virtual ScholarshipsTypes ScholarshipsTypes { get; set; }
        public virtual Countries Countries { get; set; }
        public virtual KSACities KSACities { get; set; }
        public virtual ICollection<ScholarshipsDetails> ScholarshipsDetails { get; set; }
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual Qualifications Qualifications { get; set; }
        public virtual EmployeesCodes LastUpdatedByNav { get; set; }
    }
}